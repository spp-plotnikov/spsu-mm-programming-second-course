package javafxbinarywsclient;

import java.awt.Graphics2D;
import java.awt.image.BufferedImage;
import java.io.IOException;
import java.io.InputStream;
import java.io.ObjectInputStream;
import java.net.URI;
import java.nio.ByteBuffer;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.logging.Level;
import java.util.logging.Logger;
import javafx.application.Application;
import javafx.beans.property.DoubleProperty;
import javafx.beans.property.SimpleDoubleProperty;
import static javafx.embed.swing.SwingFXUtils.toFXImage;
import javafx.fxml.FXMLLoader;
import javafx.scene.Scene;
import javafx.scene.image.Image;
import javafx.scene.image.ImageView;
import javafx.scene.image.WritableImage;
import javafx.scene.layout.AnchorPane;
import javafx.scene.layout.BorderPane;
import javafx.stage.Stage;
import javax.websocket.ClientEndpoint;
import javax.websocket.DeploymentException;
import javax.websocket.OnClose;
import javax.websocket.OnMessage;
import javax.websocket.OnOpen;
import javax.websocket.Session;
import org.glassfish.tyrus.client.ClientManager;

@ClientEndpoint
public class JavaFXBinaryWsClient extends Application {

    private static final Logger LOGGER = Logger.getLogger(JavaFXBinaryWsClient.class.getName());
    private ImageView imageView;
    private Session session;
    private final ArrayList<String> filters = new ArrayList<>();
    private boolean firstTime = true;
    private Stage primaryStage;
    private BorderPane rootLayout;
    private ImageTransformationController imageTransformationController;
    private DoubleProperty progressProperty;
    private double progress;

    @OnOpen
    public void onOpen(Session session) {
        this.session = session;
        progressProperty = new SimpleDoubleProperty(0);
    }

    @OnMessage
    public void textMessage(Session session, String msg) {
        if (firstTime) {
            String[] filtrs = msg.split(" ");
            filters.addAll(Arrays.asList(filtrs));
            firstTime = false;
        } else {
            if (msg != null) {
                progress = Double.parseDouble(msg);
                progressProperty.set(progress);
            }
        }
    }

    //слишком сложная передача туда-сюда..
    @OnMessage
    public void onMessage(ByteBuffer byteBuffer) {
        try {
            InputStream inputStream = new ByteBufferBackedInputStream(byteBuffer);
            ObjectInputStream oos = new ObjectInputStream(inputStream);
            ImageVO imageVO = (ImageVO) oos.readObject();
            System.out.println("WebSocket message Received!");
            WritableImage wimg = new WritableImage(imageVO.image.getIconWidth(), imageVO.image.getIconHeight());

            BufferedImage buffered = toBufferedImage(imageVO.image.getImage());
            Image image = toFXImage(buffered, wimg);
            imageTransformationController.setImage(image);
        } catch (IOException | ClassNotFoundException ex) {
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
        this.primaryStage = primaryStage;
        this.primaryStage.setTitle("ImageTransformation");
        initRootLayout();
        showImageRedactor();
    }

    public void initRootLayout() {
        try {
            // Загружаем корневой макет из fxml файла.
            FXMLLoader loader = new FXMLLoader();
            loader.setLocation(javafxbinarywsclient.JavaFXBinaryWsClient.class.getResource("RootLayout.fxml"));
            rootLayout = (BorderPane) loader.load();

            // Отображаем сцену, содержащую корневой макет.
            Scene scene = new Scene(rootLayout);
            primaryStage.setScene(scene);
            primaryStage.setResizable(false);
            primaryStage.show();

        } catch (IOException ex) {
            Logger.getLogger(JavaFXBinaryWsClient.class.getName()).log(Level.SEVERE, null, ex);
        }
    }

    public void showImageRedactor() {
        try {
            // Загружаем основное окно.
            FXMLLoader loader = new FXMLLoader();
            loader.setLocation(javafxbinarywsclient.JavaFXBinaryWsClient.class.getResource("ImageTransformation.fxml"));
            AnchorPane imageTransformation = (AnchorPane) loader.load();

            // Помещаем окно в центр корневого макета.
            rootLayout.setCenter(imageTransformation);

            // Даём контроллеру доступ к главному приложению.
            imageTransformationController = loader.getController();
            imageTransformationController.setMainApp(this);
            imageTransformationController.setSession(session);
            imageTransformationController.setComboBox(filters);
            imageTransformationController.bindProgress(progressProperty);

        } catch (IOException ex) {
            Logger.getLogger(JavaFXBinaryWsClient.class.getName()).log(Level.SEVERE, null, ex);

        }
    }

    public static void main(String[] args) {
        launch(args);
    }

    public Stage getPrimaryStage() {
        return primaryStage;
    }

    private void connectToWebSocket() {
        ClientManager client = ClientManager.createClient();
        client.getProperties().put("org.glassfish.tyrus.incomingBufferSize", 170000000); // sets the incoming buffer size to 170000000 bytes.
        URI uri = URI.create("ws://localhost:8080/BinaryWebSocketServer/images");
        try {
            client.connectToServer(this, uri);
        } catch (DeploymentException | IOException ex) {
            Logger.getLogger(JavaFXBinaryWsClient.class
                    .getName()).log(Level.SEVERE, null, ex);
        }
    }
}
