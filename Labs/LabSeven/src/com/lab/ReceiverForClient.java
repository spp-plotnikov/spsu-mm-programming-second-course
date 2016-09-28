package com.lab;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.net.Socket;

/**
 * Created by Katrin on 28.09.2016.
 */
public class ReceiverForClient extends  Thread {

    private Socket socket;
    private boolean flag = true;

    public void setFlag(boolean flag) {
        this.flag = flag;
    }

    public ReceiverForClient(Socket socket){
        this.socket = socket;
    }

    public void run(){
        while(flag){
            try {
                BufferedReader clientIn = new BufferedReader(new InputStreamReader(socket.getInputStream(), "UTF-8"));
                String line = clientIn.readLine();
                System.out.println("System: " + line);
            } catch (IOException e) {
                e.printStackTrace();
            }
        }

    }
}
