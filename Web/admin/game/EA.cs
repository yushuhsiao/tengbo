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
        public class Member_002 : game.Member_XXX { }

        [game("Member_002", "MemberID", BU.GameID.EA, true)]
        public class MemberRowCommand_EA : game.UserGameRowCommand<web.MemberRow, Member_002, MemberRowCommand_EA> { }
    }
}