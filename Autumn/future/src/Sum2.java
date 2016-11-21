import java.util.ArrayList;
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
        ArrayList<Future<Integer>> ans = new ArrayList<>();
        ans.add(es.submit(new Callable<Integer>() {
            @Override
            public Integer call() throws Exception {
                return new Sum2().sum(buf1);
            }

        }));
        ans.add(es.submit(new Callable<Integer>() {
            @Override
            public Integer call() throws Exception {
                return new Sum2().sum(buf2);
            }

        }));
        int res = 0;
        for (Future<Integer> it: ans) {
            try {
                res += it.get();
            } catch (InterruptedException e) {
                e.printStackTrace();
            } catch (ExecutionException e) {
                e.printStackTrace();
            }
        }
        es.shutdown();
        return res;
    }
}
