using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using Newtonsoft.Json;
using BU;
using System.Reflection;
using System.Runtime.CompilerServices;


namespace web
{
    static partial class _ExecuteCommand
    {

        //static bool Execute<TRow>(this SqlCmd sqlcmd, string sqlstr, out TRow row, out jgrid.RowResponse res) where TRow : new()
        //{
        //    try
        //    {
        //        sqlcmd.BeginTransaction();
        //        row = sqlcmd.ToObject<TRow>(sqlstr);
        //        RowException.TestNull(row);
        //        sqlcmd.Commit();
        //        res = null;
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        sqlcmd.Rollback();
        //        log.message("error", ex.Message);
        //        row = default(TRow);
        //        res = jgrid.RowResponse.Error(ex);
        //        return false;
        //    }
        //}

        //// args 任一值都不可以為 null
        //static bool GetSingleRow<TRow>(this SqlCmd sqlcmd, out TRow row, string format, params object[] keys) where TRow : new()
        //{
        //    for (int i = 0; i < keys.Length; i++)
        //    {
        //        if (keys[i] == null)
        //        {
        //            row = default(TRow);
        //            return false;
        //        }
        //    }
        //    row = sqlcmd.ToObject<TRow>(format, keys);
        //    return row != null;
        //}
    }



    #region data
    namespace data
    {
    }
    #endregion
    #region protocol
    namespace protocol
    {
    }
    #endregion
    partial class _ExecuteCommand
    {
    }



    #region data
    namespace data
    {
    }
    #endregion
    #region protocol
    namespace protocol
    {
    }
    #endregion
    partial class _ExecuteCommand
    {
    }



    #region data
    namespace data
    {
    }
    #endregion
    #region protocol
    namespace protocol
    {
    }
    #endregion
    partial class _ExecuteCommand
    {
    }
}