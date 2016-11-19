import co.paralleluniverse.fibers.Fiber;
import co.paralleluniverse.fibers.SuspendExecution;
import java.util.concurrent.ThreadLocalRandom;

public class Process extends Fiber<Void> {
    private ProcessScheduler scheduler;
    public int priority;

    public Process(ProcessScheduler shed, int priority) {
        scheduler = shed;
        this.start();
        this.priority = priority;
    }

    protected Void run() throws SuspendExecution, InterruptedException {
        System.out.println(Fiber.currentFiber().getId() + " has started! Parking...");
        Fiber.park();
        int total = 10, i = 0;
        boolean flag = false;
        while (i < total) {
            //Fiber.sleep(1000);
            Fiber.sleep(ThreadLocalRandom.current().nextInt(100, 300)); // work emulation
            long delta = ThreadLocalRandom.current().nextInt(50, 100);
            long start = System.currentTimeMillis();

            // IO emulation
            while (System.currentTimeMillis() - start < delta) {
                if (flag)
                    continue; // other fibers are done
                System.out.println(Fiber.currentFiber().getId() + " is going to switch now...");
                flag = scheduler.switchProcess(this, false);
            }
            i++;
        }

        System.out.println(Fiber.currentFiber().getId() + " has finished");

        // check if it's the last fiber and it's done
        if (scheduler.switchProcess(this, true) == true)
            scheduler.done.set(true);
        return null;
    }
}