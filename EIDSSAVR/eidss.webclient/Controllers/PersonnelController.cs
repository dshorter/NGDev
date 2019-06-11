﻿using System.Web.Mvc;
using eidss.model.Schema;
using eidss.webclient.Utils;

namespace eidss.webclient.Controllers
{
    [AuthorizeEIDSS]
    public class PersonnelController : Controller
    {
        //
        // GET: /Personnel/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SelectPerson(VetCase vcase, string idfsOfficePropertyName, string strOfficePropertyName,
            string idfsPersonPropertyName, string strPersonPropertyName, bool showEditPersonButton = false, int HACode = 0)
        {
            ViewBag.IdfsOfficePropertyName = idfsOfficePropertyName;
            ViewBag.StrOfficePropertyName = strOfficePropertyName;
            ViewBag.IdfsPersonPropertyName = idfsPersonPropertyName;
            ViewBag.StrPersonPropertyName = strPersonPropertyName;
            ViewBag.ShowEditPersonButton = showEditPersonButton;
            ViewBag.HACode = HACode;
            return PartialView(vcase);
        }
    }
}
