import java.net.URI;
import java.net.URISyntaxException;

public class Client implements Runnable {
    private int id;
    private int maxTimeout;
    private long timeStart;
    private Test test;
    private Thread mainThread;

    Client(Test test, int id, int maxTimeout) {
        this.test = test;
        this.id = id;
        this.maxTimeout = maxTimeout;
    }

    public int getId() {
        return id;
    }

    public Test getTest() {
        return test;
    }

    public void timeStart() {
        timeStart = System.currentTimeMillis();
    }

    public void timeStop() {
        finish(System.currentTimeMillis() - timeStart);
    }

    private void finish(long duration) {
        mainThread.interrupt();
        test.addResult(id, duration);
    }

    public void errorOccured() {
        mainThread.interrupt();
        test.noResult(2);
    }

    private void timeOut() {
        test.noResult(1);
    }

    @Override
    public void run() {
        try {
            URI uri = new URI("ws://localhost:8081/");
            new WebSocketClientEndpoint(uri, this);

            mainThread = Thread.currentThread();
            mainThread.sleep(maxTimeout);

            timeOut();
        } catch (URISyntaxException ex) {
            errorOccured();
            System.err.println("URISyntaxException exception: " + ex.getMessage());
        } catch (InterruptedException e) { }
    }
}
