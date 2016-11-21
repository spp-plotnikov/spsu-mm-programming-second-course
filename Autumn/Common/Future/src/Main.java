public class Main {
    public static void main(String[] args) throws Exception {
        int[] arr = {5, 52, 15, 17, 88, 11, 55};
        long res1 = new ArraySumFirst().sum(arr);
        long res2 = new ArraySumSecond().sum(arr);
        if (res1 != res2)
            throw new AssertionError("res1=" + res1 + ",res2=" + res2);
        System.out.println(res1);
    }
}
