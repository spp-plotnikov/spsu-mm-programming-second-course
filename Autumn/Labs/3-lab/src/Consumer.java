import java.util.LinkedList;

public class Consumer implements Runnable {
    private static LinkedList<Integer> buffer;
    private volatile Boolean consumerIsRunning = true;

    Consumer(LinkedList<Integer> buffer) {
        this.buffer = buffer;
    }

    public void stop() {
        consumerIsRunning = false;
        synchronized (buffer) {
            buffer.notifyAll();
        }
    }

    @Override
    public void run() {
        while (consumerIsRunning) {
            int mine;
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

                mine = buffer.removeFirst();
            }

            if (consumerIsRunning) {
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
