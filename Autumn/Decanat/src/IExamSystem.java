public interface IExamSystem {
    public void Add(long studentId, long courseId);
    public void Remove(long studentId, long courseId);
    public boolean Contains(long studentId, long courseId);
}