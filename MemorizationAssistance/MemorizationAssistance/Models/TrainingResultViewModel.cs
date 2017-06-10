using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MemorizationAssistance.Models
{
    /// <summary>
    /// 練習結果一覧
    /// </summary>
    public class TrainingResultViewModel
    {
        /// <summary>
        /// 解答状況
        /// </summary>
        public enum AnswerStatus
        {
            /// <summary>
            /// 未解答
            /// </summary>
            Unanswered,

            /// <summary>
            /// 解答済み
            /// </summary>
            Answered,
        }

        public TrainingResultViewModel(Book book)
        {
            BookId = book.Id;
            CurrentAnswerStatus = AnswerStatus.Unanswered;
            TrainingResultItems = book.QuestionDatas
                .Select(q => new TrainingResultItem()
                {
                    QuestionDataId = q.Id,
                    Order = q.Order,
                    CorrectCount = 0,
                    AllCount = 0,
                })
                .OrderBy(t => t.Order)
                .ToList();
        }

        /// <summary>
        /// 問題集ID
        /// </summary>
        public int BookId { get; set; }

        /// <summary>
        /// 現在の解答状況
        /// </summary>
        public AnswerStatus CurrentAnswerStatus { get; set; }

        /// <summary>
        /// 解答履歴
        /// </summary>
        public List<TrainingResultItem> TrainingResultItems { get; set; }

        /// <summary>
        /// 正解だったことをカウントする
        /// </summary>
        /// <param name="id"></param>
        public void AddCorrectCount(int id)
        {
            var resultItem = TrainingResultItems.Where(r => r.QuestionDataId == id).Single();
            resultItem.AllCount++;
            resultItem.CorrectCount++;
        }

        /// <summary>
        /// 不正解だったことをカウントする
        /// </summary>
        /// <param name="id"></param>
        public void AddIncorrectCount(int id)
        {
            var resultItem = TrainingResultItems.Where(r => r.QuestionDataId == id).Single();
            resultItem.AllCount++;
        }
    }

    /// <summary>
    /// 練習結果データ
    /// </summary>
    public class TrainingResultItem
    {
        /// <summary>
        /// 問題ID
        /// </summary>
        public int QuestionDataId { get; set; }

        /// <summary>
        /// 表示順( = 問題番号)
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 正解数
        /// </summary>
        public int CorrectCount { get; set; }

        /// <summary>
        /// 取り組み数
        /// </summary>
        public int AllCount { get; set; }

        /// <summary>
        /// 正答率
        /// </summary>
        [DisplayName("正答率")]
        public decimal? CorrectRate
        {
            get
            {
                if (AllCount == 0) return null;

                return (decimal)CorrectCount / AllCount * 100;
            }
        }

        public override string ToString()
        {
            var rate = "---";
            if (CorrectRate.HasValue)
            {
                rate = string.Format("{0}%", CorrectRate.Value);
            }
            return string.Format("問題{0}:{1}/{2} = {3}", Order, CorrectCount, AllCount, rate);
        }
    }
}