import java.util.concurrent.*;

class Sum2 implements IArraySum{
    public int sum(int a[]) {
        if (a.length == 1) {
            return a[0];
        }
        ExecutorService es = Executors.newFixedThreadPool(2);
        int[] buf1 = new int[a.length / 2];
        int[] buf2 = new int[a.length / 2 + (a.length & 1)];
        for (int i = 0; i < a.length / 2; i++) {
            buf1[i] = a[i];
        }
        int k = 0;
        for (int i = a.length / 2; i < a.length; i++) {
            buf2[k++] = a[i];
        }
        Future<Integer> future1 = es.submit(new func(buf1));
        Future<Integer> future2 = es.submit(new func(buf2));
        int res = 0;
        try {
            res = future1.get() + future2.get();
        } catch (InterruptedException e) {
            e.printStackTrace();
        } catch (ExecutionException e) {
            e.printStackTrace();
        }
        es.shutdown();
        return res;
    }
    static class func implements Callable<Integer> {
        int[] a;
        func(int a[]) {
            this.a = a;
        }
        public Integer call() {
            int sum = 0;
            for (int i = 0; i < a.length; i++) {
                sum += a[i];
            }
            return sum;
        }
    }

}
