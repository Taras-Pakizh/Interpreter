using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseModule
{
    public interface IExpression
    {
        IExpressionData Evaluate();
    }
}
