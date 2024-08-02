namespace Experimental.MagicLang

module Magic = 

    open FParsec
    open System

    type Operator = Eq | Ne | Ge | Gt | Le | Lt | In
    type LogicalOperator = And | Or
    type Value = Int of int | String of string | Decimal of decimal | Date of System.DateTime | Time of System.TimeSpan | DateTime of System.DateTime | List of Value list
    type Expression = 
        | Comparison of string * Operator * Value
        | Logical of Expression * LogicalOperator * Expression

    let str s = pstring s .>> spaces
    let identifier = (many1Satisfy2 System.Char.IsLetter System.Char.IsLetterOrDigit .>> spaces |>> fun s -> s) <?> "identifier"
    let decimal = (pfloat .>> spaces |>> fun f -> Decimal(decimal f)) <?> "decimal"
    let integer = (pint32 .>> spaces |>> fun i -> Int i) <?> "integer"
    let quotedString = (between (pchar '\'') (pchar '\'') (manySatisfy (fun c -> c <> '\'')) .>> spaces |>> fun s -> String s) <?> "quoted string"
    let comma = pchar ',' .>> spaces        

    let date = (manyTill anyChar (pchar 'T') .>> spaces |>> fun s -> Date(System.DateTime.Parse(System.String.Concat(s)))) <?> "date"
    let time = (manyTill anyChar (pchar 'Z') .>> spaces |>> fun s -> Time(System.TimeSpan.Parse(System.String.Concat(s)))) <?> "time"
    let dateTime = (manyTill anyChar (pchar 'Z') .>> spaces |>> fun s -> DateTime(System.DateTime.ParseExact(System.String.Concat(s), "yyyy-MM-ddTHH:mm:ss", null))) <?> "dateTime"


    let singleValue = (choice [attempt decimal; attempt integer; attempt date; attempt time; attempt dateTime; quotedString]) <?> "single value"
    let valueList = between (str "(") (str ")") (sepBy singleValue comma) |>> List
    let value = (choice [attempt singleValue; valueList]) <?> "value"




    let op = 
        (choice [
            str "eq" >>% Eq
            str "ne" >>% Ne
            str "ge" >>% Ge
            str "gt" >>% Gt
            str "le" >>% Le
            str "lt" >>% Lt
            str "in" >>% In
        ]) <?> "operator"

    let logicalOp = 
        (choice [
            str "and" >>% And
            str "or" >>% Or
        ]) <?> "logical operator"

    let rec expr() : Parser<Expression, unit> =
        (parse {
            let! left = term()
            return! choice [
                parse {
                    let! op = logicalOp
                    let! right = expr()
                    return Logical (left, op, right)
                }
                preturn left
            ]
        }) <?> "expression"

    and term() : Parser<Expression, unit> =
        (parse {
            return! choice [
                parse {
                    let! field = identifier
                    let! op = op
                    let! value = value
                    return Comparison (field, op, value)
                }
                between (str "(") (str ")") (expr())
            ]
        }) <?> "term"


    and factor() : Parser<Expression, unit> =
        (parse {
            return! choice [
                between (str "(") (str ")") (expr())
                parse {
                    let! field = identifier
                    let! op = op
                    let! value = value
                    return Comparison (field, op, value)
                }
            ]
        }) <?> "factor"


    let parseQuery = run (expr())


