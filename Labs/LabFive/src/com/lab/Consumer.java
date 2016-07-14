package com.lab;

import java.util.List;
import java.util.concurrent.Semaphore;

/**
 * Created by Katrin on 15.06.2016.
 */
public class Consumer implements Runnable {

    private final List<Request> commonList;
    private int name;
    private Semaphore semaphore;

    public int getName() {
        return name;
    }

    public Consumer(List<Request> commonList, int name, Semaphore semaphore) {
        this.commonList = commonList;
        this.name = name;
        this.semaphore = semaphore;
    }

    @Override
    public void run() {
        while (MainAppThread.CHECK.get()) {
            try {
                semaphore.acquire();
                if (commonList.size() > 0) {
                    System.out.println("Consumer with name " + getName() + " removed request with number " +
                            commonList.get(0).getNumber());
                    commonList.remove(0);
                }
                semaphore.release();
                Thread.sleep(1000);

            } catch (InterruptedException e) {
                e.printStackTrace();
            }
        }
    }
}
