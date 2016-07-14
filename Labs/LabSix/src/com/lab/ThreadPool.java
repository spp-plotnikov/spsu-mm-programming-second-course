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

    public void addNewTask(int numberOfTask, int timer) {
        for (int i = 0; i < countOfThreads; i++) {
            if (threads[i].isSleep()) {
                synchronized (threads[i]) {
                    threads[i].setTask(new Task(numberOfTask, timer));
                    threads[i].notify();
                }
                return;
            }
        }
        if (countOfThreads != SIZE) {
            synchronized (specialVarForSynch) {
                threads[countOfThreads] = new MyThread(Integer.toString(countOfThreads), taskQueue);
                threads[countOfThreads].setTask(new Task(numberOfTask, timer));
                threads[countOfThreads].start();
                countOfThreads++;
            }
        } else {
            synchronized (taskQueue) {
                taskQueue.addLast(new Task(numberOfTask, timer));
            }
        }
    }

    @Override
    public void close() throws IOException {
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
