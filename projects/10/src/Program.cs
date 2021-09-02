using System;
using System.Linq;

namespace Netsoft.Badger.Compiler.Frontend
{
    class Program
    {
        static int Main(string[] args)
        {
            var filePath = args[0];
            var content = System.IO.File.ReadAllText(filePath);
            var tokens = Tokenize(content);
            var ast = Parse(tokens);
            System.Console.WriteLine(ast);
            
            return 0;
        }

        static object Tokenize(string content){
            return content;
        }
        static object Parse(object tokens){
            return tokens;
        }
    }
}
