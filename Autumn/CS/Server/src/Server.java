import java.awt.image.BufferedImage;
import java.io.IOException;
import java.io.UTFDataFormatException;
import java.net.ServerSocket;
import java.net.Socket;
import java.net.SocketException;
import java.util.ArrayList;

public class Server {
    ServerSocket serv;
    ArrayList<MyFilter> filters;
    public Server(ArrayList<MyFilter> filters) {
        this.filters = new ArrayList();
        for (MyFilter filter : filters) {
            String newString = new String(filter.getNameFilter());
            float[] newMatrix = new float[filter.getCountColumn() * filter.getCountLine()];
            float[] matrix = filter.getMatrix();
            for (int i = 0; i < matrix.length; i++) {
                newMatrix[i] = matrix[i];
            }
            this.filters.add(new MyFilter(newString, filter.getCountLine(), filter.getCountColumn(), newMatrix));
        }
    }
    void start() throws IOException {
        serv = new ServerSocket(2500);
        while (true) {
            Socket socket = serv.accept();
            new Thread(new ClientHelper(socket)).start();
        }
    }
    class ClientHelper implements Runnable {

        Socket socket;

        ClientHelper(Socket socket) {
            this.socket = socket;
        }

        public void run() {
            try {
                Sender sender = new Sender(serv, socket);
                Receiver receiver = new Receiver(serv, socket);
                byte b = receiver.recv();
                if (b == -1) {
                    new Sender(serv, socket).sendFilters(filters);
                } else if (b == -2) {
                    StringBuffer nameFilter = new StringBuffer("");
                    BufferedImage image = null;
                    try {
                        image = new Receiver(serv, socket).recvImage(nameFilter);
                    } catch (UTFDataFormatException ex) {
                        System.out.println("Ооопс... потеря данных или при приеме или при отправке");
                    }
                    for (int i = 0; i < filters.size(); i++) {
                        if (filters.get(i).getNameFilter().equalsIgnoreCase(nameFilter.toString())) {
                            image = filters.get(i).processImage(image, sender);
                            break;
                        }
                    }
                    sender.sendImage(image);
                }
            } catch (SocketException ex) {

            } catch (IOException ex) {
                System.out.println(ex);
            }
        }
    }
}