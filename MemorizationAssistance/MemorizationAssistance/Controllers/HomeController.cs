using System.Linq;
using System.Web.Mvc;

namespace MemorizationAssistance.Controllers
{
    /// <summary>
    /// 問題集一覧画面コントローラ
    /// </summary>
    public class HomeController : BaseController
    {
        /// <summary>
        /// 一覧画面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View(db.Books.ToList());
        }
    }
}