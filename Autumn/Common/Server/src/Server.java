import java.io.FileNotFoundException;
import java.io.FileReader;
import java.io.IOException;
import java.net.ServerSocket;
import java.net.Socket;

import com.google.gson.Gson;

public class Server implements Runnable {
    String[] args;
    Filter[] filters;
    protected int serverPort = 1424;
    protected ServerSocket serverSocket = null;
    protected boolean isStopped = false;

    public Server(String[] args) {
        this.args = args;
    }

    // Parse the config and so on...
    private void init() {
        if (args.length != 1) {
            System.err.println("Usage: java -jar server.jar config.json");
            System.exit(1);
        }
        System.out.println("Server is starting...");
        FileReader config = null;
        try {
            config = new FileReader(args[0]);
        } catch (FileNotFoundException e) {
            System.err.println("Config file " + args[0] + " not found!");
            System.exit(1);
        }

        Gson gson = new Gson();
        filters = gson.fromJson(config, Filter[].class);
        System.out.println("Loaded " + filters.length + " filters");

        try {
            serverSocket = new ServerSocket(serverPort);
        } catch (IOException e) {
            System.err.println("Unable to open server socket");
        }
        System.out.println("Server is running on " + serverPort);
    }

    public void run() {
        init();
        while (!isStopped()) {
            Socket client = null;
            try {
                // for each client we got...
                client = serverSocket.accept();
            } catch (IOException e) {
                if (isStopped())
                    return;
            }
            // ... start the new processing thread
            new Thread(new ImageProcessor(client, filters)).start();
        }
    }

    public synchronized boolean isStopped() {
        return isStopped;
    }

    public synchronized void stop() {
        isStopped = true;
        try {
            serverSocket.close();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

}
