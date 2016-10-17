package com.lab.system;

import java.io.*;
import java.net.ServerSocket;
import java.net.Socket;

import static com.lab.additionalComponents.linkedList.Constants.SERVER_PORT;

/**
 * Created by Katrin on 24.09.2016.
 */
public class StarterForSystem {
    private static boolean flag;

    public static void main(String[] args) throws IOException {

        flag = true;
        ExamSystem examSystem;

        if (args[0].equals("one")) {
            examSystem = new ExamSystemImplOne();
        } else {
            examSystem = new ExamSystemImplTwo();
        }

        ServerSocket serverSocket = new ServerSocket(SERVER_PORT);

        //createHelperForClosingSystem();

        while (flag) {
            Socket client = serverSocket.accept();
            EventHandler eventHandler = new EventHandler(client, examSystem);
            eventHandler.start();
        }

        serverSocket.close();
    }

    private static void createHelperForClosingSystem(){
         new Thread(new Runnable() {
            @Override
            public void run() {
                BufferedReader reader = new BufferedReader(new InputStreamReader(System.in));
                try {
                    if (reader.readLine().equals("shutdown")) {
                        close();
                    }
                } catch (IOException e) {
                    e.printStackTrace();
                }
            }
        }).start();

    }

    public static void close() {
        try {
            StarterForSystem.flag = false;
            Socket socket = new Socket("127.0.0.1", SERVER_PORT);

            BufferedWriter clientOut = new BufferedWriter(new OutputStreamWriter(socket.getOutputStream(), "UTF-8"));
            clientOut.write("end");
            clientOut.write("\n");
            clientOut.flush();

            BufferedReader clientIn = new BufferedReader(new InputStreamReader(socket.getInputStream(), "UTF-8"));
            clientIn.readLine();

            socket.close();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}
