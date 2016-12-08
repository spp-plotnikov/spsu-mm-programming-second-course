import java.util.ArrayList;
import java.util.HashMap;

public class FirstExamSystem implements ExamSystem {
    private HashMap<Long, ArrayList<Long>> table;
    int contCount, addCount, remCount;

    public FirstExamSystem() {
        contCount = addCount = remCount = 0;
        table = new HashMap<>();
    }

    public void add(long studentId, long courseId) {
        addCount += 1;
        synchronized (table) {
            ArrayList<Long> list = table.get(studentId);
            if (list == null) {
                list = new ArrayList<>();
                list.add(courseId);
                table.put(studentId, list);
            } else {
                if (!list.contains(courseId))
                    list.add(courseId);
            }
        }
    }

    public void remove(long studentId, long courseId) {
        remCount += 1;
        synchronized (table) {
            ArrayList<Long> list = table.get(studentId);
            if (list != null) {
                if (list.contains(courseId))
                    list.remove(courseId);
            }
        }
    }

    public boolean contains(long studentId, long courseId) {
        contCount += 1;
        synchronized (table) {
            ArrayList<Long> list = table.get(studentId);
            return !(list == null || !list.contains(courseId));
        }
    }

    public String getStats() {
        return    "Contains queries: " + contCount + "\n"
                + "Add queries: " + addCount + "\n"
                + "Remove queries: " + remCount + "\n"
                + "Total queries: " + (contCount + addCount + remCount);
    }
}
