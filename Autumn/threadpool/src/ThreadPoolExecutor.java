public class ThreadPoolExecutor {
    private final TaskQueue taskQueue;
    private final myThread[] workers;

    public ThreadPoolExecutor(int numThreads) {
        taskQueue = new TaskQueue();
        workers = new myThread[numThreads];
        for (int i = 0; i < workers.length; i++) {
            workers[i] = new myThread(taskQueue);
            workers[i].start();
        }
    }

    public void add(Runnable r) {
        taskQueue.add(r);
        synchronized (taskQueue) {
            taskQueue.notify();
        }
    }
    public void shutdown() {
        for (int i = 0; i < workers.length; i++) {
            if(workers[i].isAlive()) {
                workers[i].installFlagOff();
            }
        }
    }
}

