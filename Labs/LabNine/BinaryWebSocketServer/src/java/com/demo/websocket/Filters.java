package com.demo.websocket;

import javafx.beans.property.DoubleProperty;
import javafx.beans.property.SimpleDoubleProperty;
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

    private final WritableImage dest;
    private final int height;
    private final int width;
    private final static List<String> FILTERS = Arrays.asList("Gray", "SobelX", "SobelY");
    private double progress;
    private final User user;
    private boolean flag;

    public Filters(User user, int height, int width) {
        this.user = user;
        this.dest = user.getDestImage();
        this.height = height;
        this.width = width;
        this.progress = 0;
        this.flag = true;
    }

    public Double getProgressProperty() {
        return progress;
    }

    public void setFlag(boolean flag) {
        this.flag = flag;
    }

    public boolean applyFilter(String nameF) {
        progress = 0;
        boolean answer = false;
        switch (FILTERS.indexOf(nameF)) {
            case 0:
                System.out.println("Start filt");
                answer = gray();
                System.out.println("End filt");
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

    private boolean gray() {

        PixelReader reader = user.getImage().getPixelReader();
        PixelWriter writer = dest.getPixelWriter();
        int size = (height - 1) * (width - 1);

        for (int x = 1; x < width - 1; x++) {
            for (int y = 1; y < height - 1; y++) {
                if (flag) {

                    Color color = reader.getColor(x, y);
                    double r = color.getRed();
                    double g = color.getGreen();
                    double b = color.getBlue();

                    double sum = (r + b + g) / 3;
                    Color grayColor = Color.color(sum, sum, sum);
                    writer.setColor(x, y, grayColor);

                    if ((x % 10) == 1) {
                        progress = ((double) x / (width - 1));
                        user.getSession().getAsyncRemote().sendText(String.valueOf(progress));
                    }

                } else {
                    return false;
                }

            }
        }
        return true;

    }

    private boolean sobel(String name) {

        PixelReader reader = user.getImage().getPixelReader();
        PixelWriter writer = dest.getPixelWriter();

        int[][] maskX = new int[][]{{3, 10, 3}, {0, 0, 0}, {-3, -10, -3}};
        int[][] maskY = new int[][]{{3, 0, -3}, {10, 0, -10}, {3, 0, -3}};
        double limit = 0.5;

        for (int x = 1; x < width - 1; x++) {
            for (int y = 1; y < height - 1; y++) {
                if (flag) {

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
                    if ((x % 10) == 1) {
                        progress = ((double) x / (width - 1));
                        user.getSession().getAsyncRemote().sendText(String.valueOf(progress));

                    }
                } else {
                    return false;
                }
            }

        }
        return true;
    }
}
