import java.awt.image.BufferedImage;

public class TestingSystem {
    private final int[] clientsSteps = new int[]{5, 1, 5, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 150, 175, 200};
    private final int[] pixelsSteps = new int[]{300, 10, 50, 100, 300, 500, 1000, 1200, 1500, 1700, 2000, 2500, 3500};
    private int curClientsStep;
    private int curPixelsStep;
    private final int clientsConst = 3;
    private final int pixelsConst = 300;
    private int curTestType;

    TestingSystem() {
        this.curClientsStep = 0;
        this.curPixelsStep = 0;
    }

    public void start(int curTestType) {
        System.out.println("Starting test, type: " + curTestType);
        this.curTestType = curTestType;
        nextTest();
    }

    public void nextTest() {
        BufferedImage img;
        int clientsCount;

        if (curTestType == 0) { //fixed image
            img = createBufferedImage(pixelsConst, pixelsConst);
            clientsCount = clientsSteps[curClientsStep];
            new Test(this, clientsCount, img, 30000).start();
        } else if (curTestType == 1) { // image size growing
            img = createBufferedImage(pixelsSteps[curPixelsStep], pixelsSteps[curPixelsStep]);
            clientsCount = clientsConst;
            new Test(this, clientsCount, img, 70000).start();
        }
    }

    public void testSucceed(Test test) {
        if (curTestType == 0) {
            if (curClientsStep == 0) {
                System.out.println("WARMING-UP! Clients: " + clientsSteps[curClientsStep] +
                        ", median: " + test.getMedianTime() +
                        ", average: " + test.getAverageTime() +
                        ", min: " + test.getMinTime() +
                        ", max: " + test.getMaxTime());
            } else {
                System.out.println("Clients: " + clientsSteps[curClientsStep] +
                        ", median: " + test.getMedianTime() +
                        ", average: " + test.getAverageTime() +
                        ", min: " + test.getMinTime() +
                        ", max: " + test.getMaxTime());
            }

            curClientsStep++;
            if (curClientsStep < clientsSteps.length) {
                nextTest();
            } else {
                try {
                    Thread.sleep(7000); // let's relax a little bit!
                } catch (InterruptedException e) { }

                start(1);
            }

        } else if (curTestType == 1) {
            if (curPixelsStep == 0) {
                System.out.println("WARMING-UP! Image pixels: " + (int)Math.pow(pixelsSteps[curPixelsStep], 2) +
                        ", median: " + test.getMedianTime() +
                        ", average: " + test.getAverageTime());
            } else {
                System.out.println("Image pixels: " + (int)Math.pow(pixelsSteps[curPixelsStep], 2) +
                        ", median: " + test.getMedianTime() +
                        ", average: " + test.getAverageTime());
            }

            curPixelsStep++;
            if (curPixelsStep < pixelsSteps.length) {
                nextTest();
            }
        }
    }

    public void testFailed(Test test, String reason) {
        if (curTestType == 0) {
            System.out.println("Failed at number of clients: " + clientsSteps[curClientsStep] +
                    ", reason: " + reason);
            start(1);
        } else if (curTestType == 1) {
            System.out.println("Failed at image pixels: " + (int)Math.pow(pixelsSteps[curPixelsStep], 2) +
                    ", reason: " + reason);
        }
    }

    private BufferedImage createBufferedImage(int width, int height) {
        //create buffered image object img
        BufferedImage img = new BufferedImage(width, height, BufferedImage.TYPE_INT_ARGB);

        //create random image pixel by pixel
        for (int y = 0; y < height; y++) {
            for(int x = 0; x < width; x++) {
                int a = (int)(Math.random() * 256); //alpha
                int r = (int)(Math.random() * 256); //red
                int g = (int)(Math.random() * 256); //green
                int b = (int)(Math.random() * 256); //blue

                int p = (a << 24) | (r << 16) | (g << 8) | b; //pixel

                img.setRGB(x, y, p);
            }
        }

        return img;
    }
}
