package lab.com;

import java.util.ArrayList;
import java.util.concurrent.*;

/**
 * Created by Katrin on 11.09.2016.
 */
public class ArraySumImplOne implements ArraySum {
    @Override
    public int sum(int[] a) {

        final int[] array = a;
        int sum = 0;
        int size = array.length;
        ArrayList<Future<Integer>> results = new ArrayList<>();


        if (size % 2 == 1) {
            sum += array[size - 1];
            size--;
        }
        final int finalSize = size / 2;
        ExecutorService executor = Executors.newCachedThreadPool();
        if (size > 1) {
            for (int i = 0; i < 2; i++) {
                final int[] newArray = new int[finalSize];
                System.arraycopy(array, i * finalSize, newArray, 0, finalSize);

                results.add(executor.submit(new Callable<Integer>() {
                    @Override
                    public Integer call() throws Exception {
                        return new ArraySumImplOne().sum(newArray);
                    }
                }));
            }
        }
        executor.shutdown();

        for (Future<Integer> result : results) {
            try {
                sum += result.get();
            } catch (InterruptedException | ExecutionException e) {
                e.printStackTrace();
            }
        }
        return sum;
    }
}
