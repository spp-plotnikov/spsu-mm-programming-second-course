package com.lab.picture;

import javafx.beans.property.DoubleProperty;
import javafx.beans.property.SimpleDoubleProperty;
import javafx.scene.control.ProgressIndicator;
import javafx.scene.image.Image;
import javafx.scene.image.PixelReader;
import javafx.scene.image.PixelWriter;
import javafx.scene.image.WritableImage;
import javafx.scene.paint.Color;

import java.util.Arrays;
import java.util.List;

import static java.lang.Math.sqrt;

/**
 * Created by Katrin on 06.11.2016.
 */
public class Filters {

    private Image img;
    private WritableImage dest;
    private int height;
    private int width;
    private boolean[] flag;
    private final static List<String> list = Arrays.asList("Gray", "SobelX", "SobelY");
    private DoubleProperty progressProperty;

    public Filters(Image img, WritableImage dest, int height, int width, boolean[] flag) {
        this.img = img;
        this.dest = dest;
        this.height = height;
        this.width = width;
        this.flag = flag;
        progressProperty = new SimpleDoubleProperty(0);
    }

    public DoubleProperty getProgressProperty(){
        return progressProperty;
    }

    public boolean applyFilter(String nameF) {
        progressProperty.set(0);
        boolean answer = false;
        switch (list.indexOf(nameF)) {
            case 0:
                answer = gray();
                break;
            case 1:
                answer = sobel(nameF);
                break;
            case 2:
                answer = sobel(nameF);
                break;
        }
        return answer;
    }

    public boolean gray() {

        PixelReader reader = img.getPixelReader();
        PixelWriter writer = dest.getPixelWriter();
        int size = (height - 1) * (width - 1);

        for (int x = 1; x < width - 1; x++) {
            for (int y = 1; y < height - 1; y++) {
                if (flag[0]) {
                    Color color = reader.getColor(x, y);
                    double r = color.getRed();
                    double g = color.getGreen();
                    double b = color.getBlue();

                    double sum = (r + b + g) / 3;
                    Color grayColor = Color.color(sum, sum, sum);
                    writer.setColor(x, y, grayColor);
                } else {
                    return false;
                }
            }
            progressProperty.set((double)x / (width - 1));
        }
        return true;
    }


    public boolean sobel(String name) {

        PixelReader reader = img.getPixelReader();
        PixelWriter writer = dest.getPixelWriter();

        int[][] maskX = new int[][]{{3, 10, 3}, {0, 0, 0}, {-3, -10, -3}};
        int[][] maskY = new int[][]{{3, 0, -3}, {10, 0, -10}, {3, 0, -3}};
        double limit = 0.5;

        for (int x = 1; x < width - 1; x++) {
            for (int y = 1; y < height - 1; y++) {
                if (flag[0]) {
                    double bX = 0, gX = 0, rX = 0;
                    double bY = 0, gY = 0, rY = 0;
                    if (name.equals("SobelX")) {
                        for (int i = -1; i < 2; i++) {
                            for (int j = -1; j < 2; j++) {
                                Color color = reader.getColor(x + i, y + j);
                                bX += color.getBlue() * maskX[i + 1][j + 1] / 3;
                                gX += color.getGreen() * maskX[i + 1][j + 1] / 3;
                                rX += color.getRed() * maskX[i + 1][j + 1] / 3;
                            }
                        }
                    } else {
                        for (int i = -1; i < 2; i++) {
                            for (int j = -1; j < 2; j++) {
                                Color color = reader.getColor(x + i, y + j);
                                bY += color.getBlue() * maskY[i + 1][j + 1] / 3;
                                gY += color.getGreen() * maskY[i + 1][j + 1] / 3;
                                rY += color.getRed() * maskY[i + 1][j + 1] / 3;
                            }
                        }
                    }
                    double sumX = bX + rX + gX;
                    double sumY = bY + rY + gY;
                    double sum = sqrt(sumX * sumX + sumY * sumY);
                    Color sobelColor;
                    if (sum <= limit) {
                        sobelColor = Color.color(0, 0, 0);
                    } else {
                        sobelColor = Color.color(1, 1, 1);
                    }
                    writer.setColor(x, y, sobelColor);
                } else {
                    return false;
                }
            }
            progressProperty.set(((double)x / (width - 1))) ;
        }
        return true;
    }

    // Не видно его совсем, так что не нужен.....Так?
    /*public boolean gauss() {

        PixelReader reader = img.getPixelReader();
        PixelWriter writer = dest.getPixelWriter();

        int[][] mask = new int[][]{{1, 2, 1}, {2, 4, 2}, {1, 2, 1}};
        double b = 0, g = 0, r = 0;
        for (int x = 1; x < width - 1; x++) {
            for (int y = 1; y < height - 1; y++) {
                if (flag[0]) {
                    for (int i = -1; i < 2; i++) {
                        for (int j = -1; j < 2; j++) {
                            Color color = reader.getColor(x + i, y + j);
                            b += color.getBlue() * mask[i + 1][j + 1];
                            g += color.getGreen() * mask[i + 1][j + 1];
                            r += color.getRed() * mask[i + 1][j + 1];
                        }
                    }
                    Color gaussColor = Color.color(r / 16, g / 16, b / 16);
                    writer.setColor(x, y, gaussColor);
                    b = g = r = 0;
                } else {
                    return false;
                }
            }
        }
        return true;
    }
    */
}
