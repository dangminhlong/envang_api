using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;


namespace WebEnvang.Models
{
    public class MsSqlHelper
    {
        public static Task ExecuteNonQueryTask(string conString, string storeName, string[] dsParam = null, object[] dsGiatri = null)
        {
            return Task.Run(() =>
            {
                ExecuteNoneQuery(conString, storeName, dsParam, dsGiatri);
            });
        }
        public static Task<object> ExecuteScalarTask(string conString, string storeName, string[] dsParam = null, object[] dsGiatri = null)
        {
            return Task<object>.Run(() =>
            {
                return ExecuteScalar(conString, storeName, dsParam, dsGiatri);
            });
        }
        public static Task<DataSet> ExecuteDataSetTask(string conString, string storeName, string[] dsParam = null, object[] dsGiatri = null)
        {
            return Task<DataSet>.Run(() =>
            {
                return ExecuteDataSet(conString, storeName, dsParam, dsGiatri);
            });
        }
        public static Task<DataTable> ExecuteDataTableTask(string conString, string storeName, string[] dsParam = null, object[] dsGiatri = null)
        {
            return Task<DataSet>.Run(() =>
            {
                return ExecuteDataTable(conString, storeName, dsParam, dsGiatri);
            });
        }
        public static DataSet ExecuteDataSet(string conString, string storeName, string[] dsParam, object[] dsGiatri)
        {
            using (SqlConnection conn = new SqlConnection(conString))
            {
                using (SqlCommand comm = new SqlCommand(storeName, conn))
                {
                    comm.CommandType = CommandType.StoredProcedure;
                    if (dsParam != null)
                        for (int i = 0; i < dsParam.Length; i++)
                        {
                            var param = comm.Parameters.AddWithValue(dsParam[i], dsGiatri[i]);
                            if (dsGiatri[i] is DataTable)
                                param.SqlDbType = SqlDbType.Structured;
                        }
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = comm;
                    conn.Open();
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    conn.Close();
                    return ds;
                }
            }
        }

        public static object ExecuteScalar(string conString, string storeName, string[] dsParam, object[] dsGiatri)
        {
            using (SqlConnection conn = new SqlConnection(conString))
            {
                using (SqlCommand comm = new SqlCommand(storeName, conn))
                {
                    comm.CommandType = CommandType.StoredProcedure;
                    if (dsParam != null)
                        for (int i = 0; i < dsParam.Length; i++)
                        {
                            var param = comm.Parameters.AddWithValue(dsParam[i], dsGiatri[i]);
                            if (dsGiatri[i] is DataTable)
                                param.SqlDbType = SqlDbType.Structured;
                        }
                    
                    conn.Open();
                    object result = comm.ExecuteScalar();
                    conn.Close();
                    return result;
                }
            }
        }

        public static DataTable ExecuteDataTable(string conString, string storeName, string[] dsParam, object[] dsGiatri)
        {
            using (SqlConnection conn = new SqlConnection(conString))
            {
                using (SqlCommand comm = new SqlCommand(storeName, conn))
                {
                    comm.CommandType = CommandType.StoredProcedure;
                    if (dsParam != null)
                        for (int i = 0; i < dsParam.Length; i++)
                        {
                            var param = comm.Parameters.AddWithValue(dsParam[i], dsGiatri[i]);
                            if (dsGiatri[i] is DataTable)
                                param.SqlDbType = SqlDbType.Structured;
                        }
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = comm;
                    conn.Open();
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    conn.Close();
                    return dt;
                }
            }
        }

        public static void ExecuteNoneQuery(string conString, string storeName, string[] dsParam, object[] dsGiatri)
        {
            using (SqlConnection conn = new SqlConnection(conString))
            {
                using (SqlCommand comm = new SqlCommand(storeName, conn))
                {
                    comm.CommandType = CommandType.StoredProcedure;
                    if (dsParam != null)
                        for (int i = 0; i < dsParam.Length; i++)
                        {
                            var param = comm.Parameters.AddWithValue(dsParam[i], dsGiatri[i]);
                            if (dsGiatri[i] is DataTable)
                                param.SqlDbType = SqlDbType.Structured;
                        }
                    
                    conn.Open();
                    comm.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }
    }
}