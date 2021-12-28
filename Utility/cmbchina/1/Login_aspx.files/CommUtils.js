function LoginInfo(loginId,loginType)
{
	this.loginId = loginId;
	this.loginType = loginType;
}


function ListPropertyNames(obj)
{
	var attrs = "";
	for(var attr in obj) 
	{
		attrs +=attr+"\n";
		
	}
	alert(attrs);
}

function CopyProperties(sourceObj,targetObj)
{
	for(var attr in sourceObj) 
	{
		targetObj[attr] = sourceObj[attr];
	}
}

function XMLGetFirstValue(strContent, strElementName)
{
    var strResult = "";

    var strHeadTag = "<" + strElementName + ">";
    var strTailTag = "</" + strElementName + ">"

    var nHeadPos = strContent.indexOf(strHeadTag);
    if (nHeadPos < 0)
    {
        return strResult;
    }
    var nValueBeginPos = nHeadPos + strHeadTag.length;

    var nValueEndPos = strContent.indexOf(strTailTag, nValueBeginPos);
    if (nValueEndPos < 0)
    {
        return strResult;
    }

    strResult = strContent.substring(nValueBeginPos, nValueEndPos);
    return strResult;
}


function XMLGetAllValue(strContent, strElementName)
{
    var saResult = new Array(0);

    var strHeadTag = "<" + strElementName + ">";
    var strTailTag = "</" + strElementName + ">";

    var nHeadTagLen = strHeadTag.length;
    var nTailTagLen = strTailTag.length;

    var strRemain = strContent;
    while (true)
    {
        var nHeadPos = strRemain.indexOf(strHeadTag);
        if (nHeadPos < 0)
        {
            break;
        }
        var nValueBeginPos = nHeadPos + nHeadTagLen;

        var nValueEndPos = strRemain.indexOf(strTailTag);
        if (nValueEndPos < 0)
        {
            break;
        }

        var strElementValue = strRemain.substring(nValueBeginPos, nValueEndPos);
        saResult.push(strElementValue);

        strRemain = strRemain.substring(nValueEndPos+nTailTagLen);
    }

    return saResult;
}


function XMLGetAllValue2(saResult, strContent, strElementName)
{
    for (var i=0; i<saResult.length; i++)
    {
        saResult[i] = null;
    }
    saResult.length = 0;
    try
    {
        CollectGarbage();
    }
    catch (e)
    {}

    var strHeadTag = "<" + strElementName + ">";
    var strTailTag = "</" + strElementName + ">";

    var nHeadTagLen = strHeadTag.length;
    var nTailTagLen = strTailTag.length;

    var strRemain = strContent;
    while (true)
    {
        var nHeadPos = strRemain.indexOf(strHeadTag);
        if (nHeadPos < 0)
        {
            break;
        }
        var nValueBeginPos = nHeadPos + nHeadTagLen;

        var nValueEndPos = strRemain.indexOf(strTailTag);
        if (nValueEndPos < 0)
        {
            break;
        }

        var strElementValue = strRemain.substring(nValueBeginPos, nValueEndPos);
        saResult.push(strElementValue);

        strRemain = strRemain.substring(nValueEndPos+nTailTagLen);
    }

    return;
}