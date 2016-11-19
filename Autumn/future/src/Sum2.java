import java.util.concurrent.*;

class Sum2 implements IArraySum{
    public int sum(int a[]) {
        int countProc = Runtime.getRuntime().availableProcessors() / 2;
        ExecutorService es = Executors.newFixedThreadPool(countProc);
        Future<Integer>[] future = new Future[countProc];
        for (int i = 0; i < countProc; i++) {
            int countElement = a.length / countProc + 1;
            int l = i * countElement;
            int r = l + countElement - 1;
            if (r >= a.length) {
                r = a.length - 1;
            }
            future[i] = es.submit(new func(a, l, r));
        }
        int res = 0;
        try {
            for (int i = 0; i < countProc; i++) {
                res += future[i].get();
            }
        } catch (InterruptedException e) {
            e.printStackTrace();
        } catch (ExecutionException e) {
            e.printStackTrace();
        }
        es.shutdown();
        return res;
    }
        static class func implements Callable<Integer> {
        int[] mas;
        int l;
        int r;
        func(int mas[], int l, int r) {
            this.mas = mas;
            this.l = l;
            this.r = r;
        }
        public Integer call() {
            int sum = 0;
            for (int i = l; i <= r; i++) {
                sum += mas[i];
            }
            return sum;
        }
    }

}
