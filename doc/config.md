# Configuration parameters:

## ProblemDimension 
The dimension of your problem, aka the length of the position vector/array.

## CacheSize 
The size of the lbfgs buffer, how many past value's are tracked to estimate the Hessian.

## LipschitzSafetyValue 
The lipschitz value is estimated over an delta of:
delta= max{MinimumDelta,LipschitzSafetyValue*u_0}

## MinimumDelta 
The lipschitz value is estimated over an delta of:
delta= max{MinimumDelta,LipschitzSafetyValue*u_0}

## FBEMaxIterations 
The number of times the linesearch should backtrack.

## SafetyValueLineSearch 
The linesearch condition should be :
fNew > f - df.DotProduct(df) + (1 ) / (2 * newLocation.Gamma) * (direction.DotProduct(direction))

In reality it is:
fNew > f - df.DotProduct(df)
    + (1 - safetyValueLineSearch) / (2 * newLocation.Gamma) * directionSquaredNorm
    + 1e-6 * f;

## MinGammaValue 
The linesearch parameter gamma can never be smaller then this.

