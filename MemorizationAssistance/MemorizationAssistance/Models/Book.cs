using CsvHelper;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

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

            using (var stringReader = new StringReader(questionDataCsv))
            using (var csvParser = new CsvParser(stringReader))
            {
                csvParser.Configuration.HasHeaderRecord = false;
                csvParser.Configuration.RegisterClassMap<QuestionDataCsvMap>();

                using (var csvReader = new CsvReader(csvParser))
                {
                    this.QuestionDatas = csvReader.GetRecords<QuestionData>().ToList();
                }
            }
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