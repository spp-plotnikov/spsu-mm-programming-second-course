import javax.imageio.ImageIO;
import java.awt.image.BufferedImage;
import java.io.ByteArrayInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.nio.ByteBuffer;
import java.util.Arrays;


public class ImageReceiver {
    InputStream stream;

    public ImageReceiver(InputStream stream) {
        this.stream = stream;
    }

    public BufferedImage recv() throws IOException {
        byte[] sizeAr = new byte[4];
        BufferedImage image;

        System.out.println("receiving image...");
        // Recv. the image size
        if (stream.read(sizeAr) == -1)
            throw new IOException("Can't read the image size");
        int size = ByteBuffer.wrap(sizeAr).asIntBuffer().get(), chunk_size = 256;

        // And the image itself
        byte[] imageAr = new byte[size + chunk_size];
        for (int i = 0; i < size / chunk_size + 1; i++) {
            byte[] chunk = new byte[chunk_size];
            if (stream.read(chunk) == -1)
                throw new IOException("Can't read the next chunk");
            System.arraycopy(chunk, 0, imageAr, i * chunk_size, chunk_size);
        }
        image = ImageIO.read(new ByteArrayInputStream(Arrays.copyOfRange(imageAr, 0, size)));
        System.out.println("image received");

        return image;
    }
}
