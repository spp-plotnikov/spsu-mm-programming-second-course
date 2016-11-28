import javax.imageio.IIOException;
import javax.imageio.ImageIO;
import java.awt.image.BufferedImage;
import java.io.*;
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
        dataOutputStream.writeByte(0xFE);
        dataOutputStream.writeUTF(nameFilter);
        dataOutputStream.writeInt(byteArrayOutputStream.toByteArray().length);
        dataOutputStream.write(byteArrayOutputStream.toByteArray());
        dataOutputStream.flush();
    }
}