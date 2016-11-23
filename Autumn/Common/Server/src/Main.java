public class Main {
    public static void main(String[] args) throws InterruptedException {
        Server server = new Server(args);
        Thread thread = new Thread(server);
        thread.start();

        Thread.sleep(1000);

        //server.stop();
        try {
            thread.join();
        } catch (InterruptedException e) {
            System.out.println("Stopping server...");
            server.stop();
        }
    }
}
