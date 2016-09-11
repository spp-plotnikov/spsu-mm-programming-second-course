package lab.com;

import java.util.ArrayList;
import java.util.concurrent.Callable;
import java.util.concurrent.ExecutionException;
import java.util.concurrent.Future;
import java.util.concurrent.FutureTask;

/**
 * Created by Katrin on 10.09.2016.
 */
public class ArraySumImplOneWithCall implements ArraySum, Callable<Integer> {

    private final int[] array;
    private int end;
    private final int start;


    public ArraySumImplOneWithCall(int[] array, int end, int start) {
        this.array = array;
        this.end = end;
        this.start = start;
    }

    @Override
    public int sum(int[] a) {
        return 0;
    }

    @Override
    public Integer call() throws Exception {

        int sum = 0;
        int size = end - start + 1;

        ArrayList<Future<Integer>> results = new ArrayList<>();

        if (size % 2 == 1) {
            sum += array[end];
            size--;
            end--;
        }

        if (size > 1) {
            int firstStart = start;
            int firstEnd = firstStart + size / 2 - 1;
            int secondStart = firstEnd + 1;
            int secondEnd = end;
            //0 - 3 | 0 - 1 2-2
            ArraySumImplOneWithCall arraySumImplTwo1 = new ArraySumImplOneWithCall(array, firstEnd, firstStart);
            FutureTask<Integer> task1 = new FutureTask<>(arraySumImplTwo1);
            results.add(task1);
            (new Thread(task1)).start();

            ArraySumImplOneWithCall arraySumImplTwo2 = new ArraySumImplOneWithCall(array, secondEnd, secondStart);
            FutureTask<Integer> task2 = new FutureTask<>(arraySumImplTwo2);
            results.add(task2);
            (new Thread(task2)).start();


        }

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
