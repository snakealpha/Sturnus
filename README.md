Sturnus
======

Sturnus is library that used to parse math expressions in string to a syntax tree to calculate it.

## How To Use? ##

### Calculate a expression ###
Calculate a expression just like this:
```cs
    double result = Sturnus.Calculate("1+-2*3/4-2^2");

    // result = -4.5
```

### Using variables ###
You can import introduces variables like this:
```cs
    Dictionary<string, Expression> argus = new Dictionary<string, Expression>();
    argus["argu"] = new ConstantExpression("2");
    double test3Res = Sturnus.Calculate("1+-2*3/4-{argu}^2", context: argus);

    // result = -4.5, too
```

### Using functions in a expression ###
```cs
    double result = Sturnus.Calculate("10 + Max[2, 3, 4]");

    // result = 14, which use 10 adds the max number between 2, 3 and 4.
```

### Get a parsed expression ###
Or, cache a parsed expression if you need to calculate a expression more than once. Since parse a string into a expression tree cost a lot, this will boost your calculation:
```cs
    Expression expression = Sturnus.Parse("1+-2*3/4-{argu}^2");
    Dictionary<string, double> argus = new Dictionary<string, double>();
    argus["argu"] = 2;
    double result = expression.Calculate(argus);

    // result = -4.5, three
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

## Built-in Functions ##
| Name | Number of Arguments | Description |
|------|---------------------|-------------|
| Abs | 1 | Get the abstract value of the argument. |
| Sum | No Limite | Add all values of arguments and return the sum value. |
| Max | No Limite | Return the max value of arugments. |
| Min | No Limite | Return the min value of arguments. |
| If | 3 | If 1st argument is lager or equals to 0, return 2nd, or return 3rd. |

## Extend Sturnus ##
It's possible to extend Sturnus with your own operators and functions by passing your own **Context** object. 

Your should do these things to custom your own operators:
* Inherits **Operator** Class to implement your own operator;

Or functions:
* Inherits **Function** Class;
* Overwrite **Excute** method in your function class.

After doing this, add the **Context** as *operatorContext* argument when you calaulate a expression with **Sturnus.Calculate** method or generate a expression tree with **Sturnus.Parse** method. Also, you can use **Context.GetDefaultContext** static method to get a default context with built-in operators and functions in it.

## Future of Sturnus ##
Here is some features that may include in future versions of Sturnus:
* ~~**Condition Branch** Some new operators to support condition branch may added to Sturnus. In incoming version, this feature may be implemented with some loss of operatands' value field; a verion without this loss will come in a further future.~~
*(This feature has implemented in version 1.1.0, as a function.)*
* ~~**Embeded Expression** I may introduce a new type of expression to represent a expression of any type of expression, which can support to embed a expression to a variable and produce a new expression from raw expression. You'll need not to calculate every expressions before get the final result if value of some variables come from other expressions.~~
*(Has been implemented in version 1.1.0.)*
* ~~**Functions** I'm thinking about a erlang-style synax to support capture more than 2 operands. This should be used to support complex math functions, such as sum, function and more.~~
*(Has been implemented in version 1.1.0)*
* **Improve proformance** Improve parser's algorithm, and parallel expressions.
* **Simplifing creating expression** Simplify creating when your wanna create a expression without parsing a whole expression.