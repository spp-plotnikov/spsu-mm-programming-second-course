package com.lab;

import java.util.Deque;
import java.util.concurrent.atomic.AtomicBoolean;

/**
 * Created by Katrin on 10.06.2016.
 */
public class MyThread extends Thread {

    private String name;
    private AtomicBoolean check;
    private boolean isSleep;
    private Task task;
    private Deque<Task> taskQueue;
    private int numberOfTask;

    public MyThread(String name, Deque<Task> taskQueue) {
        this.name = name;
        this.check = new AtomicBoolean(true);
        this.isSleep = true;
        this.taskQueue = taskQueue;
    }

    public Deque<Task> getTaskQueue() {
        return taskQueue;
    }

    private void setTask(Task task) {
        this.task = task;
    }

    public boolean isSleep() {
        return isSleep;
    }

    public void setSleep(boolean sleep) {
        isSleep = sleep;
    }

    public boolean isCheck() {
        return check.get();
    }

    public void setCheck(boolean check) {
        this.check.set(check);
    }

    @Override
    public void run() {
        setSleep(false);
        while (isCheck()) {
            setSleep(false);
            makeTask();
            setSleep(true);
            try {
                synchronized (this) {
                    this.wait();
                }
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
        }
    }

    private void makeTask() {
        while(true){
            synchronized (getTaskQueue()) {
                if (getTaskQueue().isEmpty()) {
                    return;
                }
                setTask(getTaskQueue().pop());
            }
            numberOfTask = task.doTask(name);
            System.out.println("Thread " + name + " completed task number " + numberOfTask);
        }
    }
}
