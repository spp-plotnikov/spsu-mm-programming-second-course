package javafxbinarywsclient;

import com.sun.javafx.collections.ObservableListWrapper;
import javafx.beans.value.ChangeListener;
import javafx.beans.value.ObservableValue;
import javafx.fxml.FXML;
import javafx.scene.control.ComboBox;
import javafx.scene.control.ProgressIndicator;
import javafx.scene.image.Image;
import javafx.scene.image.ImageView;
import javafx.scene.image.WritableImage;
import javafx.stage.FileChooser;
import java.io.File;
import java.io.IOException;
import java.io.ObjectOutputStream;
import java.io.OutputStream;
import java.util.ArrayList;
import java.util.logging.Level;
import java.util.logging.Logger;
import javafx.beans.property.DoubleProperty;
import javax.websocket.Session;

/**
 * Created by vitaly on 05.11.2016.
 */
public class ImageTransformationController {

    @FXML
    private ImageView imageView;
    @FXML
    private ComboBox<String> comboBox;
    @FXML
    private ProgressIndicator progressIndicator;

    private JavaFXBinaryWsClient mainApp;
    private final FileChooser fileChooser = new FileChooser();
    private Session session;

    public void setSession(Session session) {
        this.session = session;
    }

    /**
     * Конструктор вызывается раньше метода initialize().
     */
    public ImageTransformationController() {
    }

    @FXML
    private void initialize() {

    }

    public void setComboBox(ArrayList list) {
        comboBox.setItems(new ObservableListWrapper<>(list));
        comboBox.getSelectionModel().selectedItemProperty().addListener(new ChangeListener<String>() {
            @Override
            public void changed(ObservableValue<? extends String> observableValue, String s, String t1) {
                applyFilter(t1);
            }
        });
    }

    @FXML
    private void handleOpen() {
        File file = fileChooser.showOpenDialog(mainApp.getPrimaryStage());
        if (file != null) {
            Image img = new Image("file:" + file.getPath());
            ImageVO image = new ImageVO(img);
            try (OutputStream output = session.getBasicRemote().getSendStream();
                    ObjectOutputStream oos = new ObjectOutputStream(output);) {
                oos.writeObject(image);
            } catch (IOException ex) {
                Logger.getLogger(ImageTransformationController.class.getName()).log(Level.SEVERE, null, ex);
            }
            imageView.setImage(img);
        }
    }

    public void bindProgress(DoubleProperty progressProperty) {
        progressIndicator.progressProperty().bindBidirectional(progressProperty);
    }

    private void applyFilter(final String nameOfFilter) {
        try {
            session.getBasicRemote().sendText(nameOfFilter);
        } catch (IOException ex) {
            Logger.getLogger(ImageTransformationController.class.getName()).log(Level.SEVERE, null, ex);
        }
    }

    public void setImage(Image image) {
        imageView.setImage(image);
    }

    @FXML
    private void onCancel() {
        try {
            session.getBasicRemote().sendText("cancel");
        } catch (IOException ex) {
            Logger.getLogger(ImageTransformationController.class.getName()).log(Level.SEVERE, null, ex);
        }
    }

    /**
     * Вызывается главным приложением, которое даёт на себя ссылку.
     *
     * @param mainApp
     */
    public void setMainApp(JavaFXBinaryWsClient mainApp) {
        this.mainApp = mainApp;
    }
}
