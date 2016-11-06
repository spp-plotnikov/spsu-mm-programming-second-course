import java.util.concurrent.atomic.AtomicInteger;

// This class implements mutex using Filter algorithm
// Hours spent debugging this crap: 2.5
public class MyMutex implements Lock {
    AtomicInteger level[]; // yeah, we need them atomic, fancy that
    AtomicInteger victim[];
    public int n;

    public MyMutex(int threadCount) {
        n = threadCount;
        level = new AtomicInteger[n + 1];
        victim = new AtomicInteger[n + 1]; // actually we need only n, but...
        for (int i = 1; i <= n; i++) {
            level[i] = new AtomicInteger(0);
            victim[i] = new AtomicInteger(0);
        }
    }

    public void lock() {
        int me = (int) Thread.currentThread().getId() % n + 1;
        for (int i = 1; i < n; i++) {
            level[me].set(i);
            victim[i].set(me);

            // spin while conflict exists
            boolean conflict_exist = true;
            while (conflict_exist) {
                conflict_exist = false;
                for (int k = 1; k < n; k++) {
                    if (k != me && level[k].get() >= i && victim[i].get() == me) {
                        conflict_exist = true;
                        break;
                    }
                }
            }
        }
    }

    public void unlock() {
        int me = (int) Thread.currentThread().getId() % n + 1;
        level[me].set(0);
    }
}
