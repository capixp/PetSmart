namespace BankFactory.Creator
{
    public abstract class TaxesFactory
    {
        public abstract ITaxes CreateTaxes();

        public void Taxes(string message)
        {
            var notification = CreateTaxes();
            notification.Send(message);
        }
    }
}
