using System.Text;

namespace ClientShared
{
    public class Commands
    {
        public static byte[] GetFilters { get; } = Encoding.UTF8.GetBytes("g");
        public static byte[] ReceiveFilter { get; } = Encoding.UTF8.GetBytes("p");
    }
}