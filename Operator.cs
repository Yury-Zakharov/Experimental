using Sprache;
using System;
using System.Linq;

public enum Operator { Eq, Ne, Ge, Gt, Le, Lt, In }
public enum LogicalOperator { And, Or }
public class Value
{
    public int? IntValue { get; set; }
    public string StringValue { get; set; }
    public decimal? DecimalValue { get; set; }
    public DateTime? DateValue { get; set; }
    public TimeSpan? TimeValue { get; set; }
    public DateTime? DateTimeValue { get; set; }
    public Value[] ListValue { get; set; }
}
public class Expression
{
    public string Field { get; set; }
    public Operator? Op { get; set; }
    public Value Value { get; set; }
    public LogicalOperator? LogicalOp { get; set; }
    public Expression Left { get; set; }
    public Expression Right { get; set; }
}

public static class MagicParser
{
    public static readonly Parser<string> Identifier =
        Parse.Letter.AtLeastOnce().Text().Token();

    public static readonly Parser<Value> Decimal =
        Parse.Decimal.Select(x => new Value { DecimalValue = decimal.Parse(x) }).Token();

    public static readonly Parser<Value> Integer =
        Parse.Number.Select(x => new Value { IntValue = int.Parse(x) }).Token();

    public static readonly Parser<Value> QuotedString =
        from openQuote in Parse.Char('\'')
        from content in Parse.CharExcept('\'').Many().Text()
        from closeQuote in Parse.Char('\'')
        select new Value { StringValue = content };

    public static readonly Parser<Value> SingleValue =
        Decimal.Or(Integer).Or(QuotedString);

    public static readonly Parser<Value[]> ValueList =
        from openParen in Parse.Char('(')
        from values in SingleValue.DelimitedBy(Parse.Char(',').Token())
        from closeParen in Parse.Char(')')
        select values.ToArray();

    public static readonly Parser<Value> Value =
        SingleValue.Or(ValueList.Select(x => new Value { ListValue = x }));

	// ... rest of the parsers ...

	public static readonly Parser<Operator> Op =
		Parse.String("eq").Return(Operator.Eq)
			.Or(Parse.String("ne").Return(Operator.Ne))
			.Or(Parse.String("ge").Return(Operator.Ge))
			.Or(Parse.String("gt").Return(Operator.Gt))
			.Or(Parse.String("le").Return(Operator.Le))
			.Or(Parse.String("lt").Return(Operator.Lt))
			.Or(Parse.String("in").Return(Operator.In))
			.Token();

	public static readonly Parser<LogicalOperator> LogicalOp =
		Parse.String("and").Return(LogicalOperator.And)
			.Or(Parse.String("or").Return(LogicalOperator.Or))
			.Token();

	public static readonly Parser<Expression> Comparison =
		from field in Identifier
		from op in Op
		from value in Value
		select new Expression { Field = field, Op = op, Value = value };

	public static readonly Parser<Expression> Logical =
		from left in Comparison
		from logicalOp in LogicalOp
		from right in Parse.Ref(() => Expr)
		select new Expression { Left = left, LogicalOp = logicalOp, Right = right };

	public static readonly Parser<Expression> Expr =
		Logical.Or(Comparison);

	public static Expression ParseQuery(string input) =>
		Expr.End().Parse(input);
}
