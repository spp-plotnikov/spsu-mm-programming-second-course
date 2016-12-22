import java.awt.image.BufferedImage;
import java.util.Arrays;


public class Test {
    private TestingSystem ts;
    private int clientsCount;
    private BufferedImage imageSrc;
    private int maxTimeout;
    private long[] results;
    private int resultsCount;
    private Boolean waitingForResults;
    private int reasonFailed;

    Test(TestingSystem ts, int clientsCount, BufferedImage imageSrc, int maxTimeout) {
        this.ts = ts;
        this.clientsCount = clientsCount;
        this.imageSrc = imageSrc;
        this.maxTimeout = maxTimeout;
        this.waitingForResults = true;
        this.reasonFailed = 0;

        this.results = new long[clientsCount];
    }

    public BufferedImage getImageSrc() {
        return imageSrc;
    }

    public void start() {
        for (int i = 0; i < clientsCount; i++) {
            new Thread(new Client(this, i, maxTimeout)).start();
        }

        while (waitingForResults) {
            try {
                Thread.sleep(1500);
            } catch (InterruptedException e) { }
        }

        if (reasonFailed == 0) {
            ts.testSucceed(this);
        } else if (reasonFailed == 1) {
            ts.testFailed(this, "Time out");
        } else if (reasonFailed == 2) {
            ts.testFailed(this, "Error occured");
        }
    }

    public long getMedianTime() {
        Arrays.sort(results);
        return results[results.length/2];
    }

    public long getMinTime() {
        long min = results[0];
        for (int i = 1; i < results.length; i++) {
            if (results[i] < min) {
                min = results[i];
            }
        }
        return min;

    }

    public long getMaxTime() {
        long max = results[0];
        for (int i = 1; i < results.length; i++) {
            if (results[i] > max) {
                max = results[i];
            }
        }
        return max;

    }

    private long getSum() {
        long sum = 0;
        for (long i : results) {
            sum += i;
        }
        return sum;
    }

    public long getAverageTime() {
        return getSum()/results.length;
    }


    public void addResult(int id, long duration) {
        synchronized (results) {
            results[id] = duration;
            resultsCount++;
        }

        if (resultsCount == clientsCount) {
            waitingForResults = false;
        }
    }

    public void noResult(int reasonFailed) {
        this.reasonFailed = reasonFailed;
        waitingForResults = false;
    }
}

