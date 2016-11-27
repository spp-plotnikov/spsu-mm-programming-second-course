using System;

namespace InstServer.Model
{
    public class ModelEventArgs : EventArgs
    {
        public string ServerInformation { get; set; }

        public ModelEventArgs(string s)
        {
            ServerInformation = s;
        }
    }
}