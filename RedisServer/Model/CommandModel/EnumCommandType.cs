namespace RedisServer.Model.CommandModel
{
    public static class CommandType
    {
        public enum EnumCommandType
        {
            SET,
            GET,
            INCR,
            DECR,
            RPUSH,
            RPOP,
            LPUSH,
            LPOP,
            LINDEX,
            EXPIRE
        }

        public static bool IsBlockingCommand(EnumCommandType CommandType)
        {
            switch (CommandType)
            {
                case EnumCommandType.LINDEX:
                case EnumCommandType.GET:
                    return false;
                default:
                    return true;
            }
        } 
    }
}
