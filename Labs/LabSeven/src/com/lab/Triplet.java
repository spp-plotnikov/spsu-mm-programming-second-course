package com.lab;

/**
 * Created by Katrin on 01.10.2016.
 */
public class Triplet implements Comparable<Pair> {
    private long studentId;
    private long courseId;
    private String exam;

    public Triplet(long studentId, long courseId) {
        this.studentId = studentId;
        this.courseId = courseId;
    }

    public Triplet(long studentId, long courseId, String exam) {
        this.studentId = studentId;
        this.courseId = courseId;
        this.exam = exam;
    }

    public long getCourseId() {
        return courseId;
    }

    public long getStudentId() {
        return studentId;
    }

    public boolean equals(Triplet triplet) {
        return (this.getCourseId()==triplet.getCourseId() && this.getStudentId() == triplet.getStudentId());
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
