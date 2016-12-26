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
        for (int i = 0; i < COUNT_OF_CLIENTS; i++) {
            FutureTask<Long> ft = new FutureTask<>(new Client());
            clients.add(ft);
            exService.execute(ft);
        }
        
        for(int i =0; i< COUNT_OF_CLIENTS;i++){
            System.out.println(clients.get(i).get());
        }

        System.out.println("End");
        exService.shutdown();

    }

}
