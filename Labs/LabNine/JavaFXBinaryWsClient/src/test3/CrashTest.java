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

    public static void main(String[] args) throws InterruptedException, ExecutionException {
        ArrayList<FutureTask<Long>> clients = new ArrayList();
        ExecutorService exService = Executors.newCachedThreadPool();

        System.out.println(init(clients, exService));

        exService.shutdown();
        System.out.println("End");

    }

    private static int init(ArrayList<FutureTask<Long>> clients, ExecutorService exService) throws InterruptedException, ExecutionException {
        int count = 0;
        while (true) {
            for (int i = 0; i < count; i++) {
                Client client = new Client("D:\\kitten_7-5.jpg");
                FutureTask<Long> ft = new FutureTask<>(client);
                clients.add(ft);
                exService.execute(ft);
            }
            for (int i = 0; i < count; i++) {
                if (clients.get(i).get() == -1) {
                    System.out.println("Count: " + count);
                    return count;
                }
            }
            for(int i=0;i<clients.size();i++){
                clients.remove(0);
            }
        }
    }

}
