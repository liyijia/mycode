using LY.EMIS5.Admin.Controllers.Report;
using LY.EMIS5.BLL;
using LY.EMIS5.Const;
using LY.EMIS5.Entities.Core.Memberships;
using LY.EMIS5.Entities.Core.Organizations;
using LY.EMIS5.Entities.Report;
using NHibernate.Extensions.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LY.EMIS5.Admin.Controllers
{
    public class SalesController : Controller
    {

        [HttpGet]
        public ActionResult Index()
        {

            return View();
        }
        [HttpGet]
        public ActionResult Type1()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetArea()
        {
            var memberof = DbHelper.Query<Manager>(c => c.Id == ManagerImp.Current.Id).First().MemberOf.ToList();
            List<object> jsonlist = new List<object>();
            memberof.ForEach(c =>
            {
                if (c.Children.Count > 0)
                {
                    c.Children.ToList().ForEach(d =>
                    {
                        jsonlist.Add(new { d.Id, d.Name });
                    });
                }
                else
                {
                    jsonlist.Add(new { c.Id, c.Name });
                }
            });
            return Json(jsonlist);
        }
        [HttpPost]
        public JsonResult GetKefu()
        {
            if (ManagerImp.Current.Roles.Any(c => c.Name != "客服"))
            {
                List<int> managerdepartments = new List<int>();
                DbHelper.Query<Department>(e => e.Leader.Id == ManagerImp.Current.Id).ToList().ForEach(c =>
                {
                    managerdepartments.Add(c.Id);
                    if (c.Children.Count > 0)
                        managerdepartments.AddRange(c.Children.Select(d => d.Id).ToList());
                });
                var ll = DbHelper.Query<Manager>(c => c.MemberOf.Any(d => managerdepartments.Contains(d.Id)));
                return Json(ll.Select(c => new { c.Id, c.Name }));
            }
            else
            {
                List<object> ret = new List<object>();
                ret.Add(new { ManagerImp.Current.Id, ManagerImp.Current.Name });
                return Json(ret);
            }
        }
        [HttpPost]
        public JsonResult GetSchools(int areaid)
        {
            if (ManagerImp.Current.Roles.Any(c => c.Name != "客服"))
            {
                List<int> managerdepartments = new List<int>();
                DbHelper.Query<Department>(e => e.Leader.Id == ManagerImp.Current.Id).ToList().ForEach(c =>
                {
                    managerdepartments.Add(c.Id);
                    if (c.Children.Count > 0)
                        managerdepartments.AddRange(c.Children.Where(d => d.Id == areaid).Select(d => d.Id));
                });

                return Json(DbHelper.Query<School>(c => managerdepartments.Contains(c.MemberOf.Id)).OrderBy(c => c.Name).Select(c => new { c.Id, c.Name }));
            }
            else
            {
                return Json(DbHelper.Query<School>(c => c.SchoolManager.Contains(ManagerImp.Current) && c.MemberOf.Id == areaid).OrderBy(c => c.Name).Select(c => new { c.Id, c.Name }));
            }
            //}
        }
        [HttpPost]
        public JsonResult GetSchoolsByKefu(int kefuid)
        {
            if (ManagerImp.Current.Roles.Any(c => c.Name != "客服"))
            {
                List<int> managerdepartments = new List<int>();
                DbHelper.Query<Department>(e => e.Leader.Id == ManagerImp.Current.Id).ToList().ForEach(c =>
                {
                    managerdepartments.Add(c.Id);
                    if (c.Children.Count > 0)
                        managerdepartments.AddRange(c.Children.Select(d => d.Id));
                });

                return Json(DbHelper.Query<School>(c => managerdepartments.Contains(c.MemberOf.Id) && c.SchoolManager.Contains(ManagerImp.Get(kefuid))).OrderBy(c => c.Name).Select(c => new { c.Id, c.Name }));
            }
            else
            {
                return Json(DbHelper.Query<School>(c => c.SchoolManager.Contains(ManagerImp.Current) && c.SchoolManager.Contains(ManagerImp.Get(kefuid))).OrderBy(c => c.Name).Select(c => new { c.Id, c.Name }));
            }
            // }
        }
        [HttpPost]
        public JsonResult GetGrades(int schoolid)
        {
            if (ManagerImp.Current.Roles.Any(c => c.Name != "客服"))
            {
                return Json(DbHelper.Query<Grade>(c => c.MemberOf.Id == schoolid).OrderBy(c => c.Name).Select(c => new { c.Id, c.Name }));
            }
            else
            {
                return Json(DbHelper.Query<Grade>(c => c.MemberOf.SchoolManager.Contains(ManagerImp.Current) && c.MemberOf.Id == schoolid).OrderBy(c => c.Name).Select(c => new { c.Id, c.Name }));
            }
        }
        [HttpPost]
        public JsonResult GetClasses(int gradeid)
        {
            if (ManagerImp.Current.Roles.Any(c => c.Name != "客服"))
            {
                return Json(DbHelper.Query<Clazz>(c => c.MemberOf.Id == gradeid).OrderBy(c => c.Name).Select(c => new { c.Id, c.Name }));
            }
            else
            {
                return Json(DbHelper.Query<Clazz>(c => c.Manager == ManagerImp.Current && c.MemberOf.Id == gradeid).OrderBy(c => c.Name).Select(c => new { c.Id, c.Name }));
            }
        }
        public class datastruct
        {
            public int areaid { get; set; }
            public string areaname { get; set; }
            public List<object> coldata { get; set; }
            public int total { get; set; }
        }
        public class datastructdetail
        {
            public string schoolname { get; set; }
            public string gradename { get; set; }
            public string classname { get; set; }
            public string kidname { get; set; }
            public string mobile { get; set; }
            public List<object> coldata { get; set; }
            public int total { get; set; }
        }

        [HttpPost]
        public JsonResult getdataByArea(string areaidstr, string schoolidstr, string gradeidstr, string classidstr, string begintimestr, string endtimestr)
        {
            int areaid = int.Parse(areaidstr);
            int schoolid = int.Parse(schoolidstr);
            int gradeid = int.Parse(gradeidstr);
            int classid = int.Parse(classidstr);
            int begintime = int.Parse(DateTime.Parse(begintimestr).ToString("yyyyMM"));
            int endtime = int.Parse(DateTime.Parse(endtimestr).AddMonths(1).ToString("yyyyMM"));

            List<datastruct> results = new List<datastruct>();
            var studentlist = DbHelper.Query<Report_Customer>(c => c.TheTime >= begintime
                && c.TheTime < endtime
                && new ReportCommen().GetChildManager().Contains(c.Manager.Id)
                );
            if (classid != -1)
            {
                studentlist = studentlist.Where(c => c.School.Id == schoolid && c.Grade.Id == gradeid && c.Clazz.Id == classid);
                studentlist.GroupBy(c => new { c.Clazz.Id, c.Clazz.Name }).Select(c => new { c.Key.Id, c.Key.Name }).ToList().ForEach(e =>
                {
                    var ds = new datastruct()
                    {
                        areaid = e.Id,
                        areaname = e.Name,
                        coldata = new List<object>(),
                    };
                    var tt = studentlist.Where(c => c.Grade.Id == e.Id)
                        .OrderBy(c => c.Clazz.Id)
                        .ThenBy(c => c.TheTime)
                        .GroupBy(c => new { c.Clazz.Id, c.Clazz.Name, c.TheTime })
                        .Select(c => new { areaid = c.Key.Id, areaname = c.Key.Name, thetime = c.Key.TheTime, imoney = c.Sum(f => f.Money) })
                        .ToList();
                    tt.ForEach(f =>
                    {
                        ds.coldata.Add(f);
                        ds.total += (int)f.imoney;
                    });
                    results.Add(ds);
                });
            }
            if (gradeid != -1)
            {
                studentlist = studentlist.Where(c => c.School.Id == schoolid && c.Grade.Id == gradeid);
                studentlist.GroupBy(c => new { c.Clazz.Id, c.Clazz.Name }).Select(c => new { c.Key.Id, c.Key.Name }).ToList().ForEach(e =>
                {
                    var ds = new datastruct()
                    {
                        areaid = e.Id,
                        areaname = e.Name,
                        coldata = new List<object>(),
                    };
                    var tt = studentlist.Where(c => c.Clazz.Id == e.Id)
                        .OrderBy(c => c.Clazz.Name)
                        .ThenBy(c => c.TheTime)
                        .GroupBy(c => new { c.Clazz.Id, c.Clazz.Name, c.TheTime })
                        .Select(c => new { areaid = c.Key.Id, areaname = c.Key.Name, thetime = c.Key.TheTime, imoney = c.Sum(f => f.Money) })
                        .ToList();
                    tt.ForEach(f =>
                    {
                        ds.coldata.Add(f);
                        ds.total += (int)f.imoney;
                    });
                    results.Add(ds);
                });
            }
            else if (schoolid != -1)
            {
                studentlist = studentlist.Where(c => c.School.Id == schoolid);
                studentlist.GroupBy(c => new { c.Grade.Id, c.Grade.Name }).Select(c => new { c.Key.Id, c.Key.Name }).ToList().ForEach(e =>
                {
                    var ds = new datastruct()
                    {
                        areaid = e.Id,
                        areaname = e.Name,
                        coldata = new List<object>(),
                    };
                    var tt = studentlist.Where(c => c.Grade.Id == e.Id)
                        .OrderBy(c => c.Grade.Id)
                        .ThenBy(c => c.TheTime)
                        .GroupBy(c => new { c.Grade.Id, c.Grade.Name, c.TheTime })
                        .Select(c => new { areaid = c.Key.Id, areaname = c.Key.Name, thetime = c.Key.TheTime, imoney = c.Sum(f => f.Money) })
                        .ToList();
                    tt.ForEach(f =>
                    {
                        ds.coldata.Add(f);
                        ds.total += (int)f.imoney;
                    });
                    results.Add(ds);
                });
            }
            else if (areaid != -1)
            {
                studentlist = studentlist.Where(c => c.Department.Id == areaid);
                studentlist.GroupBy(c => new { c.School.Id, c.School.Name }).Select(c => new { c.Key.Id, c.Key.Name }).ToList().ForEach(e =>
                {
                    var ds = new datastruct()
                    {
                        areaid = e.Id,
                        areaname = e.Name,
                        coldata = new List<object>(),
                    };
                    var tt = studentlist.Where(c => c.School.Id == e.Id)
                        .OrderBy(c => c.School.Id)
                        .ThenBy(c => c.TheTime)
                        .GroupBy(c => new { c.School.Id, c.School.Name, c.TheTime })
                        .Select(c => new { areaid = c.Key.Id, areaname = c.Key.Name, thetime = c.Key.TheTime, imoney = c.Sum(f => f.Money) })
                        .ToList();
                    tt.ForEach(f =>
                    {
                        ds.coldata.Add(f);
                        ds.total += (int)f.imoney;
                    });
                    results.Add(ds);
                });
            }
            else
            {
                studentlist.GroupBy(c => new { c.Department.Id, c.Department.Name }).Select(c => new { c.Key.Id, c.Key.Name }).ToList().ForEach(e =>
                {
                    var ds = new datastruct()
                    {
                        areaid = e.Id,
                        areaname = e.Name,
                        coldata = new List<object>(),
                    };
                    var tt = studentlist.Where(c => c.Department.Id == e.Id)
                        .OrderBy(c => c.Department.Id)
                        .ThenBy(c => c.TheTime)
                        .GroupBy(c => new { c.Department.Id, c.Department.Name, c.TheTime })
                        .Select(c => new { areaid = c.Key.Id, areaname = c.Key.Name, thetime = c.Key.TheTime, imoney = c.Sum(f => f.Money) })
                        .ToList();
                    tt.ForEach(f =>
                    {
                        ds.coldata.Add(f);
                        ds.total += (int)f.imoney;
                    });
                    results.Add(ds);
                });
            }

            return Json(results);
        }
        [HttpPost]
        public JsonResult getdataByKefu(string kefuidstr, string schoolidstr, string gradeidstr, string classidstr, string begintimestr, string endtimestr)
        {
            int kefuid = int.Parse(kefuidstr);
            int schoolid = int.Parse(schoolidstr);
            int gradeid = int.Parse(gradeidstr);
            int classid = int.Parse(classidstr);
            int begintime = int.Parse(DateTime.Parse(begintimestr).ToString("yyyyMM"));
            int endtime = int.Parse(DateTime.Parse(endtimestr).AddMonths(1).ToString("yyyyMM"));

            List<datastruct> results = new List<datastruct>();
            var studentlist = DbHelper.Query<Report_Customer>(c => c.TheTime >= begintime
                && c.TheTime < endtime
                && new ReportCommen().GetChildManager().Contains(c.Manager.Id)
                );
            if (kefuid != -1)
            {
                studentlist = studentlist.Where(c => c.Manager.Id == kefuid);
                if (classid != -1)
                {
                    studentlist = studentlist.Where(c => c.School.Id == schoolid && c.Grade.Id == gradeid && c.Clazz.Id == classid);
                    studentlist.GroupBy(c => new { c.Clazz.Id, c.Clazz.Name }).Select(c => new { c.Key.Id, c.Key.Name }).ToList().ForEach(e =>
                    {
                        var ds = new datastruct()
                        {
                            areaid = e.Id,
                            areaname = e.Name,
                            coldata = new List<object>(),
                        };
                        var tt = studentlist.Where(c => c.Grade.Id == e.Id)
                            .OrderBy(c => c.Clazz.Id)
                            .ThenBy(c => c.TheTime)
                            .GroupBy(c => new { c.Clazz.Id, c.Clazz.Name, c.TheTime })
                            .Select(c => new { areaid = c.Key.Id, areaname = c.Key.Name, thetime = c.Key.TheTime, imoney = c.Sum(f => f.Money) })
                            .ToList();
                        tt.ForEach(f =>
                        {
                            ds.coldata.Add(f);
                            ds.total += (int)f.imoney;
                        });
                        results.Add(ds);
                    });
                }
                if (gradeid != -1)
                {
                    studentlist = studentlist.Where(c => c.School.Id == schoolid && c.Grade.Id == gradeid);
                    studentlist.GroupBy(c => new { c.Clazz.Id, c.Clazz.Name }).Select(c => new { c.Key.Id, c.Key.Name }).ToList().ForEach(e =>
                    {
                        var ds = new datastruct()
                        {
                            areaid = e.Id,
                            areaname = e.Name,
                            coldata = new List<object>(),
                        };
                        var tt = studentlist.Where(c => c.Clazz.Id == e.Id)
                            .OrderBy(c => c.Clazz.Name)
                            .ThenBy(c => c.TheTime)
                            .GroupBy(c => new { c.Clazz.Id, c.Clazz.Name, c.TheTime })
                            .Select(c => new { areaid = c.Key.Id, areaname = c.Key.Name, thetime = c.Key.TheTime, imoney = c.Sum(f => f.Money) })
                            .ToList();
                        tt.ForEach(f =>
                        {
                            ds.coldata.Add(f);
                            ds.total += (int)f.imoney;
                        });
                        results.Add(ds);
                    });
                }
                else if (schoolid != -1)
                {
                    studentlist = studentlist.Where(c => c.School.Id == schoolid);
                    studentlist.GroupBy(c => new { c.Grade.Id, c.Grade.Name }).Select(c => new { c.Key.Id, c.Key.Name }).ToList().ForEach(e =>
                    {
                        var ds = new datastruct()
                        {
                            areaid = e.Id,
                            areaname = e.Name,
                            coldata = new List<object>(),
                        };
                        var tt = studentlist.Where(c => c.Grade.Id == e.Id)
                            .OrderBy(c => c.Grade.Id)
                            .ThenBy(c => c.TheTime)
                            .GroupBy(c => new { c.Grade.Id, c.Grade.Name, c.TheTime })
                            .Select(c => new { areaid = c.Key.Id, areaname = c.Key.Name, thetime = c.Key.TheTime, imoney = c.Sum(f => f.Money) })
                            .ToList();
                        tt.ForEach(f =>
                        {
                            ds.coldata.Add(f);
                            ds.total += (int)f.imoney;
                        });
                        results.Add(ds);
                    });
                }
                else
                {
                    studentlist.GroupBy(c => new { c.School.Id, c.School.Name }).Select(c => new { c.Key.Id, c.Key.Name }).ToList().ForEach(e =>
                    {
                        var ds = new datastruct()
                        {
                            areaid = e.Id,
                            areaname = e.Name,
                            coldata = new List<object>(),
                        };
                        var tt = studentlist.Where(c => c.School.Id == e.Id)
                            .OrderBy(c => c.School.Id)
                            .ThenBy(c => c.TheTime)
                            .GroupBy(c => new { c.School.Id, c.School.Name, c.TheTime })
                            .Select(c => new { areaid = c.Key.Id, areaname = c.Key.Name, thetime = c.Key.TheTime, imoney = c.Sum(f => f.Money) })
                            .ToList();
                        tt.ForEach(f =>
                        {
                            ds.coldata.Add(f);
                            ds.total += (int)f.imoney;
                        });
                        results.Add(ds);
                    });
                }
            }
            else
            {
                studentlist.GroupBy(c => new { c.Manager.Id, c.Manager.Name }).Select(c => new { c.Key.Id, c.Key.Name }).ToList().ForEach(e =>
                {
                    var ds = new datastruct()
                    {
                        areaid = e.Id,
                        areaname = e.Name,
                        coldata = new List<object>(),
                    };
                    var tt = studentlist.Where(c => c.Manager.Id == e.Id)
                        .OrderBy(c => c.Manager.Id)
                        .ThenBy(c => c.TheTime)
                        .GroupBy(c => new { c.Manager.Id, c.Manager.Name, c.TheTime })
                        .Select(c => new { areaid = c.Key.Id, areaname = c.Key.Name, thetime = c.Key.TheTime, imoney = c.Sum(f => f.Money) })
                        .ToList();
                    tt.ForEach(f =>
                    {
                        ds.coldata.Add(f);
                        ds.total += (int)f.imoney;
                    });
                    results.Add(ds);
                });
            }

            return Json(results);
        }
        [HttpPost]
        public JsonResult getdataBySchool(string schoolidstr, string gradeidstr, string classidstr, string begintimestr, string endtimestr)
        {
            int schoolid = int.Parse(schoolidstr);
            int gradeid = int.Parse(gradeidstr);
            int classid = int.Parse(classidstr);
            int begintime = int.Parse(DateTime.Parse(begintimestr).ToString("yyyyMM"));
            int endtime = int.Parse(DateTime.Parse(endtimestr).AddMonths(1).ToString("yyyyMM"));

            List<datastruct> results = new List<datastruct>();
            var studentlist = DbHelper.Query<Report_Customer>(c => c.TheTime >= begintime
                && c.TheTime < endtime
                && new ReportCommen().GetChildManager().Contains(c.Manager.Id)
                );
            if (classid != -1)
            {
                studentlist = studentlist.Where(c => c.School.Id == schoolid && c.Grade.Id == gradeid && c.Clazz.Id == classid);
                studentlist.GroupBy(c => new { c.Clazz.Id, c.Clazz.Name }).Select(c => new { c.Key.Id, c.Key.Name }).ToList().ForEach(e =>
                {
                    var ds = new datastruct()
                    {
                        areaid = e.Id,
                        areaname = e.Name,
                        coldata = new List<object>(),
                    };
                    var tt = studentlist.Where(c => c.Grade.Id == e.Id)
                        .OrderBy(c => c.Clazz.Id)
                        .ThenBy(c => c.TheTime)
                        .GroupBy(c => new { c.Clazz.Id, c.Clazz.Name, c.TheTime })
                        .Select(c => new { areaid = c.Key.Id, areaname = c.Key.Name, thetime = c.Key.TheTime, imoney = c.Sum(f => f.Money) })
                        .ToList();
                    tt.ForEach(f =>
                    {
                        ds.coldata.Add(f);
                        ds.total += (int)f.imoney;
                    });
                    results.Add(ds);
                });
            }
            if (gradeid != -1)
            {
                studentlist = studentlist.Where(c => c.School.Id == schoolid && c.Grade.Id == gradeid);
                studentlist.GroupBy(c => new { c.Clazz.Id, c.Clazz.Name }).Select(c => new { c.Key.Id, c.Key.Name }).ToList().ForEach(e =>
                {
                    var ds = new datastruct()
                    {
                        areaid = e.Id,
                        areaname = e.Name,
                        coldata = new List<object>(),
                    };
                    var tt = studentlist.Where(c => c.Clazz.Id == e.Id)
                        .OrderBy(c => c.Clazz.Name)
                        .ThenBy(c => c.TheTime)
                        .GroupBy(c => new { c.Clazz.Id, c.Clazz.Name, c.TheTime })
                        .Select(c => new { areaid = c.Key.Id, areaname = c.Key.Name, thetime = c.Key.TheTime, imoney = c.Sum(f => f.Money) })
                        .ToList();
                    tt.ForEach(f =>
                    {
                        ds.coldata.Add(f);
                        ds.total += (int)f.imoney;
                    });
                    results.Add(ds);
                });
            }
            else if (schoolid != -1)
            {
                studentlist = studentlist.Where(c => c.School.Id == schoolid);
                studentlist.GroupBy(c => new { c.Grade.Id, c.Grade.Name }).Select(c => new { c.Key.Id, c.Key.Name }).ToList().ForEach(e =>
                {
                    var ds = new datastruct()
                    {
                        areaid = e.Id,
                        areaname = e.Name,
                        coldata = new List<object>(),
                    };
                    var tt = studentlist.Where(c => c.Grade.Id == e.Id)
                        .OrderBy(c => c.Grade.Id)
                        .ThenBy(c => c.TheTime)
                        .GroupBy(c => new { c.Grade.Id, c.Grade.Name, c.TheTime })
                        .Select(c => new { areaid = c.Key.Id, areaname = c.Key.Name, thetime = c.Key.TheTime, imoney = c.Sum(f => f.Money) })
                        .ToList();
                    tt.ForEach(f =>
                    {
                        ds.coldata.Add(f);
                        ds.total += (int)f.imoney;
                    });
                    results.Add(ds);
                });
            }
            else
            {
                studentlist.GroupBy(c => new { c.School.Id, c.School.Name }).Select(c => new { c.Key.Id, c.Key.Name }).ToList().ForEach(e =>
                {
                    var ds = new datastruct()
                    {
                        areaid = e.Id,
                        areaname = e.Name,
                        coldata = new List<object>(),
                    };
                    var tt = studentlist.Where(c => c.School.Id == e.Id)
                        .OrderBy(c => c.School.Id)
                        .ThenBy(c => c.TheTime)
                        .GroupBy(c => new { c.School.Id, c.School.Name, c.TheTime })
                        .Select(c => new { areaid = c.Key.Id, areaname = c.Key.Name, thetime = c.Key.TheTime, imoney = c.Sum(f => f.Money) })
                        .ToList();
                    tt.ForEach(f =>
                    {
                        ds.coldata.Add(f);
                        ds.total += (int)f.imoney;
                    });
                    results.Add(ds);
                });
            }

            return Json(results);
        }


        [HttpPost]
        public JsonResult getdataByAreaDetail(string areaidstr, string schoolidstr, string gradeidstr, string classidstr, string begintimestr, string endtimestr, string pageIndexstr, string pageSizestr)
        {
            int areaid = int.Parse(areaidstr);
            int schoolid = int.Parse(schoolidstr);
            int gradeid = int.Parse(gradeidstr);
            int classid = int.Parse(classidstr);
            int begintime = int.Parse(DateTime.Parse(begintimestr).ToString("yyyyMM"));
            int endtime = int.Parse(DateTime.Parse(endtimestr).AddMonths(1).ToString("yyyyMM"));
            int pageindex = int.Parse(pageIndexstr);
            int pagesize = int.Parse(pageSizestr);

            List<datastructdetail> results = new List<datastructdetail>();
            var studentlist = DbHelper.Query<Report_Customer>(c => c.TheTime >= begintime
                && c.TheTime < endtime
                && new ReportCommen().GetChildManager().Contains(c.Manager.Id)
                );
            if (classid != -1)
            {
                var studentidlist = studentlist.OrderBy(c => c.School.Id)
                      .ThenBy(c => c.Grade.Id)
                      .ThenBy(c => c.Clazz.Id)
                      .Where(c => c.School.Id == schoolid && c.Grade.Id == gradeid && c.Clazz.Id == classid)
                      .GroupBy(c => new
                      {
                          c.Id,
                      })
                      .Skip((pageindex - 1) * pagesize)
                      .Take(pagesize)
                      .Select(c => new
                      {
                          c.Key.Id
                      }).ToList();

                studentidlist.ForEach(f =>
                {
                    var ds = new datastructdetail()
                    {
                        coldata = new List<object>()
                    };
                    studentlist.Where(c => c.Id == f.Id)
                        .GroupBy(c => new
                        {
                            schoolname = c.School.Name,
                            gradename = c.Grade.Name,
                            classname = c.Clazz.Name,
                            kidname = c.Parent.Kid.Name,
                            mobile = c.Parent.Namecard.Phone,
                            thetime = c.TheTime
                        }).Select(c => new
                        {
                            c.Key.schoolname,
                            c.Key.gradename,
                            c.Key.classname,
                            c.Key.kidname,
                            c.Key.mobile,
                            c.Key.thetime,
                            imoney = c.Sum(g => g.Money)
                        }).ToList().ForEach(c =>
                        {
                            if (ds.schoolname == "")
                            {
                                ds.schoolname = c.schoolname;
                                ds.gradename = c.gradename;
                                ds.classname = c.classname;
                                ds.mobile = c.mobile;
                                ds.kidname = c.kidname;
                            }
                            ds.coldata.Add(c.imoney);
                            ds.coldata.Add(f);
                            ds.total += (int)c.imoney;
                        });
                    results.Add(ds);
                });
            }
            if (gradeid != -1)
            {
                var studentidlist = studentlist.OrderBy(c => c.School.Id)
                      .ThenBy(c => c.Grade.Id)
                      .ThenBy(c => c.Clazz.Id)
                      .Where(c => c.School.Id == schoolid && c.Grade.Id == gradeid)
                      .GroupBy(c => new
                      {
                          c.Id,
                      })
                      .Skip((pageindex - 1) * pagesize)
                      .Take(pagesize)
                      .Select(c => new
                      {
                          c.Key.Id
                      }).ToList();

                studentidlist.ForEach(f =>
                {
                    var ds = new datastructdetail()
                    {
                        coldata = new List<object>()
                    };
                    studentlist.Where(c => c.Id == f.Id)
                        .GroupBy(c => new
                        {
                            schoolname = c.School.Name,
                            gradename = c.Grade.Name,
                            classname = c.Clazz.Name,
                            kidname = c.Parent.Kid.Name,
                            mobile = c.Parent.Namecard.Phone,
                            thetime = c.TheTime
                        }).Select(c => new
                        {
                            c.Key.schoolname,
                            c.Key.gradename,
                            c.Key.classname,
                            c.Key.kidname,
                            c.Key.mobile,
                            c.Key.thetime,
                            imoney = c.Sum(g => g.Money)
                        }).ToList().ForEach(c =>
                        {
                            if (ds.schoolname == "")
                            {
                                ds.schoolname = c.schoolname;
                                ds.gradename = c.gradename;
                                ds.classname = c.classname;
                                ds.mobile = c.mobile;
                                ds.kidname = c.kidname;
                            }
                            ds.coldata.Add(c.imoney);
                            ds.coldata.Add(f);
                            ds.total += (int)c.imoney;
                        });
                    results.Add(ds);
                });
            }
            else if (schoolid != -1)
            {
                var studentidlist = studentlist.OrderBy(c => c.School.Id)
                     .ThenBy(c => c.Grade.Id)
                     .ThenBy(c => c.Clazz.Id)
                     .Where(c => c.School.Id == schoolid)
                     .GroupBy(c => new
                     {
                         c.Id,
                     })
                     .Skip((pageindex - 1) * pagesize)
                     .Take(pagesize)
                     .Select(c => new
                     {
                         c.Key.Id
                     }).ToList();

                studentidlist.ForEach(f =>
                {
                    var ds = new datastructdetail()
                    {
                        coldata = new List<object>()
                    };
                    studentlist.Where(c => c.Id == f.Id)
                        .GroupBy(c => new
                        {
                            schoolname = c.School.Name,
                            gradename = c.Grade.Name,
                            classname = c.Clazz.Name,
                            kidname = c.Parent.Kid.Name,
                            mobile = c.Parent.Namecard.Phone,
                            thetime = c.TheTime
                        }).Select(c => new
                        {
                            c.Key.schoolname,
                            c.Key.gradename,
                            c.Key.classname,
                            c.Key.kidname,
                            c.Key.mobile,
                            c.Key.thetime,
                            imoney = c.Sum(g => g.Money)
                        }).ToList().ForEach(c =>
                        {
                            if (ds.schoolname == "")
                            {
                                ds.schoolname = c.schoolname;
                                ds.gradename = c.gradename;
                                ds.classname = c.classname;
                                ds.mobile = c.mobile;
                                ds.kidname = c.kidname;
                            }
                            ds.coldata.Add(c.imoney);
                            ds.coldata.Add(f);
                            ds.total += (int)c.imoney;
                        });
                    results.Add(ds);
                });
            }
            else if (areaid != -1)
            {
                var studentidlist = studentlist.OrderBy(c => c.School.Id)
                    .ThenBy(c => c.Grade.Id)
                    .ThenBy(c => c.Clazz.Id)
                    .Where(c => c.Department.Id == areaid)
                    .GroupBy(c => new
                    {
                        c.Id,
                    })
                    .Skip((pageindex - 1) * pagesize)
                    .Take(pagesize)
                    .Select(c => new
                    {
                        c.Key.Id
                    }).ToList();

                studentidlist.ForEach(f =>
                {
                    var ds = new datastructdetail()
                    {
                        coldata = new List<object>()
                    };
                    studentlist.Where(c => c.Id == f.Id)
                        .GroupBy(c => new
                        {
                            schoolname = c.School.Name,
                            gradename = c.Grade.Name,
                            classname = c.Clazz.Name,
                            kidname = c.Parent.Kid.Name,
                            mobile = c.Parent.Namecard.Phone,
                            thetime = c.TheTime
                        }).Select(c => new
                        {
                            c.Key.schoolname,
                            c.Key.gradename,
                            c.Key.classname,
                            c.Key.kidname,
                            c.Key.mobile,
                            c.Key.thetime,
                            imoney = c.Sum(g => g.Money)
                        }).ToList().ForEach(c =>
                        {
                            if (ds.schoolname == "")
                            {
                                ds.schoolname = c.schoolname;
                                ds.gradename = c.gradename;
                                ds.classname = c.classname;
                                ds.mobile = c.mobile;
                                ds.kidname = c.kidname;
                            }
                            ds.coldata.Add(c.imoney);
                            ds.coldata.Add(f);
                            ds.total += (int)c.imoney;
                        });
                    results.Add(ds);
                });
            }
            else
            {

                var testlist = studentlist.Select(c => c.Parent.Id).Distinct()
                     .Skip((pageindex - 1) * pagesize)
                     .Take(pagesize).ToList();

                var rrlist = studentlist
                    .Where(c => testlist.Contains(c.Parent.Id))
                     .GroupBy(c => new
                    {
                        schoolname = c.School.Name,
                        gradename = c.Grade.Name,
                        classname = c.Clazz.Name,
                        kidname = c.Parent.Kid.Name,
                        mobile = c.Parent.Namecard.Phone,
                        thetime = c.TheTime
                    });


                var studentidlist = studentlist
                    .GroupBy(c => new { c.Parent.Id })
                    .Select(c => new { parentid = c.Key.Id })
                    .Skip((pageindex - 1) * pagesize)
                    .Take(pagesize).ToList();

                studentidlist.ForEach(f =>
                {
                    var ds = new datastructdetail()
                    {
                        coldata = new List<object>()
                    };
                    studentlist.Where(c => c.Parent.Id == f.parentid)
                        .GroupBy(c => new
                    {
                        schoolname = c.School.Name,
                        gradename = c.Grade.Name,
                        classname = c.Clazz.Name,
                        kidname = c.Parent.Kid.Name,
                        mobile = c.Parent.Namecard.Phone,
                        thetime = c.TheTime
                    }).Select(c => new
                    {
                        c.Key.schoolname,
                        c.Key.gradename,
                        c.Key.classname,
                        c.Key.kidname,
                        c.Key.mobile,
                        c.Key.thetime,
                        imoney = c.Sum(g => g.Money)
                    }).ToList().ForEach(c =>
                    {
                        if (ds.schoolname == "")
                        {
                            ds.schoolname = c.schoolname;
                            ds.gradename = c.gradename;
                            ds.classname = c.classname;
                            ds.mobile = c.mobile;
                            ds.kidname = c.kidname;
                        }
                        ds.coldata.Add(c.imoney);
                        ds.coldata.Add(f);
                        ds.total += (int)c.imoney;
                    });
                    results.Add(ds);
                });
            }
            return Json(results);
        }
        [HttpPost]
        public JsonResult getdataByKefuDetail(string kefuidstr, string schoolidstr, string gradeidstr, string classidstr, string begintimestr, string endtimestr, string pageIndexstr, string pageSizestr)
        {
            int kefuid = int.Parse(kefuidstr);
            int schoolid = int.Parse(schoolidstr);
            int gradeid = int.Parse(gradeidstr);
            int classid = int.Parse(classidstr);
            int begintime = int.Parse(DateTime.Parse(begintimestr).ToString("yyyyMM"));
            int endtime = int.Parse(DateTime.Parse(endtimestr).AddMonths(1).ToString("yyyyMM"));
            int pageindex = int.Parse(pageIndexstr);
            int pagesize = int.Parse(pageSizestr);

            List<datastruct> results = new List<datastruct>();
            var studentlist = DbHelper.Query<Report_Customer>(c => c.TheTime >= begintime
                && c.TheTime < endtime
                && new ReportCommen().GetChildManager().Contains(c.Manager.Id)
                );
            if (kefuid != -1)
            {
                studentlist = studentlist.Where(c => c.Manager.Id == kefuid);
                if (classid != -1)
                {
                    studentlist = studentlist.Where(c => c.School.Id == schoolid && c.Grade.Id == gradeid && c.Clazz.Id == classid);
                    studentlist.GroupBy(c => new { c.Clazz.Id, c.Clazz.Name }).Select(c => new { c.Key.Id, c.Key.Name }).ToList().ForEach(e =>
                    {
                        var ds = new datastruct()
                        {
                            areaid = e.Id,
                            areaname = e.Name,
                            coldata = new List<object>(),
                        };
                        var tt = studentlist.Where(c => c.Grade.Id == e.Id)
                            .OrderBy(c => c.Clazz.Id)
                            .ThenBy(c => c.TheTime)
                            .GroupBy(c => new { c.Clazz.Id, c.Clazz.Name, c.TheTime })
                            .Select(c => new { areaid = c.Key.Id, areaname = c.Key.Name, thetime = c.Key.TheTime, imoney = c.Sum(f => f.Money) })
                            .ToList();
                        tt.ForEach(f =>
                        {
                            ds.coldata.Add(f);
                            ds.total += (int)f.imoney;
                        });
                        results.Add(ds);
                    });
                }
                if (gradeid != -1)
                {
                    studentlist = studentlist.Where(c => c.School.Id == schoolid && c.Grade.Id == gradeid);
                    studentlist.GroupBy(c => new { c.Clazz.Id, c.Clazz.Name }).Select(c => new { c.Key.Id, c.Key.Name }).ToList().ForEach(e =>
                    {
                        var ds = new datastruct()
                        {
                            areaid = e.Id,
                            areaname = e.Name,
                            coldata = new List<object>(),
                        };
                        var tt = studentlist.Where(c => c.Clazz.Id == e.Id)
                            .OrderBy(c => c.Clazz.Name)
                            .ThenBy(c => c.TheTime)
                            .GroupBy(c => new { c.Clazz.Id, c.Clazz.Name, c.TheTime })
                            .Select(c => new { areaid = c.Key.Id, areaname = c.Key.Name, thetime = c.Key.TheTime, imoney = c.Sum(f => f.Money) })
                            .ToList();
                        tt.ForEach(f =>
                        {
                            ds.coldata.Add(f);
                            ds.total += (int)f.imoney;
                        });
                        results.Add(ds);
                    });
                }
                else if (schoolid != -1)
                {
                    studentlist = studentlist.Where(c => c.School.Id == schoolid);
                    studentlist.GroupBy(c => new { c.Grade.Id, c.Grade.Name }).Select(c => new { c.Key.Id, c.Key.Name }).ToList().ForEach(e =>
                    {
                        var ds = new datastruct()
                        {
                            areaid = e.Id,
                            areaname = e.Name,
                            coldata = new List<object>(),
                        };
                        var tt = studentlist.Where(c => c.Grade.Id == e.Id)
                            .OrderBy(c => c.Grade.Id)
                            .ThenBy(c => c.TheTime)
                            .GroupBy(c => new { c.Grade.Id, c.Grade.Name, c.TheTime })
                            .Select(c => new { areaid = c.Key.Id, areaname = c.Key.Name, thetime = c.Key.TheTime, imoney = c.Sum(f => f.Money) })
                            .ToList();
                        tt.ForEach(f =>
                        {
                            ds.coldata.Add(f);
                            ds.total += (int)f.imoney;
                        });
                        results.Add(ds);
                    });
                }
                else
                {
                    studentlist.GroupBy(c => new { c.School.Id, c.School.Name }).Select(c => new { c.Key.Id, c.Key.Name }).ToList().ForEach(e =>
                    {
                        var ds = new datastruct()
                        {
                            areaid = e.Id,
                            areaname = e.Name,
                            coldata = new List<object>(),
                        };
                        var tt = studentlist.Where(c => c.School.Id == e.Id)
                            .OrderBy(c => c.School.Id)
                            .ThenBy(c => c.TheTime)
                            .GroupBy(c => new { c.School.Id, c.School.Name, c.TheTime })
                            .Select(c => new { areaid = c.Key.Id, areaname = c.Key.Name, thetime = c.Key.TheTime, imoney = c.Sum(f => f.Money) })
                            .ToList();
                        tt.ForEach(f =>
                        {
                            ds.coldata.Add(f);
                            ds.total += (int)f.imoney;
                        });
                        results.Add(ds);
                    });
                }
            }
            else
            {
                studentlist.GroupBy(c => new { c.Manager.Id, c.Manager.Name }).Select(c => new { c.Key.Id, c.Key.Name }).ToList().ForEach(e =>
                {
                    var ds = new datastruct()
                    {
                        areaid = e.Id,
                        areaname = e.Name,
                        coldata = new List<object>(),
                    };
                    var tt = studentlist.Where(c => c.Manager.Id == e.Id)
                        .OrderBy(c => c.Manager.Id)
                        .ThenBy(c => c.TheTime)
                        .GroupBy(c => new { c.Manager.Id, c.Manager.Name, c.TheTime })
                        .Select(c => new { areaid = c.Key.Id, areaname = c.Key.Name, thetime = c.Key.TheTime, imoney = c.Sum(f => f.Money) })
                        .ToList();
                    tt.ForEach(f =>
                    {
                        ds.coldata.Add(f);
                        ds.total += (int)f.imoney;
                    });
                    results.Add(ds);
                });
            }

            return Json(results);
        }
        [HttpPost]
        public JsonResult getdataBySchoolDetail(string schoolidstr, string gradeidstr, string classidstr, string begintimestr, string endtimestr, string pageIndexstr, string pageSizestr)
        {
            int schoolid = int.Parse(schoolidstr);
            int gradeid = int.Parse(gradeidstr);
            int classid = int.Parse(classidstr);
            int begintime = int.Parse(DateTime.Parse(begintimestr).ToString("yyyyMM"));
            int endtime = int.Parse(DateTime.Parse(endtimestr).AddMonths(1).ToString("yyyyMM"));
            int pageindex = int.Parse(pageIndexstr);
            int pagesize = int.Parse(pageSizestr);

            List<datastruct> results = new List<datastruct>();
            var studentlist = DbHelper.Query<Report_Customer>(c => c.TheTime >= begintime
                && c.TheTime < endtime
                && new ReportCommen().GetChildManager().Contains(c.Manager.Id)
                );
            if (classid != -1)
            {
                studentlist = studentlist.Where(c => c.School.Id == schoolid && c.Grade.Id == gradeid && c.Clazz.Id == classid);
                studentlist.GroupBy(c => new { c.Clazz.Id, c.Clazz.Name }).Select(c => new { c.Key.Id, c.Key.Name }).ToList().ForEach(e =>
                {
                    var ds = new datastruct()
                    {
                        areaid = e.Id,
                        areaname = e.Name,
                        coldata = new List<object>(),
                    };
                    var tt = studentlist.Where(c => c.Grade.Id == e.Id)
                        .OrderBy(c => c.Clazz.Id)
                        .ThenBy(c => c.TheTime)
                        .GroupBy(c => new { c.Clazz.Id, c.Clazz.Name, c.TheTime })
                        .Select(c => new { areaid = c.Key.Id, areaname = c.Key.Name, thetime = c.Key.TheTime, imoney = c.Sum(f => f.Money) })
                        .ToList();
                    tt.ForEach(f =>
                    {
                        ds.coldata.Add(f);
                        ds.total += (int)f.imoney;
                    });
                    results.Add(ds);
                });
            }
            if (gradeid != -1)
            {
                studentlist = studentlist.Where(c => c.School.Id == schoolid && c.Grade.Id == gradeid);
                studentlist.GroupBy(c => new { c.Clazz.Id, c.Clazz.Name }).Select(c => new { c.Key.Id, c.Key.Name }).ToList().ForEach(e =>
                {
                    var ds = new datastruct()
                    {
                        areaid = e.Id,
                        areaname = e.Name,
                        coldata = new List<object>(),
                    };
                    var tt = studentlist.Where(c => c.Clazz.Id == e.Id)
                        .OrderBy(c => c.Clazz.Name)
                        .ThenBy(c => c.TheTime)
                        .GroupBy(c => new { c.Clazz.Id, c.Clazz.Name, c.TheTime })
                        .Select(c => new { areaid = c.Key.Id, areaname = c.Key.Name, thetime = c.Key.TheTime, imoney = c.Sum(f => f.Money) })
                        .ToList();
                    tt.ForEach(f =>
                    {
                        ds.coldata.Add(f);
                        ds.total += (int)f.imoney;
                    });
                    results.Add(ds);
                });
            }
            else if (schoolid != -1)
            {
                studentlist = studentlist.Where(c => c.School.Id == schoolid);
                studentlist.GroupBy(c => new { c.Grade.Id, c.Grade.Name }).Select(c => new { c.Key.Id, c.Key.Name }).ToList().ForEach(e =>
                {
                    var ds = new datastruct()
                    {
                        areaid = e.Id,
                        areaname = e.Name,
                        coldata = new List<object>(),
                    };
                    var tt = studentlist.Where(c => c.Grade.Id == e.Id)
                        .OrderBy(c => c.Grade.Id)
                        .ThenBy(c => c.TheTime)
                        .GroupBy(c => new { c.Grade.Id, c.Grade.Name, c.TheTime })
                        .Select(c => new { areaid = c.Key.Id, areaname = c.Key.Name, thetime = c.Key.TheTime, imoney = c.Sum(f => f.Money) })
                        .ToList();
                    tt.ForEach(f =>
                    {
                        ds.coldata.Add(f);
                        ds.total += (int)f.imoney;
                    });
                    results.Add(ds);
                });
            }
            else
            {
                studentlist.GroupBy(c => new { c.School.Id, c.School.Name }).Select(c => new { c.Key.Id, c.Key.Name }).ToList().ForEach(e =>
                {
                    var ds = new datastruct()
                    {
                        areaid = e.Id,
                        areaname = e.Name,
                        coldata = new List<object>(),
                    };
                    var tt = studentlist.Where(c => c.School.Id == e.Id)
                        .OrderBy(c => c.School.Id)
                        .ThenBy(c => c.TheTime)
                        .GroupBy(c => new { c.School.Id, c.School.Name, c.TheTime })
                        .Select(c => new { areaid = c.Key.Id, areaname = c.Key.Name, thetime = c.Key.TheTime, imoney = c.Sum(f => f.Money) })
                        .ToList();
                    tt.ForEach(f =>
                    {
                        ds.coldata.Add(f);
                        ds.total += (int)f.imoney;
                    });
                    results.Add(ds);
                });
            }

            return Json(results);
        }

    }



}
