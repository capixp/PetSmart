namespace BankFactory.ConcreteProducts
{
    public class BankOfAmericaTaxes : ITaxes
    {
        public void Send(string message)
        {
            Console.WriteLine($"Get Taxes --->>> type  {message}");
        }
    }
}
