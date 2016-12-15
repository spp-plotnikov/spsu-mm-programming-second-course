import javax.imageio.ImageIO;
import javax.swing.*;
import java.awt.image.BufferedImage;
import java.io.EOFException;
import java.io.File;
import java.io.IOException;
import java.net.ConnectException;
import java.net.Socket;
import java.net.SocketException;
import java.util.concurrent.Callable;

public class Client implements Callable {

    public Long call() {
        long startTime = System.currentTimeMillis();
        Socket socket = null;
        try {
            socket = new Socket("localhost", 2500);
            Receiver receiver = new Receiver(socket);
            new Sender(socket).sendImage("test.jpg", "Identity");
            byte b = 0;
            while (b != 100) {
                b = receiver.recv();
            }
            BufferedImage image = new Receiver(socket).receiveImage();
            File out = new File("saved.png");
            ImageIO.write(image, "jpg", out);
        } catch (ConnectException ex) {
            JPanel myRootPane = new JPanel();
            JOptionPane.showMessageDialog(myRootPane, "No connection with server", "Error", JOptionPane.DEFAULT_OPTION );
        } catch (SocketException ex) {
            //disconnect
        } catch (EOFException exc) {
//            socket.close();
        } catch (IOException e) {
            e.printStackTrace();
        }
        return System.currentTimeMillis() - startTime;
    }
}
