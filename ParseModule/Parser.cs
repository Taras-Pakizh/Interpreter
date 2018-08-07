using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LexerModule;
using ParseModule.Statements;
using ParseModule.Expressioons;
using System.Collections;

namespace ParseModule
{
    public class Parser
    {
        //Vars
        private int position;
        private List<Token> tokens;

        //Constructor
        public Parser(List<Token> list)
        {
            tokens = list;
            position = 0;
        }

        //Methods public
        private IStatement Parse()
        {
            BlockStatement program = new BlockStatement();
            while(position < tokens.Count)
                program.AddStatement(GetStatement());
            return program;
        }
        public string Run()
        {
            IStatement result = Parse();
            result.Execute();
            return ConsoleBuffer.GetBuffer();
        }
        public void Reset()
        {
            VariablesDictionary.Reset();
            LoopStack.Reset();
            ConsoleBuffer.Reset();
        }

        //Methods statement
        private IStatement GetStatement()
        {
            if (MatchToken(TokenType.IF))
                return CreateIf();
            else if (MatchToken(TokenType.WHILE))
                return CreateLoop();
            else if (MatchToken(TokenType.Var))
                return CreateAssignment();
            else if (Peek().Type == TokenType.Word)
                return CreateEqual();
            else if (MatchToken(TokenType.CONTINUE) && MatchToken(TokenType.Semicolon))
                return new ControlStatement("continue");
            else if (MatchToken(TokenType.BREAK) && MatchToken(TokenType.Semicolon))
                return new ControlStatement("break");
            else if (Peek().Type == TokenType.Function)
                return new FunctionStatement(CreateExpression());
            else throw new Exception("Unknown statement");
        }
        private IStatement CreateAssignment()
        {
            Token varName = Peek();
            if (MatchToken(TokenType.Word) && MatchToken(TokenType.Equal))
                return new AssigmentStatement(varName.Text, CreateExpression());
            else throw new Exception("Syntax error. CreateAssigment");
        }
        private IStatement CreateEqual()
        {
            Token varName = Peek();
            if (MatchToken(TokenType.Word) && MatchToken(TokenType.Equal))
                return new EqualStatement(varName.Text, CreateExpression());
            else if (Peek().Type == TokenType.LBracket)
                return new EqualStatement(varName.Text, Indexes(), CreateExpression());
            else throw new Exception("Syntax error. CreateEqual");
        }
        private IStatement CreateIf()
        {
            IExpression condition = CreateExpression();
            IStatement ifstat = CreateStatementOrBlock();
            IStatement elsestat = null;
            if (MatchToken(TokenType.ELSE))
                elsestat = CreateStatementOrBlock();
            return new IfStatement(condition, ifstat, elsestat);
        }
        private IStatement CreateLoop()
        {
            IExpression condition = CreateExpression();
            IStatement block = CreateStatementOrBlock();
            if (block is BlockStatement)
                ((BlockStatement)block).AddStatement(new ControlStatement("loop"));
            return new LoopStatement(condition, block);
        }
        private IStatement CreateStatementOrBlock()
        {
            if (Peek().Type == TokenType.LBlock) return CreateBlock();
            return GetStatement();
        }
        private IStatement CreateBlock()
        {
            BlockStatement block = new BlockStatement();
            if (!MatchToken(TokenType.LBlock)) throw new Exception("Block has no LBlock. CreateBlock");
            while (!MatchToken(TokenType.RBlock))
                block.AddStatement(GetStatement());
            return block;
        }

        //Methods expressions  add , [] ArrayIndexObject
        private IExpression CreateExpression()
        {
            IExpression result =  Logical();
            MatchToken(TokenType.Semicolon);
            return result;
        }
        private IExpression Logical()
        {
            IExpression result = Condition();
            while (true)
            {
                if (MatchToken(TokenType.AND))
                {
                    result = new ConditionExpression("&", result, Condition());
                    continue;
                }
                if (MatchToken(TokenType.OR))
                {
                    result = new ConditionExpression("|", result, Condition());
                    continue;
                }
                break;
            }
            return result;
        }
        private IExpression Condition()
        {
            IExpression result = Additive();
            while (true)
            {
                if (MatchToken(TokenType.NotEqual))
                {
                    result = new ConditionExpression("!=", result, Additive());
                    continue;
                }
                if (MatchToken(TokenType.DoubleEqual))
                {
                    result = new ConditionExpression("==", result, Additive());
                    continue;
                }
                if (MatchToken(TokenType.Bigger))
                {
                    result = new ConditionExpression(">", result, Additive());
                    continue;
                }
                if (MatchToken(TokenType.Smaller))
                {
                    result = new ConditionExpression("<", result, Additive());
                    continue;
                }
                if (MatchToken(TokenType.BiggerEqual))
                {
                    result = new ConditionExpression(">=", result, Additive());
                    continue;
                }
                if (MatchToken(TokenType.SmallerEqual))
                {
                    result = new ConditionExpression("<=", result, Additive());
                    continue;
                }
                break;
            }
            return result;
        }
        private IExpression Additive()
        {
            IExpression result = Multiplicative();
            while (true)
            {
                if (MatchToken(TokenType.Plus))
                {
                    result = new BinaryExpression('+', result, Multiplicative());
                    continue;
                }
                if (MatchToken(TokenType.Minus))
                {
                    result = new BinaryExpression('-', result, Multiplicative());
                    continue;
                }
                break;
            }
            return result;
        }
        private IExpression Multiplicative()
        {
            IExpression result = Unary();
            while (true)
            {
                if (MatchToken(TokenType.Star))
                {
                    result = new BinaryExpression('*', result, Unary());
                    continue;
                }
                if (MatchToken(TokenType.Slash))
                {
                    result = new BinaryExpression('/', result, Unary());
                    continue;
                }
                break;
            }
            return result;
        }
        private IExpression Unary()
        {
            if (MatchToken(TokenType.Minus)) return new UnaryExpression('-', Primary());
            if (MatchToken(TokenType.Plus)) return Primary();
            if (MatchToken(TokenType.Not)) return new UnaryExpression('!', Primary());
            return Primary();
        }
        private IExpression Primary()
        {
            Token current = Peek();
            if (MatchToken(TokenType.Double)) return new ValueExpression(Double.Parse(current.Text));
            if (MatchToken(TokenType.String)) return new ValueExpression(current.Text);
            if (MatchToken(TokenType.LParam))
            {
                IExpression result = CreateExpression();
                if (MatchToken(TokenType.RParam))
                    return result;
                else throw new Exception("There is no RParam");
            }
            if (MatchToken(TokenType.Word))
            {
                if (Peek().Type == TokenType.LBracket)
                    return Brackets(current.Text);
                return new VariableExpression(current.Text);
            }
            if (Peek().Type == TokenType.Function) return new FunctionExpression(current.Text, Comma());

            throw new Exception("Unknown Token");
        }

        //Parse function
        private List<IExpression> Comma()
        {
            if (!MatchToken(TokenType.Function) || !MatchToken(TokenType.LParam)) throw new Exception("Called for not existing function");
            List<IExpression> result = new List<IExpression>();
            if (MatchToken(TokenType.RParam)) return result;
            do
            {
                result.Add(Logical());
                if (MatchToken(TokenType.RParam)) break;
            } while (MatchToken(TokenType.Comma));
            return result;
        }

        //Parse Brackets
        private VariableExpression Brackets(string word)
        {
            return new VariableExpression(word, ParseBrackets());
        }
        private List<IExpression> Indexes()
        {
            List<IExpression> list = ParseBrackets();
            if (MatchToken(TokenType.Equal)) return list;
            else throw new Exception("There is not equl statement");
        }
        private List<IExpression> ParseBrackets()
        {
            List<IExpression> list = new List<IExpression>();
            while (true)
            {
                if (MatchToken(TokenType.LBracket))
                {
                    list.Add(CreateExpression());
                    if (!MatchToken(TokenType.RBracket))
                        throw new Exception("Brackets are not closed");
                }
                else break;
            }
            return list;
        }

        //Methods tokens
        private Token Peek()
        {
            return Peek(0);
        }
        private Token Peek(int move)
        {
            if (position + move >= tokens.Count) return null;
            return tokens[position + move];
        }
        private Token NextToken()
        {
            ++position;
            return Peek(0);
        }
        private bool MatchToken(TokenType type)
        {
            if (Peek() == null) return false;
            else if (Peek().Type == type) { ++position; return true; }
            return false;
        }
    }
}
