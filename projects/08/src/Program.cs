using System;
using System.Linq;

namespace Netsoft.Badger.Compiler.Backend2
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length < 1) {
                Console.Error.WriteLine("No source code is provided!");
                return 1;
            }
            var name = System.IO.Path.GetFileNameWithoutExtension(args[0]);
            var lines = System.IO.File.ReadAllLines(args[0]);
            var sm = new StackMachine(name, Console.Out);
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

                string[] c = line.Split(' ',StringSplitOptions.RemoveEmptyEntries);
                switch (c[0])
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
                        return new PopCommand(c[1], int.Parse(c[2]));
                    case "push":
                        return new PushCommand(c[1], int.Parse(c[2]));
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
                machine.Pop(this);
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
            private string _name;
            private System.IO.TextWriter _file;
            public int RegisterId{ get; set;}
            public int SP{ get; set;}
            public int Label{ get; set;}
            public StackMachine(string name, System.IO.TextWriter file) {
                RegisterId = 0;
                Label = 0;
                _file = file;
                _name = name;
            }
            public void Eq(EqualCommand command) {
                var l1 = Label++;
                var l2 = Label++;
                Compare("JEQ", l1, l2);
            }
            public void Lt(LessThanCommand command) {
                var l1 = Label++;
                var l2 = Label++;
                Compare("JLT", l1, l2);
            }
            public void Gt(GreaterThanCommand command) {
                var l1 = Label++;
                var l2 = Label++;
                Compare("JGT", l1, l2);
            }
            public void Neg(NegativeCommand command) {
                Unary("M=-M");
            }
            public void Not(NotCommand command) {
                 Unary("M=!M");
            }
            public void Add(AddCommand command) {
                Binary("D=M+D");
            }
            public void Sub(SubCommand command) {
                Binary("D=M-D");
            }
            public void And(AndCommand command) {
                 Binary("D=M&D");
            }
            public void Or(OrCommand command) {
                Binary("D=M|D");
            }
            public void Push(PushCommand command) {
                if(command.Arg1 == "constant") {
                    _file.WriteLine($"// Push constant {command.Arg2}");
                    _file.WriteLine($"@{command.Arg2}");
                    _file.WriteLine($"D=A");
                    this.PushFromDRegister();
                    return;
                }
                var segment = command.Arg1 switch {
                    "local" => "LCL",
                    "argument" => "ARG",
                    "this" => "THIS",
                    "that" => "THAT",
                    _ => ""
                };
                if (!string.IsNullOrEmpty(segment)) {
                    _file.WriteLine($"// Push argument {command.Arg2}");

                    _file.WriteLine($"@{segment}");
                    _file.WriteLine($"A=M");
                    for (int i = 0; i < command.Arg2; i++)
                    {
                        _file.WriteLine($"A=A+1");
                    }
                    _file.WriteLine($"D=M");
                    this.PushFromDRegister();
                    return;
                }
                var baseAddress = command.Arg1 switch {
                    "temp" => "5",
                    "pointer" => "3",
                    _ => ""
                };
                if (!string.IsNullOrEmpty(baseAddress)) {
                    _file.WriteLine($"@{baseAddress}");
                    for (int i = 0; i < command.Arg2; i++)
                    {
                        _file.WriteLine($"A=A+1");
                    }
                    _file.WriteLine($"D=M");
                    this.PushFromDRegister();
                    return;
                }
                if (command.Arg1 == "static") {
                    _file.WriteLine($"@{_name}.{command.Arg2}");
                    _file.WriteLine($"D=M");
                    this.PushFromDRegister();
                    return;
                }
            }
            public void Pop(PopCommand command) {
                _file.WriteLine($"// Pop {command.Arg1} {command.Arg2}");

                var segment = command.Arg1 switch {
                    "local" => "LCL",
                    "argument" => "ARG",
                    "this" => "THIS",
                    "that" => "THAT",
                    _ => ""
                };
                if (!string.IsNullOrEmpty(segment)) {
                    this.PopToARegister();
                    _file.WriteLine("D=M");
                    _file.WriteLine($"@{segment}");
                    _file.WriteLine($"A=M");
                    for (int i = 0; i < command.Arg2; i++)
                    {
                        _file.WriteLine($"A=A+1");
                    }
                    _file.WriteLine($"M=D");
                    return;
                }

                var baseAddress = command.Arg1 switch {
                    "temp" => "5",
                    "pointer" => "3",
                    _ => ""
                };
                if (!string.IsNullOrEmpty(baseAddress)) {
                    this.PopToARegister();
                    _file.WriteLine($"D=M");
                    _file.WriteLine($"@{baseAddress}");
                    for (int i = 0; i < command.Arg2; i++)
                    {
                        _file.WriteLine($"A=A+1");
                    }
                    _file.WriteLine($"M=D");
                    return;
                }

                if (command.Arg1 == "static") {
                    this.PopToARegister();
                    _file.WriteLine($"D=M");
                    _file.WriteLine($"@{_name}.{command.Arg2}");
                    _file.WriteLine($"M=D");
                    return;
                }
                Debugger.WriteLine($"\"{command.Arg1}\"");
            }

            private void Compare(string cond, int l1, int l2) {
                this.PopToARegister();
                _file.WriteLine($"D=M");
                this.PopToARegister();
                _file.WriteLine($"D=M-D");
                _file.WriteLine($"@LABEL{l1}");
                _file.WriteLine($"D;{cond}");
                _file.WriteLine($"D=0");
                _file.WriteLine($"@LABEL{l2}");
                _file.WriteLine($"0;JMP");
                _file.WriteLine($"(LABEL{l1})");
                _file.WriteLine($"D=-1");
                _file.WriteLine($"(LABEL{l2})");
                this.PushFromDRegister();
            }
            
            private void Unary(string op) {
                _file.WriteLine("@SP");
                _file.WriteLine("A=M-1");
                _file.WriteLine($"{op}");
            }
            private void Binary(string op) {
                this.PopToARegister();
                _file.WriteLine("D=M");
                this.PopToARegister();
                _file.WriteLine($"{op}");
                this.PushFromDRegister();
            }
            private void PushFromDRegister() {
                _file.WriteLine($"@SP");
                _file.WriteLine($"A=M");
                _file.WriteLine($"M=D");
                _file.WriteLine($"@SP");
                _file.WriteLine($"M=M+1");
            }
            private void PopToARegister() {
                _file.WriteLine($"@SP");
                _file.WriteLine($"M=M-1");
                _file.WriteLine($"A=M");
            }
        }
    }

    class Debugger {
        public static void WriteLine(string any) {
            Console.Error.WriteLine($"Debug:{any}");
        }
    }
}
