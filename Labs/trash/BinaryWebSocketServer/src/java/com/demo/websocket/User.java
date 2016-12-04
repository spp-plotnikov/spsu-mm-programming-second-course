
/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.demo.websocket;

import javafx.scene.image.Image;
import javafx.scene.image.WritableImage;
import javax.websocket.Session;

/**
 *
 * @author Katrin
 */
public class User {

    private Image image;
    private boolean applyingFilt;
    private final Session session;
    private Filters filter;
    private WritableImage dest;

    public Session getSession() {
        return session;
    }

    public Filters getFilter() {
        return filter;
    }

    public void setFilter(Filters filter) {
        this.filter = filter;
    }

    public User(Image image, boolean applyingFilt, Session session) {
        this.session = session;
        this.image = image;
        this.applyingFilt = applyingFilt;
    }

    public Image getImage() {
        return image;
    }

    public void setImage(Image image) {
        this.image = image;
    }

    public boolean isApplyingFilt() {
        return applyingFilt;
    }

    public void setApplyingFilt(boolean applyingFilt) {
        this.applyingFilt = applyingFilt;
    }

    public WritableImage getDestImage() {
        return dest;
    }

    public void setDestImage(int width, int height) {
        dest = new WritableImage(width, height);
    }
}
