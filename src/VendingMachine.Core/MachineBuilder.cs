using System;
using System.Collections.Generic;

namespace VendingMachine.Core
{
    public class MachineBuilder
    {
        public MachineBuilder()
        {
        }

        public Machine Build()
        {
            return new Machine();
        }
    }
}