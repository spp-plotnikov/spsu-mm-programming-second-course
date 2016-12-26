/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package javafxbinarywsclient;

import java.awt.image.BufferedImage;
import java.io.Serializable;
import javafx.embed.swing.SwingFXUtils;
import javafx.scene.image.Image;
import javax.swing.ImageIcon;

/**
 *
 * @author Katrin
 */
public class ImageVO implements Serializable {

    public ImageIcon image;

    public ImageVO(Image fximage) {
        BufferedImage bufimage = SwingFXUtils.fromFXImage(fximage, null);
        this.image = new ImageIcon(bufimage);
    }
    
    public ImageVO(java.awt.Image awtImage) {
        this.image = new ImageIcon(awtImage);
    }

}
