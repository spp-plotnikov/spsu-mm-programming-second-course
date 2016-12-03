import java.util.*;

public class FirstHashTable implements IExamSystem {
    private Map<Long, List<Long>> hashMap;

    FirstHashTable() {
        hashMap = new HashMap();
    }

    public void Add(long studentId, long courseId) {
        synchronized (hashMap) {
            if (hashMap.containsKey(studentId)) {
                List<Long> list = hashMap.get(studentId);
                if (!list.contains(courseId)) {
                    list.add(courseId);
                }
            } else {
                List<Long> list = new LinkedList();
                list.add(courseId);
                hashMap.put(studentId, list);
            }
        }
    }

    public void Remove(long studentId, long courseId) {
        synchronized (hashMap) {
            if (hashMap.containsKey(studentId)) {
                List<Long> list = hashMap.get(studentId);
                if (list.contains(courseId)) {
                    list.remove(courseId);
                }
            }
        }
    }

    public boolean Contains(long studentId, long courseId) {
        synchronized (hashMap) {
            if (hashMap.containsKey(studentId)) {
                List<Long> list = hashMap.get(studentId);
                return list.contains(courseId);
            } else {
                return false;
            }
        }
    }

    public void print() {
        for (Long id : hashMap.keySet() ) {
            List<Long> list = hashMap.get(id);
            System.out.print(id + ": ");
            for (Long courseId : list) {
                System.out.print(courseId + " ");
            }
            System.out.println();
        }
    }
}
