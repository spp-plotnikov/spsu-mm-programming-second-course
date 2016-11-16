import sun.awt.Mutex;

import java.util.Queue;

class Consumer implements Runnable {
    Mutex mutex;
    Queue queue;
    boolean[] flag;
    Consumer(Mutex m, Queue queue, boolean flag[]) {
        mutex = m;
        this.queue = queue;
        this.flag = flag;
        new Thread(this).start();
    }
    public void run() {
        while (!flag[0]) {
            mutex.lock();
            if (!queue.isEmpty()) {
                System.out.println("run cons!");
                System.out.println("read: " + queue.poll());
            } else {
                System.out.println("queue is empty!");
            }
            mutex.unlock();
            try {
                Thread.sleep(countTimeMills.timeSleepMills);
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
        }
    }
}
