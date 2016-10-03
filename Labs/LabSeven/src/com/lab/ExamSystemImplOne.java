package com.lab;

/**
 * Created by Katrin on 17.09.2016.
 */

public class ExamSystemImplOne implements ExamSystem {

    private static SyncTreeMap synchronizedMap;

    public ExamSystemImplOne() {
        synchronizedMap = new SyncTreeMap();
    }


    @Override
    public void add(long studentId, long courseId, String exam) {
        synchronizedMap.add(studentId, courseId, exam);
    }

    @Override
    public void remove(long studentId, long courseId) {
        synchronizedMap.remove(studentId, courseId);
    }

    @Override
    public boolean contains(long studentId, long courseId) {
        return synchronizedMap.contains(studentId, courseId);
    }
}
