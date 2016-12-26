/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package test3;

import java.util.ArrayList;
import java.util.concurrent.ExecutionException;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import java.util.concurrent.FutureTask;

/**
 *
 * @author Katrin
 */
public class CrashTest {

    private static final int COUNT_OF_CLIENTS = 35;

    public static void main(String[] args) throws InterruptedException, ExecutionException {
        ArrayList<FutureTask<Long>> clients = new ArrayList();
        ExecutorService exService = Executors.newCachedThreadPool();
        try {
            for (int i = 0; i < COUNT_OF_CLIENTS; i++) {
                FutureTask<Long> ft = new FutureTask<>(new Client("D:\\kitten_7-5.jpg"));
                clients.add(ft);
                exService.execute(ft);
            }
            for (int i = 0; i < COUNT_OF_CLIENTS; i++) {
                clients.get(i).get();
            }
        } catch (Exception e) {
            e.printStackTrace();
        }

        System.out.println("End");

        exService.shutdown();

    }

}
