using BU;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using Tools;

namespace web
{
    partial class game
    {
        public class Member_004 : game.Member_XXX { }

        public abstract class MemberRowCommand_KENO<T> : game.UserGameRowCommand<web.MemberRow, Member_004, T> where T : MemberRowCommand_KENO<T>, new() { }

        [game("Member_004", "MemberID", BU.GameID.KENO, true)]
        public class MemberRowCommand_KENO : game.MemberRowCommand_KENO<MemberRowCommand_KENO> { }

        [game("Member_007", "MemberID", BU.GameID.KENO_SSC, true)]
        public class MemberRowCommand_KENO_SSC : game.MemberRowCommand_KENO<MemberRowCommand_KENO_SSC> { }
    }
}