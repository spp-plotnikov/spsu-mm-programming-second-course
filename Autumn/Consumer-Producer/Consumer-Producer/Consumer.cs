using System.Threading;

namespace Consumer_Producer
{
    public class Consumer<T>
    {

        private bool _canConsume;
        public IGetable<T> Conveyor { get; }
        public int Pause { get; }

        public Consumer(int pause, IGetable<T> conveyor)
        {
            _canConsume = true;
            Pause = pause;
            Conveyor = conveyor;
        }

        public void StartConsuming()
        {
            while (_canConsume)
            {
                Conveyor.Get();
                Thread.Sleep(Pause);
            }
        }

        public void StopConsuming()
        {
            _canConsume = false;
        }
    }
}