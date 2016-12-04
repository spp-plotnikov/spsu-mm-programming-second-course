package com.demo.websocket;

import java.awt.Graphics2D;
import java.awt.image.BufferedImage;
import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.nio.ByteBuffer;
import java.util.Arrays;
import java.util.HashMap;
import java.util.List;
import java.util.logging.Level;
import java.util.logging.Logger;
import static javafx.embed.swing.SwingFXUtils.toFXImage;
import javafx.scene.image.Image;
import javafx.scene.image.WritableImage;
import javafxbinarywsclient.ImageVO;
import javax.websocket.OnClose;
import javax.websocket.OnMessage;
import javax.websocket.OnOpen;
import javax.websocket.Session;
import javax.websocket.server.ServerEndpoint;

@ServerEndpoint("/images")
public class BinaryWebSocketServer {

    private final static List<String> FILTERS = Arrays.asList("Gray", "SobelX", "SobelY");

    private final HashMap<Session, User> informationAboutSession = new HashMap<>();

    @OnOpen
    public void onOpen(Session session) {
        informationAboutSession.put(session, new User(null, false, session));
        String namesOfFilters = "";
        for (int i = 0; i < FILTERS.size() - 1; i++) {
            namesOfFilters += FILTERS.get(i) + " ";
        }
        namesOfFilters += FILTERS.get(FILTERS.size() - 1);
        try {
            session.getBasicRemote().sendText(namesOfFilters);
        } catch (IOException ex) {
            Logger.getLogger(BinaryWebSocketServer.class.getName()).log(Level.SEVERE, null, ex);
        }
    }

    @OnClose
    public void onClose(Session session) {
        informationAboutSession.remove(session);
    }

    @OnMessage
    public void textMessage(Session session, String msg) {
        if (msg.equals("cancel")) {
            informationAboutSession.get(session).getFilter().setFlag(false);
            informationAboutSession.get(session).setApplyingFilt(false);
        } else {
            applyFilter(msg, session);
        }
    }

    @OnMessage
    public void binaryMessage(InputStream inputStream, Session session) throws IOException {
        try {
            ImageVO imageVO;
            try (ObjectInputStream oin = new ObjectInputStream(inputStream)) {
                imageVO = (ImageVO) oin.readObject();
            }
            WritableImage wimg = new WritableImage(imageVO.image.getIconWidth(), imageVO.image.getIconHeight());

            BufferedImage bufferedImage = toBufferedImage(imageVO.image.getImage());
            Image fxImage = toFXImage(bufferedImage, wimg);
            informationAboutSession.get(session).setImage(fxImage);
        } catch (ClassNotFoundException ex) {
            Logger.getLogger(BinaryWebSocketServer.class.getName()).log(Level.SEVERE, null, ex);
        }
    }

    public BufferedImage toBufferedImage(java.awt.Image awtImage) {
        if (awtImage instanceof BufferedImage) {
            return (BufferedImage) awtImage;
        }
        // Create a buffered image with transparency
        BufferedImage bimage = new BufferedImage(awtImage.getWidth(null), awtImage.getHeight(null), BufferedImage.TYPE_INT_ARGB);
        // Draw the image on to the buffered image
        Graphics2D bGr = bimage.createGraphics();
        bGr.drawImage(awtImage, 0, 0, null);
        bGr.dispose();

        // Return the buffered image
        return bimage;
    }

    private void applyFilter(final String nameOfFilter, Session session) {
        Image image = informationAboutSession.get(session).getImage();
        if (image != null && !informationAboutSession.get(session).isApplyingFilt()) {
            int width = (int) image.getWidth();
            int height = (int) image.getHeight();
            informationAboutSession.get(session).setDestImage(width, height);
            Filters filter = new Filters(informationAboutSession.get(session), height, width);

            informationAboutSession.get(session).setFilter(filter);
            informationAboutSession.get(session).getFilter().setFlag(true);
            new Thread(new Runnable() {
                @Override
                public void run() {
                    informationAboutSession.get(session).setApplyingFilt(true);
                    if (informationAboutSession.get(session).getFilter().applyFilter(nameOfFilter)) {
                        sendImage(informationAboutSession.get(session).getDestImage(), session);
                        informationAboutSession.get(session).setApplyingFilt(false);
                    }
                }
            }).start();
        }
    }

    private void sendImage(WritableImage dest, Session session) {
        try {
            ImageVO imageVO = new ImageVO(dest);
            ObjectOutputStream oos;
            byte[] bytes;
            try (ByteArrayOutputStream bytesOut = new ByteArrayOutputStream()) {
                oos = new ObjectOutputStream(bytesOut);
                oos.writeObject(imageVO);
                oos.flush();
                bytes = bytesOut.toByteArray();
            }
            oos.close();
            ByteBuffer bb = ByteBuffer.wrap(bytes);
            session.getBasicRemote().sendBinary(bb);
        } catch (IOException ex) {
            Logger.getLogger(BinaryWebSocketServer.class.getName()).log(Level.SEVERE, null, ex);
        }
    }

}
