import java.util.Random;

public class Main {
    public static final int size = 1000;
    public static void main(String[] args) {
        int[] mas = new int[size];
        Random r = new Random(System.currentTimeMillis());
        for (int i = 0; i < mas.length; i++) {
            mas[i] = r.nextInt(10);
        }
        Sum1 s1 = new Sum1();
        Sum2 s2 = new Sum2();
        long time1 = System.currentTimeMillis();
        System.out.println(s1.sum(mas));
        System.out.println((System.currentTimeMillis() - time1) / 1000.0);
        time1 = System.currentTimeMillis();
        System.out.println(s2.sum(mas));
        System.out.println((System.currentTimeMillis() - time1) / 1000.0);
        System.out.println("shutdown");
    }
}

