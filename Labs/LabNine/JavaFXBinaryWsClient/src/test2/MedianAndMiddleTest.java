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

    private static final int COUNT_OF_CLIENTS = 5;
    private static final int COUNT_OF_PIC = 5;
    private static ArrayList<FutureTask<Long>> clients = new ArrayList();

    public static void main(String[] args) throws InterruptedException, ExecutionException {
        ExecutorService exService = Executors.newCachedThreadPool();
        long[] result = new long[COUNT_OF_PIC];
        long[] sum = new long[COUNT_OF_PIC];
        int commonCount = 0;

        init(exService, result, sum, "D:\\hqdefault.jpg", commonCount);
        commonCount++;

        Thread.sleep(3000);
        init(exService, result, sum, "D:\\mal.jpg", commonCount);
        commonCount++;
        Thread.sleep(3000);
        init(exService, result, sum, "D:\\42_059.jpg", commonCount);
        commonCount++;
        Thread.sleep(3000);
        init(exService, result, sum, "D:\\1335868183_malenkie_kotjata_20.jpg", commonCount);
        commonCount++;

        init(exService, result, sum, "D:\\Чихуахуа.jpg", commonCount);
        commonCount++;

        sortArr(result);

        for (int i = 0; i < COUNT_OF_PIC; i++) {
            System.out.println("Median: " + result[i]);
            System.out.println("Middle: " + sum[i] / COUNT_OF_CLIENTS);
        }

        System.out.println("End");
        exService.shutdown();
    }

    private static void init(ExecutorService exService, long[] result, long[] sum, String path, int commonCount) throws InterruptedException, ExecutionException, ExecutionException {
        long[] supportArr = new long[COUNT_OF_CLIENTS];
        for (int i = 0; i < COUNT_OF_CLIENTS; i++) {
            FutureTask<Long> ft = new FutureTask<>(new Client(path));
            clients.add(ft);
            exService.execute(ft);
        }
        for (int i = 0; i < COUNT_OF_CLIENTS; i++) {
            supportArr[i] = clients.get(i + commonCount * COUNT_OF_CLIENTS).get();
            sum[commonCount] += clients.get(i + commonCount * COUNT_OF_CLIENTS).get();
        }
        sortArr(supportArr);
        result[commonCount] = supportArr[(int) COUNT_OF_CLIENTS / 2];
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
