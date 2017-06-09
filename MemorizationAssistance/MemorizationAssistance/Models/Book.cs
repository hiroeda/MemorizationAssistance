using System;
using System.Collections.Generic;
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
        public string Name { get; set; }

        /// <summary>
        /// 問題データ
        /// </summary>
        public virtual List<QuestionData> QuestionDatas { get; set; }
    }
}