/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package test2;

import java.util.ArrayList;
import java.util.concurrent.ExecutionException;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import java.util.concurrent.FutureTask;

/**
 *
 * @author Katrin
 */
public class MedianAndMiddleTest {

    private static final int COUNT_OF_CLIENTS = 9;
    private static ArrayList<FutureTask<Long>> clients = new ArrayList();


    public static void main(String[] args) throws InterruptedException, ExecutionException {
        ExecutorService exService = Executors.newCachedThreadPool();
        
        init(exService);
        

        long[] result = new long[9];
        long sum = 0;

        for (int i = 0; i < COUNT_OF_CLIENTS; i++) {
            result[i] = clients.get(i).get();
            sum += clients.get(i).get();
        }

            sortArr(result);
            
            System.out.println("Median: " + result[(int) COUNT_OF_CLIENTS / 2 + 1]);
            System.out.println("Middle: " + sum / COUNT_OF_CLIENTS);

            System.out.println("End");
            exService.shutdown();
    }
    
    private static void init(ExecutorService exService){
        
        FutureTask<Long> ft = new FutureTask<>(new Client("D:\\hqdefault.jpg"));
        clients.add(ft);
        exService.execute(ft);
        ft = new FutureTask<>(new Client("D:\\1335868183_malenkie_kotjata_20.jpg"));
        clients.add(ft);
        exService.execute(ft);
        ft = new FutureTask<>(new Client("D:\\42_059.jpg"));
        clients.add(ft);
        exService.execute(ft);
        ft = new FutureTask<>(new Client("D:\\mal.jpg"));
        clients.add(ft);
        exService.execute(ft);
        ft = new FutureTask<>(new Client("D:\\Чихуахуа.jpg"));
        clients.add(ft);
        exService.execute(ft);
        ft = new FutureTask<>(new Client("D:\\img.jpg"));
        clients.add(ft);
        exService.execute(ft);
        ft = new FutureTask<>(new Client("D:\\chihu.jpg"));
        clients.add(ft);
        exService.execute(ft);
        ft = new FutureTask<>(new Client("D:\\1335868138_malenkie_kotjata_43.jpg"));
        clients.add(ft);
        exService.execute(ft);
        ft = new FutureTask<>(new Client("D:\\kitten_7-5.jpg"));
        clients.add(ft);
        exService.execute(ft);
    }

    private static void sortArr(long[] arr) {
        for (int i = arr.length - 1; i > 0; i--) {
            for (int j = 0; j < i; j++) {
                if (arr[j] > arr[j + 1]) {
                    long tmp = arr[j];
                    arr[j] = arr[j + 1];
                    arr[j + 1] = tmp;
                }
            }
        }
    }

}
