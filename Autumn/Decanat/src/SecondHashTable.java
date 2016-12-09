import java.util.LinkedList;
import java.util.List;

public class SecondHashTable implements IExamSystem {
    private Mapp<Long, List<Long>> hashMap;
    private int countStudents;

    SecondHashTable(int countStudents) {
        this.countStudents = countStudents;
        hashMap = new CoarseCuckooHashTable(countStudents);
    }

    public void Add(long studentId, long courseId) {
        List<Long> list = hashMap.get(studentId);
        if (list != null) {
            if (!list.contains(courseId)) {
                list.add(courseId);
            }
        } else {
            list = new LinkedList();
            list.add(courseId);
            hashMap.put(studentId, list);
        }
    }


    public void Remove(long studentId, long courseId) {
        List<Long> list = hashMap.get(studentId);
        if (list != null) {
            if (list.contains(courseId)) {
                list.remove(courseId);
            }
        }
    }

    public boolean Contains(long studentId, long courseId) {
        List<Long> list = hashMap.get(studentId);
        if (list != null) {
            return list.contains(courseId);
        } else {
            return false;
        }
    }
}
