using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MemorizationAssistance.Models
{
    /// <summary>
    /// 問題集
    /// </summary>
    public class Book
    {
        public int Id { get; set; }

        /// <summary>
        /// 問題集の名称
        /// </summary>
        [DisplayName("名称")]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 問題データ
        /// </summary>
        [DisplayName("問題データ")]
        public virtual List<QuestionData> QuestionDatas { get; set; }

        /// <summary>
        /// 渡されたCSVをQuestionDataに変換して格納する。
        /// CSVが渡ってこなかった場合は何もしない。
        /// </summary>
        /// <param name="questionDataCsv"></param>
        public void SetQuestionDatas(string questionDataCsv)
        {
            if (questionDataCsv == null) return;

            // ※ エラー処理省略
            this.QuestionDatas = questionDataCsv
                .Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
                .Select((l, index) => {
                    var items = l.Split(',');
                    return new QuestionData()
                    {
                        Order = index + 1,
                        Question = items[0],
                        Answer = items[1]
                    };
                })
                .ToList();
        }

        /// <summary>
        /// BookEditViewModelに変換する
        /// </summary>
        /// <returns></returns>
        public BookEditViewModel ToEditViewModel()
        {
            return new BookEditViewModel()
            {
                Id = this.Id,
                Name = this.Name,
                QuestionDataCsv = string.Join("\r\n",
                    this.QuestionDatas.Select(q => string.Format("{0},{1}", q.Question, q.Answer)))
            };
        }
    }

    /// <summary>
    /// 問題集追加・編集用ViewModel
    /// </summary>
    public class BookEditViewModel
    {
        public int Id { get; set; }

        /// <summary>
        /// 問題集の名称
        /// </summary>
        [DisplayName("名称")]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 問題データ
        /// </summary>
        [DisplayName("問題データ")]
        public string QuestionDataCsv { get; set; }
    }
}