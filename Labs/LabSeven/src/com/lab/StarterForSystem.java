package com.lab;

import java.io.IOException;
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
        ExamSystem examSystem = new ExamSystemImpl();

        ServerSocket serverSocket = new ServerSocket(SERVER_PORT);

        while (flag) {
            client = serverSocket.accept();
            EventHandler eventHandler = new EventHandler(client, examSystem);
            eventHandler.start();
        }
        client.close();
        serverSocket.close();

        // Если запрос поступи на просмотр, то сразу создаём новый поток
        // Если на добавление или удаление, то посылаем в очередь
        // На всех есть одна очередь, первоначально таски помещаются в неё, а потом уже берутся из неё по мере поступления.
        //
    }
}
