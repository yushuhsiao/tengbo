<%@ Page Language="C#" AutoEventWireup="true" Inherits="dinpay_aspx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>智付支付</title>
</head>
<body onload="document.DinpayAspVirForm.submit()">
    <form id="DinpayAspVirForm" name="DinpayAspVirForm" action="<%=reqURL_onLine ?? Request.Params["action_url"] %>" method="post">
        <input type="hidden" name="M_ID"        value="<%=M_ID ?? Request.Params["M_ID"]%>" />
        <input type="hidden" name="MOrderID"    value="<%=MOrderID ?? Request.Params["MOrderID"]%>" />
        <input type="hidden" name="MOAmount"    value="<%=MOAmount ?? Request.Params["MOAmount"]%>" />
        <input type="hidden" name="MOCurrency"  value="<%=MOCurrency ?? Request.Params["MOCurrency"]%>" />
        <input type="hidden" name="M_URL"       value="<%=M_URL ?? Request.Params["M_URL"]%>" />
        <input type="hidden" name="M_Language"  value="<%=M_Language ?? Request.Params["M_Language"]%>" />
        <input type="hidden" name="S_Name"      value="<%=S_Name ?? Request.Params["S_Name"]%>" />
        <input type="hidden" name="S_Address"   value="<%=S_Address ?? Request.Params["S_Address"]%>" />
        <input type="hidden" name="S_PostCode"  value="<%=S_PostCode ?? Request.Params["S_PostCode"]%>" />
        <input type="hidden" name="S_Telephone" value="<%=S_Telephone ?? Request.Params["S_Telephone"]%>" />
        <input type="hidden" name="S_Email"     value="<%=S_Email ?? Request.Params["S_Email"]%>" />
        <input type="hidden" name="R_Name"      value="<%=R_Name ?? Request.Params["R_Name"]%>" />
        <input type="hidden" name="R_Address"   value="<%=R_Address ?? Request.Params["R_Address"]%>" />
        <input type="hidden" name="R_PostCode"  value="<%=R_PostCode ?? Request.Params["R_PostCode"]%>" />
        <input type="hidden" name="R_Telephone" value="<%=R_Telephone ?? Request.Params["R_Telephone"]%>" />
        <input type="hidden" name="R_Email"     value="<%=R_Email ?? Request.Params["R_Email"]%>" />
        <input type="hidden" name="MOComment"   value="<%=MOComment ?? Request.Params["MOComment"]%>" />
        <input type="hidden" name="MODate"      value="<%=MODate ?? Request.Params["MODate"]%>" />
        <input type="hidden" name="State"       value="<%=State ?? Request.Params["State"]%>"/>
        <input type="hidden" name="digestinfo"  value="<%=digestinfo ?? Request.Params["digestinfo"]%>"/>
        <% if (!string.IsNullOrEmpty(action_url)){ %>
		<input type="hidden" name="action_url"  value="<%=action_url%>"/>
        <% } %>
    </form>
</body>
</html>
