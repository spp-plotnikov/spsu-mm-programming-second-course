import java.util.LinkedList;

public class Consumer implements Runnable {
    private static LinkedList<Integer> buffer;
    private volatile Boolean consumerIsRunning = true;
    Consumer(LinkedList<Integer> buffer) {
        this.buffer = buffer;
    }

    public void stop() {
        consumerIsRunning = false;
    }

    @Override
    public void run() {
        while (consumerIsRunning) {
            synchronized (buffer) {
                while (buffer.isEmpty() && consumerIsRunning) {
                    try {
                        buffer.wait();
                    } catch (InterruptedException e) {
                        e.printStackTrace();
                    }
                }

                if (!consumerIsRunning) {
                    break;
                }

                int mine = buffer.removeFirst();

                System.out.println("Consumer " + Thread.currentThread().getId() + " consumed " + mine);

                try {
                    Thread.sleep(2000);
                } catch (InterruptedException e) {
                    e.printStackTrace();
                }
            }
        }
    }
}
