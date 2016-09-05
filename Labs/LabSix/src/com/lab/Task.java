package com.lab;

/**
 * Created by Katrin on 10.06.2016.
 */
public class Task {

    private int timer;
    private int numberOfTask;

    public Task(int numberOfTask, int timer) {
        this.timer = timer;
        this.numberOfTask = numberOfTask;
    }

    public int doTask(String name) {
        System.out.println("Thread " + name + " started task number " + numberOfTask + " with timer: " + timer);
        try {
            Thread.sleep(timer * 1000);
        } catch (InterruptedException e) {
            e.printStackTrace();
        }
        return numberOfTask;
    }


}
