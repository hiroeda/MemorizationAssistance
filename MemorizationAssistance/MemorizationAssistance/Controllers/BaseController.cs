using MemorizationAssistance.Models;
using System.Web.Mvc;

namespace MemorizationAssistance.Controllers
{
    /// <summary>
    /// コントローラの共通部分
    /// </summary>
    public class BaseController : Controller
    {
        protected MemorizationAssistanceContext db = new MemorizationAssistanceContext();

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