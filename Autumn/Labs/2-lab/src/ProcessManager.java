import co.paralleluniverse.fibers.SuspendExecution;

import java.util.concurrent.ExecutionException;

public interface ProcessManager {
    void switchProcess(Process curProcess) throws SuspendExecution;
    void addProcess(Process process);
    void start();
    void join() throws ExecutionException, InterruptedException;
}
