import javax.imageio.ImageIO;
import java.awt.image.BufferedImage;
import java.io.ByteArrayInputStream;
import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.IOException;
import java.net.Socket;
import java.util.ArrayList;

class Receiver {
    Socket socket;

    Receiver(Socket socket) {
        this.socket = socket;
    }
    byte recv() throws IOException {
        DataInputStream dataInputStream = new DataInputStream(socket.getInputStream());

        byte b = dataInputStream.readByte();
        return b;
    }
    BufferedImage receiveImage() throws IOException {

        DataInputStream dataInputStream = new DataInputStream(socket.getInputStream());

        int size = dataInputStream.readInt();
        byte[] imageAr = new byte[size];

        for (int i = 0; i < size; i++) {
            imageAr[i] = dataInputStream.readByte();
        }
        BufferedImage image = ImageIO.read(new ByteArrayInputStream(imageAr));

        return image;
    }

    void recieveFilters(ArrayList<String> strings) throws IOException {
        DataInputStream dataInputStream = new DataInputStream(socket.getInputStream());
        DataOutputStream dataOutputStream = new DataOutputStream(socket.getOutputStream());
        dataOutputStream.writeByte(0xFF);
        byte n = dataInputStream.readByte();
        for (int i = 0; i < n; i++) {
            strings.add(dataInputStream.readUTF().toString());
        }
        dataOutputStream.flush();
    }
}
