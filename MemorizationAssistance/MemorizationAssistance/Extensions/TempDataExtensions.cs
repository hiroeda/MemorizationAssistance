using MemorizationAssistance.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MemorizationAssistance.Extensions
{
    /// <summary>
    /// TempData の拡張機能を提供します。
    /// </summary>
    public static class TempDataExtensions
    {
        /// <summary>
        /// 通知メッセージを設定します。
        /// </summary>
        /// <param name="tempData">一時データ</param>
        /// <param name="message">通知メッセージ</param>
        public static void Notice(this TempDataDictionary tempData, string message)
        {
            tempData[Constants.TEMP_NOTICE] = message;
        }

        /// <summary>
        /// 通知メッセージを取得します。
        /// </summary>
        /// <param name="tempData">一時データ</param>
        /// <returns>通知メッセージ</returns>
        public static string Notice(this TempDataDictionary tempData)
        {
            return tempData[Constants.TEMP_NOTICE] as string;
        }

        /// <summary>
        /// アラートメッセージを設定します。
        /// </summary>
        /// <param name="tempData">一時データ</param>
        /// <param name="message">アラートメッセージ</param>
        public static void Alert(this TempDataDictionary tempData, string message)
        {
            tempData[Constants.TEMP_ALERT] = message;
        }

        /// <summary>
        /// アラートメッセージを取得します。
        /// </summary>
        /// <param name="tempData">一時データ</param>
        /// <returns>アラートメッセージ</returns>
        public static string Alert(this TempDataDictionary tempData)
        {
            return tempData[Constants.TEMP_ALERT] as string;
        }
    }
}