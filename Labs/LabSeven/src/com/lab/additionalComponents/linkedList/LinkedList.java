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
        Node next = current.getNext();

        try {
            current.lock();
            if (next == null) {
                current.setNext(newNode);
            } else {
                try {
                    next.lock();
                    while (next.getNext() != null && !current.getNext().getElement().equals(triplet)) {
                        current.unlock();
                        current = next;
                        next = next.getNext();
                        next.lock();
                    }
                    if (current.getNext() == null) {
                        current.setNext(newNode);
                    } else {
                        current.getNext().setElement(triplet);
                    }
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
        if (current.getNext() == null) {
            return;
        }
        Node next = current.getNext();
        try {
            current.lock();
            if (next.getElement().equals(nodeForRemove.getElement())) {
                current.setNext(next.getNext());
            } else {
                try {
                    next.lock();
                    while (!next.getElement().equals(nodeForRemove.getElement()) && next.getNext() != null) {
                        current.unlock();
                        current = next;
                        next = next.getNext();
                        next.lock();
                    }
                    if (next.getElement().equals(nodeForRemove.getElement())) {
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

        current = headNode;
        Node next = current.getNext();
        try {
            current.lock();
            if (next == null) {
                return answer;
            } else {
                try {
                    next.lock();
                    while (next.getNext() != null && !current.getNext().getElement().equals(triplet)) {
                        current.unlock();
                        current = next;
                        next = next.getNext();
                        next.lock();
                    }
                    if (current.getElement().equals(triplet)) {
                        answer = true;
                    }
                    if(next.getNext() != null){
                        if(next.getElement().equals(triplet)){
                            answer = true;
                        }

                    }
                } finally {
                    next.unlock();
                }
            }
        } finally {
            current.unlock();
        }
        return answer;
    }
}
