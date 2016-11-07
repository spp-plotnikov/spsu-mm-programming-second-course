import java.util.Iterator;
import java.util.List;
import java.util.Queue;
import java.util.LinkedList;

public class MyThreadPool extends Thread {
    public static int threadMax = 2;
    private Queue<Runnable> pending;
    private List<Thread> running;
    public volatile boolean isStoped = true;

    public MyThreadPool() {
        running = new LinkedList<>();
        pending = new LinkedList<>();
    }

    public void enqueue(Runnable obj) {
        synchronized (pending) {
            pending.add(obj);
        }
    }

    public void run() {
        isStoped = false;
        // control loop
        while (!isStoped) {
            // check if any of running threads is done
            Iterator<Thread> it = running.iterator();
            while (it.hasNext()) {
                Thread cur = it.next();
                if (!cur.isAlive())
                    it.remove();
            }

            // check whether it is possible to start some new threads
            synchronized (pending) {
                while (!pending.isEmpty() && running.size() < threadMax) {
                    running.add(new Thread(pending.poll()));
                    running.get(running.size() - 1).start();
                }
            }
        }
    }

    public void doStop() {
        isStoped = true;
        for (Thread cur : running) {
            cur.interrupt();
            try {
                cur.join();
            }
            catch (InterruptedException e) {
                // who cares?
            }
        }
    }
    
    @Override
    protected void finalize() {
        this.doStop();
    }
}
