import javax.imageio.ImageIO;
import java.awt.image.BufferedImage;
import java.io.InputStream;
import java.io.*;
import java.nio.ByteBuffer;
import java.util.Arrays;

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
            byte[] magicWord = { (byte) 0xFF }; // means "I want to send an image, not the filter request"
            byte[] filterNum = ByteBuffer.allocate(4).putInt(num).array();
            stream.write(magicWord);
            stream.write(filterNum);

            int len = byteArrayOutputStream.size(), block_size = 256;
            byte[] size = ByteBuffer.allocate(4).putInt(len).array();
            stream.write(size);

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
        System.out.println("receiving image...");
        try {
            stream.read(sizeAr);
            int size = ByteBuffer.wrap(sizeAr).asIntBuffer().get();
            int chunk_size = 256;

            // And the image itself
            byte[] imageAr = new byte[size + chunk_size];
            for (int i = 0; i < size / chunk_size + 1; i++) {
                byte[] chunk = new byte[chunk_size];
                stream.read(chunk);
                System.arraycopy(chunk, 0, imageAr, i * chunk_size, chunk_size);
            }
            image = ImageIO.read(new ByteArrayInputStream(Arrays.copyOfRange(imageAr, 0, size)));
        } catch (IOException e) {
            e.printStackTrace();
        }
        System.out.println("image received!");
        return image;
    }
}

public class Main {
    public static void main(String[] args) throws Exception {
        MainWindow window = new MainWindow();
        window.run();
    }
}
