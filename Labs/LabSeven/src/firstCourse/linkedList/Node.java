package firstCourse.linkedList;

import com.lab.Triplet;

/**
 * Created by Katrin on 23.02.2016.
 */
public class Node {
    private volatile int index;
    private Node next;
    private Node previous;
    private Triplet element;

    //создаём конструктор для первого элемента
    public Node() {
        this.next = null;
        this.previous = null;
    }

    //создаём конструктор для последующих элементов
    public Node(Node previous, Triplet element) {
        this.next = null;
        this.previous = previous;
        this.element = element;
    }

    public int getIndex() {
        return index;
    }

    public void setIndex(int index) {
        this.index = index;
    }

    public Node getNext() {
        return next;
    }

    public void setNext(Node next) {
        this.next = next;
    }

    public Node getPrevious() {
        return previous;
    }

    public void setPrevious(Node previous) {
        this.previous = previous;
    }

    public Triplet getElement() {
        return element;
    }
}
