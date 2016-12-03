public class ArrayContainer implements Comparable<ArrayContainer> {
    int[] array;
    int index;

    public ArrayContainer(int[] array, int index) {
        this.array = array;
        this.index = index;
    }

    @Override
    public int compareTo(ArrayContainer o) {
        return this.array[this.index] - o.array[o.index];
    }
}
