﻿using System.Collections.Generic;
using System.Data.SqlClient;
using EntityFramework.BulkInsert.Helpers;

using System.Data.Entity.Spatial;
using System.Threading.Tasks;

namespace EntityFramework.BulkInsert.Providers
{
    public class SqlBulkInsertProvider : ProviderBase<SqlConnection, SqlTransaction>
    {
        public SqlBulkInsertProvider()
        {
            SetProviderIdentifier("System.Data.SqlClient.SqlConnection");
        }

        /// <summary>
        /// Runs sql bulk insert using custom IDataReader
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <param name="transaction"></param>
        public override void Run<T>(IEnumerable<T> entities, SqlTransaction transaction)
        {
            var sqlBulkCopyOptions = ToSqlBulkCopyOptions(Options.BulkCopyOptions);
            var keepIdentity = (SqlBulkCopyOptions.KeepIdentity & sqlBulkCopyOptions) > 0;
            using (var reader = new MappedDataReader<T>(entities, this))
            {
                using (var sqlBulkCopy = new SqlBulkCopy(transaction.Connection, sqlBulkCopyOptions, transaction))
                {
                    sqlBulkCopy.BulkCopyTimeout = Options.TimeOut;
                    sqlBulkCopy.BatchSize = Options.BatchSize;
                    sqlBulkCopy.DestinationTableName = string.Format("[{0}].[{1}]", reader.SchemaName, reader.TableName);
#if !NET40
                    sqlBulkCopy.EnableStreaming = Options.EnableStreaming;
#endif

                    sqlBulkCopy.NotifyAfter = Options.NotifyAfter;
                    if (Options.Callback != null)
                    {
                        sqlBulkCopy.SqlRowsCopied += (sender, args) =>
                        {
                            Options.Callback.Invoke(sender, new RowsCopiedEventArgs(args.RowsCopied));
                        };
                    }

                    foreach (var kvp in reader.Cols)
                    {
                        if (kvp.Value.IsIdentity && !keepIdentity)
                        {
                            continue;
                        }
                        sqlBulkCopy.ColumnMappings.Add(kvp.Value.ColumnName, kvp.Value.ColumnName);
                    }

                    sqlBulkCopy.WriteToServer(reader);
                }
            }
        }

#if NET45

        /// <summary>
        /// Get sql grography object from well known text
        /// </summary>
        /// <param name="wkt">Well known text representation of the value</param>
        /// <param name="srid">The identifier associated with the coordinate system.</param>
        /// <returns></returns>
        public override object GetSqlGeography(string wkt, int srid)
        {
#if EF6
            var geo = new DbGeographyWellKnownValue
            {
                WellKnownText = wkt,
                CoordinateSystemId = srid
            };

            return DbSpatialServices.Default.CreateProviderValue(geo);
#endif
#if EF5
            return DbGeography.FromText(wkt, srid);
#endif
        }

        /// <summary>
        /// Get sql geometry object from well known text
        /// </summary>
        /// <param name="wkt">Well known text representation of the value</param>
        /// <param name="srid">The identifier associated with the coordinate system.</param>
        /// <returns></returns>
        public override object GetSqlGeometry(string wkt, int srid)
        {
#if EF6          
            var geo = new DbGeometryWellKnownValue
            {
                WellKnownText = wkt,
                CoordinateSystemId = srid
            };

            return DbSpatialServices.Default.CreateProviderValue(geo);
#endif
#if EF5
            return DbGeometry.FromText(wkt, srid);
#endif
        }

        /// <summary>
        /// Runs sql bulk insert using custom IDataReader
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <param name="transaction"></param>
        public override async Task RunAsync<T>(IEnumerable<T> entities, SqlTransaction transaction)
        {
            var sqlBulkCopyOptions = ToSqlBulkCopyOptions(Options.BulkCopyOptions);
            var keepIdentity = (SqlBulkCopyOptions.KeepIdentity & sqlBulkCopyOptions) > 0;
            using (var reader = new MappedDataReader<T>(entities, this))
            {
                using (var sqlBulkCopy = new SqlBulkCopy(transaction.Connection, sqlBulkCopyOptions, transaction))
                {
                    sqlBulkCopy.BulkCopyTimeout = Options.TimeOut;
                    sqlBulkCopy.BatchSize = Options.BatchSize;
                    sqlBulkCopy.DestinationTableName = string.Format("[{0}].[{1}]", reader.SchemaName, reader.TableName);
                    sqlBulkCopy.EnableStreaming = Options.EnableStreaming;


                    sqlBulkCopy.NotifyAfter = Options.NotifyAfter;
                    if (Options.Callback != null)
                    {
                        sqlBulkCopy.SqlRowsCopied += (sender, args) =>
                        {
                            Options.Callback.Invoke(sender, new RowsCopiedEventArgs(args.RowsCopied));
                        };
                    }

                    foreach (var kvp in reader.Cols)
                    {
                        if (kvp.Value.IsIdentity && !keepIdentity)
                        {
                            continue;
                        }
                        sqlBulkCopy.ColumnMappings.Add(kvp.Value.ColumnName, kvp.Value.ColumnName);
                    }

                    await sqlBulkCopy.WriteToServerAsync(reader);
                }
            }
        }

#endif

        /// <summary>
        /// Create new sql connection
        /// </summary>
        /// <returns></returns>
        protected override SqlConnection CreateConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        private SqlBulkCopyOptions ToSqlBulkCopyOptions(BulkCopyOptions bulkCopyOptions)
        {
            return (SqlBulkCopyOptions)(int)bulkCopyOptions;
        }
    }
}