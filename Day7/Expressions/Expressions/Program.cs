using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace Expressions
{
    class Program
    {
        static void Main(string[] args)
        {
            ParameterExpression numParam = Expression.Parameter(typeof(int), "num");
            ConstantExpression five = Expression.Constant(5, typeof(int));
            BinaryExpression numLessThanFive = Expression.LessThan(numParam, five);
            Expression<Func<int, bool>> lambda1 =
                Expression.Lambda<Func<int, bool>>(
                    numLessThanFive,
                    new ParameterExpression[] { numParam });
            Func < int, bool> lambda2 = num => num < 5;

            Console.WriteLine(lambda1.Compile()(4).ToString());
            Console.WriteLine(lambda2(4).ToString());
        }
    }
}
