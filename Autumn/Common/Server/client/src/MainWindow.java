import java.awt.*;
import javax.imageio.ImageIO;
import javax.swing.*;

import java.awt.Container;
import java.awt.image.BufferedImage;
import java.io.*;
import java.net.InetAddress;
import java.net.Socket;
import java.nio.ByteBuffer;
import java.nio.file.Files;
import java.util.concurrent.atomic.AtomicBoolean;
import javax.swing.JButton;
import javax.swing.JFrame;
import javax.swing.border.Border;
import javax.swing.event.DocumentEvent;
import javax.swing.event.DocumentListener;

class MainWindow {
    private JFrame frame;
    private BufferedImage srcImg = null, resImg = null;
    private String path;
    private SwingWorker worker;
    private AtomicBoolean isRunning = new AtomicBoolean(false);
    private InetAddress serverIp;
    private int serverPort = 1424;

    private void submitImageButtonHandler() {
        if (!isRunning.get()) {

            if (srcImg == null) {
                JOptionPane.showMessageDialog(frame, "Please load an image first");
                return;
            }

            // This describes the thread, which communicates with the server
            worker = new SwingWorker<Void, Void>() {
                @Override
                public Void doInBackground() {
                    isRunning.set(true);
                    // get components
                    JButton button = (JButton) frame.getContentPane().getComponent(7);
                    JProgressBar progressBar = (JProgressBar) frame.getContentPane().getComponent(5);
                    button.setText("Abort");

                    // The following code needs to be in try-block, almost evert operation can throw IOException
                    try {
                        // send it
                        Socket s = new Socket(serverIp, serverPort);
                        OutputStream output = s.getOutputStream();
                        JComboBox comboBox = (JComboBox) frame.getContentPane().getComponent(6);
                        new ImageSender(srcImg, output).send(comboBox.getSelectedIndex());

                        // and receive the result
                        InputStream input = s.getInputStream();
                        byte[] magicWord = new byte[1];
                        do {
                            input.read(magicWord);
                            if (magicWord[0] == (byte) 0xFF) // image, not progress update
                                break;
                            // retrieve the progress
                            byte[] cur = new byte[4];
                            input.read(cur);
                            // and set the progress bar
                            progressBar.setValue(ByteBuffer.wrap(cur).getInt());
                        } while (!isCancelled());
                        if (isCancelled())
                            return null;

                        // finally, get the result image
                        resImg = new ImageReceiver(input).recv();
                    } catch (IOException e) {
                        JOptionPane.showMessageDialog(frame, "Communication problem");
                    }

                    // and set it
                    setResIcon(resImg);
                    progressBar.setValue(100);
                    return null;
                }

                @Override
                public void done() {
                    JButton button = (JButton) frame.getContentPane().getComponent(7);
                    button.setText("Submit");
                    isRunning.set(false);
                }
            };

            // starts the thread
            worker.execute();
        } else { // Abort button pressed
            worker.cancel(true);
            isRunning.set(false);
        }
    }

    // Saves the image on click
    private void saveButtonHandler() {
        JFileChooser chooser = new JFileChooser();
        int option = chooser.showSaveDialog(this.frame.getContentPane());
        if (option == JFileChooser.APPROVE_OPTION) {
            File selectedFile = chooser.getSelectedFile();
            String path = selectedFile.getAbsolutePath();
            try {
                String extension = "";
                int i = path.lastIndexOf('.');
                if (i >= 0)
                    extension = path.substring(i + 1);
                ImageIO.write(resImg, extension, new File(path));
            } catch (Exception e) {
                JOptionPane.showMessageDialog(this.frame.getContentPane(), "Can't save image :(");
            }
        }
    }

    // This method builds the GUI, very long and messy
    private void createAndShowGUI() {
        frame = new JFrame("Best GUI ever");
        frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        Container pane = frame.getContentPane();
        frame.pack();
        frame.setVisible(true);

        GridBagConstraints c = new GridBagConstraints();
        pane.setLayout(new GridBagLayout());
        c.fill = GridBagConstraints.HORIZONTAL;

        // Path edit
        JTextField pathField = new JTextField();
        pathField.getDocument().addDocumentListener(new DocumentListener() {
            public void changedUpdate(DocumentEvent e) {
                warn();
            }
            public void removeUpdate(DocumentEvent e) {
                warn();
            }
            public void insertUpdate(DocumentEvent e) {
                warn();
            }
            public void warn() {
                path = pathField.getText();
            }
        });
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
        c.ipady = 10;
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
        submitButton.addActionListener(actionEvent -> this.submitImageButtonHandler());
        pane.add(submitButton, c);

        // Save button
        JButton saveButton = new JButton("Save");
        c.fill = GridBagConstraints.HORIZONTAL;
        c.gridx = 2;
        c.gridy = 3;
        saveButton.addActionListener(actionEvent -> this.saveButtonHandler());
        pane.add(saveButton, c);

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

    // This method outputs the source image on the GUI
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

    // This method outputs the resulting image on the GUI
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

    // This method loads the source image from file
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

    // Fetches the filter list
    private void recvFilterList() {
        try {
            Socket s = new Socket(serverIp, serverPort);
            OutputStream outputStream = s.getOutputStream();
            DataInputStream inputStream = new DataInputStream(s.getInputStream());

            byte magicWord[] = { (byte) 0xFA }; // FA stands for Filter Array :)
            outputStream.write(magicWord);

            // Read the filters count
            byte len[] = new byte[4];
            inputStream.read(len);
            ByteBuffer wrapped = ByteBuffer.wrap(len);
            int size = wrapped.getInt();

            // Receive filters
            JComboBox comboBox = (JComboBox) frame.getContentPane().getComponent(6);
            String[] filters = new String[size];
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
            serverIp = InetAddress.getByName(JOptionPane.showInputDialog("Type the server IPv4", "127.0.0.1"));
        } catch (Exception e) {
            JOptionPane.showMessageDialog(this.frame, "Error: Can't parse or connect");
        }
        try {
            SwingUtilities.invokeAndWait(() -> createAndShowGUI());
        } catch (Exception e) {
            e.printStackTrace();
        }
        this.frame.setSize(400, 300);
        SwingUtilities.invokeLater(() -> recvFilterList());
    }
}