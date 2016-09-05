package com.lab;

import java.util.ArrayDeque;
import java.util.Deque;

/**
 * Created by Katrin on 26.08.2016.
 */
public class MyQueue<T> {
    private final Deque<T> taskQueue;

    public MyQueue() {
        this.taskQueue = new ArrayDeque<>();
    }

    public synchronized void addLast(T task) {
        taskQueue.addLast(task);
    }

    public synchronized T pop() {
        T task;
        if (taskQueue.isEmpty()) {
            task = null;
        } else {
            task = taskQueue.pop();
        }
        return task;
    }
}
