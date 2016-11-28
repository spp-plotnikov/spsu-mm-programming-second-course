import javax.imageio.IIOException;
import javax.imageio.ImageIO;
import java.awt.image.BufferedImage;
import java.io.ByteArrayOutputStream;
import java.io.DataOutputStream;
import java.io.File;
import java.io.IOException;
import java.net.ServerSocket;
import java.net.Socket;
import java.util.ArrayList;

class Sender {
    ServerSocket serv;
    Socket socket;

    Sender(ServerSocket serv, Socket socket) {
        this.serv = serv;
        this.socket = socket;
    }

    void sendImage(BufferedImage image) throws IOException {
        DataOutputStream dataOutputStream = new DataOutputStream(socket.getOutputStream());

        ByteArrayOutputStream byteArrayOutputStream = new ByteArrayOutputStream();
        try {
            ImageIO.write(image, "jpg", byteArrayOutputStream);
        } catch (IIOException ex) {
            System.out.println("babax");
            System.exit(5);
        }

        int size = byteArrayOutputStream.toByteArray().length;
        dataOutputStream.writeInt(size);
        byte[] buffer = byteArrayOutputStream.toByteArray();
        for (int i = 0; i < size; i++) {
            dataOutputStream.writeByte(buffer[i]);
        }

        dataOutputStream.flush();
    }

    void sendFilters(ArrayList<MyFilter> filters) throws IOException {
        DataOutputStream dataOutputStream = new DataOutputStream(socket.getOutputStream());

        dataOutputStream.writeByte(filters.size());
        for (int i = 0; i < filters.size(); i++) {
            dataOutputStream.writeUTF(filters.get(i).getNameFilter());
        }
        dataOutputStream.flush();
    }
    void sendProgress(byte n) throws IOException {
        DataOutputStream dataOutputStream = new DataOutputStream(socket.getOutputStream());
        dataOutputStream.writeByte(n);
        dataOutputStream.flush();
    }
}