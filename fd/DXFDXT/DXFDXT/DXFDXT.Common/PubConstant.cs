using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
namespace DXFDXT.Common
{

    /// <summary>
    /// 获取App.config的信息
    /// </summary>
    public class PubConstant
    {
        /// <summary>
        /// 获取连接字符串Sqlite
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                return connectionString;
            }
        }
        /// <summary>
        /// 生成文档路径
        /// </summary>
        public static string documentPath
        {
            get
            {
                string connectionString = ConfigurationManager.AppSettings["documentPath"].ToString();
                return connectionString;
            }
        }
        /// <summary>
        /// 模板路径
        /// </summary>
        public static string templatePath
        {
            get
            {
                string connectionString = ConfigurationManager.AppSettings["templatePath"].ToString();
                return connectionString;
            }
        }
        /// <summary>
        /// 得到web.config里配置项的数据库连接字符串。GPS
        /// </summary>
        /// <param name="configName">配置名称</param>
        /// <returns>配置值</returns>
        public static string GetConnectionString(string configName)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[configName].ConnectionString;
            return connectionString;
        }
        //public static void GetTest()
        //{
        //    try
        //    {
        //        using (NpgsqlConnection conn = new NpgsqlConnection(PubConstant.ConnectionString))
        //        {
        //            conn.Open();

        //        }
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    } 
        //}
    }
}
