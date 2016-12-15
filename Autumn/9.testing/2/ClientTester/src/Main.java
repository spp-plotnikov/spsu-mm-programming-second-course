import java.io.FileNotFoundException;
import java.io.PrintWriter;
import java.util.ArrayList;
import java.util.concurrent.*;

public class Main {
    public static void main(String[] args) throws ExecutionException, InterruptedException, FileNotFoundException {
        ExecutorService es = Executors.newCachedThreadPool();
        PrintWriter pw = new PrintWriter("data");
        ArrayList<Integer> sizeImage = new ArrayList<>();
        ArrayList<Double> time = new ArrayList<>();
        ArrayList<Double> timeMedian = new ArrayList<>();
        ArrayList<Double> timeAvg = new ArrayList<>();
        double t = 0;
        for (int i = 1; i <= 80; i++) {
            int x = i * 50;
            int y = i * 50;
            sizeImage.add(x * y);

            System.out.print(x * y + " ");
            Future<Long> future = es.submit(new Client(x, y));
            t = future.get() / 1000.0;
            time.add(t);
            System.out.print(String.format("%.2f", t) + " ");
            if (time.size() % 2 == 0) {
                t = (time.get(time.size() / 2) + time.get(time.size() / 2 - 1)) / 2;
            } else {
                t = (time.get(time.size() / 2));
            }
            System.out.print(String.format("%.2f", t) + " ");
            timeMedian.add(t);
            t = time.stream().mapToDouble(val -> val).average().getAsDouble();
            System.out.println(String.format("%.2f", t) + " ");
            timeAvg.add(t);
//            pw.flush();
        }
        for (int i = 0; i < sizeImage.size(); i++) {
            pw.println(sizeImage.get(i) + " " + time.get(i) + " " + timeMedian.get(i) + " " + timeAvg.get(i));
        }
        pw.close();
        es.shutdown();
    }
}
