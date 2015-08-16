get via nuget **[ValueInjecter](https://www.nuget.org/packages/ValueInjecter/)** or download [here](https://valueinjecter.codeplex.com/downloads/get/1479182)

####usage
``` ruby
var customerInput = Mapper.Map<CustomerInput>(customer); 
```
or like this:
``` ruby
var customerInput = Mapper.Map<Customer, CustomerInput>(customer); 
```
(useful when working with EF proxy objects)

by default it will only map properties with the exact same name and type (this can be changed)

####custom maps 
can be added, like this:
``` ruby
Mapper.AddMap<Customer, CustomerInput>(src =>
{
    var res = new CustomerInput();
    res.InjectFrom(src); // maps properties with same name and type
    res.FullName = src.FirstName + " " + src.LastName;
    return res;
});
```
####InjectFrom
`InjectFrom<TInjection>(source)` is used to map using a convention, when `TInjection` is not specified it will map properties with exact same name and type

it's used like this:
``` ruby
target.InjectFrom(source);
target.InjectFrom<Injection>(source);
target.InjectFrom(new Injection(parameters), source);
target.InjectFrom<Injection>(); // without source
```
you can create you own injections by inheriting `LoopInjection`, `PropertyInjection` and other base injections

see some examples of custom injections here: [injections examples] (https://github.com/omuleanu/ValueInjecter/wiki/custom-injections-examples)

####Additional parameters
an additional parameter can be set when mapping:
``` ruby
var customer = Mapper.Map<Customer>(foo, new MyClass { Title = "hi" });
```
you can use this parameter in AddMap like this:
``` ruby
Mapper.AddMap<Foo, Customer>((src, tag) =>
    {
        var par = (MyClass)tag;
        var res = new Customer { LastName = par.Title };
        ...
        return res;
    });
```
when using InjectFrom additional parameters can be sent to the injection:
``` ruby
    res.InjectFrom(new LoopInjection(new[] { "FirstName" }), customer); 
```
in this case LoopInjection will ignore "FirstName" property; you can add private fields to your custom injections and give them value via the constructor as shown above

####Flattening and unflattening
you can use `FlatLoopInjection` and `UnflatLoopInjection` directly or inherit them, you can also use the `UberFlatter` class in you custom injections, have look at the source code for these injections.

####Default map
By default `Mapper.Map` will only map properties with the exact same name and type, this can be changed by setting `Mapper.DefaultMap`, here's an example:

``` ruby
    Mapper.DefaultMap = (src, resType, tag) =>
    {
        var res = Activator.CreateInstance(resType);
        res.InjectFrom(src);
        return res;
    };
```

####Default InjectFrom
You can change the default injection by setting 
``` ruby
    StaticValueInjecter.DefaultInjection = new MyInjection();
```
    
####Multiple mappers
Multiple mappers with different configurations can be used by creating multiple instances of MapperInstance
``` ruby
	var mapper1 = new MapperInstance();
	mapper1.AddMap<Customer, Customer>(o => new Customer { FirstName = "mapper1" });
	
	var mapper2 = new MapperInstance();
	mapper2.AddMap<Customer, Customer>(o => new Customer { FirstName = "mapper2" });	

	var m1 = mapper1.Map<Customer>(customer);
	var m2 = mapper2.Map<Customer>(customer);
```
you could store the instance in a static member, or use your IoC Container

####Samples
there's samples in the source code for winforms, ASP.net web-forms, DAL, and wpf

deep cloning sample [here](https://github.com/omuleanu/ValueInjecter/blob/dae7956439cac8516979fe254a520a1942c5cdeb/Tests/Cloning.cs), and the [CloneInjection](https://github.com/omuleanu/ValueInjecter/blob/master/Tests/Injections/CloneInjection.cs)

for ASP.net MVC see http://prodinner.codeplex.com
