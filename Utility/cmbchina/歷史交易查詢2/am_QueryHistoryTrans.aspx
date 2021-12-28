https://pbsz.ebank.cmbchina.com/CmbBank_PB/UI/PBPC/DebitCard_AccountManager/am_QueryHistoryTrans.aspx


<BODY style="OVERFLOW-X: auto; OVERFLOW-Y: auto" id=body onload=InitPage() MS_POSITIONING="GridLayout"><FORM id=am_QueryHistoryTrans method=post name=am_QueryHistoryTrans action=am_QueryHistoryTrans.aspx>
<DIV><INPUT id=__EVENTTARGET type=hidden name=__EVENTTARGET> <INPUT id=__EVENTARGUMENT type=hidden name=__EVENTARGUMENT> <INPUT id=__LASTFOCUS type=hidden name=__LASTFOCUS> <INPUT id=__VIEWSTATE value=/wEPDwUJNDI5MzY3Mjk5D2QWAgIDD2QWDgIBDw8WAh4EVGV4dAUb6LSm5oi3566h55CGID4g5Lqk5piT5p+l6K+iZGQCAw8WAh4HVmlzaWJsZWhkAgUPEGQQFQEQNjIyNTg4NTc5MDk4MDIyMRUBFUEwNTcxNjIyNTg4NTc5MDk4MDIyMRQrAwFnFgFmZAIHDw8WAh8BaGRkAggPDxYCHwFnZGQCCg8WAh8BZxYIAgEPEGQQFQEhIOa0u+acn+e7k+eul+aItyDkurrmsJHluIEgMDAwMDAgFQEPMTI1MjIyNDg1MDAwMDAyFCsDAWdkZAIDDxAPFgYeDkRhdGFWYWx1ZUZpZWxkBQNLZXkeDURhdGFUZXh0RmllbGQFBVZhbHVlHgtfIURhdGFCb3VuZGdkEBUFClsg5YWo6YOoIF0h572R6ZO26Leo6KGM5a6e5pe26L2s5Ye65omL57ut6LS5DOWuouaIt+i9rOi0pgzlrqLmiLfovazotKYM6KGM5YaF5rGH5YWlFQUBLQRGRUlQBElDUlIESUNTUARSWFRZFCsDBWdnZ2dnZGQCBg8WAh4Hb25jbGljawUVb3BlbldpbkJlZ2luRGF0ZVNlbCgpZAIJDxYCHwUFE29wZW5XaW5FbmREYXRlU2VsKClkAgwPFgIfAWcWCgIBDw8WAh8ABRA2MjI1ODg1NzkwOTgwMjIxZGQCBQ8PFgIfAAUCMjRkZAIHDw8WAh8ABQMxNTBkZAIJDw8WAh8ABQo0NDAsMTEwLjAwZGQCCw8PFgIfAAUKNDQwLDg3MS43NGRkZGj5iAh39hlmz2WLDyOMym3/xpf6 type=hidden name=__VIEWSTATE> </DIV>
<SCRIPT type=text/javascript>
//<![CDATA[
var theForm = document.forms['am_QueryHistoryTrans'];
if (!theForm) {
    theForm = document.am_QueryHistoryTrans;
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

<DIV><INPUT id=__EVENTVALIDATION value=/wEWDwKXsMjaAwLK5aCoBAKAwvDZDQKehae6BQLUmfTWAQKQm4i5DQLA+7iOCgLlgPW6DgKDqZ+uBwL6jt/ZDgLluv3tCgL9mpmPAQKSm5bSAwLm9cyQAgKU69TQBd8BwOe8iU0tZKkiEdEI68MnR2PX type=hidden name=__EVENTVALIDATION> </DIV>
<TABLE id=Table1 class=tbMain border=0 cellSpacing=0>
<TBODY>
<TR>
<TD class=tdCommonTop>
<TABLE id=Table2 class=tbTitle border=0 cellSpacing=0>
<TBODY>
<TR>
<TD class=tdTitle1>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;当前功能： <SPAN id=Location>账户管理 &gt; 交易查询</SPAN>&nbsp;&nbsp;&nbsp; </TD>
<TD class=tdTitle2><FONT face=宋体></FONT></TD>
<TD class=tdTitle3>版面号：101004<IMG alt="" src="../doc/Images/title_06.gif"></TD></TR></TBODY></TABLE>
<TABLE id=Table3 class=tbBlock border=0 cellSpacing=1>
<TBODY>
<TR>
<TD class=tdCenterW20H40>一卡通卡号：</TD>
<TD class=tdLeftH40><SPAN style="FLOAT: left"><SELECT id=ddlDebitCardList onchange="javascript:setTimeout('__doPostBack(\'ddlDebitCardList\',\'\')', 0)" name=ddlDebitCardList> <OPTION selected value=A05716225885790980221>6225885790980221</OPTION></SELECT> </SPAN></TD></TR></TBODY></TABLE></TD></TR>
<TR>
<TD class=tdSpaceH12><FONT face=宋体></FONT></TD></TR>
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
<TD class=tdPanelNoSel><A class=lkPanelNoSel onclick="triggerFunc('../DebitCard_AccountManager/am_QueryTodayTrans.aspx','FORM','_self')" href="#">当天交易查询</A></TD>
<TD class=tdPanelSel><A class=lkPanelSel onclick="triggerFunc('../DebitCard_AccountManager/am_QueryHistoryTrans.aspx','FORM','_self')" href="#">历史交易查询</A></TD></TR></TBODY></TABLE><!-- END 交易查询标签 --></TD>
<TD class=tdPanelTrail></TD></TR></TBODY></TABLE></TD></TR>
<TR>
<TD id=tdQueryResult1 class=tdCommonTop>
<TABLE id=Table5 class=tbCommonColor border=0 cellSpacing=0>
<TBODY>
<TR>
<TD></TD></TR></TBODY></TABLE>
<TABLE id=Table6 class=tbCommonColor border=0 cellSpacing=1>
<TBODY>
<TR>
<TD class=tdLeftH30>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;子 账 户：<SELECT style="WIDTH: 200px" id=ddlSubAccountList name=ddlSubAccountList> <OPTION selected value=125222485000002>活期结算户 人民币 00000</OPTION></SELECT> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;交易类型：<SELECT style="WIDTH: 100px" id=ddlTransTypeList name=ddlTransTypeList> <OPTION selected value=->[ 全部 ]</OPTION> <OPTION value=FEIP>网银跨行实时转出手续费</OPTION> <OPTION value=ICRR>客户转账</OPTION> <OPTION value=ICSP>客户转账</OPTION> <OPTION value=RXTY>行内汇入</OPTION></SELECT> </TD></TR>
<TR>
<TD class=tdLeftH30>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;起始日期：<INPUT style="WIDTH: 80px" id=BeginDate value=20130401 maxLength=8 name=BeginDate><IMG style="CURSOR: pointer" id=BtnBeginDate onclick=openWinBeginDateSel() src="../doc/Images/bt_selDate.gif"> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;终止日期：<INPUT style="WIDTH: 80px" id=EndDate value=20130425 maxLength=8 name=EndDate><IMG style="CURSOR: pointer" id=BtnEndDate onclick=openWinEndDateSel() src="../doc/Images/bt_selDate.gif"> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<INPUT id=BtnOK class=btn onclick="return CheckValid();" value="查 询" type=submit name=BtnOK> </TD></TR></TBODY></TABLE></TD></TR>
<TR>
<TD class=tdSpaceH12></TD></TR>
<TR>
<TD id=tdQueryResult2 class=tdCommonTop>
<TABLE id=Table8 class=tbBlock border=0 cellSpacing=1>
<TBODY>
<TR>
<TD class=tdPrompt>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;以下是一卡通&nbsp; <SPAN id=AccountNo class=wcLabel>6225885790980221</SPAN>&nbsp;的历史交易记录：</TD></TR></TBODY></TABLE>
<TABLE style="BORDER-BOTTOM: #9fd6ff 1px solid; BORDER-LEFT: #9fd6ff 1px solid; BORDER-COLLAPSE: collapse; BORDER-TOP: #9fd6ff 1px solid; BORDER-RIGHT: #9fd6ff 1px solid" id=dgHistoryTransRecSet class=dgMain border=1 rules=all cellSpacing=0 cellPadding=3 align=center>
<TBODY>
<TR class=dgHeader align=middle>
<TD style="WIDTH: 15%" class=dgHeader>交易日期</TD>
<TD style="WIDTH: 15%" class=dgHeader>支出</TD>
<TD style="WIDTH: 15%" class=dgHeader>存入</TD>
<TD style="WIDTH: 15%" class=dgHeader>余额</TD>
<TD style="WIDTH: 15%" class=dgHeader>交易类型</TD>
<TD style="WIDTH: 25%" class=dgHeader>交易备注</TD></TR>
<TR>
<TD align=middle>2013-04-01</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>1,500.21</TD>
<TD style="COLOR: #585858" align=right>2,447.75</TD>
<TD align=left>行内汇入</TD>
<TD align=left>孟静</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-01</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>5,000.52</TD>
<TD style="COLOR: #585858" align=right>7,448.27</TD>
<TD align=left>客户转账</TD>
<TD align=left>1 鲁鹏</TD></TR>
<TR>
<TD align=middle>2013-04-01</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>5,000.23</TD>
<TD style="COLOR: #585858" align=right>12,448.50</TD>
<TD align=left>客户转账</TD>
<TD align=left>1 鲁鹏</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-01</TD>
<TD align=right>12,400.00</TD>
<TD align=right>&nbsp;</TD>
<TD style="COLOR: #585858" align=right>48.50</TD>
<TD align=left>客户转账</TD>
<TD align=left>payment 阙寿堂</TD></TR>
<TR>
<TD align=middle>2013-04-01</TD>
<TD align=right>10.00</TD>
<TD align=right>&nbsp;</TD>
<TD style="COLOR: #585858" align=right>38.50</TD>
<TD align=left>网银跨行实时转出手续费</TD>
<TD align=left>payment 阙寿堂</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-01</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>700.64</TD>
<TD style="COLOR: #585858" align=right>739.14</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 刘亮</TD></TR>
<TR>
<TD align=middle>2013-04-01</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>5,975.34</TD>
<TD style="COLOR: #585858" align=right>6,714.48</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 傅晔</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-01</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>800.78</TD>
<TD style="COLOR: #585858" align=right>7,515.26</TD>
<TD align=left>客户转账</TD>
<TD align=left>daibaby 陈超群</TD></TR>
<TR>
<TD align=middle>2013-04-01</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>1,100.56</TD>
<TD style="COLOR: #585858" align=right>8,615.82</TD>
<TD align=left>客户转账</TD>
<TD align=left>daibaby 陈超群</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-01</TD>
<TD align=right>8,600.00</TD>
<TD align=right>&nbsp;</TD>
<TD style="COLOR: #585858" align=right>15.82</TD>
<TD align=left>客户转账</TD>
<TD align=left>payment 阙寿堂</TD></TR>
<TR>
<TD align=middle>2013-04-01</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>2,000.69</TD>
<TD style="COLOR: #585858" align=right>2,016.51</TD>
<TD align=left>客户转账</TD>
<TD align=left>daibaby 陈超群</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-01</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>5,000.87</TD>
<TD style="COLOR: #585858" align=right>7,017.38</TD>
<TD align=left>客户转账</TD>
<TD align=left>daibaby 陈超群</TD></TR>
<TR>
<TD align=middle>2013-04-02</TD>
<TD align=right>6,900.00</TD>
<TD align=right>&nbsp;</TD>
<TD style="COLOR: #585858" align=right>117.38</TD>
<TD align=left>客户转账</TD>
<TD align=left>payment 阙寿堂</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-04</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>5,000.10</TD>
<TD style="COLOR: #585858" align=right>5,117.48</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 徐礼庆</TD></TR>
<TR>
<TD align=middle>2013-04-04</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>100.68</TD>
<TD style="COLOR: #585858" align=right>5,218.16</TD>
<TD align=left>行内汇入</TD>
<TD align=left>永康支行 马广辉</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-04</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>1,100.47</TD>
<TD style="COLOR: #585858" align=right>6,318.63</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 刘亮</TD></TR>
<TR>
<TD align=middle>2013-04-04</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>100.45</TD>
<TD style="COLOR: #585858" align=right>6,419.08</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记 刘慧强</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-04</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>100.90</TD>
<TD style="COLOR: #585858" align=right>6,519.98</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记 刘慧强</TD></TR>
<TR>
<TD align=middle>2013-04-04</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>100.18</TD>
<TD style="COLOR: #585858" align=right>6,620.16</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记 刘慧强</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-04</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>100.29</TD>
<TD style="COLOR: #585858" align=right>6,720.45</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记 刘慧强</TD></TR>
<TR>
<TD align=middle>2013-04-04</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>100.64</TD>
<TD style="COLOR: #585858" align=right>6,821.09</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记 刘慧强</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-04</TD>
<TD align=right>6,800.00</TD>
<TD align=right>&nbsp;</TD>
<TD style="COLOR: #585858" align=right>21.09</TD>
<TD align=left>客户转账</TD>
<TD align=left>payment 阙寿堂</TD></TR>
<TR>
<TD align=middle>2013-04-04</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>1,000.89</TD>
<TD style="COLOR: #585858" align=right>1,021.98</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 颜斌</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-04</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>433.43</TD>
<TD style="COLOR: #585858" align=right>1,455.41</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 陈欢欢</TD></TR>
<TR>
<TD align=middle>2013-04-04</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>1,000.91</TD>
<TD style="COLOR: #585858" align=right>2,456.32</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 颜斌</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-04</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>1,000.02</TD>
<TD style="COLOR: #585858" align=right>3,456.34</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 颜斌</TD></TR>
<TR>
<TD align=middle>2013-04-04</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>1,000.75</TD>
<TD style="COLOR: #585858" align=right>4,457.09</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 颜斌</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-04</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>900.27</TD>
<TD style="COLOR: #585858" align=right>5,357.36</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 颜斌</TD></TR>
<TR>
<TD align=middle>2013-04-04</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>2,000.30</TD>
<TD style="COLOR: #585858" align=right>7,357.66</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 颜斌</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-04</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>1,000.89</TD>
<TD style="COLOR: #585858" align=right>8,358.55</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 颜斌</TD></TR>
<TR>
<TD align=middle>2013-04-04</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>498.48</TD>
<TD style="COLOR: #585858" align=right>8,857.03</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 陈欢欢</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-04</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>100.67</TD>
<TD style="COLOR: #585858" align=right>8,957.70</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记 刘慧强</TD></TR>
<TR>
<TD align=middle>2013-04-04</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>100.49</TD>
<TD style="COLOR: #585858" align=right>9,058.19</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记 刘慧强</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-04</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>100.60</TD>
<TD style="COLOR: #585858" align=right>9,158.79</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记 刘慧强</TD></TR>
<TR>
<TD align=middle>2013-04-04</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>100.91</TD>
<TD style="COLOR: #585858" align=right>9,259.70</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记 刘慧强</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-04</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>100.29</TD>
<TD style="COLOR: #585858" align=right>9,359.99</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记 刘慧强</TD></TR>
<TR>
<TD align=middle>2013-04-04</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>100.79</TD>
<TD style="COLOR: #585858" align=right>9,460.78</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记 刘慧强</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-04</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>100.80</TD>
<TD style="COLOR: #585858" align=right>9,561.58</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记 刘慧强</TD></TR>
<TR>
<TD align=middle>2013-04-04</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>100.61</TD>
<TD style="COLOR: #585858" align=right>9,662.19</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记 刘慧强</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-04</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>10,000.49</TD>
<TD style="COLOR: #585858" align=right>19,662.68</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 赖秋坛</TD></TR>
<TR>
<TD align=middle>2013-04-05</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>999.46</TD>
<TD style="COLOR: #585858" align=right>20,662.14</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 陈欢欢</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-05</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>501.25</TD>
<TD style="COLOR: #585858" align=right>21,163.39</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 陈欢欢</TD></TR>
<TR>
<TD align=middle>2013-04-05</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>100.24</TD>
<TD style="COLOR: #585858" align=right>21,263.63</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记 刘慧强</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-05</TD>
<TD align=right>21,200.00</TD>
<TD align=right>&nbsp;</TD>
<TD style="COLOR: #585858" align=right>63.63</TD>
<TD align=left>客户转账</TD>
<TD align=left>payment 阙寿堂</TD></TR>
<TR>
<TD align=middle>2013-04-05</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>260.02</TD>
<TD style="COLOR: #585858" align=right>323.65</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 刘东</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-05</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>100.18</TD>
<TD style="COLOR: #585858" align=right>423.83</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 刘慧强</TD></TR>
<TR>
<TD align=middle>2013-04-05</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>200.07</TD>
<TD style="COLOR: #585858" align=right>623.90</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 刘亮</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-06</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>499.68</TD>
<TD style="COLOR: #585858" align=right>1,123.58</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 陈欢欢</TD></TR>
<TR>
<TD align=middle>2013-04-06</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>10,000.06</TD>
<TD style="COLOR: #585858" align=right>11,123.64</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 赖秋坛</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-06</TD>
<TD align=right>11,100.00</TD>
<TD align=right>&nbsp;</TD>
<TD style="COLOR: #585858" align=right>23.64</TD>
<TD align=left>客户转账</TD>
<TD align=left>payment 阙寿堂</TD></TR>
<TR>
<TD align=middle>2013-04-07</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>5,900.41</TD>
<TD style="COLOR: #585858" align=right>5,924.05</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 戴建军</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-07</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>4,000.70</TD>
<TD style="COLOR: #585858" align=right>9,924.75</TD>
<TD align=left>客户转账</TD>
<TD align=left>转账 颜斌</TD></TR>
<TR>
<TD align=middle>2013-04-07</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>4,000.90</TD>
<TD style="COLOR: #585858" align=right>13,925.65</TD>
<TD align=left>客户转账</TD>
<TD align=left>转账 颜斌</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-07</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>4,000.24</TD>
<TD style="COLOR: #585858" align=right>17,925.89</TD>
<TD align=left>客户转账</TD>
<TD align=left>转账 颜斌</TD></TR>
<TR>
<TD align=middle>2013-04-08</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>100.71</TD>
<TD style="COLOR: #585858" align=right>18,026.60</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 刘慧强</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-08</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>100.99</TD>
<TD style="COLOR: #585858" align=right>18,127.59</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 刘慧强</TD></TR>
<TR>
<TD align=middle>2013-04-08</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>100.31</TD>
<TD style="COLOR: #585858" align=right>18,227.90</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 刘慧强</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-08</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>100.28</TD>
<TD style="COLOR: #585858" align=right>18,328.18</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 刘慧强</TD></TR>
<TR>
<TD align=middle>2013-04-08</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>4,000.76</TD>
<TD style="COLOR: #585858" align=right>22,328.94</TD>
<TD align=left>客户转账</TD>
<TD align=left>daibaby 陈淑慧</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-08</TD>
<TD align=right>22,300.00</TD>
<TD align=right>&nbsp;</TD>
<TD style="COLOR: #585858" align=right>28.94</TD>
<TD align=left>客户转账</TD>
<TD align=left>payment 阙寿堂</TD></TR>
<TR>
<TD align=middle>2013-04-08</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>15,000.16</TD>
<TD style="COLOR: #585858" align=right>15,029.10</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 赖秋坛</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-08</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>2,000.54</TD>
<TD style="COLOR: #585858" align=right>17,029.64</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 颜斌</TD></TR>
<TR>
<TD align=middle>2013-04-08</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>2,000.35</TD>
<TD style="COLOR: #585858" align=right>19,029.99</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 颜斌</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-08</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>2,000.48</TD>
<TD style="COLOR: #585858" align=right>21,030.47</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 颜斌</TD></TR>
<TR>
<TD align=middle>2013-04-08</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>2,000.41</TD>
<TD style="COLOR: #585858" align=right>23,030.88</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 颜斌</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-08</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>10,000.96</TD>
<TD style="COLOR: #585858" align=right>33,031.84</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 徐礼庆</TD></TR>
<TR>
<TD align=middle>2013-04-08</TD>
<TD align=right>33,000.00</TD>
<TD align=right>&nbsp;</TD>
<TD style="COLOR: #585858" align=right>31.84</TD>
<TD align=left>客户转账</TD>
<TD align=left>payment 阙寿堂</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-08</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>10,000.48</TD>
<TD style="COLOR: #585858" align=right>10,032.32</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 徐礼庆</TD></TR>
<TR>
<TD align=middle>2013-04-08</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>2,000.52</TD>
<TD style="COLOR: #585858" align=right>12,032.84</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 颜斌</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-08</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>1,000.66</TD>
<TD style="COLOR: #585858" align=right>13,033.50</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 颜斌</TD></TR>
<TR>
<TD align=middle>2013-04-09</TD>
<TD align=right>13,000.00</TD>
<TD align=right>&nbsp;</TD>
<TD style="COLOR: #585858" align=right>33.50</TD>
<TD align=left>客户转账</TD>
<TD align=left>payment 阙寿堂</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-10</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>1,000.54</TD>
<TD style="COLOR: #585858" align=right>1,034.04</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 颜斌</TD></TR>
<TR>
<TD align=middle>2013-04-10</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>1,000.42</TD>
<TD style="COLOR: #585858" align=right>2,034.46</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 颜斌</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-10</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>1,000.42</TD>
<TD style="COLOR: #585858" align=right>3,034.88</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 颜斌</TD></TR>
<TR>
<TD align=middle>2013-04-10</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>2,000.82</TD>
<TD style="COLOR: #585858" align=right>5,035.70</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 颜斌</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-11</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>200.13</TD>
<TD style="COLOR: #585858" align=right>5,235.83</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 刘冉</TD></TR>
<TR>
<TD align=middle>2013-04-11</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>5,000.48</TD>
<TD style="COLOR: #585858" align=right>10,236.31</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 颜斌</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-11</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>1,900.65</TD>
<TD style="COLOR: #585858" align=right>12,136.96</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 钱斌</TD></TR>
<TR>
<TD align=middle>2013-04-11</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>9,000.58</TD>
<TD style="COLOR: #585858" align=right>21,137.54</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 熊国端</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-11</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>15,000.00</TD>
<TD style="COLOR: #585858" align=right>36,137.54</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 熊国端</TD></TR>
<TR>
<TD align=middle>2013-04-11</TD>
<TD align=right>36,100.00</TD>
<TD align=right>&nbsp;</TD>
<TD style="COLOR: #585858" align=right>37.54</TD>
<TD align=left>客户转账</TD>
<TD align=left>payment 阙寿堂</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-11</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>100.67</TD>
<TD style="COLOR: #585858" align=right>138.21</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 李秀凤</TD></TR>
<TR>
<TD align=middle>2013-04-11</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>10,000.14</TD>
<TD style="COLOR: #585858" align=right>10,138.35</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 赖秋坛</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-12</TD>
<TD align=right>10,100.00</TD>
<TD align=right>&nbsp;</TD>
<TD style="COLOR: #585858" align=right>38.35</TD>
<TD align=left>客户转账</TD>
<TD align=left>payment 阙寿堂</TD></TR>
<TR>
<TD align=middle>2013-04-12</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>600.05</TD>
<TD style="COLOR: #585858" align=right>638.40</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 刘亮</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-12</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>9,900.05</TD>
<TD style="COLOR: #585858" align=right>10,538.45</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 赖秋坛</TD></TR>
<TR>
<TD align=middle>2013-04-12</TD>
<TD align=right>10,500.00</TD>
<TD align=right>&nbsp;</TD>
<TD style="COLOR: #585858" align=right>38.45</TD>
<TD align=left>客户转账</TD>
<TD align=left>payment 阙寿堂</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-12</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>4,999.73</TD>
<TD style="COLOR: #585858" align=right>5,038.18</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 戴建军</TD></TR>
<TR>
<TD align=middle>2013-04-12</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>390.55</TD>
<TD style="COLOR: #585858" align=right>5,428.73</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 刘亮</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-12</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>281.10</TD>
<TD style="COLOR: #585858" align=right>5,709.83</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 陈欢欢</TD></TR>
<TR>
<TD align=middle>2013-04-12</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>201.97</TD>
<TD style="COLOR: #585858" align=right>5,911.80</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 陈欢欢</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-13</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>1,300.91</TD>
<TD style="COLOR: #585858" align=right>7,212.71</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 戴建军</TD></TR>
<TR>
<TD align=middle>2013-04-13</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>488.39</TD>
<TD style="COLOR: #585858" align=right>7,701.10</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 钱卫明</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-13</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>300.92</TD>
<TD style="COLOR: #585858" align=right>8,002.02</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 钱卫明</TD></TR>
<TR>
<TD align=middle>2013-04-13</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>3,000.56</TD>
<TD style="COLOR: #585858" align=right>11,002.58</TD>
<TD align=left>行内汇入</TD>
<TD align=left>还款 王辉</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-13</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>1,000.48</TD>
<TD style="COLOR: #585858" align=right>12,003.06</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 陈欢欢</TD></TR>
<TR>
<TD align=middle>2013-04-13</TD>
<TD align=right>12,000.00</TD>
<TD align=right>&nbsp;</TD>
<TD style="COLOR: #585858" align=right>3.06</TD>
<TD align=left>客户转账</TD>
<TD align=left>payment 阙寿堂</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-13</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>790.71</TD>
<TD style="COLOR: #585858" align=right>793.77</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 陈欢欢</TD></TR>
<TR>
<TD align=middle>2013-04-14</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>600.28</TD>
<TD style="COLOR: #585858" align=right>1,394.05</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 刘亮</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-14</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>8,000.55</TD>
<TD style="COLOR: #585858" align=right>9,394.60</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 赖秋坛</TD></TR>
<TR>
<TD align=middle>2013-04-15</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>10,000.66</TD>
<TD style="COLOR: #585858" align=right>19,395.26</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 赖秋坛</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-15</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>13,000.70</TD>
<TD style="COLOR: #585858" align=right>32,395.96</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 赖秋坛</TD></TR>
<TR>
<TD align=middle>2013-04-15</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>4,995.38</TD>
<TD style="COLOR: #585858" align=right>37,391.34</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 赖秋坛</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-15</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>2,000.31</TD>
<TD style="COLOR: #585858" align=right>39,391.65</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 赖秋坛</TD></TR>
<TR>
<TD align=middle>2013-04-15</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>600.65</TD>
<TD style="COLOR: #585858" align=right>39,992.30</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 刘亮</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-15</TD>
<TD align=right>39,900.00</TD>
<TD align=right>&nbsp;</TD>
<TD style="COLOR: #585858" align=right>92.30</TD>
<TD align=left>客户转账</TD>
<TD align=left>payment 阙寿堂</TD></TR>
<TR>
<TD align=middle>2013-04-16</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>500.71</TD>
<TD style="COLOR: #585858" align=right>593.01</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 刘亮</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-16</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>4,000.76</TD>
<TD style="COLOR: #585858" align=right>4,593.77</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 郑兴</TD></TR>
<TR>
<TD align=middle>2013-04-16</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>9,500.28</TD>
<TD style="COLOR: #585858" align=right>14,094.05</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 熊国端</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-16</TD>
<TD align=right>14,000.00</TD>
<TD align=right>&nbsp;</TD>
<TD style="COLOR: #585858" align=right>94.05</TD>
<TD align=left>客户转账</TD>
<TD align=left>payment 阙寿堂</TD></TR>
<TR>
<TD align=middle>2013-04-16</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>200.08</TD>
<TD style="COLOR: #585858" align=right>294.13</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 刘亮</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-17</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>1,999.40</TD>
<TD style="COLOR: #585858" align=right>2,293.53</TD>
<TD align=left>行内汇入</TD>
<TD align=left>还款 王辉</TD></TR>
<TR>
<TD align=middle>2013-04-17</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>1,999.38</TD>
<TD style="COLOR: #585858" align=right>4,292.91</TD>
<TD align=left>行内汇入</TD>
<TD align=left>还款 王辉</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-17</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>1,990.60</TD>
<TD style="COLOR: #585858" align=right>6,283.51</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 钱卫明</TD></TR>
<TR>
<TD align=middle>2013-04-17</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>900.92</TD>
<TD style="COLOR: #585858" align=right>7,184.43</TD>
<TD align=left>客户转账</TD>
<TD align=left>111111 李洪雷</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-17</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>660.77</TD>
<TD style="COLOR: #585858" align=right>7,845.20</TD>
<TD align=left>行内汇入</TD>
<TD align=left>还款 王辉</TD></TR>
<TR>
<TD align=middle>2013-04-17</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>7,000.87</TD>
<TD style="COLOR: #585858" align=right>14,846.07</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 赖秋坛</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-17</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>10,000.85</TD>
<TD style="COLOR: #585858" align=right>24,846.92</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 赖秋坛</TD></TR>
<TR>
<TD align=middle>2013-04-17</TD>
<TD align=right>24,800.00</TD>
<TD align=right>&nbsp;</TD>
<TD style="COLOR: #585858" align=right>46.92</TD>
<TD align=left>客户转账</TD>
<TD align=left>payment 阙寿堂</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-17</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>300.48</TD>
<TD style="COLOR: #585858" align=right>347.40</TD>
<TD align=left>客户转账</TD>
<TD align=left>111111 李洪雷</TD></TR>
<TR>
<TD align=middle>2013-04-17</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>10,000.02</TD>
<TD style="COLOR: #585858" align=right>10,347.42</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 赖秋坛</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-17</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>596.00</TD>
<TD style="COLOR: #585858" align=right>10,943.42</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 陈欢欢</TD></TR>
<TR>
<TD align=middle>2013-04-17</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>400.04</TD>
<TD style="COLOR: #585858" align=right>11,343.46</TD>
<TD align=left>客户转账</TD>
<TD align=left>111111 李洪雷</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-17</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>2,000.08</TD>
<TD style="COLOR: #585858" align=right>13,343.54</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 颜斌</TD></TR>
<TR>
<TD align=middle>2013-04-17</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>2,000.36</TD>
<TD style="COLOR: #585858" align=right>15,343.90</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 颜斌</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-17</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>1,450.09</TD>
<TD style="COLOR: #585858" align=right>16,793.99</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 赖秋坛</TD></TR>
<TR>
<TD align=middle>2013-04-17</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>500.55</TD>
<TD style="COLOR: #585858" align=right>17,294.54</TD>
<TD align=left>客户转账</TD>
<TD align=left>111211 李洪雷</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-17</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>4,995.25</TD>
<TD style="COLOR: #585858" align=right>22,289.79</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 赖秋坛</TD></TR>
<TR>
<TD align=middle>2013-04-17</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>3,500.09</TD>
<TD style="COLOR: #585858" align=right>25,789.88</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 赖秋坛</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-17</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>2,000.77</TD>
<TD style="COLOR: #585858" align=right>27,790.65</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 颜斌</TD></TR>
<TR>
<TD align=middle>2013-04-17</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>2,000.42</TD>
<TD style="COLOR: #585858" align=right>29,791.07</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 颜斌</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-17</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>400.58</TD>
<TD style="COLOR: #585858" align=right>30,191.65</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 李洪雷</TD></TR>
<TR>
<TD align=middle>2013-04-17</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>1,000.34</TD>
<TD style="COLOR: #585858" align=right>31,191.99</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 颜斌</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-17</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>2,000.94</TD>
<TD style="COLOR: #585858" align=right>33,192.93</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 颜斌</TD></TR>
<TR>
<TD align=middle>2013-04-17</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>900.15</TD>
<TD style="COLOR: #585858" align=right>34,093.08</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 李洪雷</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-17</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>2,000.86</TD>
<TD style="COLOR: #585858" align=right>36,093.94</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 颜斌</TD></TR>
<TR>
<TD align=middle>2013-04-18</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>500.58</TD>
<TD style="COLOR: #585858" align=right>36,594.52</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 李洪雷</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-18</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>4,995.35</TD>
<TD style="COLOR: #585858" align=right>41,589.87</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 赖秋坛</TD></TR>
<TR>
<TD align=middle>2013-04-18</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>5,600.25</TD>
<TD style="COLOR: #585858" align=right>47,190.12</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 赖秋坛</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-18</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>1,500.90</TD>
<TD style="COLOR: #585858" align=right>48,691.02</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 钱卫明</TD></TR>
<TR>
<TD align=middle>2013-04-18</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>3,750.95</TD>
<TD style="COLOR: #585858" align=right>52,441.97</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银转账 宋新燕</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-18</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>15,000.01</TD>
<TD style="COLOR: #585858" align=right>67,441.98</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 赖秋坛</TD></TR>
<TR>
<TD align=middle>2013-04-18</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>900.78</TD>
<TD style="COLOR: #585858" align=right>68,342.76</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 李洪雷</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-18</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>22,000.46</TD>
<TD style="COLOR: #585858" align=right>90,343.22</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 赖秋坛</TD></TR>
<TR>
<TD align=middle>2013-04-18</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>6,000.60</TD>
<TD style="COLOR: #585858" align=right>96,343.82</TD>
<TD align=left>客户转账</TD>
<TD align=left>111111 李洪雷</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-18</TD>
<TD align=right>45,000.00</TD>
<TD align=right>&nbsp;</TD>
<TD style="COLOR: #585858" align=right>51,343.82</TD>
<TD align=left>客户转账</TD>
<TD align=left>payment 阙寿堂</TD></TR>
<TR>
<TD align=middle>2013-04-18</TD>
<TD align=right>45,000.00</TD>
<TD align=right>&nbsp;</TD>
<TD style="COLOR: #585858" align=right>6,343.82</TD>
<TD align=left>客户转账</TD>
<TD align=left>payment 阙寿堂</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-18</TD>
<TD align=right>6,300.00</TD>
<TD align=right>&nbsp;</TD>
<TD style="COLOR: #585858" align=right>43.82</TD>
<TD align=left>客户转账</TD>
<TD align=left>payment 阙寿堂</TD></TR>
<TR>
<TD align=middle>2013-04-18</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>2,000.85</TD>
<TD style="COLOR: #585858" align=right>2,044.67</TD>
<TD align=left>客户转账</TD>
<TD align=left>111111 李洪雷</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-18</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>599.98</TD>
<TD style="COLOR: #585858" align=right>2,644.65</TD>
<TD align=left>行内汇入</TD>
<TD align=left>还款 王辉</TD></TR>
<TR>
<TD align=middle>2013-04-18</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>3,000.41</TD>
<TD style="COLOR: #585858" align=right>5,645.06</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 刘亮</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-18</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>2,000.17</TD>
<TD style="COLOR: #585858" align=right>7,645.23</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 刘亮</TD></TR>
<TR>
<TD align=middle>2013-04-18</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>2,100.73</TD>
<TD style="COLOR: #585858" align=right>9,745.96</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 陈欢欢</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-19</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>3,000.88</TD>
<TD style="COLOR: #585858" align=right>12,746.84</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 李洪雷</TD></TR>
<TR>
<TD align=middle>2013-04-21</TD>
<TD align=right>12,700.00</TD>
<TD align=right>&nbsp;</TD>
<TD style="COLOR: #585858" align=right>46.84</TD>
<TD align=left>客户转账</TD>
<TD align=left>payment 阙寿堂</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-24</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>4,995.68</TD>
<TD style="COLOR: #585858" align=right>5,042.52</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 赖秋坛</TD></TR>
<TR>
<TD align=middle>2013-04-24</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>510.26</TD>
<TD style="COLOR: #585858" align=right>5,552.78</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 钱卫明</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-24</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>3,600.66</TD>
<TD style="COLOR: #585858" align=right>9,153.44</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 赖秋坛</TD></TR>
<TR>
<TD align=middle>2013-04-24</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>1,000.94</TD>
<TD style="COLOR: #585858" align=right>10,154.38</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 钱卫明</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-24</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>1,380.67</TD>
<TD style="COLOR: #585858" align=right>11,535.05</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 赖秋坛</TD></TR>
<TR>
<TD align=middle>2013-04-24</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>1,200.28</TD>
<TD style="COLOR: #585858" align=right>12,735.33</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 赖秋坛</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-24</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>999.07</TD>
<TD style="COLOR: #585858" align=right>13,734.40</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 王辉</TD></TR>
<TR>
<TD align=middle>2013-04-24</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>12,300.31</TD>
<TD style="COLOR: #585858" align=right>26,034.71</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 赖秋坛</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-24</TD>
<TD align=right>26,000.00</TD>
<TD align=right>&nbsp;</TD>
<TD style="COLOR: #585858" align=right>34.71</TD>
<TD align=left>客户转账</TD>
<TD align=left>payment 阙寿堂</TD></TR>
<TR>
<TD align=middle>2013-04-24</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>2,999.53</TD>
<TD style="COLOR: #585858" align=right>3,034.24</TD>
<TD align=left>行内汇入</TD>
<TD align=left>还款 王辉</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-24</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>500.62</TD>
<TD style="COLOR: #585858" align=right>3,534.86</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 陈欢欢</TD></TR>
<TR>
<TD align=middle>2013-04-24</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>10,600.63</TD>
<TD style="COLOR: #585858" align=right>14,135.49</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 赖秋坛</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-24</TD>
<TD align=right>14,100.00</TD>
<TD align=right>&nbsp;</TD>
<TD style="COLOR: #585858" align=right>35.49</TD>
<TD align=left>客户转账</TD>
<TD align=left>转账 阙寿堂</TD></TR>
<TR>
<TD align=middle>2013-04-24</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>500.00</TD>
<TD style="COLOR: #585858" align=right>535.49</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 薛扳斌</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-24</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>7,800.48</TD>
<TD style="COLOR: #585858" align=right>8,335.97</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 赖秋坛</TD></TR>
<TR>
<TD align=middle>2013-04-24</TD>
<TD align=right>8,300.00</TD>
<TD align=right>&nbsp;</TD>
<TD style="COLOR: #585858" align=right>35.97</TD>
<TD align=left>客户转账</TD>
<TD align=left>payment 阙寿堂</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-24</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>500.79</TD>
<TD style="COLOR: #585858" align=right>536.76</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 颜斌</TD></TR>
<TR>
<TD align=middle>2013-04-24</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>492.05</TD>
<TD style="COLOR: #585858" align=right>1,028.81</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 陈欢欢</TD></TR>
<TR class=dgAlternatingItem>
<TD align=middle>2013-04-24</TD>
<TD align=right>&nbsp;</TD>
<TD align=right>680.47</TD>
<TD style="COLOR: #585858" align=right>1,709.28</TD>
<TD align=left>客户转账</TD>
<TD align=left>网银贷记接收收款 钱卫明</TD></TR></TBODY></TABLE>
<TABLE id=Table8 class=tbBlock border=0 cellSpacing=1>
<TBODY>
<TR>
<TD class=tdCommonTopColor>
<TABLE id=Table9 class=tbCommonColor border=0 cellSpacing=1>
<TBODY>
<TR>
<TD class=tdLeftW30H20>&nbsp; 支出交易笔数： <SPAN id=OutCount class=wcLabel>24</SPAN></TD>
<TD class=tdLeftW30H20>&nbsp; 存入交易笔数： <SPAN id=InCount class=wcLabel>150</SPAN></TD>
<TD class=tdLeftH20></TD></TR>
<TR>
<TD class=tdLeftW30H20>&nbsp; 支出金额合计： <SPAN id=OutTotalMoney class=wcLabel>440,110.00</SPAN></TD>
<TD class=tdLeftW30H20>&nbsp; 存入金额合计： <SPAN id=InTotalMoney class=wcLabel>440,871.74</SPAN></TD>
<TD class=tdLeftH20>&nbsp;</TD></TR></TBODY></TABLE></TD></TR></TBODY></TABLE></TD></TR>
<TR>
<TD class=tdSpaceH12></TD></TR>
<TR>
<TD class=tdCommonTop>
<TABLE id=Table10 class=tbExplain border=0 cellSpacing=1 height=80>
<TBODY>
<TR>
<TD class=tdExplain><IFRAME id=ExplainPageFrame class=ifExplain src="../doc/Htmls/101004.htm" frameBorder=0 name=ExplainPageFrame scrolling=no></IFRAME></TD></TR></TBODY></TABLE></TD></TR>
<TR>
<TD class=tdSpaceH12></TD></TR>
<SCRIPT language=javascript src="../../Base/doc/Scripts/HostPkg.js"></SCRIPT>

<SCRIPT language=javascript src="../../Base/doc/Scripts/WebSvrCom.js"></SCRIPT>

<SCRIPT language=javascript src="../../Base/doc/Scripts/default.js"></SCRIPT>

<SCRIPT language=javascript src="../../Base/doc/Scripts/BaseFunc.js"></SCRIPT>

<SCRIPT language=javascript type=text/javascript>
    var sLocID = "021020";
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
<TD style="HEIGHT: 1px"><INPUT id=ClientNo value=A8D921BC4263F86635A728E84F5D2363830603923966271200000286 type=hidden name=ClientNo><INPUT id=PanelControl type=hidden name=PanelControl>
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