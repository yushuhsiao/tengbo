(function () { // date.js
Date.CultureInfo={name:"en-US",englishName:"English (United States)",nativeName:"English (United States)",dayNames:["Sunday","Monday","Tuesday","Wednesday","Thursday","Friday","Saturday"],abbreviatedDayNames:["Sun","Mon","Tue","Wed","Thu","Fri","Sat"],shortestDayNames:["Su","Mo","Tu","We","Th","Fr","Sa"],firstLetterDayNames:["S","M","T","W","T","F","S"],monthNames:["January","February","March","April","May","June","July","August","September","October","November","December"],abbreviatedMonthNames:["Jan","Feb","Mar","Apr","May","Jun","Jul","Aug","Sep","Oct","Nov","Dec"],amDesignator:"AM",pmDesignator:"PM",firstDayOfWeek:0,twoDigitYearMax:2029,dateElementOrder:"mdy",formatPatterns:{shortDate:"M/d/yyyy",longDate:"dddd, MMMM dd, yyyy",shortTime:"h:mm tt",longTime:"h:mm:ss tt",fullDateTime:"dddd, MMMM dd, yyyy h:mm:ss tt",sortableDateTime:"yyyy-MM-ddTHH:mm:ss",universalSortableDateTime:"yyyy-MM-dd HH:mm:ssZ",rfc1123:"ddd, dd MMM yyyy HH:mm:ss GMT",monthDay:"MMMM dd",yearMonth:"MMMM, yyyy"},regexPatterns:{jan:/^jan(uary)?/i,feb:/^feb(ruary)?/i,mar:/^mar(ch)?/i,apr:/^apr(il)?/i,may:/^may/i,jun:/^jun(e)?/i,jul:/^jul(y)?/i,aug:/^aug(ust)?/i,sep:/^sep(t(ember)?)?/i,oct:/^oct(ober)?/i,nov:/^nov(ember)?/i,dec:/^dec(ember)?/i,sun:/^su(n(day)?)?/i,mon:/^mo(n(day)?)?/i,tue:/^tu(e(s(day)?)?)?/i,wed:/^we(d(nesday)?)?/i,thu:/^th(u(r(s(day)?)?)?)?/i,fri:/^fr(i(day)?)?/i,sat:/^sa(t(urday)?)?/i,future:/^next/i,past:/^last|past|prev(ious)?/i,add:/^(\+|after|from)/i,subtract:/^(\-|before|ago)/i,yesterday:/^yesterday/i,today:/^t(oday)?/i,tomorrow:/^tomorrow/i,now:/^n(ow)?/i,millisecond:/^ms|milli(second)?s?/i,second:/^sec(ond)?s?/i,minute:/^min(ute)?s?/i,hour:/^h(ou)?rs?/i,week:/^w(ee)?k/i,month:/^m(o(nth)?s?)?/i,day:/^d(ays?)?/i,year:/^y((ea)?rs?)?/i,shortMeridian:/^(a|p)/i,longMeridian:/^(a\.?m?\.?|p\.?m?\.?)/i,timezone:/^((e(s|d)t|c(s|d)t|m(s|d)t|p(s|d)t)|((gmt)?\s*(\+|\-)\s*\d\d\d\d?)|gmt)/i,ordinalSuffix:/^\s*(st|nd|rd|th)/i,timeContext:/^\s*(\:|a|p)/i},abbreviatedTimeZoneStandard:{GMT:"-000",EST:"-0400",CST:"-0500",MST:"-0600",PST:"-0700"},abbreviatedTimeZoneDST:{GMT:"-000",EDT:"-0500",CDT:"-0600",MDT:"-0700",PDT:"-0800"}}
Date.getMonthNumberFromName=function(n){for(var i=Date.CultureInfo.monthNames,u=Date.CultureInfo.abbreviatedMonthNames,r=n.toLowerCase(),t=0;t<i.length;t++)if(i[t].toLowerCase()==r||u[t].toLowerCase()==r)return t;return-1},Date.getDayNumberFromName=function(n){for(var i=Date.CultureInfo.dayNames,u=Date.CultureInfo.abbreviatedDayNames,f=Date.CultureInfo.shortestDayNames,r=n.toLowerCase(),t=0;t<i.length;t++)if(i[t].toLowerCase()==r||u[t].toLowerCase()==r)return t;return-1},Date.isLeapYear=function(n){return n%4==0&&n%100!=0||n%400==0},Date.getDaysInMonth=function(n,t){return[31,Date.isLeapYear(n)?29:28,31,30,31,30,31,31,30,31,30,31][t]},Date.getTimezoneOffset=function(n,t){return t||!1?Date.CultureInfo.abbreviatedTimeZoneDST[n.toUpperCase()]:Date.CultureInfo.abbreviatedTimeZoneStandard[n.toUpperCase()]},Date.getTimezoneAbbreviation=function(n,t){var r=t||!1?Date.CultureInfo.abbreviatedTimeZoneDST:Date.CultureInfo.abbreviatedTimeZoneStandard,i;for(i in r)if(r[i]===n)return i;return null},Date.prototype.clone=function(){return new Date(this.getTime())},Date.prototype.compareTo=function(n){if(isNaN(this))throw new Error(this);if(n instanceof Date&&!isNaN(n))return this>n?1:this<n?-1:0;throw new TypeError(n);},Date.prototype.equals=function(n){return this.compareTo(n)===0},Date.prototype.between=function(n,t){var i=this.getTime();return i>=n.getTime()&&i<=t.getTime()},Date.prototype.addMilliseconds=function(n){return this.setMilliseconds(this.getMilliseconds()+n),this},Date.prototype.addSeconds=function(n){return this.addMilliseconds(n*1e3)},Date.prototype.addMinutes=function(n){return this.addMilliseconds(n*6e4)},Date.prototype.addHours=function(n){return this.addMilliseconds(n*36e5)},Date.prototype.addDays=function(n){return this.addMilliseconds(n*864e5)},Date.prototype.addWeeks=function(n){return this.addMilliseconds(n*6048e5)},Date.prototype.addMonths=function(n){var t=this.getDate();return this.setDate(1),this.setMonth(this.getMonth()+n),this.setDate(Math.min(t,this.getDaysInMonth())),this},Date.prototype.addYears=function(n){return this.addMonths(n*12)},Date.prototype.add=function(n){if(typeof n=="number")return this._orient=n,this;var t=n;return(t.millisecond||t.milliseconds)&&this.addMilliseconds(t.millisecond||t.milliseconds),(t.second||t.seconds)&&this.addSeconds(t.second||t.seconds),(t.minute||t.minutes)&&this.addMinutes(t.minute||t.minutes),(t.hour||t.hours)&&this.addHours(t.hour||t.hours),(t.month||t.months)&&this.addMonths(t.month||t.months),(t.year||t.years)&&this.addYears(t.year||t.years),(t.day||t.days)&&this.addDays(t.day||t.days),this},Date._validate=function(n,t,i,r){if(typeof n!="number")throw new TypeError(n+" is not a Number.");else if(n<t||n>i)throw new RangeError(n+" is not a valid value for "+r+".");return!0},Date.validateMillisecond=function(n){return Date._validate(n,0,999,"milliseconds")},Date.validateSecond=function(n){return Date._validate(n,0,59,"seconds")},Date.validateMinute=function(n){return Date._validate(n,0,59,"minutes")},Date.validateHour=function(n){return Date._validate(n,0,23,"hours")},Date.validateDay=function(n,t,i){return Date._validate(n,1,Date.getDaysInMonth(t,i),"days")},Date.validateMonth=function(n){return Date._validate(n,0,11,"months")},Date.validateYear=function(n){return Date._validate(n,1,9999,"seconds")},Date.prototype.set=function(n){var t=n;return t.millisecond||t.millisecond===0||(t.millisecond=-1),t.second||t.second===0||(t.second=-1),t.minute||t.minute===0||(t.minute=-1),t.hour||t.hour===0||(t.hour=-1),t.day||t.day===0||(t.day=-1),t.month||t.month===0||(t.month=-1),t.year||t.year===0||(t.year=-1),t.millisecond!=-1&&Date.validateMillisecond(t.millisecond)&&this.addMilliseconds(t.millisecond-this.getMilliseconds()),t.second!=-1&&Date.validateSecond(t.second)&&this.addSeconds(t.second-this.getSeconds()),t.minute!=-1&&Date.validateMinute(t.minute)&&this.addMinutes(t.minute-this.getMinutes()),t.hour!=-1&&Date.validateHour(t.hour)&&this.addHours(t.hour-this.getHours()),t.month!==-1&&Date.validateMonth(t.month)&&this.addMonths(t.month-this.getMonth()),t.year!=-1&&Date.validateYear(t.year)&&this.addYears(t.year-this.getFullYear()),t.day!=-1&&Date.validateDay(t.day,this.getFullYear(),this.getMonth())&&this.addDays(t.day-this.getDate()),t.timezone&&this.setTimezone(t.timezone),t.timezoneOffset&&this.setTimezoneOffset(t.timezoneOffset),this},Date.prototype.clearTime=function(){return this.setHours(0),this.setMinutes(0),this.setSeconds(0),this.setMilliseconds(0),this},Date.prototype.isLeapYear=function(){var n=this.getFullYear();return n%4==0&&n%100!=0||n%400==0},Date.prototype.isWeekday=function(){return!(this.is().sat()||this.is().sun())},Date.prototype.getDaysInMonth=function(){return Date.getDaysInMonth(this.getFullYear(),this.getMonth())},Date.prototype.moveToFirstDayOfMonth=function(){return this.set({day:1})},Date.prototype.moveToLastDayOfMonth=function(){return this.set({day:this.getDaysInMonth()})},Date.prototype.moveToDayOfWeek=function(n,t){var i=(n-this.getDay()+7*(t||1))%7;return this.addDays(i===0?i+=7*(t||1):i)},Date.prototype.moveToMonth=function(n,t){var i=(n-this.getMonth()+12*(t||1))%12;return this.addMonths(i===0?i+=12*(t||1):i)},Date.prototype.getDayOfYear=function(){return Math.floor((this-new Date(this.getFullYear(),0,1))/864e5)},Date.prototype.getWeekOfYear=function(n){var t=this.getFullYear(),e=this.getMonth(),o=this.getDate(),s=n||Date.CultureInfo.firstDayOfWeek,r=8-new Date(t,0,1).getDay(),f,i,u;return r==8&&(r=1),f=(Date.UTC(t,e,o,0,0,0)-Date.UTC(t,0,1,0,0,0))/864e5+1,i=Math.floor((f-r+7)/7),i===s&&(t--,u=8-new Date(t,0,1).getDay(),i=u==2||u==8?53:52),i},Date.prototype.isDST=function(){return console.log("isDST"),this.toString().match(/(E|C|M|P)(S|D)T/)[2]=="D"},Date.prototype.getTimezone=function(){return Date.getTimezoneAbbreviation(this.getUTCOffset,this.isDST())},Date.prototype.setTimezoneOffset=function(n){var t=this.getTimezoneOffset(),i=Number(n)*-6/10;return this.addMinutes(i-t),this},Date.prototype.setTimezone=function(n){return this.setTimezoneOffset(Date.getTimezoneOffset(n))},Date.prototype.getUTCOffset=function(){var t=this.getTimezoneOffset()*-10/6,n;return t<0?(n=(t-1e4).toString(),n[0]+n.substr(2)):(n=(t+1e4).toString(),"+"+n.substr(1))},Date.prototype.getDayName=function(n){return n?Date.CultureInfo.abbreviatedDayNames[this.getDay()]:Date.CultureInfo.dayNames[this.getDay()]},Date.prototype.getMonthName=function(n){return n?Date.CultureInfo.abbreviatedMonthNames[this.getMonth()]:Date.CultureInfo.monthNames[this.getMonth()]},Date.prototype._toString=Date.prototype.toString,Date.prototype.toString=function(n){var t=this,i=function(n){return n.toString().length==1?"0"+n:n};return n?n.replace(/dd?d?d?|MM?M?M?|yy?y?y?|hh?|HH?|mm?|ss?|tt?|zz?z?/g,function(n){switch(n){case"hh":return i(t.getHours()<13?t.getHours():t.getHours()-12);case"h":return t.getHours()<13?t.getHours():t.getHours()-12;case"HH":return i(t.getHours());case"H":return t.getHours();case"mm":return i(t.getMinutes());case"m":return t.getMinutes();case"ss":return i(t.getSeconds());case"s":return t.getSeconds();case"yyyy":return t.getFullYear();case"yy":return t.getFullYear().toString().substring(2,4);case"dddd":return t.getDayName();case"ddd":return t.getDayName(!0);case"dd":return i(t.getDate());case"d":return t.getDate().toString();case"MMMM":return t.getMonthName();case"MMM":return t.getMonthName(!0);case"MM":return i(t.getMonth()+1);case"M":return t.getMonth()+1;case"t":return t.getHours()<12?Date.CultureInfo.amDesignator.substring(0,1):Date.CultureInfo.pmDesignator.substring(0,1);case"tt":return t.getHours()<12?Date.CultureInfo.amDesignator:Date.CultureInfo.pmDesignator;case"zzz":case"zz":case"z":return""}}):this._toString()},Date.now=function(){return new Date},Date.today=function(){return Date.now().clearTime()},Date.prototype._orient=1,Date.prototype.next=function(){return this._orient=1,this},Date.prototype.last=Date.prototype.prev=Date.prototype.previous=function(){return this._orient=-1,this},Date.prototype._is=!1,Date.prototype.is=function(){return this._is=!0,this},Number.prototype._dateElement="day",Number.prototype.fromNow=function(){var n={};return n[this._dateElement]=this,Date.now().add(n)},Number.prototype.ago=function(){var n={};return n[this._dateElement]=this*-1,Date.now().add(n)},function(){for(var n=Date.prototype,s=Number.prototype,f="sunday monday tuesday wednesday thursday friday saturday".split(/\s/),e="january february march april may june july august september october november december".split(/\s/),o="Millisecond Second Minute Hour Day Week Month Year".split(/\s/),t,a=function(n){return function(){return this._is?(this._is=!1,this.getDay()==n):this.moveToDayOfWeek(n,this._orient)}},h,i,c,l,r,u=0;u<f.length;u++)n[f[u]]=n[f[u].substring(0,3)]=a(u);for(h=function(n){return function(){return this._is?(this._is=!1,this.getMonth()===n):this.moveToMonth(n,this._orient)}},i=0;i<e.length;i++)n[e[i]]=n[e[i].substring(0,3)]=h(i);for(c=function(n){return function(){return n.substring(n.length-1)!="s"&&(n+="s"),this["add"+n](this._orient)}},l=function(n){return function(){return this._dateElement=n,this}},r=0;r<o.length;r++)t=o[r].toLowerCase(),n[t]=n[t+"s"]=c(o[r]),s[t]=s[t+"s"]=l(t)}(),Date.prototype.toJSONString=function(){return this.toString("yyyy-MM-ddThh:mm:ssZ")},Date.prototype.toShortDateString=function(){return this.toString(Date.CultureInfo.formatPatterns.shortDatePattern)},Date.prototype.toLongDateString=function(){return this.toString(Date.CultureInfo.formatPatterns.longDatePattern)},Date.prototype.toShortTimeString=function(){return this.toString(Date.CultureInfo.formatPatterns.shortTimePattern)},Date.prototype.toLongTimeString=function(){return this.toString(Date.CultureInfo.formatPatterns.longTimePattern)},Date.prototype.getOrdinal=function(){switch(this.getDate()){case 1:case 21:case 31:return"st";case 2:case 22:return"nd";case 3:case 23:return"rd";default:return"th"}},function(){var i,o,f,u;Date.Parsing={Exception:function(n){this.message="Parse error at '"+n.substring(0,10)+" ...'"}};var t=Date.Parsing,n=t.Operators={rtoken:function(n){return function(i){var r=i.match(n);if(r)return[r[0],i.substring(r[0].length)];throw new t.Exception(i);}},token:function(){return function(t){return n.rtoken(new RegExp("^s*"+t+"s*"))(t)}},stoken:function(t){return n.rtoken(new RegExp("^"+t))},until:function(n){return function(t){for(var r=[],i=null;t.length;){try{i=n.call(this,t)}catch(u){r.push(i[0]),t=i[1];continue}break}return[r,t]}},many:function(n){return function(t){for(var i=[],r=null;t.length;){try{r=n.call(this,t)}catch(u){return[i,t]}i.push(r[0]),t=r[1]}return[i,t]}},optional:function(n){return function(t){var i=null;try{i=n.call(this,t)}catch(r){return[null,t]}return[i[0],i[1]]}},not:function(n){return function(i){try{n.call(this,i)}catch(r){return[null,i]}throw new t.Exception(i);}},ignore:function(n){return n?function(t){var i=null;return i=n.call(this,t),[null,i[1]]}:null},product:function(){for(var i=arguments[0],u=Array.prototype.slice.call(arguments,1),r=[],t=0;t<i.length;t++)r.push(n.each(i[t],u));return r},cache:function(n){var r={},i=null;return function(u){try{i=r[u]=r[u]||n.call(this,u)}catch(f){i=r[u]=f}if(i instanceof t.Exception)throw i;else return i}},any:function(){var n=arguments;return function(i){for(var r=null,u=0;u<n.length;u++)if(n[u]!=null){try{r=n[u].call(this,i)}catch(f){r=null}if(r)return r}throw new t.Exception(i);}},each:function(){var n=arguments;return function(i){for(var f=[],u=null,r=0;r<n.length;r++)if(n[r]!=null){try{u=n[r].call(this,i)}catch(e){throw new t.Exception(i);}f.push(u[0]),i=u[1]}return[f,i]}},all:function(){var t=arguments,n=n;return n.each(n.optional(t))},sequence:function(i,r,u){return(r=r||n.rtoken(/^\s*/),u=u||null,i.length==1)?i[0]:function(n){for(var f=null,e=null,s=[],o=0;o<i.length;o++){try{f=i[o].call(this,n)}catch(h){break}s.push(f[0]);try{e=r.call(this,f[1])}catch(c){e=null;break}n=e[1]}if(!f)throw new t.Exception(n);if(e)throw new t.Exception(e[1]);if(u)try{f=u.call(this,f[1])}catch(l){throw new t.Exception(f[1]);}return[s,f?f[1]:n]}},between:function(t,i,u){u=u||t;var f=n.each(n.ignore(t),i,n.ignore(u));return function(n){var t=f.call(this,n);return[[t[0][0],r[0][2]],t[1]]}},list:function(t,i,r){return i=i||n.rtoken(/^\s*/),r=r||null,t instanceof Array?n.each(n.product(t.slice(0,-1),n.ignore(i)),t.slice(-1),n.ignore(r)):n.each(n.many(n.each(t,n.ignore(i))),px,n.ignore(r))},set:function(i,r,u){return r=r||n.rtoken(/^\s*/),u=u||null,function(f){for(var s=null,l=null,h=null,c=null,e=[[],f],o=!1,y,a,v=0;v<i.length;v++){h=null,l=null,s=null,o=i.length==1;try{s=i[v].call(this,f)}catch(p){continue}if(c=[[s[0]],s[1]],s[1].length>0&&!o)try{h=r.call(this,s[1])}catch(w){o=!0}else o=!0;if(o||h[1].length!==0||(o=!0),!o){for(y=[],a=0;a<i.length;a++)v!=a&&y.push(i[a]);l=n.set(y,r).call(this,h[1]),l[0].length>0&&(c[0]=c[0].concat(l[0]),c[1]=l[1])}if(c[1].length<e[1].length&&(e=c),e[1].length===0)break}if(e[0].length===0)return e;if(u){try{h=u.call(this,e[1])}catch(b){throw new t.Exception(e[1]);}e[1]=h[1]}return e}},forward:function(n,t){return function(i){return n[t].call(this,i)}},replace:function(n,t){return function(i){var r=n.call(this,i);return[t,r[1]]}},process:function(n,t){return function(i){var r=n.call(this,i);return[t.call(this,r[0]),r[1]]}},min:function(n,i){return function(r){var u=i.call(this,r);if(u[0].length<n)throw new t.Exception(r);return u}}},s=function(n){return function(){var t=null,u=[],i,r;if(arguments.length>1?t=Array.prototype.slice.call(arguments):arguments[0]instanceof Array&&(t=arguments[0]),t)for(i=0,r=t.shift();i<r.length;i++)return t.unshift(r[i]),u.push(n.apply(null,t)),t.shift(),u;else return n.apply(null,arguments)}},e="optional not ignore cache".split(/\s/);for(i=0;i<e.length;i++)n[e[i]]=s(n[e[i]]);for(o=function(n){return function(){return arguments[0]instanceof Array?n.apply(null,arguments[0]):n.apply(null,arguments)}},f="each any all".split(/\s/),u=0;u<f.length;u++)n[f[u]]=o(n[f[u]])}(),function(){var o=function(n){for(var i=[],t=0;t<n.length;t++)n[t]instanceof Array?i=i.concat(o(n[t])):n[t]&&i.push(n[t]);return i},u,f,e;Date.Grammar={},Date.Translator={hour:function(n){return function(){this.hour=Number(n)}},minute:function(n){return function(){this.minute=Number(n)}},second:function(n){return function(){this.second=Number(n)}},meridian:function(n){return function(){this.meridian=n.slice(0,1).toLowerCase()}},timezone:function(n){return function(){var t=n.replace(/[^\d\+\-]/g,"");t.length?this.timezoneOffset=Number(t):this.timezone=n.toLowerCase()}},day:function(n){var t=n[0];return function(){this.day=Number(t.match(/\d+/)[0])}},month:function(n){return function(){this.month=n.length==3?Date.getMonthNumberFromName(n):Number(n)-1}},year:function(n){return function(){var t=Number(n);this.year=n.length>2?t:t+(t+2e3<Date.CultureInfo.twoDigitYearMax?2e3:1900)}},rday:function(n){return function(){switch(n){case"yesterday":this.days=-1;break;case"tomorrow":this.days=1;break;case"today":this.days=0;break;case"now":this.days=0,this.now=!0}}},finishExact:function(n){var r,t,i;for(n=n instanceof Array?n:[n],r=new Date,this.year=r.getFullYear(),this.month=r.getMonth(),this.day=1,this.hour=0,this.minute=0,this.second=0,t=0;t<n.length;t++)n[t]&&n[t].call(this);if(this.hour=this.meridian=="p"&&this.hour<13?this.hour+12:this.hour,this.day>Date.getDaysInMonth(this.year,this.month))throw new RangeError(this.day+" is not a valid value for days.");return i=new Date(this.year,this.month,this.day,this.hour,this.minute,this.second),this.timezone?i.set({timezone:this.timezone}):this.timezoneOffset&&i.set({timezoneOffset:this.timezoneOffset}),i},finish:function(n){var f,r,t,u,i,e;if(n=n instanceof Array?o(n):[n],n.length===0)return null;for(f=0;f<n.length;f++)typeof n[f]=="function"&&n[f].call(this);return this.now?new Date:(i=Date.today(),e=!!(this.days!=null||this.orient||this.operator),e?(u=this.orient=="past"||this.operator=="subtract"?-1:1,this.weekday&&(this.unit="day",r=Date.getDayNumberFromName(this.weekday)-i.getDay(),t=7,this.days=r?(r+u*t)%t:u*t),this.month&&(this.unit="month",r=this.month-i.getMonth(),t=12,this.months=r?(r+u*t)%t:u*t,this.month=null),this.unit||(this.unit="day"),(this[this.unit+"s"]==null||this.operator!=null)&&(this.value||(this.value=1),this.unit=="week"&&(this.unit="day",this.value=this.value*7),this[this.unit+"s"]=this.value*u),i.add(this)):(this.meridian&&this.hour&&(this.hour=this.hour<13&&this.meridian=="p"?this.hour+12:this.hour),this.weekday&&!this.day&&(this.day=i.addDays(Date.getDayNumberFromName(this.weekday)-i.getDay()).getDate()),this.month&&!this.day&&(this.day=1),i.set(this)))}};var t=Date.Parsing.Operators,n=Date.Grammar,i=Date.Translator,r;n.datePartDelimiter=t.rtoken(/^([\s\-\.\,\/\x27]+)/),n.timePartDelimiter=t.stoken(":"),n.whiteSpace=t.rtoken(/^\s*/),n.generalDelimiter=t.rtoken(/^(([\s\,]|at|on)+)/),u={},n.ctoken=function(n){var r=u[n],i;if(!r){var o=Date.CultureInfo.regexPatterns,f=n.split(/\s+/),e=[];for(i=0;i<f.length;i++)e.push(t.replace(t.rtoken(o[f[i]]),f[i]));r=u[n]=t.any.apply(null,e)}return r},n.ctoken2=function(n){return t.rtoken(Date.CultureInfo.regexPatterns[n])},n.h=t.cache(t.process(t.rtoken(/^(0[0-9]|1[0-2]|[1-9])/),i.hour)),n.hh=t.cache(t.process(t.rtoken(/^(0[0-9]|1[0-2])/),i.hour)),n.H=t.cache(t.process(t.rtoken(/^([0-1][0-9]|2[0-3]|[0-9])/),i.hour)),n.HH=t.cache(t.process(t.rtoken(/^([0-1][0-9]|2[0-3])/),i.hour)),n.m=t.cache(t.process(t.rtoken(/^([0-5][0-9]|[0-9])/),i.minute)),n.mm=t.cache(t.process(t.rtoken(/^[0-5][0-9]/),i.minute)),n.s=t.cache(t.process(t.rtoken(/^([0-5][0-9]|[0-9])/),i.second)),n.ss=t.cache(t.process(t.rtoken(/^[0-5][0-9]/),i.second)),n.hms=t.cache(t.sequence([n.H,n.mm,n.ss],n.timePartDelimiter)),n.t=t.cache(t.process(n.ctoken2("shortMeridian"),i.meridian)),n.tt=t.cache(t.process(n.ctoken2("longMeridian"),i.meridian)),n.z=t.cache(t.process(t.rtoken(/^(\+|\-)?\s*\d\d\d\d?/),i.timezone)),n.zz=t.cache(t.process(t.rtoken(/^(\+|\-)\s*\d\d\d\d/),i.timezone)),n.zzz=t.cache(t.process(n.ctoken2("timezone"),i.timezone)),n.timeSuffix=t.each(t.ignore(n.whiteSpace),t.set([n.tt,n.zzz])),n.time=t.each(t.optional(t.ignore(t.stoken("T"))),n.hms,n.timeSuffix),n.d=t.cache(t.process(t.each(t.rtoken(/^([0-2]\d|3[0-1]|\d)/),t.optional(n.ctoken2("ordinalSuffix"))),i.day)),n.dd=t.cache(t.process(t.each(t.rtoken(/^([0-2]\d|3[0-1])/),t.optional(n.ctoken2("ordinalSuffix"))),i.day)),n.ddd=n.dddd=t.cache(t.process(n.ctoken("sun mon tue wed thu fri sat"),function(n){return function(){this.weekday=n}})),n.M=t.cache(t.process(t.rtoken(/^(1[0-2]|0\d|\d)/),i.month)),n.MM=t.cache(t.process(t.rtoken(/^(1[0-2]|0\d)/),i.month)),n.MMM=n.MMMM=t.cache(t.process(n.ctoken("jan feb mar apr may jun jul aug sep oct nov dec"),i.month)),n.y=t.cache(t.process(t.rtoken(/^(\d\d?)/),i.year)),n.yy=t.cache(t.process(t.rtoken(/^(\d\d)/),i.year)),n.yyy=t.cache(t.process(t.rtoken(/^(\d\d?\d?\d?)/),i.year)),n.yyyy=t.cache(t.process(t.rtoken(/^(\d\d\d\d)/),i.year)),r=function(){return t.each(t.any.apply(null,arguments),t.not(n.ctoken2("timeContext")))},n.day=r(n.d,n.dd),n.month=r(n.M,n.MMM),n.year=r(n.yyyy,n.yy),n.orientation=t.process(n.ctoken("past future"),function(n){return function(){this.orient=n}}),n.operator=t.process(n.ctoken("add subtract"),function(n){return function(){this.operator=n}}),n.rday=t.process(n.ctoken("yesterday tomorrow today now"),i.rday),n.unit=t.process(n.ctoken("minute hour day week month year"),function(n){return function(){this.unit=n}}),n.value=t.process(t.rtoken(/^\d\d?(st|nd|rd|th)?/),function(n){return function(){this.value=n.replace(/\D/g,"")}}),n.expression=t.set([n.rday,n.operator,n.value,n.unit,n.orientation,n.ddd,n.MMM]),r=function(){return t.set(arguments,n.datePartDelimiter)},n.mdy=r(n.ddd,n.month,n.day,n.year),n.ymd=r(n.ddd,n.year,n.month,n.day),n.dmy=r(n.ddd,n.day,n.month,n.year),n.date=function(t){return(n[Date.CultureInfo.dateElementOrder]||n.mdy).call(this,t)},n.format=t.process(t.many(t.any(t.process(t.rtoken(/^(dd?d?d?|MM?M?M?|yy?y?y?|hh?|HH?|mm?|ss?|tt?|zz?z?)/),function(t){if(n[t])return n[t];throw Date.Parsing.Exception(t);}),t.process(t.rtoken(/^[^dMyhHmstz]+/),function(n){return t.ignore(t.stoken(n))}))),function(n){return t.process(t.each.apply(null,n),i.finishExact)}),f={},e=function(t){return f[t]=f[t]||n.format(t)[0]},n.formats=function(n){var r,i;if(n instanceof Array){for(r=[],i=0;i<n.length;i++)r.push(e(n[i]));return t.any.apply(null,r)}return e(n)},n._formats=n.formats(["yyyy-MM-ddTHH:mm:ss","ddd, MMM dd, yyyy H:mm:ss tt","ddd MMM d yyyy HH:mm:ss zzz","d"]),n._start=t.process(t.set([n.date,n.time,n.expression],n.generalDelimiter,n.whiteSpace),i.finish),n.start=function(t){try{var i=n._formats.call({},t);if(i[1].length===0)return i}catch(r){}return n._start.call({},t)}}(),Date._parse=Date.parse,Date.parse=function(n){var t=null;if(!n)return null;try{t=Date.Grammar.start.call({},n)}catch(i){return null}return t[1].length===0?t[0]:null},Date.getParseFunction=function(n){var t=Date.Grammar.formats(n);return function(n){var i=null;try{i=t.call({},n)}catch(r){return null}return i[1].length===0?i[0]:null}},Date.parseExact=function(n,t){return Date.getParseFunction(t)(n)}
})();

function writelog(n, t) { console.log(n, t) }

function theme_change() {
    var href = '../{0}'.format(window.top.$('#css_jquery_ui_theme_d').attr('href'));
    (function _send() {
        if (this.$) {
            this.$('iframe').each(function () { _send.apply(this.contentWindow); });
            this.$('#css_jquery_ui_theme').attr('href', href);
        }
    }).apply(window.top);
}

function sendMessage(msg, data) {
    //console.log(window.location.pathname, { msg: msg, data: data });
    (function _send() {
        if (this.$) {
            this.$('iframe').each(function () { _send.apply(this.contentWindow); });
            if (this.recvMessage != null)
                if (this.$.isFunction(this.recvMessage[msg]))
                    this.recvMessage[msg](data);
        }
    }).apply(window.top);
}

function sendMsg(state, code, msg, args) { sendMessage('dbgmsg', { state: state, code: code, msg: msg, args: args }); }


function iframe_auto_height(o) {
    var o = $.extend({
        init: 0,
        interval: 500,
        duration: 300
    }, o);
    var lastHeight = 0;// document.body.clientHeight;
    function run() {
        if (lastHeight == document.body.clientHeight) {
            timer = setTimeout(run, o.interval);
        }
        else {
            lastHeight = document.body.clientHeight
            $(window.frameElement).animate(
                { height: document.body.offsetHeight },
                { queue: false, duration: o.duration, complete: run });
        }
    };
    setTimeout(run, o.init);
}


(function ($) {
    $.empty = function () { return ''; }
    $.fn.fmatter.datejs = function (cellval, opts) {
        var format, formatNaN;
        if (opts.colModel && opts.colModel.formatoptions) {
            format = opts.colModel.formatoptions.format;
            formatNaN = opts.colModel.formatoptions.formatNaN;
        }
        if (cellval) {
            var d = Date.fromISO(cellval);
            if (($.type(d) == 'date') && !isNaN(d))
                return d.toString(format || 'yyyy-MM-dd\r\nHH:mm:ss');
        }
        return formatNaN || '';
    };
    $.fn.fmatter.xml = function (cellval, opts) {
        var ret = $.jgrid.htmlEncode(cellval);
        if (ret === undefined) return '';
        return ret;
    };
    $.fn.fmatter.alias = function (cellval, opts, rwd, act) {
        var src = $(this).colModel(opts.colModel.formatoptions);// find_colModel(this.p.colModel, opts.colModel.formatoptions.alias);
        if (src) return this.formatter(opts.rowId, rwd[src.name], this.p.colModel.indexOf(src), rwd, act);
    };
    $.fn.fmatter.empty = function (cellval, opts) {
        return "";
    }
    $.fn.fmatter.currency2 = function (cellval, opts) {
        var op = $.extend({}, opts.currency);
        if (opts.colModel !== undefined && opts.colModel.formatoptions !== undefined) {
            op = $.extend({}, op, opts.colModel.formatoptions);
        }
        if ($.fmatter.isEmpty(cellval)) {
            return '';
        }
        return $.fmatter.util.NumberFormat(cellval, op);
    };

    $.fn.colTypes = {
        Field: /*    */ function (colModel) { return { width: 080, sorttype: 'text', editable: false, editonce: false, formatter: function (cellval, opts, rwd, act) { return colModel.label || colModel.name; } }; },
        Money: /*    */ function (colModel) { return { width: 080, sorttype: 'currency', editable: false, formatter: 'currency' }; },
        Money2: /*   */ function (colModel) { return { width: 080, sorttype: 'currency', editable: false, formatter: 'currency2' }; },
        Percent: /*  */ function (colModel) { return { width: 050, sorttype: 'text    ', editable: false }; },
        ACNT: /*     */ function (colModel) { return { width: 080, sorttype: 'text    ', editable: true, editonce: true }; },
        ACNT2: /*    */ function (colModel) { return { width: 080, sorttype: 'text    ', editable: false, editonce: false, fixed: true }; },
        ID: /*       */ function (colModel) { return { width: 050, sorttype: 'int     ', editable: false, fixed: true, hidden: true, key: true }; },
        CorpID: { width: 075, sorttype: 'int', editable: true, editonce: true, hidden: false, formatter: 'select', edittype: 'select', editoptions: {}, search: true, stype: 'select', searchoptions: {} },
        ACTime: /*   */ function (colModel) { return { width: 080, formatter: 'datejs', formatoptions: { format: 'yyyy-MM-dd', formatNaN: 'N/A' } }; },
        DateTime: /* */ function (colModel) { return { width: (colModel.nowrap == false ? 80 : 150), sorttype: 'date    ', editable: false, formatter: 'datejs', formatoptions: { format: 'yyyy-MM-dd' + (colModel.nowrap == false ? '\r\n' : ' ') + 'HH:mm:ss', formatNaN: 'N/A' } } },
        DateTime2: /**/ function (colModel) { return { width: (colModel.nowrap == false ? 80 : 150), sorttype: 'date    ', editable: false, formatter: 'datejs', formatoptions: { format: 'yyyy-MM-dd' + (colModel.nowrap == false ? '\r\n' : ' ') + 'HH:mm:ss', formatNaN: 'N/A' } } },
        Bonus: /*    */ function (colModel) { return { width: 080, sorttype: 'int     ', editable: true, edittype: 'text' }; },
        Currency: /* */ function (colModel) { return { width: 075, sorttype: 'text    ', editable: true, editonce: true, formatter: 'select', edittype: 'select', editoptions: { value: enums.currency_code } }; },
        Locked: /*   */ function (colModel) { return { width: 060, sorttype: 'int     ', editable: true, formatter: 'select', edittype: 'select', editoptions: { value: enums.locked }, search: true, stype: 'select', searchoptions: { defaultValue: 255, nullKey: 255, nullValue: '--', value: enums.locked, } }; }
    }
    
    function trim_string(obj) {
        for (var n in obj) {
            var obj2 = obj[n];
            if (obj2 instanceof jQuery) continue;
            if (obj2.document == document) continue;
            //if (obj2 instanceof HTMLElement) continue;
            if (typeof obj[n] == 'string')
                obj[n] = $.trim(obj[n]);
            else if (typeof obj[n] == 'object')
                trim_string(obj[n]);
        }
    }
    //function trim_htmlattr($obj) {
    //    if ($obj.document == document) {
    //    //if ($obj instanceof HTMLElement) {
    //        for (var i = 0; i < $obj.attributes.length; i++)
    //            try { $obj.attributes[i].nodeValue = $.trim($obj.attributes[i].nodeValue); } catch (e) { }
    //        for (var i = 0; i < $obj.childNodes.length; i++)
    //            trim_htmlattr($obj.childNodes[i])
    //    }
    //}

    function get_preset(pin) {
        var p_preset = {
            url: 'api', editurl: 'api', cellurl: 'api', mtype: 'POST', datatype: 'json', cmTemplate: { align: 'center' },
            rownumbers: true, // false
            shrinkToFit: false, forceFit: true, autowidth: true,
            sortable: true, sortname: 'CreateTime', sortorder: "desc",
            multiselect: false,
            jsonReader: { repeatitems: false },
            dock: 'fill', // 'fill', 'bottom', 'left', 'right', 'fill'
            cmTemplate: { align: 'center', sortable: true, search: false },
            headervisible: true, headerclass: '',

            subGridOptions: {
                plusicon: "ui-icon-triangle-1-e",
                minusicon: "ui-icon-triangle-1-s",
                openicon: "ui-icon-arrowreturn-1-e",
                selectOnExpand: true,
                selectOnCollapse: true,

                //expandOnLoad: true,
                //delayOnLoad: 50,
                //selectOnExpand: false,
                //reloadOnExpand: true,
            },
            //subGridBeforeExpand: function (pID, id) { return this.raiseevent.call(this, pin, 'subGridBeforeExpand', arguments); },
            //subGridRowExpanded: function (pID, id) { return this.raiseevent.call(this, pin, 'subGridRowExpanded', arguments); },
            //subGridRowColapsed: function (pID, id) { return this.raiseevent.call(this, pin, 'subGridRowColapsed', arguments); },
            //serializeSubGridData: function (sPostData) { return this.raiseevent.call(this, pin, 'serializeSubGridData', arguments); },

            page: 1,
            lastpage: 0,
            pager: "",
            pagerpos: 'center',
            pgbuttons: true,
            pginput: true,
            reccount: 0,
            recordpos: 'right',
            records: 0,
            //recordtext: null,
            rowList: pin.rowList || [10, 20, 30, 50, 100, 200, 500], // []
            rowNum: 50, // 20
            rownumWidth: 25,
            viewrecords: true, // false
            rowTotal: null,
            toppager: false,

            recordtext: $.jgrid.defaults.recordtext,
            emptyrecords: $.jgrid.defaults.emptyrecords,
            loadtext: $.jgrid.defaults.loadtext,
            pgtext: $.jgrid.defaults.pgtext,
            onPaging: function (pgButton) { },
            toolbar: [false, ""],
            filterToolbar: { enabled: true, initVisible: false, autosearch: true, searchOnEnter: false },
            navButton_action: {
                addRow: function () {
                    $(this).addRow()
                },
                reloadGrid: function () {
                    $(this).reloadGrid();
                },
                toggleSearch: function () {
                    $(this)[0].toggleToolbar();
                    $(this).gridSize(window);
                }
            }

        };
        return $.extend(true, p_preset, pin || {});
    }

    function init_grid(pin) {
        var $t = this;
        if (this.grid) { return; }
        this.console = pin.console || { log: $.noop };
        //this.console = window.console;
        this.raiseevent = function (obj, name, args) {
            this.console.log(name, args);
            if (obj) {
                var fn = obj[name];
                if ($.isFunction(fn))
                    return fn.apply(this, args);
            }
        };

        trim_string(pin);

        var GridRequest = function (postData) {
            return { str: JSON.stringify(this.raiseevent.call(this, pin, 'SelectCommand', [postData]) || postData) };
        };
        var GridResponse = function (data, status, xhr) {
            if (data.colVisible) {
                $t.colVisible(data, status, xhr, data.colVisible);
                delete data.colVisible;
            }
            return this.raiseevent.call(this, pin, 'beforeProcessing', arguments) || (data.rows != null);
        };
        var GridComplete = function () {
            //console.log(arguments);
            for (var i = 0; i < $t.p.colModel.length; i++) {
                var cm = $t.p.colModel[i];
                if ($.isFunction(cm.cell_init)) {
                    $('td[aria-describedby="' + this.p.id + '_' + cm.name + '"]', this).each(function () {
                        var $ind = $(this).closest('tr.jqgrow');
                        var rowid = $ind.attr('id');
                        cm.cell_init.call($t, this, cm, i, rowid);
                    });
                    //console.log('aria-describedby', this.p.id + '_' + cm.name);
                }
            }
            //$($t.p.colModel).each(function () { if ($.isFunction(this.action_init)) this.action_init.call($t, this); });
            return this.raiseevent.call(this, pin, 'gridComplete', arguments);
        };
        var RowRequest = function (postData) {
            if (postData.oper == this.p.prmNames.addoper) {
                postData.oper = 'insert'; return { str: JSON.stringify(this.raiseevent.call(this, pin, 'InsertCommand', [postData]) || postData) };
            }
            else if (postData.oper == this.p.prmNames.editoper) {
                postData.oper = 'update'; return { str: JSON.stringify(this.raiseevent.call(this, pin, 'UpdateCommand', [postData]) || postData) };
            }
            else if (postData.oper == this.p.prmNames.deloper) {
                postData.oper = 'delete'; return { str: JSON.stringify(this.raiseevent.call(this, pin, 'DeleteCommand', [postData]) || postData) };
            }
        };
        var RowResponse = function (res, rowid, o) {
            var obj;
            try{
                obj = $.parseJSON(res.responseText);
            } catch (ex) {
                sendMsg('error', null, ex.message, res.responseText);
                throw ex;
            }
            if (obj.Status == 1) {
                //obj.row = this.raiseevent.call(this, pin.editParams, 'UpdateSuccess', [res, rowid, obj.row]) || data;
                obj.row = this.raiseevent.call(this, pin, 'RowResponse', [res, rowid, obj.row]) || obj.row;
                sendMsg('normal', null, 'UpdateSuccess.');
                //data = this.raiseevent.call(this, pin, 'RowResponse', [res, rowid, o]) || data;
                return [true, obj.row];
            }
            else if (obj.Message) {
                sendMsg('error', obj.Status, obj.Message, obj.args);
            }
            else
                sendMsg('error', null, res.responseText);
            return false;
            //if ($.isArray(obj)) {
            //    var $t = this, ret = obj[0];//, res_rowid = obj[2];
            //    if (ret == true) {
            //        var data = obj[1];
            //        obj[1] = this.raiseevent.call(this, pin.editParams, 'UpdateSuccess', [res, rowid, data]) || data;
            //        addMsg('normal', 'UpdateSuccess.');
            //    }
            //    else if (ret == false) {
            //        var msg = obj[1];
            //        if (msg) {
            //            addMsg('error', res.responseText);
            //            if ((msg == 'AlreadyExist') || (msg == 'UpdateMissing') || (msg == 'FieldNeeds')) {
            //                o.errorfunc = null;
            //            }
            //            var args = $.makeArray(obj).slice(2);
            //            this.raiseevent.call(this, pin.editParams, "UpdateError_" + msg, args);
            //        }
            //    }
            //    return obj;
            //}
        };
        var CellRequest = function (postData) {
        };
        var editParams = {
            keys: true,
            successfunc: null,
            url: null,
            extraparam: {},
            restoreAfterError: false,
            mtype: "POST",

            beforeEditRow: function (o, rowid) { return this.raiseevent.call(this, pin.editParams, 'beforeEditRow', arguments); },
            oneditfunc: function (rowid) { return this.raiseevent.call(this, pin.editParams, 'oneditfunc', arguments); },
            beforeSaveRow: function (o, rowid) { return this.raiseevent.call(this, pin.editParams, 'beforeSaveRow', arguments); },
            successfunc: RowResponse,
            aftersavefunc: function (rowid, res) { return this.raiseevent.call(this, pin.editParams, 'aftersavefunc', arguments); },
            beforeCancelRow: function (o, rowid) { return this.raiseevent.call(this, pin.editParams, 'beforeCancelRow', arguments); },
            afterrestorefunc: function (rowid) { return this.raiseevent.call(this, pin.editParams, 'afterrestorefunc', arguments); },
            errorfunc: function (rowid, res, stat) {
                if (res.status != 200)
                    sendMsg('error', "HTTP " + res.status, res.statusText);
                return this.raiseevent.call(this, pin.editParams, 'errorfunc', arguments);
            }
        };
        var addRowParams = {
            rowID: null,
            initdata: {},
            //position: "first",
            useDefValues: true,
            useFormatter: false,
            addRowParams: editParams,

            // beforeAddRow
            beforeAddRow: function (addRowParams) { return this.raiseevent.call(this, pin.addRowParams, 'beforeAddRow', arguments); },
            afterInsertRow: function (rowid, rowdata, rowelem) { return this.raiseevent.call(this, pin.addRowParams, 'afterInsertRow', arguments); }
        };
        $t.addRow = function () {
            $($t).addRow($t.p.addRowParams)
        };
        $t.colVisible = function (data, status, xhr, cols) {
            for (var col in cols) {
                if (cols[col] == true)
                    $($t).showCol(col);
                else if (cols[col] == false)
                    $($t).hideCol(col);
            }
        };

        var p = get_preset(pin);

        $.fn.colTypes.Buttons = function (colModel) {
            return {
                label: ' ', width: 090, frozen: true, search: false, editable: false, sortable: false, fixed: true, align: 'left',
                action: {
                    editRow: function (ind, rowid) { $($t).editRow(rowid); },
                    saveRow: function (ind, rowid) { $($t).saveRow(rowid); },
                    restoreRow: function (ind, rowid) { $($t).restoreRow(rowid); },
                    delRow: function (ind, rowid) { $($t).delRow(rowid); }
                },
                cell_init: function (cell, cm, cellIndex, _rowid) {
                    if (cm.action && cm.action._dom) {
                        if (!cell._buttons) {
                            var data = {}, opts = {};
                            try {
                                data = $(this).getRowData(_rowid);
                                opts = JSON.parse(data[cm.name]);
                            }
                            catch (ex) {
                            }
                            cell._buttons = cm.action._dom.children().clone(true, true).each(function () {
                                var $this = $(this);
                                var opt = $.extend({ icons: { primary: $this.attr('icon') }, disabled: $this.attr('disabled') == 'disabled' }, opts[$this.attr('action')]);
                                $this.button(opt).click(function () {
                                    var action = cm.action[$this.attr('action')];
                                    if ($.isFunction(action)) {
                                        var $ind = $this.closest('tr.jqgrow');
                                        var rowid = $ind.attr('id');
                                        action.call(this, $ind[0], rowid);
                                        //console.log({ rowid: rowid, action: action, cm: cm, $cell: $cell });
                                    }
                                }).removeClass('ui-state-default');
                                //console.log({ obj: this, action: action, icon: icon });
                            });
                            $(cell).text('');
                        }
                        cell._buttons.appendTo(cell)
                        $(cell).addClass('inline-buttons').css('white-space', 'nowrap');
                    }
                }
            }
        };

        //function colModel(pin_cm) { var _super = this; } colModel.prototype.constructor = colModel;

        var custom_edit = {
            checkboxs: {
                custom_element: function (value, options, elem) {
                    var elem = [];
                    var input = [];
                    var change = function () {
                        if (options.multiple == false) {
                            if (this.checked) {
                                for (var i = 0; i < input.length; i++) {
                                    if (input[i] != this)
                                        input[i].checked = false;
                                }
                            }
                        }
                    }
                    for (var n in options.value) {
                        var $input = $('<input id="{0}_{1}" type="checkbox" value="{1}" />'.format(options.id, n, options.value[n])).change(change);
                        elem.push($input[0]);
                        input.push($input[0]);
                        $('<label for="{0}_{1}">{2}</label>'.format(options.id, n, options.value[n])).each(function () {
                            elem.push(this);
                        });
                        if (options.nowrap === false)
                            elem.push($('<br/>')[0]);
                    }
                    return $(elem);
                },
                custom_value: function (elem, action, value, options) {
                    if (options.multiple == true) {
                        var values = [];
                        $(elem).find(':checked').each(function () {
                            values.push($(this).val());
                        });
                        return values;
                    }
                    if (options.multiple == false) {
                        var value = $(elem).find(':checked:first').val();
                        if (value)
                            return value;
                    }
                    return 0;
                }
            }
        }

        $('tr.colModel td', $t).each(function () {
            var _name = $(this).attr('name');
            if (_name != null)
                $(this).attr('name', $.trim(_name));
        });

        for (var i = 0; i < p.colModel.length; i++) {
            var cm = p.colModel[i];
            cm.sorttype = $.trim(cm.sorttype);
            if (custom_edit.hasOwnProperty(cm.edittype)) {
                cm.editoptions = cm.editoptions || {};
                $.extend(true, cm.editoptions, custom_edit[cm.edittype]);
                cm.edittype = 'custom';
            }
            cm._dom = $('tr.colModel td[name="' + cm.name + '"]', $t);
            if (cm._dom.length > 0) {
                for (var j = 0; j < cm._dom[0].attributes.length; j++) {
                    if (cm._dom[0].attributes[j].name == 'name') continue;
                    cm[cm._dom[0].attributes[j].name] = $.trim(cm._dom[0].attributes[j].value);
                }
            }
            cm._dom.children().each(function () {
                var $tag = $(this);
                var prop = $tag.attr('property');
                if (prop) {
                    cm[prop] = cm[prop] || {};
                    cm[prop]._dom = $tag;
                    $tag.remove();
                }
            });

            var colType = cm.colType;
            if ($.fn.colTypes.hasOwnProperty(colType)) {
                var colType_dst = $.fn.colTypes[colType];
                if ($.isFunction(colType_dst))
                    colType_dst = colType_dst(cm);
                cm = $.extend(true, colType_dst, cm);
            }
            if (cm.searchoptions) {
                if ($.isPlainObject(cm.searchoptions.value)) {
                    if (cm.searchoptions.hasOwnProperty('nullKey') && cm.searchoptions.hasOwnProperty('nullValue')) {
                        cm.searchoptions.value = $.extend({}, cm.searchoptions.value);
                        cm.searchoptions.value[cm.searchoptions.nullKey] = cm.searchoptions.nullValue;
                    }
                }
            }

            p.colModel[i] = cm;
        }
        //console.log(p.colModel);

        $.extend(true, p, {
            // replaces
            //colModel: $.colType.init(pin.colModel),
            addRowParams: addRowParams, editParams: editParams,

            serializeGridData: GridRequest,
            serializeRowData: RowRequest,
            serializeCellData: CellRequest,
            afterInsertRow: addRowParams.afterInsertRow,


            beforeRequest: function () { return this.raiseevent.call(this, pin, 'beforeRequest', arguments); },
            loadBeforeSend: function (xhr, settings) { return this.raiseevent.call(this, pin, 'loadBeforeSend', arguments); },
            beforeProcessing: GridResponse,
            gridComplete: GridComplete,
            loadComplete: function (data) { return this.raiseevent.call(this, pin, 'loadComplete', arguments); },
            //beforeSelectRow: function (rowid, e) { return _invoke.call(this, 'beforeSelectRow', arguments) || true; },
            //onCellSelect: function (rowid, iCol, cellcontent, e) { return _invoke.call(this, 'onCellSelect', arguments); },
            //loadError: function (xhr, status, error) { return _invoke.call(this, 'loadError', arguments); },
            //ondblClickRow: function (rowid, iRow, iCol, e) { return _invoke.call(this, 'ondblClickRow', arguments); },
            //onHeaderClick: function (gridstate) { return _invoke.call(this, 'onHeaderClick', arguments); },
            //onPaging: function (pgButton) { return _invoke.call(this, 'onPaging', arguments); },
            //onRightClickRow: function (rowid, iRow, iCol, e) { return _invoke.call(this, 'onRightClickRow', arguments); },
            //onSelectAll: function (aRowids, status) { return _invoke.call(this, 'onSelectAll', arguments); },
            //onSortCol: function (index, iCol, sortorder) { return _invoke.call(this, 'onSortCol', arguments); },
            //resizeStart: function (event, index) { return _invoke.call(this, 'resizeStart', arguments); },
            //resizeStop: function (newwidth, index) { return _invoke.call(this, 'resizeStop', arguments); },
            subGridBeforeExpand: function (pID, id, ind) {
                if ($(ind).hasClass('jqgrid-new-row'))
                    return false;
                return this.raiseevent.call(this, pin, 'subGridBeforeExpand', arguments);
            },
            subGridRowCreated: function (pID, id, ind, tablediv) {
                return this.raiseevent.call(this, pin, 'subGridRowCreated', arguments);
            },
            subGridRowExpanded: function (pID, id, ind, tablediv) {
                return this.raiseevent.call(this, pin, 'subGridRowExpanded', arguments);
            },
            subGridBeforeColapsed: function (pID, id, ind, tablediv) {
                return this.raiseevent.call(this, pin, 'subGridBeforeColapsed', arguments);
            },
            subGridRowRemoved: function (pID, id, ind, tablediv) {
                return this.raiseevent.call(this, pin, 'subGridRowRemoved', arguments);
            },
            subGridRowColapsed: function (pID, id, ind, tablediv) {
                return this.raiseevent.call(this, pin, 'subGridRowColapsed', arguments);
            },
            serializeSubGridData: function (sPostData) {
                return this.raiseevent.call(this, pin, 'serializeSubGridData', arguments);
            }
        });

        var nav1 = $('.grid-option *[name=nav1]', $t);
        var nav2 = $('.grid-option *[name=nav2]', $t);
        if (nav1.length == 0) nav1 = $(p.nav1);
        if (nav2.length == 0) nav2 = $(p.nav2);
        p.nav1 = nav1;
        p.nav2 = nav2;

        p.pager = (function (value) {
            if (value == true) {
                var name = $.jgrid.jqID($t.id) + "_pager"
                $($t).after('<div id="' + name + '"></div>');
                return name;
            }
            else if (value == false)
                return '';
            return value;
        })(p.pager);
        
        p.toolbar = [pin.toolbar, [true, 'top'], [true, 'bottom'], [true, 'both']][((p.nav1.length > 0) ? 1 : 0) + ((p.nav2.length > 0) ? 2 : 0)];

        $($t).jqGrid(p);

        var $pager = $($t.p.pager);
        var $toppager = $('#' + $.jgrid.jqID($t.id) + "_toppager")
        $t.grid.$rowheader = $($t.grid.hDiv);
        $t.grid.$rowbody = $($t.grid.bDiv);
        $t.grid.$titlebar = $($t.grid.cDiv);
        $t.grid.$toolbar = $($t.grid.uDiv);
        $t.grid.$bottomtoolbar = $($t.grid.ubDiv);
        $t.grid.$pager = $pager;
        $t.grid.$pager_left = $pager.find($pager.selector + '_left');
        $t.grid.$pager_center = $pager.find($pager.selector + '_center');
        $t.grid.$pager_right = $pager.find($pager.selector + '_right');
        $t.grid.$toppager = $toppager;
        $t.grid.$toppager_left = $toppager.find($toppager.selector + '_left');
        $t.grid.$toppager_center = $toppager.find($toppager.selector + '_center');
        $t.grid.$toppager_right = $toppager.find($toppager.selector + '_right');
        p.nav1.appendTo($t.grid.$toolbar.height('auto'));
        p.nav2.appendTo($t.grid.$bottomtoolbar.height('auto'));

        if (p.filterToolbar) {
            if (p.filterToolbar.enabled == true) {
                $($t).filterToolbar(p.filterToolbar);
                $t.toggleToolbar(p.filterToolbar.initVisible);
            }
        }

        if (p.headervisible == false) {
            $t.grid.$rowheader.hide();
        }
        if (p.headerclass != '') {
            $('.ui-th-column', $t.grid.hDiv).removeClass('ui-state-default').addClass(p.headerclass);
        }

        function navButton_init() {
            var $this = $(this);
            var action = $this.attr('action');
            var disabled = $(this).attr('disabled');
            var opt = { icons: { primary: $(this).attr('icon') } };
            if ((disabled == 'true') || (disabled == 'disabled'))
                opt.disabled = true;
            $this.button(opt).css('border', 0);
            if (p.navButton_action.hasOwnProperty(action)) {
                $this.click(
                    function () {
                        //if ($(this).hasClass('ui-state-disabled')) return;
                        p.navButton_action[action].apply($t);
                    });
            }
        }
        $('[action]', p.nav1).each(navButton_init);
    };

    $.fn.jqGrid_init = function (pin) { return this.each(function () { init_grid.call(this, pin); }); }

    Array.prototype.find = function (start, name, value) {
        for (var i = start; i < this.length; i++) {
            if (this[i][name] == value)
                return i;
        }
    };

    // grid.base.js - function dragEnd()
    function jgrid_resizeColumn(colname, width) {
        return this.each(function () {
            resizeColumn.call(this.grid, this.p, this, colname, width)
        });
    }
    function resizeColumn(p, ts, colname, nw) {
        for (var idx = 0; idx < p.colModel.length; idx++) {
            if (p.colModel[idx].name == colname) {
                //var idx = this.resizing.idx,
                //nw = this.headers[idx].newWidth || this.headers[idx].width;
                //nw = parseInt(nw, 10);
                //this.resizing = false;
                //$("#rs_m" + $.jgrid.jqID(p.id)).css("display", "none");
                p.colModel[idx].width = nw;
                this.headers[idx].width = nw;
                this.headers[idx].el.style.width = nw + "px";
                this.cols[idx].style.width = nw + "px";
                if (this.footers.length > 0) { this.footers[idx].style.width = nw + "px"; }
                if (p.forceFit === true) {
                    nw = this.headers[idx + p.nv].newWidth || this.headers[idx + p.nv].width;
                    this.headers[idx + p.nv].width = nw;
                    this.headers[idx + p.nv].el.style.width = nw + "px";
                    this.cols[idx + p.nv].style.width = nw + "px";
                    if (this.footers.length > 0) { this.footers[idx + p.nv].style.width = nw + "px"; }
                    p.colModel[idx + p.nv].width = nw;
                } else {
                    p.tblwidth = this.newWidth || p.tblwidth;
                    $('table:first', this.bDiv).css("width", p.tblwidth + "px");
                    $('table:first', this.hDiv).css("width", p.tblwidth + "px");
                    this.hDiv.scrollLeft = this.bDiv.scrollLeft;
                    if (p.footerrow) {
                        $('table:first', this.sDiv).css("width", p.tblwidth + "px");
                        this.sDiv.scrollLeft = this.bDiv.scrollLeft;
                    }
                }
                //$(ts).triggerHandler("jqGridResizeStop", [nw, idx]);
                //if ($.isFunction(p.resizeStop)) { p.resizeStop.call(ts, nw, idx); }
            }
        }
    }

    var treeReader = {
        level_field: "level",
        parent_id_field: "Parent",
        leaf_field: "isLeaf",
        expanded_field: "expanded",
        loaded: "loaded",
        icon_field: "icon"
    }
    function treeGridConvertRows(src, dst, p, parent, level) {
        var cnt = 0;
        for (var n = src.find(0, p.treeReader.parent_id_field, parent) ; n != undefined ; n = src.find(n + 1, p.treeReader.parent_id_field, parent)) {
            src[n][p.treeReader.level_field] = level;
            src[n][p.treeReader.expanded_field] = true;
            src[n][p.treeReader.loaded] = true;
            dst.push(src[n]);

            src[n][p.treeReader.leaf_field] = treeGridConvertRows(src, dst, p, src[n][p.ExpandColumn], level + 1) == 0;
            cnt++;
            //console.log(0, n);
        }
        return cnt;
    }
    function jgrid_treeGridConvertRows(data) {
        return this.each(function () {
            var rows1 = data.rows;
            var rows2 = [];
            treeGridConvertRows(rows1, rows2, this.p, null, 0);
            data.rows = rows2;
        });
    }

    $.jgrid.extend({
        reloadGrid: function () {
            this.trigger("reloadGrid");
        },
        colModel: function (name) {
            var colModel = this.getGridParam('colModel');
            if ($.isArray(colModel)) {
                for (var i = 0; i < colModel.length; i++) {
                    if (colModel[i].name == name)
                        return colModel[i];
                }
            }
        },
        resizeColumn: jgrid_resizeColumn,
        treeGridConvertRows: jgrid_treeGridConvertRows,
        gridSize: function (obj) {
            this.gridWidth($(obj).innerWidth());
            this.gridHeight($(obj).innerHeight());
            return this;
        },
        gridWidth: function (width) {
            if (typeof width == 'object')
                width = $(width).innerWidth() - 3;
            if (width) {
                this.setGridWidth(width, false);
            }
            return this.getGridParam('width');
        },
        gridHeight: function (height) {
            if (typeof height == 'object')
                height = $(height).innerHeight() - 3;
            var $t = this[0];
            if (height) {
                var gbox_height = this.gridHeight();
                var grid_height = this.getGridParam('height');
                var diff = gbox_height - grid_height;
                $($t).setGridHeight(Math.max(height - diff, 30), false);
            }
            return this.gridContainer().height();
        },
        gridContainer: function () {
            return this.closest("#gbox_" + $.jgrid.jqID(this[0].p.id));
        },
        getCellObj: function (rowid, col) {
            var $t = this[0];
            if ($t && $t.grid) {
                var ind = this.getInd(rowid, true);
                if (ind) {
                    for (var i = 0; i < $t.p.colModel.length; i++) {
                        if ($t.p.colModel[i].name == col) {
                            return ind.cells[i];
                        }
                    }
                }
            }
        },
        prmNames: function () {
            var $t = this[0];
            if ($t && $t.grid) {
                return $t.p.prmNames;
            }
        },
        subGridRow: function (rowid, func) {
            var $t = this[0];
            if ($t && $t.grid) {
                var $sgrow = $(this.getInd(rowid, true).nextSibling);
                if ($sgrow.hasClass('ui-subgrid'))
                    func.call($sgrow);
            }
        }
    });

})(jQuery);
