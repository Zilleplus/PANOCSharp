# constrain

## box constraint aka ProxBox
Put the value back within the borders of the box

The constructor has 3 parameters:
- size: size of the border, values higher then this are pushed back inside
- penalty: cost of a value outside the box
- dimension: the dimension of the box

When at least point point is outside the size border. The cost is equal to the penalty, otherwise the cost is zero.

The proximal operator will put all the points that are outside the bounding box defined by the size parameter, on the nearest border.

example:

```
var constraint = new ProxBox(size:2,penalty:10,dimension:2);
```
