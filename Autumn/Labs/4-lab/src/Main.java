public class Main {
    public static void main(String[] args) {
        ThreadPool pool = new ThreadPool();

        pool.enqueue(() -> {
            System.out.println("1: 1");
            try {
                Thread.sleep(1000);
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
            System.out.println("1: 2");
        });

        pool.enqueue(() -> {
            System.out.println("2: 1");
            try {
                Thread.sleep(5000);
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
            System.out.println("2: 2");
        });

        try {
            Thread.sleep(3000);
        } catch (InterruptedException e) {
            e.printStackTrace();
        }

        pool.dispose();
    }
}
