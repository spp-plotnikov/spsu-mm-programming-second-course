import java.awt.image.BufferedImage;
import java.awt.image.BufferedImageOp;
import java.awt.image.Kernel;

class MyFilter {
    private String nameFilter;
    private float[] matrix;
    private int countLine;
    private int countColumn;

    MyFilter(String nameFilter, int countLine, int countColumn, float[] matrix) {
        this.nameFilter = nameFilter;
        this.matrix = matrix;
        this.countLine = countLine;
        this.countColumn = countColumn;
    }

    public BufferedImage processImage(BufferedImage image, Sender sender) {
        BufferedImageOp blurFilter = new ConvolveOp(new Kernel(countLine, countColumn, matrix), ConvolveOp.EDGE_NO_OP, null, sender);
        return blurFilter.filter(image, null);
    }

    String getNameFilter() {
        return nameFilter;
    }

    int getCountLine() {
        return countLine;
    }

    int getCountColumn() {
        return countColumn;
    }

    float[] getMatrix() {
        return matrix;
    }
}