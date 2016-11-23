import javax.imageio.ImageIO;
import java.awt.image.BufferedImage;
import java.io.File;
import java.io.InputStream;
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

public class Main {
    public static void main(String[] args) throws Exception {
        File fr = new File("/tmp/test.jpg");
        BufferedImage im = ImageIO.read(fr);

        Socket s = new Socket("127.0.0.1", 1424);
        OutputStream output = s.getOutputStream();
        new ImageSender(im, output).send();

        InputStream input = s.getInputStream();
        BufferedImage res = new ImageReceiver(input).recv();

        File fw = new File("/tmp/out1.jpg");
        ImageIO.write(res, "jpeg", fw);
    }
}
