/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package test;

import java.awt.Image;
import java.io.File;
import java.io.IOException;
import java.io.InputStream;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.io.OutputStream;
import java.net.URI;
import java.nio.ByteBuffer;
import java.util.concurrent.Callable;
import java.util.logging.Level;
import java.util.logging.Logger;
import javafxbinarywsclient.ByteBufferBackedInputStream;
import javafxbinarywsclient.ImageTransformationController;
import javafxbinarywsclient.ImageVO;
import javafxbinarywsclient.JavaFXBinaryWsClient;
import javax.imageio.ImageIO;
import javax.websocket.ClientEndpoint;
import javax.websocket.DeploymentException;
import javax.websocket.OnMessage;
import javax.websocket.OnOpen;
import javax.websocket.Session;
import org.glassfish.tyrus.client.ClientManager;

/**
 *
 * @author Katrin
 */
@ClientEndpoint
public class Client implements Callable<Long> {

    private long start;
    private long finish;
    private Session session;
    public volatile long result;
    
    @OnOpen
    public void onOpen(Session session) {
        this.session = session;
    }

    @OnMessage
    public void textMessage(Session session, String msg) {
    }

    @OnMessage
    public void onMessage(ByteBuffer byteBuffer) {
        try {
            InputStream inputStream = new ByteBufferBackedInputStream(byteBuffer);
            ObjectInputStream oos = new ObjectInputStream(inputStream);
            ImageVO imageVO = (ImageVO) oos.readObject();

        } catch (IOException | ClassNotFoundException ex) {
            Logger.getLogger(JavaFXBinaryWsClient.class.getName()).log(Level.SEVERE, null, ex);
        }
        try {
            session.close();
        } catch (IOException ex) {
            Logger.getLogger(Client.class.getName()).log(Level.SEVERE, null, ex);
        }
        finish = System.currentTimeMillis();
        result = finish - start;
    }

    @Override
    public Long call() {
        ClientManager client = ClientManager.createClient();
        client.getProperties().put("org.glassfish.tyrus.incomingBufferSize", 170000000); // sets the incoming buffer size to 170000000 bytes.
        URI uri = URI.create("ws://localhost:8080/BinaryWebSocketServer/images");
        try {
            client.connectToServer(this, uri);
        } catch (DeploymentException | IOException ex) {
            Logger.getLogger(JavaFXBinaryWsClient.class
                    .getName()).log(Level.SEVERE, null, ex);
        }
        start = System.currentTimeMillis();
        sendImage("D:\\child.jpg");

        try {
            Thread.sleep(3000);
        } catch (InterruptedException ex) {
            Logger.getLogger(Client.class.getName()).log(Level.SEVERE, null, ex);
        }
        applyFilter("Gray");
        while (result == 0) {
        }
        return result;
    }

    public void sendImage(String path) {
        File file = new File(path);
            try {
                Image img;
                img = ImageIO.read(file);
                ImageVO image = new ImageVO(img);
                try (OutputStream output = session.getBasicRemote().getSendStream();
                        ObjectOutputStream oos = new ObjectOutputStream(output);) {
                    oos.writeObject(image);

                } catch (IOException ex) {
                    Logger.getLogger(ImageTransformationController.class
                            .getName()).log(Level.SEVERE, null, ex);

                }
            } catch (IOException ex) {
                Logger.getLogger(Client.class
                        .getName()).log(Level.SEVERE, null, ex);
            }

    }

    private void applyFilter(final String nameOfFilter) {
        try {
            session.getBasicRemote().sendText(nameOfFilter);

        } catch (IOException ex) {
            Logger.getLogger(ImageTransformationController.class
                    .getName()).log(Level.SEVERE, null, ex);
        }
    }

}
