import java.util.concurrent.*;


public class ArraySumByChunks implements IArraySum {
    private final int magicThreadPoolSize = Runtime.getRuntime().availableProcessors() * 2;

    /*
     * splitOnAlmostEqualChunks returns array of segments' start and end positions:
     * [0start, 0end/1start, 1end/2start, ..., (N-1)end/Nstart, Nend], N = 0...count-1
    */
    private int[] splitOnAlmostEqualChunks(int length, int count) {
        int rest = length % count;
        int[] pos = new int[count + 1];
        pos[0] = 0;
        pos[count] = length;
        for (int i = 1; i < count; i++) {
            pos[i] = pos[i - 1] + length/count;
            if (rest != 0) {
                pos[i]++;
                rest--;
            }

        }

        return pos;
    }

    public int Sum(int[] array) {
        ExecutorService pool = Executors.newFixedThreadPool(magicThreadPoolSize);

        int partsCount = magicThreadPoolSize; //make them equal
        Future<Integer>[] futures = new Future[partsCount];

        int[] positions = splitOnAlmostEqualChunks(array.length, partsCount);

        for (int i = 0; i < partsCount; i++) {
            Callable<Integer> callable = new SumCallable(array, positions[i], positions[i + 1]);
            futures[i] = pool.submit(callable);
        }

        int sum = 0;
        for (int i = 0; i < partsCount; i++) {
            try {
                sum += futures[i].get();
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
            int partialSum = 0;
            for (int i = partStart; i < partEnd; i++) {
                partialSum += array[i];
            }

            return partialSum;
        }
    }
}