package com.lab;

import java.util.Map;
import java.util.concurrent.ConcurrentSkipListMap;

/**
 * Created by Katrin on 17.09.2016.
 */

public class ExamSystemImpl implements ExamSystem {

    private static Map<Pair, Boolean> concurrentSkipListMap;

    public ExamSystemImpl() {
        concurrentSkipListMap = new ConcurrentSkipListMap<>();
    }


    @Override
    public void add(long studentId, long courseId, boolean exam) {
        concurrentSkipListMap.put(new Pair(studentId, courseId), exam);
    }

    @Override
    public void remove(long studentId, long courseId) {
        concurrentSkipListMap.remove(new Pair(studentId, courseId));
    }

    @Override
    public boolean contains(long studentId, long courseId) {
        return concurrentSkipListMap.containsKey(new Pair(studentId, courseId));
    }
}
