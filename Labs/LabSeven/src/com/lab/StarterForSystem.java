package com.lab;

import java.io.*;
import java.net.ServerSocket;
import java.net.Socket;

import static com.lab.Constants.SERVER_PORT;

/**
 * Created by Katrin on 24.09.2016.
 */
public class StarterForSystem {
    public static boolean flag = true;
    private static Socket client;

    public static void main(String[] args) throws IOException {
        ExamSystem examSystem = new ExamSystemImplTwo();

        final ServerSocket serverSocket = new ServerSocket(SERVER_PORT);

        while (flag) {
            client = serverSocket.accept();
            EventHandler eventHandler = new EventHandler(client, examSystem);
            eventHandler.start();
        }

        client.close();
        serverSocket.close();
    }
}
