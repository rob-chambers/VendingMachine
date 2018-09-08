namespace VendingMachine.Core
{
    /// <summary>
    /// Builder pattern - idea was to allow building of machines that support just coins or coins and notes for example.
    /// Possibly use inheritance on the Machine class for this (i.e. Machine would become abstract)
    /// </summary>
    public class MachineBuilder
    {
        public void AddNoteFacility()
        {
        }

        public Machine Build()
        {
            return new Machine();
        }
    }
}