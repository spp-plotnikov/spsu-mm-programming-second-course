import com.google.gson.Gson;
import com.google.gson.GsonBuilder;

import javax.imageio.ImageIO;
import java.awt.image.BufferedImage;
import java.io.File;
import java.io.FileWriter;
import java.io.Writer;
import java.net.InetAddress;
import java.util.ArrayList;
import java.util.Collections;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import java.util.concurrent.Future;

class TestResult {
    public int count;
    public double minTime;
    public double maxTime;
    public double avgTime;
    public double medTime;
}

public class Main {

    public static void main(String[] args) throws Exception {
        BufferedImage image = ImageIO.read(new File("test/test.jpg"));
        TestResult[] output = new TestResult[200];

        for (int count = 1; count < 200; count++) {
            ExecutorService executor = Executors.newCachedThreadPool();
            ArrayList<Future<Long>> pendings = new ArrayList<>(count);
            ArrayList<Long> results = new ArrayList<>(count);
            for (int i = 0; i < count; i++) {
                SimpleClient client = new SimpleClient(InetAddress.getByName("127.0.0.1"), 1424, image);
                pendings.add(executor.submit(client));
            }

            boolean failed = false;
            for (Future<Long> cur : pendings) {
                Long val = cur.get();
                if (val == -1) {
                    failed = true;
                    break;
                }
                results.add(val);
            }
            executor.shutdown();
            if (failed) {
                System.out.println("!!! FAILED AT " + count + " !!!");
                break;
            }

            Collections.sort(results);

            double median;
            if (count % 2 == 0)
                median = ((double) results.get(count / 2) + (double) results.get(count / 2 - 1)) / 2;
            else
                median = (double) results.get(count / 2);
            Double average = results.stream().mapToDouble(val -> val).average().getAsDouble();
            Double min = results.stream().mapToDouble(val -> val).min().getAsDouble();
            Double max = results.stream().mapToDouble(val -> val).max().getAsDouble();

            System.out.println("=== COUNT=" + count + " ===");
            System.out.println("Min: " + min);
            System.out.println("Max: " + average);
            System.out.println("Average: " + average);
            System.out.println("Median:  " + median);

            TestResult t = new TestResult();
            t.count = count;
            t.minTime = min;
            t.maxTime = max;
            t.avgTime = average;
            t.medTime = median;
            output[count] = t;
        }

        try (Writer writer = new FileWriter("results.json")) {
            Gson gson = new GsonBuilder().create();
            gson.toJson(output, writer);
        }
    }
}
