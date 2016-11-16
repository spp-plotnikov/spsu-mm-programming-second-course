import sun.awt.Mutex;

import java.util.Queue;
import java.util.Random;

class Producer implements Runnable {
    Mutex mutex;
    boolean[] flag;
    Queue queue;
    Producer(Mutex m, Queue queue, boolean flag[]) {
        mutex = m;
        this.queue = queue;
        this.flag = flag;
        new Thread(this).start();
    }
    public void run() {
        Random r = new Random(System.currentTimeMillis());
        while(!flag[0]) {
            int num = r.nextInt(100);
            mutex.lock();
            queue.add(num);
            System.out.println("run prod");
            System.out.println("write: " + num);
            mutex.unlock();
            try {
                Thread.sleep(countTimeMills.timeSleepMills);
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
        }
    }
}
