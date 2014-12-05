Sturnus
======

Sturnus is library that used to parse math expressions in string to a syntax tree to calculate it.

## How To Use? ##
Calculate a expression just like this:
```cs
    double result = Sturnus.Calculate("1+-2*3/4-2^2");

    // result = -4.5
```
You can import introduces variables like this:
```cs
    Dictionary<string, double> argus = new Dictionary<string, double>();
    argus["argu"] = 2;
    double test3Res = Sturnus.Calculate("1+-2*3/4-{argu}^2", context: argus);

    // result = -4.5, too
```
Or, cache a parsed expression if you need to calculate a expression more than once. Since parse a string into a expression tree cost a lot, this will boost your calculation a lot:
```cs
    Expression expression = Sturnus.Parse("1+-2*3/4-{argu}^2");
    Dictionary<string, double> argus = new Dictionary<string, double>();
    argus["argu"] = 2;
    double result = expression.Calculate(argus);

    // result = -4.5, again
```

## Built-in Operators ##
| Priority | Operators | Associativity | Commit |
|----------|-----------|--------|------|
| 12 | Abs(uniary) | Right | Abstract |
| 12 | -(uniary) | Right | Negative |
| 10 | ^ | Left | Power |
| 9 | % | Left | Mod |
| 9 | * | Left | Mutilple |
| 9 | / | Left | Divide |
| 8 | + | Left | Plus |
| 8 | - | Left | Minus |

* **Note:** Operators that in a couple of bracket will have weight plused by 100.

## Extend Sturnus ##
It's possible to extend Sturnus with your own operators. Your should do these things to use your custom operators:
* Inherits **Operator** Class to implement your own operator;
* Add them to a **OperatorContext** instance;
* Pass the **OperatorContext** as *operatorContext* argument when you calaulate a expression with **Sturnus.Calculate** method or generate a expression tree with **Sturnus.Parse** method.

## Future of Sturnus ##
Here is some features that may include in future versions of Sturnus:
* **Condition Branch** Some new operators to support condition branch may added to Sturnus. In incoming version, this feature may be implemented with some loss of operatands' value field; a verion without this loss will come in a further future.
* **Embeded Expression** I may introduce a new type of expression to represent a expression of any type of expression, which can support to embed a expression to a variable and produce a new expression from raw expression. You'll need not to calculate every expressions before get the final result if value of some variables come from other expressions.
* **Functions** I'm thinking about a erlang-style synax to support capture more than 2 operands. This should be used to support complex math functions, such as sum, function and more.