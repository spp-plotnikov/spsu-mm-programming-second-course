using System;

namespace InstClient.Model
{
    public class ReceivedFiltEventArgs : EventArgs
    {
        public string[] Filters { get; set; }

        public ReceivedFiltEventArgs(string[] list)
        {
            Filters = list;
        }
    }
}