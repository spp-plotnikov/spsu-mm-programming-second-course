package com.lab;

import java.util.ArrayList;
import java.util.Random;

/**
 * Created by Katrin on 21.06.2016.
 */
public class ProcessManager {

    public static ArrayList<Integer> fibersId = new ArrayList<>();
    private static ArrayList<Integer> deletesFibersId = new ArrayList<>();

    private static int idOfCurrentFiber = 0;

    public static void processManagerSwitch(boolean fiberFinished) {
        if (fiberFinished) {
            if (fibersId.get(idOfCurrentFiber) != Fiber.getPrimaryId()) {
                deletesFibersId.add(fibersId.get(idOfCurrentFiber));
            }
            System.out.println("Fiber with id: " + fibersId.get(idOfCurrentFiber) + " finished work");
            fibersId.remove(idOfCurrentFiber);
        }
        if (fibersId.size() > 0) {
            idOfCurrentFiber = new Random().nextInt(fibersId.size());
            Fiber.fiberSwitch(fibersId.get(idOfCurrentFiber));
        } else {
            System.out.println("Complete deleted... ");
            Fiber.fiberSwitch(Fiber.getPrimaryId());
        }
    }

}






