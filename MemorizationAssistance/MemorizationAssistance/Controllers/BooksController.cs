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
    public class BooksController : Controller
    {
        private MemorizationAssistanceContext db = new MemorizationAssistanceContext();

        // GET: Books
        public ActionResult Index()
        {
            return View(db.Books.ToList());
        }

        // GET: Books/Details/5
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

        // GET: Books/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
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

        // GET: Books/Edit/5
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

        // POST: Books/Edit/5
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
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

        // GET: Books/Delete/5
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

        // POST: Books/Delete/5
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
