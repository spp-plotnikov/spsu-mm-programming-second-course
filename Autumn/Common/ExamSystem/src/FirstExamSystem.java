import java.util.ArrayList;
import java.util.HashMap;

public class FirstExamSystem implements ExamSystem {
    private HashMap<Long, ArrayList<Long>> table;

    public FirstExamSystem(int maxSize) {
        table = new HashMap<>(maxSize);
    }

    public void add(long studentId, long courseId) {
        synchronized (table) {
            ArrayList<Long> list = table.get(studentId);
            if (list == null) {
                list = new ArrayList<>();
                list.add(courseId);
                table.put(studentId, list);
            } else {
                list.add(courseId);
            }
        }
    }

    public void remove(long studentId, long courseId) {
        synchronized (table) {
            ArrayList<Long> list = table.get(studentId);
            if (list == null)
                System.out.println("No records for student: " + studentId);
            else {
                if (!list.contains(courseId))
                    System.out.println("No such record: " + studentId + " course: " + courseId);
                else
                    list.remove(courseId);
            }
        }
    }

    public boolean contains(long studentId, long courseId) {
        synchronized (table) {
            ArrayList<Long> list = table.get(studentId);
            return !(list == null || !list.contains(courseId));
        }
    }
}
