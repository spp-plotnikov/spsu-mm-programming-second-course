package lab.com;

import java.util.ArrayList;
import java.util.concurrent.*;

/**
 * Created by Katrin on 11.09.2016.
 */
public class ArraySumImplTwo implements ArraySum {
    private static final int COUNT_OF_THREADS = 10;

    @Override
    public int sum(int[] a) {
        int sum = 0;
        int[] array = a;
        int size = array.length;
        ArrayList<Future<Integer>> results = new ArrayList<>();
        ExecutorService executor = Executors.newCachedThreadPool();
        int i = 0;
        final int countOfElements = size / COUNT_OF_THREADS;

        while (size > 0) {
            final int[] newArray = new int[countOfElements + 1];
            System.arraycopy(array, i, newArray, 0, countOfElements);
            i += countOfElements;
            results.add(executor.submit(new Callable<Integer>() {
                @Override
                public Integer call() throws Exception {
                    int sum = 0;
                    for (int j = 0; j < countOfElements; j++) {
                        sum += newArray[j];
                    }
                    return sum;
                }
            }));
            size -= countOfElements;
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
