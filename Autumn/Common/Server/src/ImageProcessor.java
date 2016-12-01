import javax.imageio.ImageIO;
import java.awt.*;
import java.awt.image.BufferedImage;
import java.awt.image.BufferedImageOp;
import java.awt.image.ConvolveOp;
import java.awt.image.Kernel;
import java.io.*;
import java.net.Socket;
import java.nio.ByteBuffer;
import java.util.Arrays;

// The following classes are used to transfer images
class ImageSender {
    BufferedImage im;
    OutputStream stream;

    public ImageSender(BufferedImage im, OutputStream stream) {
        this.im = im;
        this.stream = stream;
    }

    public void send() {
        try {
            ByteArrayOutputStream byteArrayOutputStream = new ByteArrayOutputStream();
            ImageIO.write(im, "jpg", byteArrayOutputStream);

            // Send the size of the image in bytes
            int len = byteArrayOutputStream.size(), block_size = 256;
            byte[] size = ByteBuffer.allocate(4).putInt(len).array();
            stream.write(size);

            // Then the image itself
            byte[] bytes = byteArrayOutputStream.toByteArray();
            for (int i = 0; i < len / block_size + 1; i++) {
                byte[] chunk = Arrays.copyOfRange(bytes, i * block_size, (i+1) * block_size);
                stream.write(chunk);
                stream.flush();
            }
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}

class ImageReceiver {
    InputStream stream;

    public ImageReceiver(InputStream stream) {
        this.stream = stream;
    }

    public BufferedImage recv()  {
        byte[] sizeAr = new byte[4];
        BufferedImage image = null;
        try {
            System.out.println("receiving image...");
            // Recv. the image size
            stream.read(sizeAr);
            int size = ByteBuffer.wrap(sizeAr).asIntBuffer().get(), chunk_size = 256;

            // And the image itself
            byte[] imageAr = new byte[size + chunk_size];
            for (int i = 0; i < size / chunk_size + 1; i++) {
                byte[] chunk = new byte[chunk_size];
                stream.read(chunk);
                System.arraycopy(chunk, 0, imageAr, i * chunk_size, chunk_size);
            }
            image = ImageIO.read(new ByteArrayInputStream(Arrays.copyOfRange(imageAr, 0, size)));
            System.out.println("image received");
        } catch (IOException e) {
            e.printStackTrace();
        }
        return image;
    }
}

class ProgressSender {
    private OutputStream os;
    private long last = 0;

    public ProgressSender(OutputStream os) {
        this.os = os;
    }

    public void send(int progress) {
        if (System.currentTimeMillis() - last < 1000)
            return;
        byte magic[] = { 0x01 };
        byte cur[] = ByteBuffer.allocate(4).putInt(progress).array();
        try {
            os.write(magic);
            os.write(cur);
        } catch (IOException e) {
            e.printStackTrace();
        }
        last = System.currentTimeMillis();
    }
}

// This class is used to process single client
public class ImageProcessor implements Runnable {
    private Filter[] filters;
    private BufferedImage image;
    protected Socket client;

    public ImageProcessor(Socket s, Filter[] filters) {
        this.client = s;
        this.filters = filters;
    }

    public void run() {
        int filterNum = -1;
        try {
            InputStream input = client.getInputStream();
            byte[] magicWord = new byte[1];
            input.read(magicWord);
            // 0xFF means "image" (i.e. not filter fetching request)
            if (magicWord[0] == (byte) 0xFF) {
                // first read which filter to use
                byte[] num = new byte[4];
                input.read(num);
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
        } catch (IOException e) {
            e.printStackTrace();
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
            new ImageSender(res, output).send();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}
