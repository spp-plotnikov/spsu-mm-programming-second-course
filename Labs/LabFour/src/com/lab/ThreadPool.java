package com.lab;

import java.io.IOException;

/**
 * Created by Katrin on 10.06.2016.
 */
public class ThreadPool implements AutoCloseable {

    private static final int SIZE = 11;
    private final MyQueue<Task> taskQueue;
    private MyThread[] threads;
    private int countOfThreads;
    private final Object specialVarForSync = new Object();

    public ThreadPool() {
        this.taskQueue = new MyQueue<>();
        this.threads = new MyThread[SIZE];
        this.countOfThreads = 0;
    }

    public void enqueue(Task task) {
        taskQueue.addLast(task);

        for (int i = 0; i < countOfThreads; i++) {
            synchronized (threads[i]) {
                if (threads[i].getState() == Thread.State.WAITING) {
                    threads[i].notify();
                    return;
                }
            }
        }
        synchronized (specialVarForSync) {
            if (countOfThreads != SIZE) {
                threads[countOfThreads] = new MyThread(Integer.toString(countOfThreads), taskQueue);
                threads[countOfThreads].start();
                countOfThreads++;
            }
        }

    }

    @Override
    public void close() throws IOException, InterruptedException {
        for (int i = 0; i < countOfThreads; i++) {
            threads[i].setCheck(false);
            synchronized (threads[i]) {
                threads[i].notify();
            }
        }
    }
}
