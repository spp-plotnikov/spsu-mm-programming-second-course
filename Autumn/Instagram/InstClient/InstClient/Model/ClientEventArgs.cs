using System;

namespace InstClient.Model
{
    public class ClientEventArgs : EventArgs
    {

        public string Message { get; set; }

        public ClientEventArgs(string s)
        {
            Message = s;
        }
    }
}