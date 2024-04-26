using CityCompareProxy.Models;
using System.Reflection;

namespace CityCompareProxy.Helper
{
    public class Mapper
    {
        public static T MapScbResponseToEntity<T>(ScbResponse scbResponse, Func<ScbResponse, T> entityMapper)
        {
            T entity = entityMapper(scbResponse);
            PropertyInfo parentIdProperty = typeof(T).GetProperty("Id");

            if (entity != null && parentIdProperty != null)
            {
                Guid parentId = (Guid)parentIdProperty.GetValue(entity);

                foreach (var dataItem in scbResponse.Data ?? Enumerable.Empty<DataItem>())
                {
                    PropertyInfo itemsProperty = typeof(T).GetProperty("Items");
                    if (itemsProperty != null)
                    {
                        var itemsList = (List<Data>)itemsProperty.GetValue(entity);
                        itemsList.Add(MapDataItemToData(dataItem, parentId));
                    }
                }
            }

            return entity;
        }

        private static Data MapDataItemToData(DataItem dataItem, Guid parentId)
        {
            return new Data
            {
                ParentId = parentId,
                Id = Guid.NewGuid(),
                Key = dataItem.Key,
                Values = dataItem.Values
            };
        }
    }
}
