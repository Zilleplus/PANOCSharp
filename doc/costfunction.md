# Cost function

## Basics
A costfunction must implement the IFunction interface, when provided with a position. It should return the cost at that point as well as the gradient at that point.

```
public interface IFunction
{
	(double cost, Vector<double> gradient) Evaluate(Vector<double> position);
}
```

## Helper classes
There are 2 helper classes, that provide a more convenient way of defining an interface. VectorFunction and Function. For example to define the polynomial f(x,y) = x^2 + y^2

### Function
The easiest way to get started with panocsharp is to use the Function class to create a cost function.  

```
var costFunction = new Vector(
	x=> (Math.Pow(x[0],2)+ Math.Pow(x[1],2), new doube[]{2*x[0] , 2*x[1]}));

```

### VectorFunction
Panocsharp uses the MathNET numerics libary, a VectorFunction accepts a lambda function that uses the vector from MAthNet numerics. 

```
var costFunction = new VectorFunction(
	x => (x.PointwisePower(2).Sum(), 2 * x.PointwisePower(2 - 1)));
```
