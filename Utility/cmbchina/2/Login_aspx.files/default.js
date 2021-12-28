
String.prototype.trim = function()
{
	return this.replace(/(^\s*)|(\s*$)/g, "");
}

String.prototype.gblen = function()
{
    return this.replace(/[^\x00-\xff]/g,"**").length;
}

String.prototype.suiteLen = function(min, max)
{
	var len = this.gblen();
	return len >=min && len <=max;
}

function MM_preloadImages() { //v3.0
  var d=document; if(d.images){ if(!d.MM_p) d.MM_p=new Array();
    var i,j=d.MM_p.length,a=MM_preloadImages.arguments; for(i=0; i<a.length; i++)
    if (a[i].indexOf("#")!=0){ d.MM_p[j]=new Image; d.MM_p[j++].src=a[i];}}
}

function MM_swapImgRestore() { //v3.0
  var i,x,a=document.MM_sr; for(i=0;a&&i<a.length&&(x=a[i])&&x.oSrc;i++) x.src=x.oSrc;
}

function MM_findObj(n, d) { //v4.01
  var p,i,x;  if(!d) d=document; if((p=n.indexOf("?"))>0&&parent.frames.length) {
    d=parent.frames[n.substring(p+1)].document; n=n.substring(0,p);}
  if(!(x=d[n])&&d.all) x=d.all[n]; for (i=0;!x&&i<d.forms.length;i++) x=d.forms[i][n];
  for(i=0;!x&&d.layers&&i<d.layers.length;i++) x=MM_findObj(n,d.layers[i].document);
  if(!x && d.getElementById) x=d.getElementById(n); return x;
}

function MM_swapImage() { //v3.0
  var i,j=0,x,a=MM_swapImage.arguments; document.MM_sr=new Array; for(i=0;i<(a.length-2);i+=3)
   if ((x=MM_findObj(a[i]))!=null){document.MM_sr[j++]=x; if(!x.oSrc) x.oSrc=x.src; x.src=a[i+2];}
}


var wnds = new Array();
function OpenWnd(url, w, h, n, s, loc)
{
	var nWinLeft, nWinTop;
	if(loc == 1)
	{
		nWinLeft = event.screenX;
		nWinTop = event.screenY;
	}
	else
	{
		nWinLeft = (screen.width-w)/2;
		nWinTop = (screen.height-h)/2;
	}

	if(n != null && n != '' && n.charAt(0) != '_' && wnds[n] != null)
	{
		wnds[n].close();
	}

	wnds[n] = window.open(url, n,'resizable=1,scrollbars='+s+',width=' + w + ',height='+ h + ',top=' + nWinTop + ',left=' + nWinLeft);
}

function GetVirtualPathRoot() {
    var currUrl = "" + window.location.href;
    var pos1 = currUrl.indexOf("//");
    var pos2 = currUrl.indexOf("/", pos1 + 2);
    var pos3 = currUrl.indexOf("/", pos2 + 1);
    var preUrl = currUrl.substr(0, pos3 + 1);
    return preUrl;
}

function GetHelperDialogUrl() {
    var dialogUrl = GetVirtualPathRoot() + "UI/Base/DialogHelper.aspx";
    return dialogUrl;
}

function IsSameHost(actionName) {
    actionName = actionName.toLowerCase();
    if (actionName.indexOf("http://") >= 0 || actionName.indexOf("https://") >= 0) {
        if (actionName.indexOf("pbsz.ebank.cmbchina.com") < 0 && actionName.indexOf("pbnj.ebank.cmbchina.com") < 0)
        return false;
    }
    return true;
}


var dialogFeatures = "";
var newwinFeatures = "";
function triggerFunc(actionName, openType, targetName)
{
	/*add history op*/
	try{
		if( typeof(window.top.TryAddHistoryOpFromTriggerFunc) == "function" )
		{
			if(window != window.top)
				window.top.TryAddHistoryOpFromTriggerFunc(arguments,window.name);
			else
				window.top.TryAddHistoryOpFromTriggerFunc(arguments,null);
		}
	}catch(e)
	{}

	var funcTrigger = new FuncTrigger(actionName);

	if (dialogFeatures.length > 0)
	{
		funcTrigger.setDialogFeatures(dialogFeatures);
	}

	if (newwinFeatures.length > 0)
	{
		funcTrigger.setNewWindowFeatures(newwinFeatures);
	}

	//funcTrigger.funcUrl = actionName;
	funcTrigger.setHelperDialog(GetHelperDialogUrl());

	if (IsSameHost(actionName)) {
	    var ClientNo = document.getElementById("ClientNo");
	    funcTrigger.addFormParam("ClientNo", ClientNo.value);
	}
	
	if(arguments.length >= 4)
	{
		for(var i=3; i<arguments.length; i++)
		{
			var args = arguments[i].split("=");
			funcTrigger.addFormParam(args[0],args[1]);

		}
	}

	funcTrigger.setOpenType(openType);
	funcTrigger.setFormTarget(targetName);
	//funcTrigger.enableDebug();
	funcTrigger.trigger();
	
	
}


function triggerFuncEx(actionName, openType, targetName)
{
	/*add history op*/
	try{
		if( typeof(window.top.TryAddHistoryOpFromTriggerFunc) == "function" )
		{
			if(window != window.top)
				window.top.TryAddHistoryOpFromTriggerFunc(arguments,window.name);
			else
				window.top.TryAddHistoryOpFromTriggerFunc(arguments,null);
		}
	}catch(e)
	{}
	var funcTrigger = new FuncTrigger(actionName);

	if (dialogFeatures.length > 0)
	{
		funcTrigger.setDialogFeatures(dialogFeatures);
	}

	if (newwinFeatures.length > 0)
	{
		funcTrigger.setNewWindowFeatures(newwinFeatures);
	}

	//funcTrigger.funcUrl = actionName;
	funcTrigger.setHelperDialog(GetHelperDialogUrl());

	var ClientNo = document.getElementById("ClientNo");
	funcTrigger.addFormParam("ClientNo",ClientNo.value);

	if(arguments.length >= 4)
	{
		for(var i=3; i<arguments.length; i++)
		{
			var args = arguments[i].split("=");
			funcTrigger.addFormParam(args[0],args[1]);

		}
	}

	funcTrigger.setOpenType(openType);
	funcTrigger.setFormTarget(targetName);
	//funcTrigger.enableDebug();
	var returnValue = funcTrigger.trigger();
	if (returnValue) RefreshPage();
	
}



//add by tianchao,20070905
//support the arguments having "="
function triggerFuncEq(actionName, openType, targetName)
{
	/*add history op*/
	try{
		if( typeof(window.top.TryAddHistoryOpFromTriggerFunc) == "function" )
		{
			if(window != window.top)
				window.top.TryAddHistoryOpFromTriggerFunc(arguments,window.name);
			else
				window.top.TryAddHistoryOpFromTriggerFunc(arguments,null);
		}
	}catch(e)
	{}
	var funcTrigger = new FuncTrigger(actionName);

	if (dialogFeatures.length > 0)
	{
		funcTrigger.setDialogFeatures(dialogFeatures);
	}

	if (newwinFeatures.length > 0)
	{
		funcTrigger.setNewWindowFeatures(newwinFeatures);
	}

	funcTrigger.setHelperDialog(GetHelperDialogUrl());

	var ClientNo = document.getElementById("ClientNo");
	funcTrigger.addFormParam("ClientNo",ClientNo.value);

	if(arguments.length >= 4)
	{
		for(var i=3; i<arguments.length; i++)
		{
			var args = arguments[i].split("=");
			var key = args[0];
			var value = args[1];
			for(var j=2;j<args.length;j++)
			{
				value = value + "=" + args[j];
			}
			funcTrigger.addFormParam(key,value);

		}
	}

	funcTrigger.setOpenType(openType);
	funcTrigger.setFormTarget(targetName);
	//funcTrigger.enableDebug();
	funcTrigger.trigger();
}

//add by tianchao,20071207
//support the arguments having "="
//return value;
function triggerFuncEr(actionName, openType, targetName)
{
	/*add history op*/
	try{
		if( typeof(window.top.TryAddHistoryOpFromTriggerFunc) == "function" )
		{
			if(window != window.top)
				window.top.TryAddHistoryOpFromTriggerFunc(arguments,window.name);
			else
				window.top.TryAddHistoryOpFromTriggerFunc(arguments,null);
		}
	}catch(e)
	{}
	var funcTrigger = new FuncTrigger(actionName);

	if (dialogFeatures.length > 0)
	{
		funcTrigger.setDialogFeatures(dialogFeatures);
	}

	if (newwinFeatures.length > 0)
	{
		funcTrigger.setNewWindowFeatures(newwinFeatures);
	}

	funcTrigger.setHelperDialog(GetHelperDialogUrl());

	var ClientNo = document.getElementById("ClientNo");
	funcTrigger.addFormParam("ClientNo",ClientNo.value);

	if(arguments.length >= 4)
	{
		for(var i=3; i<arguments.length; i++)
		{
			var args = arguments[i].split("=");
			var key = args[0];
			var value = args[1];
			for(var j=2;j<args.length;j++)
			{
				value = value + "=" + args[j];
			}
			funcTrigger.addFormParam(key,value);

		}
	}

	funcTrigger.setOpenType(openType);
	funcTrigger.setFormTarget(targetName);
	//funcTrigger.enableDebug();
	var returnValue = funcTrigger.trigger();
	return returnValue;
}


function triggerFuncEx2(actionName, openType, targetName)
{
	/*add history op*/
	/*try{
		if( typeof(window.top.TryAddHistoryOpFromTriggerFunc) == "function" )
		{
			if(window != window.top)
				window.top.TryAddHistoryOpFromTriggerFunc(arguments,window.name);
			else
				window.top.TryAddHistoryOpFromTriggerFunc(arguments,null);
		}
	}catch(e)
	{}
    */
	var funcTrigger = new FuncTrigger(actionName);

	if (dialogFeatures.length > 0)
	{
		funcTrigger.setDialogFeatures(dialogFeatures);
	}

	if (newwinFeatures.length > 0)
	{
		funcTrigger.setNewWindowFeatures(newwinFeatures);
	}

	funcTrigger.setHelperDialog(GetHelperDialogUrl());

	var ClientNo = document.getElementById("ClientNo");
	funcTrigger.addFormParam("ClientNo",ClientNo.value);

	if(arguments.length >= 4)
	{
		for(var i=3; i<arguments.length; i++)
		{
			var args = arguments[i].split("=");
			funcTrigger.addFormParam(args[0],args[1]);

		}
	}

	funcTrigger.setOpenType(openType);
	funcTrigger.setFormTarget(targetName);
	//funcTrigger.enableDebug();
	var returnValue = funcTrigger.trigger();
	RefreshPage();
}

function triggerFuncEx3(actionName, openType, targetName, dialogFeatures, CallbackFunc)
{
    /*add history op*/
    try {
        if (typeof (window.top.TryAddHistoryOpFromTriggerFunc) == "function") {
            if (window != window.top)
                window.top.TryAddHistoryOpFromTriggerFunc(arguments, window.name);
            else
                window.top.TryAddHistoryOpFromTriggerFunc(arguments, null);
        }
    } catch (e)
	{ }
    var funcTrigger = new FuncTrigger(actionName);

    if (dialogFeatures.length > 0) {
        funcTrigger.setDialogFeatures(dialogFeatures);
    }

    if (newwinFeatures.length > 0) {
        funcTrigger.setNewWindowFeatures(newwinFeatures);
    }

    funcTrigger.setHelperDialog(GetHelperDialogUrl());

    var ClientNo = document.getElementById("ClientNo");
    funcTrigger.addFormParam("ClientNo", ClientNo.value);

    if (arguments.length >= 6) {
        for (var i = 5; i < arguments.length; i++) {
            var args = arguments[i].split("=");
            funcTrigger.addFormParam(args[0], args[1]);

        }
    }

    funcTrigger.setOpenType(openType);
    funcTrigger.setFormTarget(targetName);
    funcTrigger.setDialogFeatures(dialogFeatures);
    //funcTrigger.enableDebug();
    var returnValue = funcTrigger.trigger();
    if (returnValue) CallbackFunc();
}

//add by czc, 20090618
//sopport return value
function triggerFuncRe(actionName, openType, targetName) {
    /*add history op*/
    try {
        if (typeof (window.top.TryAddHistoryOpFromTriggerFunc) == "function") {
            if (window != window.top)
                window.top.TryAddHistoryOpFromTriggerFunc(arguments, window.name);
            else
                window.top.TryAddHistoryOpFromTriggerFunc(arguments, null);
        }
    } catch (e)
	{ }
    var funcTrigger = new FuncTrigger(actionName);

    if (dialogFeatures.length > 0) {
        funcTrigger.setDialogFeatures(dialogFeatures);
    }

    if (newwinFeatures.length > 0) {
        funcTrigger.setNewWindowFeatures(newwinFeatures);
    }

    funcTrigger.setHelperDialog(GetHelperDialogUrl());

    var ClientNo = document.getElementById("ClientNo");
    funcTrigger.addFormParam("ClientNo", ClientNo.value);

    if (arguments.length >= 4) {
        for (var i = 3; i < arguments.length; i++) {
            var args = arguments[i].split("=");
            funcTrigger.addFormParam(args[0], args[1]);

        }
    }

    funcTrigger.setOpenType(openType);
    funcTrigger.setFormTarget(targetName);
    //funcTrigger.enableDebug();
    var returnValue = funcTrigger.trigger();
    return returnValue;
}

function Trim(s)
{
	return s.replace( /^\s*/, "" ).replace( /\s*$/, "" );
}


function IsDigit(cCheck)
{
    return (('0'<=cCheck) && (cCheck<='9'));
}


function IsAllDigit(str)
{
    if ((str == null) || (str.length == 0))
    {
        return false;
    }

    var chCurrent;
	for (i=0; i<str.length; i++)
    {
        chCurrent = str.charAt(i);
        if (!IsDigit(chCurrent))
        {
            return false;
		}
    }
	return true;
}

function IsDigitOrChar(cCheck)
{
    return ((('A'<=cCheck) && (cCheck<='Z')) || (('a'<=cCheck) && (cCheck<='z')) || (('0'<=cCheck) && (cCheck<='9')));
}


function IsAllDigitAndChar(str)
{
    if ((str == null) || (str.length == 0))
    {
        return false;
    }

    var chCurrent;
	for (i=0; i<str.length; i++)
    {
        chCurrent = str.charAt(i);
        if (!IsDigitOrChar(chCurrent))
        {
            return false;
		}
    }
	return true;
}


function IsAllChar(str)
{
	var reg = /[^a-zA-Z]/g;
	return (!reg.test(Trim(str)));
}


function IsCharAndNum(str)
{
	var reg = /[^0-9a-zA-Z]/g;
	return (!reg.test(Trim(str)));
}


function IsASCII(str)
{
	var len = str.length;
	for(var i=0; i<len; i++)
	{
		if((parseInt(str.charCodeAt(i))<0)||(parseInt(str.charCodeAt(i))>255))
		{
			return false;
		}
	}
	return true;
}


//
var _g_maxDays = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
var _g_maxDays_leap = [31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];

//
function IsLeapYear(year)
{
	if(year%4!=0)
		return false;
	if( year%100==0 && year%400!=0 )
		return false;
	return true;
}

//判断一个8位的纯数字的字符串如20041208是否是一个合法的日期,这里只做基本的判断
function IsValidDate( stringDate )
{
	var strDate = Trim(stringDate);
	if(strDate.length != 8) return false;
	if(!IsAllDigit(strDate)) return false;
	var nYear = parseInt(strDate.substring( 0, 4 ),10);//取出前4位为年
	var nMonth = parseInt(strDate.substring( 4, 6 ),10);
	var nDay = parseInt(strDate.substring( 6, 8 ),10);
	if ( nYear <= 1900 || nMonth > 12 || nMonth < 1 )
	{
		return false;
	}
	if(nDay<=_g_maxDays[nMonth-1])
	return true;
	if(nMonth==2)
	{
		if( IsLeapYear(nYear) && nDay==29 )
			return true;
		else
			return false;
	}
	return false;
}

function AddDate(strDate,addDay)
{
	var nYear = parseInt(strDate.substring( 0, 4 ),10);
	var nMonth = parseInt(strDate.substring( 4, 6 ),10);
	var nDay = parseInt(strDate.substring( 6, 8 ),10);
	nDay += addDay;
	while(nDay>_g_maxDays[nMonth-1])
	{
		if(nMonth==2 && IsLeapYear(nYear))
		{
			if(nDay==29)
				break;
			nDay-=29;
			nMonth = 3;
			continue;
		}
		nDay -= _g_maxDays[nMonth-1];
		nMonth++;
		if(nMonth==13)
		{
			nYear += 1;
			nMonth = 1;
		}
	}
	return "" + nYear + (nMonth>9?nMonth:("0"+nMonth)) + (nDay>9?nDay:("0"+nDay));
}

/************************************************************************/
/*当月在内加三个月，并取加后月的最后一天                                */
/*如：AddMonth("20090820", 3)="20091031"                                */
/************************************************************************/
function AddMonth(strDate, addMonth) {
    var nYear = parseInt(strDate.substring(0, 4), 10);
    var nMonth = parseInt(strDate.substring(4, 6), 10);
    var nDay = parseInt(strDate.substring(6, 8), 10);

    nMonth += addMonth-1;
    if (nMonth >= 13)
    {
        nYear += 1;
        nMonth -= 12;
    }

    if (IsLeapYear(nYear))
    {
        nDay = _g_maxDays_leap[nMonth - 1];
    }
    else 
    {
        nDay = _g_maxDays[nMonth - 1];
    }

    return "" + nYear + (nMonth > 9 ? nMonth : ("0" + nMonth)) + (nDay > 9 ? nDay : ("0" + nDay));
}

/************************************************************************/
/*当月不在内加三个月                                                    */
/*如：AddMonth("20090820", 3)="20091120"                                */
/************************************************************************/
function AddMonthNew(strDate, addMonth) {
    var nYear = parseInt(strDate.substring(0, 4), 10);
    var nMonth = parseInt(strDate.substring(4, 6), 10);
    var nDay = parseInt(strDate.substring(6, 8), 10);
    var nExDay = "";

    nMonth += addMonth;
    if (nMonth >= 13)
    {
        nYear += 1;
        nMonth -= 12;
    }

    if (IsLeapYear(nYear))
    {
        nExDay = _g_maxDays_leap[nMonth - 1];
    }
    else 
    {
        nExDay = _g_maxDays[nMonth - 1];
    }
    nDay = nDay < nExDay ? nDay : nExDay;
    return "" + nYear + (nMonth > 9 ? nMonth : ("0" + nMonth)) + (nDay > 9 ? nDay : ("0" + nDay));
}

//判断一个6位的纯数字的字符串如075560是否是一个合法的时间,这里只做基本的判断
function IsValidTime( stringTime )
{
	var strTime = Trim(stringTime);
	if(strTime.length != 6) return false;
	if(!IsAllDigit(strTime)) return false;
	var nHour = strTime.substring( 0, 2 );
	var nMinute = strTime.substring( 2, 4 );
	var nSecond = strTime.substring( 4, 6 );
	if ( nHour >= 24 || nMinute > 60 ||nSecond > 60 )
	{
		return false;
	}
	return true;
}


function IsValidMoney(str)
{
    if ((str == null) || (str.length == 0))
    {
        return false;
    }

    var chCurrent;
    var dotFounded = false;
    var dotIndex=-1;
    var lenBeforeDot = str.length;
    var lenAfterDot = 0;

    for (i=0; i<str.length; i++)
    {
        chCurrent = str.charAt(i);
        if (!IsDigit(chCurrent))
        {
            if (chCurrent == '.')
            {
                if (dotFounded)
                {
                    return false;
                }
                dotFounded = true;
                dotIndex = i;
                if ((dotIndex == 0) || (dotIndex == str.length-1))
                {
                    return false;
                }

                lenBeforeDot = dotIndex;
                lenAfterDot = str.length - dotIndex -1;
             }
             else
             {
                return false;
             }
        }
    }

    if ((lenBeforeDot > 11) || (lenAfterDot >2))
    {
        return false;
    }

    return true;
}

//增加对日元为角分情况的判断
function IsValidMoneyWithCurrency(str, CurrencyID) {
    if ((str == null) || (str.length == 0)) {
        return false;
    }

    var chCurrent;
    var dotFounded = false;
    var dotIndex = -1;
    var lenBeforeDot = str.length;
    var lenAfterDot = 0;
    var isAfterDotNonZero = false;

    for (i = 0; i < str.length; i++) {
        chCurrent = str.charAt(i);
        if (!IsDigit(chCurrent)) {
            if (chCurrent == '.') {
                if (dotFounded) {
                    return false;
                }
                dotFounded = true;
                dotIndex = i;
                if ((dotIndex == 0) || (dotIndex == str.length - 1)) {
                    return false;
                }

                lenBeforeDot = dotIndex;
                lenAfterDot = str.length - dotIndex - 1;
            }
            else {
                return false;
            }
        }
        //如果小数部分有非零数字,
        isAfterDotNonZero = dotFounded && ('0' < chCurrent && chCurrent <= '9');

        if (CurrencyID == "65" && isAfterDotNonZero) {
            break;
        }
    }
    //日元的最小面值为1日元，不存在角位、分位的情况.
    //65=日元
    if (CurrencyID == "65" && lenAfterDot > 0 && isAfterDotNonZero)
        return false;

    if ((lenBeforeDot > 11) || (lenAfterDot > 2)) {
        return false;
    }

    return true;
}

function IsValidPassword(strPassword)
{
	if(strPassword.length != 6) return false;
	if(!IsAllDigit(strPassword)) return false;
	return true;
}


function IsValidFund(sCode)
{
	if(sCode.length != 6) return false;
	return true;
}


function IsValidPrice(str,intLen,decLen)
{
    if ((str == null) || (str.length == 0))
    {
        return false;
    }

    var chCurrent;
    var dotFounded = false;
    var dotIndex=-1;
    var lenBeforeDot = str.length;
    var lenAfterDot = 0;

    for (i=0; i<str.length; i++)
    {
        chCurrent = str.charAt(i);
        if (!IsDigit(chCurrent))
        {
            if (chCurrent == '.')
            {
                if (dotFounded)
                {
                    return false;
                }
                dotFounded = true;
                dotIndex = i;
                if ((dotIndex == 0) || (dotIndex == str.length-1))
                {
                    return false;
                }

                lenBeforeDot = dotIndex;
                lenAfterDot = str.length - dotIndex -1;
             }
             else
             {
                return false;
             }
        }
    }

    if ((lenBeforeDot > intLen) || (lenAfterDot >decLen))
    {
        return false;
    }

    return true;
}


function IsValidEmail(email)
{
  var re = /\w+@\w+\.\w+/;
  if (re.test(email))
    return true;
  else
    return false;
}


/*
function IsValidPhone(phone)
{
	var reg=/(^[0-9]{3,4}\-[0-9]{3,8}$)|(^[0-9]{3,4}[0-9]{3,8}$)|(^[0-9]{3,8}$)|(^\([0-9]{3,4}\)[0-9]{3,8}$)|(^0{0,1}13[0-9]{9}$)/;
	return reg.test(Trim(phone));

}
*/


function IsValidPhone(val)
{
   if (val.length > 20) return false;
   val = val.split(" ").join("");
   val = val.split("-").join("");
   if(val.indexOf("+")==0) val=val.substring(1);
   // check whether you have a numberic value
   return !isNaN(val);
}


function IsValidMobile(phone)
{
    var reg = /(^0{0,1}[0-9]{11}$)/;
	return reg.test(Trim(phone));

}




function GetToday()
{
    var today = new Date();
    var year = today.getYear().toString();
    var month = (today.getMonth()+1).toString();
    var day = today.getDate().toString();

    if(month.length == 1)
    {
        month = "0"+month;
    }
    if(day.length == 1)
    {
        day = "0"+day;
    }
    var strToday = year+month+day;
    return strToday;
}


function ChnStrLen(str,maxLen)
{
	var len = str.length;
	var chnLen = 0;
	var flag = true;
	for(var i=0; i<len; i++)
	{
		if((parseInt(str.charCodeAt(i))<0)||(parseInt(str.charCodeAt(i))>255))
		{
			chnLen += 2;
			flag = false;
		}
		else
		{
			chnLen += 1;
		}
	}
	return (maxLen >= chnLen);

}


function TruncStr(str,len)
{
	var retStr = "";
	var incrLen = 0;
	for(var i=0; i<str.length; i++)
	{
		if(IsAscii(str.charAt(i)))
		{
			retStr += str.charAt(i);
			incrLen += 1;
		}
		else
		{
			retStr += str.charAt(i);
			incrLen += 2;
		}
		if(incrLen >= 2*len)
		{
			break;
		}
	}
	return retStr;
}


function MyAlert(theText,notice)
{
	  alert(notice);
	  theText.focus();
	  theText.select();
}

function trMouseOver(element) {
    addClass(element, "bgOn");
}

function trMouseOut(element) {
    removeClass(element, "bgOn");
}

function hasClass(element, className) {
    var reg = new RegExp('(\\s|^)' + className + '(\\s|$)');
    return element.className.match(reg);
}

function addClass(element, className) {
    if (!this.hasClass(element, className)) {
        element.className += " " + className; 
    }
}

function removeClass(element, className) {
    if (hasClass(element, className)) {
        var reg = new RegExp('(\\s|^)' + className + '(\\s|$)');
        element.className = element.className.replace(reg, ' ');
    }
}

function GetDisplayAmt(sAmt)//金额转换
{
    var sIntPart = "";
    var sFracPart = "";
    var nDotIndex = sAmt.toString().lastIndexOf('.');
    if (nDotIndex >= 0)
    {
        sIntPart = sAmt.toString().substr(0, nDotIndex);
        sFracPart = sAmt.toString().substring(nDotIndex);  //包含小数点
        if(sFracPart.length==1)
        {
            sFracPart = sFracPart+"00";
        }
        if(sFracPart.length==2)
        {
            sFracPart = sFracPart+"0";
        }
        sAmt=sIntPart;
    }
    var j = 0;
    var i = 0;
    for(; i < sAmt.length; i++)
    {
        if(sAmt.substr(i,1)>0)
        {
            j++;
            break;
        }
    }
    if(j>0)
    {
        sAmt=sAmt.toString().substring(j+i-1);
    }
    else
    {
        sAmt=0;
        return sAmt+sFracPart;
    }
    var nGrpNum = sAmt.length / 3;
    var nRemainNum = sAmt.length % 3;
    if(nGrpNum>1)
    {
        var sbResult="";
        sbResult+=sAmt.toString().substring(0, nRemainNum);
        for (var i = 0; i < nGrpNum; i++)
        {
            sbResult+=",";
            sbResult+=sAmt.toString().substr(nRemainNum + i * 3, 3);
        }
        sAmt=sbResult;
    }
    if(sAmt.substring(0,1)==",")
    {
        sAmt=sAmt.substring(1);
    }
    
    if(sAmt.substring(sAmt.length-1)==",")
    {
        sAmt=sAmt.substring(0,sAmt.length-1);
    }
    sAmt+=sFracPart;
    
    return sAmt;

}

function IsValidInt(str,intMaxLength) {
    if (str == null || Trim(str) == "") {
        return false;
    }
    if(str.length>intMaxLength)
    {
       return false;
    }
    var reg = /(^[1-9][0-9]*$)/;
    return reg.test(str);          
}

function IsValidFloat(str,intMaxLength,floatMaxLength)
{
    if (str == null || Trim(str) == "") {
        return false;
    }
    var reg = /(^(([1-9]\d*(\.\d+){0,1})|(0\.\d*[1-9]\d*))$)/;
    if(!reg.test(str))
    {
       return false;
    }
    var site=str.indexOf(".");
    var intLength=0;
    var floatLength=0;
    if(site<0)
    {
       intLength=str.length;
    }
    else
    {
       intLength=site;
       floatLength=str.length-site-1;
    }
    if(intLength>intMaxLength)
    {
      return false;
    }
    if(floatLength>floatMaxLength)
    {
      return false;
    }
    return true;               
}

//add by wyh 20110901
function GetBrowserVersion() 
{
    var browser = navigator.appName;
    if (browser == "Netscape")
        return "NotIE";
    else
        return "IE";
}

function GetFlashObj(FlashObjName) 
{
    var isIE = navigator.appName.indexOf("Microsoft") != -1;
    return (isIE) ? window[FlashObjName] : document[FlashObjName];
}

function CloseWindow() 
{
    if (GetBrowserVersion() == "IE") 
    {
        window.close();
    }
    else 
    {
        newwin = window.open(location.href, '_parent', '');
        newwin.close();
    }
}

//提示更新非IE浏览器的安全控件Begin
function IfSafeEditExits() {
    var aliedit = null;
    if (window.ActiveXObject) {
        try {
            aliedit = new ActiveXObject('CMBSafe.SafeCtl');
        }
        catch (er) { };
    }
    else {
        aliedit = navigator.plugins['CMBEdit Plugin'] || null;
    }
    return aliedit ? true : false;
}

function GetSafaEdit(name) {
    return document.getElementById(name);
}

function GetVersion(CtrlName) {
    var a = IfSafeEditExits();
    if (a) {
        var b = GetSafaEdit(CtrlName);
        return b.Version;
    }
    else
        return "";
}

function UpdateCtrl(ServerVersion, CtrlName) {
    var a = GetVersion(CtrlName);
    if (ServerVersion > a) {
        alert("请重新下载最新安全控件");
        /*window.open('http://site.cmbchina.com/download/SafeEditInstall.exe');
        window.location.reload();*/
    }
}

function UpdateNotIECtrl(ServerVersion, CtrlName)
{
    if (GetBrowserVersion() == "NotIE") 
    {
        UpdateCtrl(ServerVersion, CtrlName);
    }	
}
//提示更新非IE浏览器的安全控件End

function addEventHandler(oTarget, sEventType, fnHandler) 
{//函数关联到某个事件，例：oTarget为window，sEventType为"load"，fnHandler为JS函数MsgDIVShowPre
    if (oTarget.addEventListener) 
    {//For FireFox等非IE浏览器
        oTarget.addEventListener(sEventType, fnHandler, false);
    }
    else if (oTarget.attachEvent) 
    {//For IE
        oTarget.attachEvent("on" + sEventType, fnHandler);
    }
    else 
    {
        oTarget["on" + sEventType] = fnHandler;
    }
}

function delEventHandler(oTarget, sEventType, fnHandler) 
{//函数撤销关联某个事件，例：oTarget为window，sEventType为"load"，fnHandler为JS函数MsgDIVShowPre
    if (oTarget.removeEventListener) 
    {//For FireFox等非IE浏览器
        oTarget.removeEventListener(sEventType, fnHandler, false);
    }
    else if (oTarget.detachEvent) 
    {//For IE
        oTarget.detachEvent("on" + sEventType, fnHandler);
    }
}

//判断一个4位的纯数字的字符串如1213是否是一个合法的时间
function IsValidInfoTime( stringTime )
{
	var strTime = Trim(stringTime);
	if(strTime.length != 4) return false;
	if(!IsAllDigit(strTime)) return false;
	var nHour = strTime.substring( 0, 2 );
	var nMinute = strTime.substring( 2, 4 );
	if ( nHour >= 24 || nMinute > 60 )
	{
		return false;
	}
	return true;
}

//判断开始时间与结束时间的大小
function IsValidTimePair(fromTime,toTime)
{
	if((fromTime == "")&&(toTime == ""))
	{
		return true;
	}
	if((fromTime == "")||(toTime == ""))
	{
		return false;
	}
	if(fromTime >= toTime)
	{
		return false;
	}
	return true;
}

//判断一个字符串是否含有非法字符
function IsValidDesc(str)
{
    var nLen = str.length;
    for (var i=0; i<nLen; i++)
    {
        if (str.charAt(i)=='<' || str.charAt(i)=='>' || str.charAt(i)=='\'' || str.charAt(i)=='\"')
        {
            return false;
        }
    }

    if (str.indexOf("&lt")>=0 || str.indexOf("&gt")>=0)
    {
        return false;
    }

    return true;
}

function setInnerText(o, s) 
{
	while (o.childNodes.length != 0) 
	{
		o.removeChild(o.childNodes[0]);
	}
	o.appendChild(document.createTextNode(s));
}


function getInnerText(o) 
{
	var sRet = "";
	for (var i = 0; i < o.childNodes.length; i++) 
	{
		if (o.childNodes[i].childNodes.length != 0) 
		{
			sRet += getInnerText(o.childNodes[i]);
		}

		if (o.childNodes[i].nodeValue) 
		{
			if (o.currentStyle.display == "block") 
			{
				sRet += o.childNodes[i].nodeValue + "\n";
			} 
			else 
			{
				sRet += o.childNodes[i].nodeValue;
			}
		}
	}
	return sRet;
}

function getCurrentStyle(obj)
{
	if(!window.getComputedStyle){
		window.getComputedStyle=function(obj){
			return obj.currentStyle;
		};
	}
	return getComputedStyle(obj, null);
}

function extendPrototypeOfInnertext()
{
	if (/Firefox/.test(window.navigator.userAgent)) 
	{
		HTMLElement.prototype.__defineGetter__("innerText", function() {
			return getInnerText(this);
		})

		HTMLElement.prototype.__defineSetter__("innerText", function(s) {
			setInnerText(this, s);
		})
	}
	if(window.getComputedStyle)
		HTMLElement.prototype.__defineGetter__("currentStyle", function() {
			return getCurrentStyle(this);
	});
		
}

function OnCopy(obj) {
    try {
        if (obj != null) {
            var rng = window.document.body.createTextRange();
            rng.moveToElementText(obj);
            rng.execCommand("copy");
        }
        else {
            var rng = window.document.body.createTextRange();
            rng.execCommand("copy");
        }
    }
    catch (e) {
        alert("您的浏览器版本太低，请升级您的浏览器！");
    }
}
 

function OnPrint(obj) {
 
    try {
        if (obj != null) {
            obj.focus();
            obj.print();
        }
        else {
            window.focus();
            window.print();
        }
    }
    catch (e) {
        alert("您的浏览器版本太低，请升级您的浏览器！");
    }
}


function next(result) 
{
    if (window.event) 
    {
        window.event.returnValue = result;
    }
    return result;
}

function GetBrowserType()
{
   if(navigator.userAgent.toLowerCase().indexOf("msie")>0)return "ie";//IE
   if(navigator.userAgent.toLowerCase().indexOf("firefox")>0)return "firefox";//Firefox
   if(navigator.userAgent.toLowerCase().indexOf("chrome")>0)return "chrome";//Chrome
   if(navigator.userAgent.toLowerCase().indexOf("safari")>0)return "safari";//Safari
   return "other";
}

function InvokeClick(element) 
{
	if(element.click)element.click();
	else if(element.fireEvent)element.fireEvent('onclick');
	else if(document.createEvent)
	{
		var evt = document.createEvent("MouseEvents"); 
		evt.initEvent("click", true, true); 
		element.dispatchEvent(evt); 
	}
}

//阻止事件冒泡的通用函数
function stopBubble(e)
{
	if(e&&e.stopPropagation){
		e.stopPropagation();
	}else{
		window.event.cancelBubble = true;
	}
}

function InitEditCtrls()
{
	var browserType=GetBrowserType();
	if(browserType=="ie" || browserType=="firefox")
		return;
	if(arguments.length >= 1)
	{
		for(var i=0;i<arguments.length;i++) 
		{
			EnforceInitEditCtrl(arguments[i]);
		}
	}
}

function EnforceInitEditCtrl(targetId)
{
	var funcName = "init"+targetId;
	if(funcName in window)
	{
		try
		{
			if(typeof(eval(funcName)) == "function")
			{
				eval(funcName+"()");
			}
		}
		catch(e)
		{
			//alert("not function");
		}
	}
}

function InitGlobalEditCtrls()
{
	var browserType=GetBrowserType();
	if(browserType=="ie" || browserType=="firefox")
		return;
	var objTags = document.getElementsByTagName("OBJECT");
	for(var i=0;i<objTags.length;i++)
	{
		if(objTags[i].id.trim() != "")
			EnforceInitEditCtrl(objTags[i].id);
	}
}

//判断全角字符
function chkHalf(str)
{    
  if(/[\x00-\xFF]/.test(str))
  {  
    return false;   
  }  
  else
  {  
    return true;   
  }     
}
//判断一个字符串是否含有全角或者半角空格
function IsConAllSpace(str)
{
    var nLen = str.length;
    for (var i=0; i<nLen; i++)
    {
        if ((str.charCodeAt(i) == 12288) || (str.charCodeAt(i) == 32))
        {
            return false;
        }
    }
    return true;
}

//半角转换为全角函数不包括半角空格
function HalfToAll(str) 
{ 
    var tmp = str; 
    for(var i=0;i<str.length;i++) 
    {  
        if(str.charCodeAt(i)<127 && str.charCodeAt(i)!=32) 
        { 
            tmp= tmp.substring(0,i) + String.fromCharCode(str.charCodeAt(i)+65248) + tmp.substring(i+1,str.length); 
        } 
    }
    return tmp; 
}	
//获取字符串所含字符数，汉字2个字符
function getStrLen(str)
{
    var len = 0;
    var cnstrCount = 0; 
    for(var i = 0 ; i < str.length ; i++)
    {
      if(str.charCodeAt(i)>255)
       cnstrCount = cnstrCount + 1 ;
    }
    len = str.length + cnstrCount;
    return len;
}

//转账汇款业务节假日提示--js控制
function holidayTipOfTransfer(rootNode) 
{
    var showHolidayTip = 0;
    var rootNode = document.getElementById(rootNode);
    if(showHolidayTip)
    {
        var tipText = "温馨提示：受端午节及人民银行系统维护影响，6月21日-6月24日期间，您的跨行转账业务可能会受影响或暂停处理，敬请谅解，建议您提前做好相关资金安排。";
        var divBorder = document.createElement("div")
        divBorder.style.cssText = "width: 100%; border: 1px solid #94CFF3; margin-bottom:5px; margin-top:5px;";
        var tableNode = document.createElement("table");
        tableNode.style.cssText = "margin:1px 3px 1px 3px;";
        var rowNode = tableNode.insertRow(0);
        var cellImg = rowNode.insertCell(0);
        var cellText = rowNode.insertCell(1);
        var img = document.createElement("img");
        var spanNode = document.createElement("span");
        img.setAttribute("src", "../doc/Images/tips.gif");
        cellImg.setAttribute("valign", "top");
        cellImg.appendChild(img);
        spanNode.style.cssText = "color: #31682e;letter-spacing: 1px;";
        spanNode.innerHTML = tipText;
        cellText.appendChild(spanNode);
        divBorder.appendChild(tableNode);
        rootNode.appendChild(divBorder);
    }
    else
    {
        rootNode.className = "tdSpaceH12";
    }
}

function InsertSafeEditNode(safeEditID) 
{
    if(!window.ActiveXObject)
    {            
        var safeEditList = new Array();
        var safeEditParentNode = document.getElementById(safeEditID).parentNode;
        
        for(var i=0; i<safeEditParentNode.childNodes.length; i++)
        {
            if(safeEditParentNode.childNodes[i].nodeType==1 && safeEditParentNode.childNodes[i].tagName.toUpperCase()=="OBJECT")
            {
                safeEditList.push(safeEditParentNode.childNodes[i]);
            }
        }
        
        for(var j=0; j<safeEditList.length; j++)
        {
            safeEditParentNode.removeChild(safeEditList[j]);
        }
        
        var fakeSite_Table = document.createElement("table");
        var fakeSite_Tr = fakeSite_Table.insertRow(0);
        var fakeSite_Td = fakeSite_Tr.insertCell(0);
        
        for(var j=0; j<safeEditList.length; j++)
        {
            safeEditList[j].width=0;
            safeEditList[j].height=0;
            safeEditList[j].style.width=0;
            safeEditList[j].style.height=0;
            fakeSite_Td.appendChild(safeEditList[j]);
        }
        
        document.forms[0].appendChild(fakeSite_Table);
        
        EnforceInitEditCtrl(safeEditID);
    }            
}