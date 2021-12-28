

//Login GenShell.The common entry of PEBank
//Global var defined here
var GenShellLogoutFlag = false;

/*
//Url of GenServer home
var GenServerIndexUrl = "/CmbBank_GenShell/UI/GenShellPC/Login/GenIndex.aspx";

var GenShellServerUrl = "/CmbBank_GenShell/UI/GenShellPC/";

//Url to apply Token
var GenServerTokenApplyUrl = "/CmbBank_GenShell/UI/GenShellPC/Login/ApplyToken.aspx";

//Login url for Security Server
var SecurityServerLoginUrl = "/CmbBank_GenShell/UI/GenShellPC/Login/SecurityLogin.aspx";
*/

//The global xmlHttpObj
var xmlHttpObj = createXmlHttp();

//Variables used by GenIndex.aspx begin
var TokenArray = new Array();
var LoginRecordArray = new Array();

var Default_DialogFeatures = "dialogHeight: 550px; dialogWidth: 750px;  edge: Raised; center: Yes; help: Yes; resizable: No; status: No;";
var Custom_DialogFeatures = null;

var Default_NewWindowFeatures = "status=no,scrollbars=yes,width=750,height=550,top=0,left=0";
var Custom_NewWindowFeatures = null;

var Default_FormTarget = "mainWorkArea";
var MainWorkAreaIFrame = null;
var Custom_FormTarget = null;
//Variables used by GenIndex.aspx end

function InitGenShellLogin(ClientNo) {
    var xmlResult = "<ClientNo>" + ClientNo + "</ClientNo><ServerURL>" + GenShellServerUrl + "</ServerURL>";
    var loginRecord = new LoginRecord(xmlResult);
    LoginRecordArray["CBANK_SHELL"] = loginRecord;
}

function IsValidAuthName(authName) {
    if (authName == "CBANK_PB") return true;
    if (authName == "CBANK_INVEST") return true;
    if (authName == "CBANK_CREDITCARD") return true;
    if (authName == "CBANK_USER") return true;
    if (authName == "CBANK_CREDITCARDV2") return true;
    //alert("Invalid AuthName : " + authName);
    alert("\u65e0\u6548\u6388\u6743\u540d\u79f0\uff1a" + authName);
    return false;
}


/*Connect transaction server
*Params: AuthName,GenShell-ClientNo,TokenArray,LoginRecordArray
*		AuthName could be:CBANK_PB,CBANK_CREDITCARD,CBANK_INVEST,CBANK_USER
*Steps:
*	Step1: Find token,if find OK,go to Step3
*	Step2: Apply token and save into TokenArray,if apply failed,return false
*	Step3: Iteratr and login all ServerUrls, find the first availabel TransServer
*	Setp4: Login transServer and save login info into LoginRecordArray
*/
function ConnectTranServer(authName, GS_ClientNo, TokenArray, LoginRecordArray) {
    var bLoginRet = false;

    var token = TokenArray[authName];
    if (token == undefined) {
        var applyTokenInfo = new Object();
        applyTokenInfo.ClientNo = GS_ClientNo;
        applyTokenInfo.AuthName = "<AuthName>" + authName + "</AuthName>";

        //Apply token and save token to TokeyArray

        var bRet = GenApplyToken(applyTokenInfo, TokenArray, authName);

        if (bRet) {//Apply token OK
            token = TokenArray[authName];
        }
        else {//Apply token failed
            //alert("Apply Token failed:"+authName);
            return false;
        }
    }

    var authData = XMLGetFirstValue(token, "AuthData");
    //alert(authData);
    var serverUrls = XMLGetAllValue(token, "LoginURL");
    var loginInfo = new Object();
    loginInfo.AuthToken = token;
    loginInfo.ClientNo = GS_ClientNo;

    /*
    if(authName == "CBANK_PB")
    {//BasicTrans login, login directly with token
    }
    else if(authName == "CBANK_CREDITCARD")
    {//CreditCard login, user should input some extra data to login
    }
    else if(authName == "CBANK_INVEST")
    {//Invest login, user should input some extra data to login
    }

	else if(authName == "CBANK_SECURITY")
    {//Security login, user should input some extra data to login
    loginInfo.ServerUrls = serverUrls;
    ////////////////login style1 begin:user input extra data,and close the window,if failed,alert message
    //var userData = window.showModalDialog(SecurityServerLoginUrl ,loginInfo);
    //ListPropertyNames(userData);
    //ListPropertyNames(loginInfo);
    //CopyProperties(userData,loginInfo);
    //ListPropertyNames(loginInfo);
    ////////////////login style1 end

		////////////////login style2 begin:user input extra data,and login, if ok,close the window
    var loginResult = window.showModalDialog(SecurityServerLoginUrl,loginInfo);
    if(loginResult.logined)
    {
    var loginResponse = loginResult.loginResponse;
    var loginRecord = new LoginRecord(loginResult.loginResponse);

			LoginRecordArray[authName] = loginRecord;

			//Login ok, delete Token data
    delete loginResult.loginResponse;
    delete TokenArray[authName];
    return true;
    }
    else
    {
    return false;
    }
    ///////////////////// style2 end
    }
    else
    {
    alert("Illegal AuthName!");
    return false;
    }
    */
    for (var i = 0; i < serverUrls.length; i++) {
        //alert(serverUrls[i]);

        var loginResult = GenLoginTransServer(serverUrls[i], loginInfo);
        if (loginResult) {
            if (loginResult.logined) {
                var loginResponse = loginResult.loginResponse;
                var loginRecord = new LoginRecord(loginResult.loginResponse);

                LoginRecordArray[authName] = loginRecord;

                //Login ok, delete Token data
                delete loginResult.loginResponse;
                delete TokenArray[authName];
                bLoginRet = true;
                break;
            }
            else {
                if (loginResult.continueFlag) {
                    continue
                }
                else {
                    break;
                }
            }
        }
        else {//Fatal error occured!
            //alert("Fatal error occurs! Login Failed!");
            alert("\u767b\u5f55\u5931\u8d25\uff0c\u8bf7\u7a0d\u540e\u518d\u8bd5:" + authName);
            break;
        }
    }
    return bLoginRet;

}

//Login TransServer with Token
function GenLoginTransServer(serverUrl, loginInfo) {
    var dynaXmlhttp = new DynaXmlHttp();
    dynaXmlhttp.addParams(loginInfo);
    dynaXmlhttp.setXmlHttpObj(xmlHttpObj);
    dynaXmlhttp.setAction(serverUrl);
    //dynaXmlhttp.enableDebug();

    var loginResult = new Object();
    loginResult.continueFlag = true;
    loginResult.logined = false;
    //alert("GenLoginTransServer:"+serverUrl);
    if (dynaXmlhttp.send()) {
        var xmlResult = dynaXmlhttp.getResponseText();
        //alert(xmlResult);
        var loginFlag = XMLGetFirstValue(xmlResult, "LoginFlag");
        var loadFlag = XMLGetFirstValue(xmlResult, "LoadFlag");

        if (loginFlag == "") {//Fatal error, result data not xml format,balance to another server
            loginResult.continueFlag = true;
            loginResult.logined = false;
        }
        else if (loginFlag == "0") {//login OK
            loginResult.continueFlag = false;
            loginResult.logined = true;
            loginResult.loginResponse = xmlResult;
        }
        else if (loadFlag == "1") {//server can not accept more request, balance to another server
            loginResult.continueFlag = true;
            loginResult.logined = false;
        }
        else {//login failed
            loginResult.continueFlag = false;
            loginResult.logined = false;
            alert("\u767b\u5f55\u5931\u8d25\uff1a" + XMLGetFirstValue(xmlResult, "LoginMessage"));
        }

    }
    return loginResult;
}

//Parse token and save into TokenArray
function GenParseToken(genSvrToken, TokenArray) {
    var tokens = XMLGetAllValue(genSvrToken, "AuthToken");
    for (var i = 0; i < tokens.length; i++) {
        var token = tokens[i];
        var tokenName = XMLGetFirstValue(token, "AuthName");
        TokenArray[tokenName] = "<AuthToken>" + token + "</AuthToken>";
    }
}

//Apply token with authname,GenShellClientNo
function GenApplyToken(applyInfo, TokenArray, authName) {
    var dynaXmlhttp = new DynaXmlHttp();
    dynaXmlhttp.addParams(applyInfo);
    dynaXmlhttp.setXmlHttpObj(xmlHttpObj);
    dynaXmlhttp.setAction(GenServerTokenApplyUrl);
    //dynaXmlhttp.enableDebug();
    if (dynaXmlhttp.send()) {
        var xmlResult = dynaXmlhttp.getResponseText();
        var resultFlag = XMLGetFirstValue(xmlResult, "ApplyFlag");
        if (resultFlag == "") {
            //alert("Fatal error occurs! Apply Token Failed#1!");
            alert("\u7cfb\u7edf\u5fd9#1[" + authName + "]\uff0c\u8bf7\u91cd\u65b0\u767b\u5f55!");
            return false;
        }
        else if (resultFlag == "Y") {
            GenParseToken(XMLGetFirstValue(xmlResult, "ApplyedToken"), TokenArray);
            return true;
        }
        else {
            alert("\u767b\u5f55\u5931\u8d25\uff1a" + XMLGetFirstValue(xmlResult, "ApplyMessage"));
            return false;
        }
    }
    else {
        //alert("Fatal error occurs! Apply Token Failed#2!");
        alert("\u7cfb\u7edf\u5fd9#2[" + authName + "]\uff0c\u8bf7\u91cd\u65b0\u767b\u5f55!");
        return false;
    }
}

function GenLogoutTransServer() {
    for (var authName in LoginRecordArray) {
    		if(authName == "event")
    			continue;
        var loginRecord = LoginRecordArray[authName];

        var dynaXmlhttp = new DynaXmlHttp();

        dynaXmlhttp.addParam("ClientNo", loginRecord.clientNo);
        dynaXmlhttp.setXmlHttpObj(xmlHttpObj);
        //dynaXmlhttp.setAction(loginRecord.serverUrl+"Login/Logout.aspx");
        dynaXmlhttp.setAction(loginRecord.serverUrl.concat("Login/Logout.aspx"));

        dynaXmlhttp.send();
        //alert(dynaXmlhttp.getResponseText());
    }
}

function GenLogoutTransServerOnly() {
    for (var authName in LoginRecordArray) {
        if (authName == "CBANK_SHELL") continue;
        var loginRecord = LoginRecordArray[authName];

        var dynaXmlhttp = new DynaXmlHttp();

        dynaXmlhttp.addParam("ClientNo", loginRecord.clientNo);
        dynaXmlhttp.setXmlHttpObj(xmlHttpObj);
        //dynaXmlhttp.setAction(loginRecord.serverUrl+"Login/Logout.aspx");
        dynaXmlhttp.setAction(loginRecord.serverUrl.concat("Login/Logout.aspx"));

        dynaXmlhttp.send();
        //alert(dynaXmlhttp.getResponseText());
    }
}

//Logout GenServer shell,delete all loginRecord info
function GenLogoutShell(logoutInfo, logoutUrl) {

    //logout all trans server
    GenLogoutTransServer();
    for (var authName in LoginRecordArray) {
        delete LoginRecordArray[authName];
    }
    /*
    var dynaXmlhttp = new DynaXmlHttp();
    dynaXmlhttp.addParams(logoutInfo);
    dynaXmlhttp.setXmlHttpObj(xmlHttpObj);
    dynaXmlhttp.setAction(logoutUrl);
    if(dynaXmlhttp.send())
    {
    GenShellLogoutFlag = true;
    for(var authName in LoginRecordArray)
    {
    delete LoginRecordArray[authName];

		}

		var xmlResult = dynaXmlhttp.getResponseText();
    //alert(xmlResult);
    return true;
    }
    else
    {
    alert("Fatal error occurs! Logout Failed!#2");
    return false;
    }
    */
}

//Login GenServer shell,params:loginid,password,logintype,etc..
function GenLoginShell(loginInfo, loginUrl, genIndexUrl) {
    var dynaXmlhttp = new DynaXmlHttp();

    dynaXmlhttp.addParams(loginInfo);
    dynaXmlhttp.setXmlHttpObj(xmlHttpObj);
    dynaXmlhttp.setAction(loginUrl);

    if (dynaXmlhttp.send()) {
        var xmlResult = dynaXmlhttp.getResponseText();
        var resultFlag = XMLGetFirstValue(xmlResult, "LoginFlag");
        if (resultFlag == "") {
            //alert("Fatal error occurs! Login Failed!#1");
            alert("\u767b\u5f55\u5931\u8d25#1\uff0c\u8bf7\u7a0d\u540e\u518d\u8bd5!");
            //alert(xmlResult);	//add alert by wlf 20060726
            return false;
        }
        else if (resultFlag == "Y") {
            notIELoginStatistic();
            if (genIndexUrl != null && genIndexUrl != "") {
                var dynaForm = new DynaForm("transLoginFrom");

                //dynaForm.setTarget("CMB_GenServer_Win");
                //dynaForm.setFeatures("status=yes,resizable=yes,scrollbars=yes,location=no,menubar=no,toolbar=no,top=0,left=0");
                dynaForm.setAction(genIndexUrl);
                dynaForm.addHidden("ClientNo", loginInfo["ClientNo"]);
                //dynaForm.enableDebug();
                dynaForm.submit();
            }
            return true;
        }
        else {
            //ClientWarrant
            var sLoginErrCode = XMLGetFirstValue(xmlResult, "LoginErrCode");
            if (sLoginErrCode == "CW00" || sLoginErrCode == "CW011" || sLoginErrCode == "CW001") {
                var Custom_DialogFeaturesTemp = Custom_DialogFeatures;
                Custom_DialogFeatures = "dialogHeight: 600px; dialogWidth: 782px;  edge: Raised; center: Yes; help: Yes; resizable: No; status: No;";
                CallFunc('CBANK_SHELL', 'Login/CW_Introduce.aspx', 'MODAL_DIALOG', null);
                Custom_DialogFeatures = Custom_DialogFeaturesTemp;
            }
            else {
                var msg = "\u767b\u5f55\u5931\u8d25\uff1a"+XMLGetFirstValue(xmlResult,"LoginMessage");
                alert(msg.replace(/\\n/g,"\n"));//把\\n转成\n，使换行生效
                //alert("\u767b\u5f55\u5931\u8d25\uff1a" + XMLGetFirstValue(xmlResult, "LoginMessage"));

                if (loginInfo.LoginType == "C") {
                    var loginErrCode = XMLGetFirstValue(xmlResult, "LoginErrCode");
                    if (loginErrCode == "24") {
                        window.open('/CmbBank_CreditCard/UI/CreditCardPC/CreditCard/cs_RegisterNetBank.aspx', 'CreditCardActivate', 'scrollbars=yes,toolbar=no,location=no,status=no,menubar=no,resizable=no,dependent=no,width=760,height=520,top=' + nWinTop + ',left=' + nWinLeft);
                    }

                    if (loginErrCode == "84" || loginErrCode == "85") {
                        window.open('/CmbBank_CreditCard/UI/CreditCardPC/CreditCard/cs_ProOfActivate.aspx', 'CreditCardActivate', 'scrollbars=yes,toolbar=no,location=no,status=no,menubar=no,resizable=no,dependent=no,width=760,height=520,top=' + nWinTop + ',left=' + nWinLeft);
                    }
                    
                    //信用卡新系统
                    if (loginErrCode == "CH402E") {
                        window.open('/CmbBank_CreditCardV2/UI/CreditCardPC/CreditCardV2/cs_ProOfActivate.aspx', 'CreditCardActivate', 'scrollbars=yes,toolbar=no,location=no,status=no,menubar=no,resizable=no,dependent=no,width=760,height=520,top=' + nWinTop + ',left=' + nWinLeft);
                    }
                    
                }

                if (loginInfo.LoginType == "D") {
                    var loginErrCode = XMLGetFirstValue(xmlResult, "LoginErrCode");
                    if (loginErrCode == "11") {
                        var nWinLeft = 0;
                        var nWinTop = 0;
                        //window.open('/ea/activatenbs.aspx', 'EAccountSetNetBankPwd', 'menubar=no,toolbar=no,location=no,directories=no,status=yes,resizable=yes,scrollbars=yes,width=780,height=500,top=' + nWinTop + ',left=' + nWinLeft);
                        window.open('https://user.cmbchina.com/User/Login?returnUrl=%2f%3fop=ActivateNetBank', 'EAccountSetNetBankPwd', 'menubar=no,toolbar=no,location=no,directories=no,status=yes,resizable=yes,scrollbars=yes,width=780,height=500,top=' + nWinTop + ',left=' + nWinLeft);
                    }
                }               
            }
            return false;
        }
    }
    else {
        //alert("Fatal error occurs! Login Failed!#2");
        alert("\u767b\u5f55\u5931\u8d25#2\uff0c\u8bf7\u7a0d\u540e\u518d\u8bd5!");
        return false;
    }
}

function notIELoginStatistic() {
    try {
        if (GetBrowserVersion() != "IE") {
            if (navigator.platform == "MacIntel") {
                EventTrack('macosx.htm', '大众版Mac登录成功');
            }
            else {
                EventTrack('chromeandfirefox.htm', '大众版Chrome和FireFox等登录成功');
            }
        }
    } catch (ex) { }
}

function LoginRecord(loginResponse) {
    this.clientNo = XMLGetFirstValue(loginResponse, "ClientNo");
    this.serverUrl = XMLGetFirstValue(loginResponse, "ServerURL");
}

/*set features for Modal/Modeless dialog*/
function setDialogFeatures(features) {
    Custom_DialogFeatures = features;
}

function getDialogFeatures() {
    if (Custom_DialogFeatures == null) {
        return Default_DialogFeatures;
    }
    else {
        var retFeatures = Custom_DialogFeatures;
        Custom_DialogFeatures = null;
        return retFeatures;
    }
}

/*set features for window.open(...)*/
function setNewWindowFeatures(features) {
    NewWindowFeatures = features;
}

function getNewWindowFeatures() {
    if (Custom_NewWindowFeatures == null) {
        return Default_NewWindowFeatures;
    }
    else {
        var retFeatures = Custom_NewWindowFeatures;
        Custom_NewWindowFeatures = null;
        return retFeatures;
    }
}

/*set target for DynaForm*/
function setFormTarget(target) {
    Custom_FormTarget = target;
}

function getFormTarget() {
    if (Custom_FormTarget == null) {
        return Default_FormTarget;
    }
    else {
        var retTarget = Custom_FormTarget;
        Custom_FormTarget = null;
        return retTarget;
    }
}







/*Trigger trans function from menu,link,etc.. Mainly used by GenIndex.aspx
* params:
*	param1: authName, could be CBANK_PB,CBANK_CREDITCARD,CBANK_INVEST,CBANK_USER
*	param2: funcName, the function to be fired
*	param3: target of the function result. could be: FORM,MODAL_DIALOG,MODELESS_DIALOG
*	paramn: other param transfer to function,format:paramName=paramValue
*   更新说明（2008-11-18）：大众版导航功能需要切换菜单和记录历史操作，由于CallFunc并不支持，故
*   使用CallFuncEx2替换CallFunc的功能，保留CallFunc以提供向下兼容功能（防备万一），所有后续开发
*   应使用CallFuncEx2代替CallFunc。
*   CallFuncEx2对CallFunc做了包装，以提供切换菜单和记录历史操作的功能，因此不能删除CallFunc函数
*/
function CallFunc(authName, funcName, targetType, target) {
    if (window.event) {
        window.event.returnValue = false;
    }



    var loginRecord = LoginRecordArray[authName]
    if (loginRecord == undefined) {//not login yet, forward user to login transaction server
        //If the authname is valid, comment this check when needed
        if (!IsValidAuthName(authName)) {
            return false;
        }
        if (ConnectTranServer(authName, GenShell_ClientNo, TokenArray, LoginRecordArray)) {
            loginRecord = LoginRecordArray[authName];
        }
        else {
            //alert("ConnectTranServer failed:"+authName);
            return false;
        }
    }

    //build action for dynaForm
    var action = loginRecord.serverUrl + funcName;
    //alert(action);
    var dynaForm = new DynaForm("GenForm");
    dynaForm.setAction(action);
    dynaForm.addHidden("ClientNo", loginRecord.clientNo);

    if (arguments.length >= 5) {
        for (var i = 4; i < arguments.length; i++) {
            var args = arguments[i].split("=");
            dynaForm.addHidden(args[0], args[1]);
        }
    }
    //dynaForm.enableDebug();
    if (targetType == "MODAL_DIALOG") {
        g_ModalDialogClick = true;
        window.showModalDialog("/CmbBank_GenShell/UI/Base/DialogHelper.aspx", dynaForm, getDialogFeatures());
        return true;
    }
    else if (targetType == "MODELESS_DIALOG") {
        window.showModelessDialog("/CmbBank_GenShell/UI/Base/DialogHelper.aspx", dynaForm, getDialogFeatures());
        return true;
    }
    else {
        if (target == null) {
            target = getFormTarget();
        }

        if (target == Default_FormTarget) {

            //if(MainWorkAreaIFrame)
            {
                //mainWorkArea.document.write("<style TYPE='text/css'>.mycontent {font: 9pt/14pt '宋体';}</style>");
                mainWorkArea.document.write("<body style='margin-top:0;margin-bottom:0;margin-left:0;margin-right:0;'><table height=300 width=100% align=center><tr><td height=100% width=100% align=center><font size=2pt>\u6b63\u5728\u52a0\u8f7d\u6570\u636e\uff0c\u8bf7\u7a0d\u5019 ... ...</font></tr></td></table></body>");
                //mainWorkArea.document.close();
                //mainWorkArea.location = "/CmbBank_GenShell/UI/GenShellPC/Base/GenBlankPage.aspx";
                //return;
            }

        }

        dynaForm.setFeatures(getNewWindowFeatures());
        dynaForm.setTarget(target);
        //dynaForm.enableDebug();
        dynaForm.submit();
        return true;
    }

    var loginRecord = LoginRecordArray["CBANK_SHELL"];
}

//request from shell to other transserver by xmlhttp
function CallFuncEx(authName, funcName, prid, boolAsync, callBackFunc) {
    //alert("in callfuncex");
    if (window.event) {
        window.event.returnValue = false;
    }

    var loginRecord = LoginRecordArray[authName]
    if (loginRecord == undefined) {//not login yet, forward user to login transaction server
        //If the authname is valid, comment this check when needed
        if (!IsValidAuthName(authName)) {
            //alert("!IsValidAuthName(authName)");
            return false;
        }
        if (ConnectTranServer(authName, GenShell_ClientNo, TokenArray, LoginRecordArray)) {
            loginRecord = LoginRecordArray[authName];
        }
        else {
            alert("ConnectTranServer failed:" + authName);
            return false;
        }
    }

    //build action for dynaForm
    var action = loginRecord.serverUrl + funcName;
    var hostPkg = new HostPkg(prid);
    if (arguments.length >= 6) {
        for (var i = 5; i < arguments.length; i++) {
            var args = arguments[i].split("=");
            hostPkg.AddParam(args[0], args[1]);
        }
    }
    hostPkg.AddParam("ClientNo", loginRecord.clientNo);

    var webSvrCom = new WebSvrCom(action);
    webSvrCom.WSCSetAsync(boolAsync);
    webSvrCom.WSCClearHostPkg();
    webSvrCom.WSCAddHostPkg(hostPkg);
    webSvrCom.WSCSendRequest(callBackFunc);
    //alert("exit ok");

    //var loginRecord = LoginRecordArray["CBANK_SHELL"];
}


var g_currMenuType = '';
function CallFuncEx2(menuType, authName, funcName, targetType, target) {
    if (window.event) {
        window.event.returnValue = false;
    }
    var sJsonCardInfo = document.getElementById("CardsInfoForType").value;
    var jsonCardInfo = null;
    try {
        jsonCardInfo = eval("(" + sJsonCardInfo + ")");
    } catch (e) {
        jsonCardInfo = null
    }
    if (jsonCardInfo != null && menuType != null) {
        if (jsonCardInfo[menuType] != true) {
            if (menuType == "A")
                alert("对不起，您没有绑定一卡通，无法使用相关功能！");
            else if (menuType == "B")
                alert("对不起，您没有绑定存折，无法使用相关功能！");
            else if (menuType == "C_A" || menuType == "C")
                alert("对不起，您没有绑定信用卡，无法使用相关功能！");
            else if (menuType == "D")
                alert("您需要使用一网通用户后才能使用相关功能！");
            else
                alert("您暂时无法使用当前功能！");
            return;
        }
    } else {
        return;
    }

    try {
        TryAddHistoryOpFromCallFunc(arguments);
    } catch (e) { }

    var args = new Array();
    for (var i = 1; i < arguments.length; i++) {
        args.push(arguments[i]);
    }
    var r = CallFunc.apply(this, args);
    //args.
    //arguments
    if (r) {
        if (jsonCardInfo['D'] == null)
            return;
        if (g_currMenuType != menuType) {
            switchImg(menuType);
            switchMenuOnly(menuType);

        }
    }
}

function GenSendXmlRequest(data2Send, svrUrl) {
    var dynaXmlhttp = new DynaXmlHttp();
    dynaXmlhttp.addParams(data2Send);
    dynaXmlhttp.setXmlHttpObj(xmlHttpObj);
    dynaXmlhttp.setAction(svrUrl);
    //dynaXmlhttp.enableDebug();
    if (dynaXmlhttp.send()) {
        //alert("dd:"+dynaXmlhttp.getResponseHeader("Content-Type"));
        return dynaXmlhttp.getResponseText();
    }
    else {
        //alert("Fatal error occurs! Send XmlRequest Failed!");
        alert("\u83b7\u53d6\u83dc\u5355\u6570\u636e\u5931\u8d25\uff0c\u8bf7\u6309F5\u5237\u65b0\u60a8\u7684\u9875\u9762!");
        return false;
    }
}

/*
if(window.onload)
{
alert("document.body");
}
else
{
window.onload=SetGenStyle;
}

alert(window.document.body.onload);
*/

function SetGenStyle(styleTitle, theDocument) {
    if (theDocument) {
    }
    else {
        theDocument = document;
    }
    var GenStyleCookie = new Cookie(theDocument, "Cmb_GenServer_Style", 24 * 365 * 10);
    if (styleTitle) {
    }
    else {
        if (GenStyleCookie.load()) {
            styleTitle = GenStyleCookie.style;
        }
        else {
            styleTitle = "default";
        }
    }

    var links = theDocument.getElementsByTagName("link");
    for (i = 0; links[i]; i++) {
        if (links[i].getAttribute("rel").indexOf("style") != -1 && links[i].getAttribute("title")) {
            links[i].disabled = true;
            if (links[i].getAttribute("title").indexOf(styleTitle) != -1)
                links[i].disabled = false;
        }
    }
    //alert(styleTitle);
    GenStyleCookie.style = styleTitle;
    GenStyleCookie.store();
}
