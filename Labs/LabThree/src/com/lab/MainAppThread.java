package com.lab;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.Semaphore;
import java.util.concurrent.atomic.AtomicBoolean;

/**
 * Created by Katrin on 15.06.2016.
 */
public class MainAppThread {

    public static AtomicBoolean check = new AtomicBoolean(true);
    private final static List<Request> commonList = new ArrayList<>();

    private static final int COUNTOFCONSUMER = 5;
    private static final int COUNTOFMANUFACTURER = 5;

    private final static List<Thread> listOfConsumer = new ArrayList<>(COUNTOFCONSUMER) ;
    private final static List<Thread> listOfManufacturer = new ArrayList<>(COUNTOFMANUFACTURER) ;


    public static void main(String[] args) throws IOException, InterruptedException {


        System.out.println("Please, press any key for stop..");
        Thread.sleep(2000);

        /*for (int i = 0; i < COUNTOFCONSUMER; i++) {
            Thread m = new Thread(new Consumer(commonList, i));
            Thread c = new Thread(new Manufacturer(commonList, i));
            m.start();
            c.start();
        }*/

        Semaphore semaphore = new Semaphore(1);

        for (int i = 0; i < COUNTOFCONSUMER; i++) {
            Thread c = new Thread(new Manufacturer(commonList, i, semaphore));
            listOfConsumer.add(c);
            c.start();
        }
        for (int i = 0; i < COUNTOFMANUFACTURER; i++) {
            Thread m = new Thread(new Consumer(commonList, i, semaphore));
            listOfManufacturer.add(m);
            m.start();
        }

        BufferedReader reader = new BufferedReader(new InputStreamReader(System.in));
        reader.readLine();
        check.set(false);

        for (int i = 0; i < COUNTOFCONSUMER; i++) {
            listOfConsumer.get(0).join();
            listOfConsumer.remove(0);
        }
        for (int i = 0; i < COUNTOFMANUFACTURER; i++) {
            listOfManufacturer.get(0).join();
            listOfManufacturer.remove(0);
        }
    }
}
