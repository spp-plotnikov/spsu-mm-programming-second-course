public class myThread extends Thread {
    private boolean flagOff = false;
    private final TaskQueue taskQueue;
    myThread(TaskQueue taskQueue) {
        this.taskQueue = taskQueue;
    }

    public void installFlagOff() {
        flagOff = true;
    }
    public void run() {
        while (true) {
            Runnable r = taskQueue.pop();
            if (r == null) {
                synchronized (taskQueue) {
                    while (taskQueue.isEmpty() && !flagOff) try {
                        taskQueue.wait();
                    } catch (InterruptedException e) {
                        e.printStackTrace();
                    }
                    if (flagOff) {
                        break;
                    }
                }
            } else {
                r.run();
            }
        }
    }
}
