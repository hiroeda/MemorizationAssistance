using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MemorizationAssistance.Common
{
    public static class Constants
    {
        #region セッション
        
        /// <summary>
        /// 解答結果を保存するセッションキー
        /// </summary>
        public const string SESSION_TEST_RESULT = "TEST_RESULT";

        #endregion

        #region TempData

        /// <summary>
        /// 通常の通知で表示するTempDataのキー
        /// </summary>
        public const string TEMP_NOTICE = "NOTICE";

        /// <summary>
        /// アラートの通知で表示するTempDataのキー
        /// </summary>
        public const string TEMP_ALERT = "ALERT";

        #endregion
    }

}