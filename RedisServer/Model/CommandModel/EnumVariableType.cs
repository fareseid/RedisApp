using RedisServer.Helper;
using RedisServer.Model.RedisModel;

namespace RedisServer.Model.CommandModel
{
    public static class VariableType { 
        public enum EnumVariableType
        {
            NA,
            STRING,
            INT,
            LIST
        }

        public static IRedisItem CreateItem(EnumVariableType VariableType)
        {
            switch (VariableType)
            {
                case EnumVariableType.STRING:
                    return new RedisStringItem();
                case EnumVariableType.INT:
                    return new RedisIntItem();
                case EnumVariableType.LIST:
                    return new RedisListItem();
                case EnumVariableType.NA:
                    return null;
                default:
                    throw new AppException("Invalid Command");
            }
        }

        public static EnumVariableType ToVariableType(string Value)
        {
            return int.TryParse(Value, out _) ? EnumVariableType.INT : EnumVariableType.STRING;
        } 

    }
}
