
####usage
``` ruby
var customerInput = Mapper.Map<CustomerInput>(customer); 
```
or like this:
``` ruby
var customerInput = Mapper.Map<Customer, CustomerInput>(customer); 
```
(useful when working with EF proxy objects)

by default it will only map properties with the exact same name and type

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
`InjectFrom<TInjection>(source)` is used to map using a convention when `TInjection` is not specified it will map properties with exact same name and type

you can create you own injections by inheriting `LoopInjection` or `PropertyInjection`
for flattening/unflattening you can use `FlatLoopInjection` and `UnflatLoopInjection`

and there's more, see some examples of custom injections here: (put link to wiki page here)

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
