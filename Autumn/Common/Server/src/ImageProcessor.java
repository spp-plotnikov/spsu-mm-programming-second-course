import javax.imageio.ImageIO;
import java.awt.image.BufferedImage;
import java.awt.image.BufferedImageOp;
import java.awt.image.ConvolveOp;
import java.awt.image.Kernel;
import java.io.*;
import java.net.Socket;
import java.nio.ByteBuffer;

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
            byte[] size = ByteBuffer.allocate(4).putInt(byteArrayOutputStream.size()).array();
            stream.write(size);

            // Then the image itself
            stream.write(byteArrayOutputStream.toByteArray());
            stream.flush();
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
            // Recv. the image size
            stream.read(sizeAr);
            int size = ByteBuffer.wrap(sizeAr).asIntBuffer().get();

            // And the image itself
            byte[] imageAr = new byte[size];
            stream.read(imageAr);
            image = ImageIO.read(new ByteArrayInputStream(imageAr));
        } catch (IOException e) {
            e.printStackTrace();
        }
        return image;
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
            }
            else { // unknown magic number
                System.err.println("protocol violation, disconnecting client...");
                return;
            }
        } catch (IOException e) {
            e.printStackTrace();
        }

        if (filterNum >= filters.length || filterNum < 0) {
            System.err.println("Got wrong filter number from client, disconnecting...");
            return;
        }

        // Process the image
        Filter filter = filters[filterNum];
        Kernel kernel = new Kernel(filter.h, filter.w, filter.values);
        BufferedImageOp op = new ConvolveOp(kernel);
        BufferedImage res = op.filter(image, null);

        // Send the image back
        try {
            OutputStream output = client.getOutputStream();
            byte magicWord[] = { (byte) 0xFF }; // means "image"
            output.write(magicWord);
            new ImageSender(res, output).send();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}
