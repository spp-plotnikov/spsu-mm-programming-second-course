import java.util.*;
import java.util.concurrent.*;

public class ArraySumSecond implements ArraySum {
    static int threadCnt = 2;

    public long sum(int[] arr) throws Exception {
        if (threadCnt < 2)
            throw new AssertionError("Thread count should be 2 or more!");

        ArrayList<Future<Long>> toAdd = new ArrayList<>();
        ExecutorService executor = Executors.newFixedThreadPool(threadCnt);
        int chunkSize = arr.length / threadCnt;

        if (arr.length > 1) {
            for (int i = 0; i < threadCnt; i++) {
                int[] buffer = new int[chunkSize];
                System.arraycopy(arr, i * chunkSize, buffer, 0, chunkSize);
                Callable<Long> task = () -> new ArraySumSecond().sum(buffer);
                toAdd.add(executor.submit(task));
            }
        }

        long res = 0;
        for (Future<Long> result : toAdd)
            res += result.get();
        for (int i = chunkSize * threadCnt; i < arr.length; i++)
            res += arr[i];
        return res;
    }
}
