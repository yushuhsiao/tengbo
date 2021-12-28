https://pbsz.ebank.cmbchina.com/CmbBank_PB/UI/PBPC/DebitCard_AccountManager/am_QueryTodayTrans.aspx
<BODY id=body onload=InitPage() MS_POSITIONING="GridLayout"><FORM id=am_QueryTodayTrans onsubmit=CheckValid() method=post name=am_QueryTodayTrans action=am_QueryTodayTrans.aspx>
<DIV><INPUT id=__EVENTTARGET type=hidden name=__EVENTTARGET> <INPUT id=__EVENTARGUMENT type=hidden name=__EVENTARGUMENT> <INPUT id=__LASTFOCUS type=hidden name=__LASTFOCUS> <INPUT id=__VIEWSTATE value=/wEPDwULLTEyNzMwNzYwNjgPZBYCAgMPZBYQAgEPDxYCHgRUZXh0BRvotKbmiLfnrqHnkIYgPiDkuqTmmJPmn6Xor6JkZAIDDxYCHgdWaXNpYmxlaGQCBQ8PFgIfAAUGMTAxMDAzZGQCBw8QZBAVARA2MjI1ODg1NzkwOTgwMjIxFQEVQTA1NzE2MjI1ODg1NzkwOTgwMjIxFCsDAWcWAWZkAgkPDxYCHwFnZGQCCg8PFgIfAWhkZAIMDxYCHwFnFgQCAQ8QZBAVASEg5rS75pyf57uT566X5oi3IOS6uuawkeW4gSAwMDAwMCAVAQ8xMjUyMjI0ODUwMDAwMDIUKwMBZ2RkAgMPEA8WBh4ORGF0YVZhbHVlRmllbGQFA0tleR4NRGF0YVRleHRGaWVsZAUFVmFsdWUeC18hRGF0YUJvdW5kZ2QQFQEKWyDlhajpg6ggXRUBAS0UKwMBZ2RkAg4PFgIfAWcWCgIBDw8WAh8ABRA2MjI1ODg1NzkwOTgwMjIxZGQCBQ8PFgIfAAUBMGRkAgcPDxYCHwAFATBkZAIJDw8WAh8ABQQwLjAwZGQCCw8PFgIfAAUEMC4wMGRkZB+pGtgX8UY3swn/20fPGBMBEmNY type=hidden name=__VIEWSTATE> </DIV>
<SCRIPT type=text/javascript>
//<![CDATA[
var theForm = document.forms['am_QueryTodayTrans'];
if (!theForm) {
    theForm = document.am_QueryTodayTrans;
}
function __doPostBack(eventTarget, eventArgument) {
    if (!theForm.onsubmit || (theForm.onsubmit() != false)) {
        theForm.__EVENTTARGET.value = eventTarget;
        theForm.__EVENTARGUMENT.value = eventArgument;
        theForm.submit();
    }
}
//]]>
</SCRIPT>

<DIV><INPUT id=__EVENTVALIDATION value=/wEWCQK69MKtDwLK5aCoBAKAwvDZDQKehae6BQLUmfTWAQL9mpmPAQKSm5bSAwLm9cyQAgKU69TQBZSpPdnDtBDq20gdc0k166nvc/KN type=hidden name=__EVENTVALIDATION> </DIV>
<TABLE id=Table1 class=tbMain border=0 cellSpacing=0>
<TBODY>
<TR>
<TD class=tdCommonTop>
<TABLE id=Table2 class=tbTitle border=0 cellSpacing=0>
<TBODY>
<TR>
<TD class=tdTitle1>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;当前功能： <SPAN id=Location>账户管理 &gt; 交易查询</SPAN>&nbsp;&nbsp;&nbsp; </TD>
<TD class=tdTitle2></TD>
<TD class=tdTitle3>版面号：<SPAN id=lbPageNo>101003</SPAN><IMG alt="" src="../doc/Images/title_06.gif"></TD></TR></TBODY></TABLE>
<TABLE id=Table3 class=tbBlock border=0 cellSpacing=1>
<TBODY>
<TR>
<TD class=tdCenterW20H40>一卡通卡号：</TD>
<TD class=tdLeftH40><SELECT id=ddlDebitCardList onchange="javascript:setTimeout('__doPostBack(\'ddlDebitCardList\',\'\')', 0)" name=ddlDebitCardList> <OPTION selected value=A05716225885790980221>6225885790980221</OPTION></SELECT> </TD></TR></TBODY></TABLE></TD></TR>
<TR>
<TD class=tdSpaceH12><FONT face=宋体></FONT><FONT face=宋体></FONT></TD></TR>
<TR>
<TD class=tdCommonTop>
<TABLE id=Table4 class=tbPanel border=0 cellSpacing=0 cellPadding=0>
<TBODY>
<TR>
<TD class=tdPanelContent><!-- 交易查询标签 -->
<TABLE class=tbPanelContent border=0 cellSpacing=0 cellPadding=0 width=300>
<TBODY>
<TR>
<TD class=tdPanelHead></TD>
<TD class=tdPanelSel><A class=lkPanelSel onclick="triggerFunc('../DebitCard_AccountManager/am_QueryTodayTrans.aspx','FORM','_self')" href="#">当天交易查询</A></TD>
<TD class=tdPanelNoSel><A class=lkPanelNoSel onclick="triggerFunc('../DebitCard_AccountManager/am_QueryHistoryTrans.aspx','FORM','_self')" href="#">历史交易查询</A></TD></TR></TBODY></TABLE><!-- END 交易查询标签 --></TD>
<TD class=tdPanelTrail><FONT face=宋体></FONT></TD></TR></TBODY></TABLE></TD></TR>
<TR>
<TD id=tdQueryResult1 class=tdCommonTop>
<TABLE id=Table5 class=tbCommonColor border=0 cellSpacing=0>
<TBODY>
<TR>
<TD></TD></TR></TBODY></TABLE>
<TABLE id=Table6 class=tbCommonColor border=0 cellSpacing=1>
<TBODY>
<TR>
<TD class=tdLeftH30>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;子账户：<SELECT style="WIDTH: 200px" id=ddlSubAccountList name=ddlSubAccountList> <OPTION selected value=125222485000002>活期结算户 人民币 00000</OPTION></SELECT> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;交易类型：<SELECT style="WIDTH: 100px" id=ddlTransTypeList name=ddlTransTypeList> <OPTION selected value=->[ 全部 ]</OPTION></SELECT> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<INPUT id=BtnOK class=btn value="查 询" type=submit name=BtnOK> </TD></TR></TBODY></TABLE></TD></TR>
<TR>
<TD class=tdSpaceH12></TD></TR>
<TR>
<TD id=tdQueryResult2 class=tdCommonTop>
<TABLE id=Table7 class=tbBlock border=0 cellSpacing=1>
<TBODY>
<TR>
<TD class=tdPrompt>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;以下是一卡通 <SPAN id=AccountNo class=wcLabel>6225885790980221</SPAN>&nbsp;的当天交易记录：</TD></TR></TBODY></TABLE>
<TABLE style="BORDER-BOTTOM: #9fd6ff 1px solid; BORDER-LEFT: #9fd6ff 1px solid; BORDER-COLLAPSE: collapse; BORDER-TOP: #9fd6ff 1px solid; BORDER-RIGHT: #9fd6ff 1px solid" id=dgTodayTransRecSet class=dgMain border=1 rules=all cellSpacing=0 cellPadding=3 align=center>
<TBODY>
<TR class=dgHeader align=middle>
<TD style="WIDTH: 15%" class=dgHeader>交易日期</TD>
<TD style="WIDTH: 15%" class=dgHeader>支出</TD>
<TD style="WIDTH: 15%" class=dgHeader>存入</TD>
<TD style="WIDTH: 15%" class=dgHeader>余额</TD>
<TD style="WIDTH: 15%" class=dgHeader>交易类型</TD>
<TD style="WIDTH: 25%" class=dgHeader>交易备注</TD></TR></TBODY></TABLE>
<TABLE id=Table8 class=tbBlock border=0 cellSpacing=1>
<TBODY>
<TR>
<TD class=tdCommonTopColor>
<TABLE id=Table9 class=tbCommonColor border=0 cellSpacing=1>
<TBODY>
<TR>
<TD class=tdLeftW30H20>&nbsp; 支出交易笔数： <SPAN id=OutCount class=wcLabel>0</SPAN></TD>
<TD class=tdLeftW30H20>&nbsp; 存入交易笔数： <SPAN id=InCount class=wcLabel>0</SPAN></TD>
<TD class=tdLeftH20></TD></TR>
<TR>
<TD class=tdLeftW30H20>&nbsp; 支出金额合计： <SPAN id=OutTotalMoney class=wcLabel>0.00</SPAN></TD>
<TD class=tdLeftW30H20>&nbsp; 存入金额合计： <SPAN id=InTotalMoney class=wcLabel>0.00</SPAN></TD>
<TD class=tdLeftH20><FONT face=宋体></FONT></TD></TR></TBODY></TABLE></TD></TR></TBODY></TABLE></TD></TR>
<TR>
<TD class=tdSpaceH12></TD></TR>
<TR>
<TD class=tdCommonTop>
<TABLE id=Table10 class=tbExplain border=0 cellSpacing=1 height=80>
<TBODY>
<TR>
<TD class=tdExplain><IFRAME id=ExplainPageFrame class=ifExplain src="../doc/Htmls/101003.htm" frameBorder=0 name=ExplainPageFrame scrolling=no></IFRAME></TD></TR></TBODY></TABLE></TD></TR>
<TR>
<TD class=tdSpaceH12></TD></TR>
<SCRIPT language=javascript src="../../Base/doc/Scripts/HostPkg.js"></SCRIPT>

<SCRIPT language=javascript src="../../Base/doc/Scripts/WebSvrCom.js"></SCRIPT>

<SCRIPT language=javascript src="../../Base/doc/Scripts/default.js"></SCRIPT>

<SCRIPT language=javascript src="../../Base/doc/Scripts/BaseFunc.js"></SCRIPT>

<SCRIPT language=javascript type=text/javascript>
    var sLocID = "021010";
    var sCurAcctInfo = "A05716225885790980221"; //信用卡请求时，为SIDType；一卡通时为AccountUID
    var g_RefreshFlg = 0;
    var sBusType = "PB"
    var AdvXmlRequestUrl = "";
    
    function showAdv() {
        if (g_RefreshFlg != 0)
            return;
        g_RefreshFlg = 1;

        if (sBusType == "CreditCard") {
            AdvXmlRequestUrl = "/CmbBank_CreditCard/UI/CreditCardPC/CreditCard/AdvRequest.aspx";
        }
        else if (sBusType == "CreditCardV2") {
            AdvXmlRequestUrl = "/CmbBank_CreditCardV2/UI/CreditCardPC/CreditCardV2/AdvRequest.aspx";
        }
        else if (sBusType == "PB") {
            AdvXmlRequestUrl = "/CmbBank_PB/UI/PBPC/DebitCard/AdvRequest.aspx";
        }
        else if (sBusType == "SHELL") {
            AdvXmlRequestUrl = "/CmbBank_GenShell/UI/GenShellPC/Login/AdvRequest.aspx";
        }
        else {
            return;
        }
        //adFrame.document.write("<style TYPE='text/css'>.mycontent {font: 9pt/14pt '宋体';font-weight:bold;color:#777777;background-color:#FFFFAA;text-align:center;vertical-align:middle;height:20px;width:90px}</style>"); //DDECF3
        //adFrame.document.write("<body style='margin-top:0;margin-bottom:0;margin-left:0;margin-right:0;'><div class='mycontent'>加载中 ...</div></body>");
        //adFrame.document.close();

        
        var hostPkg = new HostPkg("showAdvertisement");
        hostPkg.AddParam("CurAcctInfo", sCurAcctInfo);
        hostPkg.AddParam("LocID", sLocID);
        var webSvrCom = new WebSvrCom(AdvXmlRequestUrl);

        webSvrCom.WSCClearHostPkg();
        webSvrCom.WSCAddHostPkg(hostPkg);
        webSvrCom.async = true;
        webSvrCom.WSCSendRequest(OnLdasResponse);
    }

    function OnLdasResponse(xmlHttp) {
        var xmlResponse = xmlHttp.responseText;
        //检查错误码
        var errorCode = XMLGetValueByElementName(xmlResponse, "ErrorCode");
        if (errorCode == "0") {
            adFrame.location = XMLGetValueByElementName(xmlResponse, "Data");
        }
        else {
            var objTrAdv = document.getElementById("trAdv");
            if (objTrAdv != null) {
                objTrAdv.style.display = "none";
            }
        }
        return;
    }

</SCRIPT>

<TR id=trAdv>
<TD class=tdCommonTop>
<TABLE style="PADDING-BOTTOM: 2px; PADDING-LEFT: 2px; BORDER-SPACING: 1px; PADDING-RIGHT: 2px; PADDING-TOP: 2px" id=tbAdv class=tbAdv cellSpacing=1 cellPadding=2>
<TBODY>
<TR>
<TD id=tdAdv class=tdAdv><IFRAME id=adFrame class=ifAdv onload=showAdv() src="../../Base/Loading.htm" frameBorder=0 name=adFrame scrolling=no></IFRAME></TD></TR></TBODY></TABLE></TD></TR>
<TR>
<TD style="HEIGHT: 1px"><INPUT id=uc_Adv_AdvLocID type=hidden name=uc_Adv$AdvLocID></TD></TR>
<TR>
<TD style="HEIGHT: 1px"><INPUT id=ClientNo value=A8D921BC4263F86635A728E84F5D2363451789122173240100025862 type=hidden name=ClientNo><INPUT id=PanelControl type=hidden name=PanelControl>
<SCRIPT language=javascript type=text/javascript src="../../doc/Scripts/default.js"></SCRIPT>

<SCRIPT language=JavaScript type=text/JavaScript>
		var DIV_MSG_AdjustDIVSize = null;
		var DIV_MSG_SelectedObj = null;
		var DIV_MSG_ActionPage = "/CmbBank_PB/UI/Base/cn/Page/MessagePage3.aspx";
		var param1 = "";
////////////////////////////////////////////////////////////////////////////////////////////////////////////
//层的显示、隐藏以及大小调整
		////////////////////////////////////////////////////////////////////////////////////////////////////////////
		function IsIe6() {
		    var browser = navigator.appName;
		    var b_version = navigator.appVersion;
		    b_version = b_version.replace(/[ ]/g, "");
		    /*var version = b_version.split(";");
		    var trim_Version = version[1].replace(/[ ]/g, "");
		    if (browser == "Microsoft Internet Explorer" && trim_Version == "MSIE6.0")*/
		    if (browser == "Microsoft Internet Explorer" && b_version.indexOf("MSIE6.0")>=0) 
		    {
		        return true;
		    }
		    return false;
		}
		
		function IsXhtmlDoctype(){
            try{
                var res=false;
                /*********************************************
                Just check for internet explorer.
                **********************************************/
                if (typeof document.namespaces != "undefined" && document.all[0].nodeType == 8) {
                    var strDocType = "" + document.all[0].nodeValue;
                    
                    return strDocType.indexOf("XHTML") != -1;
                }
                else if (document.doctype != null) {
                    var strDocType = "" + document.doctype.toString();
                    
                }
                return false;
            } catch (e) {
                
                 return false;
            }
            return false;
        }

        var g_isXhtmlDocType = IsXhtmlDoctype();


		var g_isIe6 = IsIe6();
		
		function AdjustFrameWidth(id)
		{
			var frameIdOBJECT = window.document.getElementById(id);
			/*
			if(frameIdOBJECT.offsetWidth < 500)
			{
				frameIdOBJECT.style.width = 500;
			}
			else if(frameIdOBJECT.offsetWidth > window.document.body.clientWidth)
			{
				frameIdOBJECT.style.width = window.document.body.clientWidth;
			}
			*/
			frameIdOBJECT.style.width = 539;
		} 

		function AdjustFrameHeight(id)
		{
			var frameIdDOM = window.frames[id];
			var frameIdOBJECT = window.document.getElementById(id);
			
			if(GetBrowserVersion()=="IE")
			{
                frameIdOBJECT.style.height = frameIdOBJECT.contentWindow.document.body.scrollHeight<204?204:frameIdOBJECT.contentWindow.document.body.scrollHeight;
            }
            else if(/Firefox/.test(window.navigator.userAgent))
                frameIdOBJECT.style.height = frameIdOBJECT.contentWindow.document.body.offsetHeight<204?204:frameIdOBJECT.contentWindow.document.body.offsetHeight;
            else
            {
                var contentHeight = 0;
                var bodyChildNodes = frameIdOBJECT.contentWindow.document.body.childNodes;
                for(var i=0 ; i<bodyChildNodes.length; i++)
                {
                    if(bodyChildNodes[i].nodeType==1)
                    {
                        contentHeight += bodyChildNodes[i].scrollHeight;
                    }
                }
                frameIdOBJECT.height = contentHeight<204?204:contentHeight;
            }

//			    if (frameIdOBJECT.contentWindow.document.body.scrollHeight > 204)
//			        frameIdOBJECT.style.height = frameIdOBJECT.contentWindow.document.body.scrollHeight;
//			    else
//			        frameIdOBJECT.style.height = 204;

			var imgLefter = window.document.getElementById("imgLefter");
			var imgRighter = window.document.getElementById("imgRighter");
			imgLefter.height = frameIdOBJECT.offsetHeight;
			imgRighter.height = frameIdOBJECT.offsetHeight;
		}
		
		function AdjustDIVSize(msgID,shieldID)
		{
			var MsgDIV = window.document.getElementById(msgID);
			var ShieldDIV = window.document.getElementById(shieldID);
			if(MsgDIV != null)
			{
				MsgDIV.style.width = 565;//MsgDIV.scrollWidth;
				if(MsgDIV.scrollHeight>263)
				    MsgDIV.style.height = MsgDIV.scrollHeight;
				else
				    MsgDIV.style.height = 263;
				MsgDIV.style.left = (window.document.body.clientWidth - MsgDIV.offsetWidth) / 2;
				if (window.top == window && !g_isIe6 && g_isXhtmlDocType)
				    MsgDIV.style.top = 100;
				else {
				    MsgDIV.style.position = "absolute";
				    MsgDIV.style.top = (window.document.body.clientHeight - MsgDIV.offsetHeight) / 2;
				}
			}
			
			if(ShieldDIV != null)
			{
				ShieldDIV.style.width = window.document.body.clientWidth;
				
				if(window.document.body.scrollHeight > window.document.body.clientHeight)
				{
					ShieldDIV.style.height = window.document.body.scrollHeight;
				}
				else
				{
					ShieldDIV.style.height = window.document.body.clientHeight;
				}
				if(ShieldDIV.style.height<432)
				    ShieldDIV.style.height=432;
			}			
		}
		
		function AdjustMsgDIVSize()
		{
			AdjustDIVSize('MsgDIV','');
		}
		
		function MsgShow(DIV_MSG_MsgXml) {
		    param1 = "MsgXml='" + DIV_MSG_MsgXml + "'";
		    //window.attachEvent("onload", MsgDIVShowPre);
		    addEventHandler(window, "load", MsgDIVShowPre); 					
		}
		
		function StopTabAndEnter()
		{
             if(event.keyCode==9 || event.keyCode==13)
             {  
                 event.returnValue=false;
                 return false;
             }
         }

         function directShow(DIV_MSG_MsgXml) {
         param1 = "MsgXml='" + DIV_MSG_MsgXml + "'";
         MsgDIVShowPre();
         }
         function MsgDIVShowPre() {
             var ShieldDIV = window.document.getElementById("ShieldDIV");
             AdjustDIVSize('', 'ShieldDIV');
             ShieldDIV.style.visibility = "visible";
             AdjustFrameWidth('MsgIframe');
             triggerFuncEq(DIV_MSG_ActionPage, 'FORM', 'MsgIframe', param1);
             var content = window.document.getElementById("MsgIframe");
	         /*content.attachEvent("onload", MsgDIVShow);
	         document.attachEvent("onkeydown", StopTabAndEnter);*/
             addEventHandler(content, "load", MsgDIVShow);
             addEventHandler(document, "keydown", StopTabAndEnter); 
		     
         }
                
		function MsgDIVShow()
		{
		    var MsgTable = window.document.getElementById("MsgTable");
			MsgTable.style.display = "inline";
			AdjustFrameHeight("MsgIframe");
			AdjustDIVSize('MsgDIV','');
			var MsgDIV = window.document.getElementById("MsgDIV");
			MsgDIV.style.visibility = "visible";
			MsgDIV.onmousedown = grabObj;
			
			ShieldDIV.onresize = AdjustMsgDIVSize;
			
			DIV_MSG_AdjustDIVSize = window.setInterval("AdjustDIVSize('','ShieldDIV')",100);
		}
		
		function MsgHide()
		{
			var MsgDIV = window.document.getElementById("MsgDIV");
			var MsgIframe = window.document.getElementById("MsgIframe");
			var ShieldDIV = window.document.getElementById("ShieldDIV");
			
			if(MsgDIV.style.visibility == "visible")
			{
				MsgDIV.style.visibility = "hidden";
				MsgDIV.style.width = 0;
				MsgDIV.style.height = 0;
				MsgDIV.onmousedown = null;
				MsgIframe.style.width = "100%";
				MsgIframe.style.height = "100%";
				
	 			var MsgTable = window.document.getElementById("MsgTable");
				MsgTable.style.display = "none";				
			}			
			if(ShieldDIV.style.visibility == "visible")
			{
				ShieldDIV.style.visibility = "hidden";
				ShieldDIV.style.width = 0;
				ShieldDIV.style.height = 0;
				ShieldDIV.onresize = null;
			}
			
			if(DIV_MSG_AdjustDIVSize != null)
			{
				window.clearInterval(DIV_MSG_AdjustDIVSize);
			}
		    /*MsgIframe.detachEvent("onload", MsgDIVShow);
		    document.detachEvent("onkeydown", StopTabAndEnter);*/
			delEventHandler(MsgIframe, "load", MsgDIVShow);
			delEventHandler(document, "keydown", StopTabAndEnter);
			
			
			
		}
		
////////////////////////////////////////////////////////////////////////////////////////////////////////////		
//层的移动
////////////////////////////////////////////////////////////////////////////////////////////////////////////		
		function grabObj(evt)
		{
		    var evt = evt||event;
			var imgObjID = getEventElement(evt);
			if(imgObjID == "imgHeader")
			{
				DIV_MSG_SelectedObj = window.document.getElementById("MsgDIV");
			}
            
            
			offsetX = window.event?window.event.offsetX:evt.layerX;
			offsetY = window.event?window.event.offsetY:evt.layerY;

			var MsgDIV = window.document.getElementById("MsgDIV");
			MsgDIV.onmousemove = dragObj;
			MsgDIV.onmouseup = releaseObj;	
			// prevent further processing of mouseDown event so 
  			// that the Macintosh doesn't display the contextual 
			// menu and lets dragging work normally.
			return false;
		}
			
		function dragObj(evt)
		{
			if (DIV_MSG_SelectedObj)
			{
			    var evt = evt||event;
				x = evt.clientX - offsetX;
				y = evt.clientY - offsetY;

				if(x<0) x = 0;
				if((x+DIV_MSG_SelectedObj.offsetWidth)>window.document.body.clientWidth) x=window.document.body.clientWidth-DIV_MSG_SelectedObj.offsetWidth;

				DIV_MSG_SelectedObj.style.left = x;

				if(y<0) y=0;
				if((y+DIV_MSG_SelectedObj.offsetHeight)>window.document.body.clientHeight) y=window.document.body.clientHeight-DIV_MSG_SelectedObj.offsetHeight;

				DIV_MSG_SelectedObj.style.top = y;

				//prevent further system response to dragging
				if(window.event)
				    window.event.returnValue = false;
				return false
			}
		}

		function releaseObj()
		{
			if (DIV_MSG_SelectedObj)
			{
				DIV_MSG_SelectedObj = null;
				var MsgDIV = window.document.getElementById("MsgDIV");
				MsgDIV.onmousemove = null;
				MsgDIV.onmouseup = null;				
			}
		}
		
////////////////////////////////////////////////////////////////////////////////////////////////////////////		
//其他事件处理
////////////////////////////////////////////////////////////////////////////////////////////////////////////		
		function ChangeSrc(flag)
		{
			var imgClose = window.document.getElementById("imgClose");
			if(flag == 2)
			{
				imgClose.src = "/CmbBank_PB" +'/UI/Base/doc/Images/MessageNew9.gif';
			}
			else
			{
			    imgClose.src = "/CmbBank_PB" + '/UI/Base/doc/Images/MessageNew12.gif';
			}
		}
		
		function getEventElement(evt)
        {
            evt=evt||event;
            return (evt.srcElement||evt.target).id;
        }
		
		
</SCRIPT>
 
<DIV style="Z-INDEX: 10; POSITION: absolute; FILTER: alpha(opacity=10); WIDTH: 0px; HEIGHT: 0px; VISIBILITY: hidden; TOP: 0px; LEFT: 0px; opacity: 0.1" id=ShieldDIV><IFRAME style="OVERFLOW: hidden" id=ShieldIftame height="100%" marginHeight=0 src="/CmbBank_PB/UI/Base/DefaultDiv.htm" frameBorder=0 width="100%" marginWidth=0 scrolling=no>
    </IFRAME></DIV>
<DIV style="Z-INDEX: 15; POSITION: fixed; WIDTH: 0px; HEIGHT: 0px; VISIBILITY: hidden; TOP: 0px; LEFT: 0px" id=MsgDIV>
<TABLE style="DISPLAY: none" id=MsgTable border=0 cellSpacing=0 cellPadding=0 align=center>
<TBODY>
<TR>
<TD colSpan=3><IMG id=imgHeader alt="" src="/CmbBank_PB/UI/Base/doc/Images/MessageNew7.jpg">
<DIV style="POSITION: absolute; TOP: 10px; LEFT: 535px"><IMG style="CURSOR: hand" id=imgClose onmouseover=ChangeSrc(1); onmouseout=ChangeSrc(2); onclick=MsgHide(); alt="" src="/CmbBank_PB/UI/Base/doc/Images/MessageNew9.gif"> </DIV></TD></TR>
<TR>
<TD><IMG id=imgLefter src="/CmbBank_PB/UI/Base/doc/Images/MessageNew10.gif" width=13 height=10></TD>
<TD><IFRAME id=MsgIframe height=10 marginHeight=0 src="/CmbBank_PB/UI/Base/Blank.htm" frameBorder=0 width=537 name=MsgIframe marginWidth=0 scrolling=no>
                </IFRAME></TD>
<TD><IMG id=imgRighter src="/CmbBank_PB/UI/Base/doc/Images/MessageNew11.gif" width=13 height=10></TD></TR>
<TR>
<TD colSpan=3><IMG src="/CmbBank_PB/UI/Base/doc/Images/MessageNew8.jpg"> </TD></TR></TBODY></TABLE></DIV></TD></TR></TBODY></TABLE></FORM></BODY>