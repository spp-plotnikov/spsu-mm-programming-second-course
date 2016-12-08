import java.lang.reflect.Array;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.concurrent.Semaphore;

public class SecondExamSystem implements ExamSystem {
    private ArrayList<Semaphore> mutexes;
    private volatile int mutexesUsed;
    private int size;
    int contCount, addCount, remCount;
    private final static int enlargeNumber = 10000;
    private HashMap<Long, ArrayList<Long>> table;
    private HashMap<Long, Integer> idToMutex;


    public SecondExamSystem() {
        contCount = addCount = remCount = 0;
        mutexesUsed = 0;
        idToMutex = new HashMap<>();
        table = new HashMap<>();
        mutexes = new ArrayList<>();
        size = 0;
        enlarge();
    }

    private void enlarge() {
        for (int i = 0; i < enlargeNumber; i++) // 10 will be default size
            mutexes.add(new Semaphore(1));
    }

    public void add(long studentId, long courseId) {
        addCount += 1;
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
            mutexes.get(mutexIdx).acquire();
            ArrayList<Long> list = table.get(studentId);
            if (list == null) {
                ArrayList<Long> newList = new ArrayList<>();
                newList.add(courseId);
                table.put(studentId, newList);
            } else {
                list.add(courseId);
            }
        } catch (InterruptedException e) {
            e.printStackTrace();
        } finally {
            mutexes.get(mutexIdx).release();
        }
    }

    public void remove(long studentId, long courseId) {
        remCount += 1;
        int mutexIdx;
        synchronized (idToMutex) {
            if (!idToMutex.containsKey(studentId)) {
                // System.out.println("No such student: " + studentId);
                return; // nothing to delete
            } else {
                mutexIdx = idToMutex.get(studentId);
            }
        }

        try {
            mutexes.get(mutexIdx).acquire();
            ArrayList<Long> list = table.get(studentId);
            if (list != null) {
                list.remove(courseId);
            }
        } catch (InterruptedException e) {
            e.printStackTrace();
        } finally {
            mutexes.get(mutexIdx).release();
        }
    }

    public boolean contains(long studentId, long courseId) {
        contCount += 1;
        int mutexIdx;
        synchronized (idToMutex) {
            if (!idToMutex.containsKey(studentId))
                return false;
            else
                mutexIdx = idToMutex.get(studentId);
        }

        try {
            mutexes.get(mutexIdx).acquire();
            ArrayList<Long> list = table.get(studentId);
            return !(list == null || !list.contains(courseId));
        } catch (InterruptedException e) {
            e.printStackTrace();
        } finally {
            mutexes.get(mutexIdx).release();
        }

        return false; // unreachable
    }

    public String getStats() {
        return    "Contains queries: " + contCount + "\n"
                + "Add queries: " + addCount + "\n"
                + "Remove queries: " + remCount + "\n"
                + "Total queries: " + (contCount + addCount + remCount);
    }
}
