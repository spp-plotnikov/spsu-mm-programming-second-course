package com.lab;

import java.util.ArrayList;

import static com.lab.ProcessManager.fibersInfo;

/**
 * Created by Katrin on 16.06.2016.
 */
public class Main {

    private static ArrayList<Fiber> fibers = new ArrayList<>();
    public static Integer idOfCurrentFiber;

    public static void main(String[] args) throws InterruptedException {
        for (int i = 0; i < 5; i++) {
            final Process process = new Process();
            Fiber fiber = new Fiber(new Runnable() {
                @Override
                public void run() {
                    try {
                        process.run();
                    } catch (InterruptedException e) {
                        e.printStackTrace();
                    }
                }
            });
            fibersInfo.put(fiber.getId(), process.getPriority());
            fibers.add(fiber);
            ProcessManager.fibersId.add(fiber.getId());
        }
        ProcessManager.processManagerSwitch(false);

        for (Fiber fiber : fibers) {
            if (!fiber.isPrimary) {
                Thread.sleep(42);
                fiber.delete();
            }
        }

        System.out.println("end");
    }

}


