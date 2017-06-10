using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

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
}