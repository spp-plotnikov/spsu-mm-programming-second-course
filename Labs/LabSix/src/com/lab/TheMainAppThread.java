package com.lab;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.util.Random;

/**
 * Created by Katrin on 10.06.2016.
 */
public class TheMainAppThread {

    public static void main(String[] args) throws IOException, InterruptedException {
        try (ThreadPool threadPool = new ThreadPool();
             BufferedReader reader = new BufferedReader(new InputStreamReader(System.in))) {
            Random random = new Random();
            int numberOfTask = 1;

            System.out.println("Write '-' if you want to stop..");

            while (!(reader.readLine()).equals("-")) {
                threadPool.enqueue(new Task(numberOfTask, random.nextInt(10)));
                numberOfTask++;
            }
        }
    }
}
