namespace VendingMachine.Core
{
    public abstract class CreditProvider
    {
        public abstract decimal Total { get; }
    }
}
