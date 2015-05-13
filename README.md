
usage:
``` ruby
var customerInput = Mapper.Map<CustomerInput>(customer); 
```
or like this:
``` ruby
var customerInput = Mapper.Map<Customer, CustomerInput>(customer); 
```
(useful when working with EF proxy objects)

by default it will only map properties with the exact same name and type

custom maps can be specified, like this:
``` ruby
Mapper.AddMap<Customer, CustomerInput>(src =>
{
    var res = new CustomerInput();
    res.InjectFrom(src); // maps properties with same name and type
    res.FullName = src.FirstName + " " + src.LastName;
    return res;
});
```
