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
    public class TBFDMAN
    {
        public TBFDMAN()
        { }
        #region  BasicMethod



        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(DXFDXT.Model.TBFDMAN model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into TBFDMAN(");
            strSql.Append("FDMANID,FDID,SZNAME,SZSEX,SZDATE,RELATION,DEMO,ISFIRE,JSNAME,JSPHONE)");
            strSql.Append(" values (");
            strSql.Append("@FDMANID,@FDID,@SZNAME,@SZSEX,@SZDATE,@RELATION,@DEMO,@ISFIRE,@JSNAME,@JSPHONE)");
            NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@FDMANID", NpgsqlDbType.Varchar,36),
					new NpgsqlParameter("@FDID", NpgsqlDbType.Varchar,36),
					new NpgsqlParameter("@SZNAME", NpgsqlDbType.Varchar,50),
					new NpgsqlParameter("@SZSEX", NpgsqlDbType.Integer),
					new NpgsqlParameter("@SZDATE", NpgsqlDbType.Date),
					new NpgsqlParameter("@RELATION", NpgsqlDbType.Varchar,50) ,
                    new NpgsqlParameter("@DEMO", NpgsqlDbType.Varchar,200),
                    new NpgsqlParameter("@ISFIRE",NpgsqlDbType.Integer),
                    new NpgsqlParameter("@JSNAME",NpgsqlDbType.Varchar,100),
                    new NpgsqlParameter("@JSPHONE",NpgsqlDbType.Varchar,100)
                                           };
            parameters[0].Value = model.FDMANID;
            parameters[1].Value = model.FDID;
            parameters[2].Value = model.SZNAME;
            parameters[3].Value = model.SZSEX;
            parameters[4].Value = model.SZDATE;
            parameters[5].Value = model.RELATION;
            parameters[6].Value = model.DEMO;
            parameters[7].Value = model.ISFIRE;
            parameters[8].Value = model.JSNAME;
            parameters[9].Value = model.JSPHONE;
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
        /// 更新一条数据
        /// </summary>
        public bool Update(DXFDXT.Model.TBFDMAN model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update TBFDMAN set ");
            strSql.Append("FDID=@FDID,SZNAME=@SZNAME,SZSEX=@SZSEX,SZDATE=@SZDATE,RELATION=@RELATION,DEMO=@DEMO,ISFIRE=@ISFIRE,JSNAME=@JSNAME,JSPHONE=@JSPHONE ");
            strSql.Append(" where FDMANID=@FDMANID ");
            NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@FDMANID", NpgsqlDbType.Varchar,36),
					new NpgsqlParameter("@FDID", NpgsqlDbType.Varchar,36),
					new NpgsqlParameter("@SZNAME", NpgsqlDbType.Varchar,50),
					new NpgsqlParameter("@SZSEX", NpgsqlDbType.Integer),
					new NpgsqlParameter("@SZDATE", NpgsqlDbType.Date),
					new NpgsqlParameter("@RELATION", NpgsqlDbType.Varchar,50) ,
                    new NpgsqlParameter("@DEMO", NpgsqlDbType.Varchar,200),
                    new NpgsqlParameter("@ISFIRE",NpgsqlDbType.Integer),
                    new NpgsqlParameter("@JSNAME",NpgsqlDbType.Varchar,100),
                    new NpgsqlParameter("@JSPHONE",NpgsqlDbType.Varchar,100)
                                           };
            parameters[0].Value = model.FDMANID;
            parameters[1].Value = model.FDID;
            parameters[2].Value = model.SZNAME;
            parameters[3].Value = model.SZSEX;
            parameters[4].Value = model.SZDATE;
            parameters[5].Value = model.RELATION;
            parameters[6].Value = model.DEMO;
            parameters[7].Value = model.ISFIRE;
            parameters[8].Value = model.JSNAME;
            parameters[9].Value = model.JSPHONE;
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
            strSql.Append("delete from TBFDMAN ");
            if (!string.IsNullOrEmpty(strWhere))
            {
                strSql.Append(" where " + strWhere);
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
        public DXFDXT.Model.TBFDMAN GetModel(string strWhere)
        {
            //该表无主键信息，请自定义主键/条件字段
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  FDMANID,FDID,SZNAME,SZSEX,SZDATE,RELATION,DEMO,ISFIRE,JSNAME,JSPHONE from TBFDMAN ");
            if (!string.IsNullOrEmpty(strWhere))
            {
                strSql.Append(" where " + strWhere);
            }


            DXFDXT.Model.TBFDMAN model = new DXFDXT.Model.TBFDMAN();
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
        public DXFDXT.Model.TBFDMAN DataRowToModel(DataRow row)
        {
            DXFDXT.Model.TBFDMAN model = new DXFDXT.Model.TBFDMAN();
            if (row != null)
            {
                if (row["FDMANID"] != null)
                {
                    model.FDMANID = row["FDMANID"].ToString();
                }
                if (row["FDID"] != null)
                {
                    model.FDID = row["FDID"].ToString();
                }
                if (row["SZNAME"] != null)
                {
                    model.SZNAME = row["SZNAME"].ToString();
                }

                if (row["SZSEX"] != null && row["SZSEX"].ToString() != "")
                {
                    model.SZSEX = int.Parse(row["SZSEX"].ToString());
                }
                if (row["SZDATE"] != null && row["SZDATE"].ToString() != "")
                {
                    model.SZDATE = DateTime.Parse(row["SZDATE"].ToString());
                }
                if (row["RELATION"] != null)
                {
                    model.RELATION = row["RELATION"].ToString();
                }
                if (row["DEMO"] != null)
                {
                    model.DEMO = row["DEMO"].ToString();
                }
                if (row["ISFIRE"] != null && row["ISFIRE"].ToString() != "")
                {
                    model.ISFIRE = int.Parse(row["ISFIRE"].ToString());
                }
                if (row["JSNAME"] != null)
                {
                    model.JSNAME = row["JSNAME"].ToString();
                }
                if (row["JSPHONE"] != null)
                {
                    model.JSPHONE = row["JSPHONE"].ToString();
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
            strSql.Append("select FDMANID,FDID,SZNAME,SZSEX,SZDATE,RELATION,DEMO,ISFIRE,JSNAME,JSPHONE ");
            strSql.Append(" FROM TBFDMAN ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
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
            strSql.Append(" FDMANID,FDID,SZNAME,SZSEX,SZDATE,RELATION,DEMO,ISFIRE,JSNAME,JSPHONE ");
            strSql.Append(" FROM TBFDMAN ");
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
            strSql.Append("select count(1) FROM TBFDMAN ");
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
                strSql.Append("order by T. desc");
            }
            strSql.Append(")AS Row, T.*  from TBFDMAN T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DbHelperPostSQL.Query(strSql.ToString());
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
            parameters[0].Value = "TBFDMAN";
            parameters[1].Value = "";
            parameters[2].Value = PageSize;
            parameters[3].Value = PageIndex;
            parameters[4].Value = 0;
            parameters[5].Value = 0;
            parameters[6].Value = strWhere;	
            return DbHelperPostSQL.RunProcedure("UP_GetRecordByPage",parameters,"ds");
        }*/

        #endregion  BasicMethod

        #region 附加业务，导出报表
        public  DataSet exportExcel()
        {
            string strSql = "select tbfdinfo.fdno as no,tbfdinfo.villname as vill,tbfdman.szname as name,tbfdman.szsex as sex,tbfdman.szdate as date,tbfdman.isfire as isfire ,tbfdman.jsname as js,tbfdman.jsphone as phone,replace(st_astext(tbfdinfo.geom),'POINT(','')as Point from tbfdinfo  join tbfdman on tbfdinfo.fdid=tbfdman.fdid order by tbfdinfo.fdno ";
            return DbHelperPostSQL.Query(strSql.ToString());
        }
        #endregion
    }
}
