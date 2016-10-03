package firstCourse.linkedList;

import com.lab.Triplet;

/**
 * Created by Katrin on 23.02.2016.
 */
public class LinkedList {

    private Node node;
    private volatile int size = 0;

    public LinkedList() {
        this.node = new Node();
    }

    private Node headNode = null;

    public int size() {
        return this.size;
    }

    public boolean add(Triplet o) {
        Node next = new Node(node, o);
        synchronized(node){
            if (headNode == null) {
                headNode = next;
                headNode.setIndex(size() + 1);
                node = headNode;
            } else {
                synchronized (node.getPrevious()){
                    node.setNext(next);
                    node = next;
                    node.setIndex(size() + 1);
                }
            }
            size++;
        }
        return true;
    }

    public boolean contains(Triplet o) {
        boolean answer = false;
        Node node = headNode;
        for (int i = 0; i < size(); i++) {
            if (node.getElement().equals(o)) {
                answer = true;
            }
            node = node.getNext();
        }
        return answer;
    }

    public void remove(Triplet triplet) {
        Node node = headNode;
        synchronized (node) {
            for (int i = 1; i < size() + 1; i++) {
                if (node.getElement().equals(triplet)) {
                    if (node == headNode) {
                        int index = node.getIndex();
                        synchronized (node.getNext()) {
                            node.getNext().setPrevious(null);
                            node = node.getNext();
                            headNode = node;
                        }
                        for (int j = index; j < size() - 1; j++) {
                            node.setIndex(j);
                            node = node.getNext();
                        }
                        node.setIndex(node.getIndex() - 1);
                    } else {
                        int index = node.getIndex();
                        synchronized (node.getPrevious()){
                            synchronized (node.getNext()){
                                node.getPrevious().setNext(node.getNext());
                                node = node.getNext();
                            }
                        }
                        for (int j = index; j < size() - 1; j++) {
                            node.setIndex(j);
                            node = node.getNext();
                        }
                        node.setIndex(node.getIndex() - 1);

                    }
                    size--;
                }
                node = node.getNext();
            }
        }
    }
}
