<%@ Page Language="C#" AutoEventWireup="true" Inherits="System.Web.UI.Page" %>
<%@ Import Namespace="BU" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="web" %>

<!DOCTYPE html>
<script runat="server">

    protected void Page_Load(object sender, EventArgs e)
    {
        dinpay.config dinpay_config = dinpay.GetConfig(Member.Manager.DefaultCorpID);
        Member member = HttpContext.Current.User as Member;
        decimal? amount = Request.Form["n1"].ToDecimal();
        if (!amount.HasValue)
        {
            Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
            return;
        }
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
        {
            MemberRow memberRow = member.GetMemberRow(sqlcmd, true, "Name", "Tel", "Mail");
            MemberTranRow tranRow = new web.dinpay() { LogType = BU.LogType.Dinpay, MemberID = member.ID, Amount1 = amount, }.Insert(null, sqlcmd);

            DinpayAspVirForm.Action = dinpay_config.action;
            M_ID.Value = dinpay_config.M_ID;                                           //<----商家号-------->
            MODate.Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");        //<---不可以为空的--->
            string id_str = Convert.ToBase64String(tranRow.ID.Value.ToByteArray()); ;
            MOrderID.Value = id_str.Substring(0, id_str.Length - 2);
            //<----定单号-------->
            MOAmount.Value = tranRow.Amount1.ToString();                           //<----定单金额------> 
            MOCurrency.Value = "1";                                             //<----币种-默认为1---->
            M_URL.Value = dinpay_config.M_URL;                                         //<--返回地址-此项默认为空不起作用-->
            M_Language.Value = "1";
            S_Name.Value = "test1";
            S_Address.Value = "dinpay";
            S_PostCode.Value = "518000";
            S_Telephone.Value = "0755-88833166";
            S_Email.Value = "service@dinpay.com";
            R_Name.Value = "test2";
            R_Address.Value = "dinpay";
            R_PostCode.Value = "518000";
            R_Telephone.Value = "0755-82384511";
            R_Email.Value = "service@dinpay.com";
            MOComment.Value = "dinpay";
            State.Value = "0";

            string OrderMessage = M_ID.Value + MOrderID.Value + MOAmount.Value + MOCurrency.Value + M_URL.Value + M_Language.Value + S_PostCode.Value + S_Telephone.Value + S_Email.Value + R_PostCode.Value + R_Telephone.Value + R_Email.Value + MODate.Value + dinpay_config.Key;
            //Response.Write("串起来的订单信息为：" + OrderMessage + "<br>");
            string digest = FormsAuthentication.HashPasswordForStoringInConfigFile(OrderMessage, "md5").Trim().ToUpper();
            //Response.Write("加密认证为：" + digest);
            digestinfo.Value = digest;
        }
    }
</script>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="DinpayAspVirForm" name="DinpayAspVirForm" runat="server">
        <input runat="server" type="hidden" id="M_ID" name="M_ID" />
        <input runat="server" type="hidden" id="MOrderID" name="MOrderID" />
        <input runat="server" type="hidden" id="MOAmount" name="MOAmount" />
        <input runat="server" type="hidden" id="MOCurrency" name="MOCurrency" />
        <input runat="server" type="hidden" id="M_URL" name="M_URL" />
        <input runat="server" type="hidden" id="M_Language" name="M_Language" />
        <input runat="server" type="hidden" id="S_Name" name="S_Name" />
        <input runat="server" type="hidden" id="S_Address" name="S_Address" />
        <input runat="server" type="hidden" id="S_PostCode" name="S_PostCode" />
        <input runat="server" type="hidden" id="S_Telephone" name="S_Telephone" />
        <input runat="server" type="hidden" id="S_Email" name="S_Email" />
        <input runat="server" type="hidden" id="R_Name" name="R_Name" />
        <input runat="server" type="hidden" id="R_Address" name="R_Address" />
        <input runat="server" type="hidden" id="R_PostCode" name="R_PostCode" />
        <input runat="server" type="hidden" id="R_Telephone" name="R_Telephone" />
        <input runat="server" type="hidden" id="R_Email" name="R_Email" />
        <input runat="server" type="hidden" id="MOComment" name="MOComment" />
        <input runat="server" type="hidden" id="MODate" name="MODate" />
        <input runat="server" type="hidden" id="State" name="State" />
        <input runat="server" type="hidden" id="digestinfo" name="digestinfo" />
        <script type="text/javascript">document.DinpayAspVirForm.submit();</script>
    </form>
</body>
</html>
