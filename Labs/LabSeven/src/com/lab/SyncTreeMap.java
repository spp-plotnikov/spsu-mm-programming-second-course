package com.lab;

import java.util.SortedMap;
import java.util.TreeMap;

/**
 * Created by Katrin on 17.09.2016.
 */
public class SyncTreeMap {

    private final SortedMap<Pair, String> map;

    public SyncTreeMap() {
        this.map = new TreeMap<>();
    }

    synchronized public void add(long studentId, long courseId, String exam) {
        map.put(new Pair(studentId, courseId), exam);
    }

    synchronized public void remove(long studentId, long courseId) {
        map.remove(new Pair(studentId, courseId));
    }

    public boolean contains(long studentId, long courseId) {
        return map.containsKey(new Pair(studentId, courseId));
    }
}
