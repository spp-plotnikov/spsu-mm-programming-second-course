import java.awt.image.BufferedImage;
import java.io.*;
import java.net.InetAddress;
import java.net.InetSocketAddress;
import java.net.Socket;
import java.util.concurrent.Callable;


public class SimpleClient implements Callable<Long> {
    private InetAddress address;
    private BufferedImage image;
    private int port;

    public SimpleClient(InetAddress address, int port, BufferedImage image) {
        this.address = address;
        this.image = image;
        this.port = port;
    }

    public Long call() {
        long startTime = System.currentTimeMillis();
        try {
            Socket s = new Socket();
            s.connect(new InetSocketAddress(address, port));
            OutputStream output = s.getOutputStream();

            new ImageSender(image, output).send(0);

            InputStream input = s.getInputStream();
            byte[] magicWord = new byte[1];
            for (;;) {
                if (input.read(magicWord) == -1)
                    throw new IOException("Can't read magicWord");
                if (magicWord[0] == (byte) 0xFF)
                    break;
                byte[] cur = new byte[4];
                if (input.read(cur) == -1)
                    throw new IOException("Can't read current progress");
            }

            // get the result and forget it
            new ImageReceiver(input).recv();
        } catch (IOException e) {
            System.out.println("CRASHED: " + e.getMessage());
            return (long) -1;
        }
        return System.currentTimeMillis() - startTime;
    }
}
