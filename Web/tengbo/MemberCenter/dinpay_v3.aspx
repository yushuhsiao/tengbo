<%@ Page Language="C#" AutoEventWireup="true" Inherits="dinpay_v3_aspx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>智付支付</title>
</head>
<body onload="document.DinpayAspVirForm.submit()">
    <form id="DinpayAspVirForm" name="DinpayAspVirForm" action="<%=reqURL_onLine      ?? Request.Params["action_url"] %>" method="post">
        <input type="hidden" name="signSrc"              value="<%=signSrc            ?? Request.Params["signSrc"] %>" />
        <input type="hidden" name="merchant_code"        value="<%=merchant_code      ?? Request.Params["merchant_code"] %>" />
        <input type="hidden" name="bank_code"            value="<%=bank_code          ?? Request.Params["bank_code"] %>" />
        <input type="hidden" name="order_no"             value="<%=order_no           ?? Request.Params["order_no"] %>" />
        <input type="hidden" name="order_amount"         value="<%=order_amount       ?? Request.Params["order_amount"] %>" />
        <input type="hidden" name="service_type"         value="<%=service_type       ?? Request.Params["service_type"] %>" />
        <input type="hidden" name="input_charset"        value="<%=input_charset      ?? Request.Params["input_charset"] %>" />
        <input type="hidden" name="notify_url"           value="<%=notify_url         ?? Request.Params["notify_url"] %>" />
        <input type="hidden" name="interface_version"    value="<%=interface_version  ?? Request.Params["interface_version"] %>" />
        <input type="hidden" name="sign_type"            value="<%=sign_type          ?? Request.Params["sign_type"] %>" />
        <input type="hidden" name="order_time"           value="<%=order_time         ?? Request.Params["order_time"] %>" />
        <input type="hidden" name="product_name"         value="<%=product_name       ?? Request.Params["product_name"] %>" />
        <input type="hidden" name="client_ip"            value="<%=client_ip          ?? Request.Params["client_ip"] %>" />
        <input type="hidden" name="extend_param"         value="<%=extend_param       ?? Request.Params["extend_param"] %>" />
        <input type="hidden" name="extra_return_param"   value="<%=extra_return_param ?? Request.Params["extra_return_param"] %>" />
        <input type="hidden" name="product_code"         value="<%=product_code       ?? Request.Params["product_code"] %>" />
        <input type="hidden" name="product_desc"         value="<%=product_desc       ?? Request.Params["product_desc"] %>" />
        <input type="hidden" name="product_num"          value="<%=product_num        ?? Request.Params["product_num"] %>" />
        <input type="hidden" name="return_url"           value="<%=return_url         ?? Request.Params["return_url"] %>" />
        <input type="hidden" name="show_url"             value="<%=show_url           ?? Request.Params["show_url"] %>" />
        <% if (!string.IsNullOrEmpty(action_url)){ %>
		<input type="hidden" name="action_url"  value="<%=action_url%>"/>
        <% } %>
    </form>
</body>
</html>
