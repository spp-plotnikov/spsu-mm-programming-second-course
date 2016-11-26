import javax.imageio.ImageIO;
import java.awt.image.BufferedImage;
import java.io.ByteArrayOutputStream;
import java.io.DataOutputStream;
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
        ImageIO.write(image, "jpg", byteArrayOutputStream);

        int size = byteArrayOutputStream.toByteArray().length;
        dataOutputStream.writeInt(size);
        dataOutputStream.write(byteArrayOutputStream.toByteArray());
    }

    void sendFilters(ArrayList<MyFilter> filters) throws IOException {
        DataOutputStream dataOutputStream = new DataOutputStream(socket.getOutputStream());

        dataOutputStream.writeByte(filters.size());
        for (int i = 0; i < filters.size(); i++) {
            dataOutputStream.writeUTF(filters.get(i).getNameFilter());
        }
    }
    void sendProgress(byte n) throws IOException {
        DataOutputStream dataOutputStream = new DataOutputStream(socket.getOutputStream());
        dataOutputStream.writeByte(n);
    }
}