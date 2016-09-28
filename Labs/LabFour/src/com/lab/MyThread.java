package com.lab;

import java.util.concurrent.atomic.AtomicBoolean;

/**
 * Created by Katrin on 10.06.2016.
 */
public class MyThread extends Thread {

    private String name;
    private AtomicBoolean check;
    private Task task;
    private MyQueue<Task> taskQueue;
    private int numberOfTask;

    public MyThread(String name, MyQueue<Task> taskQueue) {
        this.name = name;
        this.check = new AtomicBoolean(true);
        this.taskQueue = taskQueue;
    }

    private MyQueue<Task> getTaskQueue() {
        return taskQueue;
    }

    private void setTask(Task task) {
        this.task = task;
    }

    private boolean isCheck() {
        return check.get();
    }

    public void setCheck(boolean check) {
        this.check.set(check);
    }

    @Override
    public void run() {
        makeTask();
        while (isCheck()) {
            try {
                synchronized (this) {
                    if (isCheck()) {
                        this.wait();
                    }
                }
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
            makeTask();
        }
    }

    private void makeTask() {
        while (true) {
            setTask(getTaskQueue().pop());
            if (task == null) {
                return;
            }
            numberOfTask = task.doTask(name);
            System.out.println("Thread " + name + " completed task number " + numberOfTask);
        }
    }
}
