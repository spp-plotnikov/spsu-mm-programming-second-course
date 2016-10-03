package com.lab;

import java.io.*;
import java.net.Socket;

import static com.lab.Constants.SERVER_PORT;

/**
 * Created by Katrin on 28.09.2016.
 */
public class Client {

    public static void main(String[] args) throws IOException {
        BufferedReader br = new BufferedReader(new InputStreamReader(System.in));

        Socket socket = new Socket("127.0.0.1", SERVER_PORT);
        ReceiverForClient receiver = new ReceiverForClient(socket);
        receiver.start();

        System.out.println("Введите данные в таком формате:");
        System.out.println("<Вид услуги> <идентификатор студента> <курс> " +
                "(если добавляете, то через пробел ещё и зачёт/незачёт");

        String line = "start";
        BufferedWriter clientOut = new BufferedWriter(new OutputStreamWriter(socket.getOutputStream(), "UTF-8"));

        while (!line.equals("end")) {
            line = br.readLine();
            clientOut.write(line);
            clientOut.write("\n");
            clientOut.flush();
        }
        receiver.setFlag(false);
    }
}
