using MemorizationAssistance.Common;
using MemorizationAssistance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace MemorizationAssistance.Controllers
{
    /// <summary>
    /// 暗記対策コントローラ
    /// </summary>
    public class TrainingController : Controller
    {
        private MemorizationAssistanceContext db = new MemorizationAssistanceContext();

        /// <summary>
        /// 出題画面。Homeから問題集ID指定で呼び出される。
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="currentQuestionNumber"></param>
        /// <returns></returns>
        public ActionResult Index(int? bookId, int? currentQuestionNumber)
        {
            // 問題の取得
            if (bookId == null)
            {
                // Homeから問題集を指定して呼び出される前提
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var book = db.Books.Find(bookId);
            if (book == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (currentQuestionNumber == null)
            {
                // 解答中の問題の指定がない場合は問題集を開いたばかりなので、
                // 解答結果をクリアする。
                Session[Constants.TEST_RESULT] = new TrainingResultViewModel(book);
                currentQuestionNumber = 1;
            }
            var questionData = db.QuestionDatas
                .Where(q => q.BookId == bookId)
                .Where(q => q.Order == currentQuestionNumber)
                .SingleOrDefault();
            if (questionData == null)
            {
                return HttpNotFound();
            }

            var testResult = Session[Constants.TEST_RESULT] as TrainingResultViewModel;
            testResult.CurrentAnswerStatus = TrainingResultViewModel.AnswerStatus.Unanswered;

            return View(questionData);
        }

        /// <summary>
        /// 解答確認
        /// </summary>
        /// <param name="questionData"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(QuestionData questionData)
        {
            // セッションから解答状況を取り出し
            var testResult = Session[Constants.TEST_RESULT] as TrainingResultViewModel;
            if (testResult == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // 正解の取り出し
            var correctAnswer = db.QuestionDatas.Find(questionData.Id);

            // 解答状況に応じて処理を切り分け
            switch (testResult.CurrentAnswerStatus)
            {
                case TrainingResultViewModel.AnswerStatus.Unanswered:
                    // 未解答から解答状態に変更
                    testResult.CurrentAnswerStatus = TrainingResultViewModel.AnswerStatus.Answered;
                    if (correctAnswer.Answer == questionData.Answer)
                    {
                        // 正解をカウント
                        testResult.AddCorrectCount(questionData.Id);
                        // 次の問題に移行
                        return RedirectToAction("Index", new {
                            bookId = testResult.BookId,
                            currentQuestionNumber = questionData.Order < testResult.TrainingResultItems.Count ? questionData.Order + 1 : 1
                        });
                    }
                    else
                    {
                        // 不正解をカウント
                        testResult.AddIncorrectCount(questionData.Id);
                    }
                    break;
                case TrainingResultViewModel.AnswerStatus.Answered:
                    // すでに解答済みの場合
                    if (correctAnswer.Answer == questionData.Answer)
                    {
                        // 次の問題に移行
                        return RedirectToAction("Index", new
                        {
                            bookId = testResult.BookId,
                            currentQuestionNumber = questionData.Order < testResult.TrainingResultItems.Count ? questionData.Order + 1 : 1
                        });
                    }
                    else
                    {
                        // まだ間違っていることを通知
                    }
                    break;
            }
            return View(questionData);
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