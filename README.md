## Task description ##

- Create new classes: `Vehicle` and `Car`.  
- Add following fields to `Vehicle` class:  
	- `name`;  
	- `maxSpeed` (can be changed only via constructor).  
	Fields should NOT be accessible from outside of the class.
- Create parametrized constructor with two parameters in order to initialize `Vehicle`.  
- Add following properties:  
	- `Name` (Must be accessible only in current class and for inheritors. Property is available both for read and write operations);
	- `MaxSpeed` (Can be accessible for inheritors and outside of the class. Property is available only for reading).  
- Inherit `Car` class from `Vehicle` class.
- Implement parametrized constructor for `Car` class and call base class constructor.
- Implement following methods in `Car` class:  
	- the first would change the name of `Car`;  
	- the second should NOT modify anything, but retrieve the name of `Car`.  
	Both methods must be accessible outside of the class!
      
*Topics - classes, inheritance.*