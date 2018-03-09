/**  版本信息模板在安装目录下，可自行修改。
* TBUSERS.cs
*
* 功 能： N/A
* 类 名： TBUSERS
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2017/11/10 10:31:42   N/A    初版
*
* Copyright (c) 2012 Maticsoft Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：动软卓越（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Data;
using System.Text;
using NpgsqlTypes;using Npgsql;
using DXFDXT.Common; //Please add references
namespace DXFDXT.DAL
{
	/// <summary>
	/// 数据访问类:TBUSERS
	/// </summary>
	public partial class TBUSERS
	{
		public TBUSERS()
		{}
		#region  BasicMethod



		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(DXFDXT.Model.TBUSERS model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into TBUSERS(");
			strSql.Append("USERID,USERNAME,PASSWORD,STATE,LASTLOGINTIME,CURRENTLOGINTIME,DESCRIPTION,ZYZH,ROLEID)");
			strSql.Append(" values (");
            strSql.Append("@USERID,@USERNAME,@PASSWORD,@STATE,@LASTLOGINTIME,@CURRENTLOGINTIME,@DESCRIPTION,@ZYZH,@ROLEID)");
			NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@USERID", NpgsqlDbType.Varchar,36),
					new NpgsqlParameter("@USERNAME", NpgsqlDbType.Varchar,100),
					new NpgsqlParameter("@PASSWORD", NpgsqlDbType.Varchar,100),
					new NpgsqlParameter("@STATE", NpgsqlDbType.Varchar,100),
					new NpgsqlParameter("@LASTLOGINTIME", NpgsqlDbType.Date,3),
					new NpgsqlParameter("@CURRENTLOGINTIME", NpgsqlDbType.Date,3),
					new NpgsqlParameter("@DESCRIPTION", NpgsqlDbType.Varchar,200),
                    new NpgsqlParameter("@ZYZH", NpgsqlDbType.Varchar,100),
                    new NpgsqlParameter("@ROLEID", NpgsqlDbType.Integer)
                                           };
			parameters[0].Value = model.USERID;
			parameters[1].Value = model.USERNAME;
			parameters[2].Value = model.PASSWORD;
			parameters[3].Value = model.STATE;
			parameters[4].Value = model.LASTLOGINTIME;
			parameters[5].Value = model.CURRENTLOGINTIME;
			parameters[6].Value = model.DESCRIPTION;
            parameters[7].Value = model.ZYZH;
            parameters[8].Value = model.ROLEID;
			int rows=DbHelperPostSQL.ExecuteSql(strSql.ToString(),parameters);
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
		public bool Update(DXFDXT.Model.TBUSERS model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update TBUSERS set ");
			strSql.Append("USERID=@USERID,");
			strSql.Append("USERNAME=@USERNAME,");
			strSql.Append("PASSWORD=@PASSWORD,");
			strSql.Append("STATE=@STATE,");
			strSql.Append("LASTLOGINTIME=@LASTLOGINTIME,");
			strSql.Append("CURRENTLOGINTIME=@CURRENTLOGINTIME,");
            strSql.Append("DESCRIPTION=@DESCRIPTION,ZYZH=@ZYZH,ROLEID=@ROLEID");
			strSql.Append(" where ");
			NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@USERID", NpgsqlDbType.Varchar,36),
					new NpgsqlParameter("@USERNAME", NpgsqlDbType.Varchar,100),
					new NpgsqlParameter("@PASSWORD", NpgsqlDbType.Varchar,100),
					new NpgsqlParameter("@STATE", NpgsqlDbType.Varchar,100),
					new NpgsqlParameter("@LASTLOGINTIME", NpgsqlDbType.Date,3),
					new NpgsqlParameter("@CURRENTLOGINTIME", NpgsqlDbType.Date,3),
					new NpgsqlParameter("@DESCRIPTION", NpgsqlDbType.Varchar,200),
                    new NpgsqlParameter("@ZYZH", NpgsqlDbType.Varchar,100),
                    new NpgsqlParameter("@ROLEID", NpgsqlDbType.Integer),
                                           };
			parameters[0].Value = model.USERID;
			parameters[1].Value = model.USERNAME;
			parameters[2].Value = model.PASSWORD;
			parameters[3].Value = model.STATE;
			parameters[4].Value = model.LASTLOGINTIME;
			parameters[5].Value = model.CURRENTLOGINTIME;
			parameters[6].Value = model.DESCRIPTION;
            parameters[7].Value = model.ZYZH;
            parameters[8].Value = model.ROLEID;
			int rows=DbHelperPostSQL.ExecuteSql(strSql.ToString(),parameters);
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
		public bool Delete()
		{
			//该表无主键信息，请自定义主键/条件字段
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from TBUSERS ");
			strSql.Append(" where ");
			NpgsqlParameter[] parameters = {
			};

			int rows=DbHelperPostSQL.ExecuteSql(strSql.ToString(),parameters);
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
		public DXFDXT.Model.TBUSERS GetModel(string strWhere)
		{
			//该表无主键信息，请自定义主键/条件字段
			StringBuilder strSql=new StringBuilder();
            strSql.Append("select   USERID,USERNAME,PASSWORD,STATE,LASTLOGINTIME,CURRENTLOGINTIME,DESCRIPTION,ZYZH,ROLEID from TBUSERS ");
            if (!string.IsNullOrEmpty(strWhere))
            {
                strSql.Append(" where " + strWhere);
            }
            
			 
			DXFDXT.Model.TBUSERS model=new DXFDXT.Model.TBUSERS();
			DataSet ds=DbHelperPostSQL.Query(strSql.ToString());
			if(ds.Tables[0].Rows.Count>0)
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
		public DXFDXT.Model.TBUSERS DataRowToModel(DataRow row)
		{
			DXFDXT.Model.TBUSERS model=new DXFDXT.Model.TBUSERS();
			if (row != null)
			{
				if(row["USERID"]!=null)
				{
					model.USERID=row["USERID"].ToString();
				}
				if(row["USERNAME"]!=null)
				{
					model.USERNAME=row["USERNAME"].ToString();
				}
				if(row["PASSWORD"]!=null)
				{
					model.PASSWORD=row["PASSWORD"].ToString();
				}
				if(row["STATE"]!=null)
				{
					model.STATE=row["STATE"].ToString();
				}
				if(row["LASTLOGINTIME"]!=null && row["LASTLOGINTIME"].ToString()!="")
				{
					model.LASTLOGINTIME=DateTime.Parse(row["LASTLOGINTIME"].ToString());
				}
				if(row["CURRENTLOGINTIME"]!=null && row["CURRENTLOGINTIME"].ToString()!="")
				{
					model.CURRENTLOGINTIME=DateTime.Parse(row["CURRENTLOGINTIME"].ToString());
				}
				if(row["DESCRIPTION"]!=null)
				{
					model.DESCRIPTION=row["DESCRIPTION"].ToString();
				}
                if(row["ZYZH"]!=null)
				{
					model.ZYZH=row["ZYZH"].ToString();
				}
                if (row["ROLEID"] != null && row["ROLEID"].ToString()!="")
				{
                    model.ROLEID =int.Parse(row["ROLEID"].ToString());
				}
			}
			return model;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
            strSql.Append("select USERID,USERNAME,PASSWORD,STATE,LASTLOGINTIME,CURRENTLOGINTIME,DESCRIPTION,ZYZH,ROLEID ");
			strSql.Append(" FROM TBUSERS ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return DbHelperPostSQL.Query(strSql.ToString());
		}

		/// <summary>
		/// 获得前几行数据
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select ");
			if(Top>0)
			{
				strSql.Append(" top "+Top.ToString());
			}
            strSql.Append(" USERID,USERNAME,PASSWORD,STATE,LASTLOGINTIME,CURRENTLOGINTIME,DESCRIPTION,ZYZH,ROLEID ");
			strSql.Append(" FROM TBUSERS ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by " + filedOrder);
			return DbHelperPostSQL.Query(strSql.ToString());
		}

		/// <summary>
		/// 获取记录总数
		/// </summary>
		public int GetRecordCount(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) FROM TBUSERS ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
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
			StringBuilder strSql=new StringBuilder();
			strSql.Append("SELECT * FROM ( ");
			strSql.Append(" SELECT ROW_NUMBER() OVER (");
			if (!string.IsNullOrEmpty(orderby.Trim()))
			{
				strSql.Append("order by T." + orderby );
			}
			else
			{
				strSql.Append("order by T. desc");
			}
			strSql.Append(")AS Row, T.*  from TBUSERS T ");
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
			parameters[0].Value = "TBUSERS";
			parameters[1].Value = "";
			parameters[2].Value = PageSize;
			parameters[3].Value = PageIndex;
			parameters[4].Value = 0;
			parameters[5].Value = 0;
			parameters[6].Value = strWhere;	
			return DbHelperPostSQL.RunProcedure("UP_GetRecordByPage",parameters,"ds");
		}*/

		#endregion  BasicMethod
		#region  ExtensionMethod

		#endregion  ExtensionMethod
	}
}

