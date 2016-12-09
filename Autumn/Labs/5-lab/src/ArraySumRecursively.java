import java.util.Arrays;
import java.util.concurrent.*;


public class ArraySumRecursively implements IArraySum {
    private final int notMagicThreadPoolSize = 2;

    public int Sum(int[] array) {
        if (array.length == 1) {
            return array[0];
        }

        ExecutorService pool = Executors.newFixedThreadPool(notMagicThreadPoolSize);

        Future<Integer>[] futures = new Future[2];

        Callable<Integer> callable;
        callable = new SumCallable(array, 0, array.length/2);
        futures[0] = pool.submit(callable);
        callable = new SumCallable(array, array.length/2, array.length);
        futures[1] = pool.submit(callable);

        int sum = 0;
        for (Future<Integer> future : futures) {
            try {
                sum += future.get();
            } catch (InterruptedException e) {
                e.printStackTrace();
            } catch (ExecutionException e) {
                e.printStackTrace();
            }
        }
        pool.shutdown();
        return sum;
    }

    private class SumCallable implements Callable {
        private int[] array;
        private int partStart;
        private int partEnd;

        public SumCallable(int[] array, int partStart, int partEnd) {
            this.array = array;
            this.partStart = partStart;
            this.partEnd = partEnd;
        }

        public Integer call() {
            return Sum(Arrays.copyOfRange(array, partStart, partEnd));
        }
    }
}