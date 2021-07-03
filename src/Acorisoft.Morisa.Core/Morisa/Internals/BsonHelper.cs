using LiteDB;

namespace Acorisoft.Morisa.Internals
{
    internal class BsonHelper
    {
        public static BsonExpression Eq(BsonValue value)
        {
            return Query.EQ(Constants.IdMoniker, value);
        }
    }
}