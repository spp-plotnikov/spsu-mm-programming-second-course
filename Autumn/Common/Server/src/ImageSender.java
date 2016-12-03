import javax.imageio.ImageIO;
import java.awt.image.BufferedImage;
import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.io.OutputStream;
import java.nio.ByteBuffer;
import java.util.Arrays;


public class ImageSender {
    private BufferedImage im;
    OutputStream stream;

    public ImageSender(BufferedImage im, OutputStream stream) {
        this.im = im;
        this.stream = stream;
    }

    public void send(int num) throws IOException {
        ByteArrayOutputStream byteArrayOutputStream = new ByteArrayOutputStream();
        ImageIO.write(im, "jpg", byteArrayOutputStream);

        if (num != -1) { // -1 means it's the server who uses this class
            byte[] magicWord = {(byte) 0xFF}; // means "I want to send an image, not the filter request"
            byte[] filterNum = ByteBuffer.allocate(4).putInt(num).array();
            stream.write(magicWord);
            stream.write(filterNum);
            stream.flush();
        }

        int len = byteArrayOutputStream.size(), block_size = 256;
        byte[] size = ByteBuffer.allocate(4).putInt(len).array();
        stream.write(size);

        byte[] bytes = byteArrayOutputStream.toByteArray();
        for (int i = 0; i < len / block_size + 1; i++) {
            byte[] chunk = Arrays.copyOfRange(bytes, i * block_size, (i+1) * block_size);
            stream.write(chunk);
            stream.flush();
        }
    }
}
