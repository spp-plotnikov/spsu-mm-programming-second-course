public interface Mapp<K,V> {
    V put(K key, V value);
    V get(K key);
    V remove(K key);
    public boolean containsKey(K key);
}