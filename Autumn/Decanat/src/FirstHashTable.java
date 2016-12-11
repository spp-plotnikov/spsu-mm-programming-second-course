import java.util.*;

public class FirstHashTable implements IExamSystem {
    private Map<Long, List<Long>> hashMap;
    private int countStudents;
    FirstHashTable(int countStudents) {
        this.countStudents = countStudents;
        hashMap = new HashMap(countStudents);
    }

    public void add(long studentId, long courseId) {
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

    public void remove(long studentId, long courseId) {
        synchronized (hashMap) {
            if (hashMap.containsKey(studentId)) {
                List<Long> list = hashMap.get(studentId);
                if (list.contains(courseId)) {
                    list.remove(courseId);
                }
            }
        }
    }

    public boolean contains(long studentId, long courseId) {
        synchronized (hashMap) {
            if (hashMap.containsKey(studentId)) {
                List<Long> list = hashMap.get(studentId);
                return list.contains(courseId);
            } else {
                return false;
            }
        }
    }
}
