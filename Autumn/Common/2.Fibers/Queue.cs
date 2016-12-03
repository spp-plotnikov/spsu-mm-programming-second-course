using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fibers
{
    public struct FiberItem
    {
        public uint Id;
        public int Priority;
        public int CPUtime;
        public FiberItem(uint id, int priority, int cpuTime)
        {
            this.Id = id;
            this.Priority = priority;
            this.CPUtime = cpuTime;
        }
    }

    public class SwitchQueue
    {
        private List<FiberItem> queue;
        private Dictionary<int, FiberItem> cpuTimeDistribution;

        public SwitchQueue()
        {
            this.queue = new List<FiberItem>();
            this.cpuTimeDistribution = new Dictionary<int, FiberItem>();
        }

        public void Enqueue(FiberItem item)
        {
            queue.Add(item);
            cpuTimeDistribution.Add(item.CPUtime, item);
        }

        public FiberItem Dequeue()
        {
            if (queue.Count > 0)
            {
                FiberItem top = queue[0];
                cpuTimeDistribution.Remove(top.CPUtime);
                queue.RemoveAt(0);
                return top;
            }
            throw new InvalidOperationException();
        }

        public bool TryGetIdByCPUtime(int time)
        {
            FiberItem id;
            if (cpuTimeDistribution.TryGetValue(time, out id))
                return true;
            return false;
        }

        public void Remove(FiberItem item)
        {
            cpuTimeDistribution.Remove(item.CPUtime);
            queue.Remove(item);
        }

        public FiberItem Peek()
        {   
            if (queue.Count > 0)
                return queue[0];
            throw new InvalidOperationException();
        }

        public FiberItem Last()
        {
            if (queue.Count > 0)
                return queue[queue.Count - 1];
            throw new InvalidOperationException();
        }


        public FiberItem GetIdByCPUtime(int time)
        {
            return cpuTimeDistribution[time];
        }

        public int Count()
        {
            return queue.Count;
        }

        public void Sort()
        {
            queue = queue.OrderBy(x => x.Priority).ToList();
        }

        public void Clear()
        {
            foreach (FiberItem fiber in queue)
            {
                Fiber.Delete(fiber.Id);
            }
        }

    }
}
