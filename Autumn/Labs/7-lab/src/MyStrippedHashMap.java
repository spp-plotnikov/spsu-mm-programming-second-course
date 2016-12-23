import java.util.LinkedList;
import java.util.concurrent.locks.ReentrantLock;

public class MyStrippedHashMap<K, V> {
    private LinkedList<Entry<K, V>>[] table;
    protected int mapSize;
    final ReentrantLock[] locks;

    public MyStrippedHashMap(int capacity) {
        this.mapSize = 0;
        this.table = new LinkedList[capacity];

        for (int i = 0; i < capacity; i++) {
            this.table[i] = new LinkedList();
        }

        this.locks = new ReentrantLock[capacity];
        for (int i = 0; i < locks.length; i++) {
            this.locks[i] = new ReentrantLock();
        }
    }

    public void put(K key, V value) {
        acquire(key);
        try {
            int keyHash = key.hashCode();
            int myBucket = getPositiveHash(key) % table.length;

            for (Entry<K, V> entrySearch : table[myBucket]) {
                if (entrySearch.key.hashCode() == keyHash && entrySearch.key.equals(key)) {
                    entrySearch.value = value;
                    return;
                }
            }

            Entry<K, V> entry = new Entry(key, value);
            table[myBucket].add(entry);
            mapSize++;
        } finally {
            release(key);
        }

        if (isTimeToResize()) {
            resize();
        }

    }

    public V get(K key) {
        acquire(key);
        try {
            int keyHash = key.hashCode();
            int myBucket = getPositiveHash(key) % table.length;

            for (Entry<K, V> entry : table[myBucket]) {
                if (entry.key.hashCode() == keyHash && entry.key.equals(key)) {
                    return entry.value;
                }
            }

        } finally {
            release(key);
        }

        return null;
    }

    public void remove(K key) {
        acquire(key);
        try {
            int keyHash = key.hashCode();
            int myBucket = getPositiveHash(key) % table.length;

            LinkedList<Entry<K, V>> toRemove = new LinkedList();
            for (Entry<K, V> entry : table[myBucket]) {
                if (entry.key.hashCode() == keyHash && entry.key.equals(key)) {
                    toRemove.add(entry);
                    mapSize--;
                }
            }
            table[myBucket].removeAll(toRemove);

        } finally {
            release(key);
        }
    }

    public boolean isTimeToResize() {
        return mapSize / table.length > 3;
    }

    public void resize() {
        int oldCapacity = table.length;

        for (ReentrantLock lock : locks) {
            lock.lock();
        }

        try {
            if (oldCapacity != table.length) {
                return;
            }

            int newCapacity = 2 * oldCapacity;
            LinkedList<Entry<K, V>>[] oldTable = table;
            table = new LinkedList[newCapacity];

            for (int i = 0; i < newCapacity; i++)
                table[i] = new LinkedList();

            for (LinkedList<Entry<K, V>> bucket : oldTable) {
                for (Entry<K, V> entry : bucket) {
                    table[getPositiveHash(entry.key) % table.length].add(entry);
                }
            }
        } finally {
            for (ReentrantLock lock : locks) {
                lock.unlock();
            }
        }
    }



    public void acquire(K x) {
        locks[getPositiveHash(x) % locks.length].lock();
    }

    public void release(K x) {
        locks[getPositiveHash(x) % locks.length].unlock();
    }

    private int getPositiveHash(K x) {
        return x.hashCode() & 0x7fffffff;
    }
}
