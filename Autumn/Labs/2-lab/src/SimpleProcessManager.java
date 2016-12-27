import co.paralleluniverse.fibers.Fiber;
import co.paralleluniverse.fibers.SuspendExecution;
import co.paralleluniverse.strands.SuspendableRunnable;

import java.util.LinkedList;
import java.util.concurrent.ExecutionException;

public class SimpleProcessManager implements ProcessManager {
    private volatile LinkedList<Process> processesList; // processes queue, processes can leave and enter it during program execution
    private volatile LinkedList<Process> processesStorage; // storage, every added process stays here up to the end of program execution

    SimpleProcessManager() {
        this.processesList = new LinkedList<>();
        this.processesStorage = new LinkedList<>();
    }

    @Override
    public void addProcess(Process process) {
        processesList.add(process);
        processesStorage.add(process);
    }

    @Override
    public void switchProcess(Process curProcess) throws SuspendExecution {
        if (processesList.size() != 0) {
            Process nextProcess = processesList.removeFirst();

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
                processesList.add(curProcess);
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
        if (processesList.size() != 0) {
            Process process = processesList.removeFirst();
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