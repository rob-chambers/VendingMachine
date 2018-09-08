using System;

namespace VendingMachine.Core
{
    public class ChangeGivenEventArgs : EventArgs
    {
        public ChangeGivenEventArgs(decimal change)
        {
            Change = change;
        }

        public decimal Change { get; private set; }
    }
}