using System.ComponentModel.DataAnnotations;

namespace CityCompareProxy.Models
{
    public class City
    {
        [Key]
        public string Id { get; set; }
        public HousePrices HousePrices { get; set; }
    }

    public class HousePrices
    {
        public Guid Id { get; set; }
        public string CityId { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public List<Data> Items { get; set; }

    }

    public class Data
    {
        public Guid Id { get; set; }
        public Guid ParentId { get; set; }
        public List<string>? Key { get; set; }
        public List<string>? Values { get; set; }
    }


}
