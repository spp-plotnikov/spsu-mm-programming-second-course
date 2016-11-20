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
        if(lst.size() > 0) {
            synchronized(lst) {
                Runnable r = (Runnable)lst.get(0);
                try {
                    lst.remove(0);
                } catch (NoSuchElementException ex) {
                    return null;
                }
                return r;
            }
        }
        return null;
    }

    public boolean isEmpty() {
        return lst.isEmpty();
    }
}