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
    int count;
    double minTime;
    double maxTime;
    double avgTime;
    double medTime;
}

public class Main {
    static TestResult getResult(ArrayList<Long> input) {
        Collections.sort(input);

        double median;
        if (input.size() % 2 == 0)
            median = ((double) input.get(input.size() / 2) + (double) input.get(input.size() / 2 - 1)) / 2;
        else
            median = (double) input.get(input.size() / 2);
        Double average = input.stream().mapToDouble(val -> val).average().getAsDouble();
        Double min = input.stream().mapToDouble(val -> val).min().getAsDouble();
        Double max = input.stream().mapToDouble(val -> val).max().getAsDouble();

        TestResult t = new TestResult();
        t.count = input.size();
        t.minTime = min;
        t.maxTime = max;
        t.avgTime = average;
        t.medTime = median;
        return t;
    }

    public static void main(String[] args) throws Exception {
        if (args.length > 1) {
            System.out.println("Wrong arguments, expected <image> or (none)");
            System.exit(1);
        } else if (args.length == 1) { // run test for a single image (controlled by python script)
            BufferedImage image = ImageIO.read(new File(args[0]));
            int count = 50;
            ExecutorService executor = Executors.newCachedThreadPool();
            ArrayList<Future<Long>> pendings = new ArrayList<>(count);
            ArrayList<Long> results = new ArrayList<>(count);
            for (int i = 0; i < count; i++) {
                SimpleClient client = new SimpleClient(InetAddress.getByName("127.0.0.1"), 1424, image);
                pendings.add(executor.submit(client));
            }

            for (Future<Long> cur : pendings)
                results.add(cur.get());
            executor.shutdown();

            TestResult testResult = getResult(results);
            try (Writer writer = new FileWriter(".tmp_res.json")) {
                Gson gson = new GsonBuilder().create();
                gson.toJson(testResult, writer);
            }
            return; // exit
        }

        // otherwise run general stress test
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

            output[count] = getResult(results);
            System.out.println("=== COUNT=" + count + " ===");
            System.out.println("Min: " + output[count].minTime);
            System.out.println("Max: " + output[count].maxTime);
            System.out.println("Average: " + output[count].avgTime);
            System.out.println("Median:  " + output[count].medTime);
        }

        try (Writer writer = new FileWriter("results.json")) {
            Gson gson = new GsonBuilder().create();
            gson.toJson(output, writer);
        }
    }
}
