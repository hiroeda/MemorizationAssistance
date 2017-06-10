using MemorizationAssistance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MemorizationAssistance.Controllers
{
    /// <summary>
    /// ホーム画面コントローラ
    /// </summary>
    public class HomeController : Controller
    {
        private MemorizationAssistanceContext db = new MemorizationAssistanceContext();

        /// <summary>
        /// 一覧画面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View(db.Books.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}