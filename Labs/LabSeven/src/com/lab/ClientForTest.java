package com.lab;

import java.io.*;
import java.net.Socket;

/**
 * Created by Katrin on 28.09.2016.
 */
public class ClientForTest extends Thread {

    private String flag;
    private Socket socket;

    public ClientForTest(String flag, String port) throws IOException {
        socket = new Socket("127.0.0.1", Integer.parseInt(port));
        this.flag = flag;
    }

    @Override
    public void run(){
        try {
            BufferedWriter clientOut = new BufferedWriter(new OutputStreamWriter(socket.getOutputStream(), "UTF-8"));
            clientOut.write(flag);
            clientOut.write("\n");
            clientOut.flush();

            BufferedReader clientIn = new BufferedReader(new InputStreamReader(socket.getInputStream(), "UTF-8"));
            clientIn.readLine();

            clientOut.write("end");
            clientOut.write("\n");
            clientOut.flush();

            clientIn.readLine();

            socket.close();
        } catch (IOException e) {
            e.printStackTrace();
        }


    }
}
