lastScrollY=0;
function heartBeat(){ 
var diffY;
if (document.documentElement && document.documentElement.scrollTop)
diffY = document.documentElement.scrollTop;
else if (document.body)
diffY = document.body.scrollTop
else
{/*Netscape stuff*/}
percent=.1*(diffY-lastScrollY); 
if(percent>0)percent=Math.ceil(percent); 
else percent=Math.floor(percent); 
document.getElementById("lovexin").style.top=parseInt(document.getElementById
("lovexin").style.top)+percent+"px";
document.getElementById("lovexin1").style.top=parseInt(document.getElementById
("lovexin1").style.top)+percent+"px";
lastScrollY=lastScrollY+percent; 
}
suspendcode14 = "<DIV id=\"lovexin\" style='left:10px;POSITION:absolute;TOP:150px;z-index:999;'><a href=\"javascript:void(0);\" onclick=\"javascript:window.open(' http://www.tengbo8.com/Register.aspx','newwindow')\"><img src=image/left.png border='0'></a></div>"

suspendcode15 = "<DIV id=\"lovexin1\" style='right:10px;POSITION:absolute;TOP:150px;z-index:999;'><a href=\"javascript:void(0);\" onclick=\"javascript:window.open('http://kf1.learnsaas.com/chat/chatClient/chatbox.jsp?companyID=293933&jid=8086264846&lan=zh_CN&tm=1375596097871','newwindow','resizable=yes,width=650,height=520')\"><img src=image/right.png border='0'></a></div>"
//                                                                                                                                                                  http://kf1.learnsaas.com/chat/chatClient/chatbox.jsp?companyID=293933&jid=8086264846&enterurl=\"\"&pagetitle=\"\"&lan=zh_CN&tm=1375596097871
document.write(suspendcode14); 
document.write(suspendcode15); 
window.setInterval("heartBeat()",1);