using Silverfly.Repl;

namespace Sample.Brainfuck;

public class Repl : ReplInstance<BrainfuckParser>
{
    protected override void Evaluate(string input)
    {
        var helloWorld = """
                                                  ++++++++++
                                                  [
                                                   >+++++++>++++++++++>+++>+<<<<-
                                                  ]                       
                                                  >++.                    #'H'
                                                  >+.                     #'e'
                                                  +++++++.                #'l'
                                                  .                       #'l'
                                                  +++.                    #'o'
                                                  >++.                    #Space
                                                  <<+++++++++++++++.      #'W'
                                                  >.                      #'o'
                                                  +++.                    #'r'
                                                  ------.                 #'l'
                                                  --------.               #'d'
                                                  >+.                     #'!'
                                                  >.                      
                                                  +++.                    
                         """;
        var parsed = Parser.Parse(helloWorld);

        EvalListener.Listener.Listen(new EvaluationContext(), parsed.Tree);
    }
}
