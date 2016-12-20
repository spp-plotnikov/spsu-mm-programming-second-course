import java.util.LinkedList;
import java.util.Random;

public class Producer implements Runnable {
    private LinkedList<Integer> buffer;
    private volatile Boolean producerIsRunning = true;

    Producer(LinkedList<Integer> buffer) {
        this.buffer = buffer;
    }

    public void stop() {
        producerIsRunning = false;
    }

    @Override
    public void run() {
        while (producerIsRunning) {
            Random random = new Random();
            int i = random.nextInt(100);
            synchronized (buffer) {
                buffer.add(i);
                buffer.notifyAll();
            }

            System.out.println("Producer " + Thread.currentThread().getId() + " produced " + i);

            try {
                Thread.sleep(2000);
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
        }
    }
}
