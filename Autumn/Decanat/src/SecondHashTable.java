import java.util.concurrent.CopyOnWriteArrayList;
import java.util.concurrent.locks.ReentrantLock;

public class SecondHashTable implements IExamSystem {
    private RefinableHashTable<Long, CopyOnWriteArrayList> hashMap;
    private int countStudents;
    ReentrantLock lock;

    SecondHashTable(int countStudents) {
        this.countStudents = countStudents;
        lock = new ReentrantLock();
        hashMap = new RefinableHashTable(countStudents);
    }

    public void add(long studentId, long courseId) {
        lock.lock();
        CopyOnWriteArrayList<Long> list = hashMap.get(studentId);
        if (list == null) {
            list = new CopyOnWriteArrayList();
            list.add(courseId);
            hashMap.put(studentId, list);
        }
        lock.unlock();
        if (!list.contains(courseId)) {
            list.add(courseId);
        }
    }

    public void remove(long studentId, long courseId) {
        CopyOnWriteArrayList<Long> list = hashMap.get(studentId);
        list.remove(courseId);
    }

    public boolean contains(long studentId, long courseId) {
        CopyOnWriteArrayList<Long> list = hashMap.get(studentId);
        if (list == null) {
            return false;
        }
        return  list.contains(courseId);
    }
}
