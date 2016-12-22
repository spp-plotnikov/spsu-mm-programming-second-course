import co.paralleluniverse.fibers.Fiber;
import co.paralleluniverse.fibers.SuspendExecution;
import co.paralleluniverse.strands.SuspendableRunnable;
import co.paralleluniverse.strands.Strand;
import java.util.*;
import java.util.concurrent.ExecutionException;
import java.util.concurrent.ThreadLocalRandom;

/* краткое описание кода
                                           $"   *.                      ---------
               d$$$$$$$P"                  $    J                       |       |
                   ^$.                     4r  "                        |       |
                   d"b                    .db                           |       |
                  P   $                  e" $                           |       |
         ..ec.. ."     *.              zP   $.zec..                     |       |
     .^        3*b.     *.           .P" .@"4F      "4                  |       |
   ."         d"  ^b.    *c        .$"  d"   $         %                _+_+_+_+_
  /          P      $.    "c      d"   @     3r         3               |       |
 4        .eE........$r===e$$$$eeP    J       *..        b              |       |
 $       $$$$$       $   4$$$$$$$     F       d$$$.      4              |       |
 $       $$$$$       $   4$$$$$$$     L       *$$$"      4              |       |
 4         "      ""3P ===$$$$$$"     3                  P               |     |
  *                 $       """        b                J                 |   |
   ".             .P                    %.             @                   | |
     %.         z*"                      ^%.        .r"                     #
        "*==*""                             ^"*==*""                        #
                                                                            #
*/

public class ProcessManager {
    private volatile Queue<Process> queue;
    private int countProcess;
    private boolean[] used;
    private boolean finish;
    Process[] processes;

    public ProcessManager(int count) {
        countProcess = count;
        queue = new LinkedList<>();
        processes = new Process[countProcess];
        used = new boolean[countProcess];
        for (int i = 0; i < countProcess; i++) {
            processes[i] = new Process(this, ThreadLocalRandom.current().nextInt(1, 10));
        }
        for (int i = 0; i < countProcess; i++) {
            for (int j = i + 1; j < countProcess; j++) {
                if (processes[i].priority < processes[j].priority) {
                    Process tmp = processes[i];
                    processes[i] = processes[j];
                    processes[j] = tmp;
                }
            }
        }
        for (int i = 0; i < countProcess; i++) {
            queue.add(processes[i]);
            processes[i].start();
        }
    }

    public boolean NoPrioritySwitch(Process current, boolean isFinished) throws SuspendExecution {
        if (queue.isEmpty()) {
            return true;
        }
        Process next = queue.poll();
        if (!isFinished) {
            queue.add(current);
        }
        if (current == next) {
            if (isFinished) {
                next = queue.poll();
            }
        }
        _switch(current, next);

        if (isFinished) {
            return false;
        }
        Fiber.park();
        return false;
    }

    public boolean PrioritySwitch(Process current, boolean isFinished) throws SuspendExecution {
        if (isFinished) {
            finish = true;
        }
        Process next = null;
        if (!finish) {
            next = processes[0];
            processes[0].priority--;
            for (int i = 1; i < countProcess; i++) {
                if (processes[i - 1].priority < processes[i].priority) {
                    Process tmp = processes[i];
                    processes[i] = processes[i - 1];
                    processes[i - 1] = tmp;
                }
            }
        } else {
            for (int i = 0; i < countProcess; i++) {
                if (processes[i] == current) {
                    used[i] = true;
                }
            }
            for (int i = 0; i < countProcess; i++) {
                if (!used[i]) {
                    used[i] = true;
                    next = processes[i];
                    _switch(current, next);
                    return true;
                }
            }
            if (next == null) {
                return true;
            }
        }
        _switch(current, next);
        Fiber.park();
        return false;
    }

    private void _switch(Process current, Process next) {
        new Fiber<Void>((SuspendableRunnable) () -> {
            while (current.getState() == Strand.State.RUNNING) { }
            next.unpark();
        }).start();
    }

    public void go() {
        Process next = queue.peek();
        next.unpark();
    }

    public void join() throws ExecutionException, InterruptedException {
        for (int i = 0; i < countProcess; i++) {
            processes[i].join();
        }
        System.out.println("All fibers finished");
    }
}