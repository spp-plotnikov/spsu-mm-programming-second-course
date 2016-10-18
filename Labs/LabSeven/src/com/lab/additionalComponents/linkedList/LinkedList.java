package com.lab.additionalComponents.linkedList;

/**
 * Created by Katrin on 23.02.2016.
 */
public class LinkedList {

    private Node current;
    private Node headNode = new Node();

    public LinkedList() {
        headNode = new Node();
    }

    public void add(Triplet triplet) {
        Node newNode = new Node(triplet);
        current = headNode;
        current.lock();
        Node next = current.getNext();
        try {
            if (next == null) {
                current.setNext(newNode);
            } else {
                try {
                    next.lock();
                    while (next.getNext() != null && !next.getNext().getElement().equals(triplet)) {
                        current.unlock();
                        current = next;
                        next = next.getNext();
                        next.lock();
                    }
                    next.setNext(newNode);
                } finally {
                    next.unlock();
                }
            }
        } finally {
            current.unlock();
        }
    }

    public void remove(Triplet triplet) {
        Node nodeForRemove = new Node(triplet);
        current = headNode;
        if(current.getNext() == null){
            return;
        }
        Node next = current.getNext();
        current.lock();
        try {
            if (next.getElement().equals(nodeForRemove.getElement())) {
                current.setNext(next.getNext());
            } else {
                try {
                    next.lock();
                    while (!next.getElement().equals(nodeForRemove.getElement()) && next.getElement() != null) {
                        current.unlock();
                        current = next;
                        next = next.getNext();
                        next.lock();
                    }
                    if (next.getElement() != null) {
                        current.setNext(next.getNext());
                    }
                } finally {
                    next.unlock();
                }
            }
        } finally {
            current.unlock();
        }
    }

    public boolean contains(Triplet triplet) {
        boolean answer = false;
        Node node = headNode;
        while (!node.getElement().equals(triplet) && node.getNext() != null) {
            node = node.getNext();
        }
        if (node.getElement().equals(triplet)) {
            answer = true;
        }
        return answer;
    }
}
