public class Main {

    public static void main(String[] args) throws Exception {
        //FirstExamSystem f = new FirstExamSystem();
        SecondExamSystem es = new SecondExamSystem(500000);
        Deanery d = new Deanery(es);
        Thread t = new Thread(d);
        t.start();
        t.join();
    }
}
