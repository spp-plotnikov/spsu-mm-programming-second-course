import java.awt.image.BufferedImage;
import java.awt.image.BufferedImageOp;
import java.awt.image.Kernel;
import java.io.*;
import java.net.Socket;
import java.net.SocketException;
import java.nio.ByteBuffer;


class ProgressSender {
    private DataOutputStream os;
    private long last = 0;

    ProgressSender(OutputStream os) {
        this.os = new DataOutputStream(os);
    }

    void send(int progress) throws IOException {
        if (System.currentTimeMillis() - last < 1000)
            return;
        byte magic[] = { 0x01 };
        byte cur[] = ByteBuffer.allocate(4).putInt(progress).array();

        os.write(magic);
        os.write(cur);

        last = System.currentTimeMillis();
    }
}

// This class is used to process single client
public class ImageProcessor implements Runnable {
    private Filter[] filters;
    private Socket client;

    public ImageProcessor(Socket s, Filter[] filters) {
        this.client = s;
        this.filters = filters;
    }

    public void run() {
        int filterNum;
        BufferedImage image;
        try {
            InputStream input = client.getInputStream();
            byte[] magicWord = new byte[1];
            if (input.read(magicWord) == -1)
                throw new IOException("Can't read magicWord");

            // 0xFF means "image" (i.e. not filter fetching request)
            if (magicWord[0] == (byte) 0xFF) {
                // first read which filter to use
                byte[] num = new byte[4];
                if (input.read(num) == -1)
                    throw new IOException("Can't read filter number");
                filterNum = ByteBuffer.wrap(num).getInt();

                // read image
                image = new ImageReceiver(input).recv();
            }
            else if (magicWord[0] == (byte) 0xFA ) { // FA stands for Filter Array ;)
                DataOutputStream output = new DataOutputStream(client.getOutputStream());

                // sending the filters count
                ByteBuffer buf = ByteBuffer.allocate(4);
                buf.putInt(filters.length);
                byte[] len = buf.array();
                output.write(len);

                // sending filter names
                for (Filter f: filters)
                    output.writeUTF(f.name);
                return;
            }
            else { // unknown magic number
                System.err.println("protocol violation, disconnecting client...");
                return;
            }
        } catch (Exception e) {
            System.err.println("Exception while recv-ing image!");
            return;
        }

        if (filterNum >= filters.length || filterNum < 0) {
            System.err.println("Got wrong filter number (" + filterNum + ") from client, disconnecting...");
            return;
        }

        // Send the image back
        try {
            OutputStream output = client.getOutputStream();

            Filter filter = filters[filterNum];
            Kernel kernel = new Kernel(filter.h, filter.w, filter.values);
            BufferedImageOp op = new MyConvolveOp(kernel, new ProgressSender(output));
            BufferedImage res = op.filter(image, null);

            byte magicWord[] = { (byte) 0xFF }; // means "image"
            output.write(magicWord);
            new ImageSender(res, output).send(-1);
        } catch (Exception e) {
            System.err.println("Exception while processing image!");
        }
    }
}
