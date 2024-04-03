namespace CityCompareProxy.Models
{
    
    public class ScbResponse
    {
        public List<Column>? Columns { get; set; }
        public List<DataItem>? Data { get; set; }
        public List<MetadataItem>? Metadata { get; set; }
    }

    public class Column
    {
        public string? Code { get; set; }
        public string? Text { get; set; }
        public string? Type { get; set; }
    }

    public class DataItem
    {
        public List<string>? Key { get; set; }
        public List<string>? Values { get; set; }
    }

    public class MetadataItem
    {
        public string? Infofile { get; set; }
        public DateTime Updated { get; set; }
        public string? Label { get; set; }
        public string? Source { get; set; }
    }
}
