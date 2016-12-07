namespace Consumer_Producer
{
    public interface IPutable<T>
    {
        void Put(T obj);
        void Stop();
    }
}