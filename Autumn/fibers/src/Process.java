import co.paralleluniverse.fibers.Fiber;
import co.paralleluniverse.fibers.SuspendExecution;
import java.util.concurrent.ThreadLocalRandom;

public class Process extends Fiber<Void> {
    private ProcessManager processManager;
    public int priority;

    public Process(ProcessManager processManager, int priority) {
        this.processManager = processManager;
        this.priority = priority;
    }

    protected Void run() throws SuspendExecution, InterruptedException {
        System.out.println("Fiber id: " + Fiber.currentFiber().getId() + " process priority: " + this.priority);
        Fiber.park();
        boolean flag = false;
        for (int i = 0; i < 10; i++) {
            Fiber.sleep(ThreadLocalRandom.current().nextInt(100, 300));
            long delta = ThreadLocalRandom.current().nextInt(50, 100);
            long start = System.currentTimeMillis();
            do {
                if (flag) {
                    continue;
                }
                flag = processManager.noPrioritySwitch(this, false);
            } while (System.currentTimeMillis() - start < delta);
        }

        System.out.println(Fiber.currentFiber().getId() + " has finished");

        processManager.noPrioritySwitch(this, true);
        return null;
    }
}