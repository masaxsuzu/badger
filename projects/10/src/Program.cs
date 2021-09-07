using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Netsoft.Badger.Compiler.Frontend
{
    class Program
    {
        static int Main(string[] args)
        {
            string content = "";
            try
            {
                var filePath = args[0];
                content = System.IO.File.ReadAllText(filePath);
                var tokens = Tokenize(content);

                TestTokens(Console.Out, tokens);
                return 0;
            }
            catch(TokenizeException tex)
            {
                PointToPosition(Console.Error, content, tex.Pos, tex.Error);
                return 1;
            }
        }
        
        static void TestTokens(TextWriter writer, Token[] tokens)
        {
            writer.WriteLine("<tokens>");
            foreach (var token in tokens)
            {
                var literal = token.Literal == "<"
                    ? "&lt;"
                    : token.Literal == ">"
                    ? "&gt;"
                    : token.Literal;
                Console.WriteLine($"<{token.Type}> {literal} </{token.Type}>");
            }
            writer.WriteLine("</tokens>");
        }

        static void PointToPosition(TextWriter writer, string content, int pos, string message)
        {
            var (n,p) = LineNumber(content,pos);
            var line = content.Split(new char[]{'\n'}).ToArray()[n];
            writer.WriteLine($":{n+1}");
            writer.WriteLine($"{line}");
            writer.Write($"{string.Join(' ' ,Enumerable.Range(0,p+1).Select(_ => "").ToArray())}");
            writer.WriteLine($"^ {message}");
        }
        static (int,int) LineNumber(string content, int pos)
        {
            var lines = content.Split(new char[]{'\n'}).ToArray();
            var start = 0;
            var end = -1;
            for (int i = 0; i < lines.Length; i++)
            {
                start = end+1;
                end = start + lines[i].Length; // \n
                if(start <= pos && pos <= end)
                {
                    return (i, pos - start);
                }
            }
            return (lines.Length -1 ,content.Length);
        }
        static Token[] Tokenize(string content){
            var tokenizer = new Lexer(content);
            Token token;
            var tokens = new List<Token>();
            do
            {
                token = tokenizer.NextToken();
                tokens.Add(token);
            } while (token.Type != TokenType.eof);

            return tokens.Where(token => token.Type != TokenType.eof).ToArray();
        }
        static object Parse(object tokens){
            return tokens;
        }
    }
    class Lexer
    {
        private string _input;
        private int _currentPosition;
        private int _nextPosition;
        private char _ch;
        private string[] _keywords = new string[]
        {
            "class", "constructor", "function", "method", "field",
            "static", "var", "int", "char", "boolean", "void",
            "true", "false", "null", "this", "let",
            "do", "if", "else", "while", "return"
        };
        public Lexer(string input)
        {
            _input = input;
            _currentPosition = 0;
            _nextPosition = 0;
            _ch = (char)0;

            ReadChar();
        }

        private void ReadChar()
        {
            _ch = _nextPosition >= _input.Length ? (char)0 : _input[_nextPosition];
            _currentPosition = _nextPosition;
            _nextPosition += 1;
        }

        public Token NextToken()
        {
            SkipWhitespace();

            var token = new Token();
            var position = _currentPosition;
            switch (_ch)
            {
                case (char)0:
                    token.Position = position;
                    token.Type = TokenType.eof;
                    break;
                case '{':
                case '}':
                case '(':
                case ')':
                case '[':
                case ']':
                case '.':
                case ',':
                case ';':
                case '+':
                case '*':
                case '-':
                case '&':
                case '|':
                case '<':
                case '>':
                case '=':
                case '~':
                    token.Position = position;
                    token.Type = TokenType.symbol;
                    token.Literal = _ch.ToString();
                    break;
                case '/':
                    var n1 = Peek(1);
                    var n2 = Peek(2);

                    if(n1 == '*' && n2 == '*')
                    {
                        // /** comment */
                        ReadChar();
                        ReadChar();
                        ReadChar();
                        Until(new char[] { '*', '/'});
                        ReadChar();
                        ReadChar();
                        return NextToken();
                    }
                    else if(n1 == '*')
                    {
                        // /* comment */
                        ReadChar();
                        ReadChar();
                        Until(new char[] { '*', '/'});
                        ReadChar();
                        ReadChar();
                        return NextToken();
                    }
                    else if(n1 == '/')
                    {
                        // // comment
                        ReadChar();
                        ReadChar();
                        Until(new char[] { '\n' });
                        ReadChar();
                        return NextToken();
                    }
                    token.Position = position;
                    token.Type = TokenType.symbol;
                    token.Literal = _ch.ToString();
                    break;
                case '"':
                    token.Type = TokenType.stringConstant;
                    token.Position = position + 1;
                    token.Literal = ReadString();
                    break;
                default:
                    if (IsLetter(_ch))
                    {
                        token.Position = position;
                        (token.Type, token.Literal) = ReadKeywordOrIdentifier();
                        return token;
                    }
                    else if (IsDigit(_ch))
                    {
                        token.Position = position;
                        token.Literal = ReadNumber();
                        token.Type = TokenType.integerConstant;
                        return token;
                    }
                    throw new TokenizeException("Illegal token", _currentPosition);
            }
            ReadChar();
            return token;
        }

        private void SkipWhitespace()
        {
            while (_ch == ' ' || _ch == '\t' || _ch == '\n' || _ch == '\r')
            {
                 ReadChar();
            }
        }

        private void Until(char[] expected)
        {
            var n = expected.Length;
            var current = Enumerable.Range(0, n).Select(i => Peek(i)).ToArray();
            while (!Equals(expected, current))
            {
                if(current.First() == 0) {
                    throw new TokenizeException("Not found sequence", _currentPosition);
                }
                ReadChar();
                current = Enumerable.Range(0, n).Select(i => Peek(i)).ToArray();
            }
        }
        private bool Equals(char[] x, char[] y)
        {
            if(x.Length != y.Length) return false;
            for (int i = 0; i < x.Length; i++)
            {
                if(x[i]!= y[i]) return false;
            }
            return true;
        }

        private string ReadString()
        {
            var from = _currentPosition + 1;
            while (true)
            {
                ReadChar();
                if(_ch == '"')
                {
                    break;    
                }
                else if(_ch == 0)
                {
                    throw new TokenizeException("String must be closed", _currentPosition);
                }
            }
            return _input[from.._currentPosition];
        }

        private (TokenType, string) ReadKeywordOrIdentifier()
        {
            var from = _currentPosition;
            while(IsLetter(_ch))
            {
                ReadChar();
            }

            var literal = _input[from.._currentPosition];
            var type = _keywords.Contains(literal) ? TokenType.keyword : TokenType.identifier;

            return (type, literal);
        }

        private string ReadNumber()
        {
            var from = _currentPosition;
            while(IsDigit(_ch))
            {
                ReadChar();
            }
            return _input[from.._currentPosition];
        }

        private char Peek(int next)
        {
            var pos = _currentPosition+next;
            if(pos >= _input.Length) return (char)0;
            return _input[pos];
        }

        private bool IsLetter(char ch)
        {
            return ('a' <= ch && ch <= 'z') ||
                    ('A' <= ch && ch <= 'Z') ||
                    ch == '_';
        }

        private bool IsDigit(char ch)
        {
            return '0' <= ch && ch <= '9';
        }
    }

    class Token
    {
        public TokenType Type { get; set; }
        public string Literal {get; set;}
        public int Position {get; set;}

        public override string ToString()
        {
            return $"{{\"Type\":\"{Type}\", \"Literal\":\"{Literal}\", \"Position\":\"{Position}\"}}";
        }
    }

    enum TokenType
    {
        eof,
        symbol,
        keyword,
        integerConstant,
        stringConstant,
        identifier,
        illegal,
    }

    class TokenizeException: Exception
    {
        public string Error {get;set;}
        public int Pos {get;set;}
        public TokenizeException(string error, int pos):base()
        {
            Pos = pos;
            Error = error;
        }
    }
}
