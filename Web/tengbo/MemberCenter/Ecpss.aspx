<%@ Page Language="C#" AutoEventWireup="true" Inherits="ecpss_aspx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>汇潮支付</title>
</head>
<body onload="document.yeepay.submit()">
    
    <form name="yeepay" action="<%=reqURL_onLine ?? Request.Form["authorizationURL"] %>" method="post">
		<input type="hidden" name="OrderDesc"		           value="<%=OrderDesc ?? Request.Form["OrderDesc"]%>" />
		<input type="hidden" name="Remark"				       value="<%=Remark ?? Request.Form["Remark"]%>" />
		<input type="hidden" name="AdviceURL"				   value="<%=AdviceURL ?? Request.Form["AdviceURL"]%>" />
		<input type="hidden" name="ReturnURL"				   value="<%=ReturnURL ?? Request.Form["ReturnURL"]%>" />
		<input type="hidden" name="BillNo"					   value="<%=BillNo ?? Request.Form["BillNo"]%>" />
		<input type="hidden" name="MerNo"					   value="<%=MerNo ?? Request.Form["MerNo"]%>" />
		<input type="hidden" name="Amount"				       value="<%=Amount ?? Request.Form["Amount"]%>" />
		<input type="hidden" name="SignInfo"				   value="<%=SignInfo ?? Request.Form["SignInfo"]%>" />
		<input type="hidden" name="defaultBankNumber"		   value="<%=defaultBankNumber ?? Request.Form["defaultBankNumber"]%>" />
		<input type="hidden" name="orderTime"				   value="<%=orderTime ?? Request.Form["orderTime"]%>" />
		<input type="hidden" name="products"		           value="<%=products ?? Request.Form["products"]%>" />
        <% if (!string.IsNullOrEmpty(authorizationURL)) { %>   
		<input type="hidden" name="authorizationURL"		   value="<%=authorizationURL%>" />
        <% } %>
	</form>
</body>
</html>
