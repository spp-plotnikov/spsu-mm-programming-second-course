package com.lab.picture.view;

import com.lab.picture.Filters;
import com.lab.picture.MainApp;
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
import java.util.Arrays;
import java.util.List;

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

    private MainApp mainApp;
    private FileChooser fileChooser = new FileChooser();
    private Image img;
    private WritableImage dest;
    private int height;
    private int width;
    private boolean[] flag;
    private Filters filter;
    private File file;
    private boolean applyingFilter;

    // TODO: настроить получение списка от сервера
    private final static List<String> list = Arrays.asList("Gray", "SobelX", "SobelY");


    /**
     * Конструктор вызывается раньше метода initialize().
     */
    public ImageTransformationController() {
    }

    @FXML
    private void initialize() {
        comboBox.setItems(new ObservableListWrapper<String>(list));
        comboBox.getSelectionModel().selectedItemProperty().addListener(new ChangeListener<String>() {
            @Override
            public void changed(ObservableValue<? extends String> observableValue, String s, String t1) {
                applyFilter(t1);
            }
        });
    }

    @FXML
    private void handleOpen() {
        file = fileChooser.showOpenDialog(mainApp.getPrimaryStage());
        if (file != null) {
            img = new Image("file:" + file.getPath());
            imageView.setImage(img);
            height = (int) img.getHeight();
            width = (int) img.getWidth();
        }
    }

    /**
     * Вызывается, когда пользователь выбрал фильтр.
     *
     * @param nameOfFilter
     */
    private void applyFilter(final String nameOfFilter) {
        if (img != null && !applyingFilter) {
            flag = new boolean[1];
            flag[0] = true;
            dest = new WritableImage(width, height);
            filter = new Filters(img, dest, height, width, flag);
            progressIndicator.progressProperty().bindBidirectional(filter.getProgressProperty());

            new Thread(new Runnable() {
                @Override
                public void run() {
                    applyingFilter = true;
                    if (filter.applyFilter(nameOfFilter)) {
                        imageView.setImage(dest);
                        applyingFilter = false;
                    }
                }
            }).start();
        }
    }

    @FXML
    private void onCancel(){
        flag[0] = false;
    }

    /**
     * Вызывается главным приложением, которое даёт на себя ссылку.
     *
     * @param mainApp
     */
    public void setMainApp(MainApp mainApp) {
        this.mainApp = mainApp;
    }
}
