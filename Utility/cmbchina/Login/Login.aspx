https://pbsz.ebank.cmbchina.com/CmbBank_GenShell/UI/GenShellPC/Login/Login.aspx


<BODY id=body class=bdLogin onload=initPage() leftMargin=0 topMargin=0 ms_positioning="GridLayout"><FORM id=LoginForm onsubmit="return doLogin();" method=post name=LoginForm action=Login.aspx>
<DIV><INPUT id=__VIEWSTATE value=/wEPDwULLTE2MDE3NDE5ODUPZBYCZg9kFgQCAg9kFgJmDw9kFggeBVN0eWxlBYMBYmFja2dyb3VuZC1wb3NpdGlvbjpyaWdodCBib3R0b207YmFja2dyb3VuZC1yZXBlYXQ6bm8tcmVwZWF0O0JBQ0tHUk9VTkQtSU1BR0U6dXJsKC9DbWJCYW5rX0dlblNoZWxsL1VJL0Jhc2UvZG9jL2ltYWdlcy9kcm9wYmcuR0lGKTseB29uZm9jdXMFHEJyYW5jaE5vX1Nob3dEaXYodGhpcy52YWx1ZSkeB29uY2xpY2sFHEJyYW5jaE5vX1Nob3dEaXYodGhpcy52YWx1ZSkeCHJlYWRPbmx5BQR0cnVlZAIKDxYCHgNzcmMFdS4uLy4uL0Jhc2UvRXh0cmFXb3JkLmFzcHg/Q2xpZW50Tm89QThEOTIxQkM0MjYzRjg2NjM1QTcyOEU4NEY1RDIzNjM0NDE5NjQ0MDAxNjMxMjk0MDAwMDE5NDkmd2lkdGg9NjAmaGVpZ2h0PTI4JnR5cGU9MGRk53Zvsuxk6axPZsQYC8Q1LxiYMWw= type=hidden name=__VIEWSTATE> </DIV>
<DIV><INPUT id=NetBankUser type=hidden name=NetBankUser> <INPUT id=NetBankPwd type=hidden name=NetBankPwd> <INPUT id=DebitCardNo type=hidden name=DebitCardNo> <INPUT id=DebitCardQueryPwd type=hidden name=DebitCardQueryPwd> <INPUT id=CreditCardNo type=hidden name=CreditCardNo> <INPUT id=IdNo type=hidden name=IdNo> <INPUT id=CreditCardQueryPwd type=hidden name=CreditCardQueryPwd> <INPUT id=BookNo type=hidden name=BookNo> <INPUT id=BookPwd type=hidden name=BookPwd> <INPUT id=__EVENTVALIDATION value=/wEWBQLZ7O5NAs3+16oCAv3YqswFApOX0YkBAsPytr8LSgYDcgsuyPbxWpfhYX8v6gTywKg= type=hidden name=__EVENTVALIDATION> </DIV>
<TABLE id=LoginTable border=0 cellSpacing=0 cellPadding=0 width=726 bgColor=#ffffff align=center>
<TBODY>
<TR>
<TD>
<TABLE border=0 cellSpacing=0 cellPadding=0 width="100%">
<TBODY>
<TR>
<TD><IMG alt="" src="../doc/images/login_bg8.gif" width=405 height=70> </TD>
<TD style="BACKGROUND-IMAGE: url(../doc/images/login_bg9.gif); PADDING-RIGHT: 10px; BACKGROUND-REPEAT: no-repeat; HEIGHT: 70px" vAlign=top width=321 align=right>
<DIV style="LINE-HEIGHT: 20px"><A style="BACKGROUND-COLOR: white" id=linkLanguage class=Login5 href="../../GenShellPC_EN/Login/Login.aspx">English</A></DIV></TD></TR></TBODY></TABLE></TD></TR>
<TR>
<TD>
<TABLE id=Table2 border=0 cellSpacing=0 cellPadding=0 width="100%" align=center>
<TBODY>
<TR>
<TD background=../doc/images/login_bg10.gif width=5></TD>
<TD vAlign=top>
<TABLE id=Table3 border=0 cellSpacing=0 cellPadding=0 width="100%" background=../doc/images/login_bg2.gif>
<TBODY>
<TR>
<TD>
<TABLE id=Table4 border=0 cellSpacing=0 cellPadding=0 width="100%">
<TBODY>
<TR>
<TD height=25 width=52><IMG style="CURSOR: pointer" id=ImgDebitCard onclick="changeLoginType('A')" alt="" src="../doc/images/login_debitcard1.gif" width=52 height="100%"> </TD>
<TD height=25 width=1><IMG alt="" src="../doc/images/login_bg11.gif" width=1 height="100%"> </TD>
<TD height=25 width=52><IMG style="CURSOR: pointer" id=ImgCreditCard onclick="changeLoginType('C')" alt="" src="../doc/images/login_creditcard2.gif" width=52 height="100%"> </TD>
<TD height=25 width=1><IMG alt="" src="../doc/images/login_bg11.gif" width=1 height="100%"> </TD>
<TD height=25 width=44><IMG style="CURSOR: pointer" id=ImgBook onclick="changeLoginType('B')" alt="" src="../doc/images/login_book2.gif" width=44 height="100%"> </TD>
<TD height=25 width=1><IMG alt="" src="../doc/images/login_bg11.gif" width=1 height="100%"> </TD>
<TD height=25 width=109><IMG style="CURSOR: pointer" id=ImgUser onclick="changeLoginType('D')" alt="" src="../doc/images/login_user2.gif" width=109 height="100%"> </TD></TR></TBODY></TABLE></TD></TR>
<TR>
<TD height=258 vAlign=top background=../doc/images/login_bg2.gif colSpan=5>
<TABLE id=Table5 border=0 cellSpacing=0 cellPadding=0 width="100%">
<TBODY>
<TR>
<TD bgColor=#037ad9 height=1></TD></TR>
<TR>
<TD id=tdLoginList height=143 vAlign=top background=../doc/images/login_bg12.gif>
<TABLE id=Table6 border=0 cellSpacing=0 cellPadding=0 width="98%" align=center height="100%">
<TBODY>
<TR>
<TD height=6 colSpan=2>&nbsp; </TD></TR>
<TR id=trNetBankUser class=hide>
<TD style="WIDTH: 30%" vAlign=center align=middle>&nbsp;&nbsp;&nbsp;用&nbsp;户&nbsp;名： </TD>
<TD vAlign=center>
<TABLE border=0 cellSpacing=0 cellPadding=0 width="100%" align=center height="100%">
<TBODY>
<TR>
<TD width=105>
<OBJECT id=NetBankUser_Ctrl codeBase="https://site.cmbchina.com/download/CMBEdit.cab#version=1,2,0,1" classid=clsid:0CA54D3F-CEAE-48AF-9A2B-31909CB9515D data="data:application/x-oleobject;base64,P02lDK7Or0iaKzGQnLlRXRAHAADYEwAA2BMAAA==" width=100 height=20>
<SPAN>未安装控件，<a class=Login1 target='_blank' href='/CmbBank_HelpCenter/UI/GenShellAcc/Default.aspx' style="color:red">我要安装</a>。</SPAN></OBJECT>
<SCRIPT language=javascript>
			function initNetBankUser_Ctrl()
            {
                try
                {
                    document.getElementById('NetBankUser_Ctrl').MaxLength = "32";
			        if (!false) document.getElementById('NetBankUser_Ctrl').PasswdCtrl = false;
                    if (true) document.getElementById('NetBankUser_Ctrl').Option("#1#https://pbsz.ebank.cmbchina.com/CmbBank_GenShell/UI/Manage/GetInfo.aspx?ClientNo=A8D921BC4263F86635A728E84F5D2363441964400163129400001949");
                    //if ("A8D921BC4263F86635A728E84F5D2363441964400163129400001949" != "") document.getElementById('NetBankUser_Ctrl').Info = "A8D921BC4263F86635A728E84F5D2363441964400163129400001949";
					document.getElementById('NetBankUser_Ctrl').Info = "A8D921BC4263F86635A728E84F5D2363441964400163129400001949";
					document.getElementById('NetBankUser_Ctrl').value = "";
                }
                catch(err)
                {
                    //window.alert('sorry, please install safe input control first.');
                }
			}
			
            if (window.attachEvent) 
            {//For IE
               window.attachEvent("onload", initNetBankUser_Ctrl);
            } 
            else if (window.addEventListener) 
            {//For FireFox
                if(document.getElementById('NetBankUser_Ctrl').MaxLength != undefined)
				{
                    window.addEventListener("load", initNetBankUser_Ctrl, "false");
                }
            } 
            else 
            {
                window["onload"] = initNetBankUser_Ctrl;
            }
			</SCRIPT>

<SCRIPT language=javascript>
			function fillNetBankUser_Ctrl()
            {
                try
                {
				document.getElementById('NetBankUser_Ctrl').Info = "A8D921BC4263F86635A728E84F5D2363441964400163129400001949";
			        if (true) document.getElementById('NetBankUser').value = document.getElementById('NetBankUser_Ctrl').IValue;
			        else document.getElementById('NetBankUser').value = document.getElementById('NetBankUser_Ctrl').value;
					//if ("A8D921BC4263F86635A728E84F5D2363441964400163129400001949" != "") document.getElementById('NetBankUser_Ctrl').Info = "A8D921BC4263F86635A728E84F5D2363441964400163129400001949";
					
                    return true;
    			}
                catch(err)
                {
                    /*因为登陆页面Login.aspx上登录成功后，会删除LoginForm，所以在LoginForm上的安全控件就都会
                      被删除，提交时再通过document.getElementById取安全控件就会取不到了，所以不做相关提示了。*/
                    //window.alert('很抱歉，请先安装或更新安全控件。');
                    return false;
                }
            }
			
            if (document.forms[0].attachEvent) 
            {//For IE
                document.forms[0].attachEvent("onsubmit", fillNetBankUser_Ctrl);
            }               
            else if (document.forms[0].addEventListener) 
            {//For FireFox
                document.forms[0].addEventListener("submit", fillNetBankUser_Ctrl, false);
            } 
            else 
            {
                document.forms[0]["onsubmit"] = fillNetBankUser_Ctrl;
            }
			</SCRIPT>

<SCRIPT language=javascript>
			function focusNetBankUser_Ctrl()
            {
                try
                {
                    document.getElementById('NetBankUser_Ctrl').MaxLength = "32";
			        if (!false) document.getElementById('NetBankUser_Ctrl').PasswdCtrl = false;
                    if (true) document.getElementById('NetBankUser_Ctrl').Option("#1#https://pbsz.ebank.cmbchina.com/CmbBank_GenShell/UI/Manage/GetInfo.aspx?ClientNo=A8D921BC4263F86635A728E84F5D2363441964400163129400001949");
                }
                catch(err)
                {
      
                }
			}

            if (window.addEventListener) 
            {
                document.getElementById('NetBankUser_Ctrl').addEventListener("focus", focusNetBankUser_Ctrl, "false");
            } 
			</SCRIPT>
</TD>
<TD><A class=Login3 tabIndex=-1 onclick=EAccountGetLoginNameWinOpen() href="#">忘记登录名？</A> </TD></TR></TBODY></TABLE></TD></TR>
<TR id=trNBUserTips class=hide>
<TD>&nbsp; </TD>
<TD vAlign=top>请输入登录名/手机号/邮箱 </TD></TR>
<TR id=trNetBankPwd class=hide>
<TD style="WIDTH: 30%" vAlign=center align=middle><IMG style="CURSOR: hand" onclick=EAccountWhatNetBankPwdWinOpen() border=0 alt=什么是网银密码？ align=top src="../doc/images/lock.gif" width=11 height=12>网银密码： </TD>
<TD vAlign=center>
<TABLE border=0 cellSpacing=0 cellPadding=0 width="100%" align=center height="100%">
<TBODY>
<TR>
<TD width=105>
<OBJECT id=NetBankPwd_Ctrl codeBase="https://site.cmbchina.com/download/CMBEdit.cab#version=1,2,0,1" classid=clsid:0CA54D3F-CEAE-48AF-9A2B-31909CB9515D data="data:application/x-oleobject;base64,P02lDK7Or0iaKzGQnLlRXRAHAADYEwAA2BMAAA==" width=100 height=20>
<SPAN>未安装控件，<a class=Login1 target='_blank' href='/CmbBank_HelpCenter/UI/GenShellAcc/Default.aspx' style="color:red">我要安装</a>。</SPAN></OBJECT>
<SCRIPT language=javascript>
			function initNetBankPwd_Ctrl()
            {
                try
                {
                    document.getElementById('NetBankPwd_Ctrl').MaxLength = "32";
			        if (!true) document.getElementById('NetBankPwd_Ctrl').PasswdCtrl = false;
                    if (true) document.getElementById('NetBankPwd_Ctrl').Option("#1#https://pbsz.ebank.cmbchina.com/CmbBank_GenShell/UI/Manage/GetInfo.aspx?ClientNo=A8D921BC4263F86635A728E84F5D2363441964400163129400001949");
                    //if ("A8D921BC4263F86635A728E84F5D2363441964400163129400001949" != "") document.getElementById('NetBankPwd_Ctrl').Info = "A8D921BC4263F86635A728E84F5D2363441964400163129400001949";
					document.getElementById('NetBankPwd_Ctrl').Info = "A8D921BC4263F86635A728E84F5D2363441964400163129400001949";
					document.getElementById('NetBankPwd_Ctrl').value = "";
                }
                catch(err)
                {
                    //window.alert('sorry, please install safe input control first.');
                }
			}
			
            if (window.attachEvent) 
            {//For IE
               window.attachEvent("onload", initNetBankPwd_Ctrl);
            } 
            else if (window.addEventListener) 
            {//For FireFox
                if(document.getElementById('NetBankPwd_Ctrl').MaxLength != undefined)
				{
                    window.addEventListener("load", initNetBankPwd_Ctrl, "false");
                }
            } 
            else 
            {
                window["onload"] = initNetBankPwd_Ctrl;
            }
			</SCRIPT>

<SCRIPT language=javascript>
			function fillNetBankPwd_Ctrl()
            {
                try
                {
				document.getElementById('NetBankPwd_Ctrl').Info = "A8D921BC4263F86635A728E84F5D2363441964400163129400001949";
			        if (true) document.getElementById('NetBankPwd').value = document.getElementById('NetBankPwd_Ctrl').IValue;
			        else document.getElementById('NetBankPwd').value = document.getElementById('NetBankPwd_Ctrl').value;
					//if ("A8D921BC4263F86635A728E84F5D2363441964400163129400001949" != "") document.getElementById('NetBankPwd_Ctrl').Info = "A8D921BC4263F86635A728E84F5D2363441964400163129400001949";
					
                    return true;
    			}
                catch(err)
                {
                    /*因为登陆页面Login.aspx上登录成功后，会删除LoginForm，所以在LoginForm上的安全控件就都会
                      被删除，提交时再通过document.getElementById取安全控件就会取不到了，所以不做相关提示了。*/
                    //window.alert('很抱歉，请先安装或更新安全控件。');
                    return false;
                }
            }
			
            if (document.forms[0].attachEvent) 
            {//For IE
                document.forms[0].attachEvent("onsubmit", fillNetBankPwd_Ctrl);
            }               
            else if (document.forms[0].addEventListener) 
            {//For FireFox
                document.forms[0].addEventListener("submit", fillNetBankPwd_Ctrl, false);
            } 
            else 
            {
                document.forms[0]["onsubmit"] = fillNetBankPwd_Ctrl;
            }
			</SCRIPT>

<SCRIPT language=javascript>
			function focusNetBankPwd_Ctrl()
            {
                try
                {
                    document.getElementById('NetBankPwd_Ctrl').MaxLength = "32";
			        if (!true) document.getElementById('NetBankPwd_Ctrl').PasswdCtrl = false;
                    if (true) document.getElementById('NetBankPwd_Ctrl').Option("#1#https://pbsz.ebank.cmbchina.com/CmbBank_GenShell/UI/Manage/GetInfo.aspx?ClientNo=A8D921BC4263F86635A728E84F5D2363441964400163129400001949");
                }
                catch(err)
                {
      
                }
			}

            if (window.addEventListener) 
            {
                document.getElementById('NetBankPwd_Ctrl').addEventListener("focus", focusNetBankPwd_Ctrl, "false");
            } 
			</SCRIPT>
</TD>
<TD><A class=Login3 tabIndex=-1 onclick=EAccountGetPwdBaseWinOpen() href="#">忘记密码？</A> </TD></TR></TBODY></TABLE></TD></TR>
<TR id=trBranchNo class=showFF>
<TD style="WIDTH: 30%" vAlign=center align=middle>&nbsp;&nbsp;&nbsp;开&nbsp; 户&nbsp;地： </TD>
<TD vAlign=center><INPUT style="BACKGROUND-IMAGE: url(/CmbBank_GenShell/UI/Base/doc/images/dropbg.GIF); WIDTH: 160px; BACKGROUND-REPEAT: no-repeat; BACKGROUND-POSITION: right bottom" id=BranchNo_tbxAddress onfocus=BranchNo_ShowDiv(this.value) onclick=BranchNo_ShowDiv(this.value) value=金华 readOnly name=BranchNo$tbxAddress><INPUT style="DISPLAY: none" id=BranchNo_branchNo value=0579 name=BranchNo$branchNo><INPUT id=BranchNo_hiddenBranchNo value=0579 type=hidden name=BranchNo> 
<DIV style="DISPLAY: none" id=BranchNo_forpopdiv></DIV>
<STYLE type=text/css>
.popupcontent{ 
    position:absolute; 
    border:1px solid #000000; 
    line-height:16px; 
    background-color:#F2F9FF; 
    display:none; 
    cursor:default; 
    padding:2 5 2 5px; 
}
</STYLE>

<SCRIPT>
		function BranchNo_SetBranchNo(no)
		{
			document.getElementById("BranchNo_branchNo").value = no;
			document.getElementById("BranchNo_hiddenBranchNo").value = no;
		}
		
		function BranchNo_SetValue(no)
		{
			document.getElementById("BranchNo_branchNo").value = no;
			document.getElementById("BranchNo_hiddenBranchNo").value = no;
			var ds = BranchNo_dataSource;
			for(var i=0;i<ds.length;i++)
			{
				for(j=1;j<ds[i].length;j++)
				{
					if(no==ds[i][j][0])
					{
						document.getElementById("BranchNo_tbxAddress").value = ds[i][j][1];
					}
				}
			}
		}
		
		function BranchNo_GetPos(control,pos)
		{
		    pos.top = control.offsetTop;
		    pos.left = control.offsetLeft;
		    
		    while(control = control.offsetParent)
		    {
		        pos.top += control.offsetTop;
		        pos.left += control.offsetLeft;
		    }
		}
		
		function BranchNo_DivSetVisible(state) 
          { 
            var DivRef = document.getElementById('BranchNo_popupcontent'); 
            var IfrRef = document.getElementById('BranchNo_divshim'); 
              if(state) 
              { 
               DivRef.style.display = "block"; 
               IfrRef.style.width = DivRef.offsetWidth; 
               IfrRef.style.height = DivRef.offsetHeight; 
               IfrRef.style.top = DivRef.style.top; 
               IfrRef.style.left = DivRef.style.left; 
               IfrRef.style.zIndex = DivRef.style.zIndex - 1; 
               IfrRef.style.display = "block"; 
              } 
              else 
              { 
               DivRef.style.display = "none"; 
               IfrRef.style.display = "none"; 
              } 
          } 

		
		function BranchNo_ShowDiv(value)
		{
            if(!BranchNo_inited)
		    {
		        BranchNo_inited = BranchNo_Init();
		    }
		    if(window.event!=null)
			    window.event.returnValue = false;
			var div = document.getElementById("BranchNo_forpopdiv");
			div.innerHTML = BranchNo_html;
			var address = document.getElementsByName("BranchNo_address");
			
			for(var i=0;i<address.length;i++)
			{
				if(address[i].innerHTML == value)
				{
					address[i].className = "currcity";
					//address[i].style.fontWeight = "bold";
					break;
				}
			}
			
			var textInput = document.getElementById("BranchNo_tbxAddress");
			
			if(window.createPopup)
			{
			    BranchNo_pop.document.body.innerHTML = "";
			    BranchNo_pop.document.write('<body id=\"popBody\" scroll=auto>');
			    BranchNo_pop.document.write('<style>a:link { color: black; text-decoration: none} \n a:visited { color: black; text-decoration: none} \n a:active { color: #800080; text-decoration: underline} A:hover{background-color: gray;color:#ffffff;text-decoration: underline} \n a.currcity:link { color: blue;font-weight:bold; text-decoration: none} \n   a.currcity:visited { color: blue;font-weight:bold; text-decoration: none} \n   A.currcity:hover{background-color: gray;font-weight:bold;color:#ffffff;text-decoration: underline}</style>');
			    BranchNo_pop.document.write(div.innerHTML);
			    BranchNo_pop.document.write('</body>');
			    BranchNo_pop.document.body.style.backgroundColor = "#F2F9FF";
			    BranchNo_pop.document.body.style.border = "solid gray 1px";
    		
			    
			    BranchNo_pop.show(0,textInput.offsetHeight,BranchNo_gpopwidth,300,textInput);
			}
			else
			{
			    var pos = new Object();
                pos.left = 0;
                pos.top = 0;
                BranchNo_GetPos(textInput,pos);
                BranchNo_pop.style.left = pos.left; 
		        BranchNo_pop.style.top = pos.top+textInput.offsetHeight;
	            BranchNo_pop.style.width = BranchNo_gpopwidth + "px";
                BranchNo_pop.style.height = "300px"; 
                
                BranchNo_pop.style.backgroundColor = "#F2F9FF"; 
                BranchNo_pop.style.border = "solid #999999 1px"; 
                BranchNo_pop.style.fontSize = "12px"; 
                var ocbody; 
                var ocbody = "<body scroll=auto><style>a:link { color: black; text-decoration: none} \n a:visited { color: black; text-decoration: none} \n a:active { color: #800080; text-decoration: underline} A:hover{background-color: gray;color:#ffffff;text-decoration: underline} \n a.currcity:link { color: blue;font-weight:bold; text-decoration: none} \n   a.currcity:visited { color: blue;font-weight:bold; text-decoration: none} \n   A.currcity:hover{background-color: gray;font-weight:bold;color:#ffffff;text-decoration: underline}</style>"
			    ocbody += div.innerHTML;
			    ocbody += "</body>";
			    BranchNo_pop.innerHTML=ocbody; 
			
			    BranchNo_DivSetVisible(true); 
			    if(top.ResetHeight)
			        top.ResetHeight();
			}
			
			return false;
		}
		
		
		function BranchNo_HideDiv(value,no)
		{
			var text = document.getElementById("BranchNo_tbxAddress");
			text.value = value;
			
			
			document.getElementById("BranchNo_hiddenBranchNo").value = no;
			document.getElementById("BranchNo_branchNo").value = no;

			if(text.nextId!=null)
			{
				try{document.getElementById(text.nextId).focus();}catch(e){}
			}
			if(window.createPopup)
			    BranchNo_pop.hide();
			else
			    BranchNo_DivSetVisible(false); 
		}
		var BranchNo_dataSource = [["A",["0372","安阳"],["0412","鞍山"]],["B",["0010","北京"],["0472","包头"],["0130","滨海"],["0543","滨州"],["0918","宝鸡"]],["C",["0023","重庆"],["0028","成都"],["0731","长沙"],["0519","常州"],["0431","长春"]],["D",["0411","大连"],["0769","东莞"],["0415","丹东"],["0546","东营"],["0459","大庆"]],["E",["0477","鄂尔多斯"]],["F",["0591","福州"],["0757","佛山"],["0413","抚顺"]],["G",["0020","广州"],["0797","赣州"],["0851","贵阳"]],["H",["0571","杭州"],["0551","合肥"],["0451","哈尔滨"],["0714","黄石"],["0471","呼和浩特"],["0572","湖州"],["0554","淮南"],["0734","衡阳"],["0752","惠州"],["0873","红河"],["0713","黄冈"],["0470","呼伦贝尔"],["0898","海口"],["0561","淮北"]],["J",["0531","济南"],["0579","金华"],["0573","嘉兴"],["0613","江都"],["0792","九江"],["0356","晋城"],["0537","济宁"],["0511","江阴"],["0750","江门"]],["K",["0871","昆明"]],["L",["0931","兰州"],["0888","丽江"],["0833","乐山"],["0539","临沂"],["0379","洛阳"],["0772","柳州"],["0830","泸州"],["0597","龙岩"],["0738","娄底"]],["M",["0981","绵阳"],["0555","马鞍山"]],["N",["0025","南京"],["0791","南昌"],["0574","宁波"],["0513","南通"],["0771","南宁"]],["P",["0427","盘锦"],["0594","莆田"]],["Q",["0532","青岛"],["0595","泉州"],["0570","衢州"],["0874","曲靖"]],["R",["0587","瑞安"],["0633","日照"]],["S",["0755","深圳"],["0021","上海"],["0024","沈阳"],["0512","苏州"],["0575","绍兴"],["0311","石家庄"],["0793","上饶"],["0719","十堰"],["0349","朔州"]],["T",["0022","天津"],["0351","太原"],["0576","台州"],["0523","泰州"],["0435","通化"]],["W",["0027","武汉"],["0991","乌鲁木齐"],["0510","无锡"],["0577","温州"],["0536","潍坊"],["0553","芜湖"],["0631","威海"]],["X",["0029","西安"],["0592","厦门"],["0732","湘潭"],["0910","咸阳"],["0712","孝感"],["0710","襄樊"],["0516","徐州"],["0972","西宁"]],["Y",["0717","宜昌"],["0535","烟台"],["0514","扬州"],["0610","宜兴"],["0912","榆林"],["0417","营口"],["0951","银川"],["0515","盐城"]],["Z",["0371","郑州"],["0733","株洲"],["0612","镇江"],["0533","淄博"],["0760","中山"],["0596","漳州"],["0656","珠海"],["0850","遵义"],["0759","湛江"]]];
		var BranchNo_pop = null;
		var BranchNo_html;
		var BranchNo_gpopwidth;
		var BranchNo_inited = false;
	
	

        
		function BranchNo_Init(event)
		{
		    if(window.createPopup)
		        BranchNo_pop = window.createPopup();
		    else
		        BranchNo_pop = document.getElementById("BranchNo_popupcontent");
		    if(BranchNo_pop==null)
		    {
		        alert("弹出城市选择控件被拦截，请关闭拦截程序后再按F5或者刷新重试！");
		        return false;
		    } 
		    
		    var dataSource = BranchNo_dataSource;
			var html = "<style>A:hover{background-color: gray;}</style>"
			html += "<table style='FONT-SIZE: 12px; LINE-HEIGHT: 16px; margin-top:10px;' width='100%'><tr id=\"trPopBody\">";
			var currColumnSize =0;
			var colunns = 0;
			for(var i=0;i<dataSource.length;i++)
			{
				var currSeries = dataSource[i];
				if(currColumnSize>=10 && currColumnSize+currSeries.length+1>12)
				{
					html += "</td>"
					currColumnSize = 0;
				}
				if(currColumnSize==0)
				{
					html += "<td vAlign=\"top\" align=\"center\">";
					colunns++;
				}else
				{
					html += "<br>";
					currColumnSize++;
				}
				
				for(var j=0;j<currSeries.length;j++)
				{
					if(currColumnSize!=0)
						html += "<br>";
					if(j==0)
						html += "<span style=\"color:red;FONT-Family:'Times New Roman';FONT-WEIGHT: bold;font-size:12px\">"+currSeries[j]+"</span>";
					else
					{
					    if(window.createPopup)
					    {
						    html += "<a name='BranchNo_address' href='#'  onclick=\"parent.BranchNo_HideDiv('" + currSeries[j][1] +"','" + currSeries[j][0] +"');return false;\">" + currSeries[j][1] + "</a>";
					    }
					    else
					    {
					        html += "<a name='BranchNo_address' href='#'  onclick=\"BranchNo_HideDiv('" + currSeries[j][1] +"','" + currSeries[j][0] +"');return false;\">" + currSeries[j][1] + "</a>";
					    }
					}
					currColumnSize++;
				}
			}
			
			html += "</td></tr></table>";
			BranchNo_html = html;
			BranchNo_gpopwidth =60*colunns;
		    
			return true;
		}
		if(""!="")
	    {
		    BranchNo_SetValue("");
	    }
	    
	    document.onclick=function(event)
	    {
	        event = event? event:window.event;   
	        var srcelement = event.target?event.target:event.srcElement; 
	        var docDivs = document.getElementsByTagName("div");
	        var front = "";
	        for(var i=0;i<docDivs.length;i++)
	        {
	            try{
	                if(docDivs[i].id !=null && docDivs[i].id.indexOf("_popupcontent")!=-1 && (docDivs[i].style.display==""||docDivs[i].style.display=="block"))
	                {
	                		var temp=docDivs[i].id.lastIndexOf("_");
	                		front=docDivs[i].id.substring(0,temp);
	                		/*
	                    var temp = docDivs[i].id.split("_");
	                    for(var j=0; j<temp.length-1; j++)
	                    {
	                        front = front+temp[j];
	                    }
	                    */
	                    break;
	                }
	            }catch(err){}
	        }
	        if(front!="" && (srcelement.id==null || srcelement.id.indexOf(front)==-1))//!=(front+"_popupcontent")&&srcelement.id!="BranchNo_tbxAddress"&&srcelement.id!=(front+"_divshim"))
	        {
	            //BranchNo_DivSetVisible(false);
	            var DivRef = document.getElementById(front+'_popupcontent'); 
                var IfrRef = document.getElementById(front+'_divshim');
                if(DivRef!=null)
                	DivRef.style.display="none";
                if(IfrRef!=null)
                	IfrRef.style.display="none";
	        }
	    }
</SCRIPT>

<DIV style="Z-INDEX: 100" id=BranchNo_popupcontent class=popupcontent></DIV><IFRAME style="POSITION: absolute; WIDTH: 0px; DISPLAY: none; HEIGHT: 0px; TOP: 0px; LEFT: 0px" id=BranchNo_divshim src="javascript:false;" frameBorder=0 scrolling=no> 
</IFRAME></TD></TR>
<TR id=trDebitCardNo class=showFF>
<TD style="WIDTH: 30%" vAlign=center align=middle>&nbsp;&nbsp;&nbsp;卡&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;号： </TD>
<TD vAlign=center>
<OBJECT id=DebitCardNo_Ctrl codeBase="https://site.cmbchina.com/download/CMBEdit.cab#version=1,2,0,1" classid=clsid:0CA54D3F-CEAE-48AF-9A2B-31909CB9515D data="data:application/x-oleobject;base64,P02lDK7Or0iaKzGQnLlRXRAHAACJEAAAEQIAAA==" width=160 height=20>
<SPAN>未安装控件，<a class=Login1 target='_blank' href='/CmbBank_HelpCenter/UI/GenShellAcc/Default.aspx' style="color:red">我要安装</a>。</SPAN></OBJECT>
<SCRIPT language=javascript>
			function initDebitCardNo_Ctrl()
            {
                try
                {
                    document.getElementById('DebitCardNo_Ctrl').MaxLength = "16";
			        if (!false) document.getElementById('DebitCardNo_Ctrl').PasswdCtrl = false;
                    if (true) document.getElementById('DebitCardNo_Ctrl').Option("#1#https://pbsz.ebank.cmbchina.com/CmbBank_GenShell/UI/Manage/GetInfo.aspx?ClientNo=A8D921BC4263F86635A728E84F5D2363441964400163129400001949");
                    //if ("A8D921BC4263F86635A728E84F5D2363441964400163129400001949" != "") document.getElementById('DebitCardNo_Ctrl').Info = "A8D921BC4263F86635A728E84F5D2363441964400163129400001949";
					document.getElementById('DebitCardNo_Ctrl').Info = "A8D921BC4263F86635A728E84F5D2363441964400163129400001949";
					document.getElementById('DebitCardNo_Ctrl').value = "";
                }
                catch(err)
                {
                    //window.alert('sorry, please install safe input control first.');
                }
			}
			
            if (window.attachEvent) 
            {//For IE
               window.attachEvent("onload", initDebitCardNo_Ctrl);
            } 
            else if (window.addEventListener) 
            {//For FireFox
                if(document.getElementById('DebitCardNo_Ctrl').MaxLength != undefined)
				{
                    window.addEventListener("load", initDebitCardNo_Ctrl, "false");
                }
            } 
            else 
            {
                window["onload"] = initDebitCardNo_Ctrl;
            }
			</SCRIPT>

<SCRIPT language=javascript>
			function fillDebitCardNo_Ctrl()
            {
                try
                {
				document.getElementById('DebitCardNo_Ctrl').Info = "A8D921BC4263F86635A728E84F5D2363441964400163129400001949";
			        if (true) document.getElementById('DebitCardNo').value = document.getElementById('DebitCardNo_Ctrl').IValue;
			        else document.getElementById('DebitCardNo').value = document.getElementById('DebitCardNo_Ctrl').value;
					//if ("A8D921BC4263F86635A728E84F5D2363441964400163129400001949" != "") document.getElementById('DebitCardNo_Ctrl').Info = "A8D921BC4263F86635A728E84F5D2363441964400163129400001949";
					
                    return true;
    			}
                catch(err)
                {
                    /*因为登陆页面Login.aspx上登录成功后，会删除LoginForm，所以在LoginForm上的安全控件就都会
                      被删除，提交时再通过document.getElementById取安全控件就会取不到了，所以不做相关提示了。*/
                    //window.alert('很抱歉，请先安装或更新安全控件。');
                    return false;
                }
            }
			
            if (document.forms[0].attachEvent) 
            {//For IE
                document.forms[0].attachEvent("onsubmit", fillDebitCardNo_Ctrl);
            }               
            else if (document.forms[0].addEventListener) 
            {//For FireFox
                document.forms[0].addEventListener("submit", fillDebitCardNo_Ctrl, false);
            } 
            else 
            {
                document.forms[0]["onsubmit"] = fillDebitCardNo_Ctrl;
            }
			</SCRIPT>

<SCRIPT language=javascript>
			function focusDebitCardNo_Ctrl()
            {
                try
                {
                    document.getElementById('DebitCardNo_Ctrl').MaxLength = "16";
			        if (!false) document.getElementById('DebitCardNo_Ctrl').PasswdCtrl = false;
                    if (true) document.getElementById('DebitCardNo_Ctrl').Option("#1#https://pbsz.ebank.cmbchina.com/CmbBank_GenShell/UI/Manage/GetInfo.aspx?ClientNo=A8D921BC4263F86635A728E84F5D2363441964400163129400001949");
                }
                catch(err)
                {
      
                }
			}

            if (window.addEventListener) 
            {
                document.getElementById('DebitCardNo_Ctrl').addEventListener("focus", focusDebitCardNo_Ctrl, "false");
            } 
			</SCRIPT>
</TD></TR>
<TR id=trDebitCardQueryPwd class=showFF>
<TD style="WIDTH: 30%" vAlign=center align=middle>&nbsp;&nbsp;&nbsp;查询密码： </TD>
<TD vAlign=center>
<TABLE border=0 cellSpacing=0 cellPadding=0 width="100%" align=center height="100%">
<TBODY>
<TR>
<TD width=105>
<OBJECT id=DebitCardQueryPwd_Ctrl codeBase="https://site.cmbchina.com/download/CMBEdit.cab#version=1,2,0,1" classid=clsid:0CA54D3F-CEAE-48AF-9A2B-31909CB9515D data="data:application/x-oleobject;base64,P02lDK7Or0iaKzGQnLlRXRAHAABVCgAAEQIAAA==" width=100 height=20>
<SPAN>未安装控件，<a class=Login1 target='_blank' href='/CmbBank_HelpCenter/UI/GenShellAcc/Default.aspx' style="color:red">我要安装</a>。</SPAN></OBJECT>
<SCRIPT language=javascript>
			function initDebitCardQueryPwd_Ctrl()
            {
                try
                {
                    document.getElementById('DebitCardQueryPwd_Ctrl').MaxLength = "6";
			        if (!true) document.getElementById('DebitCardQueryPwd_Ctrl').PasswdCtrl = false;
                    if (true) document.getElementById('DebitCardQueryPwd_Ctrl').Option("#1#https://pbsz.ebank.cmbchina.com/CmbBank_GenShell/UI/Manage/GetInfo.aspx?ClientNo=A8D921BC4263F86635A728E84F5D2363441964400163129400001949");
                    //if ("A8D921BC4263F86635A728E84F5D2363441964400163129400001949" != "") document.getElementById('DebitCardQueryPwd_Ctrl').Info = "A8D921BC4263F86635A728E84F5D2363441964400163129400001949";
					document.getElementById('DebitCardQueryPwd_Ctrl').Info = "A8D921BC4263F86635A728E84F5D2363441964400163129400001949";
					document.getElementById('DebitCardQueryPwd_Ctrl').value = "";
                }
                catch(err)
                {
                    //window.alert('sorry, please install safe input control first.');
                }
			}
			
            if (window.attachEvent) 
            {//For IE
               window.attachEvent("onload", initDebitCardQueryPwd_Ctrl);
            } 
            else if (window.addEventListener) 
            {//For FireFox
                if(document.getElementById('DebitCardQueryPwd_Ctrl').MaxLength != undefined)
				{
                    window.addEventListener("load", initDebitCardQueryPwd_Ctrl, "false");
                }
            } 
            else 
            {
                window["onload"] = initDebitCardQueryPwd_Ctrl;
            }
			</SCRIPT>

<SCRIPT language=javascript>
			function fillDebitCardQueryPwd_Ctrl()
            {
                try
                {
				document.getElementById('DebitCardQueryPwd_Ctrl').Info = "A8D921BC4263F86635A728E84F5D2363441964400163129400001949";
			        if (true) document.getElementById('DebitCardQueryPwd').value = document.getElementById('DebitCardQueryPwd_Ctrl').IValue;
			        else document.getElementById('DebitCardQueryPwd').value = document.getElementById('DebitCardQueryPwd_Ctrl').value;
					//if ("A8D921BC4263F86635A728E84F5D2363441964400163129400001949" != "") document.getElementById('DebitCardQueryPwd_Ctrl').Info = "A8D921BC4263F86635A728E84F5D2363441964400163129400001949";
					
                    return true;
    			}
                catch(err)
                {
                    /*因为登陆页面Login.aspx上登录成功后，会删除LoginForm，所以在LoginForm上的安全控件就都会
                      被删除，提交时再通过document.getElementById取安全控件就会取不到了，所以不做相关提示了。*/
                    //window.alert('很抱歉，请先安装或更新安全控件。');
                    return false;
                }
            }
			
            if (document.forms[0].attachEvent) 
            {//For IE
                document.forms[0].attachEvent("onsubmit", fillDebitCardQueryPwd_Ctrl);
            }               
            else if (document.forms[0].addEventListener) 
            {//For FireFox
                document.forms[0].addEventListener("submit", fillDebitCardQueryPwd_Ctrl, false);
            } 
            else 
            {
                document.forms[0]["onsubmit"] = fillDebitCardQueryPwd_Ctrl;
            }
			</SCRIPT>

<SCRIPT language=javascript>
			function focusDebitCardQueryPwd_Ctrl()
            {
                try
                {
                    document.getElementById('DebitCardQueryPwd_Ctrl').MaxLength = "6";
			        if (!true) document.getElementById('DebitCardQueryPwd_Ctrl').PasswdCtrl = false;
                    if (true) document.getElementById('DebitCardQueryPwd_Ctrl').Option("#1#https://pbsz.ebank.cmbchina.com/CmbBank_GenShell/UI/Manage/GetInfo.aspx?ClientNo=A8D921BC4263F86635A728E84F5D2363441964400163129400001949");
                }
                catch(err)
                {
      
                }
			}

            if (window.addEventListener) 
            {
                document.getElementById('DebitCardQueryPwd_Ctrl').addEventListener("focus", focusDebitCardQueryPwd_Ctrl, "false");
            } 
			</SCRIPT>
</TD>
<TD><A class=Login3 tabIndex=-1 onclick="CallFunc('CBANK_SHELL','DebitCard/am_ModiPwd.aspx','FORM','DebitCardRelFunc1')" href="#">忘记密码？</A>&nbsp; </TD></TR></TBODY></TABLE></TD></TR>
<TR id=trCreditCardLoginType class=hide height="20%">
<TD style="WIDTH: 30%" vAlign=baseline align=middle>&nbsp;&nbsp;&nbsp;登录方式： </TD>
<TD vAlign=baseline><INPUT id=LoginByPID onclick=changeCreditCardLoginType(); CHECKED type=radio name=rbCreditCardLoginType><LABEL for=LoginByPID><FONT color=black>证件</FONT></LABEL> <INPUT id=LoginByCreditCardNo onclick=changeCreditCardLoginType(); type=radio name=rbCreditCardLoginType><LABEL for=LoginByCreditCardNo><FONT color=black>卡号</FONT></LABEL> </TD></TR>
<TR id=trCreditCardNo class=hide>
<TD style="WIDTH: 30%" vAlign=center align=middle>&nbsp;&nbsp;&nbsp;卡&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;号： </TD>
<TD vAlign=center>
<OBJECT id=CreditCardNo_Ctrl codeBase="https://site.cmbchina.com/download/CMBEdit.cab#version=1,2,0,1" classid=clsid:0CA54D3F-CEAE-48AF-9A2B-31909CB9515D data="data:application/x-oleobject;base64,P02lDK7Or0iaKzGQnLlRXRAHAADYEwAA2BMAAA==" width=160 height=20>
<SPAN>未安装控件，<a class=Login1 target='_blank' href='/CmbBank_HelpCenter/UI/GenShellAcc/Default.aspx' style="color:red">我要安装</a>。</SPAN></OBJECT>
<SCRIPT language=javascript>
			function initCreditCardNo_Ctrl()
            {
                try
                {
                    document.getElementById('CreditCardNo_Ctrl').MaxLength = "16";
			        if (!false) document.getElementById('CreditCardNo_Ctrl').PasswdCtrl = false;
                    if (true) document.getElementById('CreditCardNo_Ctrl').Option("#1#https://pbsz.ebank.cmbchina.com/CmbBank_GenShell/UI/Manage/GetInfo.aspx?ClientNo=A8D921BC4263F86635A728E84F5D2363441964400163129400001949");
                    //if ("A8D921BC4263F86635A728E84F5D2363441964400163129400001949" != "") document.getElementById('CreditCardNo_Ctrl').Info = "A8D921BC4263F86635A728E84F5D2363441964400163129400001949";
					document.getElementById('CreditCardNo_Ctrl').Info = "A8D921BC4263F86635A728E84F5D2363441964400163129400001949";
					document.getElementById('CreditCardNo_Ctrl').value = "";
                }
                catch(err)
                {
                    //window.alert('sorry, please install safe input control first.');
                }
			}
			
            if (window.attachEvent) 
            {//For IE
               window.attachEvent("onload", initCreditCardNo_Ctrl);
            } 
            else if (window.addEventListener) 
            {//For FireFox
                if(document.getElementById('CreditCardNo_Ctrl').MaxLength != undefined)
				{
                    window.addEventListener("load", initCreditCardNo_Ctrl, "false");
                }
            } 
            else 
            {
                window["onload"] = initCreditCardNo_Ctrl;
            }
			</SCRIPT>

<SCRIPT language=javascript>
			function fillCreditCardNo_Ctrl()
            {
                try
                {
				document.getElementById('CreditCardNo_Ctrl').Info = "A8D921BC4263F86635A728E84F5D2363441964400163129400001949";
			        if (true) document.getElementById('CreditCardNo').value = document.getElementById('CreditCardNo_Ctrl').IValue;
			        else document.getElementById('CreditCardNo').value = document.getElementById('CreditCardNo_Ctrl').value;
					//if ("A8D921BC4263F86635A728E84F5D2363441964400163129400001949" != "") document.getElementById('CreditCardNo_Ctrl').Info = "A8D921BC4263F86635A728E84F5D2363441964400163129400001949";
					
                    return true;
    			}
                catch(err)
                {
                    /*因为登陆页面Login.aspx上登录成功后，会删除LoginForm，所以在LoginForm上的安全控件就都会
                      被删除，提交时再通过document.getElementById取安全控件就会取不到了，所以不做相关提示了。*/
                    //window.alert('很抱歉，请先安装或更新安全控件。');
                    return false;
                }
            }
			
            if (document.forms[0].attachEvent) 
            {//For IE
                document.forms[0].attachEvent("onsubmit", fillCreditCardNo_Ctrl);
            }               
            else if (document.forms[0].addEventListener) 
            {//For FireFox
                document.forms[0].addEventListener("submit", fillCreditCardNo_Ctrl, false);
            } 
            else 
            {
                document.forms[0]["onsubmit"] = fillCreditCardNo_Ctrl;
            }
			</SCRIPT>

<SCRIPT language=javascript>
			function focusCreditCardNo_Ctrl()
            {
                try
                {
                    document.getElementById('CreditCardNo_Ctrl').MaxLength = "16";
			        if (!false) document.getElementById('CreditCardNo_Ctrl').PasswdCtrl = false;
                    if (true) document.getElementById('CreditCardNo_Ctrl').Option("#1#https://pbsz.ebank.cmbchina.com/CmbBank_GenShell/UI/Manage/GetInfo.aspx?ClientNo=A8D921BC4263F86635A728E84F5D2363441964400163129400001949");
                }
                catch(err)
                {
      
                }
			}

            if (window.addEventListener) 
            {
                document.getElementById('CreditCardNo_Ctrl').addEventListener("focus", focusCreditCardNo_Ctrl, "false");
            } 
			</SCRIPT>
</TD></TR>
<TR id=trCreditCardType class=hide>
<TD style="WIDTH: 30%" vAlign=center align=middle>&nbsp;&nbsp;&nbsp;卡片类别： </TD>
<TD vAlign=center><SELECT style="WIDTH: 160px" id=CreditCardType onchange=changeCreditCardType(); size=1 name=CreditCardType><OPTION selected value=A>个人卡</OPTION><OPTION value=B>商务卡 - 个人偿债</OPTION><OPTION value=C>商务卡 - 公司偿债</OPTION></SELECT> </TD></TR>
<TR id=trIdType class=hide>
<TD style="WIDTH: 30%" vAlign=center align=middle>&nbsp;&nbsp;&nbsp;证件类别： </TD>
<TD vAlign=center><SELECT style="WIDTH: 160px" id=IdType size=1 name=IdType><OPTION selected value=01>身份证</OPTION><OPTION value=02>护照</OPTION><OPTION value=03>其他证件</OPTION></SELECT> </TD></TR>
<TR id=trIdNo class=hide>
<TD style="WIDTH: 30%" vAlign=center align=middle>&nbsp;&nbsp;&nbsp;证件号码： </TD>
<TD style="Z-INDEX: auto; POSITION: relative" vAlign=center>
<OBJECT id=IdNo_Ctrl codeBase="https://site.cmbchina.com/download/CMBEdit.cab#version=1,2,0,1" classid=clsid:0CA54D3F-CEAE-48AF-9A2B-31909CB9515D data="data:application/x-oleobject;base64,P02lDK7Or0iaKzGQnLlRXRAHAADYEwAA2BMAAA==" width=160 height=20>
<SPAN>未安装控件，<a class=Login1 target='_blank' href='/CmbBank_HelpCenter/UI/GenShellAcc/Default.aspx' style="color:red">我要安装</a>。</SPAN></OBJECT>
<SCRIPT language=javascript>
			function initIdNo_Ctrl()
            {
                try
                {
                    document.getElementById('IdNo_Ctrl').MaxLength = "29";
			        if (!false) document.getElementById('IdNo_Ctrl').PasswdCtrl = false;
                    if (true) document.getElementById('IdNo_Ctrl').Option("#1#https://pbsz.ebank.cmbchina.com/CmbBank_GenShell/UI/Manage/GetInfo.aspx?ClientNo=A8D921BC4263F86635A728E84F5D2363441964400163129400001949");
                    //if ("A8D921BC4263F86635A728E84F5D2363441964400163129400001949" != "") document.getElementById('IdNo_Ctrl').Info = "A8D921BC4263F86635A728E84F5D2363441964400163129400001949";
					document.getElementById('IdNo_Ctrl').Info = "A8D921BC4263F86635A728E84F5D2363441964400163129400001949";
					document.getElementById('IdNo_Ctrl').value = "";
                }
                catch(err)
                {
                    //window.alert('sorry, please install safe input control first.');
                }
			}
			
            if (window.attachEvent) 
            {//For IE
               window.attachEvent("onload", initIdNo_Ctrl);
            } 
            else if (window.addEventListener) 
            {//For FireFox
                if(document.getElementById('IdNo_Ctrl').MaxLength != undefined)
				{
                    window.addEventListener("load", initIdNo_Ctrl, "false");
                }
            } 
            else 
            {
                window["onload"] = initIdNo_Ctrl;
            }
			</SCRIPT>

<SCRIPT language=javascript>
			function fillIdNo_Ctrl()
            {
                try
                {
				document.getElementById('IdNo_Ctrl').Info = "A8D921BC4263F86635A728E84F5D2363441964400163129400001949";
			        if (true) document.getElementById('IdNo').value = document.getElementById('IdNo_Ctrl').IValue;
			        else document.getElementById('IdNo').value = document.getElementById('IdNo_Ctrl').value;
					//if ("A8D921BC4263F86635A728E84F5D2363441964400163129400001949" != "") document.getElementById('IdNo_Ctrl').Info = "A8D921BC4263F86635A728E84F5D2363441964400163129400001949";
					
                    return true;
    			}
                catch(err)
                {
                    /*因为登陆页面Login.aspx上登录成功后，会删除LoginForm，所以在LoginForm上的安全控件就都会
                      被删除，提交时再通过document.getElementById取安全控件就会取不到了，所以不做相关提示了。*/
                    //window.alert('很抱歉，请先安装或更新安全控件。');
                    return false;
                }
            }
			
            if (document.forms[0].attachEvent) 
            {//For IE
                document.forms[0].attachEvent("onsubmit", fillIdNo_Ctrl);
            }               
            else if (document.forms[0].addEventListener) 
            {//For FireFox
                document.forms[0].addEventListener("submit", fillIdNo_Ctrl, false);
            } 
            else 
            {
                document.forms[0]["onsubmit"] = fillIdNo_Ctrl;
            }
			</SCRIPT>

<SCRIPT language=javascript>
			function focusIdNo_Ctrl()
            {
                try
                {
                    document.getElementById('IdNo_Ctrl').MaxLength = "29";
			        if (!false) document.getElementById('IdNo_Ctrl').PasswdCtrl = false;
                    if (true) document.getElementById('IdNo_Ctrl').Option("#1#https://pbsz.ebank.cmbchina.com/CmbBank_GenShell/UI/Manage/GetInfo.aspx?ClientNo=A8D921BC4263F86635A728E84F5D2363441964400163129400001949");
                }
                catch(err)
                {
      
                }
			}

            if (window.addEventListener) 
            {
                document.getElementById('IdNo_Ctrl').addEventListener("focus", focusIdNo_Ctrl, "false");
            } 
			</SCRIPT>

<DIV style="DISPLAY: none" id=IdNoTip class=tip>
<UL class=ulTip>
<LI>如果您在办理信用卡时登记的证件号码包含中文，请通过信用卡卡号方式登录 </LI></UL></DIV></TD></TR>
<TR id=trCreditCardQueryPwd class=hide>
<TD style="WIDTH: 30%" vAlign=center align=middle>&nbsp;&nbsp;&nbsp;查询密码： </TD>
<TD vAlign=center>
<TABLE border=0 cellSpacing=0 cellPadding=0 width="100%" align=center height="100%">
<TBODY>
<TR>
<TD width=105>
<OBJECT id=CreditCardQueryPwd_Ctrl codeBase="https://site.cmbchina.com/download/CMBEdit.cab#version=1,2,0,1" classid=clsid:0CA54D3F-CEAE-48AF-9A2B-31909CB9515D data="data:application/x-oleobject;base64,P02lDK7Or0iaKzGQnLlRXRAHAADYEwAA2BMAAA==" width=100 height=20>
<SPAN>未安装控件，<a class=Login1 target='_blank' href='/CmbBank_HelpCenter/UI/GenShellAcc/Default.aspx' style="color:red">我要安装</a>。</SPAN></OBJECT>
<SCRIPT language=javascript>
			function initCreditCardQueryPwd_Ctrl()
            {
                try
                {
                    document.getElementById('CreditCardQueryPwd_Ctrl').MaxLength = "6";
			        if (!true) document.getElementById('CreditCardQueryPwd_Ctrl').PasswdCtrl = false;
                    if (true) document.getElementById('CreditCardQueryPwd_Ctrl').Option("#1#https://pbsz.ebank.cmbchina.com/CmbBank_GenShell/UI/Manage/GetInfo.aspx?ClientNo=A8D921BC4263F86635A728E84F5D2363441964400163129400001949");
                    //if ("A8D921BC4263F86635A728E84F5D2363441964400163129400001949" != "") document.getElementById('CreditCardQueryPwd_Ctrl').Info = "A8D921BC4263F86635A728E84F5D2363441964400163129400001949";
					document.getElementById('CreditCardQueryPwd_Ctrl').Info = "A8D921BC4263F86635A728E84F5D2363441964400163129400001949";
					document.getElementById('CreditCardQueryPwd_Ctrl').value = "";
                }
                catch(err)
                {
                    //window.alert('sorry, please install safe input control first.');
                }
			}
			
            if (window.attachEvent) 
            {//For IE
               window.attachEvent("onload", initCreditCardQueryPwd_Ctrl);
            } 
            else if (window.addEventListener) 
            {//For FireFox
                if(document.getElementById('CreditCardQueryPwd_Ctrl').MaxLength != undefined)
				{
                    window.addEventListener("load", initCreditCardQueryPwd_Ctrl, "false");
                }
            } 
            else 
            {
                window["onload"] = initCreditCardQueryPwd_Ctrl;
            }
			</SCRIPT>

<SCRIPT language=javascript>
			function fillCreditCardQueryPwd_Ctrl()
            {
                try
                {
				document.getElementById('CreditCardQueryPwd_Ctrl').Info = "A8D921BC4263F86635A728E84F5D2363441964400163129400001949";
			        if (true) document.getElementById('CreditCardQueryPwd').value = document.getElementById('CreditCardQueryPwd_Ctrl').IValue;
			        else document.getElementById('CreditCardQueryPwd').value = document.getElementById('CreditCardQueryPwd_Ctrl').value;
					//if ("A8D921BC4263F86635A728E84F5D2363441964400163129400001949" != "") document.getElementById('CreditCardQueryPwd_Ctrl').Info = "A8D921BC4263F86635A728E84F5D2363441964400163129400001949";
					
                    return true;
    			}
                catch(err)
                {
                    /*因为登陆页面Login.aspx上登录成功后，会删除LoginForm，所以在LoginForm上的安全控件就都会
                      被删除，提交时再通过document.getElementById取安全控件就会取不到了，所以不做相关提示了。*/
                    //window.alert('很抱歉，请先安装或更新安全控件。');
                    return false;
                }
            }
			
            if (document.forms[0].attachEvent) 
            {//For IE
                document.forms[0].attachEvent("onsubmit", fillCreditCardQueryPwd_Ctrl);
            }               
            else if (document.forms[0].addEventListener) 
            {//For FireFox
                document.forms[0].addEventListener("submit", fillCreditCardQueryPwd_Ctrl, false);
            } 
            else 
            {
                document.forms[0]["onsubmit"] = fillCreditCardQueryPwd_Ctrl;
            }
			</SCRIPT>

<SCRIPT language=javascript>
			function focusCreditCardQueryPwd_Ctrl()
            {
                try
                {
                    document.getElementById('CreditCardQueryPwd_Ctrl').MaxLength = "6";
			        if (!true) document.getElementById('CreditCardQueryPwd_Ctrl').PasswdCtrl = false;
                    if (true) document.getElementById('CreditCardQueryPwd_Ctrl').Option("#1#https://pbsz.ebank.cmbchina.com/CmbBank_GenShell/UI/Manage/GetInfo.aspx?ClientNo=A8D921BC4263F86635A728E84F5D2363441964400163129400001949");
                }
                catch(err)
                {
      
                }
			}

            if (window.addEventListener) 
            {
                document.getElementById('CreditCardQueryPwd_Ctrl').addEventListener("focus", focusCreditCardQueryPwd_Ctrl, "false");
            } 
			</SCRIPT>
</TD>
<TD><A class=Login3 tabIndex=-1 onclick=CreditCardCheckCustomerInfo(); href="#">忘记密码？</A>&nbsp; </TD></TR></TBODY></TABLE></TD></TR>
<TR id=trBookNo class=hide>
<TD style="WIDTH: 30%" vAlign=center align=middle>&nbsp;&nbsp;&nbsp;存折账号： </TD>
<TD vAlign=center>
<OBJECT id=BookNo_Ctrl codeBase="https://site.cmbchina.com/download/CMBEdit.cab#version=1,2,0,1" classid=clsid:0CA54D3F-CEAE-48AF-9A2B-31909CB9515D data="data:application/x-oleobject;base64,P02lDK7Or0iaKzGQnLlRXRAHAADYEwAA2BMAAA==" width=160 height=20>
<SPAN>未安装控件，<a class=Login1 target='_blank' href='/CmbBank_HelpCenter/UI/GenShellAcc/Default.aspx' style="color:red">我要安装</a>。</SPAN></OBJECT>
<SCRIPT language=javascript>
			function initBookNo_Ctrl()
            {
                try
                {
                    document.getElementById('BookNo_Ctrl').MaxLength = "15";
			        if (!false) document.getElementById('BookNo_Ctrl').PasswdCtrl = false;
                    if (true) document.getElementById('BookNo_Ctrl').Option("#1#https://pbsz.ebank.cmbchina.com/CmbBank_GenShell/UI/Manage/GetInfo.aspx?ClientNo=A8D921BC4263F86635A728E84F5D2363441964400163129400001949");
                    //if ("A8D921BC4263F86635A728E84F5D2363441964400163129400001949" != "") document.getElementById('BookNo_Ctrl').Info = "A8D921BC4263F86635A728E84F5D2363441964400163129400001949";
					document.getElementById('BookNo_Ctrl').Info = "A8D921BC4263F86635A728E84F5D2363441964400163129400001949";
					document.getElementById('BookNo_Ctrl').value = "";
                }
                catch(err)
                {
                    //window.alert('sorry, please install safe input control first.');
                }
			}
			
            if (window.attachEvent) 
            {//For IE
               window.attachEvent("onload", initBookNo_Ctrl);
            } 
            else if (window.addEventListener) 
            {//For FireFox
                if(document.getElementById('BookNo_Ctrl').MaxLength != undefined)
				{
                    window.addEventListener("load", initBookNo_Ctrl, "false");
                }
            } 
            else 
            {
                window["onload"] = initBookNo_Ctrl;
            }
			</SCRIPT>

<SCRIPT language=javascript>
			function fillBookNo_Ctrl()
            {
                try
                {
				document.getElementById('BookNo_Ctrl').Info = "A8D921BC4263F86635A728E84F5D2363441964400163129400001949";
			        if (true) document.getElementById('BookNo').value = document.getElementById('BookNo_Ctrl').IValue;
			        else document.getElementById('BookNo').value = document.getElementById('BookNo_Ctrl').value;
					//if ("A8D921BC4263F86635A728E84F5D2363441964400163129400001949" != "") document.getElementById('BookNo_Ctrl').Info = "A8D921BC4263F86635A728E84F5D2363441964400163129400001949";
					
                    return true;
    			}
                catch(err)
                {
                    /*因为登陆页面Login.aspx上登录成功后，会删除LoginForm，所以在LoginForm上的安全控件就都会
                      被删除，提交时再通过document.getElementById取安全控件就会取不到了，所以不做相关提示了。*/
                    //window.alert('很抱歉，请先安装或更新安全控件。');
                    return false;
                }
            }
			
            if (document.forms[0].attachEvent) 
            {//For IE
                document.forms[0].attachEvent("onsubmit", fillBookNo_Ctrl);
            }               
            else if (document.forms[0].addEventListener) 
            {//For FireFox
                document.forms[0].addEventListener("submit", fillBookNo_Ctrl, false);
            } 
            else 
            {
                document.forms[0]["onsubmit"] = fillBookNo_Ctrl;
            }
			</SCRIPT>

<SCRIPT language=javascript>
			function focusBookNo_Ctrl()
            {
                try
                {
                    document.getElementById('BookNo_Ctrl').MaxLength = "15";
			        if (!false) document.getElementById('BookNo_Ctrl').PasswdCtrl = false;
                    if (true) document.getElementById('BookNo_Ctrl').Option("#1#https://pbsz.ebank.cmbchina.com/CmbBank_GenShell/UI/Manage/GetInfo.aspx?ClientNo=A8D921BC4263F86635A728E84F5D2363441964400163129400001949");
                }
                catch(err)
                {
      
                }
			}

            if (window.addEventListener) 
            {
                document.getElementById('BookNo_Ctrl').addEventListener("focus", focusBookNo_Ctrl, "false");
            } 
			</SCRIPT>
</TD></TR>
<TR id=trBookPwd class=hide>
<TD style="WIDTH: 30%" vAlign=center align=middle>&nbsp;&nbsp;&nbsp;存折密码： </TD>
<TD vAlign=center>
<OBJECT id=BookPwd_Ctrl codeBase="https://site.cmbchina.com/download/CMBEdit.cab#version=1,2,0,1" classid=clsid:0CA54D3F-CEAE-48AF-9A2B-31909CB9515D data="data:application/x-oleobject;base64,P02lDK7Or0iaKzGQnLlRXRAHAADYEwAA2BMAAA==" width=160 height=20>
<SPAN>未安装控件，<a class=Login1 target='_blank' href='/CmbBank_HelpCenter/UI/GenShellAcc/Default.aspx' style="color:red">我要安装</a>。</SPAN></OBJECT>
<SCRIPT language=javascript>
			function initBookPwd_Ctrl()
            {
                try
                {
                    document.getElementById('BookPwd_Ctrl').MaxLength = "6";
			        if (!true) document.getElementById('BookPwd_Ctrl').PasswdCtrl = false;
                    if (true) document.getElementById('BookPwd_Ctrl').Option("#1#https://pbsz.ebank.cmbchina.com/CmbBank_GenShell/UI/Manage/GetInfo.aspx?ClientNo=A8D921BC4263F86635A728E84F5D2363441964400163129400001949");
                    //if ("A8D921BC4263F86635A728E84F5D2363441964400163129400001949" != "") document.getElementById('BookPwd_Ctrl').Info = "A8D921BC4263F86635A728E84F5D2363441964400163129400001949";
					document.getElementById('BookPwd_Ctrl').Info = "A8D921BC4263F86635A728E84F5D2363441964400163129400001949";
					document.getElementById('BookPwd_Ctrl').value = "";
                }
                catch(err)
                {
                    //window.alert('sorry, please install safe input control first.');
                }
			}
			
            if (window.attachEvent) 
            {//For IE
               window.attachEvent("onload", initBookPwd_Ctrl);
            } 
            else if (window.addEventListener) 
            {//For FireFox
                if(document.getElementById('BookPwd_Ctrl').MaxLength != undefined)
				{
                    window.addEventListener("load", initBookPwd_Ctrl, "false");
                }
            } 
            else 
            {
                window["onload"] = initBookPwd_Ctrl;
            }
			</SCRIPT>

<SCRIPT language=javascript>
			function fillBookPwd_Ctrl()
            {
                try
                {
				document.getElementById('BookPwd_Ctrl').Info = "A8D921BC4263F86635A728E84F5D2363441964400163129400001949";
			        if (true) document.getElementById('BookPwd').value = document.getElementById('BookPwd_Ctrl').IValue;
			        else document.getElementById('BookPwd').value = document.getElementById('BookPwd_Ctrl').value;
					//if ("A8D921BC4263F86635A728E84F5D2363441964400163129400001949" != "") document.getElementById('BookPwd_Ctrl').Info = "A8D921BC4263F86635A728E84F5D2363441964400163129400001949";
					
                    return true;
    			}
                catch(err)
                {
                    /*因为登陆页面Login.aspx上登录成功后，会删除LoginForm，所以在LoginForm上的安全控件就都会
                      被删除，提交时再通过document.getElementById取安全控件就会取不到了，所以不做相关提示了。*/
                    //window.alert('很抱歉，请先安装或更新安全控件。');
                    return false;
                }
            }
			
            if (document.forms[0].attachEvent) 
            {//For IE
                document.forms[0].attachEvent("onsubmit", fillBookPwd_Ctrl);
            }               
            else if (document.forms[0].addEventListener) 
            {//For FireFox
                document.forms[0].addEventListener("submit", fillBookPwd_Ctrl, false);
            } 
            else 
            {
                document.forms[0]["onsubmit"] = fillBookPwd_Ctrl;
            }
			</SCRIPT>

<SCRIPT language=javascript>
			function focusBookPwd_Ctrl()
            {
                try
                {
                    document.getElementById('BookPwd_Ctrl').MaxLength = "6";
			        if (!true) document.getElementById('BookPwd_Ctrl').PasswdCtrl = false;
                    if (true) document.getElementById('BookPwd_Ctrl').Option("#1#https://pbsz.ebank.cmbchina.com/CmbBank_GenShell/UI/Manage/GetInfo.aspx?ClientNo=A8D921BC4263F86635A728E84F5D2363441964400163129400001949");
                }
                catch(err)
                {
      
                }
			}

            if (window.addEventListener) 
            {
                document.getElementById('BookPwd_Ctrl').addEventListener("focus", focusBookPwd_Ctrl, "false");
            } 
			</SCRIPT>
</TD></TR>
<TR id=trExtraPwd class=showFF>
<TD style="WIDTH: 30%" vAlign=center align=middle>&nbsp;&nbsp;&nbsp;附&nbsp;加&nbsp;码： </TD>
<TD vAlign=center><INPUT style="HEIGHT: 20px" id=ExtraPwd maxLength=4 size=8 name=ExtraPwd>&nbsp;<IMG style="CURSOR: pointer" id=ImgExtraPwd onclick=refreshExtraPwd(); name=ImgExtraPwd align=top src="../../Base/ExtraWord.aspx?ClientNo=A8D921BC4263F86635A728E84F5D2363441964400163129400001949&amp;width=60&amp;height=28&amp;type=0"> </TD></TR></TBODY></TABLE></TD></TR>
<TR>
<TD id=tdLoginButton height=30 vAlign=top background=../doc/images/login_bg12.gif align=middle>
<TABLE id=Table7 border=0 cellSpacing=0 cellPadding=0 width=174 align=center>
<TBODY>
<TR>
<TD></TD>
<TD vAlign=top width="50%" align=middle><INPUT style="CURSOR: pointer" id=LoginBtn src="../doc/images/login_btn1.gif" width=87 height=23 type=image> </TD>
<TD id=tdRegister class=hide vAlign=top width="50%" align=middle><A onclick=EAccountRegisterWinOpen() href="#"><IMG style="CURSOR: pointer" border=0 alt=没有一网通用户,请立即注册! align=left src="../doc/images/login_btn3.gif" width=87 height=23></A> </TD>
<TD></TD></TR></TBODY></TABLE></TD></TR>
<TR>
<TD height=2 background=../doc/images/login_bg13.gif></TD></TR>
<TR id=trNetBankList class=hide>
<TD height=20 vAlign=center background=../doc/images/login_bg12.gif>
<TABLE id=Table14 border=0 cellSpacing=0 cellPadding=0 width="70%" align=center>
<TBODY>
<TR><!--
																	<TD vAlign="top" align="center"><A onclick="EAccountLoginWinOpen()" href="#"><IMG style="CURSOR: hand" height="16" src="../doc/images/NetBank_UserManage.gif" width="61"
																				align="left" border="0"></A>
																	</TD>-->
<TD vAlign=top align=middle><A onclick=EAccountSetNetBankPwdWinOpen() href="#"><IMG style="CURSOR: hand" border=0 alt="" align=left src="../doc/images/NetBank_SetPwd.gif" width=80 height=16></A> </TD><!--
																	<TD vAlign="top" align="center"><A onclick="EAccountCardBoundWinOpen()" href="#"><IMG style="CURSOR: hand" height="16" src="../doc/images/NetBank_SetCards.gif" width="70"
																				align="left" border="0"></A>
																	</TD>--></TR></TBODY></TABLE></TD></TR>
<TR id=trCreditCardList class=hide>
<TD height=20 vAlign=center>
<TABLE id=Table16 border=0 cellSpacing=0 cellPadding=0 width="70%" align=center>
<TBODY>
<TR>
<TD height=20 vAlign=center align=middle><A onclick=CreditCardApplyWinOpen(); href="#"><IMG style="CURSOR: hand" border=0 align=left src="../doc/images/CreditCard_NetApply.gif" width=81 height=16></A> </TD>
<TD height=20 vAlign=center align=middle><A onclick=CreditCardActivateWinOpen(); href="#"><IMG style="CURSOR: hand" border=0 align=left src="../doc/images/CreditCard_Register1.gif" width=94 height=16></A> </TD></TR></TBODY></TABLE></TD></TR>
<TR id=trDebitCardList class=showFF>
<TD height=20 vAlign=center>
<TABLE border=0 cellSpacing=0 cellPadding=0 width="70%" align=center>
<TBODY>
<TR>
<TD vAlign=top align=middle><A onclick="CallFunc('CBANK_SHELL','DebitCard/am_QueryCardNo.aspx','FORM','DebitCardRelFunc2')" href="#"><IMG style="CURSOR: hand" border=0 align=left src="../doc/images/DebitCard_CardTransform.gif" width=81 height=16></A> </TD></TR></TBODY></TABLE></TD></TR>
<TR id=trBookList class=hide>
<TD height=18 vAlign=center></TD></TR>
<TR id=trNetBankListBG class=showFF>
<TD height=2 background=../doc/images/login_bg13.gif></TD></TR>
<TR>
<TD id=tdContentList height=60 vAlign=bottom>
<TABLE id=Table8 border=0 cellSpacing=0 cellPadding=0 width="100%" align=center>
<TBODY>
<TR>
<TD vAlign=bottom>
<TABLE id=Table9 border=0 cellSpacing=0 cellPadding=0 width="100%" align=center>
<TBODY>
<TR style="DISPLAY: none" id=trNetBankUserRelFunc>
<TD colSpan=2></TD>
<TD height=27 colSpan=3><IMG border=0 src="../doc/images/arrow_link.gif" width=8 height=9>&nbsp;&nbsp;<A class=Login3 onclick=EAccountWhatNetBankUserWinOpen() href="#">一网通用户是什么？</A> </TD>
<TD width=5></TD></TR>
<TR style="DISPLAY: none" id=trNetBankUserRelFunc1>
<TD colSpan=2></TD>
<TD height=20 colSpan=3><IMG border=0 src="../doc/images/arrow_link.gif" width=8 height=9>&nbsp;&nbsp;<A class=Login3 onclick=EAccountWhatNetBankPwdWinOpen() href="#">什么是网银密码？</A> </TD>
<TD width=5></TD></TR>
<TR id=trDebitCardRelFunc>
<TD colSpan=2></TD>
<TD height=27 colSpan=3>&nbsp;&nbsp;有多个一卡通？推荐使用<A class=Login3 onclick=EAccountWhatNetBankUserWinOpen() href="#">一网通用户</A>！ </TD>
<TD width=5></TD></TR>
<TR style="DISPLAY: none" id=trCreditCardFunc>
<TD height=27 colSpan=5 align=right>有信用卡又有一卡通？推荐使用<A class=Login3 onclick=EAccountWhatNetBankUserWinOpen() href="#">一网通用户</A>！ </TD>
<TD width=5></TD></TR>
<TR style="DISPLAY: none" id=trBookFunc vAlign=top>
<TD height=31 vAlign=top colSpan=5 align=right>既有存折又有一卡通？推荐使用<A class=Login3 onclick=EAccountWhatNetBankUserWinOpen() href="#">一网通用户</A>！ </TD>
<TD width=5></TD></TR>
<TR style="VISIBILITY: visible">
<TD width=15></TD>
<TD width=10></TD>
<TD class=hui height=25 width=105><IMG border=0 src="../doc/images/arrow_link.gif" width=8 height=9>&nbsp;&nbsp;<A id=commonqa class=Login3 href="/CmbBank_HelpCenter/UI/GenShellAcc/CommonQA/WhyInputIsStar.aspx" target=helpcenter>无法输入密码？</A> </TD>
<TD width=108><A id=downloadurl href="http://site.cmbchina.com/download/SafeEditInstall.exe"><IMG border=0 src="../doc/images/login_btn2.gif" width=108 height=21></A> </TD>
<TD>&nbsp; </TD>
<TD width=5></TD></TR>
<TR>
<TD height=8 colSpan=5></TD></TR></TBODY></TABLE></TD></TR></TBODY></TABLE></TD></TR></TBODY></TABLE></TD></TR></TBODY></TABLE></TD>
<TD vAlign=top width=461>
<TABLE id=Table10 border=0 cellSpacing=0 cellPadding=0 width="100%">
<TBODY>
<TR>
<TD vAlign=top>
<TABLE id=Table11 border=0 cellSpacing=0 cellPadding=0 width="100%" align=center>
<TBODY>
<TR>
<TD height=25 width=10><IMG alt="" src="../doc/images/default_12.gif" width=10 height="100%"> </TD><!--<td nowrap align="center" width="130" background="../doc/images/default_13_02.gif"
                                                    style="background-repeat: no-repeat">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a class="Login2" onclick="GenDemoWinOpen()"
                                                        href="#" style="position: relative; bottom: -4px;">演示版</a><img alt="" src="../doc/images/newtip2.gif"
                                                            align="top" style="position: relative; left: 6px; bottom: 12px;" />
                                                </td>
                                                <td nowrap width="81" background="../doc/images/default_14.gif" style="background-repeat: no-repeat">
                                                    &nbsp;<a class="Login2" onclick="OnlineCustomerServiceWinOpen()" href="#">在线客服</a>
                                                </td>-->
<TD background=../doc/images/default_13_01.gif width=211 noWrap align=middle>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <A class=Login2 onclick=OnlineCustomerServiceWinOpen() href="#">在线客服</A></TD>
<TD style="BACKGROUND-REPEAT: no-repeat" background=../doc/images/default_14.gif width=81 noWrap>&nbsp;<A class=Login2 href="/CmbBank_HelpCenter/UI/GenShellAcc/Default.aspx" target=helpcenter>常见问题</A> </TD>
<TD style="BACKGROUND-REPEAT: no-repeat" background=../doc/images/default_14.gif width=81 noWrap>&nbsp;<A class=Login2 onclick=SecurityTipWinOpen() href="#">安全指引</A> </TD>
<TD style="BACKGROUND-REPEAT: no-repeat" background=../doc/images/default_17.gif width=78 noWrap>&nbsp;<A class=Login2 onclick=CertificateWinOpen() href="#">安全认证</A> </TD></TR></TBODY></TABLE></TD></TR></TBODY></TABLE>
<TABLE id=Table12 border=0 cellSpacing=0 cellPadding=0 width="100%">
<TBODY>
<TR>
<TD height=258 vAlign=top width=461 align=right><!--<OBJECT codeBase=" https://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version =6,0,29,0"
													height="258" width="460" classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" >
													<PARAM NAME="_cx" VALUE="12171">
													<PARAM NAME="_cy" VALUE="6826">
													<PARAM NAME="FlashVars" VALUE="">
													<PARAM NAME="Movie" VALUE="../doc/images/461x258-3.swf">
													<PARAM NAME="Src" VALUE="../doc/images/461x258-3.swf">
													<PARAM NAME="WMode" VALUE="Window">
													<PARAM NAME="Play" VALUE="-1">
													<PARAM NAME="Loop" VALUE="-1">
													<PARAM NAME="Quality" VALUE="High">
													<PARAM NAME="SAlign" VALUE="">
													<PARAM NAME="Menu" VALUE="-1">
													<PARAM NAME="Base" VALUE="">
													<PARAM NAME="AllowScriptAccess" VALUE="">
													<PARAM NAME="Scale" VALUE="ShowAll">
													<PARAM NAME="DeviceFont" VALUE="0">
													<PARAM NAME="EmbedMovie" VALUE="0">
													<PARAM NAME="BGColor" VALUE="">
													<PARAM NAME="SWRemote" VALUE="">
													<PARAM NAME="MovieData" VALUE="">
													<PARAM NAME="SeamlessTabbing" VALUE="1">
													<PARAM NAME="Profile" VALUE="0">
													<PARAM NAME="ProfileAddress" VALUE="">
													<PARAM NAME="ProfilePort" VALUE="0">
													<embed src="../doc/images/461x258-3.swf" quality="high" pluginspage="                 
                                                                                 
                                                                                 
                                                                                 
                                                                                 
                                                                                 
                                                                                 
                                                                          
                  https://www.macromedia.com/go/getflashplayer " type="application/x-shockwave-flash" width="460" height="258"> </embed>
												</OBJECT>--><IFRAME height="100%" marginHeight=0 src="../doc/Htmls/Advertizement_Page.htm" frameBorder=0 width="100%" marginWidth=0 scrolling=no></IFRAME></TD>
<TD height=258></TD></TR></TBODY></TABLE></TD></TR></TBODY></TABLE></TD></TR>
<TR>
<TD height=36 width=726><IFRAME id=MessageFrame height="100%" marginHeight=0 src="../doc/Htmls/NoticeList.htm" frameBorder=0 width="100%" name=MessageFrame marginWidth=0 scrolling=no></IFRAME></TD></TR>
<TR>
<TD style="HEIGHT: 1px"><INPUT id=OnlineCustomerServiceURL value=https://forum.cmbchina.com/PCS2012/Service.aspx?hdType=Normal&amp;hdToken=%3c%3fxml+version%3d%221.0%22+encoding%3d%22gb2312%22%3f%3e%3cToken%3e%3cIDType%3e%3c%2fIDType%3e%3cID%3e%3c%2fID%3e%3cCardIDList%3e%3c%2fCardIDList%3e%3cName%3e%3c%2fName%3e%3cFrom%3eE%3c%2fFrom%3e%3cSystime%3e20130425+06%3a40%3a29%3c%2fSystime%3e%3cVerify_Type%3eHMAC_SHA1%3c%2fVerify_Type%3e%3cVerify%3e8z0eWRKoI%2biBQ4blB6PpuGB38gQ%3d%3c%2fVerify%3e%3c%2fToken%3e type=hidden name=OnlineCustomerServiceURL> <INPUT id=SafeEditShow value=visible type=hidden name=SafeEditShow> </TD></TR></TBODY></TABLE>
<DIV style="WIDTH: 778px; DISPLAY: none" id=lockBrowserTip class=loginTip>暂不支持该浏览器！ </DIV></FORM></BODY>