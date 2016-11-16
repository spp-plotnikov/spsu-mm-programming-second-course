package com.lab.picture;

import com.lab.picture.view.ImageTransformationController;
import javafx.application.Application;
import javafx.fxml.FXMLLoader;
import javafx.scene.Scene;
import javafx.scene.layout.AnchorPane;
import javafx.scene.layout.BorderPane;
import javafx.stage.Stage;

import java.io.IOException;

/**
 * Created by vitaly on 05.11.2016.
 */

public class MainApp extends Application {

    private Stage primaryStage;
    private BorderPane rootLayout;
    private ImageTransformationController imageTransformationController;

    @Override
    public void start(Stage primaryStage) {
        this.primaryStage = primaryStage;
        this.primaryStage.setTitle("ImageTransformation");

        initRootLayout();

        showImageRedactor();
    }

    /**
     * Инициализирует корневой макет.
     */
    public void initRootLayout() {
        try {
            // Загружаем корневой макет из fxml файла.
            FXMLLoader loader = new FXMLLoader();
            loader.setLocation(MainApp.class.getResource("view/RootLayout.fxml"));
            rootLayout = (BorderPane) loader.load();

            // Отображаем сцену, содержащую корневой макет.
            Scene scene = new Scene(rootLayout);
            primaryStage.setScene(scene);
            primaryStage.setResizable(false);
            primaryStage.show();

        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    /**
     * Показывает в корневом макете основное окно.
     */
    public void showImageRedactor() {
        try {
            // Загружаем основное окно.
            FXMLLoader loader = new FXMLLoader();
            loader.setLocation(MainApp.class.getResource("view/ImageTransformation.fxml"));
            AnchorPane imageTransformation = (AnchorPane) loader.load();

            // Помещаем окно в центр корневого макета.
            rootLayout.setCenter(imageTransformation);

            // Даём контроллеру доступ к главному приложению.
            imageTransformationController = loader.getController();
            imageTransformationController.setMainApp(this);

        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    /**
     * Возвращает главную сцену.
     *
     * @return primaryStage
     */
    public Stage getPrimaryStage() {
        return primaryStage;
    }

    public static void main(String[] args) {
        launch(args);
    }
}
