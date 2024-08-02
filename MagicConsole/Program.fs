// For more information see https://aka.ms/fsharp-console-apps
printfn "Hello from F#"
open Experimental.MagicLang
open FParsec

let testQuery = "someField eq 2 and otherField in (1,2,3) or (foo eq 3 and bar gt 2)"
match Magic.parseQuery testQuery
    with
            | Success (ast, _, _) -> printfn "AST: %A" ast
            | Failure (errorMsg, _, _) -> printfn "Parse error: %s" errorMsg
