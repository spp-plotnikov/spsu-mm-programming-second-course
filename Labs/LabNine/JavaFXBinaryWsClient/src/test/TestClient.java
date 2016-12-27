/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package test;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.concurrent.ExecutionException;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import java.util.concurrent.FutureTask;
import javax.websocket.Session;

/**
 *
 * @author Katrin
 */
public class TestClient {

    private static final int COUNT_OF_CLIENTS = 10;

    public static void main(String[] args) throws InterruptedException, ExecutionException {
        ArrayList<FutureTask<Long>> clients = new ArrayList();
        ExecutorService exService = Executors.newCachedThreadPool();
        int[] result = new int[COUNT_OF_CLIENTS];
        int commonCount = 0;
        for (int j = 1; j < COUNT_OF_CLIENTS; j++) {
            for (int i = 0; i < j; i++) {
                FutureTask<Long> ft = new FutureTask<>(new Client());
                clients.add(ft);
                exService.execute(ft);
                commonCount++;
            }
            for (int i = 0; i < j; i++) {
                result[j] += clients.get(commonCount - j + i).get();
            }
        }
        for (int i = 1; i < COUNT_OF_CLIENTS; i++) {
            System.out.println(result[i]/i);
        }

        System.out.println("End");
        exService.shutdown();

    }

}
