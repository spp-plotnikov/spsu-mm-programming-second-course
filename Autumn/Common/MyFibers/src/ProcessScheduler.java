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
    private volatile TreeSet<Process> prQ;
    private volatile Queue<Process> regQ;
    private volatile Process next;
    public volatile AtomicBoolean done;
    public boolean isPriorityMode;

    public ProcessScheduler(boolean mode, int n) {
        done = new AtomicBoolean(false);
        isPriorityMode = mode;
        prQ = new TreeSet<>(new ProcessComparator());
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
        if (isPriorityMode) {
            // All other fibers are already stopped!
            if (prQ.isEmpty() || prQ.size() == 1 && prQ.first() == cur)
                return true;
        } else {
            if (regQ.isEmpty() || regQ.size() == 1 && regQ.peek() == cur)
                return true;
        }

        // Figuring out who's next...
        do {
            if (isPriorityMode) {
                Random generator = new Random();
                int rand = generator.nextInt(10);
                if (rand < 3) // 30%
                    next = prQ.last();
                else
                    next = prQ.first();
                prQ.remove(next);
                System.out.println("removed " + next);
            } else
                next = regQ.poll();
        } while (next == cur);

        // If the current fiber hasn't finished yet, forward it
        if (!isFinished) {
            System.out.println("adding " + cur);
            if (isPriorityMode)
                if (!prQ.contains(cur))
                    prQ.add(cur);
            else
                if (!regQ.contains(cur))
                    regQ.add(cur);
        }

        System.out.println(next);
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
        if (isPriorityMode) {
            next = prQ.first();
            prQ.remove(next);
        }
        else
            next = regQ.poll();
        next.unpark();
    }

    public void join() throws Exception {
        Process cur;
        for (;;) {
            if (isPriorityMode) {
                if (!prQ.isEmpty())
                    cur = prQ.first();
                else
                    cur = null;
            }
            else
                cur = regQ.peek();
            if (done.get())
                break;
            if (cur != null) // otherwise we have 1 last fiber running
                cur.join();
        }
    }
}
