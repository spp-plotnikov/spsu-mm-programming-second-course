import java.awt.*;
import javax.imageio.ImageIO;
import javax.swing.*;

import java.awt.Container;
import java.awt.image.BufferedImage;
import java.io.*;
import java.net.Socket;
import java.nio.ByteBuffer;
import javax.swing.JButton;
import javax.swing.JFrame;
import javax.swing.border.Border;

public class MainWindow {
    private JFrame frame;
    private BufferedImage srcImg = null, resImg = null;
    private GridBagConstraints c;
    private String path;
    private String[] filters;

    private void submitImage() {
        // Making it thread-safe
        if (!SwingUtilities.isEventDispatchThread())
            SwingUtilities.invokeLater(() -> submitImage());

        if (srcImg == null) {
            JOptionPane.showMessageDialog(frame, "Please load an image first");
            return;
        }

        try {
            // send it
            Socket s = new Socket("127.0.0.1", 1424);
            OutputStream output = s.getOutputStream();
            JComboBox comboBox = (JComboBox) frame.getContentPane().getComponent(6);
            new ImageSender(srcImg, output).send(comboBox.getSelectedIndex());

            // and receive the result
            InputStream input = s.getInputStream();
            byte[] magicWord = new byte[1];
            input.read(magicWord);
            assert magicWord[0] == (byte) 0xFF;
            resImg = new ImageReceiver(input).recv();
        } catch (IOException e) {
            JOptionPane.showMessageDialog(frame, "Communication problem");
        }
        setResIcon(resImg);
    }

    public void addComponentsToPane(Container pane) {
        pane.setLayout(new GridBagLayout());
        c = new GridBagConstraints();
        c.fill = GridBagConstraints.HORIZONTAL;

        // Path edit
        JTextField pathField = new JTextField();
        c.weightx = 0.5;
        c.ipadx = 300;
        c.fill = GridBagConstraints.HORIZONTAL;
        c.gridx = 0;
        c.gridy = 0;
        pane.add(pathField, c);

        // Load buttion
        JButton chooseButton = new JButton("Choose...");
        c.ipadx = 20;
        c.fill = GridBagConstraints.HORIZONTAL;
        c.gridx = 1;
        c.gridy = 0;
        chooseButton.addActionListener(actionEvent -> {
            JFileChooser chooser = new JFileChooser();
            int option = chooser.showOpenDialog(pane);
            if (option == JFileChooser.APPROVE_OPTION) {
                File selectedFile = chooser.getSelectedFile();
                path = selectedFile.getAbsolutePath();
                pathField.setText(path);
            }
        });
        pane.add(chooseButton, c);

        // Load button
        JButton loadButton = new JButton("Load");
        loadButton.addActionListener(actionEvent -> loadImage());
        c.fill = GridBagConstraints.HORIZONTAL;
        c.gridx = 2;
        c.gridy = 0;
        pane.add(loadButton, c);

        // First image
        JLabel srcLabel;
        srcLabel = new JLabel("Please load an image");
        Border border = BorderFactory.createLineBorder(Color.LIGHT_GRAY, 1);
        srcLabel.setBorder(border);
        c.fill = GridBagConstraints.HORIZONTAL;
        c.weightx = 0.0;
        c.gridx = 0;
        c.gridy = 1;
        c.ipady = 150;
        c.gridwidth = 6;
        pane.add(srcLabel, c);

        // Second image
        JLabel resLabel;
        resLabel = new JLabel("Result will be displayed here");
        resLabel.setBorder(border);
        c.fill = GridBagConstraints.HORIZONTAL;
        c.gridx = 1;
        c.gridy = 1;
        c.ipadx = 125;
        c.ipady = 150;
        c.gridwidth = 6;
        pane.add(resLabel, c);

        // Progress bar
        JProgressBar progressBar = new JProgressBar();
        c.gridx = 0;
        c.ipadx = 0;
        c.ipady = 0;
        c.gridy = 2;
        pane.add(progressBar, c);

        // Filters
        JComboBox comboBox = new JComboBox();
        c.fill = GridBagConstraints.HORIZONTAL;
        c.ipadx = c.ipady = 0;
        c.gridwidth = 1;
        c.gridx = 0;
        c.gridy = 3;
        pane.add(comboBox, c);

        // Submit button
        JButton submitButton = new JButton("Submit");
        c.fill = GridBagConstraints.HORIZONTAL;
        c.gridx = 1;
        c.gridy = 3;
        submitButton.addActionListener(actionEvent -> this.submitImage());
        pane.add(submitButton, c);

        // Exit button
        JButton exitButton = new JButton("Exit");
        c.fill = GridBagConstraints.HORIZONTAL;
        c.ipady = 0;
        c.weighty = 1.0;
        c.anchor = GridBagConstraints.PAGE_END;
        c.insets = new Insets(10, 0, 0, 0);
        c.gridx = 1;
        c.gridwidth = 2;
        c.gridy = 5;
        exitButton.addActionListener(actionEvent -> frame.dispose());
        pane.add(exitButton, c);
    }

    private void setSrcIcon(BufferedImage src) {
        if (src == null)
            return;
        ImageIcon icon = new ImageIcon(src);
        Image im = icon.getImage();
        im = im.getScaledInstance(200, 200, java.awt.Image.SCALE_SMOOTH);
        icon = new ImageIcon(im);
        JLabel label = (JLabel) frame.getContentPane().getComponent(3);
        label.setText("");
        label.setBorder(null);
        label.setIcon(icon);
        frame.setSize(500, 500);
    }

    private void setResIcon(BufferedImage src) {
        if (src == null)
            return;
        ImageIcon icon = new ImageIcon(src);
        Image im = icon.getImage();
        im = im.getScaledInstance(200, 200, Image.SCALE_SMOOTH);
        icon = new ImageIcon(im);
        JLabel label = (JLabel) frame.getContentPane().getComponent(4);
        label.setIcon(icon);
        label.setText("");
        label.setBorder(null);
    }

    private void loadImage() {
        if (path == null)
            return;

        try {
            srcImg = ImageIO.read(new File(path));
            if (srcImg == null)
                throw new IOException();
            setSrcIcon(srcImg);
        } catch (IOException e) {
            e.printStackTrace();
            JOptionPane.showMessageDialog(frame, "Unable to read image");
        }
    }

    private void createAndShowGUI() {
        frame = new JFrame("Best GUI ever");
        frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        addComponentsToPane(frame.getContentPane());
        frame.pack();
        //frame.setSize(500, 500);
        frame.setVisible(true);
    }

    private void recvFilterList() {
        try {
            Socket s = new Socket("127.0.0.1", 1424);
            OutputStream outputStream = s.getOutputStream();
            DataInputStream inputStream = new DataInputStream(s.getInputStream());

            byte magicWord[] = { (byte) 0xFA };
            outputStream.write(magicWord);

            byte len[] = new byte[4];
            inputStream.read(len);
            ByteBuffer wrapped = ByteBuffer.wrap(len);
            int size = wrapped.getInt();
            System.out.println(size);
            JComboBox comboBox = (JComboBox) frame.getContentPane().getComponent(6);
            filters = new String[size];
            for (int i = 0; i < size; i++) {
                filters[i] = inputStream.readUTF();
                comboBox.addItem(filters[i]);
            }
        } catch (IOException e) {
            JOptionPane.showMessageDialog(frame, "Failed to receive filters");
        }
    }

    public void run() {
        try {
            javax.swing.SwingUtilities.invokeAndWait(() -> createAndShowGUI());
        } catch (Exception e) {
            e.printStackTrace();
        }
        javax.swing.SwingUtilities.invokeLater(() -> recvFilterList());
    }
}