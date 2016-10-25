package com.lab.test;

import com.lab.system.StarterForSystem;

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

        doTest("one");
        doTest("two");
        doTest("two");
        doTest("two");
        doTest("two");
        doTest("two");
        doTest("two");
        doTest("two");
        doTest("two");
        doTest("two");
        doTest("two");
        doTest("two");
        doTest("two");

    }

    private static void doTest(String param) throws IOException {

        long startTime = System.currentTimeMillis();
        final String[] strs = new String[]{param};

        Thread systemThread = new Thread(new Runnable() {
            @Override
            public void run() {
                try {
                    StarterForSystem.main(strs);
                } catch (IOException e) {
                    e.printStackTrace();
                }
            }
        });

        systemThread.start();

        Random random = new Random();
        int idStudent;
        int idCourse;



        for (int i = 0; i < COUNT_OF_ADDS; i++) {
            idStudent = random.nextInt(99999999) + 1;
            idCourse = random.nextInt(999999) % 6 + 1;
            Thread thread = new ClientForTest("add " + idStudent + " " + idCourse + " accepted");
            arrayList.add(thread);
            thread.start();
        }

        for (int i = 0; i < COUNT_OF_CONTAINS / 2; i++) {
            Thread thread = new ClientForTest("contains " + i + " " + ((i % 6) + 1));
            arrayList.add(thread);
            thread.start();
        }

        for (int i = 0; i < COUNT_OF_REMOVES; i++) {
            idStudent = random.nextInt(99999999) + 1;
            idCourse = random.nextInt(999999) % 6 + 1;
            Thread thread = new ClientForTest("remove " + idStudent + " " + idCourse);
            arrayList.add(thread);
            thread.start();

        }

        for (int i = 0; i < COUNT_OF_CONTAINS / 2; i++) {
            Thread thread = new ClientForTest("contains " + i + " " + ((i % 6) + 1));
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

        try {
            systemThread.join();
        } catch (InterruptedException e) {
            e.printStackTrace();
        }
        System.out.println(System.currentTimeMillis() - startTime);
    }
}
