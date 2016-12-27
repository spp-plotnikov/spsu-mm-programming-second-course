import co.paralleluniverse.fibers.Fiber;
import co.paralleluniverse.fibers.SuspendExecution;
import java.util.concurrent.ThreadLocalRandom;


public class Process extends Fiber<Void> implements Comparable<Process> {
    private int priority;
    private int processState; //0: initialised, 1: started/executing/unparked, 2: parked/suspended, 3: finished;
    private ProcessManager processManager;

    public Process(ProcessManager processManager, int priority) {
        this.processManager = processManager;
        this.priority = priority;
        this.processState = 0;
    }

    public Process(ProcessManager processManager) { //non-priority process constructor
        this.processManager = processManager;
        this.priority = 0;
        this.processState = 0;
    }

    public int getPriority() {
        return priority;
    }

    public int getProcessState() {
        return processState;
    }

    public void setProcessState(int state) {
        processState = state;
    }

    @Override
    public int compareTo(Process process) {
        return this.getPriority() - process.getPriority();
    }

    protected Void run() throws SuspendExecution, InterruptedException {
        System.out.println("Process id: " + Process.currentFiber().getId() +
                " process priority: " + this.getPriority());

        int timesRepeat = ThreadLocalRandom.current().nextInt(1, 4);
        for (int i = 0; i < timesRepeat; i++) {
            Process.sleep(1000); // our process "work". Processes like to sleep, i'm too

            int ioTime = ThreadLocalRandom.current().nextInt(10, 100);
            long start = System.currentTimeMillis();

            /*
             * "making IO request"
             * uhh, IO ops are so slow. We can't waste time here,
             * lets notify manager that it could switch to another process
            */

            while (System.currentTimeMillis() - start < ioTime) {
                // io response is not ready, switch one more time
                System.out.println("IO response #" + i + " not ready in process id: " + Process.currentFiber().getId());
                processManager.switchProcess(this);
            }

            System.out.println("IO response #" + i + " READY in process id: " + Process.currentFiber().getId());
        }

        setProcessState(3);
        System.out.println("Process id: " + Process.currentFiber().getId() + " has finished");
        processManager.switchProcess(this);


        return null; // specific Quasar return for Void
    }
}
