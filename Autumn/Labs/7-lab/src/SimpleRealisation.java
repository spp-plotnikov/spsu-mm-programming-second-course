import java.util.HashSet;

public class SimpleRealisation implements IExamSystem {
    private MyStrippedHashMap<Long, HashSet<Long>> storage;
    private final int INITIAL_CAPACITY = 100;

    SimpleRealisation() {
        this.storage = new MyStrippedHashMap(INITIAL_CAPACITY);
    }

    @Override
    public void add(long studentId, long courseId) {
        HashSet courses = storage.get(studentId);
        synchronized (courses) {
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
        HashSet courses = storage.get(studentId);
        synchronized (courses) {
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
        HashSet courses = storage.get(studentId);
        synchronized (courses) {
            if (courses != null) {
                return courses.contains(courseId);
            } else {
                return false;
            }
        }
    }
}
