package lab.com;

import java.util.Date;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

/**
 * Created by Katrin on 10.09.2016.
 */
public class MainThread {
    private static final int COUNT_OF_ELEMENTS = 10000;

    public static void main(String[] args) {

        int[] a = createArray();

        long time = (new Date()).getTime();

        ExecutorService executor = Executors.newCachedThreadPool();
        ArraySum arraySumImplOne = new ArraySumImplOne(executor);
        try {
            System.out.println(arraySumImplOne.sum(a));
        } catch (Exception e) {
            e.printStackTrace();
        }
        System.out.println(((new Date()).getTime() - time) / 1000);
        executor.shutdown();

        time = (new Date()).getTime();
        ArraySumImplTwo arraySumImplTwo = new ArraySumImplTwo();
        try {
            System.out.println(arraySumImplTwo.sum(a));
        } catch (Exception e) {
            e.printStackTrace();
        }
        System.out.println(((new Date()).getTime() - time) / 1000);


        time = (new Date()).getTime();
        System.out.println(makeStandartSum(a));
        System.out.println(((new Date()).getTime() - time) / 1000);


    }

    private static int makeStandartSum(int[] a) {
        int sum = 0;
        for (int anA : a) {
            sum += anA;
        }
        return sum;
    }

    private static int[] createArray() {
        int[] a = new int[COUNT_OF_ELEMENTS];
        for (int i = 0; i < COUNT_OF_ELEMENTS; i++) {
            a[i] = i;
        }
        return a;
    }
}
