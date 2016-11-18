public class Main {
    public static final int count = 2;
    public static void main(String[] args) {

        ThreadPoolExecutor executor = new ThreadPoolExecutor(count);
        for (int i = count * 2; i > 0; i--) {
            Runnable t = new test(1000000009, i);
            executor.add(t);
        }
        executor.shutdown();
    }
    public static class test implements Runnable {
        private int num;
        private int id;
        public test(int n, int id) {
            this.num = n;
            this.id = id;
        }
        public void run() {
            long time = System.currentTimeMillis();
            for (int i = 0; i < id; i++) {
                isPrime(num);
            }
            System.out.println("Done, id: " + id + "time: " + (System.currentTimeMillis() - time) / 1000.0);
        }
    }
    private static boolean isPrime(int num) {
        for (int i = 2; i < num; i++) {
            if (num % i == 0)
                return false;
        }
        return true;
    }
}


