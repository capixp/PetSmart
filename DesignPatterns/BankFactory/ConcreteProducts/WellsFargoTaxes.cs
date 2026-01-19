namespace BankFactory.ConcreteProducts
{
    public class WellsFargoTaxes : ITaxes
    {
        public void Send(string message)
        {
            Console.WriteLine($"Get Taxes --->>> type  {message}");
        }
    }
}
