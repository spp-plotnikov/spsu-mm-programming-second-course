public class Filter {
    private String name;
    private float[] kernel;
    private int width;
    private int height;

    Filter(String name, int width, int height, float[] kernel) {
        this.name = name;
        this.kernel = kernel;
        this.width = width;
        this.height = height;
    }

    public String getName() {
        return name;
    }

    public float[] getKernel() {
        return kernel;
    }

    public int getWidth() {
        return width;
    }

    public int getHeight() {
        return height;
    }
}
