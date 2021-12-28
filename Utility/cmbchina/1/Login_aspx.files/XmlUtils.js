
//Create a XmlDOM Object
function createXmlDom() 
{
	var xmlDom = new ActiveXObject("Microsoft.XMLDOM"); //MSXML2.DOMDocument
	
}
//Create a XmlHttp Object
function createXmlHttp() 
{
	var xmlhttp = false;
	// branch for native XMLHttpRequest object
    if(window.XMLHttpRequest)
	{
    	try
		{
			xmlhttp = new XMLHttpRequest();
	    }
		catch(e)
		{
			xmlhttp = false;
			alert("window.XMLHttpRequest:" + e.message);	//add alert by wlf 20060726
	    }
	}
	// branch for IE/Windows ActiveX version
	else if(window.ActiveXObject)
	{
       	try
		{
        		xmlhttp = new ActiveXObject("Msxml2.XMLHTTP");
	     }
		catch(e)
		{
        	try
			{
          			xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
	       	}
			catch(e)
			{
          			xmlhttp = false;
					alert("window.ActiveXObject:" + e.message);	//add alert by wlf 20060726
	       	}
		}
    }
	return xmlhttp;
}

function processXmlHttpResponse()
{
	if(xmlHttpObj.readyState == 4)
	{ 
		if(xmlHttpObj.status == 200)
		{ 
          alert(xmlHttpObj.responseText);
	    } 
		else
		{ 
          alert(xmlHttpObj.statusText); 
		} 
	} 
}

//The common method: send xmlhttp request to server,if bRetXmlObj is true,return responseXML,else return responseText.
function sendXmlHttpRequest(url,params,bFormSubmit,bRetXmlObj)
{
	if(xmlHttpObj)
	{
  		if(bFormSubmit == undefined)
  		{
  			bFormSubmit = true;
  		}
  		if(bRetXmlObj == undefined)
  		{ 
  			bRetXmlObj = true;
  		}
  		
  		//xmlHttpObj.onreadystatechange = processXmlHttpResponse; 
		xmlHttpObj.open("POST",url,false);
		//xmlHttpObj.setRequestHeader("Content-Length",params.length); 
		//Send Xml base data
		//xmlHttpObj.setRequestHeader("Content-Type",""text/xml; charset=utf-8""); 
		//Send form base data
		if(bFormSubmit)
		{
			xmlHttpObj.setRequestHeader("Content-Type","application/x-www-form-urlencoded"); 
		}
		xmlHttpObj.send(params);
		
  		//alert(xmlHttpObj.responseText);
  		//alert(bRetXmlObj);
  		
  		if(bRetXmlObj)
  		{
  			return xmlHttpObj.responseXML; 
  		}
  		else
  		{
			return xmlHttpObj.responseText; 
		}
  		
	}
	else
	{
		alert("XmlHttpObj Init failed!");
		return false;
	}
}

function getXmlData(xmlObj,nodename)
{
	var item = xmlObj.documentElement.getElementsByTagName(nodename);
	return item(0).text;
}




