package com.lab;

import java.io.*;
import java.net.Socket;

import static com.lab.Constants.*;

/**
 * Created by Katrin on 28.09.2016.
 */
public class EventHandler extends Thread {

    private Socket socket;
    private ExamSystem examSystem;
    private BufferedReader serverIn;
    private BufferedWriter serverOut;
    private String line;
    private String[] params;

    public EventHandler(Socket socket, ExamSystem examSystem) {
        this.socket = socket;
        this.examSystem = examSystem;
    }

    public void run() {

        try {
            serverIn = new BufferedReader(new InputStreamReader(socket.getInputStream(), "UTF-8"));
            serverOut = new BufferedWriter(new OutputStreamWriter(socket.getOutputStream(), "UTF-8"));
            line = serverIn.readLine();
        } catch (IOException e) {
            e.printStackTrace();
        }

        try {
            while (!line.equals("end")) {
                params = line.split(" ");
                if (params[0].equals(CONTAINS) && params.length == 3) {
                    serverOut.write("" + examSystem.contains(Long.parseLong(params[1]), Long.parseLong(params[2])));
                    serverOut.write("\n");
                    serverOut.flush();
                } else if (params[0].equals(ADD) && params.length == 4) {
                    examSystem.add(Long.parseLong(params[1]), Long.parseLong(params[2]), params[3]);
                    serverOut.write("added..");
                    serverOut.write("\n");
                    serverOut.flush();
                } else if (params[0].equals(REMOVE) && params.length == 3) {
                    examSystem.remove(Long.parseLong(params[1]), Long.parseLong(params[2]));
                    serverOut.write("remove..");
                    serverOut.write("\n");
                    serverOut.flush();
                } else {
                    serverOut.write("wrong command..");
                    serverOut.write("\n");
                    serverOut.flush();
                }
                line = serverIn.readLine();
            }
            close();

        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    private void close() throws IOException {
        serverOut.write("end");
        serverOut.flush();
        serverOut.close();
        serverIn.close();
    }
}
