import java.util.ArrayList;
import java.util.concurrent.locks.ReentrantLock;

public class SecondHashTable implements IExamSystem {
    private RefinableHashTable<Long, ArrayList> hashMap;
    private int countStudents;
    ReentrantLock lock;

    SecondHashTable(int countStudents) {
        this.countStudents = countStudents;
        lock = new ReentrantLock();
        hashMap = new RefinableHashTable(countStudents);
    }

    public void add(long studentId, long courseId) {
        lock.lock();
        ArrayList<Long> list = hashMap.get(studentId);
        if (list == null) {
            list = new ArrayList();
            list.add(courseId);
            hashMap.put(studentId, list);
        }
        lock.unlock();
        synchronized (list) {
            if (!list.contains(courseId)) {
                list.add(courseId);
            }
        }
    }

    public void remove(long studentId, long courseId) {
        ArrayList<Long> list = hashMap.get(studentId);
        synchronized (list) {
            list.remove(courseId);
        }
    }

    public boolean contains(long studentId, long courseId) {
        ArrayList<Long> list = hashMap.get(studentId);
        if (list == null) {
            return false;
        }
        synchronized (list) {
            return list.contains(courseId);
        }
    }
}
