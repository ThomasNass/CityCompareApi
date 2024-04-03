using CityCompareProxy.Models;

namespace CityCompareProxy.Helper
{
    public class Mapper
    {
        public static HousePrices MapScbResponseToHousePrices(ScbResponse scbResponse)
        {
            HousePrices housePrices = new HousePrices
            {
                Id = Guid.NewGuid(), // Generate a new Guid for HousePrices
                LastUpdateDate = DateTime.Now, // Set the LastUpdateDate to current date/time
                Items = new List<Data>()
            };

            // Generate a new Guid to be used as the parent id
            Guid parentId = housePrices.Id;

            // Map DataItems to Data objects and set the ParentId
            if (scbResponse.Data != null)
            {
                foreach (var dataItem in scbResponse.Data)
                {
                    housePrices.Items.Add(MapDataItemToData(dataItem, parentId));
                }
            }

            return housePrices;
        }

        private static Data MapDataItemToData(DataItem dataItem, Guid id)
        {
            return new Data
            {
                ParentId =id,
                Id = Guid.NewGuid(), // Generate a new Guid for Data
                Key = dataItem.Key,
                Values = dataItem.Values
            };
        }
    }
}
