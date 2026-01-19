using BankFactory.ConcreteFactories;
using BankFactory.Creator;
using Microsoft.Extensions.DependencyInjection;
using BankFactory.ConcreteProducts;

var services = new ServiceCollection();

// Products
services.AddTransient<WellsFargoTaxes>();
services.AddTransient<BankOfAmericaTaxes>();

// Func<T> to create products without using "new" inside factories
services.AddTransient<Func<WellsFargoTaxes>>(sp =>
    () => sp.GetRequiredService<WellsFargoTaxes>());

services.AddTransient<Func<BankOfAmericaTaxes>>(sp =>
    () => sp.GetRequiredService<BankOfAmericaTaxes>());

// Factories registered by key (bank)
services.AddKeyedTransient<TaxesFactory, WellsFargoTaxesFactory>("WellsFargo");
services.AddKeyedTransient<TaxesFactory, BankOfAmericaTaxesFactory>("BankOfAmerica");

var provider = services.BuildServiceProvider();

// WITHOUT using new
var wfFactory = provider.GetRequiredKeyedService<TaxesFactory>("WellsFargo");
wfFactory.Taxes("WellsFargo");

var boaFactory = provider.GetRequiredKeyedService<TaxesFactory>("BankOfAmerica");
boaFactory.Taxes("BankOfAmerica");