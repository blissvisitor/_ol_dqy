﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXFDXT.Model
{
    public class TBFDINFO
    {
        #region Model
        private string _fdid;
        private string _villname;
        private string _fdno; 
        //private int _isfire;
        //public string _jsname;
        //public string _jsphone; 
        public int _qfyx;
        public string _demo;
        /// <summary>
        /// 坟地信息id
        /// </summary>
        public string FDID
        {
            set { _fdid = value; }
            get { return _fdid; }
        }
        /// <summary>
        /// 村庄名称
        /// </summary>
        public string VILLNAME
        {
            set { _villname = value; }
            get { return _villname; }
        }
        /// <summary>
        /// 坟地编号
        /// </summary>
        public string FDNO
        {
            set { _fdno = value; }
            get { return _fdno; }
        }
       
        ///// <summary>
        ///// 是否火花
        ///// </summary>
        //public int ISFIRE
        //{
        //    set { _isfire = value; }
        //    get { return _isfire; }
        //}
    
        ///// <summary>
        ///// 家属姓名
        ///// </summary>
        //public string JSNAME
        //{
        //    set { _jsname = value; }
        //    get { return _jsname; }
        //}
        ///// <summary>
        ///// 家属电话
        ///// </summary>
        //public string JSPHONE
        //{
        //    set { _jsphone = value; }
        //    get { return _jsphone; }
        //}
       
        /// <summary>
        /// 迁坟意向
        /// </summary>
        public int QFYX
        {
            set { _qfyx = value; }
            get { return _qfyx; }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string DEMO
        {
            set { _demo = value; }
            get { return _demo; }
        }
        public int Index;
        public string GEOM { get; set; }
        /// <summary>
        /// 死者姓名
        /// </summary>
        public string SZNAME { get; set; }
        /// <summary>
        /// 死者性别
        /// </summary>
        public int SZSEX { get; set; }
        /// <summary>
        /// 死亡时间
        /// </summary>
        public DateTime? SWDATE { get; set; }
        public string FDMANID { get; set; }
        /// <summary>
        /// 家属关系
        /// </summary>
        public string RELATION { get; set; }
        /// <summary>
        /// 是否火化
        /// </summary>
        public int ISFIRE { get; set; }
        /// <summary>
        /// 家属姓名
        /// </summary>
        public string JSNAME { get; set; }
        /// <summary>
        /// 家属电话
        /// </summary>
        public string JSPHONE { get; set; }
        #endregion Model
    }
}
