import sun.awt.Mutex;

import java.util.LinkedList;
import java.util.List;

public class SecondHashTable implements IExamSystem {
    private CoarseCuckooHashTable<Long, List<Long>> hashMap;
    private int countStudents;
    Mutex mutex;

    SecondHashTable(int countStudents, Mutex mutex) {
        this.countStudents = countStudents;
        this.mutex = mutex;
        hashMap = new CoarseCuckooHashTable(countStudents);
    }

    public void add(long studentId, long courseId) {
        List<Long> list = hashMap.get(studentId);
        if (list != null) {
            if (!list.contains(courseId)) {
                list.add(courseId);
            }
        } else {
            mutex.lock();
            list = hashMap.get(studentId);
            if (list == null) {
                list = new LinkedList();
                list.add(courseId);
                hashMap.put(studentId, list);
            } else {
                if (!list.contains(courseId)) {
                    list.add(courseId);
                }
            }
            mutex.unlock();
        }
    }

    public void remove(long studentId, long courseId) {
        List<Long> list = hashMap.get(studentId);
        if (list != null) {
            if (list.contains(courseId)) {
                list.remove(courseId);
            }
        }
    }

    public boolean contains(long studentId, long courseId) {
        List<Long> list = hashMap.get(studentId);
        if (list != null) {
            return list.contains(courseId);
        } else {
            return false;
        }
    }
}
