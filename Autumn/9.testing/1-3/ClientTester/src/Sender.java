import javax.imageio.IIOException;
import javax.imageio.ImageIO;
import java.awt.image.BufferedImage;
import java.io.ByteArrayOutputStream;
import java.io.DataOutputStream;
import java.io.File;
import java.io.IOException;
import java.net.Socket;

class Sender {
    Socket socket;

    Sender(Socket socket) {
        this.socket = socket;
    }

    void sendImage(String path, String nameFilter) throws IOException {
        BufferedImage image = ImageIO.read(new File(path.toString()));

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
}