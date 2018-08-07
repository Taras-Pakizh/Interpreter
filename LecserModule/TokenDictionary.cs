using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexerModule
{
    static class TokenDictionary
    {
        //Vars
        private static Dictionary<char, TokenType> dictionary = new Dictionary<char, TokenType>()
        {
            [';'] = TokenType.Semicolon,
            [','] = TokenType.Comma,
            ['='] = TokenType.Equal,
            ['+'] = TokenType.Plus,
            ['-'] = TokenType.Minus,
            ['*'] = TokenType.Star,
            ['/'] = TokenType.Slash,
            ['>'] = TokenType.Bigger,
            ['<'] = TokenType.Smaller,
            ['"'] = TokenType.Quotes,
            ['['] = TokenType.LBracket,
            [']'] = TokenType.RBracket,
            ['('] = TokenType.LParam,
            [')'] = TokenType.RParam,
            ['{'] = TokenType.LBlock,
            ['}'] = TokenType.RBlock,
            ['&'] = TokenType.AND,
            ['|'] = TokenType.OR,
            ['!'] = TokenType.Not,
        };
        private static Dictionary<string, TokenType> keyWords = new Dictionary<string, TokenType>()
        {
            ["if"] = TokenType.IF,
            ["for"] = TokenType.FOR,
            ["while"] = TokenType.WHILE,
            ["var"] = TokenType.Var,
            ["else"] = TokenType.ELSE,
            ["continue"] = TokenType.CONTINUE,
            ["break"] = TokenType.BREAK,
        };

        public static TokenType GetTokenType(char symbol)
        {
            TokenType result = TokenType.Unknown;
            foreach(var item in dictionary)
                if (item.Key == symbol) { result = item.Value; break; }
            return result;
        }
        public static TokenType GetWordType(string text)
        {
            TokenType result = TokenType.Word;
            foreach(var item in keyWords)
                if(item.Key == text) { result = item.Value; break; }
            return result;
        }
    }
}
