using System;

namespace GitOperations.VS.Service
{
    public class CommandOutputEventArgs : EventArgs
    {
        public CommandOutputEventArgs(string output)
        {
            Output = output;
        }

        public string Output { get; set; }
    }
}