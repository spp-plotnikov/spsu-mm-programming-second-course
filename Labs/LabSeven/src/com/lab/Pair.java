package com.lab;

/**
 * Created by Katrin on 28.09.2016.
 */
public class Pair implements Comparable<Pair> {
    private long studentId;
    private long courseId;

    public Pair(long studentId, long courseId) {
        this.studentId = studentId;
        this.courseId = courseId;
    }

    public long getCourseId() {
        return courseId;
    }

    public long getStudentId() {
        return studentId;
    }

    @Override
    public int compareTo(Pair pair) {
        int result;
        if (studentId - pair.getStudentId() > 0) {
            result = -1;
        } else if (studentId - pair.getStudentId() < 0) {
            result = 1;
        } else {
            result = 0;
        }
        return result;
    }
}
