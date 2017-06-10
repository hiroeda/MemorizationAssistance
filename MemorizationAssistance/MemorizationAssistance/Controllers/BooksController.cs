using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MemorizationAssistance.Models;

namespace MemorizationAssistance.Controllers
{
    /// <summary>
    /// 問題集管理コントローラ
    /// </summary>
    public class BooksController : Controller
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

        /// <summary>
        /// 詳細画面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        /// <summary>
        /// 追加画面(初回表示)
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// 追加画面(処理実施)
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,QuestionDataCsv")] BookEditViewModel book)
        {
            if (ModelState.IsValid)
            {
                var model = new Book();
                UpdateModel(model);
                model.SetQuestionDatas(book.QuestionDataCsv);
                db.Books.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(book);
        }

        /// <summary>
        /// 編集画面(初回表示)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Include(b => b.QuestionDatas).SingleOrDefault(b => b.Id == id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book.ToEditViewModel());
        }

        /// <summary>
        /// 編集画面(処理実施)
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,QuestionDataCsv")] BookEditViewModel book)
        {
            if (ModelState.IsValid)
            {
                using (var tx = db.Database.BeginTransaction())
                {
                    var model = db.Books.Find(book.Id);

                    // 現状の問題文を削除
                    for (int i = model.QuestionDatas.Count - 1; i >= 0; i--)
                    {
                        db.QuestionDatas.Remove(model.QuestionDatas[i]);
                    }
                    db.SaveChanges();

                    // 新しい設定でDB更新
                    UpdateModel(model);
                    model.SetQuestionDatas(book.QuestionDataCsv);
                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();

                    tx.Commit();
                    return RedirectToAction("Index");
                }
            }
            return View(book);
        }

        /// <summary>
        /// 削除画面(初回表示)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        /// <summary>
        /// 削除画面(処理実施)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = db.Books.Find(id);
            db.Books.Remove(book);
            db.SaveChanges();
            return RedirectToAction("Index");
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
