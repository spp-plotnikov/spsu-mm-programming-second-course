import javax.imageio.IIOException;
import javax.imageio.ImageIO;
import java.awt.*;
import java.awt.image.BufferedImage;
import java.io.ByteArrayOutputStream;
import java.io.DataOutputStream;
import java.io.File;
import java.io.IOException;
import java.net.Socket;

class Sender {
    Socket socket;
    int x;
    int y;

    Sender(Socket socket, int x, int y) {
        this.x = x;
        this.y = y;
        this.socket = socket;
    }

    void sendImage(String path, String nameFilter) throws IOException {
        BufferedImage image = ImageIO.read(new File(path.toString()));
        image = compressImage(image);
        DataOutputStream dataOutputStream = new DataOutputStream(socket.getOutputStream());

        ByteArrayOutputStream byteArrayOutputStream = new ByteArrayOutputStream();


        try {
            ImageIO.write(image, "jpg", byteArrayOutputStream);
        } catch (IIOException ex) {
            System.out.println("едва ли это был настоящий jpg");
            System.exit(1);
        }

        int size = byteArrayOutputStream.toByteArray().length;
        dataOutputStream.writeByte(0xFE);
        dataOutputStream.writeUTF(nameFilter);
        dataOutputStream.writeInt(size);
        byte[] buffer = byteArrayOutputStream.toByteArray();
        for (int i = 0; i < size; i++) {
            dataOutputStream.writeByte(buffer[i]);
        }
        dataOutputStream.flush();
    }

    BufferedImage compressImage(BufferedImage originalImage) throws IOException {
        BufferedImage scaled = new BufferedImage(x, y, BufferedImage.TYPE_INT_RGB);
        Graphics2D g = scaled.createGraphics();
        g.setRenderingHint(RenderingHints.KEY_INTERPOLATION,
                RenderingHints.VALUE_INTERPOLATION_BICUBIC);
        g.setRenderingHint(RenderingHints.KEY_RENDERING,
                RenderingHints.VALUE_RENDER_QUALITY);
        g.drawImage(originalImage, 0, 0, scaled.getWidth(), scaled.getHeight(), null);
        g.dispose();

        return scaled;
    }
}