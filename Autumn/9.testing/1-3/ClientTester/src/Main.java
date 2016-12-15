import java.io.FileNotFoundException;
import java.io.PrintWriter;
import java.util.ArrayList;
import java.util.concurrent.*;

public class Main {
    public static void main(String[] args) throws ExecutionException, InterruptedException, FileNotFoundException {
        ExecutorService es = Executors.newCachedThreadPool();
        int countClient = 1000;
        PrintWriter pw = new PrintWriter("data");
        double sum = 0;
        for (int i = 1; i < countClient; i++) {
            ArrayList<Future<Long>> ans = new ArrayList<>();
            for (int j = 0; j < i; j++) {
                ans.add(es.submit(new Client()));
            }

            for (int j = 0; j < i; j++) {
                sum += ans.get(i - 1).get() / 1000.0;
            }
            sum /= i;
            System.out.println(i);
            pw.println(i + " " + String.format("%.2f", sum));
            pw.flush();
        }
//        pw.close();
        es.shutdown();
    }
}
