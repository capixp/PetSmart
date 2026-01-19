using BankFactory.ConcreteProducts;
using BankFactory.Creator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankFactory.ConcreteFactories
{
    public class BankOfAmericaTaxesFactory : TaxesFactory
    {
        private readonly Func<BankOfAmericaTaxes> _create;

        public BankOfAmericaTaxesFactory(Func<BankOfAmericaTaxes> create)
            => _create = create;

        public override ITaxes CreateTaxes()
            => _create();
    }
}
