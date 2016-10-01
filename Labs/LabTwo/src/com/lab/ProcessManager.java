package com.lab;

import java.util.ArrayList;
import java.util.Map;
import java.util.Random;
import java.util.TreeMap;

import static java.lang.Thread.sleep;

/**
 * Created by Katrin on 21.06.2016.
 */
public class ProcessManager {

    public static ArrayList<Integer> fibersId = new ArrayList<>();
    public static Map<Integer, Process> fibersInformation = new TreeMap<>();
    private static int idOfCurrentFiber = 0;
    private static Map<Integer, Integer> points = new TreeMap<>();


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
    public static void processManagerSwitch(boolean fiberFinished) throws InterruptedException {
        if (fiberFinished) {
            idOfCurrentFiber = Main.idOfCurrentFiber;
            System.out.println("Fiber with id: " + idOfCurrentFiber + " finished work");
            fibersInformation.remove(idOfCurrentFiber);
            points.remove(idOfCurrentFiber);
        }
        if (fibersInformation.size() > 0) {
            idOfCurrentFiber = chooseNewFiber();
            Main.idOfCurrentFiber = idOfCurrentFiber;
            sleep(100);
            Fiber.fiberSwitch(idOfCurrentFiber);
        } else {
            System.out.println("Complete deleted... ");
            Fiber.fiberSwitch(Fiber.getPrimaryId());
        }
    }

    private static int chooseNewFiber() {
        int sum;
        for (Map.Entry<Integer, Process> fiber : fibersInformation.entrySet()) {
            int totalDuration = fiber.getValue().getTotalDuration() % 10;
            sum = totalDuration + fiber.getValue().getPriority() * (new Random().nextInt(totalDuration + 1));
            points.put(fiber.getKey(), sum);
        }
        return findFiberWithMaxPriority();
    }

    private static int findFiberWithMaxPriority() {
        int maxPriority = 0;
        int fiberId = 0;
        for (Map.Entry<Integer, Integer> fiber : points.entrySet()) {
            if (maxPriority < fiber.getValue()) {
                maxPriority = fiber.getValue();
                fiberId = fiber.getKey();
            }
        }
        return fiberId;
    }

}






