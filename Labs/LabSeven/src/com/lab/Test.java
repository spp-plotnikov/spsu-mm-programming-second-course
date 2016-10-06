package com.lab;

import java.io.IOException;
import java.util.ArrayList;
import java.util.Random;

/**
 * Created by Katrin on 05.10.2016.
 */
public class Test {
    private static int COUNT_OF_TASK = 100;
    private static ArrayList<Thread> arrayList = new ArrayList<>();
    private static int COUNT_OF_ADDS = (int) (COUNT_OF_TASK * 0.09);
    private static int COUNT_OF_CONTAINS = (int) (COUNT_OF_TASK * 0.9);
    private static int COUNT_OF_REMOVES = (int) (COUNT_OF_TASK * 0.01);


    public static void main(String[] args) throws IOException {
        doTest("one", "6666");
        doTest("two", "9999");


    }

    public static void doTest(String param, String port) throws IOException {

        long startTime = System.currentTimeMillis();
        final String[] strs = new String[]{param, port};
        new Thread(new Runnable() {
            @Override
            public void run() {
                try {
                    StarterForSystem.main(strs);
                } catch (IOException e) {
                    e.printStackTrace();
                }
            }
        }).start();

        for (int i = 0; i < COUNT_OF_ADDS; i++) {
            int idStudent = new Random(99999999).nextInt() + 1;
            int idCourse = new Random(99999999).nextInt() % 6 + 1;
            Thread thread = new ClientForTest("add " + idStudent + " " + idCourse + " accepted", strs[1]);
            arrayList.add(thread);
            thread.start();
        }

        for (int i = 0; i < COUNT_OF_CONTAINS / 2; i++) {
            Thread thread = new ClientForTest("contains " + i + " " + ((i % 6) + 1), strs[1]);
            arrayList.add(thread);
            thread.start();
        }

        for (int i = 0; i < COUNT_OF_REMOVES; i++) {
            int idStudent = new Random(99999999).nextInt() + 1;
            int idCourse = new Random(99999999).nextInt() % 6 + 1;
            Thread thread = new ClientForTest("remove " + idStudent + " " + idCourse, strs[1]);
            arrayList.add(thread);
            thread.start();

        }

        for (int i = 0; i < COUNT_OF_CONTAINS / 2; i++) {
            Thread thread = new ClientForTest("contains " + i + " " + ((i % 6) + 1), strs[1]);
            arrayList.add(thread);
            thread.start();
        }

        for (Thread thread : arrayList) {
            try {
                thread.join();
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
        }

        StarterForSystem.close();

        System.out.println(System.currentTimeMillis() - startTime);


    }
}
