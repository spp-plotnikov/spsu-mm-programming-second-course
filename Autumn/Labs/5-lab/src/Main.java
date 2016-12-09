import java.util.Random;

public class Main {
    private static final int LENGTH = 1000;

    private static int[] getFilledArray() {
        Random random = new Random();
        int[] array = new int[LENGTH];
        for (int i = 0 ; i < LENGTH; i++) {
            array[i] = random.nextInt(100);
        }

        return array;
    }

    public static void main(String args[]) throws Exception {
        int[] array = getFilledArray();
        //int[] array = new int[]{1,2,3,4,5,6,7,8,9,10}; //for test

        ArraySumByChunks sumChunks = new ArraySumByChunks();
        int sumCh = sumChunks.Sum(array);

        ArraySumRecursively sumRecursively = new ArraySumRecursively();
        int sumR = sumRecursively.Sum(array);

        System.out.println(sumCh);
        System.out.println(sumR);
    }
}
