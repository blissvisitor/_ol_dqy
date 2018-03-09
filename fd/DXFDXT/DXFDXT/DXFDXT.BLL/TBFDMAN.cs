﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXFDXT.BLL
{
    public class TBFDMAN
    {
        private readonly DXFDXT.DAL.TBFDMAN dal = new DXFDXT.DAL.TBFDMAN();
        public TBFDMAN()
        { }
        #region  BasicMethod

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(DXFDXT.Model.TBFDMAN model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(DXFDXT.Model.TBFDMAN model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(string strWhere)
        {
            //该表无主键信息，请自定义主键/条件字段
            return dal.Delete(strWhere);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public DXFDXT.Model.TBFDMAN GetModel(string strWhere)
        {
            //该表无主键信息，请自定义主键/条件字段
            return dal.GetModel(strWhere);
        }


        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return dal.GetList(strWhere);
        }
        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            return dal.GetList(Top, strWhere, filedOrder);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<DXFDXT.Model.TBFDMAN> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<DXFDXT.Model.TBFDMAN> DataTableToList(DataTable dt)
        {
            List<DXFDXT.Model.TBFDMAN> modelList = new List<DXFDXT.Model.TBFDMAN>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                DXFDXT.Model.TBFDMAN model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = dal.DataRowToModel(dt.Rows[n]);
                    if (model != null)
                    {
                        modelList.Add(model);
                    }
                }
            }
            return modelList;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetAllList()
        {
            return GetList("");
        }

        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            return dal.GetRecordCount(strWhere);
        }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            return dal.GetListByPage(strWhere, orderby, startIndex, endIndex);
        }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        //public DataSet GetList(int PageSize,int PageIndex,string strWhere)
        //{
        //return dal.GetList(PageSize,PageIndex,strWhere);
        //}
        public DataSet exportExcel()
        {
            return dal.exportExcel();
        }
        #endregion  BasicMethod

        #region 附加业务  导出报表
       
        #endregion

    }
}
