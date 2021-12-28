var DefaultStyle = ["../doc/Styles/GenShell.css","../doc/Styles/Menu.css","../doc/Styles/VMenu.css"];
var DefaultStyleObj = [];

var NetbankStyle = ["../doc/Styles/GenShell-Netbank.css","../doc/Styles/Menu-Netbank.css","../doc/Styles/VMenu-Netbank.css"];
var NetbankstyleObj = [];

function EnableCssStyle(styleObjArray)
{
	var len = styleObjArray.length;
	for(var i=0; i<len; i++)
	{
		styleObjArray[i].disabled = false;
	}
}
function DisableCssStyle(styleObjArray)
{
	var len = styleObjArray.length;
	for(var i=0; i<len; i++)
	{
		styleObjArray[i].disabled = true;
	}
}
//Disable all Created CSS objs, edit it when needed
function DisableAllCssStyle()
{
	DisableCssStyle(DefaultStyleObj);
	DisableCssStyle(NetbankstyleObj);
}
function CreateCssStyle(styleArray,styleObjArray,theDocument)
{
	if(styleObjArray.len > 0) return;
	var len = styleArray.length;
	for(var i=0; i<len; i++)
	{
		styleObjArray[i] = theDocument.createStyleSheet(styleArray[i]);
	}
}

function SetStyle(styleType,bStore2Cookie,theDocument)
{
	var cookieName = "Cmb_GenServer_Style";
	if(theDocument){}
	else
	{
		theDocument = document;
	}
	var styleCookie = new Cookie(theDocument, cookieName, 24*365*10);
	if(styleType){}
	else
	{
		if(styleCookie.load())
		{
			styleType = styleCookie.style;
		}
		else
		{
			styleType = "default";
		}
	}
	var styleObjArray;
	var styleArray;
	if(styleType == "netbank")
	{
		styleObjArray = NetbankstyleObj;
		styleArray = NetbankStyle;
	}
	else
	{
		styleObjArray = DefaultStyleObj;
		styleArray = DefaultStyle;
	}
	if(styleObjArray.length <= 0)
	{
		CreateCssStyle(styleArray,styleObjArray,theDocument);
	}
	DisableAllCssStyle();
	EnableCssStyle(styleObjArray);
	//alert(styleType);
	if(bStore2Cookie)
	{
		styleCookie.style = styleType;
		styleCookie.store();
	}
}