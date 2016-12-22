public class Main {
    public static void main(String[] args) throws Exception {
        ProcessManager shed = new ProcessManager(5);
        shed.go();
        shed.join();
    }
}