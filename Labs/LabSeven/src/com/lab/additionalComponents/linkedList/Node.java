package com.lab.additionalComponents.linkedList;

import java.util.concurrent.locks.ReentrantLock;

/**
 * Created by Katrin on 23.02.2016.
 */
public class Node extends ReentrantLock {
    private Node next;
    private Triplet element;

    //создаём конструктор для первого элемента
    public Node() {
        this.next = null;
        this.element = new Triplet(1, 1, "1");
    }

    //создаём конструктор для последующих элементов
    public Node(Triplet element) {
        this.next = null;
        this.element = element;
    }

    public Node getNext() {
        return next;
    }

    public void setNext(Node next) {
        this.next = next;
    }

    public Triplet getElement() {
        return element;
    }
}
