using System.ComponentModel.DataAnnotations;

namespace CityCompareProxy.Models
{
    public class City
    {
        [Key]
        public Guid Id { get; set; }
        public string LauCode { get; set; }
        public string Name { get; set; }
        public HousePrices HousePrices { get; set; }
        public PopulationGrowth PopulationGrowth { get; set; }
        public Income Income { get; set; }
        public PopulationGenderData PopulationGenderData { get; set; }
        public ElectionData ElectionData { get; set; }
        public MunicipalityElectionData MunicipalityElectionData { get; set; }
    }

    public class Data
    {
        public Guid Id { get; set; }
        public Guid ParentId { get; set; }
        public List<string>? Key { get; set; }
        public List<string>? Values { get; set; }
        public string Year => Key[1];
        public double Value => Values.Count > 0 && double.TryParse(Values[0], out var result) ? result : 0;
    }

    public interface IDataEntity
    {
        Guid Id { get; set; }
        Guid CityId { get; set; }
        List<Data> Items { get; set; }
    }

    public abstract class DataEntity : IDataEntity
    {
        public Guid Id { get; set; }
        public Guid CityId { get; set; }
        public List<Data> Items { get; set; }
        public DateTime LastUpdateDate { get; set; }
    }
    public class HousePrices : DataEntity
    {
    }

    public class PopulationGrowth :DataEntity
    {
    }
    public class Income :DataEntity
    {
    }
    public class PopulationGenderData :DataEntity
    {
    }
    public class ElectionData :DataEntity
    {
    }
    public class MunicipalityElectionData :DataEntity
    {
    }


}
