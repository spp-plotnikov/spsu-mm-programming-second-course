public class Main {
    final static int size = 10000000; // if crashes, it means we have to enlarge it
    final static int count = 10;

    public static void main(String[] args) throws Exception {
        ExamSystem systems[] = { new FirstExamSystem(size), new SecondExamSystem(size) };
        for (ExamSystem es: systems) {
            System.out.println("System type: " + es.getClass());
            long startTime = System.currentTimeMillis();
            Thread threads[] = new Thread[count];
            for (int i = 0; i < count; i++) {
                AngryRectorate r = new AngryRectorate(es);
                threads[i] = new Thread(r);
                threads[i].start();
            }
            for (Thread t: threads)
                t.join();
            System.out.println(es.getStats());
            System.out.println("== Elapsed: " + (System.currentTimeMillis() - startTime) + "ms ==");
        }
    }
}
