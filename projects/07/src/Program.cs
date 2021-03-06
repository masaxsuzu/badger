using System;
using System.Linq;

namespace Netsoft.Badger.Compiler.Backend
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length < 1) {
                Console.Error.WriteLine("No source code is provided!");
                return 1;
            }

            var lines = System.IO.File.ReadAllLines(args[0]);
            var sm = new StackMachine(Console.Out);
            foreach (var line in lines)
            {
                var command = ParseCommand(line);
                command.Generate(sm);
            }
            return 0;
        }

        static ICommand ParseCommand(string line) {
            var chars = line.ToCharArray();
            int i = 0;
            while (i < chars.Length)
            {
                if (Char.IsWhiteSpace(chars[i])) {
                    i++;
                    continue;
                }
                // comment
                if (chars[i] == '/') {
                     if (chars[i+1] == '/') {
                            return new Comment(line);
                     }
                    throw new Exception($"got {chars[i+1]}, want /");
                }

                string c = line.Length switch
                {
                    2 => new string(line.AsSpan(i,2).ToArray()),
                    3 => new string(line.AsSpan(i,3).ToArray()),
                    _ => new string(line.AsSpan(i,4).ToArray()),
                };
                switch (c)
                {
                    case "eq":
                        return new EqualCommand();
                    case "lt":
                        return new LessThanCommand();
                    case "gt":
                        return new GreaterThanCommand();
                    case "neg":
                        return new NegativeCommand();
                    case "not":
                        return new NotCommand();
                    case "and":
                        return new AndCommand();
                    case "or":
                        return new OrCommand();
                    case "pop":
                        string[] rest = new string(line.AsSpan(i+4,line.Length - i - 4).ToArray()).Split(' ');
                        string a = rest[0];
                        int b = int.Parse(rest[1]);
                        return new PopCommand(a, b);
                    case "push":
                        string[] rest1 = new string(line.AsSpan(i+5,line.Length - i - 5).ToArray()).Split(' ');
                        string a1 = rest1[0];
                        int b1 = int.Parse(rest1[1]);
                        return new PushCommand(a1, b1);
                    case "add":
                        return new AddCommand();
                    case "sub":
                        return new SubCommand();
                    default:
                        throw new Exception($"Unknown command {c}");
                }
            }
            return new NewLine(line);
        }

        interface ICommand {
            void Generate(StackMachine machine);
        }

        public class NewLine: ICommand {
            public NewLine(string line){
            }
            public  void Generate(StackMachine machine) {

            }
        }
        public class Comment: ICommand {
            public string Line{get;set;}
            public Comment(string line){
                Line = line;
            }
            public  void Generate(StackMachine machine) {
                
            }
        }

        public class EqualCommand: ICommand {
            public EqualCommand(){}
            public  void Generate(StackMachine machine) {
                machine.Eq(this);
            }
        }

        public class LessThanCommand: ICommand {
            public LessThanCommand(){}
            public  void Generate(StackMachine machine) {
                machine.Lt(this);
            }
        }
        public class GreaterThanCommand: ICommand {
            public GreaterThanCommand(){}
            public  void Generate(StackMachine machine) {
                machine.Gt(this);
            }
        }
        public class NegativeCommand: ICommand {
            public NegativeCommand(){}
            public  void Generate(StackMachine machine) {
                machine.Neg(this);
            }
        }
        public class NotCommand: ICommand {
            public NotCommand(){}
            public  void Generate(StackMachine machine) {
                machine.Not(this);
            }
        }

        public class AndCommand: ICommand {
            public AndCommand(){}
            public  void Generate(StackMachine machine) {
                machine.And(this);
            }
        }
         public class OrCommand: ICommand {
            public OrCommand(){}
            public  void Generate(StackMachine machine) {
                machine.Or(this);
            }
        }

        public class PushCommand: ICommand{
            public string Arg1 {get; set;}
            public int Arg2 {get; set;}
            public PushCommand(string arg1, int arg2) {
                Arg1 = arg1;
                Arg2 = arg2;
            }
            public  void Generate(StackMachine machine) {
                machine.Push(this);                
            }
        }
        public class PopCommand: ICommand {
            public string Arg1 {get; set;}
            public int Arg2 {get; set;}
            public PopCommand(string arg1, int arg2) {
                Arg1 = arg1;
                Arg2 = arg2;
            }
            public  void Generate(StackMachine machine) {
            }
        }
        public class AddCommand: ICommand {
            public AddCommand() {}
            public  void Generate(StackMachine machine) {
                machine.Add(this);
            }
        }
        public class SubCommand: ICommand {
            public SubCommand() {}
            public  void Generate(StackMachine machine) {
                machine.Sub(this);
            }
        }

        public class StackMachine {
            private System.IO.TextWriter _file;
            public int RegisterId{ get; set;}
            public int SP{ get; set;}
            public int Label{ get; set;}
            public StackMachine(System.IO.TextWriter file) {
                RegisterId = 0;
                _file = file;
                Label = 0;
            }
            public void Eq(EqualCommand command) {
                var l = Label++;
                // Pop and Pop and Push
                _file.WriteLine($"// POP left");
                _file.WriteLine($"@SP");
                _file.WriteLine($"A=M-1");
                _file.WriteLine($"D=M");
                _file.WriteLine($"@SP");
                _file.WriteLine($"M=M-1");
                _file.WriteLine($"// POP right");
                _file.WriteLine($"@SP");
                _file.WriteLine($"A=M-1");
                _file.WriteLine($"D=D-M");                
                _file.WriteLine($"@SP");
                _file.WriteLine($"M=M-1");
                
                _file.WriteLine($"// Push a == b");
                
                _file.WriteLine($"@EQ{l}");
                _file.WriteLine($"D;JEQ");
                _file.WriteLine($"@SP");
                _file.WriteLine($"A=M");
                _file.WriteLine($"M=0");    // a != b
                _file.WriteLine($"@EQ_END{l}");
                _file.WriteLine($"0;JMP");
                _file.WriteLine($"(EQ{l})");
                _file.WriteLine($"@SP");
                _file.WriteLine($"A=M");
                _file.WriteLine($"M=-1");   // a == b
                _file.WriteLine($"(EQ_END{l})");
                _file.WriteLine($"@SP");
                _file.WriteLine($"M=M+1");
            }
            public void Lt(LessThanCommand command) {
                var l = Label++;
                // Pop and Pop and Push
                _file.WriteLine($"// POP left");
                _file.WriteLine($"@SP");
                _file.WriteLine($"A=M-1");
                _file.WriteLine($"D=M");
                _file.WriteLine($"@SP");
                _file.WriteLine($"M=M-1");
                _file.WriteLine($"// POP right");
                _file.WriteLine($"@SP");
                _file.WriteLine($"A=M-1");
                _file.WriteLine($"D=M-D");                
                _file.WriteLine($"@SP");
                _file.WriteLine($"M=M-1");
                
                _file.WriteLine($"// Push a < b");
                
                _file.WriteLine($"@LT{l}");
                _file.WriteLine($"D;JLT");
                _file.WriteLine($"@SP");
                _file.WriteLine($"A=M");
                _file.WriteLine($"M=0");    // a >= b
                _file.WriteLine($"@LT_END{l}");
                _file.WriteLine($"0;JMP");
                _file.WriteLine($"(LT{l})");
                _file.WriteLine($"@SP");
                _file.WriteLine($"A=M");
                _file.WriteLine($"M=-1");   // a < b
                _file.WriteLine($"(LT_END{l})");
                _file.WriteLine($"@SP");
                _file.WriteLine($"M=M+1");
            }
            public void Gt(GreaterThanCommand command) {
                 var l = Label++;
                // Pop and Pop and Push
                _file.WriteLine($"// POP left");
                _file.WriteLine($"@SP");
                _file.WriteLine($"A=M-1");
                _file.WriteLine($"D=M");
                _file.WriteLine($"@SP");
                _file.WriteLine($"M=M-1");
                _file.WriteLine($"// POP right");
                _file.WriteLine($"@SP");
                _file.WriteLine($"A=M-1");
                _file.WriteLine($"D=M-D");                
                _file.WriteLine($"@SP");
                _file.WriteLine($"M=M-1");
                
                _file.WriteLine($"// Push b > a");
                
                _file.WriteLine($"@GT{l}");
                _file.WriteLine($"D;JGT");
                _file.WriteLine($"@SP");
                _file.WriteLine($"A=M");
                _file.WriteLine($"M=0");    // a <= b
                _file.WriteLine($"@GT_END{l}");
                _file.WriteLine($"0;JMP");
                _file.WriteLine($"(GT{l})");
                _file.WriteLine($"@SP");
                _file.WriteLine($"A=M");
                _file.WriteLine($"M=-1");   // a < b
                _file.WriteLine($"(GT_END{l})");
                _file.WriteLine($"@SP");
                _file.WriteLine($"M=M+1");
            }
            public void Neg(NegativeCommand command) {
                // Pop and Push
                _file.WriteLine($"// POP left");
                _file.WriteLine($"@SP");
                _file.WriteLine($"A=M-1");
                _file.WriteLine($"D=-M");
                _file.WriteLine($"@SP");
                _file.WriteLine($"M=M-1");
                _file.WriteLine($"// Push -a");
                _file.WriteLine($"@SP");
                _file.WriteLine($"A=M");
                _file.WriteLine($"M=D");    // -a
                _file.WriteLine($"@SP");
                _file.WriteLine($"M=M+1");
            }
            public void Not(NotCommand command) {
                // Pop and Push
                _file.WriteLine($"// POP left");
                _file.WriteLine($"@SP");
                _file.WriteLine($"A=M-1");
                _file.WriteLine($"D=!M");
                _file.WriteLine($"@SP");
                _file.WriteLine($"M=M-1");

                _file.WriteLine($"// Push !a");
                _file.WriteLine($"@SP");
                _file.WriteLine($"A=M");
                _file.WriteLine($"M=D");    // !a
                _file.WriteLine($"@SP");
                _file.WriteLine($"M=M+1");
            }
            public void And(AndCommand command) {
                // Pop and Pop and Push
                _file.WriteLine($"// POP left");
                _file.WriteLine($"@SP");
                _file.WriteLine($"A=M-1");
                _file.WriteLine($"D=M");
                _file.WriteLine($"@SP");
                _file.WriteLine($"M=M-1");
                _file.WriteLine($"// POP right");
                _file.WriteLine($"@SP");
                _file.WriteLine($"A=M-1");
                _file.WriteLine($"D=D&M");                
                _file.WriteLine($"@SP");
                _file.WriteLine($"M=M-1");
                
                _file.WriteLine($"// Push a & b");
                _file.WriteLine($"A=M");
                _file.WriteLine($"M=D");
                _file.WriteLine($"@SP");
                _file.WriteLine($"M=M+1");
            }
            public void Or(OrCommand command) {
                // Pop and Pop and Push
                _file.WriteLine($"// POP left");
                _file.WriteLine($"@SP");
                _file.WriteLine($"A=M-1");
                _file.WriteLine($"D=M");
                _file.WriteLine($"@SP");
                _file.WriteLine($"M=M-1");
                _file.WriteLine($"// POP right");
                _file.WriteLine($"@SP");
                _file.WriteLine($"A=M-1");
                _file.WriteLine($"D=D|M");                
                _file.WriteLine($"@SP");
                _file.WriteLine($"M=M-1");
                
                _file.WriteLine($"// Push a | b");
                _file.WriteLine($"A=M");
                _file.WriteLine($"M=D");
                _file.WriteLine($"@SP");
                _file.WriteLine($"M=M+1");
            }
            public void Push(PushCommand command) {
                if(command.Arg1 == "constant") {
                    _file.WriteLine($"// Push {command.Arg2}");
                    _file.WriteLine($"@{command.Arg2}");
                    _file.WriteLine($"D=A");
                    _file.WriteLine($"@SP");
                    _file.WriteLine($"A=M");
                    _file.WriteLine($"M=D");
                    _file.WriteLine($"@SP");
                    _file.WriteLine($"M=M+1");
                    return;
                }
            }
            public void Add(AddCommand command) {
                // Pop and Pop and Push
                _file.WriteLine($"// POP left");
                _file.WriteLine($"@SP");
                _file.WriteLine($"A=M-1");
                _file.WriteLine($"D=M");
                _file.WriteLine($"@SP");
                _file.WriteLine($"M=M-1");
                _file.WriteLine($"// POP right");
                _file.WriteLine($"@SP");
                _file.WriteLine($"A=M-1");
                _file.WriteLine($"D=D+M");                
                _file.WriteLine($"@SP");
                _file.WriteLine($"M=M-1");
                _file.WriteLine($"// Push a+b");
                _file.WriteLine($"A=M");
                _file.WriteLine($"M=D");
                _file.WriteLine($"@SP");
                _file.WriteLine($"M=M+1");

                return;
            }
            public void Sub(SubCommand command) {
                // Pop and Pop and Push
                _file.WriteLine($"// POP left");
                _file.WriteLine($"@SP");
                _file.WriteLine($"A=M-1");
                _file.WriteLine($"D=M");
                _file.WriteLine($"@SP");
                _file.WriteLine($"M=M-1");
                _file.WriteLine($"// POP right");
                _file.WriteLine($"@SP");
                _file.WriteLine($"A=M-1");
                _file.WriteLine($"D=M-D");                
                _file.WriteLine($"@SP");
                _file.WriteLine($"M=M-1");
                _file.WriteLine($"// Push a+b");
                _file.WriteLine($"A=M");
                _file.WriteLine($"M=D");
                _file.WriteLine($"@SP");
                _file.WriteLine($"M=M+1");
            }
        }
    }

    class Debugger {
        public static void WriteLine(string any) {
            Console.Error.WriteLine($"Debug:{any}");
        }
    }
}
