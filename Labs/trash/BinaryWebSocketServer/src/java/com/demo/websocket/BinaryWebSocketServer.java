//package com.demo.websocket;
//
//import java.io.ByteArrayInputStream;
//import java.io.IOException;
//import java.io.InputStream;
//import java.nio.ByteBuffer;
//import java.nio.charset.Charset;
//import java.util.Arrays;
//import java.util.Collections;
//import java.util.HashMap;
//import java.util.HashSet;
//import java.util.List;
//import java.util.Map;
//import java.util.Set;
//import java.util.concurrent.ConcurrentHashMap;
//import java.util.logging.Level;
//import java.util.logging.Logger;
//import javax.websocket.OnClose;
//import javax.websocket.OnMessage;
//import javax.websocket.OnOpen;
//import javax.websocket.Session;
//import javax.websocket.server.ServerEndpoint;
//import javafx.scene.image.Image;
//import javafx.scene.image.WritableImage;
//import java.io.ByteArrayOutputStream;
//import javax.imageio.ImageIO;
//import javafx.embed.swing.SwingFXUtils;
//
//@ServerEndpoint("/images")
//public class BinaryWebSocketServer {
//
//    private final static List<String> filters = Arrays.asList("Gray", "SobelX", "SobelY");
//    private static final ConcurrentHashMap<Session, User> users = new ConcurrentHashMap<>();
//
//    @OnOpen
//    public void onOpen(Session session) {
//
////        String namesOfFilters = "";
////        for (int i = 0; i < filters.size() - 1; i++) {
////            namesOfFilters = filters.get(i) + " ";
////        }
////        namesOfFilters += filters.get(filters.size());
////        try {
////            session.getBasicRemote().sendBinary(ByteBuffer.wrap(namesOfFilters.getBytes(Charset.forName("UTF-8"))));
////        } catch (IOException ex) {
////            Logger.getLogger(BinaryWebSocketServer.class.getName()).log(Level.SEVERE, null, ex);
////        }
////        users.put(session, new User(session));
//    }
//
//    @OnClose
//    public void onClose(Session session) {
//        users.remove(session);
//    }
//
//    @OnMessage
//    public void onMessage(InputStream inputStream, Session session) {
//        //if(!users.get(session).getFlag){
//
//        users.get(session).setFlag(true);
//        Image img = new Image(inputStream);
//        int height = (int) img.getHeight();
//        int width = (int) img.getWidth();
//
//        WritableImage dest = new WritableImage(width, height);
//        users.get(session).setDest(dest);
//        Filters filter = new Filters(img, dest, height, width);
//        users.get(session).setFilter(filter);
//        applyFilter("Gray", session);
//         
//    }
//
//    //TODO: применение фильтра на сервере
//    private void applyFilter(final String nameOfFilter, Session session) {
//        if (users.get(session).getImg() != null) {
//
//            new Thread(new Runnable() {
//                @Override
//                public void run() {
//
//                    if (users.get(session).getFilter().applyFilter(nameOfFilter)) {
//
//                        ByteArrayOutputStream byteOutput = new ByteArrayOutputStream();
//                        Image img = users.get(session).getImg();
//                        try {
//                            ImageIO.write(SwingFXUtils.fromFXImage(img, null), "jpeg", byteOutput);
//                        } catch (IOException ex) {
//                            Logger.getLogger(BinaryWebSocketServer.class.getName()).log(Level.SEVERE, null, ex);
//                        }
//                        ByteBuffer buf = ByteBuffer.wrap(byteOutput.toByteArray());
//                        try {
//                            session.getBasicRemote().sendBinary(buf);
//                        } catch (IOException ex) {
//                            Logger.getLogger(BinaryWebSocketServer.class.getName()).log(Level.SEVERE, null, ex);
//                        }
//
//                    }
//                }
//            }).start();
//        }
//    }
//}
package com.demo.websocket;

import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.io.OutputStream;
import java.nio.ByteBuffer;
import java.util.Collections;
import java.util.HashSet;
import java.util.Set;
import java.util.logging.Level;
import java.util.logging.Logger;
import javafxbinarywsclient.ImageVO;
import javax.websocket.MessageHandler;
import javax.websocket.OnClose;
import javax.websocket.OnMessage;
import javax.websocket.OnOpen;
import javax.websocket.Session;
import javax.websocket.server.ServerEndpoint;
import sun.misc.IOUtils;
import static sun.nio.cs.Surrogate.is;

@ServerEndpoint("/images")
public class BinaryWebSocketServer {

    private static final Set<Session> sessions = Collections.synchronizedSet(new HashSet<Session>());

    @OnOpen
    public void onOpen(Session session) {
        sessions.add(session);
    }

    @OnClose
    public void onClose(Session session) {
        sessions.remove(session);
    }

    @OnMessage
    public void onMessage(InputStream is, Session session1) throws IOException {
        try {
            ObjectInputStream oin = new ObjectInputStream(is);
            ImageVO ts = (ImageVO) oin.readObject();
            oin.close();
            int size = session1.getMaxBinaryMessageBufferSize();
            ByteArrayOutputStream bytesOut = new ByteArrayOutputStream();
            ObjectOutputStream oos = new ObjectOutputStream(bytesOut);
            oos.writeObject(ts);
            oos.flush();
            byte[] bytes = bytesOut.toByteArray();
            bytesOut.close();
            oos.close();
            ByteBuffer bb = ByteBuffer.wrap(bytes);
            //bb.flip();
            
                session1.getBasicRemote().sendBinary(bb);
            
        } catch (IOException ex) {
            Logger.getLogger(BinaryWebSocketServer.class.getName()).log(Level.SEVERE, null, ex);
        } catch (ClassNotFoundException ex) {
            Logger.getLogger(BinaryWebSocketServer.class.getName()).log(Level.SEVERE, null, ex);
        }

    }
}
