import javax.imageio.ImageIO;
import javax.swing.*;
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
    JLabel jLabel2 = null;
    JLabel jLabel1 = null;
    JComboBox jComboBox = null;
    int sizeImageW = 350;
    int getSizeImageH = 350;
    private void addComponent() throws IOException {
        container.setLayout(null);

        jLabel1 = new JLabel("Загрузите изображение", SwingConstants.CENTER);
        container.add(jLabel1);
        jLabel1.setBounds(30, 50, sizeImageW, getSizeImageH);
        jLabel2 = new JLabel("Здесь будет полученное изображение", SwingConstants.CENTER);
        container.add(jLabel2);
        jLabel2.setBounds(420, 50, sizeImageW, getSizeImageH);

        progressBar = new JProgressBar();
        progressBar.setValue(0);
        progressBar.setStringPainted(true);
        progressBar.setBorder(BorderFactory.createTitledBorder("Прогресс..."));
        container.add(progressBar);
        progressBar.setBounds(10, 400, 780, 40);
        progressBar.setVisible(false);

        ArrayList<String> strings = new ArrayList();
        Socket socket = null;
        try {
            socket = new Socket("localhost", 2500);
            new Receiver(socket).recieveFilters(strings);
            socket.close();
        } catch (ConnectException ex) {
            JPanel myRootPane = new JPanel();
            JOptionPane.showMessageDialog(myRootPane, "Отсутствует соединение с сервером", "Ошибка", JOptionPane.DEFAULT_OPTION );
        }

        jComboBox = new JComboBox(strings.toArray());
        container.add(jComboBox);
        jComboBox.setBounds(10, 10, 200, 20);


        JButton button1 = new JButton("выбрать файл");
        container.add(button1);
        button1.setBounds(220, 10, 150, 20);


        JButton button3 = new JButton("Отмена");
        JButton button2 = new JButton("Отправить");
        container.add(button2);
        button2.setBounds(380, 10, 150, 20);
        button2.setEnabled(false);
        button2.addActionListener(new ActionListener() {
            public void actionPerformed(ActionEvent actionEvent) {
                progressBar.setVisible(true);
                button2.setVisible(false);
                button3.setVisible(true);
                new Thread(new forUpdate(button2, button3)).start();
            }
        });


        container.add(button3);
        button3.setBounds(380, 10, 150, 20);
        button3.addActionListener(new ActionListener() {
            public void actionPerformed(ActionEvent actionEvent) {
                button3.setVisible(false);
                button2.setVisible(true);
                newProgressBarValue(0);
                disconncet();
            }
        });
        button1.addActionListener(new ActionListener() {
            public void actionPerformed(ActionEvent actionEvent) {
                JFileChooser fc = new JFileChooser();
                int returnVal = fc.showOpenDialog(null);

                if (returnVal == JFileChooser.APPROVE_OPTION) {
                    File file = fc.getSelectedFile();
                    path = file.toString();
                    if (!path.toString().endsWith("jpg")) {
                        JPanel myRootPane = new JPanel();
                        JOptionPane.showMessageDialog(myRootPane, "выбрите jpg файл", "ошибка", JOptionPane.DEFAULT_OPTION );
                        return;
                    }

                    try {
                        jLabel1.setIcon(new ImageIcon(compressImage(ImageIO.read(new File(path)))));
                    } catch (IOException e) {
                        e.printStackTrace();
                    }
                }
                button2.setEnabled(true);
            }
        });
    }

    void disconncet() {
        try {
            socket.close();
        } catch (IOException e) {

        }
    }

    class forUpdate implements Runnable {
        private JButton button;
        private JButton button2;
        public forUpdate(JButton button, JButton button2) {
            this.button = button;
            this.button2 = button2;
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
                jLabel2.setIcon(new ImageIcon(compressImage(image)));
            } catch (ConnectException ex) {
                JPanel myRootPane = new JPanel();
                JOptionPane.showMessageDialog(myRootPane, "Отсутствует соединение с сервером", "ошибка", JOptionPane.DEFAULT_OPTION );
            } catch (SocketException ex) {
                //disconnect
            } catch (IOException e) {
                e.printStackTrace();
            }

            button2.setVisible(false);
            button.setVisible(true);
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

        JFrame frame = new JFrame("Приложение наложения фильтров на JPG");
        frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);


        container = frame.getContentPane();
        addComponent();
        frame.setSize(820, 500);
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
