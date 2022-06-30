using Kztek.Data.LockerEvent.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Data.LockerEvent.SqlHelper
{
    public static class ExcuteSQLEvent
    {
        //private static string connStr = "Data Source=FUTECH-PC;Initial Catalog=FU_EAZYREG;User Id=sa;Password=123456";
        private static string connStr = ConfigurationManager.ConnectionStrings["KztekLockerEventEntities"].ConnectionString;
        /// <summary>
        /// Execute query dạng text
        /// </summary>
        /// <param name="strSQL">Câu lệnh dạng text</param>
        /// <returns></returns>
        public static bool Execute(string strSQL)
        {
            bool isSuccess = true;
            //FUTECHEntitiesReg db = new FUTECHEntitiesReg();
            //var objConnect = db.SystemConfigies.FirstOrDefault();
            //if (objConnect!=null)
            //{
            //    connStr = objConnect.ConnectStringGenerate;
            //}
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    SqlCommand comm = new SqlCommand();
                    comm.Connection = conn;
                    comm.CommandText = strSQL;
                    int i = comm.ExecuteNonQuery();
                    isSuccess = i > 0 ? true : false;
                }
                catch (Exception ex)
                {
                    //throw ex;
                    isSuccess = false;
                }
                finally
                {
                    conn.Close();
                }
            }
            return isSuccess;
        }

        /// <summary>
        /// Excute query dạng txt, connect truyền vào
        /// </summary>
        /// <param name="strSQL"></param>
        /// <param name="connect"></param>
        /// <returns></returns>
        public static bool ExecuteSub(string strSQL, string connect)
        {
            bool isSuccess = true;
            using (SqlConnection conn = new SqlConnection(connect))
            {
                try
                {
                    conn.Open();
                    SqlCommand comm = new SqlCommand();
                    comm.Connection = conn;
                    comm.CommandText = strSQL;
                    isSuccess = comm.ExecuteNonQuery() > 0 ? true : false;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conn.Close();
                }
            }
            return isSuccess;
        }

        /// <summary>
        /// Excute query dạng text, dùng cho các câu cần phải ở master
        /// </summary>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        public static bool ExecuteMaster(string strSQL)
        {
            bool isSuccess = true;
            //FUTECHEntitiesReg db = new FUTECHEntitiesReg();
            //var objConnect = db.SystemConfigies.FirstOrDefault();
            //if (objConnect != null)
            //{
            //    connStr = objConnect.ConnectStringGenerate;
            //}
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    SqlCommand comm = new SqlCommand();
                    comm.Connection = conn;
                    conn.ChangeDatabase("master");
                    comm.CommandText = strSQL;
                    isSuccess = comm.ExecuteNonQuery() > 0 ? true : false;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conn.Close();
                }
            }
            return isSuccess;
        }

        /// <summary>
        /// Execute query dạng text hoặc store producedure với tham số pars, connect truyền vào
        /// </summary>
        /// <param name="strSQL"></param>
        /// <param name="pars"></param>
        /// <param name="isStoredProcedure"></param>
        /// <returns></returns>
        public static bool Execute(string strSQL, SqlParameter[] pars, string connect, bool isStoredProcedure = false)
        {
            bool isSuccess = true;
            using (SqlConnection conn = new SqlConnection(connect))
            {
                try
                {
                    conn.Open();
                    SqlCommand comm = new SqlCommand();
                    comm.Connection = conn;
                    if (isStoredProcedure)
                    {
                        comm.CommandType = CommandType.StoredProcedure;
                    }
                    else
                    {
                        comm.CommandType = CommandType.Text;
                    }
                    comm.CommandText = strSQL;
                    if (pars != null && pars.Length > 0)
                    {
                        comm.Parameters.AddRange(pars);
                    }
                    isSuccess = comm.ExecuteNonQuery() > 0 ? true : false;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conn.Close();
                }
            }
            return isSuccess;
        }

        /// <summary>
        /// Lấy danh sách thông tin theo câu lệnh truyền vào
        /// </summary>
        /// <param name="strSQL"></param>
        /// <param name="connect"></param>
        /// <param name="isStoredProcedure"></param>
        /// <returns></returns>
        public static DataTable GetTable(string strSQL, string connect, bool isStoredProcedure = false)
        {
            //Khai báo 1 biến bảng để chứa dữ liệu
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connect))
            {
                try
                {
                    //Mở kết nối
                    conn.Open();
                    //Khai báo 1 đối tượng để thực hiện công việc
                    //SqlCommand comm = new SqlCommand(strSQL, conn);
                    SqlCommand comm = new SqlCommand();
                    //Thực hiện công việc trên database nào
                    comm.Connection = conn;
                    //Nếu sử dụng thủ tục
                    if (isStoredProcedure)
                    {
                        comm.CommandType = CommandType.StoredProcedure;
                    }
                    else
                    {
                        comm.CommandType = CommandType.Text;
                    }
                    //Công việc cần thực hiện
                    comm.CommandText = strSQL;
                    //Khai báo 1 đối tượng để chứa dữ liệu tạm thời lấy được từ db lên
                    SqlDataAdapter adapter = new SqlDataAdapter(comm);
                    //Đổ dữ liệu vào bảng
                    adapter.Fill(dt);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    //Đóng kết nối
                    conn.Close();
                }
            }
            return dt;
        }

        /// <summary>
        /// Lấy danh sách theo store procedure với tham số
        /// </summary>
        /// <param name="strSQL"></param>
        /// <param name="pars"></param>
        /// <param name="connect"></param>
        /// <param name="isStoredProcedure"></param>
        /// <returns></returns>
        public static DataTable GetTable(string strSQL, SqlParameter[] pars, string connect, bool isStoredProcedure = false)
        {
            //Khai báo 1 biến bảng để chứa dữ liệu
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connect))
            {
                try
                {
                    //Mở kết nối
                    conn.Open();
                    //Khai báo 1 đối tượng để thực hiện công việc
                    //SqlCommand comm = new SqlCommand(strSQL, conn);
                    SqlCommand comm = new SqlCommand();
                    //Thực hiện công việc trên database nào
                    comm.Connection = conn;
                    //Nếu sử dụng thủ tục
                    if (isStoredProcedure)
                    {
                        comm.CommandType = CommandType.StoredProcedure;
                    }
                    else
                    {
                        comm.CommandType = CommandType.Text;
                    }
                    //Công việc cần thực hiện
                    comm.CommandText = strSQL;
                    //Nếu câu lệnh truy vấn có tham số thì thêm vào comm
                    if (pars != null && pars.Length > 0)
                    {
                        comm.Parameters.AddRange(pars);
                    }
                    //Khai báo 1 đối tượng để chứa dữ liệu tạm thời lấy được từ db lên
                    SqlDataAdapter adapter = new SqlDataAdapter(comm);
                    //Đổ dữ liệu vào bảng
                    adapter.Fill(dt);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    //Đóng kết nối
                    conn.Close();
                }
            }
            return dt;
        }

        public static List<T> ConvertTo<T>(DataTable datatable) where T : new()
        {
            List<T> Temp = new List<T>();
            try
            {
                List<string> columnsNames = new List<string>();
                foreach (DataColumn DataColumn in datatable.Columns)
                    columnsNames.Add(DataColumn.ColumnName);
                Temp = datatable.AsEnumerable().ToList().ConvertAll<T>(row => getObject<T>(row, columnsNames));
                return Temp;
            }
            catch
            {
                return Temp;
            }

        }

        public static T getObject<T>(DataRow row, List<string> columnsName) where T : new()
        {
            T obj = new T();
            try
            {
                string columnname = "";
                string value = "";
                PropertyInfo[] Properties;
                Properties = typeof(T).GetProperties();
                foreach (PropertyInfo objProperty in Properties)
                {
                    columnname = columnsName.Find(name => name.ToLower() == objProperty.Name.ToLower());
                    if (!string.IsNullOrEmpty(columnname))
                    {
                        value = row[columnname].ToString();
                        if (!string.IsNullOrEmpty(value))
                        {
                            if (Nullable.GetUnderlyingType(objProperty.PropertyType) != null)
                            {
                                value = row[columnname].ToString().Replace("$", "").Replace(",", "");
                                objProperty.SetValue(obj, Convert.ChangeType(value, Type.GetType(Nullable.GetUnderlyingType(objProperty.PropertyType).ToString())), null);
                            }
                            else
                            {
                                value = row[columnname].ToString().Replace("%", "");
                                objProperty.SetValue(obj, Convert.ChangeType(value, Type.GetType(objProperty.PropertyType.ToString())), null);
                            }
                        }
                    }
                }
                return obj;
            }
            catch
            {
                return obj;
            }
        }

        public static DataSet GetDataSet(string strSQL, bool isStoredProcedure = false)
        {
            //Khai báo 1 biến bảng để chứa dữ liệu
            var ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    //Mở kết nối
                    conn.Open();
                    //Khai báo 1 đối tượng để thực hiện công việc
                    //SqlCommand comm = new SqlCommand(strSQL, conn);
                    SqlCommand comm = new SqlCommand();
                    //Thực hiện công việc trên database nào
                    comm.Connection = conn;
                    //Nếu sử dụng thủ tục
                    if (isStoredProcedure)
                    {
                        comm.CommandType = CommandType.StoredProcedure;
                    }
                    else
                    {
                        comm.CommandType = CommandType.Text;
                    }
                    //Công việc cần thực hiện
                    comm.CommandText = strSQL;
                    //Khai báo 1 đối tượng để chứa dữ liệu tạm thời lấy được từ db lên
                    SqlDataAdapter adapter = new SqlDataAdapter(comm);
                    //Đổ dữ liệu vào bảng

                    adapter.Fill(ds);

                    adapter.Dispose();
                    comm.Dispose();
                    conn.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    //Đóng kết nối
                    conn.Close();
                }
            }
            return ds;
        }

        /// <summary>
        /// Converts a DataTable to a list with generic objects
        /// </summary>
        /// <typeparam name="T">Generic object</typeparam>
        /// <param name="table">DataTable</param>
        /// <returns>List with generic objects</returns>
        public static List<T> DataTableToList<T>(this DataTable table) where T : class, new()
        {
            try
            {
                List<T> list = new List<T>();

                foreach (var row in table.AsEnumerable())
                {
                    T obj = new T();

                    foreach (var prop in obj.GetType().GetProperties())
                    {
                        try
                        {
                            PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
                            propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType), null);
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    list.Add(obj);
                }

                return list;
            }
            catch
            {
                return null;
            }
        }

        //DataTable dtTable = GetEmployeeDataTable();
        //List<Employee> employeeList = dtTable.DataTableToList<Employee>();

        public static int ExcuteNone(string strSQL)
        {
            using (var _DataContext = new DatabaseFactory().Get())
            {
                var result = _DataContext.Database.ExecuteSqlCommand(strSQL);
                return result;
            }
        }

        public static int ExecuteReturnRow(string strSQL, string connect)
        {
            using (SqlConnection conn = new SqlConnection(connect))
            {
                int row = 0;
                try
                {
                    conn.Open();
                    SqlCommand comm = new SqlCommand();
                    comm.Connection = conn;
                    comm.CommandText = strSQL;
                    row = Convert.ToInt32(comm.ExecuteScalar());
                    comm.Dispose();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conn.Close();
                }
                return row;
            }
        }

        public static string ExecuteReturnValue(string strSQL, string connect, string columnname)
        {
            string value = "";
            using (SqlConnection conn = new SqlConnection(connect))
            {
                try
                {
                    conn.Open();
                    SqlCommand comm = new SqlCommand();
                    comm.Connection = conn;
                    comm.CommandText = strSQL;
                    int row = Convert.ToInt32(comm.ExecuteScalar());
                    if (row > 0)
                    {
                        SqlDataReader rdr = comm.ExecuteReader();
                        while (rdr.Read())
                        {
                            value = (string)rdr[columnname];
                        }
                    }
                    else
                    {
                        value = "No row matched";
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conn.Close();
                }
            }
            return value;
        }
    }
}
