package javafxbinarywsclient;

import java.awt.Graphics2D;
import java.awt.image.BufferedImage;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.io.OutputStream;
import java.net.URI;
import java.nio.ByteBuffer;
import java.util.logging.Level;
import java.util.logging.Logger;
import javafx.application.Application;
import static javafx.embed.swing.SwingFXUtils.toFXImage;
import javafx.event.ActionEvent;
import javafx.event.EventHandler;
import javafx.scene.Scene;
import javafx.scene.control.Button;
import javafx.scene.image.Image;
import javafx.scene.image.ImageView;
import javafx.scene.image.WritableImage;
import javafx.scene.layout.AnchorPane;
import javafx.stage.FileChooser;
import javafx.stage.Stage;
import javax.websocket.ClientEndpoint;
import javax.websocket.ContainerProvider;
import javax.websocket.DeploymentException;
import javax.websocket.OnClose;
import javax.websocket.OnMessage;
import javax.websocket.OnOpen;
import javax.websocket.Session;
import javax.websocket.WebSocketContainer;

@ClientEndpoint
public class JavaFXBinaryWsClient extends Application {

    private static final Logger LOGGER = Logger.getLogger(JavaFXBinaryWsClient.class.getName());
    private ImageView imageView;
    private Session session;

    @OnOpen
    public void onOpen(Session session) {
        this.session = session;
    }

    @OnMessage
    public void onMessage(ByteBuffer input) {
        try {
            InputStream is = new ByteBufferBackedInputStream(input);
            ObjectInputStream oos = new ObjectInputStream(is);
            ImageVO img = (ImageVO) oos.readObject();
            System.out.println("WebSocket message Received!");
            WritableImage wimg = new WritableImage(img.image.getIconWidth(), img.image.getIconHeight());
          
            BufferedImage buffered = toBufferedImage(img.image.getImage());
            Image image = toFXImage(buffered,wimg);
            imageView.setImage(image);
        } catch (IOException ex) {
            Logger.getLogger(JavaFXBinaryWsClient.class.getName()).log(Level.SEVERE, null, ex);
        } catch (ClassNotFoundException ex) {
            Logger.getLogger(JavaFXBinaryWsClient.class.getName()).log(Level.SEVERE, null, ex);
        }

    }
    public BufferedImage toBufferedImage(java.awt.Image img) {
        if (img instanceof BufferedImage) {
            return (BufferedImage) img;
        }

        // Create a buffered image with transparency
        BufferedImage bimage = new BufferedImage(img.getWidth(null), img.getHeight(null), BufferedImage.TYPE_INT_ARGB);

        // Draw the image on to the buffered image
        Graphics2D bGr = bimage.createGraphics();
        bGr.drawImage(img, 0, 0, null);
        bGr.dispose();

        // Return the buffered image
        return bimage;
    }

    @OnClose
    public void onClose() {
        connectToWebSocket();
    }

    @Override
    public void start(final Stage primaryStage) {
        connectToWebSocket();

        Button btn = new Button();
        btn.setText("Send Image!");
        btn.setPrefSize(400, 27);
        btn.setOnAction(new EventHandler<ActionEvent>() {
            @Override
            public void handle(ActionEvent event) {
                selectAndSendImage(primaryStage);
            }
        });
        imageView = new ImageView();
        imageView.setFitHeight(400);
        imageView.setFitWidth(400);
        imageView.setPreserveRatio(true);
        imageView.setSmooth(true);

        AnchorPane root = new AnchorPane();

        AnchorPane.setTopAnchor(btn, 0.0);
        AnchorPane.setLeftAnchor(btn, 0.0);
        AnchorPane.setRightAnchor(btn, 0.0);
        AnchorPane.setTopAnchor(imageView, 27.0);
        AnchorPane.setBottomAnchor(imageView, 0.0);
        AnchorPane.setLeftAnchor(imageView, 0.0);
        AnchorPane.setRightAnchor(imageView, 0.0);

        root.getChildren().add(btn);
        root.getChildren().add(imageView);

        Scene scene = new Scene(root, 400, 427);

        primaryStage.setTitle("Image push!");
        primaryStage.setScene(scene);
        primaryStage.show();
    }

    public static void main(String[] args) {
        launch(args);
    }

    private void selectAndSendImage(Stage stage) {
        FileChooser fileChooser = new FileChooser();
        fileChooser.setTitle("Select Image to Send");
        File file = fileChooser.showOpenDialog(stage);
        Image img = new Image("file:" + file.getPath());
        ImageVO image = new ImageVO(img);
        try (OutputStream output = session.getBasicRemote().getSendStream();
                ObjectOutputStream oos = new ObjectOutputStream(output);) {
            oos.writeObject(image);

        } catch (IOException ex) {
            Logger.getLogger(JavaFXBinaryWsClient.class.getName()).log(Level.SEVERE, null, ex);
        }
    }

    private void connectToWebSocket() {
        WebSocketContainer container = ContainerProvider.getWebSocketContainer();
        try {
            URI uri = URI.create("ws://localhost:8080/BinaryWebSocketServer/images");
            container.connectToServer(this, uri);
        } catch (DeploymentException | IOException ex) {
            LOGGER.log(Level.SEVERE, null, ex);
            System.exit(-1);
        }
    }
    
    class ByteBufferBackedInputStream extends InputStream {

        ByteBuffer buf;

        ByteBufferBackedInputStream(ByteBuffer buf) {
            this.buf = buf;
        }

        public synchronized int read() throws IOException {
            if (!buf.hasRemaining()) {
                return -1;
            }
            return buf.get();
        }

        public synchronized int read(byte[] bytes, int off, int len) throws IOException {
            len = Math.min(len, buf.remaining());
            buf.get(bytes, off, len);
            return len;
        }
    }

    class ByteBufferBackedOutputStream extends OutputStream {

        ByteBuffer buf;

        ByteBufferBackedOutputStream(ByteBuffer buf) {
            this.buf = buf;
        }

        public synchronized void write(int b) throws IOException {
            buf.put((byte) b);
        }

        public synchronized void write(byte[] bytes, int off, int len) throws IOException {
            buf.put(bytes, off, len);
        }
    }
}
