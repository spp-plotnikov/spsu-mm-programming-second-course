import javax.imageio.ImageIO;
import java.awt.image.BufferedImage;
import java.io.ByteArrayInputStream;
import java.io.DataInputStream;
import java.io.EOFException;
import java.io.IOException;
import java.net.ServerSocket;
import java.net.Socket;

class Receiver {

    ServerSocket serv;
    Socket socket;

    Receiver(ServerSocket serv, Socket socket) {
        this.serv = serv;
        this.socket = socket;
    }

    byte recv() throws IOException {
        DataInputStream dataInputStream = new DataInputStream(socket.getInputStream());
        try {
            byte b = dataInputStream.readByte();
            return b;
        } catch (EOFException ex) {

        }
        return 0;
    }

    BufferedImage recvImage(StringBuffer nameFilter) throws IOException {
        DataInputStream dataInputStream = new DataInputStream(socket.getInputStream());
        nameFilter.append(dataInputStream.readUTF().toString());
        int size = dataInputStream.readInt();
        byte[] imageAr = new byte[size];
        dataInputStream.read(imageAr);
        ByteArrayInputStream byteArrayInputStream = new ByteArrayInputStream(imageAr);
        BufferedImage image = ImageIO.read(byteArrayInputStream);

        return image;
    }

}