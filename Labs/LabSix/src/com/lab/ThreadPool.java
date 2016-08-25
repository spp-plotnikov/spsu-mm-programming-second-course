package com.lab;

import java.io.IOException;
import java.util.ArrayDeque;
import java.util.Deque;

/**
 * Created by Katrin on 10.06.2016.
 */
public class ThreadPool implements AutoCloseable {

    private static final int SIZE = 11;
    private final Deque<Task> taskQueue;
    private MyThread[] threads;
    private int countOfThreads;

    private final Object specialVarForSynch = new Object();

    public ThreadPool() {
        this.taskQueue = new ArrayDeque<>();
        this.threads = new MyThread[SIZE];
        this.countOfThreads = 0;
    }

    public void enqueue(Task task) {
        for (int i = 0; i < countOfThreads; i++) {
            synchronized (threads[i]) {
                if (threads[i].isSleep()) {
                    taskQueue.addLast(task);
                    threads[i].notify();
                    return;
                }
            }
        }
        synchronized (specialVarForSynch) {
            if (countOfThreads != SIZE) {
                threads[countOfThreads] = new MyThread(Integer.toString(countOfThreads), taskQueue);
                taskQueue.addLast(task);
                threads[countOfThreads].start();
                countOfThreads++;
            } else {
                synchronized (taskQueue) {
                    taskQueue.addLast(task);
                }
            }
        }
    }

    @Override
    public void close() throws IOException, InterruptedException {
        for (int i = 0; i < countOfThreads; i++) {
            threads[i].setCheck(false);
            while (!threads[i].isSleep()) {
                try {
                    Thread.sleep(1000);
                } catch (InterruptedException e) {
                    e.printStackTrace();
                }
            }
            synchronized (threads[i]) {
                threads[i].notify();
            }
        }
    }
}
