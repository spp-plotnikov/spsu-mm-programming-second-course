package com.lab;

import java.util.List;
import java.util.concurrent.Semaphore;

/**
 * Created by Katrin on 15.06.2016.
 */
public class Manufacturer implements Runnable {

    private final List<Request> commonList;
    private int name;
    private Semaphore semaphore;

    public Manufacturer(List<Request> commonList, int name, Semaphore semaphore) {
        this.commonList = commonList;
        this.name = name;
        this.semaphore = semaphore;
    }

    public int getName() {
        return name;
    }

    @Override
    public void run() {
        int countOfRequest = 1;
        while (MainAppThread.check.get()) {
            try {
                semaphore.acquire();
                String number = getName() + "." + countOfRequest;
                commonList.add(new Request(number));
                System.out.println("Manufacturer with name " + getName() + " created request number " +
                        commonList.get(commonList.size() - 1).getNumber());
                semaphore.release();
                Thread.sleep(1000);
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
            countOfRequest++;
        }
    }
}
