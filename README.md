get via nuget **ValueInjecter** or download [here](https://valueinjecter.codeplex.com/downloads/get/1456536)

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
