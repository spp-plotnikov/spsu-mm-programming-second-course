import java.util.LinkedList;
import java.util.List;
import java.util.NoSuchElementException;

class TaskQueue {

    private final List lst;

    public TaskQueue() {
        lst = new LinkedList();
    }

    public void add(Runnable r) {
        synchronized (lst) {
            lst.add(r);
        }
    }

    public Runnable pop() {
        synchronized(lst) {
            if(lst.size() > 0) {
                Runnable r = (Runnable)lst.get(0);
                lst.remove(0);
                return r;
            }
        }
        return null;
    }

    public boolean isEmpty() {
        return lst.isEmpty();
    }
}