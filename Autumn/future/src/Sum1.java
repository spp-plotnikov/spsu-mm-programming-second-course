import java.util.concurrent.*;

class Sum1 implements IArraySum {
    static int countProc = Runtime.getRuntime().availableProcessors();
    public int sum(int[] a) {
        ExecutorService es = Executors.newFixedThreadPool(countProc);
        Future<Integer>[] future = new Future[countProc];
        for (int i = 0; i < future.length; i++) {
            future[i] = es.submit(new func(a, i));
        }
        int res = 0;
        try {
            for (int i = 0; i < countProc; i++) {
                res += future[i].get();
            }
        } catch (InterruptedException exc) {
            System.out.println(exc);
        } catch (ExecutionException exc) {
            System.out.println(exc);
        }
        es.shutdown();
        return res;
    }
    static class func implements Callable<Integer> {
        int[] mas;
        int me;
        func(int mas[], int id) {
            this.mas = mas;
            me = id;
        }
        public Integer call() {
            int sum = 0;
            for (int i = me; i < mas.length; i += countProc) {
                sum += mas[i];
            }
            return sum;
        }
    }
}
