import java.util.HashMap;
import java.util.HashSet;

public class MoreSimpleRealisation implements IExamSystem {
    private HashMap<Long, HashSet<Long>> storage;

    public MoreSimpleRealisation() {
        this.storage = new HashMap();
    }

    @Override
    public void add(long studentId, long courseId) {
        synchronized (storage) {
            HashSet courses = storage.get(studentId);
            if (courses != null) {
                courses.add(courseId);
            } else {
                courses = new HashSet();
                courses.add(courseId);
                storage.put(studentId, courses);
            }
        }
    }

    @Override
    public void remove(long studentId, long courseId) {
        synchronized (storage) {
            HashSet courses = storage.get(studentId);
            if (courses != null) {
                courses.remove(courseId);
                if (courses.isEmpty()) {
                    storage.remove(studentId);
                }
            }
        }
    }

    @Override
    public boolean contains(long studentId, long courseId) {
        synchronized (storage) {
            HashSet courses = storage.get(studentId);
            if (courses != null) {
                return courses.contains(courseId);
            } else {
                return false;
            }
        }
    }
}