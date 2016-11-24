import javax.imageio.ImageIO;
import java.awt.image.BufferedImage;
import java.io.InputStream;
import java.io.*;
import java.nio.ByteBuffer;

class ImageSender {
    BufferedImage im;
    OutputStream stream;

    public ImageSender(BufferedImage im, OutputStream stream) {
        this.im = im;
        this.stream = stream;
    }

    public void send(int num) {
        try {
            ByteArrayOutputStream byteArrayOutputStream = new ByteArrayOutputStream();
            ImageIO.write(im, "jpg", byteArrayOutputStream);
            byte[] magicWord = { 0x01 }; // means "I want to send an image, not the filter request"
            byte[] filterNum = ByteBuffer.allocate(4).putInt(num).array();
            byte[] size = ByteBuffer.allocate(4).putInt(byteArrayOutputStream.size()).array();
            stream.write(magicWord);
            stream.write(filterNum);
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
        MainWindow window = new MainWindow();
        window.run();
    }
}
