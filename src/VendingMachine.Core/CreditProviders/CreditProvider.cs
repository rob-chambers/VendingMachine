﻿namespace VendingMachine.Core.CreditProviders
{
    /// <summary>
    /// A way to supply credit to the machine
    /// </summary>
    public abstract class CreditProvider
    {
        public abstract decimal Total { get; }
    }
}