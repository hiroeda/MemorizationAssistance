using CsvHelper.Configuration;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MemorizationAssistance.Models
{
    /// <summary>
    /// 問題データ
    /// </summary>
    public class QuestionData
    {
        public int Id { get; set; }

        /// <summary>
        /// 表示順
        /// </summary>
        [DisplayName("表示順")]
        public int Order { get; set; }

        /// <summary>
        /// 問題文
        /// </summary>
        [DisplayName("問題文")]
        [Required]
        public string Question { get; set; }

        /// <summary>
        /// 問題文(練習用)
        /// </summary>
        [DisplayName("問題文")]
        [NotMapped]
        public string QuestionForTraining
        {
            get
            {
                return string.Format("問題{0}) {1}", Order, Question);
            }
        }

        /// <summary>
        /// 解答
        /// </summary>
        [DisplayName("解答")]
        [Required]
        public string Answer { get; set; }

        /// <summary>
        /// 問題集ID
        /// </summary>
        public int BookId { get; set; }

        /// <summary>
        /// 問題集
        /// </summary>
        public Book Book { get; set; }
    }

    /// <summary>
    /// CSVデータとQuestionDataオブジェクトとのマッピング
    /// </summary>
    class QuestionDataCsvMap : CsvClassMap<QuestionData>
    {
        public QuestionDataCsvMap()
        {
            Map(m => m.Question).Index(0);
            Map(m => m.Answer).Index(1);
            Map(m => m.Order).ConvertUsing(r => r.Row);
        }
    }

}