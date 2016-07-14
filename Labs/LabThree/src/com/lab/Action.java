package com.lab;

import com.sun.jna.Callback;

/**
 * Created by Katrin on 16.06.2016.
 */
public interface Action extends Callback {
    void callback() throws InterruptedException;
}
