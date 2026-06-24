namespace UsersApi.Common.Querying
{
    public class QueryOptions
    {
        public string FilterExpression { get; set; } = string.Empty;
        public string OrderByExpression { get; set; } = string.Empty;
        public int Skip { get; set; }
        public int Take { get; set; } = 10;
        public string[] Parameters { get; set; } = [];
    }
}
