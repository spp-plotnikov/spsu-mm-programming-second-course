package com.lab;

import java.util.ArrayList;
import java.util.Map;

/**
 * Created by Katrin on 21.06.2016.
 */
public class ProcessManager {

    public static ArrayList<Integer> fibersId = new ArrayList<>();
    private static final int priorityLevelsNumber = 10;
    private static int idOfCurrentFiber = 0;

    // Without priority
/*    public static void processManagerSwitch(boolean fiberFinished) {
        if (fiberFinished) {
            idOfCurrentFiber = Main.idOfCurrentFiber;
            System.out.println("Fiber with id: " + fibersId.get(idOfCurrentFiber) + " finished work");
            fibersId.remove(idOfCurrentFiber);
        }
        if (fibersId.size() > 0) {
            idOfCurrentFiber = new Random().nextInt(fibersId.size());
            Main.idOfCurrentFiber = idOfCurrentFiber;
            Fiber.fiberSwitch(fibersId.get(idOfCurrentFiber));
        } else {
            System.out.println("Complete deleted... ");
            Fiber.fiberSwitch(Fiber.getPrimaryId());
        }
    }*/

    // With priority
    public static void processManagerSwitch(boolean fiberFinished) {
        if (fiberFinished) {
            idOfCurrentFiber = Main.idOfCurrentFiber;
            System.out.println("Fiber with id: " + idOfCurrentFiber + " finished work");
            Main.fibersInfo.remove(idOfCurrentFiber);

        }
        if (Main.fibersInfo.size() > 0) {
            int idOfCurrentFiber = findMaxPriority();
            Main.idOfCurrentFiber = idOfCurrentFiber;
            Fiber.fiberSwitch(idOfCurrentFiber);
        } else {
            System.out.println("Complete deleted... ");
            Fiber.fiberSwitch(Fiber.getPrimaryId());
        }
    }

    private static int findMaxPriority() {
        int maxPriority = priorityLevelsNumber + 1;
        int fiberId = 0;
        for (Map.Entry<Integer, Integer> fiber : Main.fibersInfo.entrySet()) {
            if( maxPriority > fiber.getValue()){
                maxPriority = fiber.getValue();
                fiberId = fiber.getKey();
            }
        }
        return fiberId;
    }

}






