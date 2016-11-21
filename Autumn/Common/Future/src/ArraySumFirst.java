import java.util.*;
import java.util.concurrent.*;

public class ArraySumFirst implements ArraySum {
    static int threadCnt = 2;

    public long sum(int[] arr) throws Exception {
        List<Future<Long>> toAdd = new LinkedList<>();
        ExecutorService executor = Executors.newFixedThreadPool(threadCnt);
        int chunkSize = arr.length / threadCnt;

        for (int i = 0; i < threadCnt; i++) {
            final int cur = i;
            Callable<Long> task = () -> {
                long chunkSum = 0;
                for (int j = 0; j < chunkSize; j++)
                    chunkSum += arr[cur * chunkSize + j];
                return chunkSum;
            };
            toAdd.add(executor.submit(task));
        }

        long res = 0;
        for (Future<Long> result : toAdd)
            res += result.get();
        for (int i = chunkSize * threadCnt; i < arr.length; i++)
            res += arr[i];
        return res;
    }
}
