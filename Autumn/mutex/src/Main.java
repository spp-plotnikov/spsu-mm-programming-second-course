import sun.awt.Mutex;
import java.util.LinkedList;
import java.util.Queue;

class countTimeMills {
    public static final int timeSleepMills = 20;
}
public class Main {
    public static void main(String[] args) {
        int countCons = 2;
        int countProd = 4;
        boolean[] flag = new boolean[] {false};
        Mutex mutex = new Mutex();
        Queue queue = new LinkedList();
        for (int i = 0; i < countProd; i++) {
            new Producer(mutex, queue, flag);
        }
        for (int i = 0; i < countCons; i++) {
            new Consumer(mutex, queue, flag);
        }
        new KeyCatcher(flag);
    }
}



