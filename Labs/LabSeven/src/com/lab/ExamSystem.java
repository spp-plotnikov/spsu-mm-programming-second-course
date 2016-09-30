package com.lab;

/**
 * Created by Katrin on 17.09.2016.
 */
public interface ExamSystem {
    void add(long studentId, long courseId, String exam);

    void remove(long studentId, long courseId);

    boolean contains(long studentId, long courseId);
}

