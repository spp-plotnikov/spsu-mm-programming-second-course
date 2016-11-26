import java.io.*;
import java.util.ArrayList;
import java.util.Scanner;

public class Main {

    public static void main(String[] args) throws IOException {
        ArrayList<MyFilter> filters = new ArrayList();
        inputCfg(filters);
        Server server = new Server(filters);
        server.start();
    }
    static void inputCfg(ArrayList<MyFilter> filters) throws FileNotFoundException {
        Scanner sc = new Scanner(new File("cfg"));
        while (sc.hasNext()) {
            String name = sc.next();
            int n = sc.nextInt();
            int m = sc.nextInt();
            float[] floats = new float[n * m];
            for (int i = 0; i < n * m; i++) {
                floats[i] = sc.nextFloat();
            }
            filters.add(new MyFilter(name, n, m, floats));
        }
    }
}






