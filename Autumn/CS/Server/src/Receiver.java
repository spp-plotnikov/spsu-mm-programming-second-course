import javax.imageio.ImageIO;
import java.awt.image.BufferedImage;
import java.io.ByteArrayInputStream;
import java.io.DataInputStream;
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
        byte b = dataInputStream.readByte();
        return b;
    }

    BufferedImage recvImage(StringBuffer nameFilter) throws IOException {
        DataInputStream dataInputStream = new DataInputStream(socket.getInputStream());

        int size = dataInputStream.readInt();
        byte[] imageAr = new byte[size];
        dataInputStream.read(imageAr);
        BufferedImage image = ImageIO.read(new ByteArrayInputStream(imageAr));
        nameFilter.append(dataInputStream.readUTF().toString());

        return image;
    }

}