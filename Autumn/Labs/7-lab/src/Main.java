public class Main {
    public static void main(String[] args) {
        IExamSystem moreSimpleStorage = new MoreSimpleRealisation();
        IExamSystem simpleStorage = new SimpleRealisation();

        TestingSystem ts = new TestingSystem(moreSimpleStorage, 20);
        System.out.println("Time for moreSimpleStorage: " + ts.start());


        ts = new TestingSystem(simpleStorage, 20);
        System.out.println("Time for simpleStorage: " + ts.start());
    }
}
