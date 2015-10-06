using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Sonic
{
    public class MP3Provider : QueryProvider
    {
        /// <summary>
        /// Returns objects that match the Expression
        /// input
        /// </summary>
        public override object Execute(Expression expression)
        {
            //Get MethodCallExpression where original 
            //Expression would have been something
            //like :
            //
            // MP3.Files.Where<MP3>(mp3 => mp3.FileName.ToLower().Contains("prison"));
            MethodCallExpression mex = expression as MethodCallExpression;

            //get out the lambdaExpression
            Expression<Func<MP3,Boolean>> lambdaExpression =
                (Expression<Func<MP3, Boolean>>)
                    (mex.Arguments[1] as UnaryExpression).Operand;

            //get out the Func
            Func<MP3, Boolean> filter = lambdaExpression.Compile();

            //And now query the actual database using this filter

            //NOTE : To be honest we could have gone straight to the
            //QueryXML.GetMatchingMP3Files() method without this
            //QueryProvider....But I wanted to write it, to see
            //if I could understand QueryProviders a bit more.
            //So there.
            return XMLAndSQLQueryOperations.GetMatchingMP3Files(filter);
        }
    }
}
