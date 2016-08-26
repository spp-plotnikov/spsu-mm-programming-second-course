package com.lab;

import java.util.ArrayDeque;
import java.util.Deque;

/**
 * Created by SA on 26.08.2016.
 */
public class MyQueue<T> {
    private final Deque<T> taskQueue;

    public MyQueue() {
        this.taskQueue = new ArrayDeque<>();
    }

    public synchronized void addLast(T task) {
        taskQueue.addLast(task);
    }

    public synchronized T pop(){
        return taskQueue.pop();
    }
    public synchronized boolean isEmpty(){
        return taskQueue.isEmpty();
    }

}
