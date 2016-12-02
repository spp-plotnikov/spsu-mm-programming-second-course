import javax.imageio.ImageIO;
import javax.swing.*;
import javax.swing.filechooser.FileNameExtensionFilter;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.image.BufferedImage;
import java.io.File;
import java.io.IOException;
import java.net.ConnectException;
import java.net.Socket;
import java.net.SocketException;
import java.util.ArrayList;

public class Form {
    Socket socket = null;
    JProgressBar progressBar;
    String path = null;
    Container container;
    JLabel resultImage = null;
    JLabel sourceImage = null;
    JComboBox jComboBox = null;
    int sizeImageW = 350;
    int getSizeImageH = 350;
    private void addComponent() throws IOException {
        container.setLayout(null);

        sourceImage = new JLabel("Download image", SwingConstants.CENTER);
        container.add(sourceImage);
        sourceImage.setBounds(30, 50, sizeImageW, getSizeImageH);
        resultImage = new JLabel("There will be new image", SwingConstants.CENTER);
        container.add(resultImage);
        resultImage.setBounds(420, 50, sizeImageW, getSizeImageH);

        progressBar = new JProgressBar();
        progressBar.setValue(0);
        progressBar.setStringPainted(true);
        progressBar.setBorder(BorderFactory.createTitledBorder("Progress..."));
        container.add(progressBar);
        progressBar.setBounds(10, 400, 780, 40);
        progressBar.setVisible(false);

        ArrayList<String> strings = new ArrayList();

        try {
            socket = new Socket("localhost", 2500);
            new Receiver(socket).recieveFilters(strings);
            socket.close();
        } catch (ConnectException ex) {
            JPanel myRootPane = new JPanel();
            JOptionPane.showMessageDialog(myRootPane, "No connection with server", "Error", JOptionPane.DEFAULT_OPTION );
        }

        jComboBox = new JComboBox(strings.toArray());
        container.add(jComboBox);
        jComboBox.setBounds(10, 10, 200, 20);


        JButton openFileButton = new JButton("Open File");
        container.add(openFileButton);
        openFileButton.setBounds(220, 10, 150, 20);


        JButton cancelButton = new JButton("Cancel");
        JButton sendButton = new JButton("Send");
        container.add(sendButton);
        sendButton.setBounds(380, 10, 150, 20);
        sendButton.setEnabled(false);
        sendButton.addActionListener(new ActionListener() {
            public void actionPerformed(ActionEvent actionEvent) {
                progressBar.setVisible(true);
                newProgressBarValue(0);
                sendButton.setVisible(false);
                cancelButton.setVisible(true);
                new Thread(new NetworkHelper(sendButton, cancelButton)).start();
            }
        });


        container.add(cancelButton);
        cancelButton.setBounds(380, 10, 150, 20);
        cancelButton.addActionListener(new ActionListener() {
            public void actionPerformed(ActionEvent actionEvent) {
                cancelButton.setVisible(false);
                sendButton.setVisible(true);
                newProgressBarValue(0);
                disconnect();
            }
        });
        openFileButton.addActionListener(new ActionListener() {
            public void actionPerformed(ActionEvent actionEvent) {
                JFileChooser fc = new JFileChooser();
                fc.setAcceptAllFileFilterUsed(false);
                fc.addChoosableFileFilter(new FileNameExtensionFilter("Image (*.jpg)", "jpg"));
                int returnVal = fc.showOpenDialog(null);

                if (returnVal == JFileChooser.APPROVE_OPTION) {
                    File file = fc.getSelectedFile();
                    path = file.toString();
                    try {
                        sourceImage.setIcon(new ImageIcon(compressImage(ImageIO.read(new File(path)))));
                    } catch (IOException e) {
                        e.printStackTrace();
                    }
                }
                sendButton.setEnabled(true);
            }
        });
    }

    void disconnect() {
        try {
            socket.close();
        } catch (IOException e) {

        }
    }

    class NetworkHelper implements Runnable {
        private JButton sendButton;
        private JButton cancelButton;
        public NetworkHelper(JButton sendButton, JButton cancelButton) {
            this.sendButton = sendButton;
            this.cancelButton = cancelButton;
        }
        public void run() {
            BufferedImage image = null;
            try {
                socket = new Socket("localhost", 2500);
                Receiver receiver = new Receiver(socket);
                new Sender(socket).sendImage(path, jComboBox.getSelectedItem().toString());
                byte b = 0;
                while (b != 100) {
                    b = receiver.recv();
                    newProgressBarValue(b);
                }
                image = new Receiver(socket).receiveImage();
                resultImage.setIcon(new ImageIcon(compressImage(image)));
            } catch (ConnectException ex) {
                JPanel myRootPane = new JPanel();
                JOptionPane.showMessageDialog(myRootPane, "No connection with server", "Error", JOptionPane.DEFAULT_OPTION );
            } catch (SocketException ex) {
                //disconnect
            } catch (IOException e) {
                e.printStackTrace();
            }

            cancelButton.setVisible(false);
            sendButton.setVisible(true);
        }
    }
    BufferedImage compressImage(BufferedImage originalImage) throws IOException {
        BufferedImage scaled = new BufferedImage(sizeImageW, getSizeImageH,
                BufferedImage.TYPE_INT_RGB);
        Graphics2D g = scaled.createGraphics();
        g.setRenderingHint(RenderingHints.KEY_INTERPOLATION,
                RenderingHints.VALUE_INTERPOLATION_BICUBIC);
        g.setRenderingHint(RenderingHints.KEY_RENDERING,
                RenderingHints.VALUE_RENDER_QUALITY);
        g.drawImage(originalImage, 0, 0, scaled.getWidth(), scaled.getHeight(), null);
        g.dispose();

        return scaled;
    }

    void newProgressBarValue(int n) {
        progressBar.setValue(n);
    }
    private void createAndShowGUI() throws IOException {
        JFrame.setDefaultLookAndFeelDecorated(true);

        JFrame frame = new JFrame("Filters overlay application to JPG");
        frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);


        container = frame.getContentPane();
        addComponent();
        frame.setSize(820, 500);
        frame.setResizable(false);
        frame.setVisible(true);
    }


    void start() {
        SwingUtilities.invokeLater(new Runnable() {
            @Override
            public void run() {
                try {
                    createAndShowGUI();
                } catch (IOException e) {
                    e.printStackTrace();
                }
            }
        });
    }

}