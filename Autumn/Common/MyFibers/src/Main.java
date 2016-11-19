public class Main {

    static int procN = 5;

    public static void main(String[] args) throws Exception {
        // Here second argument determines the strategy being used!
        ProcessScheduler shed = new ProcessScheduler(true, procN);
        shed.activate();
        shed.join();
    }
}
