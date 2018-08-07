using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexerModule
{
    public class Lexer
    {
        //Vars
        private List<Token> tokens;
        private string input;
        private int position;

        //Properties

        //Constructor
        public Lexer(string code)
        {
            input = code;
            tokens = new List<Token>();
            position = -1;
        }

        //Methods public
        public List<Token> Tokenize()
        {
            string temp;
            while(position < input.Length)
            {
                temp = NextChar();
                if (temp.Length == 0) break;
                if (Char.IsWhiteSpace(temp, 0)) continue;
                tokens.Add(GetToken(temp));
            }

            return tokens;
        }

        //Methods private
        private string Peek()
        {
            if (position >= input.Length) return "";
            return input[position].ToString();
        }
        private string Peek(int move)
        {
            if (position + move >= input.Length || position + move < 0) return "";
            return input[position + move].ToString();
        }
        private string NextChar()
        {
            ++position;
            return Peek();
        }
        private Token GetToken(string symbol)
        {
            TokenType type = TokenDictionary.GetTokenType(symbol[0]);
            string text = String.Copy(symbol);
            if (type == TokenType.Unknown)
            {
                if (Char.IsDigit(symbol, 0))
                {
                    type = TokenType.Double;
                    text = GetDouble();
                }
                else if (Char.IsLetter(symbol, 0) || symbol[0] == '_')
                {
                    type = TokenType.Word;
                    text = GetWord();
                }
            }

            string temp = Peek(1);
            if (type == TokenType.Quotes)
            {
                type = TokenType.String;
                text = GetString();
            }
            else if(type == TokenType.Not && temp != "" && temp[0] == '=')
            {
                type = TokenType.NotEqual;
                text = "!=";
                ++position;
            }
            else if(temp != "" && temp[0] == '=')
            {
                if (type == TokenType.Equal) { type = TokenType.DoubleEqual; text = "=="; position++; }
                else if (type == TokenType.Bigger) { type = TokenType.BiggerEqual; text = ">="; position++; }
                else if (type == TokenType.Smaller) { type = TokenType.SmallerEqual; text = "<="; position++; }
            }
            FindKeyWords();

            return new Token(text, type);
        }

        //Tokenizes
        private string GetString()
        {
            StringBuilder buffer = new StringBuilder();
            string temp = NextChar();
            while(temp[0] != '"')
            {
                buffer.Append(temp[0]);
                temp = NextChar();
                if (temp == "") break;
            }
            return buffer.ToString();
        }
        private string GetWord()
        {
            StringBuilder buffer = new StringBuilder();
            string temp = Peek();
            while(Char.IsLetterOrDigit(temp[0]) || temp[0] == '_')
            {
                buffer.Append(temp[0]);
                temp = NextChar();
                if (temp == "") break;
            }
            --position;
            return buffer.ToString();
        }
        private string GetDouble()
        {
            StringBuilder buffer = new StringBuilder();
            string temp = Peek();
            while (Char.IsDigit(temp[0]) || temp[0] == '.')
            {
                buffer.Append(temp[0]);
                temp = NextChar();
                if (temp == "") break;
            }
            --position;
            return buffer.ToString();
        }
        private void FindKeyWords()
        {
            for(int i = 0; i < tokens.Count; ++i)
            {
                if (tokens[i].Type == TokenType.Word)
                    tokens[i].Type = TokenDictionary.GetWordType(tokens[i].Text);
                if (tokens[i].Type == TokenType.Word && i != tokens.Count - 1 && tokens[i + 1].Type == TokenType.LParam)
                    tokens[i].Type = TokenType.Function;
            }
        }
    }
}
