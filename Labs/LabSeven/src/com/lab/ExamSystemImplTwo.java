package com.lab;

import firstCourse.linkedList.LinkedList;

/**
 * Created by Katrin on 01.10.2016.
 */
public class ExamSystemImplTwo implements ExamSystem {

    private LinkedList linkedList = new LinkedList();

    @Override
    public void add(long studentId, long courseId, String exam) {
        linkedList.add(new Triplet(studentId, courseId, exam));
    }

    @Override
    public void remove(long studentId, long courseId) {
        linkedList.remove(new Triplet(studentId, courseId));
    }

    @Override
    public boolean contains(long studentId, long courseId) {
        return linkedList.contains(new Triplet(studentId, courseId));
    }
}
