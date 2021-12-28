<%@ Control Language="C#" AutoEventWireup="true" Inherits="SiteControl" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="Newtonsoft.Json" %>

<script runat="server">
    web.CashChannelRow channel;
    string hasDinPay = "关闭";
    string hasYeePay = "关闭";
    string hasEcpss = "关闭";
    
    protected void Page_Load(object sender, EventArgs e)
    {
        //设置默认支付方式
        this.ptype = this.ptype ?? this.ThirdpaytDefault;
        foreach (web.CashChannelRow c in web.CashChannelRow.Cache.Instance.Rows)
        {
            if (c.CorpID != _global_asax.DefaultCorpID) continue;
            
            if (c is web.DinpayRowData) 
                { hasDinPay = "开启"; if (this.ptype == "dinpay") channel = c; }
            else if (c is web.YeepayRowData) 
                { hasYeePay = "开启"; if (this.ptype == "yeepay") channel = c; }
            else if (c is web.EcpssRowData) 
                { hasEcpss = "开启"; if (this.ptype == "ecpss") channel = c; }
        }
                
        //this.ptype = this.ptype == null ? "dinpay" : this.ptype;
        //for (; ; )
        //{   
        //    channel = web.CashChannelRow.Cache.Instance.RandomGetRow();
        //    if (channel.CorpID != _global_asax.DefaultCorpID) continue;
        //    if ((channel == null) ||
        //        ((channel is web.DinpayRowData) && this.ptype == "dinpay") ||
        //        ((channel is web.YeepayRowData) && this.ptype == "yeepay") ||
        //        ((channel is web.EcpssRowData) && this.ptype == "ecpss"))
        //        break;
        //}
        //channel = new web.YeepayRowData() { ID = Guid.NewGuid() };
    }
    
    [JsonProperty("ptype")]
    string ptype;
</script>

<div class="user_center_content" index="05_1" reload="true" style="display: <%=this.css_display%>;">
    <%--<script type="text/javascript">
        function <%=randID("f1")%>_() {
            var $n1 = $('#<%=randID("table_id")%> input[name="n1"]');
            var value = parseFloat($n1.val_trim());
            if (isNaN(value)) {
                $('#<%=randID("table_id")%> .user_errmsg').text('请输入存款金额');
            }
            else {<% /*if (web.dinpay.CurrentConfig.Enabled)*/
        for (; ; )
        {
            channel = web.CashChannelRow.Cache.Instance.RandomGetRow();
            if ((channel == null) ||
                (channel is web.DinpayRowData) ||
                (channel is web.YeepayRowData))
                break;
        }
        channel = new web.YeepayRowData() { ID = Guid.NewGuid() };
        if (channel != null) { %> <%=randID("form_id")%>_.submit(); <% } %>
            } $n1.val('');
        }

    </script>--%>
    <style style="text/css">
        .ptype {
            background-image: url(../image/notselected.png); height: 41px;width: 125px; margin-right: 19px; float: left; cursor: pointer;
        }
        .ptype:hover {
            background-image: url(../image/selected.png);
        }
        .ptype_selected {
            background-image: url(../image/selected.png);
            cursor: default;
        }
        .ptype_title {
            width: 80px;
            line-height: 33px;
            height: 33px;
            margin: 0px;
            font-size: 14px;
        }
        .ptype_status {
            background-color: #ffffff;
            color: #000000;
            width: 30px; line-height: 20px; height: 18px; margin: 7.5px;
        }
        .ptype_title, .ptype_status {
            float: left;
            white-space: nowrap;
            text-align: center;
        }
        .ptype_status {
            /*border-bottom-right-radius: 4px*//*{cornerRadius};*/
            border-bottom-left-radius: 4px/*{cornerRadius}*/;
            border-top-right-radius: 4px/*{cornerRadius}*/;
            border-top-left-radius: 4px/*{cornerRadius}*/;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            $(".money_tabs .ptype[rel='<%=this.ptype%>']").addClass("ptype_selected");

            $(".money_tabs .ptype").click(function () {
                if ($(this).attr("rel") == "<%=this.ptype%>" || $(this).find(".ptype_status").text() == "关闭")
                    return;
                $(this).siblings("div").removeClass("ptype_selected");
                $(this).addClass("ptype_selected");
                $("#hid_Ptype").val($(this).attr("rel"));
                $.nav('05').navshow('05_1', $(".money_tabs").getPostData());
            });
        });
    </script>
    <div class="item_title">在线充值</div>
    <div>提示！如当前支付方式未能支付成功，请您尝试其他支付方式进行支付。农行K码支付请选择支付方式二！</div>
    <br />
    <div class="money_tabs" style="margin: 0px;">
        <div class="ptype" rel="ecpss">
            <div class="ptype_title">支付方式一</div>
            <div class="ptype_status"><%=hasEcpss%></div>
        </div>
        <div class="ptype" rel="dinpay">
            <div class="ptype_title">支付方式二</div>
            <div class="ptype_status"><%=hasDinPay%></div>
        </div>
         <div class="ptype" rel="yeepay">
            <div class="ptype_title">支付方式三</div>
            <div class="ptype_status"><%=hasYeePay%></div>
        </div>
        <input type="hidden" name="ptype" id="hid_Ptype" />
    </div>
    <br />
    <br />
    <br />
    <br />
    <div>
        <table id="<%=randID("table_id")%>" class="user_table" border="0" style="width:100%">
            <tr>
                <td>
                    <% if (channel is web.DinpayRowData) { %>
                    <table>
                        <tr><td class="title" style="text-align: right; width: 102px;">存款金额：  </td>
                            <td class="value thirdpay" style="text-align: left;">
                                <form id="<%=randID("form_id")%>_" action="dinpay.aspx" method="post" target="_blank">
                                    <input name="n0" type="hidden" value="<%=channel.ID%>" />
                                    <input name="n1" type="text" class="thirdpay"/>
                                </form>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>&nbsp;<input type="button" class="info_button" value="提交" onclick="<%=randID("f1")%>_()"<% if (channel==null) { %> disabled="disabled" <% } %> /></td>
                        </tr>
                    </table>
                    <script type="text/javascript">
                        function <%=randID("f1")%>_() {
                            var $n1 = $('#<%=randID("table_id")%> input[name="n1"]');
                            var value = parseFloat($n1.val_trim());
                            if (isNaN(value)) {
                                $('#<%=randID("table_id")%> .user_errmsg').text('请输入存款金额');
                            }
                            else {
                                <%=randID("form_id")%>_.submit();
                            } $n1.val('');
                        }
                    </script>
                    <% } else if (channel is web.YeepayRowData) { %>
                    <form id="<%=randID("form_id")%>_" action="yeepay.aspx" method="post" target="_blank">
                        <table style="width:100%;">
                            <tr>
                                <td colspan="4" style="text-align:left; padding-left: 10px; padding-bottom: 15px;">
                                    <% int n = 1;
                                        foreach (KeyValuePair<string, string> p in yeepay_aspx.banklist) { %>
                                    <input type="radio" name="n3" id="bk<%=n%>" value="<%=p.Key%>" style=""/><label style="width: 110px; height: 30px; display: inline-block; text-align: left;" for="bk<%=n%>"><%=p.Value%></label>
                                    <% n++; if ((n % 4) == 1) { %><br /><% } } %>
                                </td>
                            </tr>
                            <tr>
                                <td class="title" style="text-align: right; width: 150px; color: yellow; font-size: 1.1em; font-weight: bold;">存款金额：  </td>
                                <td class="value thirdpay" style="text-align: left;">
                                    <input name="n0" type="hidden" value="<%=channel.ID%>" />
                                    <input name="n1" type="text" class="thirdpay" />
                                </td>
                                <td style="width: 72px;">
                                    <input type="button" class="info_button" value="提交" onclick="<%=randID("f1")%>_()"<% if (channel==null) { %> disabled="disabled" <% } %> />
                                </td>
                                <td class="user_errmsg" style="color: red;"></td>
                            </tr>
                        </table>
                    </form>
                    <script type="text/javascript">
                        function <%=randID("f1")%>_() {
                            var $n1 = $('#<%=randID("table_id")%> input[name="n1"]');
                            var $n3 = $('#<%=randID("table_id")%> input:checked[name="n3"]');
                            var value = parseFloat($n1.val_trim());
                            if (isNaN(value)) {
                                $('#<%=randID("table_id")%> .user_errmsg').text('请输入存款金额');
                            }
                            else if ($n3.val() == undefined) {
                                $('#<%=randID("table_id")%> .user_errmsg').text('请选择银行');
                            }
                            else {
                                <%=randID("form_id")%>_.submit();
                            } $n1.val('');
                    }
                    </script>
                    <% } else if (channel is web.EcpssRowData) { %>
                        <form id="<%=randID("form_id")%>_" action="ecpss.aspx" method="post" target="_blank">
                        <table style="width:100%;">
                            <tr>
                                <td colspan="4" style="text-align:left; padding-left: 10px; padding-bottom: 15px;">
                                    <% int n = 1;
                                        foreach (KeyValuePair<string, string> p in ecpss_aspx.banklist) { %>
                                    <input type="radio" name="n3" id="bk<%=n%>" value="<%=p.Key%>" style=""/><label style="width: 110px; height: 30px; display: inline-block; text-align: left;" for="bk<%=n%>"><%=p.Value%></label>
                                    <% n++; if ((n % 4) == 1) { %><br /><% } } %>
                                </td>
                            </tr>
                            <tr>
                                <td class="title" style="text-align: right; width: 150px; color: yellow; font-size: 1.1em; font-weight: bold;">存款金额：  </td>
                                <td class="value thirdpay" style="text-align: left;">
                                    <input name="n0" type="hidden" value="<%=channel.ID%>" />
                                    <input name="n1" type="text" class="thirdpay" />
                                </td>
                                <td style="width: 72px;">
                                    <input type="button" class="info_button" value="提交" onclick="<%=randID("f1")%>_()"<% if (channel==null) { %> disabled="disabled" <% } %> />
                                </td>
                                <td class="user_errmsg" style="color: red;"></td>
                            </tr>
                        </table>
                    </form>
                    <script type="text/javascript">
                        function <%=randID("f1")%>_() {
                            var $n1 = $('#<%=randID("table_id")%> input[name="n1"]');
                            var $n3 = $('#<%=randID("table_id")%> input:checked[name="n3"]');
                            var value = parseFloat($n1.val_trim());
                            if (isNaN(value)) {
                                $('#<%=randID("table_id")%> .user_errmsg').text('请输入存款金额');
                            }
                            else if ($n3.val() == undefined) {
                                $('#<%=randID("table_id")%> .user_errmsg').text('请选择银行');
                            }
                            else {
                                <%=randID("form_id")%>_.submit();
                            } $n1.val('');
                    }
                    </script>
                    <% } else { %>
                    <span style="color:red;"><%--在线支付维护中，暂时无法使用请选择绿色通道支付！--%>当前支付方式已关闭，请尝试使用其他支付方式进行支付！</span>
                    <% } %>
                </td>
            </tr>
            <tr>
                <td style="padding-top: 10px;">
                    <div class="prompt">在线支付提示：
                        <ol>
                            <li>上面所列银行为“在线支付”所支持的银行，请点击圆圈，选择您的支付银行。</li>
                            <li>在 “存款金额”   栏目中填写您要支付的金额， 并点击“提交”按钮。</li>
                        </ol>
                    </div>
                </td>
            </tr>
        </table>
    </div>

<%--    <table id="<%=randID("table_id")%>" class="user_table" border="0" style="width: 100%;">
        <tr>
            <td colspan="3">
            </td>
        </tr>
        <tr><td class="title" style="text-align: right; width: 102px;">存款金额：  </td>
            <td class="value thirdpay" style="text-align: left;">
                <% if (channel is web.DinpayRowData) { %>
                <form id="<%=randID("form_id")%>_" action="dinpay.aspx" method="post" target="_blank">
                    <input name="n0" type="hidden" value="<%=channel.ID%>" />
                    <input name="n1" type="text" class="thirdpay"/>
                </form>
                <% } else if (channel is web.YeepayRowData) { %>
                <form id="<%=randID("form_id")%>_" action="yeepay.aspx" method="post" target="_blank">
                    <input name="n0" type="hidden" value="<%=channel.ID%>" />
                    <input name="n1" type="text" class="thirdpay" />
                </form>
                <% } else { %>
                <input type="text" disabled="disabled" class="thirdpay"/>
                <% } %>
            </td>
            <td>
                <input type="button" class="info_button" value="提交" onclick="<%=randID("f1")%>_()"<% if (channel==null) { %> disabled="disabled" <% } %> />
            </td>
        </tr>
                <% if (channel is web.DinpayRowData) { %>
        <tr><td class="title" style="text-align: right; width: 102px;">存款金额：  </td>
            <td class="value thirdpay" style="text-align: left;">
                <form id="<%=randID("form_id")%>_" action="dinpay.aspx" method="post" target="_blank">
                    <input name="n0" type="hidden" value="<%=channel.ID%>" />
                    <input name="n1" type="text" class="thirdpay"/>
                </form>
            </td>
            <td>
                <input type="button" class="info_button" value="提交" onclick="<%=randID("f1")%>_()"<% if (channel==null) { %> disabled="disabled" <% } %> />
            </td>
        </tr>
                <% } else if (channel is web.YeepayRowData) { %>
        <tr><td class="title" style="text-align: right; width: 102px;">存款金额：  </td>
            <td class="value thirdpay" style="text-align: left;">
                <form id="<%=randID("form_id")%>_" action="yeepay.aspx" method="post" target="_blank">
                    <input name="n0" type="hidden" value="<%=channel.ID%>" />
                    <input name="n1" type="text" class="thirdpay" />
                </form>
            </td>
            <td>
                <input type="button" class="info_button" value="提交" onclick="<%=randID("f1")%>_()"<% if (channel==null) { %> disabled="disabled" <% } %> />
            </td>
        </tr>
                <% } else { %>
        <tr><td class="title" style="text-align: right; width: 102px;">存款金额：  </td>
            <td class="value thirdpay" style="text-align: left;">
                <% if (channel is web.DinpayRowData) { %>
                <form id="<%=randID("form_id")%>_" action="dinpay.aspx" method="post" target="_blank">
                    <input name="n0" type="hidden" value="<%=channel.ID%>" />
                    <input name="n1" type="text" class="thirdpay"/>
                </form>
                <% } else if (channel is web.YeepayRowData) { %>
                <form id="<%=randID("form_id")%>_" action="yeepay.aspx" method="post" target="_blank">
                    <input name="n0" type="hidden" value="<%=channel.ID%>" />
                    <input name="n1" type="text" class="thirdpay" />
                </form>
                <% } else { %>
                <input type="text" disabled="disabled" class="thirdpay"/>
                <% } %>
            </td>
            <td>
                <input type="button" class="info_button" value="提交" onclick="<%=randID("f1")%>_()"<% if (channel==null) { %> disabled="disabled" <% } %> />
            </td>
        </tr>
                <% } %>



        <tr>
            <td class="title"></td><td colspan="2" class="value" style="color: yellow;">
                <% if (channel==null) { %>在线支付维护中，暂时无法使用请选择绿色通道支付！<% } else { %>建议存款时请添加零头 例如：存500元的可以存501或508，以便我们优先为您处理！<% } %>
           </td>
        </tr>
        <tr>
            <td></td>
            <td colspan="2" class="submit" style="text-align: left;padding-left: 7px;">
                <input type="button" class="info_button" value="提交" onclick="<%=randID("f1")%>_()"<% if (channel==null) { %> disabled="disabled" <% } %> />
                <span class="user_errmsg"></span>
        </td></tr>
    </table>--%>
</div>