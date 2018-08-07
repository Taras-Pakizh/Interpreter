using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexerModule
{
    public class Token
    {
        //Vars
        private string info;
        private TokenType type;

        //Properties
        public string Text { get { return info; } }
        public TokenType Type { get { return type; } set { type = value; } }

        //Constructor
        public Token(string _info, TokenType _type)
        {
            info = _info;
            type = _type;
        }
    }
}
