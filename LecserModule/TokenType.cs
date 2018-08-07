using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexerModule
{
    public enum TokenType
    {
        //main
        Semicolon = 0,
        Comma,
        Equal,
        Unknown,

        //words
        Var,
        Word,
        IF, FOR, WHILE, ELSE, CONTINUE, BREAK,
        Function,

        //operations
        Plus,
        Minus,
        Star,
        Slash,
        DoubleEqual,
        NotEqual,
        Not,
        Bigger,
        Smaller,
        BiggerEqual,
        SmallerEqual,
        OR,
        AND,

        //data
        Double,
        String,

        //Brackets
        Quotes,
        LBracket,
        RBracket,
        LParam,
        RParam,
        LBlock,
        RBlock
    }
}
