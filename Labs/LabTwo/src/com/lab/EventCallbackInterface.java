package com.lab;

import com.sun.jna.Callback;

/**
 * Created by Katrin on 17.06.2016.
 */
public interface EventCallbackInterface extends Callback {
    int callback(int param) throws InterruptedException;
}

