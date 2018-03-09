using DXFDXT.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DXFDXT.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 获取坟地信息列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="fdno">坟地编号</param>
        /// <param name="villname">村庄名称</param>
        /// <param name="szname">死者名称</param>
        /// <param name="szsex">死者性别</param>
        /// <param name="swstarttime">死亡时间</param>
        /// <param name="swendtime"></param>
        /// <param name="jsname">家属名称</param>
        /// <param name="ishh">是否火化</param>
        /// <param name="isqf">迁坟意向</param>
        /// <returns></returns>
        public ActionResult GetFdInfo(Pagination pagination, string fdno, string szsex, string jsname, string geom, string ishh, string isqf)
        {
            string condition = "1=1";
            if (!string.IsNullOrEmpty(fdno))
            {
                condition += " and (fdno like '%" + fdno + "%' or szname like '%" + fdno + "%' )";
            }

            if (!string.IsNullOrEmpty(szsex))
            {
                if (szsex == "0")
                {
                    condition += " and (szsex=" + szsex + " or szsex is null) ";
                }
                else
                {
                    condition += " and szsex=" + szsex + " ";
                }
            }
            if (!string.IsNullOrEmpty(ishh))
            {
                if (ishh == "0")
                {
                    condition += " and (isfire=" + ishh + " or isfire is null) ";
                }
                else
                {
                    condition += " and isfire=" + ishh + " ";
                }
            }
            if (!string.IsNullOrEmpty(isqf))
            {
                if (isqf == "0")
                {
                    condition += " and (qfyx=" + isqf + " or qfyx is null) ";
                }
                else
                {
                    condition += " and qfyx=" + isqf + " ";
                }
            }
            if (!string.IsNullOrEmpty(jsname))
            {
                condition += " and jsname like '%" + jsname + "%' ";
            }
            if (!string.IsNullOrEmpty(geom))
            {
                condition += " and st_intersects(st_geomfromtext('" + geom + "',4326),T.geom) ";
            }

            int startRecord = (pagination.page * pagination.rows) + 1;
            int endRecord = (pagination.page + 1) * pagination.rows;
            DataTable dt = new BLL.TBFDINFO().GetSzListByPage(condition, "", startRecord, endRecord).Tables[0];
           
            List<Model.TBFDINFO> lst = new List<Model.TBFDINFO>();
            if (dt != null && dt.Rows.Count > 0)
            {
                int index = startRecord;
                foreach (DataRow dr in dt.Rows)
                {
                    Model.TBFDINFO model = new Model.TBFDINFO();
                    model.Index = index;
                    model.FDID = dr["FDID"].ToString();
                    model.FDNO = dr["FDNO"].ToString();
                    model.FDMANID = dr["FDMANID"].ToString();
                    model.SZNAME = dr["SZNAME"].ToString();
                    if (dr["SZSEX"].ToString() != "")
                    {
                        model.SZSEX = int.Parse(dr["SZSEX"].ToString());
                    }
                    if (dr["ISFIRE"].ToString() != "")
                    {
                        model.ISFIRE = int.Parse(dr["ISFIRE"].ToString());
                    }
                    if (dr["SZDATE"].ToString() != "")
                    {
                        model.SWDATE = DateTime.Parse(dr["SZDATE"].ToString());
                    }
                    if (dr["GEOM"].ToString() != "")
                    {
                        model.GEOM = dr["GEOM"].ToString();
                    }
                    lst.Add(model);
                    index++;
                }
            }
            int totalCount = new BLL.TBFDINFO().GetSzRecordCount(condition);
            return Json(new { rows = lst, total = totalCount });
        }
        /// <summary>
        /// 统计
        /// </summary>
        /// <returns></returns>
        public ActionResult Statics()
        {

            return View();
        }
        /// <summary>
        /// 导出Excel，后续在做自定义导出
        /// </summary>
        /// <returns></returns>
        public ActionResult ExportExcel()
        {

            string condition = string.Empty;          
            string excelfilename = "大兴区北臧村镇坟墓信息系统.xls";
            DataTable dtNew = new DataTable("table");
            dtNew.Columns.Add("坟地编号");
            dtNew.Columns.Add("所在村域");
            dtNew.Columns.Add("死者姓名");
            dtNew.Columns.Add("死者性别");
            dtNew.Columns.Add("死亡日期"); 
            dtNew.Columns.Add("是否火化");
            dtNew.Columns.Add("家属姓名"); 
            dtNew.Columns.Add("联系电话");
            dtNew.Columns.Add("经度");
            dtNew.Columns.Add("纬度");
            //模拟从数据库获取的数据表
            DataTable dtsql = new BLL.TBFDMAN().exportExcel().Tables[0];
            foreach (DataRow item in dtsql.Rows)
            {
                string[] point = item["point"].ToString().Split(' ');
                DataRow dr = dtNew.NewRow();
                dr["坟地编号"] = item["no"];
                dr["所在村域"] = item["vill"]; 
                dr["死者姓名"] = item["name"];
                dr["死者性别"] = item["sex"].ToString() == "0" ? "男" : "女";
                dr["死亡日期"] = !string.IsNullOrEmpty(item["date"].ToString() )? Convert.ToDateTime(item["date"]).Year.ToString() : "";
                dr["是否火化"] = item["isfire"].ToString() == "1" ? "是" : "否";
                dr["家属姓名"] = item["js"];
                dr["联系电话"] = item["phone"];
                dr["经度"] = point[0]; dr["纬度"] = point[1].Replace(")","");
                dtNew.Rows.Add(dr);
            }
           
            dtNew.TableName = "sheet1";
            return File(ExcelHelper.Export(new List<DataTable>() { dtNew }, excelfilename), "application/ms-excel", Url.Encode(excelfilename));
 
        }
        /// <summary>
        /// 获取图表
        /// </summary>
        /// <param name="fdno"></param>
        /// <param name="villname"></param>
        /// <param name="szname"></param>
        /// <param name="szsex"></param>
        /// <param name="swstarttime"></param>
        /// <param name="swendtime"></param>
        /// <param name="jsname"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult GetChart(string fdno, string szsex, string jsname, string type, string geom, string ishh, string isqf)
        {
            string condition = "1=1";
            if (!string.IsNullOrEmpty(fdno))
            {
                condition += " and (fdno like '%" + fdno + "%' or szname like '%" + fdno + "%' )";
            }
            if (!string.IsNullOrEmpty(ishh))
            {
                condition += " and isfire=" + ishh + " ";
            }
            if (!string.IsNullOrEmpty(isqf))
            {
                condition += " and qfyx=" + isqf + " ";
            }
            if (!string.IsNullOrEmpty(szsex))
            {
                condition += " and szsex=" + szsex + " ";
            }

            if (!string.IsNullOrEmpty(jsname))
            {
                condition += " and jsname like '%" + jsname + "%' ";
            }
            if (!string.IsNullOrEmpty(geom))
            {

            }
            //村庄
            if (type == "0")
            {
                DataTable dt = new BLL.TBFDINFO().GetChartVillage(condition, "villname").Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    string xaxis = string.Empty;
                    string series = string.Empty;
                    series += "[{data:[";
                    xaxis += "[";
                    foreach (DataRow dr in dt.Rows)
                    {
                        int count = int.Parse(dr["count"].ToString() == "" ? "0" : dr["count"].ToString());
                        if (count > 0)
                        {
                            series += count + ",";
                            string name = dr["name"].ToString() == "" ? "未知" : dr["name"].ToString();
                            xaxis += "'" + name + "'" + ",";
                        }

                    }
                    series = series.Substring(0, series.Length - 1);
                    series += "]}]";
                    xaxis = xaxis.Substring(0, xaxis.Length - 1);
                    xaxis += "]";
                    return Content(series + "/" + xaxis);
                }
                else
                {
                    string xaxis = string.Empty;
                    string series = string.Empty;
                    series += "[{name:'',data:[";
                    xaxis += "[";
                    series += "]}]";
                    xaxis += "]";
                    return Content(series + "/" + xaxis);
                }
            }
            else if (type == "1")
            {
                //性别
                DataTable dt = new BLL.TBFDINFO().GetChart(condition, "szsex").Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    string xaxis = string.Empty;
                    string series = string.Empty;
                    series += "[{data:[";
                    xaxis += "[";
                    foreach (DataRow dr in dt.Rows)
                    {
                        int count = int.Parse(dr["count"].ToString() == "" ? "0" : dr["count"].ToString());
                        if (count > 0)
                        {
                            string name = "男";
                            if (dr["name"].ToString() == "1")
                            {
                                name = "女";
                            }
                            series += count + ",";
                            xaxis += "'" + name + "'" + ",";
                        }

                    }
                    series = series.Substring(0, series.Length - 1);
                    series += "]}]";
                    xaxis = xaxis.Substring(0, xaxis.Length - 1);
                    xaxis += "]";
                    return Content(series + "/" + xaxis);
                }
                else
                {
                    string xaxis = string.Empty;
                    string series = string.Empty;
                    series += "[{name:'',data:[";
                    xaxis += "[";
                    series += "]}]";
                    xaxis += "]";
                    return Content(series + "/" + xaxis);
                }
            }
            else if (type == "2")
            {
                //迁坟意向

                DataTable dt = new BLL.TBFDINFO().GetChart(condition, "qfyx").Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    string xaxis = string.Empty;
                    string series = string.Empty;
                    series += "[{data:[";
                    xaxis += "[";
                    foreach (DataRow dr in dt.Rows)
                    {
                        int count = int.Parse(dr["count"].ToString() == "" ? "0" : dr["count"].ToString());
                        if (count > 0)
                        {
                            string name = "放弃";
                            if (dr["name"].ToString() == "1")
                            {
                                name = "迁坟";
                            }
                            series += count + ",";
                            xaxis += "'" + name + "'" + ",";
                        }

                    }
                    series = series.Substring(0, series.Length - 1);
                    series += "]}]";
                    xaxis = xaxis.Substring(0, xaxis.Length - 1);
                    xaxis += "]";
                    return Content(series + "/" + xaxis);
                }
                else
                {
                    string xaxis = string.Empty;
                    string series = string.Empty;
                    series += "[{name:'',data:[";
                    xaxis += "[";
                    series += "]}]";
                    xaxis += "]";
                    return Content(series + "/" + xaxis);
                }
            }
            else if (type == "3")
            {
                //是否火化

                DataTable dt = new BLL.TBFDINFO().GetChart(condition, "isfire").Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    string xaxis = string.Empty;
                    string series = string.Empty;
                    series += "[{data:[";
                    xaxis += "[";
                    foreach (DataRow dr in dt.Rows)
                    {
                        int count = int.Parse(dr["count"].ToString() == "" ? "0" : dr["count"].ToString());
                        if (count > 0)
                        {
                            string name = "否";
                            if (dr["name"].ToString() == "1")
                            {
                                name = "是";
                            }
                            series += count + ",";
                            xaxis += "'" + name + "'" + ",";
                        }

                    }
                    series = series.Substring(0, series.Length - 1);
                    series += "]}]";
                    xaxis = xaxis.Substring(0, xaxis.Length - 1);
                    xaxis += "]";
                    return Content(series + "/" + xaxis);
                }
                else
                {
                    string xaxis = string.Empty;
                    string series = string.Empty;
                    series += "[{name:'',data:[";
                    xaxis += "[";
                    series += "]}]";
                    xaxis += "]";
                    return Content(series + "/" + xaxis);
                }
            }
            return Content("");
        }
        /// <summary>
        /// 删除坟地人员信息
        /// </summary>
        /// <param name="fdmanid"></param>
        /// <param name="fdid"></param>
        /// <returns></returns>
        public ActionResult DelFdMan(string fdmanid, string fdid)
        {
            bool result = new BLL.TBFDMAN().Delete(" fdmanid='" + fdmanid + "'");
            if (result)
            {
                DataTable dt = new BLL.TBFDMAN().GetList(" fdid='" + fdid + "'").Tables[0];
                if (dt == null || (dt != null && dt.Rows.Count == 0))
                {
                    new BLL.TBFDINFO().Delete(" fdid='" + fdid + "'");
                }
                return Content("1");
            }
            return Content("");
        }
        /// <summary>
        /// 坟地编辑信息
        /// </summary>
        /// <param name="fdid"></param>
        /// <param name="fdmanid"></param>
        /// <returns></returns>
        public ActionResult EditFd(string fdid, string fdmanid)
        {
            Model.TBFDINFO fdModel = new BLL.TBFDINFO().GetModel("fdid='" + fdid + "'");
            if (fdModel == null)
            {
                fdModel = new Model.TBFDINFO();
            }
            Model.TBFDMAN model = new BLL.TBFDMAN().GetModel(" fdmanid='" + fdmanid + "'");
            if (model == null)
            {
                model = new Model.TBFDMAN();
            }
            fdModel.SZNAME = model.SZNAME;
            fdModel.SZSEX = model.SZSEX;
            fdModel.SWDATE = model.SZDATE;
            fdModel.RELATION = model.RELATION;
            fdModel.FDMANID = model.FDMANID;
            fdModel.DEMO = model.DEMO;
            fdModel.ISFIRE = model.ISFIRE;
            fdModel.JSNAME = model.JSNAME;
            fdModel.JSPHONE = model.JSPHONE;
            return View(fdModel);
        }
        public ActionResult Save(Model.TBFDINFO model)
        {
            Model.TBFDINFO fdModel = new BLL.TBFDINFO().GetModel("fdid='" + model.FDID + "'");
            if (fdModel != null)
            {
                fdModel.VILLNAME = model.VILLNAME;
                fdModel.QFYX = model.QFYX;
                fdModel.DEMO = model.DEMO;
                bool result = new BLL.TBFDINFO().UpdateFd(fdModel);
                if (result)
                {
                    Model.TBFDMAN fdMan = new BLL.TBFDMAN().GetModel(" fdmanid='" + model.FDMANID + "'");
                    fdMan.SZNAME = model.SZNAME;
                    fdMan.SZSEX = model.SZSEX;
                    fdMan.SZDATE = model.SWDATE;
                    fdMan.RELATION = model.RELATION;
                    fdMan.DEMO = model.DEMO;
                    fdMan.ISFIRE = model.ISFIRE;
                    fdMan.JSNAME = model.JSNAME;
                    fdMan.JSPHONE = model.JSPHONE;
                    result = new BLL.TBFDMAN().Update(fdMan);
                    if (result)
                    {
                        return Content("1");
                    }
                }
            }
            return Content("");
        }
        /// <summary>
        /// 得到相同坟地合并的死者姓名
        /// </summary>
        /// <param name="fdid"></param>
        /// <returns></returns>
        public ActionResult getfdszname(string fdid )
        {
            if (!string.IsNullOrEmpty(fdid))
            {
                List<DXFDXT.Model.TBFDMAN> tbfdmanlist = new BLL.TBFDMAN().GetModelList("FDID='" + fdid + "'");
                string szname = "";
                for (int i = 0; i < tbfdmanlist.Count; i++)
                {
                    if (i != tbfdmanlist.Count - 1)
                    {
                        if (!string.IsNullOrEmpty(tbfdmanlist[i].SZNAME))
                        {
                            szname += tbfdmanlist[i].SZNAME + "，";
                        }
                        else
                        {
                            szname += "无名，";
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(tbfdmanlist[i].SZNAME))
                        {
                            szname += tbfdmanlist[i].SZNAME;
                        }
                        else
                        {
                            szname += "无名";
                        }
                    }
                }
                //List<DXFDXT.Model.TBFDINFO> model=new List<DXFDXT.Model.TBFDINFO>;
                //model[0].FDNO=fdModel.FDNO;
                //model[0].SZNAME=szname;
                //string json = JsonHelper.ObjectToJSON(fdModel);
                return Content(szname);
            }
            else
                return Content("无名");
        }
    }
}
