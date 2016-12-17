import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.atomic.AtomicMarkableReference;
import java.util.concurrent.locks.ReentrantLock;

public class RefinableHashTable<K,V> extends BaseHashTable<K,V> {
    AtomicMarkableReference<Thread> owner;
    volatile ReentrantLock[] locks;

    public RefinableHashTable (int capacity) {
        super(capacity);
        locks = new ReentrantLock[capacity];
        for (int j = 0; j < capacity; j++) {
            locks[j] = new ReentrantLock();
        }
        owner = new AtomicMarkableReference<Thread>(null, false);
    }

    protected void acquire(K key) {
        boolean[] mark = {true};
        Thread me = Thread.currentThread();
        Thread who;
        while (true) {
            do {
                who = owner.get(mark);
            } while (mark[0] && who != me);
            ReentrantLock[] oldLocks = this.locks;
            int index = (key.hashCode() & 0x7FFFFFFF) % table.length;
            ReentrantLock oldLock = oldLocks[index];
            oldLock.lock();
            who = owner.get(mark);
            if ((!mark[0] || who == me) && this.locks == oldLocks) {
                return;
            } else {
                oldLock.unlock();
            }
        }
    }

    protected void release(K key) {
        int index = (key.hashCode() & 0x7FFFFFFF) % table.length;
        locks[index].unlock();
    }

    protected void quiesce() {
        for (ReentrantLock lock : locks) {
            while (lock.isLocked()) {}
        }
    }

    protected void resize() {
        int oldCapacity = table.length;
        int newCapacity = 2 * oldCapacity;
        Thread me = Thread.currentThread();
        if (owner.compareAndSet(null, me, false, true)) {
            try {
                if (table.length != oldCapacity) {
                    return;
                }
                quiesce();
                List<Entry<K,V>>[] oldTable = table;
                table = (List<Entry<K,V>>[]) new List[newCapacity];
                for (int i = 0; i < newCapacity; i++)
                    table[i] = new ArrayList<Entry<K,V>>();
                locks = new ReentrantLock[newCapacity];
                for (int j = 0; j < locks.length; j++) {
                    locks[j] = new ReentrantLock();
                }
                initializeFrom(oldTable);
            } finally {
                owner.set(null, false);
            }
        }
    }

    protected boolean policy() {
        return size / table.length > 4;
    }

    protected void initializeFrom(List<Entry<K,V>>[] oldTable) {
        for (List<Entry<K,V>> bucket : oldTable) {
            for (Entry<K,V> e : bucket) {
                int index = (e.key.hashCode() & 0x7FFFFFFF) % table.length;
                table[index].add(e);
            }
        }
    }
}
