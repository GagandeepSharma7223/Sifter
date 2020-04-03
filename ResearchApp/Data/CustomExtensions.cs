using Kendo.Mvc;
using Kendo.Mvc.UI;
using Microsoft.EntityFrameworkCore;
using ResearchApp.Models;
using ResearchApp.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace ResearchApp.Data
{
    public static partial class CustomExtensions
    {
        public static IQueryable Query(this SifterContext context, string entityName) =>
            context.Query(context.Model.FindEntityType(entityName).ClrType);

        static readonly MethodInfo SetMethod = typeof(SifterContext).GetMethod(nameof(SifterContext.Set));

        public static IQueryable Query(this SifterContext context, Type entityType) =>
            (IQueryable)SetMethod.MakeGenericMethod(entityType).Invoke(context, null);

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
        (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public static IEnumerable<dynamic> DynamicListFromSql(this DbContext db, string Sql,
            Dictionary<string, object> Params, string fieldType = "string")
        {
            using (var cmd = db.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = Sql;
                if (cmd.Connection.State != ConnectionState.Open) { cmd.Connection.Open(); }

                foreach (KeyValuePair<string, object> p in Params)
                {
                    DbParameter dbParameter = cmd.CreateParameter();
                    dbParameter.ParameterName = p.Key;
                    dbParameter.Value = p.Value;
                    cmd.Parameters.Add(dbParameter);
                }

                using (var dataReader = cmd.ExecuteReader())
                {
                    var indices = Enumerable.Range(0, dataReader.FieldCount).ToList();

                    foreach (IDataRecord record in dataReader as IEnumerable)
                        if (fieldType == "object")
                        {
                            yield return new DropdownOptions
                            {
                                Id = (int)record[0],
                                Option = record[1].ToString()
                            };
                        }
                        else if (fieldType == "list")
                        {
                            yield return Enumerable.Range(0, dataReader.FieldCount).ToDictionary(dataReader.GetName, dataReader.GetValue);
                        }
                        else
                        {
                            yield return record[0];
                        }
                }
            }
        }

        public static int ExecuteScalarFromSql(this DbContext db, string Sql,
           Dictionary<string, object> Params)
        {
            using (var cmd = db.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = Sql;
                if (cmd.Connection.State != ConnectionState.Open) { cmd.Connection.Open(); }

                foreach (KeyValuePair<string, object> p in Params)
                {
                    DbParameter dbParameter = cmd.CreateParameter();
                    dbParameter.ParameterName = p.Key;
                    dbParameter.Value = p.Value == null ? DBNull.Value : p.Value;
                    cmd.Parameters.Add(dbParameter);
                }
                return (int)cmd.ExecuteScalar();
            }
        }

        public static void AddProperty(ExpandoObject expando, string propertyName, object propertyValue)
        {
            // ExpandoObject supports IDictionary so we can extend it like this
            var expandoDict = expando as IDictionary<string, object>;
            if (expandoDict.ContainsKey(propertyName))
                expandoDict[propertyName] = propertyValue;
            else
                expandoDict.Add(propertyName, propertyValue);
        }

        public static void ApplyFilter(this DataSourceRequest request)
        {
            if (request.Sorts != null && request.Sorts.Any())
            {
                foreach (SortDescriptor sortDescriptor in request.Sorts)
                {
                    switch (sortDescriptor.Member)
                    {
                        case "Author":
                            sortDescriptor.Member = "Author.FullName";
                            break;
                        case "Translator":
                            sortDescriptor.Member = "Author.FullName";
                            break;
                        case "Editor":
                            sortDescriptor.Member = "Author.FullName";
                            break;
                        case "City":
                            sortDescriptor.Member = "City.Name";
                            break;
                        case "Publisher":
                            sortDescriptor.Member = "Publisher.Name";
                            break;
                        case "Language":
                            sortDescriptor.Member = "Language.Name";
                            break;
                        case "Region":
                            sortDescriptor.Member = "Region.Name";
                            break;

                    }
                }
            }
        }
    }
}
