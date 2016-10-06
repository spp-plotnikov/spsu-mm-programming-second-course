package com.lab;

import java.io.*;
import java.net.ServerSocket;
import java.net.Socket;

/**
 * Created by Katrin on 24.09.2016.
 */
public class StarterForSystem {
    private static boolean flag = true;
    private static ExamSystem examSystem;
    private static int port;

    public static void main(String[] args) throws IOException {
        port = Integer.parseInt(args[1]);
        if (args[0].equals("one")) {
            examSystem = new ExamSystemImplOne();
        } else {
            examSystem = new ExamSystemImplTwo();
        }

        final ServerSocket serverSocket = new ServerSocket(port);

        /*new Thread(new Runnable() {
            @Override
            public void run() {
                BufferedReader reader = new BufferedReader(new InputStreamReader(System.in));
                try {
                    if (reader.readLine().equals("shutdown")) {
                        StarterForSystem.flag = false;
                        Socket socket = new Socket("127.0.0.1", SERVER_PORT);

                        BufferedWriter clientOut = new BufferedWriter(new OutputStreamWriter(socket.getOutputStream(), "UTF-8"));
                        clientOut.write("end");
                        clientOut.write("\n");
                        clientOut.flush();

                        BufferedReader clientIn = new BufferedReader(new InputStreamReader(socket.getInputStream(), "UTF-8"));
                        clientIn.readLine();

                        socket.close();
                    }
                } catch (IOException e) {
                    e.printStackTrace();
                }
            }
        }).start();*/

        while (flag) {
            Socket client = serverSocket.accept();
            EventHandler eventHandler = new EventHandler(client, examSystem);
            eventHandler.start();
        }

        serverSocket.close();
    }

    public static void close() {
        try {
            StarterForSystem.flag = false;
            Socket socket = new Socket("127.0.0.1", port);

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
