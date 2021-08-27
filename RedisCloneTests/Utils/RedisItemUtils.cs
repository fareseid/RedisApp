using RedisServer.Model.RedisModel;

namespace RedisCloneTests.Utils
{
    public static class RedisItemUtils
    {
        public static RedisListItem BuildRedisListItem(params string[] StringItems)
        {
            RedisListItem RedisListItem = new RedisListItem();
            for (int i = 0; i < StringItems.Length; i++)
            {
                RedisListItem.PushRight(BuildRedisStringItem(StringItems[i]));
            }
            return RedisListItem;
        }
        public static RedisStringItem BuildRedisStringItem(string StringValue)
        {
            RedisStringItem StringItem = new RedisStringItem();
            StringItem.SetValue(StringValue);
            return StringItem;
        }
        public static RedisIntItem BuildRedisIntItem(int IntValue)
        {
            RedisIntItem StringItem = new RedisIntItem();
            StringItem.SetValue(IntValue);
            return StringItem;
        }
    }
}
