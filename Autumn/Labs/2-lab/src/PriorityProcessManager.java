import co.paralleluniverse.fibers.Fiber;
import co.paralleluniverse.fibers.SuspendExecution;
import co.paralleluniverse.strands.SuspendableRunnable;

import java.util.LinkedList;
import java.util.concurrent.ExecutionException;

public class PriorityProcessManager implements ProcessManager {
    private volatile IntervalHeap<Process> processesDeque; // processes queue, processes can leave and enter it during program execution
    private volatile LinkedList<Process> processesStorage; // every added process stays here up to the end of program execution
    private volatile byte counter;

    PriorityProcessManager() {
        this.processesDeque = new IntervalHeap<>();
        this.processesStorage = new LinkedList<>();
        this.counter = 20;
    }

    @Override
    public void addProcess(Process process) {
        processesDeque.add(process);
        processesStorage.add(process);
    }

    @Override
    public void switchProcess(Process curProcess) throws SuspendExecution {
        if (processesDeque.size() != 0) {
            Process nextProcess;

            if (--counter == 0) { // for processes with low priority
                nextProcess = processesDeque.dequeueMax();
                counter = 20;
            } else { // for processes with high priority
                nextProcess = processesDeque.dequeueMin();
            }

            new Fiber<Void>((SuspendableRunnable) () -> {
                while (curProcess.getProcessState() == 1) { } // switch only when curProcess not executing

                if (nextProcess.getProcessState() == 0) { // if not started, only initialised
                    nextProcess.setProcessState(1);
                    nextProcess.start();
                } else if (nextProcess.getProcessState() == 2) { // if parked
                    nextProcess.setProcessState(1);
                    nextProcess.unpark();
                }
            }).start();


            if (curProcess.getProcessState() != 3) { // if not finished
                curProcess.setProcessState(2);
                processesDeque.add(curProcess);
                Fiber.park();
            }

        } else {
            if (curProcess.getProcessState() == 3) { // if finished
                System.out.println("Nothing to switch! Exiting...");
            }
        }
    }

    @Override
    public void start() {
        if (processesDeque.size() != 0) {
            Process process = processesDeque.dequeueMin();
            process.setProcessState(1);
            process.start();
        }
    }

    @Override
    public void join() throws ExecutionException, InterruptedException {
        for (Process process : processesStorage) {
            process.join();
        }
    }
}