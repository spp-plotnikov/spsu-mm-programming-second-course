import java.util.ArrayList;
import java.util.HashMap;
import java.util.concurrent.Semaphore;

public class SecondExamSystem implements ExamSystem {
    private Semaphore[] mutexes;
    private volatile int mutexesUsed;
    private HashMap<Long, ArrayList<Long>> table;
    private HashMap<Long, Integer> idToMutex;

    public SecondExamSystem(int maxSize) {
        mutexesUsed = 0;
        idToMutex = new HashMap<>(maxSize);
        table = new HashMap<>(maxSize);
        mutexes = new Semaphore[maxSize];
        for (int i = 0; i < maxSize; i++)
            mutexes[i] = new Semaphore(1);
    }

    public void add(long studentId, long courseId) {
        int mutexIdx;
        synchronized (idToMutex) {
            if (!idToMutex.containsKey(studentId)) {
                mutexIdx = mutexesUsed;
                idToMutex.put(studentId, mutexIdx);
                mutexesUsed += 1;
            } else {
                mutexIdx = idToMutex.get(studentId);
            }
        }

        try {
            mutexes[mutexIdx].acquire();
            ArrayList<Long> list = table.get(studentId);
            if (list == null) {
                ArrayList<Long> newList = new ArrayList<>();
                newList.add(courseId);
                table.put(studentId, newList);
            } else {
                if (!list.contains(courseId))
                    list.add(courseId);
            }
        } catch (InterruptedException e) {
            e.printStackTrace();
        } finally {
            mutexes[mutexIdx].release();
        }
    }

    public void remove(long studentId, long courseId) {
        int mutexIdx;
        synchronized (idToMutex) {
            if (!idToMutex.containsKey(studentId)) {
                System.out.println("No such student: " + studentId);
                return; // nothing to delete
            } else {
                mutexIdx = idToMutex.get(studentId);
            }
        }

        try {
            mutexes[mutexIdx].acquire();
            ArrayList<Long> list = table.get(studentId);
            if (list == null) {
                System.out.println("No records for student: " + studentId);
            } else {
                if (!list.contains(courseId))
                    System.out.println("No such record: " + studentId + " course: " + courseId);
                else
                    list.remove(courseId);
            }
        } catch (InterruptedException e) {
            e.printStackTrace();
        } finally {
            mutexes[mutexIdx].release();
        }
    }

    public boolean contains(long studentId, long courseId) {
        int mutexIdx;
        synchronized (idToMutex) {
            if (!idToMutex.containsKey(studentId))
                return false; // nothing to delete
            else
                mutexIdx = idToMutex.get(studentId);
        }

        try {
            mutexes[mutexIdx].acquire();
            return table.get(studentId).contains(courseId);
        } catch (InterruptedException e) {
            e.printStackTrace();
        } finally {
            mutexes[mutexIdx].release();
        }

        return false; // unreachable
    }
}
