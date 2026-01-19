using BankFactory.ConcreteProducts;
using BankFactory.Creator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankFactory.ConcreteFactories
{
    public class WellsFargoTaxesFactory : TaxesFactory
    {
        private readonly Func<WellsFargoTaxes> _create;

        public WellsFargoTaxesFactory(Func<WellsFargoTaxes> create)
            => _create = create;

        //public override ITaxes CreateTaxes()
        //    => _create();

        public override ITaxes CreateTaxes()
        {
            WellsFargoTaxes instance = _create();
            return instance;
        }
    }
}
