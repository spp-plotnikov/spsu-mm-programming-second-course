import java.util.LinkedList;
import java.util.List;

class TaskQueue {

    private final List lst;

    public TaskQueue() {
        lst = new LinkedList();
    }

    public void add(Runnable r) {
        lst.add(r);
    }

    public Runnable pop() {
        if(lst.size() > 0) {
            synchronized(lst) {
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