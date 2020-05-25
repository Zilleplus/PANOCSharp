# PANOCSharp
# implementation of Panoc

## howto use
Checkout the demo project in the demo's folder.

First create a config:
var defaultConfig = new ConfigPanoc(problemDimension: 2);

Define your cost function and constraint(the Function class works with array's instead of vectors):
```
var degree = 2;
var costFunction = new VectorFunction(x => (x.PointwisePower(degree).Sum(), degree * x.PointwisePower(degree - 1)));
var constraint = new ProxBox(size: 1, penalty: 1, dimension: 2);
```
Then create a solver:
```
var solver = new PANOCSolver(costFunction,constraint,defaultconfig);
```

Finally call Solve with your initial solution:
```
var solution = solver.Solve(new double[] { 1.0,1.0 });
```

# documentation

1. [cost function](doc/costfunction.md)
2. [configuration](doc/config.md)
3. [constraint](doc/constraint.md)

