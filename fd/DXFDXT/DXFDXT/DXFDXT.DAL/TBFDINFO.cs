using DXFDXT.Common;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXFDXT.DAL
{
    public class TBFDINFO
    {
        #region  BasicMethod
        /// <summary>
        /// 更新图形信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="geom"></param>
        /// <returns></returns>
        public bool UpdateGeom(string id,string geom) {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(geom)) return false;
            string sql = "update tbfdinfo set geom='st_geomfromtext("+geom+",4326)' where fdid='"+id+"'";
            int num= DbHelperPostSQL.ExecuteSql(sql);
            if (num > 0) {
                return true;
            }
            return false;

        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(DXFDXT.Model.TBFDINFO model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into TBFDINFO(");
            strSql.Append("FDID,VILLNAME,FDNO,QFYX,DEMO)");
            strSql.Append(" values (");
            strSql.Append("@FDID,@VILLNAME,@FDNO,@QFYX,@DEMO)");
            NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@FDID", NpgsqlDbType.Varchar,36),
					new NpgsqlParameter("@VILLNAME", NpgsqlDbType.Varchar,100),
					new NpgsqlParameter("@FDNO", NpgsqlDbType.Varchar,100),
                    new NpgsqlParameter("@QFYX", NpgsqlDbType.Integer),
                    new NpgsqlParameter("@DEMO", NpgsqlDbType.Varchar,200)
                                           };
            parameters[0].Value = model.FDID;
            parameters[1].Value = model.VILLNAME;
            parameters[2].Value = model.FDNO;
            parameters[3].Value = model.QFYX;
            parameters[4].Value = model.DEMO;
            int rows = DbHelperPostSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return UpdateGeom(model.FDID, model.GEOM);
                //return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(DXFDXT.Model.TBFDINFO model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update TBFDINFO set ");
            strSql.Append("VILLNAME=@VILLNAME,");
            strSql.Append("FDNO=@FDNO,"); 
            strSql.Append("QFYX=@QFYX,DEMO=@DEMO");
            strSql.Append(" where  FDID=@FDID ");
            NpgsqlParameter[] parameters = {
				    new NpgsqlParameter("@FDID", NpgsqlDbType.Varchar,36),
					new NpgsqlParameter("@VILLNAME", NpgsqlDbType.Varchar,100),
					new NpgsqlParameter("@FDNO", NpgsqlDbType.Varchar,100),
                    new NpgsqlParameter("@QFYX", NpgsqlDbType.Integer),
                    new NpgsqlParameter("@DEMO", NpgsqlDbType.Varchar,200)
                                           };
            parameters[0].Value = model.FDID;
            parameters[1].Value = model.VILLNAME;
            parameters[2].Value = model.FDNO;
            parameters[3].Value = model.QFYX;
            parameters[4].Value = model.DEMO;
            int rows = DbHelperPostSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return UpdateGeom(model.FDID, model.GEOM);
                //return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool UpdateFd(DXFDXT.Model.TBFDINFO model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update TBFDINFO set ");
            strSql.Append("VILLNAME=@VILLNAME,");
            strSql.Append("FDNO=@FDNO,");
            strSql.Append("QFYX=@QFYX,DEMO=@DEMO");
            strSql.Append(" where  FDID=@FDID ");
            NpgsqlParameter[] parameters = {
                    new NpgsqlParameter("@FDID", NpgsqlDbType.Varchar,36),
                    new NpgsqlParameter("@VILLNAME", NpgsqlDbType.Varchar,100),
                    new NpgsqlParameter("@FDNO", NpgsqlDbType.Varchar,100),
                    new NpgsqlParameter("@QFYX", NpgsqlDbType.Integer),
                    new NpgsqlParameter("@DEMO", NpgsqlDbType.Varchar,200)
                                           };
            parameters[0].Value = model.FDID;
            parameters[1].Value = model.VILLNAME;
            parameters[2].Value = model.FDNO;
            parameters[3].Value = model.QFYX;
            parameters[4].Value = model.DEMO;
            int rows = DbHelperPostSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(string strWhere)
        {
            //该表无主键信息，请自定义主键/条件字段
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from TBFDINFO ");

            if (!string.IsNullOrEmpty(strWhere))
            {
                 strSql.Append(" where "+strWhere);
            }
            int rows = DbHelperPostSQL.ExecuteSql(strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public DXFDXT.Model.TBFDINFO GetModel(string strWhere)
        {
            //该表无主键信息，请自定义主键/条件字段
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select   FDID,VILLNAME,FDNO,QFYX,DEMO,st_astext(geom) as GEOM  from TBFDINFO ");
            if (!string.IsNullOrEmpty(strWhere))
            {
                strSql.Append(" where " + strWhere);
            }


            DXFDXT.Model.TBFDINFO model = new DXFDXT.Model.TBFDINFO();
            DataSet ds = DbHelperPostSQL.Query(strSql.ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public DXFDXT.Model.TBFDINFO DataRowToModel(DataRow row)
        {
            DXFDXT.Model.TBFDINFO model = new DXFDXT.Model.TBFDINFO();
            if (row != null)
            {
                if (row["FDID"] != null)
                {
                    model.FDID = row["FDID"].ToString();
                }
                if (row["VILLNAME"] != null)
                {
                    model.VILLNAME = row["VILLNAME"].ToString();
                }
                if (row["FDNO"] != null)
                {
                    model.FDNO = row["FDNO"].ToString();
                }
              
                //if (row["ISFIRE"] != null && row["ISFIRE"].ToString() != "")
                //{
                //    model.ISFIRE = int.Parse(row["ISFIRE"].ToString());
                //}
                //if (row["JSNAME"] != null)
                //{
                //    model.JSNAME = row["JSNAME"].ToString();
                //}
                //if (row["JSPHONE"] != null)
                //{
                //    model.JSPHONE = row["JSPHONE"].ToString();
                //}
                if (row["QFYX"] != null && row["QFYX"].ToString() != "")
                {
                    model.QFYX = int.Parse(row["QFYX"].ToString());
                }
               
                if (row["DEMO"] != null)
                {
                    model.DEMO = row["DEMO"].ToString();
                }
                if (row["GEOM"] != null)
                {
                    model.GEOM = row["GEOM"].ToString();
                }
            }
            return model;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select FDID,VILLNAME,FDNO,QFYX,DEMO,st_astext(geom) as GEOM ");
            strSql.Append(" FROM TBFDINFO ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperPostSQL.Query(strSql.ToString());
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetChartVillage(string strWhere, string groupname)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select " + groupname + " as name,count(*) as count ");
            strSql.Append(" FROM TBFDINFO   T inner join TBFDMAN a on T.FDID=a.FDID ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" group by " + groupname + " ");
            return DbHelperPostSQL.Query(strSql.ToString());
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetChart(string strWhere,string groupname)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select (case when aa is null or aa=0 then 0 else 1 end) as  name,sum(count)as count from(select " + groupname+" as aa,count(*) as count ");
            strSql.Append(" FROM TBFDINFO   T inner join TBFDMAN a on T.FDID=a.FDID ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" group by "+groupname+ " ) t group by (case when aa is null or aa=0 then 0 else 1 end)");
            return DbHelperPostSQL.Query(strSql.ToString());
        }
        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top.ToString());
            }
            strSql.Append(" FDID,VILLNAME,FDNO,QFYX,DEMO,st_astext(geom) as GEOM  ");
            strSql.Append(" FROM TBFDINFO ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DbHelperPostSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM TBFDINFO ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            object obj = DbHelperPostSQL.GetSingle(strSql.ToString());
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            if (!string.IsNullOrEmpty(orderby.Trim()))
            {
                strSql.Append("order by T." + orderby);
            }
            else
            {
                strSql.Append("order by T.fdno desc");
            }
            strSql.Append(")AS Row, FDID,villname,FDNO,QFYX,DEMO,st_astext(geom) as GEOM  from TBFDINFO T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DbHelperPostSQL.Query(strSql.ToString());
        }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetSzListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            if (!string.IsNullOrEmpty(orderby.Trim()))
            {
                strSql.Append("order by T." + orderby);
            }
            else
            {
                strSql.Append("order by T.fdno desc, a.SZNAME ");
            }
            strSql.Append(")AS Row, T.FDID,villname,FDNO,a.ISFIRE,a.JSNAME,a.JSPHONE,QFYX,a.DEMO,st_astext(geom) as GEOM,a.SZNAME,a.SZSEX,a.SZDATE,a.FDMANID  from TBFDINFO T inner join TBFDMAN a on T.FDID=a.FDID ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DbHelperPostSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetSzRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM TBFDINFO T  inner join TBFDMAN a on T.FDID=a.FDID ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            object obj = DbHelperPostSQL.GetSingle(strSql.ToString());
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
        /*
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetList(int PageSize,int PageIndex,string strWhere)
        {
            NpgsqlParameter[] parameters = {
                    new NpgsqlParameter("@tblName", NpgsqlDbType.Varchar, 255),
                    new NpgsqlParameter("@fldName", NpgsqlDbType.Varchar, 255),
                    new NpgsqlParameter("@PageSize", NpgsqlDbType.Integer),
                    new NpgsqlParameter("@PageIndex", NpgsqlDbType.Integer),
                    new NpgsqlParameter("@IsReCount", SqlDbType.Bit),
                    new NpgsqlParameter("@OrderType", SqlDbType.Bit),
                    new NpgsqlParameter("@strWhere", NpgsqlDbType.Varchar,1000),
                    };
            parameters[0].Value = "TBFDINFO";
            parameters[1].Value = "";
            parameters[2].Value = PageSize;
            parameters[3].Value = PageIndex;
            parameters[4].Value = 0;
            parameters[5].Value = 0;
            parameters[6].Value = strWhere;	
            return DbHelperPostSQL.RunProcedure("UP_GetRecordByPage",parameters,"ds");
        }*/

        #endregion  BasicMethod
    }
}
