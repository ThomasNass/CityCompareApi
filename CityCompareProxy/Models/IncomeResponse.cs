namespace CityCompareProxy.Models
{
    public class IncomeResponse
    {
        public QueryItem[] Query { get; set; }
        public ResponseItem Response { get; set; }
    }

    public class QueryItem
    {
        public string Code { get; set; }
        public SelectionItem Selection { get; set; }
    }

    public class SelectionItem
    {
        public string Filter { get; set; }
        public string[] Values { get; set; }
    }

    public class ResponseItem
    {
        public string Format { get; set; }
    }
}
