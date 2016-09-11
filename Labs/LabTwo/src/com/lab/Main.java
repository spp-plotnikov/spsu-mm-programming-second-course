package com.lab;

/**
 * Created by Katrin on 16.06.2016.
 */
public class Main {

    public static void main(String[] args) throws InterruptedException {
        for (int i = 0; i < 5; i++) {
            final Process process = new Process();
            Fiber fiber = new Fiber(new Action() {
                @Override
                public void callback() throws InterruptedException {
                    process.begin();
                }
            });
            ProcessManager.fibersId.add(fiber.getId());
        }
        ProcessManager.processManagerSwitch(false);
    }

}


