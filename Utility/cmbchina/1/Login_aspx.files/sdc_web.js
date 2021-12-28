
var gDomain="sdc.cmbchina.com";	// SDC Production Mode Domain
var gDcsId="dcs4ns95v10000kbeqtivijo3_4v5j";
var gFpc="WTFPC";
var gConvert=true;
var gWTIDJS=window.document.createElement("script"); 

window.document.getElementsByTagName("head")[0].appendChild(gWTIDJS); 

if ((typeof(gConvert)!="undefined")&&gConvert&&(document.cookie.indexOf(gFpc+"=")==-1)&&(document.cookie.indexOf("WTLOPTOUT=")==-1)){
 gWTIDJS.src="http"+(window.location.protocol.indexOf('https:')==0?'s':'')+"://"+gDomain+"/"+gDcsId+"/wtid.js"; 
}

//setTimeout('setsdcjs()',0);
//function setsdcjs(){   
//   var js_path="/CmbBank_GenShell/UI/GenShellPC/doc/Scripts/sdc_cmb.js";
//   /*
//   if (window.location.protocol.indexOf('https:')==0)
//   {
//		js_path="https://www.cmbchina.com/cmb2005web/script/sdc_cmb.js";
//   }
//   */                     
//   var SDC_js=document.createElement("script");   
//   SDC_js.src=js_path;
//   var headElem=document.getElementsByTagName("head")[0];
//   headElem.appendChild(SDC_js);   
//}