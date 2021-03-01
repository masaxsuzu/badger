using System;
using System.Collections.Generic;
using System.Linq;
namespace Netsoft.Badger.Assembler
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
            SymbolTable symbolTable = CreateSymbolTable(lines);
            var commands = GenerateCommands(lines, symbolTable);
            Debugger.WriteLine(symbolTable.ToString());
            GenerateCode(commands, symbolTable);
            return 0;
        }

        static SymbolTable CreateSymbolTable(string[] lines) {
            var symbolTable = new SymbolTable();
            int labelAddress = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                labelAddress = ParseSymbol(lines[i], symbolTable, labelAddress);
            }
            return symbolTable;
        }

        static IReadOnlyCollection<ICode> GenerateCommands(string[] lines, SymbolTable symbolTable) {
            var codes = new List<ICode>();

            foreach (var line in lines)
            {
                var code = ParseCode(line, symbolTable);
                if(code != null) {
                    codes.Add(code);
                }
            }
            return codes;
        }
        
        static void GenerateCode(IEnumerable<ICode> commands, SymbolTable symbolTable) {
            foreach (var command in commands)
            {
                Console.WriteLine(command.Generate(symbolTable));
            }
        }

        static int  ParseSymbol(string line, SymbolTable symbolTable, int labelAddress) {
            var chars = line.ToCharArray();
            int i = 0;
            while (i < chars.Length)
            {
                if (Char.IsWhiteSpace(chars[i])) {
                    i++;
                    continue;
                }
                switch (chars[i])
                {
                    // Comment
                    case '/':
                        if (chars[i+1] == '/') {
                            return labelAddress;
                        }
                        throw new Exception($"got {chars[i+1]}, want /");
                    // Label symbol
                    case '(':
                        var x = ParseLabel(line.AsSpan(i).ToArray());
                        symbolTable.AddLabelSymbol(x, (ushort) (labelAddress));
                        return labelAddress;
                    // A command
                    case '@':
                        ParseACommand(line.AsSpan(i).ToArray());
                        labelAddress++;
                        return labelAddress;
                    // C command
                    default:
                        ParseCCommand(line.AsSpan(i).ToArray());
                        labelAddress++;
                        return labelAddress;
                }
            }
            return labelAddress;
        }
        static ICode ParseCode(string line, SymbolTable symbolTable) {
            var chars = line.ToCharArray();
            int i = 0;
            while (i < chars.Length)
            {
                if (Char.IsWhiteSpace(chars[i])) {
                    i++;
                    continue;
                }
                switch (chars[i])
                {
                    // Comment
                    case '/':
                        if (chars[i+1] == '/') {
                            return null;
                        }
                        throw new Exception($"got {chars[i+1]}, want /");
                    // Label symbol
                    case '(':
                        var symbol = ParseLabel(line.AsSpan(i).ToArray());
                        return null;
                    // A command
                    case '@':
                        var aCommand = ParseACommand(line.AsSpan(i).ToArray());
                        symbolTable.AddSymbol(aCommand.Value);
                        return aCommand;
                    // C command
                    default:
                        // dest=comp;jump
                        // dest=comp
                        // comp;jump
                        var cCommand  = ParseCCommand(line.AsSpan(i).ToArray());
                        return cCommand;
                }
            }
            return null;
        }

        static string ParseLabel(Char[] chars) {
            for (int i = 0; i < chars.Length; i++)
            {
                if (chars[i] == ')') {
                    return new string(chars.AsSpan(1, i - 1).ToArray());
                }
            }
            throw new Exception($"got {chars[chars.Length - 1]}, want )");
        }
        static ACommand ParseACommand(Char[] chars) {
            for (int i = 0; i < chars.Length; i++)
            {
                if (Char.IsWhiteSpace(chars[i]) || (chars[i] == '/' && chars[i+1] == '/')) {
                    return new ACommand(new string(chars.AsSpan(1, i - 1).ToArray()));
                }
            }
            return new ACommand(new string(chars.AsSpan(1, chars.Length - 1).ToArray()));
        }

        static CCommand ParseCCommand(Char[] chars) {
            var command = new string(chars.AsSpan(0, chars.Length).ToArray());
            for (int i = 0; i < chars.Length; i++) {
                if (Char.IsWhiteSpace(chars[i]) || (chars[i] == '/' && chars[i+1] == '/')) {
                    command = new string(chars.AsSpan(0, i).ToArray());
                    break;
                }
            }
            
            if (!command.Contains("=") && !command.Contains(";")) {
                throw new Exception("= or ; must be included");
            }
            string dest = null;
            string comp = null;
            string jump = null;
            if (command.Contains("=") && !command.Contains(";")) {
                var x1 = command.Split(new char[] {'='});
                dest = x1[0];
                comp= x1[1];
            }
            if (!command.Contains("=") && command.Contains(";")) {
                var x2 = command.Split(new char[] {';'});
                comp = x2[0];
                jump = x2[1];
            }
            if (command.Contains("=") && command.Contains(";")) {
                var x3 = command.Split(new char[] {'=',';'});
                dest = x3[0];
                comp = x3[1];
                jump = x3[2];
            }

            return new CCommand(dest, comp, jump);
        }
    }

    public class SymbolTable {
        ushort _variableAddress;
        private Dictionary<string,ushort> _table;
        public SymbolTable() {
            _variableAddress = 16;
            _table = new Dictionary<string, ushort>();
            _table.Add("SP", 0);
            _table.Add("LCL", 1);
            _table.Add("ARG", 2);
            _table.Add("THIS", 3);
            _table.Add("THAT", 4);
            _table.Add("R0", 0);
            _table.Add("R1", 1);
            _table.Add("R2", 2);
            _table.Add("R3", 3);
            _table.Add("R4", 4);
            _table.Add("R5", 5);
            _table.Add("R6", 6);
            _table.Add("R7", 7);
            _table.Add("R8", 8);
            _table.Add("R9", 9);
            _table.Add("R10", 10);
            _table.Add("R11", 11);
            _table.Add("R12", 12);
            _table.Add("R13", 13);
            _table.Add("R14", 14);
            _table.Add("R15", 15);
            _table.Add("SCREEN", 16384);
            _table.Add("KBD", 24576);
        }
        public void AddLabelSymbol(string name, ushort address) {
            if(!_table.ContainsKey(name)) {
                _table.Add(name, address);
            }
        }

        public void AddSymbol(string name) {
            if (ushort.TryParse(name, out ushort n)) {
                return;
            }
            if (!_table.ContainsKey(name)) { 
                _table.Add(name, _variableAddress);
                _variableAddress++;
            }
        }

        public ushort Find(string name) {
            return _table[name];
        }

        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();
            sb.Append("{");
            foreach (var item in _table.Select((v, i) => new {i, v}))
            {
                sb.Append($"\"{item.v.Key}\": {item.v.Value}");
                if(item.i < _table.Count - 1) {
                    sb.Append(", ");
                }
            }
            sb.Append("}");
            return sb.ToString();
        }
    }

    interface ICode {
        string Generate(SymbolTable symbolTable);
    }

    public class ACommand : ICode {
        public string Value {get; set;}
        public ACommand(string value) {
            Value = value;
        }
        public override string ToString()
        {
            return $"{Value}";
        }

        public string Generate(SymbolTable symbolTable) {
            if (ushort.TryParse(Value, out ushort n)) {
                return Convert.ToString(n,2).PadLeft(16, '0');
            }
            var x = symbolTable.Find(Value);
            return Convert.ToString(x,2).PadLeft(16, '0');
        }
    }
    public class CCommand : ICode {
        public string Dest {get; set;}
        public string Comp {get; set;}
        public string Jump {get; set;}

        public CCommand(string dest, string comp, string jump) {
            Dest = dest;
            Comp = comp;
            Jump = jump;
        }
        public override string ToString()
        {
            return $"{Dest}={Comp};{Jump}";
        }

        public string Generate(SymbolTable symbolTable) {
            var d = Dest switch
            {
                "M" => "001",
                "D" => "010",
                "MD" => "011",
                "A" => "100",
                "AM" => "101",
                "AD" => "110",
                "AMD" => "111",
                _ => "000"
            };

            var j = Jump switch
            {
                "JGT" => "001",
                "JEQ" => "010",
                "JGE" => "011",
                "JLT" => "100",
                "JNE" => "101",
                "JLE" => "110",
                "JMP" => "111",
                _ => "000"
            };

            var c = Comp switch {
                "0"     => "0" + "101010",
                "1"     => "0" + "111111",
                "-1"    => "0" + "111010",
                "D"     => "0" + "001100",
                "A"     => "0" + "110000",
                "!D"    => "0" + "001101",
                "!A"    => "0" + "110001",
                "-D"    => "0" + "001111",
                "-A"    => "0" + "110011",
                "D+1"   => "0" + "011111",
                "A+1"   => "0" + "110111",
                "D-1"   => "0" + "001110",
                "A-1"   => "0" + "110010",
                "D+A"   => "0" + "000010",
                "D-A"   => "0" + "010011",
                "A-D"   => "0" + "000111",
                "D&A"   => "0" + "000000",
                "D|A"   => "0" + "010101",
                "M"     => "1" + "110000",
                "!M"    => "1" + "110001",
                "-M"    => "1" + "110011",
                "M+1"   => "1" + "110111",
                "M-1"   => "1" + "110010",
                "D+M"   => "1" + "000010",
                "D-M"   => "1" + "010011",
                "M-D"   => "1" + "000111",
                "D&M"   => "1" + "000000",
                "D|M"   => "1" + "010101",
                _ => throw new Exception($"Invalid comp {Comp}")
            };
            return "111" + c + d + j;
        }
    }

    class Debugger {
        public static void WriteLine(string any) {
            // Console.Error.WriteLine($"Debug:{any}");
        }
    }
}
