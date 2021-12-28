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
        public class Member_003 : game.Member_XXX { }

        public abstract class MemberRowCommand_WFT<T> : game.UserGameRowCommand<web.MemberRow, Member_003, T> where T : MemberRowCommand_WFT<T>, new() { }

        [game("Member_003", "MemberID", BU.GameID.WFT, true)]
        public class MemberRowCommand_WFT : game.MemberRowCommand_WFT<MemberRowCommand_WFT> { }

        [game("Member_008", "MemberID", BU.GameID.WFT_SPORTS, true)]
        public class MemberRowCommand_WFT_SPORTS : game.MemberRowCommand_WFT<MemberRowCommand_WFT_SPORTS> { }
    }
}