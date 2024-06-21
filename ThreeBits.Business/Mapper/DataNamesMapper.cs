using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreeBits.Business.Mapper;

namespace ThreeBits.Business.Common
{
    public class DataNamesMapper<TDB> where TDB : class, new()
    {
        public TDB Map(DataRow row)
        {
            TDB entity = new TDB();
            return Map(row, entity);
        }

        public TDB Map(DataRow row, TDB entity)
        {
            var columnNames = row.Table.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();
            var properties = (typeof(TDB)).GetProperties()
                                              .Where(x => x.GetCustomAttributes(typeof(DataNamesAttribute), true).Any())
                                              .ToList();
            foreach (var prop in properties)
            {
                PropertyMapHelper.Map(typeof(TDB), row, prop, entity);
            }

            return entity;
        }

        public IEnumerable<TDB> Map(DataTable table)
        {
            List<TDB> entities = new List<TDB>();
            var columnNames = table.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();
            var properties = (typeof(TDB)).GetProperties()
                                              .Where(x => x.GetCustomAttributes(typeof(DataNamesAttribute), true).Any())
                                              .ToList();
            foreach (DataRow row in table.Rows)
            {
                TDB entity = new TDB();
                foreach (var prop in properties)
                {
                    PropertyMapHelper.Map(typeof(TDB), row, prop, entity);
                }
                entities.Add(entity);
            }

            return entities;
        }

        public IEnumerable<TDB> Map(DataRow[] rows)
        {
            List<TDB> entities = new List<TDB>();

            if (rows.Length > 0)
            {
                var columnNames = rows[0].Table.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();
                var properties = (typeof(TDB)).GetProperties()
                                                  .Where(x => x.GetCustomAttributes(typeof(DataNamesAttribute), true).Any())
                                                  .ToList();
                foreach (DataRow row in rows)
                {
                    TDB entity = new TDB();
                    foreach (var prop in properties)
                    {
                        PropertyMapHelper.Map(typeof(TDB), row, prop, entity);
                    }
                    entities.Add(entity);
                }
            }
            return entities;
        }
    }
}
