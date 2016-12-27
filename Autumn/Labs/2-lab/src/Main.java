import java.util.concurrent.ExecutionException;

public class Main {
    public static void main(String[] args) throws ExecutionException, InterruptedException {

        System.out.println("Priority process manager started\n");

        ProcessManager priorityProcessManager = new PriorityProcessManager();

        priorityProcessManager.addProcess(new Process(priorityProcessManager, 5));
        priorityProcessManager.addProcess(new Process(priorityProcessManager, 1));
        priorityProcessManager.addProcess(new Process(priorityProcessManager, 15));
        priorityProcessManager.addProcess(new Process(priorityProcessManager, 10));

        priorityProcessManager.start();
        priorityProcessManager.join();


        System.out.println("\nSimple process manager started\n");

        ProcessManager simpleProcessManager = new SimpleProcessManager();

        simpleProcessManager.addProcess(new Process(simpleProcessManager));
        simpleProcessManager.addProcess(new Process(simpleProcessManager));
        simpleProcessManager.addProcess(new Process(simpleProcessManager));
        simpleProcessManager.addProcess(new Process(simpleProcessManager));

        simpleProcessManager.start();
        simpleProcessManager.join();

    }
}