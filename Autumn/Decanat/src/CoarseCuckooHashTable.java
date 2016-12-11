import java.util.Random;
import java.util.concurrent.locks.Lock;
import java.util.concurrent.locks.ReentrantLock;


public class CoarseCuckooHashTable<K,V> {
    protected Entry<K,V>[][] table;
    protected Lock lock;
    protected int size;
    protected static final int LIMIT = 32;
    private Random random;

    public CoarseCuckooHashTable(int capacity) {
        lock = new ReentrantLock();
        table = (Entry<K, V>[][]) new Entry[2][capacity];
        size = capacity;
        random = new Random();
    }

    private final int fistHashFunc(K key) {
        return Math.abs(key.hashCode() % size);
    }

    private final int secondHashFunc(K key) {
        random.setSeed(key.hashCode());
        return random.nextInt(size);
    }

    public boolean containsKey(K key) {
        lock.lock();
        try {
            if (table[0][fistHashFunc(key)] != null && key.equals(table[0][fistHashFunc(key)].key)) {
                return true;
            } else if (table[1][secondHashFunc(key)] != null && key.equals(table[1][secondHashFunc(key)].key)) {
                return true;
            }
            return false;
        } finally {
            lock.unlock();
        }
    }

    public V put(K key, V value) {
        lock.lock();
        try {
            if (containsKey(key)) {
                if (key.equals(table[0][fistHashFunc(key)].key)) {
                    V result = table[0][fistHashFunc(key)].value;
                    table[0][fistHashFunc(key)].value = value;
                    return result;
                } else if (key.equals(table[1][secondHashFunc(key)].key)) {
                    V result = table[1][secondHashFunc(key)].value;
                    table[1][secondHashFunc(key)].value = value;
                    return result;
                }
            }
            Entry<K,V> e = new Entry<K, V>(key.hashCode(),key,value);
            for (int i = 0; i < LIMIT; i++) {
                e = swap(0, fistHashFunc(key), e);
                if (e == null) {
                    return null;
                }
                e = swap(1, secondHashFunc(key), e);
                if (e == null) {
                    return null;
                }
            }
            return null;
        } finally {
            lock.unlock();
        }
    }

    public V get(K key) {
        lock.lock();
        V result = null;
        int firstHash = fistHashFunc(key);
        int secondHash = secondHashFunc(key);
        try {
            if (table[0][firstHash] != null) {
                result = table[0][firstHash].value;
                return result;
            } else if (table[1][secondHash] != null) {
                result = table[1][secondHash].value;
                return result;
            }
            return result;
        } finally {
            lock.unlock();
        }
    }

    public V remove(K key) {
        lock.lock();
        V result = null;
        int firstHash = fistHashFunc(key);
        int secondHash = secondHashFunc(key);
        try {
            if (key.equals(table[0][firstHash])) {
                result = table[0][firstHash].value;
                table[0][firstHash] = null;
                return result;
            } else if (key.equals(table[1][secondHash])) {
                result = table[1][secondHash].value;
                table[1][secondHash] = null;
                return result;
            }
            return result;
        } finally {
            lock.unlock();
        }
    }

    private Entry<K, V> swap(int i, int j, Entry<K,V> entry) {
        Entry<K,V> e = table[i][j];
        table[i][j] = entry;
        return e;
    }
}