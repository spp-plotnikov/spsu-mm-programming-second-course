import javax.imageio.ImageIO;
import java.awt.image.BufferedImage;
import java.awt.image.BufferedImageOp;
import java.awt.image.ConvolveOp;
import java.awt.image.Kernel;
import java.io.*;
import java.net.Socket;
import java.nio.ByteBuffer;

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

            byte[] size = ByteBuffer.allocate(4).putInt(byteArrayOutputStream.size()).array();
            stream.write(size);
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
            stream.read(sizeAr);
            int size = ByteBuffer.wrap(sizeAr).asIntBuffer().get();

            byte[] imageAr = new byte[size];
            stream.read(imageAr);

            image = ImageIO.read(new ByteArrayInputStream(imageAr));
        } catch (IOException e) {
            e.printStackTrace();
        }
        return image;
    }
}

public class ImageProcessor implements Runnable {
    private Filter[] filters;
    private BufferedImage image;
    protected Socket client;

    public ImageProcessor(Socket s, Filter[] filters) {
        this.client = s;
        this.filters = filters;
    }

    public void run() {
        System.out.println("connection established");
        try {
            InputStream input = client.getInputStream();
            image = new ImageReceiver(input).recv();
        } catch (IOException e) {
            e.printStackTrace();
        }

        Filter filter = filters[0];
        Kernel kernel = new Kernel(filter.h, filter.w, filter.values);
        BufferedImageOp op = new ConvolveOp(kernel);
        BufferedImage res = op.filter(image, null);

        try {
            OutputStream output = client.getOutputStream();
            new ImageSender(res, output).send();
        } catch (IOException e) {
            e.printStackTrace();
        }
        System.out.println("image sent");
    }
}
