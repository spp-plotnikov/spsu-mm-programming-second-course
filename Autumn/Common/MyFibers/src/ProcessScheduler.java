import co.paralleluniverse.fibers.Fiber;
import co.paralleluniverse.fibers.SuspendExecution;
import co.paralleluniverse.strands.SuspendableRunnable;
import co.paralleluniverse.strands.Strand;
import java.util.*;
import java.util.concurrent.ThreadLocalRandom;
import java.util.concurrent.atomic.AtomicBoolean;

class ProcessComparator implements Comparator<Process> {
    public int compare(Process a, Process b) {
        if (a.equals(b))
            return 0;
        if (a.priority < b.priority)
            return -1;
        return 1;
    }
}

public class ProcessScheduler {
    private PriorityQueue<Process> prQ;
    private Queue<Process> regQ;
    private volatile Process next, cur;
    public volatile AtomicBoolean done;
    public boolean isPriorityMode;

    public ProcessScheduler(boolean mode, int n) {
        done = new AtomicBoolean(false);
        isPriorityMode = mode;
        prQ = new PriorityQueue<>(new ProcessComparator());
        regQ = new LinkedList<>();

        for (int i = 0; i < n; i++) {
            Process p = new Process(this, ThreadLocalRandom.current().nextInt(1, 10));
            prQ.add(p);
            regQ.add(p);
        }
    }

    // isFinished = true if the current fiber has just finished and no longer needs to be resumed
    // returns true if all other fibers are finished
    public boolean switchProcess(Process cur, boolean isFinished) throws SuspendExecution, InterruptedException {
        // Figuring out who's next...
        if (isPriorityMode)
            next = prQ.poll();
        else
            next = regQ.poll();
        if (next == null) {
            // All other fibers are already stopped!
            return true;
        }

        // If the current fiber hasn't finished yet, forward it
        if (!isFinished) {
            if (isPriorityMode)
                prQ.add(cur);
            else
                regQ.add(cur);
        }

        // To avoid non-atomic switch!
        new Fiber<Void>((SuspendableRunnable) () -> {
            while (cur.getState() == Strand.State.RUNNING) { }
                next.unpark();
        }).start();

        if (isFinished)
            return false;
        System.out.println(cur.getId() + " parked");
        Fiber.park();
        return false;
    }

    public void activate() {
        if (isPriorityMode)
            next = prQ.poll();
        else
            next = regQ.poll();
        next.unpark();
    }

    public void join() throws Exception {
        Process cur = null;
        for (;;) {
            if (isPriorityMode)
                cur = prQ.peek();
            else
                cur = regQ.peek();
            if (done.get())
                break;
            if (cur != null) // otherwise we have 1 last fiber running
                cur.join();
        }
    }
}
