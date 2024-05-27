[![buildtest](https://github.com/omuleanu/ValueInjecter/actions/workflows/buildtest.yml/badge.svg)](https://github.com/omuleanu/ValueInjecter/actions/workflows/buildtest.yml) ![NuGet Downloads](https://img.shields.io/nuget/dt/ValueInjecter)

Get it via nuget **[ValueInjecter](https://www.nuget.org/packages/ValueInjecter/)** 

ValueInjecter automatically maps and copies property values from one object to another.

#### Usage
```
var customerInput = Mapper.Map<CustomerInput>(customer); 
```
or explicitly typed:
```
var customerInput = Mapper.Map<Customer, CustomerInput>(customer); 
```

By default, ValueInjecter will only map properties with the exact same name and type, but this can be changed by adding custom maps for types that have different properties

#### Custom maps 

```
Mapper.AddMap<FromType, ResType>(src =>
{
    var res = new ResType();
    res.InjectFrom(src); // maps properties with same name and type
    res.FullName = src.FirstName + " " + src.LastName;
    return res;
});
```

#### Map to existing object
```
Mapper.AddMap<Customer, Customer>((from, tag) =>
{
    var existing = tag as Customer;
    existing.InjectFrom(from);
    return existing;
});

var customer = GetCustomer();
var res = new Customer();

Mapper.Map<Customer>(customer, res);
```
#### InjectFrom
`InjectFrom<TInjection>(source)` is used to map using a convention, when `TInjection` is not specified it will map properties with exact same name and type:
```
target.InjectFrom(source);
target.InjectFrom<Injection>(source);
target.InjectFrom(new Injection(parameters), source);
target.InjectFrom<Injection>(); // without source
```
You can create you own injections by inheriting `LoopInjection`, `PropertyInjection` and other base injections.

See some examples of custom injections here: [injections examples](https://github.com/omuleanu/ValueInjecter/wiki/custom-injections-examples).

#### Additional parameters
```
var customer = Mapper.Map<Customer>(foo, new MyClass { Title = "hi" });
```
Using the parameter:
```
Mapper.AddMap<Foo, Customer>((src, tag) =>
    {
        var par = (MyClass)tag;
        var res = new Customer { LastName = par.Title };
        ...
        return res;
    });
```

#### Ignoring properties
```
    res.InjectFrom(new LoopInjection(new[] { "FirstName" }), customer); 
```
This tells `LoopInjection` to ignore the `FirstName` property of the `customer` object.

#### Flattening and unflattening
You can use `FlatLoopInjection` and `UnflatLoopInjection` directly or inherit them, you can also use the `UberFlatter` class in you custom injections, have look at the source code for these injections.

#### Default map
For pairs of types that don't have a mapping created using `Mapper.AddMap`, there's a default map being used.
This default map will only map properties with the exact same name and type, this can be changed by setting `Mapper.DefaultMap`, here's an example that sets the default map:

```
    Mapper.DefaultMap = (src, resType, tag) =>
    {
        // this is the source code of default map 
        var res = Activator.CreateInstance(resType);
        res.InjectFrom(src);
        return res;
    };
```
So if you call `Mapper.Map<Customer>(customerInput)` and before you've created a map using Mapper.AddMap<CustomerInput, Customer>

#### Default InjectFrom
You can change the default injection by setting 
```
    StaticValueInjecter.DefaultInjection = new MyInjection();
```
    
#### Multiple mappers
Multiple mappers with different configurations can be used by creating multiple instances of MapperInstance
```
var mapper1 = new MapperInstance();
var mapper2 = new MapperInstance();

mapper1.AddMap<Customer, CustomerInput>((from) =>
{
    var input = new CustomerInput();
    input.InjectFrom(from);
    return input;
});

mapper2.AddMap<Customer, CustomerInput>((from) =>
{
    var input = new CustomerInput();
    input.FirstName = from.FirstName;
    return input;
});

var input1 = mapper1.Map<CustomerInput>(customer);
var input2 = mapper2.Map<CustomerInput>(customer); // has only FirstName set
```
You could store the instance in a static member, or use your IoC Container.

#### Samples
There are samples in the source code for WinForms, ASP.NET Web Forms, DAL, and WPF.

A deep cloning sample is [here](https://github.com/omuleanu/ValueInjecter/blob/dae7956439cac8516979fe254a520a1942c5cdeb/Tests/Cloning.cs), and [CloneInjection](https://github.com/omuleanu/ValueInjecter/blob/master/Tests/Injections/CloneInjection.cs).

For ASP.NET MVC see http://prodinner.codeplex.com.

**Questions:** http://stackoverflow.com/questions/tagged/valueinjecter

**Chat:** https://gitter.im/omuleanu/ValueInjecter
