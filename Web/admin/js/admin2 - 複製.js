﻿(function () { // date.js
    Date.CultureInfo = { name: "en-US", englishName: "English (United States)", nativeName: "English (United States)", dayNames: ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"], abbreviatedDayNames: ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"], shortestDayNames: ["Su", "Mo", "Tu", "We", "Th", "Fr", "Sa"], firstLetterDayNames: ["S", "M", "T", "W", "T", "F", "S"], monthNames: ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"], abbreviatedMonthNames: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"], amDesignator: "AM", pmDesignator: "PM", firstDayOfWeek: 0, twoDigitYearMax: 2029, dateElementOrder: "mdy", formatPatterns: { shortDate: "M/d/yyyy", longDate: "dddd, MMMM dd, yyyy", shortTime: "h:mm tt", longTime: "h:mm:ss tt", fullDateTime: "dddd, MMMM dd, yyyy h:mm:ss tt", sortableDateTime: "yyyy-MM-ddTHH:mm:ss", universalSortableDateTime: "yyyy-MM-dd HH:mm:ssZ", rfc1123: "ddd, dd MMM yyyy HH:mm:ss GMT", monthDay: "MMMM dd", yearMonth: "MMMM, yyyy" }, regexPatterns: { jan: /^jan(uary)?/i, feb: /^feb(ruary)?/i, mar: /^mar(ch)?/i, apr: /^apr(il)?/i, may: /^may/i, jun: /^jun(e)?/i, jul: /^jul(y)?/i, aug: /^aug(ust)?/i, sep: /^sep(t(ember)?)?/i, oct: /^oct(ober)?/i, nov: /^nov(ember)?/i, dec: /^dec(ember)?/i, sun: /^su(n(day)?)?/i, mon: /^mo(n(day)?)?/i, tue: /^tu(e(s(day)?)?)?/i, wed: /^we(d(nesday)?)?/i, thu: /^th(u(r(s(day)?)?)?)?/i, fri: /^fr(i(day)?)?/i, sat: /^sa(t(urday)?)?/i, future: /^next/i, past: /^last|past|prev(ious)?/i, add: /^(\+|after|from)/i, subtract: /^(\-|before|ago)/i, yesterday: /^yesterday/i, today: /^t(oday)?/i, tomorrow: /^tomorrow/i, now: /^n(ow)?/i, millisecond: /^ms|milli(second)?s?/i, second: /^sec(ond)?s?/i, minute: /^min(ute)?s?/i, hour: /^h(ou)?rs?/i, week: /^w(ee)?k/i, month: /^m(o(nth)?s?)?/i, day: /^d(ays?)?/i, year: /^y((ea)?rs?)?/i, shortMeridian: /^(a|p)/i, longMeridian: /^(a\.?m?\.?|p\.?m?\.?)/i, timezone: /^((e(s|d)t|c(s|d)t|m(s|d)t|p(s|d)t)|((gmt)?\s*(\+|\-)\s*\d\d\d\d?)|gmt)/i, ordinalSuffix: /^\s*(st|nd|rd|th)/i, timeContext: /^\s*(\:|a|p)/i }, abbreviatedTimeZoneStandard: { GMT: "-000", EST: "-0400", CST: "-0500", MST: "-0600", PST: "-0700" }, abbreviatedTimeZoneDST: { GMT: "-000", EDT: "-0500", CDT: "-0600", MDT: "-0700", PDT: "-0800" } }
    Date.getMonthNumberFromName = function (n) { for (var i = Date.CultureInfo.monthNames, u = Date.CultureInfo.abbreviatedMonthNames, r = n.toLowerCase(), t = 0; t < i.length; t++) if (i[t].toLowerCase() == r || u[t].toLowerCase() == r) return t; return -1 }, Date.getDayNumberFromName = function (n) { for (var i = Date.CultureInfo.dayNames, u = Date.CultureInfo.abbreviatedDayNames, f = Date.CultureInfo.shortestDayNames, r = n.toLowerCase(), t = 0; t < i.length; t++) if (i[t].toLowerCase() == r || u[t].toLowerCase() == r) return t; return -1 }, Date.isLeapYear = function (n) { return n % 4 == 0 && n % 100 != 0 || n % 400 == 0 }, Date.getDaysInMonth = function (n, t) { return [31, Date.isLeapYear(n) ? 29 : 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31][t] }, Date.getTimezoneOffset = function (n, t) { return t || !1 ? Date.CultureInfo.abbreviatedTimeZoneDST[n.toUpperCase()] : Date.CultureInfo.abbreviatedTimeZoneStandard[n.toUpperCase()] }, Date.getTimezoneAbbreviation = function (n, t) { var r = t || !1 ? Date.CultureInfo.abbreviatedTimeZoneDST : Date.CultureInfo.abbreviatedTimeZoneStandard, i; for (i in r) if (r[i] === n) return i; return null }, Date.prototype.clone = function () { return new Date(this.getTime()) }, Date.prototype.compareTo = function (n) { if (isNaN(this)) throw new Error(this); if (n instanceof Date && !isNaN(n)) return this > n ? 1 : this < n ? -1 : 0; throw new TypeError(n); }, Date.prototype.equals = function (n) { return this.compareTo(n) === 0 }, Date.prototype.between = function (n, t) { var i = this.getTime(); return i >= n.getTime() && i <= t.getTime() }, Date.prototype.addMilliseconds = function (n) { return this.setMilliseconds(this.getMilliseconds() + n), this }, Date.prototype.addSeconds = function (n) { return this.addMilliseconds(n * 1e3) }, Date.prototype.addMinutes = function (n) { return this.addMilliseconds(n * 6e4) }, Date.prototype.addHours = function (n) { return this.addMilliseconds(n * 36e5) }, Date.prototype.addDays = function (n) { return this.addMilliseconds(n * 864e5) }, Date.prototype.addWeeks = function (n) { return this.addMilliseconds(n * 6048e5) }, Date.prototype.addMonths = function (n) { var t = this.getDate(); return this.setDate(1), this.setMonth(this.getMonth() + n), this.setDate(Math.min(t, this.getDaysInMonth())), this }, Date.prototype.addYears = function (n) { return this.addMonths(n * 12) }, Date.prototype.add = function (n) { if (typeof n == "number") return this._orient = n, this; var t = n; return (t.millisecond || t.milliseconds) && this.addMilliseconds(t.millisecond || t.milliseconds), (t.second || t.seconds) && this.addSeconds(t.second || t.seconds), (t.minute || t.minutes) && this.addMinutes(t.minute || t.minutes), (t.hour || t.hours) && this.addHours(t.hour || t.hours), (t.month || t.months) && this.addMonths(t.month || t.months), (t.year || t.years) && this.addYears(t.year || t.years), (t.day || t.days) && this.addDays(t.day || t.days), this }, Date._validate = function (n, t, i, r) { if (typeof n != "number") throw new TypeError(n + " is not a Number."); else if (n < t || n > i) throw new RangeError(n + " is not a valid value for " + r + "."); return !0 }, Date.validateMillisecond = function (n) { return Date._validate(n, 0, 999, "milliseconds") }, Date.validateSecond = function (n) { return Date._validate(n, 0, 59, "seconds") }, Date.validateMinute = function (n) { return Date._validate(n, 0, 59, "minutes") }, Date.validateHour = function (n) { return Date._validate(n, 0, 23, "hours") }, Date.validateDay = function (n, t, i) { return Date._validate(n, 1, Date.getDaysInMonth(t, i), "days") }, Date.validateMonth = function (n) { return Date._validate(n, 0, 11, "months") }, Date.validateYear = function (n) { return Date._validate(n, 1, 9999, "seconds") }, Date.prototype.set = function (n) { var t = n; return t.millisecond || t.millisecond === 0 || (t.millisecond = -1), t.second || t.second === 0 || (t.second = -1), t.minute || t.minute === 0 || (t.minute = -1), t.hour || t.hour === 0 || (t.hour = -1), t.day || t.day === 0 || (t.day = -1), t.month || t.month === 0 || (t.month = -1), t.year || t.year === 0 || (t.year = -1), t.millisecond != -1 && Date.validateMillisecond(t.millisecond) && this.addMilliseconds(t.millisecond - this.getMilliseconds()), t.second != -1 && Date.validateSecond(t.second) && this.addSeconds(t.second - this.getSeconds()), t.minute != -1 && Date.validateMinute(t.minute) && this.addMinutes(t.minute - this.getMinutes()), t.hour != -1 && Date.validateHour(t.hour) && this.addHours(t.hour - this.getHours()), t.month !== -1 && Date.validateMonth(t.month) && this.addMonths(t.month - this.getMonth()), t.year != -1 && Date.validateYear(t.year) && this.addYears(t.year - this.getFullYear()), t.day != -1 && Date.validateDay(t.day, this.getFullYear(), this.getMonth()) && this.addDays(t.day - this.getDate()), t.timezone && this.setTimezone(t.timezone), t.timezoneOffset && this.setTimezoneOffset(t.timezoneOffset), this }, Date.prototype.clearTime = function () { return this.setHours(0), this.setMinutes(0), this.setSeconds(0), this.setMilliseconds(0), this }, Date.prototype.isLeapYear = function () { var n = this.getFullYear(); return n % 4 == 0 && n % 100 != 0 || n % 400 == 0 }, Date.prototype.isWeekday = function () { return !(this.is().sat() || this.is().sun()) }, Date.prototype.getDaysInMonth = function () { return Date.getDaysInMonth(this.getFullYear(), this.getMonth()) }, Date.prototype.moveToFirstDayOfMonth = function () { return this.set({ day: 1 }) }, Date.prototype.moveToLastDayOfMonth = function () { return this.set({ day: this.getDaysInMonth() }) }, Date.prototype.moveToDayOfWeek = function (n, t) { var i = (n - this.getDay() + 7 * (t || 1)) % 7; return this.addDays(i === 0 ? i += 7 * (t || 1) : i) }, Date.prototype.moveToMonth = function (n, t) { var i = (n - this.getMonth() + 12 * (t || 1)) % 12; return this.addMonths(i === 0 ? i += 12 * (t || 1) : i) }, Date.prototype.getDayOfYear = function () { return Math.floor((this - new Date(this.getFullYear(), 0, 1)) / 864e5) }, Date.prototype.getWeekOfYear = function (n) { var t = this.getFullYear(), e = this.getMonth(), o = this.getDate(), s = n || Date.CultureInfo.firstDayOfWeek, r = 8 - new Date(t, 0, 1).getDay(), f, i, u; return r == 8 && (r = 1), f = (Date.UTC(t, e, o, 0, 0, 0) - Date.UTC(t, 0, 1, 0, 0, 0)) / 864e5 + 1, i = Math.floor((f - r + 7) / 7), i === s && (t--, u = 8 - new Date(t, 0, 1).getDay(), i = u == 2 || u == 8 ? 53 : 52), i }, Date.prototype.isDST = function () { return console.log("isDST"), this.toString().match(/(E|C|M|P)(S|D)T/)[2] == "D" }, Date.prototype.getTimezone = function () { return Date.getTimezoneAbbreviation(this.getUTCOffset, this.isDST()) }, Date.prototype.setTimezoneOffset = function (n) { var t = this.getTimezoneOffset(), i = Number(n) * -6 / 10; return this.addMinutes(i - t), this }, Date.prototype.setTimezone = function (n) { return this.setTimezoneOffset(Date.getTimezoneOffset(n)) }, Date.prototype.getUTCOffset = function () { var t = this.getTimezoneOffset() * -10 / 6, n; return t < 0 ? (n = (t - 1e4).toString(), n[0] + n.substr(2)) : (n = (t + 1e4).toString(), "+" + n.substr(1)) }, Date.prototype.getDayName = function (n) { return n ? Date.CultureInfo.abbreviatedDayNames[this.getDay()] : Date.CultureInfo.dayNames[this.getDay()] }, Date.prototype.getMonthName = function (n) { return n ? Date.CultureInfo.abbreviatedMonthNames[this.getMonth()] : Date.CultureInfo.monthNames[this.getMonth()] }, Date.prototype._toString = Date.prototype.toString, Date.prototype.toString = function (n) { var t = this, i = function (n) { return n.toString().length == 1 ? "0" + n : n }; return n ? n.replace(/dd?d?d?|MM?M?M?|yy?y?y?|hh?|HH?|mm?|ss?|tt?|zz?z?/g, function (n) { switch (n) { case "hh": return i(t.getHours() < 13 ? t.getHours() : t.getHours() - 12); case "h": return t.getHours() < 13 ? t.getHours() : t.getHours() - 12; case "HH": return i(t.getHours()); case "H": return t.getHours(); case "mm": return i(t.getMinutes()); case "m": return t.getMinutes(); case "ss": return i(t.getSeconds()); case "s": return t.getSeconds(); case "yyyy": return t.getFullYear(); case "yy": return t.getFullYear().toString().substring(2, 4); case "dddd": return t.getDayName(); case "ddd": return t.getDayName(!0); case "dd": return i(t.getDate()); case "d": return t.getDate().toString(); case "MMMM": return t.getMonthName(); case "MMM": return t.getMonthName(!0); case "MM": return i(t.getMonth() + 1); case "M": return t.getMonth() + 1; case "t": return t.getHours() < 12 ? Date.CultureInfo.amDesignator.substring(0, 1) : Date.CultureInfo.pmDesignator.substring(0, 1); case "tt": return t.getHours() < 12 ? Date.CultureInfo.amDesignator : Date.CultureInfo.pmDesignator; case "zzz": case "zz": case "z": return "" } }) : this._toString() }, Date.now = function () { return new Date }, Date.today = function () { return Date.now().clearTime() }, Date.prototype._orient = 1, Date.prototype.next = function () { return this._orient = 1, this }, Date.prototype.last = Date.prototype.prev = Date.prototype.previous = function () { return this._orient = -1, this }, Date.prototype._is = !1, Date.prototype.is = function () { return this._is = !0, this }, Number.prototype._dateElement = "day", Number.prototype.fromNow = function () { var n = {}; return n[this._dateElement] = this, Date.now().add(n) }, Number.prototype.ago = function () { var n = {}; return n[this._dateElement] = this * -1, Date.now().add(n) }, function () { for (var n = Date.prototype, s = Number.prototype, f = "sunday monday tuesday wednesday thursday friday saturday".split(/\s/), e = "january february march april may june july august september october november december".split(/\s/), o = "Millisecond Second Minute Hour Day Week Month Year".split(/\s/), t, a = function (n) { return function () { return this._is ? (this._is = !1, this.getDay() == n) : this.moveToDayOfWeek(n, this._orient) } }, h, i, c, l, r, u = 0; u < f.length; u++) n[f[u]] = n[f[u].substring(0, 3)] = a(u); for (h = function (n) { return function () { return this._is ? (this._is = !1, this.getMonth() === n) : this.moveToMonth(n, this._orient) } }, i = 0; i < e.length; i++) n[e[i]] = n[e[i].substring(0, 3)] = h(i); for (c = function (n) { return function () { return n.substring(n.length - 1) != "s" && (n += "s"), this["add" + n](this._orient) } }, l = function (n) { return function () { return this._dateElement = n, this } }, r = 0; r < o.length; r++) t = o[r].toLowerCase(), n[t] = n[t + "s"] = c(o[r]), s[t] = s[t + "s"] = l(t) }(), Date.prototype.toJSONString = function () { return this.toString("yyyy-MM-ddThh:mm:ssZ") }, Date.prototype.toShortDateString = function () { return this.toString(Date.CultureInfo.formatPatterns.shortDatePattern) }, Date.prototype.toLongDateString = function () { return this.toString(Date.CultureInfo.formatPatterns.longDatePattern) }, Date.prototype.toShortTimeString = function () { return this.toString(Date.CultureInfo.formatPatterns.shortTimePattern) }, Date.prototype.toLongTimeString = function () { return this.toString(Date.CultureInfo.formatPatterns.longTimePattern) }, Date.prototype.getOrdinal = function () { switch (this.getDate()) { case 1: case 21: case 31: return "st"; case 2: case 22: return "nd"; case 3: case 23: return "rd"; default: return "th" } }, function () { var i, o, f, u; Date.Parsing = { Exception: function (n) { this.message = "Parse error at '" + n.substring(0, 10) + " ...'" } }; var t = Date.Parsing, n = t.Operators = { rtoken: function (n) { return function (i) { var r = i.match(n); if (r) return [r[0], i.substring(r[0].length)]; throw new t.Exception(i); } }, token: function () { return function (t) { return n.rtoken(new RegExp("^s*" + t + "s*"))(t) } }, stoken: function (t) { return n.rtoken(new RegExp("^" + t)) }, until: function (n) { return function (t) { for (var r = [], i = null; t.length;) { try { i = n.call(this, t) } catch (u) { r.push(i[0]), t = i[1]; continue } break } return [r, t] } }, many: function (n) { return function (t) { for (var i = [], r = null; t.length;) { try { r = n.call(this, t) } catch (u) { return [i, t] } i.push(r[0]), t = r[1] } return [i, t] } }, optional: function (n) { return function (t) { var i = null; try { i = n.call(this, t) } catch (r) { return [null, t] } return [i[0], i[1]] } }, not: function (n) { return function (i) { try { n.call(this, i) } catch (r) { return [null, i] } throw new t.Exception(i); } }, ignore: function (n) { return n ? function (t) { var i = null; return i = n.call(this, t), [null, i[1]] } : null }, product: function () { for (var i = arguments[0], u = Array.prototype.slice.call(arguments, 1), r = [], t = 0; t < i.length; t++) r.push(n.each(i[t], u)); return r }, cache: function (n) { var r = {}, i = null; return function (u) { try { i = r[u] = r[u] || n.call(this, u) } catch (f) { i = r[u] = f } if (i instanceof t.Exception) throw i; else return i } }, any: function () { var n = arguments; return function (i) { for (var r = null, u = 0; u < n.length; u++) if (n[u] != null) { try { r = n[u].call(this, i) } catch (f) { r = null } if (r) return r } throw new t.Exception(i); } }, each: function () { var n = arguments; return function (i) { for (var f = [], u = null, r = 0; r < n.length; r++) if (n[r] != null) { try { u = n[r].call(this, i) } catch (e) { throw new t.Exception(i); } f.push(u[0]), i = u[1] } return [f, i] } }, all: function () { var t = arguments, n = n; return n.each(n.optional(t)) }, sequence: function (i, r, u) { return (r = r || n.rtoken(/^\s*/), u = u || null, i.length == 1) ? i[0] : function (n) { for (var f = null, e = null, s = [], o = 0; o < i.length; o++) { try { f = i[o].call(this, n) } catch (h) { break } s.push(f[0]); try { e = r.call(this, f[1]) } catch (c) { e = null; break } n = e[1] } if (!f) throw new t.Exception(n); if (e) throw new t.Exception(e[1]); if (u) try { f = u.call(this, f[1]) } catch (l) { throw new t.Exception(f[1]); } return [s, f ? f[1] : n] } }, between: function (t, i, u) { u = u || t; var f = n.each(n.ignore(t), i, n.ignore(u)); return function (n) { var t = f.call(this, n); return [[t[0][0], r[0][2]], t[1]] } }, list: function (t, i, r) { return i = i || n.rtoken(/^\s*/), r = r || null, t instanceof Array ? n.each(n.product(t.slice(0, -1), n.ignore(i)), t.slice(-1), n.ignore(r)) : n.each(n.many(n.each(t, n.ignore(i))), px, n.ignore(r)) }, set: function (i, r, u) { return r = r || n.rtoken(/^\s*/), u = u || null, function (f) { for (var s = null, l = null, h = null, c = null, e = [[], f], o = !1, y, a, v = 0; v < i.length; v++) { h = null, l = null, s = null, o = i.length == 1; try { s = i[v].call(this, f) } catch (p) { continue } if (c = [[s[0]], s[1]], s[1].length > 0 && !o) try { h = r.call(this, s[1]) } catch (w) { o = !0 } else o = !0; if (o || h[1].length !== 0 || (o = !0), !o) { for (y = [], a = 0; a < i.length; a++) v != a && y.push(i[a]); l = n.set(y, r).call(this, h[1]), l[0].length > 0 && (c[0] = c[0].concat(l[0]), c[1] = l[1]) } if (c[1].length < e[1].length && (e = c), e[1].length === 0) break } if (e[0].length === 0) return e; if (u) { try { h = u.call(this, e[1]) } catch (b) { throw new t.Exception(e[1]); } e[1] = h[1] } return e } }, forward: function (n, t) { return function (i) { return n[t].call(this, i) } }, replace: function (n, t) { return function (i) { var r = n.call(this, i); return [t, r[1]] } }, process: function (n, t) { return function (i) { var r = n.call(this, i); return [t.call(this, r[0]), r[1]] } }, min: function (n, i) { return function (r) { var u = i.call(this, r); if (u[0].length < n) throw new t.Exception(r); return u } } }, s = function (n) { return function () { var t = null, u = [], i, r; if (arguments.length > 1 ? t = Array.prototype.slice.call(arguments) : arguments[0] instanceof Array && (t = arguments[0]), t) for (i = 0, r = t.shift() ; i < r.length; i++) return t.unshift(r[i]), u.push(n.apply(null, t)), t.shift(), u; else return n.apply(null, arguments) } }, e = "optional not ignore cache".split(/\s/); for (i = 0; i < e.length; i++) n[e[i]] = s(n[e[i]]); for (o = function (n) { return function () { return arguments[0] instanceof Array ? n.apply(null, arguments[0]) : n.apply(null, arguments) } }, f = "each any all".split(/\s/), u = 0; u < f.length; u++) n[f[u]] = o(n[f[u]]) }(), function () { var o = function (n) { for (var i = [], t = 0; t < n.length; t++) n[t] instanceof Array ? i = i.concat(o(n[t])) : n[t] && i.push(n[t]); return i }, u, f, e; Date.Grammar = {}, Date.Translator = { hour: function (n) { return function () { this.hour = Number(n) } }, minute: function (n) { return function () { this.minute = Number(n) } }, second: function (n) { return function () { this.second = Number(n) } }, meridian: function (n) { return function () { this.meridian = n.slice(0, 1).toLowerCase() } }, timezone: function (n) { return function () { var t = n.replace(/[^\d\+\-]/g, ""); t.length ? this.timezoneOffset = Number(t) : this.timezone = n.toLowerCase() } }, day: function (n) { var t = n[0]; return function () { this.day = Number(t.match(/\d+/)[0]) } }, month: function (n) { return function () { this.month = n.length == 3 ? Date.getMonthNumberFromName(n) : Number(n) - 1 } }, year: function (n) { return function () { var t = Number(n); this.year = n.length > 2 ? t : t + (t + 2e3 < Date.CultureInfo.twoDigitYearMax ? 2e3 : 1900) } }, rday: function (n) { return function () { switch (n) { case "yesterday": this.days = -1; break; case "tomorrow": this.days = 1; break; case "today": this.days = 0; break; case "now": this.days = 0, this.now = !0 } } }, finishExact: function (n) { var r, t, i; for (n = n instanceof Array ? n : [n], r = new Date, this.year = r.getFullYear(), this.month = r.getMonth(), this.day = 1, this.hour = 0, this.minute = 0, this.second = 0, t = 0; t < n.length; t++) n[t] && n[t].call(this); if (this.hour = this.meridian == "p" && this.hour < 13 ? this.hour + 12 : this.hour, this.day > Date.getDaysInMonth(this.year, this.month)) throw new RangeError(this.day + " is not a valid value for days."); return i = new Date(this.year, this.month, this.day, this.hour, this.minute, this.second), this.timezone ? i.set({ timezone: this.timezone }) : this.timezoneOffset && i.set({ timezoneOffset: this.timezoneOffset }), i }, finish: function (n) { var f, r, t, u, i, e; if (n = n instanceof Array ? o(n) : [n], n.length === 0) return null; for (f = 0; f < n.length; f++) typeof n[f] == "function" && n[f].call(this); return this.now ? new Date : (i = Date.today(), e = !!(this.days != null || this.orient || this.operator), e ? (u = this.orient == "past" || this.operator == "subtract" ? -1 : 1, this.weekday && (this.unit = "day", r = Date.getDayNumberFromName(this.weekday) - i.getDay(), t = 7, this.days = r ? (r + u * t) % t : u * t), this.month && (this.unit = "month", r = this.month - i.getMonth(), t = 12, this.months = r ? (r + u * t) % t : u * t, this.month = null), this.unit || (this.unit = "day"), (this[this.unit + "s"] == null || this.operator != null) && (this.value || (this.value = 1), this.unit == "week" && (this.unit = "day", this.value = this.value * 7), this[this.unit + "s"] = this.value * u), i.add(this)) : (this.meridian && this.hour && (this.hour = this.hour < 13 && this.meridian == "p" ? this.hour + 12 : this.hour), this.weekday && !this.day && (this.day = i.addDays(Date.getDayNumberFromName(this.weekday) - i.getDay()).getDate()), this.month && !this.day && (this.day = 1), i.set(this))) } }; var t = Date.Parsing.Operators, n = Date.Grammar, i = Date.Translator, r; n.datePartDelimiter = t.rtoken(/^([\s\-\.\,\/\x27]+)/), n.timePartDelimiter = t.stoken(":"), n.whiteSpace = t.rtoken(/^\s*/), n.generalDelimiter = t.rtoken(/^(([\s\,]|at|on)+)/), u = {}, n.ctoken = function (n) { var r = u[n], i; if (!r) { var o = Date.CultureInfo.regexPatterns, f = n.split(/\s+/), e = []; for (i = 0; i < f.length; i++) e.push(t.replace(t.rtoken(o[f[i]]), f[i])); r = u[n] = t.any.apply(null, e) } return r }, n.ctoken2 = function (n) { return t.rtoken(Date.CultureInfo.regexPatterns[n]) }, n.h = t.cache(t.process(t.rtoken(/^(0[0-9]|1[0-2]|[1-9])/), i.hour)), n.hh = t.cache(t.process(t.rtoken(/^(0[0-9]|1[0-2])/), i.hour)), n.H = t.cache(t.process(t.rtoken(/^([0-1][0-9]|2[0-3]|[0-9])/), i.hour)), n.HH = t.cache(t.process(t.rtoken(/^([0-1][0-9]|2[0-3])/), i.hour)), n.m = t.cache(t.process(t.rtoken(/^([0-5][0-9]|[0-9])/), i.minute)), n.mm = t.cache(t.process(t.rtoken(/^[0-5][0-9]/), i.minute)), n.s = t.cache(t.process(t.rtoken(/^([0-5][0-9]|[0-9])/), i.second)), n.ss = t.cache(t.process(t.rtoken(/^[0-5][0-9]/), i.second)), n.hms = t.cache(t.sequence([n.H, n.mm, n.ss], n.timePartDelimiter)), n.t = t.cache(t.process(n.ctoken2("shortMeridian"), i.meridian)), n.tt = t.cache(t.process(n.ctoken2("longMeridian"), i.meridian)), n.z = t.cache(t.process(t.rtoken(/^(\+|\-)?\s*\d\d\d\d?/), i.timezone)), n.zz = t.cache(t.process(t.rtoken(/^(\+|\-)\s*\d\d\d\d/), i.timezone)), n.zzz = t.cache(t.process(n.ctoken2("timezone"), i.timezone)), n.timeSuffix = t.each(t.ignore(n.whiteSpace), t.set([n.tt, n.zzz])), n.time = t.each(t.optional(t.ignore(t.stoken("T"))), n.hms, n.timeSuffix), n.d = t.cache(t.process(t.each(t.rtoken(/^([0-2]\d|3[0-1]|\d)/), t.optional(n.ctoken2("ordinalSuffix"))), i.day)), n.dd = t.cache(t.process(t.each(t.rtoken(/^([0-2]\d|3[0-1])/), t.optional(n.ctoken2("ordinalSuffix"))), i.day)), n.ddd = n.dddd = t.cache(t.process(n.ctoken("sun mon tue wed thu fri sat"), function (n) { return function () { this.weekday = n } })), n.M = t.cache(t.process(t.rtoken(/^(1[0-2]|0\d|\d)/), i.month)), n.MM = t.cache(t.process(t.rtoken(/^(1[0-2]|0\d)/), i.month)), n.MMM = n.MMMM = t.cache(t.process(n.ctoken("jan feb mar apr may jun jul aug sep oct nov dec"), i.month)), n.y = t.cache(t.process(t.rtoken(/^(\d\d?)/), i.year)), n.yy = t.cache(t.process(t.rtoken(/^(\d\d)/), i.year)), n.yyy = t.cache(t.process(t.rtoken(/^(\d\d?\d?\d?)/), i.year)), n.yyyy = t.cache(t.process(t.rtoken(/^(\d\d\d\d)/), i.year)), r = function () { return t.each(t.any.apply(null, arguments), t.not(n.ctoken2("timeContext"))) }, n.day = r(n.d, n.dd), n.month = r(n.M, n.MMM), n.year = r(n.yyyy, n.yy), n.orientation = t.process(n.ctoken("past future"), function (n) { return function () { this.orient = n } }), n.operator = t.process(n.ctoken("add subtract"), function (n) { return function () { this.operator = n } }), n.rday = t.process(n.ctoken("yesterday tomorrow today now"), i.rday), n.unit = t.process(n.ctoken("minute hour day week month year"), function (n) { return function () { this.unit = n } }), n.value = t.process(t.rtoken(/^\d\d?(st|nd|rd|th)?/), function (n) { return function () { this.value = n.replace(/\D/g, "") } }), n.expression = t.set([n.rday, n.operator, n.value, n.unit, n.orientation, n.ddd, n.MMM]), r = function () { return t.set(arguments, n.datePartDelimiter) }, n.mdy = r(n.ddd, n.month, n.day, n.year), n.ymd = r(n.ddd, n.year, n.month, n.day), n.dmy = r(n.ddd, n.day, n.month, n.year), n.date = function (t) { return (n[Date.CultureInfo.dateElementOrder] || n.mdy).call(this, t) }, n.format = t.process(t.many(t.any(t.process(t.rtoken(/^(dd?d?d?|MM?M?M?|yy?y?y?|hh?|HH?|mm?|ss?|tt?|zz?z?)/), function (t) { if (n[t]) return n[t]; throw Date.Parsing.Exception(t); }), t.process(t.rtoken(/^[^dMyhHmstz]+/), function (n) { return t.ignore(t.stoken(n)) }))), function (n) { return t.process(t.each.apply(null, n), i.finishExact) }), f = {}, e = function (t) { return f[t] = f[t] || n.format(t)[0] }, n.formats = function (n) { var r, i; if (n instanceof Array) { for (r = [], i = 0; i < n.length; i++) r.push(e(n[i])); return t.any.apply(null, r) } return e(n) }, n._formats = n.formats(["yyyy-MM-ddTHH:mm:ss", "ddd, MMM dd, yyyy H:mm:ss tt", "ddd MMM d yyyy HH:mm:ss zzz", "d"]), n._start = t.process(t.set([n.date, n.time, n.expression], n.generalDelimiter, n.whiteSpace), i.finish), n.start = function (t) { try { var i = n._formats.call({}, t); if (i[1].length === 0) return i } catch (r) { } return n._start.call({}, t) } }(), Date._parse = Date.parse, Date.parse = function (n) { var t = null; if (!n) return null; try { t = Date.Grammar.start.call({}, n) } catch (i) { return null } return t[1].length === 0 ? t[0] : null }, Date.getParseFunction = function (n) { var t = Date.Grammar.formats(n); return function (n) { var i = null; try { i = t.call({}, n) } catch (r) { return null } return i[1].length === 0 ? i[0] : null } }, Date.parseExact = function (n, t) { return Date.getParseFunction(t)(n) }
})();

// Message
(function () {
    $.Message = function (msg, data) {
        (function _send() {
            if (this.$) {
                this.$('iframe').each(function () {
                    _send.apply(this.contentWindow);
                });
                if (this.$.Message != null)
                    if (this.$.isFunction(this.$.Message[msg]))
                        this.$.Message[msg].call(this, data);
            }
        }).apply(window.top);
    };

    $.Message.themes = new function () {
        var _this = this;
        this.url = null;
        this.theme = null;

        this.set = $.Message.themes_change = function (theme) {
            if ($.jqx == undefined) return;
            if ($.jqx.theme == undefined) return;
            if ($.jqx.theme == theme) return;
            _this.theme = $.jqx.theme;
            $.jqx.theme = theme;
            $('#css_theme').prop('href', _this.url + 'jqx.' + theme + '.css');
            if ($.isFunction($.Message.themes.change))
                $.Message.themes.change.call(window, theme, _this.theme);
        }
        this.init = function (css_url, theme) {
            _this.url = css_url;
            $.Message.themes_change(theme || window.top.$.jqx.theme || 'ui-start');
        }
        this.notify = function (theme) {
            $.Message('themes_change', theme);
        }
        return this;
    }

    $.GetValue = function (name) {
        var result = null;
        (function _send() {
            if (result != null) return;
            if (this.$) {
                this.$('iframe').each(function () {
                    if (result == null)
                        _send.apply(this.contentWindow);
                });
                if (this.$.GetValue != null)
                    if (this.$.isFunction(this.$.GetValue[name]))
                        result = this.$.GetValue[name]();
            }
        }).apply(window.top);
        return result;
    };

    $.Message.themes.init($('#css_theme').attr('path'));
})();

// i18n
(function () {
    //var path = '';
    //    //$('script[tag="lang"]').attr('path');
    //path += '' + window.location + '-';
    //path += (window.navigator.userLanguage || window.navigator.language);
    //console.log(path);
    ////console.log( + '/lang-' + );
    //$('script[tag="lang"]').prop('src', path);

    $.lang = window.lang = function (name) {
        name = $.trim(name);
        return name;
    }

    //document.write('<script type="text/"></script>');
    //document.write(window.navigator.userLanguage || window.navigator.language);
})();

// jqxwidgets extension
(function ($) {
    $.o = function (o) { return o; };
    
    $.injection = function (src, prop, before, after) {
        var prev = src[prop]
        if (!$.isFunction(prev)) console.log('Uabled to injection function : ' + prop);
        if (!$.isFunction(before)) before = function () { }
        if (!$.isFunction(after)) after = function () { }
        src[prop] = function () {
            before.apply(this, arguments);
            var ret = prev.apply(this, arguments);
            after.apply(this, arguments);
            return ret;
        }
    }
    
    $.replace_function = function (src, prop, replace) {
        replace._original = src[prop];
        src[prop] = replace;
    }

    $.fn.addClass_jqx = function (e) { return this.addClass(e + ' ' + e + '-' + $.jqx.theme); }

    $.fn.removeClass_jqx = function (e) { return this.removeClass(e + ' ' + e + '-' + $.jqx.theme); }

    $.lists = {
        Corps: {
            0: {
                Name: 'root',
                Groups: { '00000000-0000-0000-0000-000000000000': '', },
            },
        },
        Groups: { '00000000-0000-0000-0000-000000000000': '', },
    };

    if (!$.jqx) return;

    $.fn.jqxButton2 = function () {
        var icon = this.attr('icon');
        var $ret = this.jqxButton(arguments);
        if (icon) {
            this.addClass('button-icon-text');
            this.wrapInner('<span class="button-text"></span>');
            var $icon = $('<span class="button-icon" style="' + this.attr('icon-style') + '"></span>').addClass_jqx('jqx-icon-' + icon);
            $icon.insertBefore(this.children().first());
        }
        this.removeAttr('icon icon-style');
        return $ret;
    };

    $.fn.jqxMenu2 = function (o) {
        this.jqxMenu(o);
        this.removeClass_jqx('jqx-widget-header');//.addClass_jqx('jqx-widget-content');
        return this;
    };

    $.fn.jqxGrid_formatData = function (data) {
        var inst = this.jqxGrid("getInstance");
        if (inst.dataview.filters.length > 0) {
            data._filter = new Array();
            for (var i = 0; i < inst.dataview.filters.length; i++) {
                var f1 = inst.dataview.filters[i];
                var f2 = f1.filter.getfilters();
                if (f2.length > 0) {
                    var o = {
                        operator: f1.operator,
                        datafield: f1.datafield,
                        filters: f2,
                        //str: new Array(),
                    }
                    //for (var j = 0; j < f2.length; j++) o.str.push(JSON.stringify(f2[j]));
                    data._filter.push(o);
                }
            }
        }
        data._sort = inst.getsortinformation();
        data._pager = inst.getpaginginformation();
        return data;
    };

    $.jqx._jqxGrid.prototype.getInstance = function () {
        return this;
    };


    $.replace_function($.jqx._jqxGrid.prototype, 'deleterow', function (rowid) {
        this._CleanupEditor(rowid);
        return arguments.callee._original.apply(this, arguments);
    });

    $.replace_function($.jqx.dataview, 'refresh', function () {
        this.grid._ResetEditor();
        return arguments.callee._original.apply(this, arguments);
    });

    $.replace_function($.jqx._jqxGrid.prototype, '_rendercell', function (datagrid, column, bound, cellvalue, cellelement, q) {
        var _rendercell = arguments.callee._original;
        if (datagrid.columns_src != null) {
            var column_src = datagrid.columns_src[column.datafield];
            if (column_src != null) {
                if ($.isFunction(column_src.cellformatter)) {
                    var tmp = column_src.cellformatter(column, column_src, bound, cellvalue);
                    if (tmp != null) arguments[3] = tmp;
                }
            }
        };
        return _rendercell.apply(this, arguments);
    });
    //$.replace_function($.jqx._jqxGrid.prototype, '_rendercell', function (datagrid, column, bound, cellvalue, cellelement, q) {
    //    var _rendercell = arguments.callee._original;
    //    if (datagrid.columns_src != null) {
    //        var column_src = datagrid.columns_src[column.datafield]
    //        if (column_src != null) {
    //            var row = datagrid.getboundindex(bound);
    //            var rowid = datagrid.getrowid(row);
    //            var editor = datagrid._GetEditor(rowid);
    //            var datafield = column.datafield;
    //            if (editor.rendercell_cancel === true) return false;
    //            if (editor.isEditing === true) {
    //                arguments[3] = 0;
    //                var ret = _rendercell.apply(this, arguments);
    //                var cell = editor.editcells[column.datafield];
    //                if ((cell == null) && (column_src.CreateCells)) {
    //                    cell = editor.cells[column.datafield] = column_src.CreateEditCell.call(column_src, editor, column);
    //                }
    //                if ((cell != null) && (column_src.RenderEdit)) {
    //                    column_src.RenderEdit.call(column_src, editor, cell, column, cellvalue, cellelement);
    //                }
    //                return ret;
    //            }

    //            var edit = ((column.editable === true) && (editor.isEditing === true));
    //            var remove = ((column.removable === true) && (editor.isRemoving === true));
    //            if (edit || remove || column_src.RenderCell) {
    //                var cell = editor.cells[column.datafield];
    //                if ((cell == null) && (column_src.CreateCells)) {
    //                    cell = editor.cells[column.datafield] = column_src.CreateCells.call(column_src, editor, column);
    //                }
    //                if (cell != null) {
    //                    if (edit) {
    //                        if (column_src.RenderEdit.call(column_src, editor, cell, column, cellvalue, cellelement) === true)
    //                            return true;
    //                    }
    //                    else if (remove) {
    //                        if (column_src.RenderRemove.call(column_src, editor, cell, column, cellvalue, cellelement) === true)
    //                            return true;
    //                    }
    //                    else  if (column_src.RenderEdit.call(column_src, editor, cell, column, cellvalue, cellelement) === true)
    //                        return true;
    //                }
    //            }

    //            if ($.isFunction(column_src.cellformatter)) {
    //                var tmp = column_src.cellformatter(column, column_src, bound, cellvalue);
    //                if (tmp != null) arguments[3] = tmp;
    //            }
    //        }
    //    }
    //    return _rendercell.apply(this, arguments);
    //    //
    //    //if (editor.rendercell_cancel === true) return false;
    //    //if (editor.isEditing) {
    //    //}
    //    //else if (editor.isRemoving) {
    //    //}
    //    //else {
    //    //    var cellvalue2 = null;
    //    //    if ($.isFunction(column_src.cellformatter))
    //    //        cellvalue2 = column_src.cellformatter(column, column_src, bound, cellvalue);
    //    //    return arguments.callee._original.call(this, datagrid, column, bound, cellvalue2 == null ? cellvalue : cellvalue2, cellelement, q);
    //    //}
    //    //return ret;
    //    //
    //    //var ret = arguments.callee._original.call(this, datagrid, column, bound, cellvalue2 == null ? cellvalue : cellvalue2, cellelement, q);
    //    //var row = datagrid.getboundindex(bound);
    //    //var editor = datagrid._GetEditor(datagrid.getrowid(row));
    //    //var elem = editor.elem[column.datafield];
    //    //if (elem == null) {
    //    //    if (column_src.CreateEditor)
    //    //        elem = column_src.CreateEditor.call(datagrid, row, editor, column, column_src, cellvalue, cellelement);
    //    //}
    //    //if (elem != null) {
    //    //    if (column_src.ShowEditor)
    //    //        column_src.ShowEditor.call(datagrid, row, editor, elem, column, column_src, cellvalue, cellelement);
    //    //}
    //});

    function row_editor(rowid, datagrid) {
        //console.log('new editor({0})'.format(rowid));
        var _this = this;
        this.rowid = rowid;
        this.owner = datagrid;
        this.rendercell_cancel = false;
        this.isEditing = false;
        this.isEditing_init = false;
        this.isRemoving = false;
        this.isNew = false;
        (this.reset = function () {
            _this.cells = {};
            _this.editcells = {};
            _this.removecells = {};
        })();

        this.update = function () {
            for (var i = 0; i < _this.owner.columns.records.length; i++) {
                if (_this.owner.columns.records[i].editable != true) continue;
                var boundindex = _this.owner.getrowboundindexbyid(_this.rowid);
                _this.rendercell_cancel = true;
                datagrid.begincelledit(boundindex, _this.owner.columns.records[i].datafield);
                _this.rendercell_cancel = false;
                datagrid.endcelledit(boundindex, _this.owner.columns.records[i].datafield, true);
                break;
            }
        };

        this.BeginEditRow = function () {
            if (_this.isRemoving) return;
            _this.isEditing = true;
            _this.isEditing_init = true;
            _this.update(_this.rowid);
            this.isEditing_init = false;
        };
        this.EndEditRow = function () {
            if (_this.isRemoving) return;
            _this.isEditing = false;
            if (_this.isNew)
                _this.owner.deleterow(rowid);
            else
                _this.update(_this.rowid);
        };
        this.CommitEditRow = function () {
            if (_this.isRemoving) return;
            _this.isEditing = false;
            _this.update(_this.rowid);
        };
        this.BeginRemoveRow = function () {
            if (_this.isEditing) return;
            _this.isRemoving = true;
            _this.isEditing_init = true;
            _this.update(_this.rowid);
            _this.isEditing_init = false;
        };
        this.EndRemoveRow = function () {
            if (_this.isEditing) return;
            _this.isRemoving = false;
            _this.update(_this.rowid);
        }
        this.CommitRemoveRow = function () {
            if (_this.isEditing) return;
            _this.isRemoving = false;
            _this.update(_this.rowid);
        };

    };

    $.extend($.jqx._jqxGrid.prototype, {
        _Init: function (opts) {
            this.columns_src = {
            };
            for (var i = 0; i < opts.options.columns.length; i++)
                this.columns_src[opts.options.columns[i].datafield] = opts.options.columns[i];
        },
        _CleanupEditor: function (rowid) {
            var data = this.getrowdatabyid(rowid);
            if (data != null) return;
            console.log(rowid);
            delete this._Editors[rowid];
        },
        _ResetEditor: function () {
            if (this._Editors == null) return;
            for (var n in this._Editors) {
                this._Editors[n].reset();
            }
        },
        _GetEditor: function (rowid) {
            if (this._Editors == null)
                this._Editors = {
                };
            if (this._Editors[rowid])
                return this._Editors[rowid];
            else
                return this._Editors[rowid] = new row_editor(rowid, this);
        },
        AddRow: function (data) {
            data = data || {};
            //this.source.addrow = function (rowid, rowdata, position, commit) {
            //    uid = rowid;
            //    commit(true);
            //};
            if (this.addrow(null, data, 'first')) {
                var editor = this._GetEditor(data.uid);
                editor.isNew = true;
                editor.BeginEditRow();
            }
            //var editor = this._GetEditor(row);
        },
        EditRow: function (rowid, commit) {
            var editor = this._GetEditor(rowid);
            if (commit === true)
                editor.CommitEditRow();
            else if (commit === false)
                editor.EndEditRow();
            else
                editor.BeginEditRow();
        },
        RemoveRow: function (rowid, commit) {
            var editor = this._GetEditor(rowid);
            if (commit === true)
                editor.CommitRemoveRow();
            else if (commit === false)
                editor.EndRemoveRow();
            else
                editor.BeginRemoveRow();
        },

        getcolumn_src: function (d) {
            if (this.columns_src)
                return this.columns_src[d];
            return null;
        },
    });

    $.col = function (type, opts) {
        if (typeof type != 'string') {
            opts = type;
            type = '';
        }
        type = $.trim(type);
        var opt2 = $.extend(true, {}, col_default, $.col.defines[type], opts);

        if (opt2.datafield)
            opt2.datafield = $.trim(opt2.datafield);
        if (opt2.text && (opt2.text != ' '))
            opt2.text = $.trim(opt2.text);
        if ((opt2.text == '') || (opt2.text == null))
            opt2.text = opt2.datafield;
        return opt2;
    }

    var col_default = {
        align: 'center', cellsalign: 'center',
        //_rendercell: function (datagrid, column, bound, cellvalue, cellelement, q) { },
        //CreateEditor: function (editor, column, column_src, cellvalue, cellelement) {
        //    if (column.editable != true) return;
        //    if (editor.isEditing != true) return;
        //    return $('<input autocomplete="off" autocorrect="off" autocapitalize="off" spellcheck="false" type="textbox" style="width: 99%; height: 100%;"/>')
        //        .jqxInput().removeClass(this.toThemeProperty("jqx-rc-all"));
        //},
        //ShowEditor: function (editor, elem, column, column_src, cellvalue, cellelement) {
        //    if (editor.isEditing) {
        //        $(cellelement).empty();
        //        elem.appendTo(cellelement).val(cellvalue);
        //    }
        //},
    };

    $.col.defines = {
        Action: {
            width: 100, datafield: '_Actions', text: ' ', cellsalign: 'center',
            editable: true, columntype: 'custom',
            buttons: {
                edit: true, remove: true, text: ['Edit', 'Cancel', 'Save', 'Remove', 'Cancel', 'Remove']
            },
            pinned: true, sortable: false, hideable: false, groupable: false, exportable: false, resizable: false, enabletooltips: false, filterable: false, menu: false,
            //cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) { return ''; },
            //CreateEditCell: function (editor, column) {
            //    var cells = [];
            //    (cells[0] = $('<button class="grid-actions">' + this.buttons.text[0] + '</button>')).jqxButton().on('click', editor.BeginEditRow);
            //    (cells[1] = $('<button class="grid-actions">' + this.buttons.text[1] + '</button>')).jqxButton().on('click', editor.EndEditRow);
            //    (cells[2] = $('<button class="grid-actions">' + this.buttons.text[2] + '</button>')).jqxButton().on('click', editor.CommitEditRow);
            //    return cells;
            //},
            //CreateEditCell1: function (editor, column) {
            //    var cells = [];
            //    (cells[0] = $('<button class="grid-actions">' + this.buttons.text[0] + '</button>')).jqxButton().on('click', editor.BeginEditRow);
            //    (cells[1] = $('<button class="grid-actions">' + this.buttons.text[1] + '</button>')).jqxButton().on('click', editor.EndEditRow);
            //    (cells[2] = $('<button class="grid-actions">' + this.buttons.text[2] + '</button>')).jqxButton().on('click', editor.CommitEditRow);
            //    (cells[3] = $('<button class="grid-actions">' + this.buttons.text[3] + '</button>')).jqxButton().on('click', editor.BeginRemoveRow);
            //    (cells[4] = $('<button class="grid-actions">' + this.buttons.text[4] + '</button>')).jqxButton().on('click', editor.EndRemoveRow);
            //    (cells[5] = $('<button class="grid-actions">' + this.buttons.text[5] + '</button>')).jqxButton().on('click', editor.CommitRemoveRow);
            //    return cells;
            //},
            //RenderEdit: function (editor, cell, column, cellvalue, cellelement) {
            //},
            //RenderRemove: function (editor, cell, column, cellvalue, cellelement) {
            //},
            //RenderCell: function (editor, cell, column, cellvalue, cellelement) {
            //    var cell = this.getcells(editor, column);
            //    if (this.buttons.edit) cell[0].appendTo(cellelement);
            //    if (this.buttons.remove) cell[3].appendTo(cellelement);
            //    return true;
            //},
            //CreateEditor: function (editor, column, column_src, cellvalue, cellelement) {
            //    var _this = this;
            //    var elem = {
            //    };
            //    elem.all = $(
            //            '<button class="grid-actions">' + column_src.buttons.text[0] + '</button>' +
            //            '<button class="grid-actions">' + column_src.buttons.text[1] + '</button>' +
            //            '<button class="grid-actions">' + column_src.buttons.text[2] + '</button>' +
            //            '<button class="grid-actions">' + column_src.buttons.text[3] + '</button>' +
            //            '<button class="grid-actions">' + column_src.buttons.text[4] + '</button>' +
            //            '<button class="grid-actions">' + column_src.buttons.text[5] + '</button>');
            //    //(elem.e1 = $(elem.all[0])).jqxButton().on('click', function () { _this.EditRow(editor.rowid); });
            //    //(elem.e2 = $(elem.all[1])).jqxButton().on('click', function () { _this.EditRow(editor.rowid, false); });
            //    //(elem.e3 = $(elem.all[2])).jqxButton().on('click', function () { _this.EditRow(editor.rowid, true); });
            //    //(elem.r1 = $(elem.all[3])).jqxButton().on('click', function () { _this.RemoveRow(editor.rowid); });
            //    //(elem.r2 = $(elem.all[4])).jqxButton().on('click', function () { _this.RemoveRow(editor.rowid, false); });
            //    //(elem.r3 = $(elem.all[5])).jqxButton().on('click', function () { _this.RemoveRow(editor.rowid, true); });
            //    (elem.e1 = $(elem.all[0])).jqxButton().on('click', editor.BeginEditRow);
            //    (elem.e2 = $(elem.all[1])).jqxButton().on('click', editor.EndEditRow);
            //    (elem.e3 = $(elem.all[2])).jqxButton().on('click', editor.CommitEditRow);
            //    (elem.r1 = $(elem.all[3])).jqxButton().on('click', editor.BeginRemoveRow);
            //    (elem.r2 = $(elem.all[4])).jqxButton().on('click', editor.EndRemoveRow);
            //    (elem.r3 = $(elem.all[5])).jqxButton().on('click', editor.CommitRemoveRow);
            //    return elem;
            //},
            //ShowEditor: function (editor, elem, column, column_src, cellvalue, cellelement) {
            //    $(cellelement).empty();
            //    if (editor.isEditing) {
            //        elem.e2.appendTo(cellelement);
            //        elem.e3.appendTo(cellelement);
            //    }
            //    else if (editor.isRemoving) {
            //        elem.r2.appendTo(cellelement);
            //        elem.r3.appendTo(cellelement);
            //    }
            //    else {
            //        if (column_src.buttons.edit) elem.e1.appendTo(cellelement);
            //        if (column_src.buttons.remove) elem.r1.appendTo(cellelement);
            //    }
            //},
        },
        CorpID: {
            cellformatter: function (column, column_src, bound, cellvalue) {
                var c = $.lists.Corps[cellvalue];
                if (c) return c.Name;
            }
        },
        GroupID: {
            cellformatter: function (column, column_src, bound, cellvalue) {
                return $.lists.Groups[cellvalue];
            },
        },
        Locked: {
            //CreateEditor: function (editor, column, column_src, cellvalue, cellelement) {
            //    var elem = $('<div>123</div>').appendTo(cellelement);
            //    elem.jqxSwitchButton({ width: 80, height: 20 });
            //    return elem;
            //},
            //ShowEditor: function (editor, elem, column, column_src, cellvalue, cellelement) {
            //    $(cellelement).empty().append(elem);
            //},
        },
        Balance: {
        }
    };

    $.fn.jqxGrid2 = function (o) {
        var $grid = this;
        var datagrid = null;
        var opts = o;

        var source = $.extend(true, {
            //id: 'ID',
            //root: "rows",
            //record: "",
            url: 'api',
            datatype: "json",
            type: 'post',
            datafields: [],
            mapChar: '.',
            //localdata: [],
            //data: { a: 1, b: 2 },
            loadallrecords: false,
        }, o.source);
        var settings = $.extend(true, {
            //async: true,
            //autoBind: false,
            //contentType: "application/x-www-form-urlencoded",
            //processData: function () { },
            //beforeSend: function () { },
            //loadError: function (xhr, status, error) { },
            //beforeLoadComplete: function () { },
            downloadComplete: function (data, status, xhr) {
                //console.log(arguments);
            },
            //loadComplete: function (data) { },
            //loadServerData: function () { },
        }, o.settings);
        if (o.settings.SelectCommand) {
            delete settings.SelectCommand;
            settings.formatData = function (data) {
                if (datagrid) {
                    if (datagrid.dataview.filters.length > 0) {
                        data._filter = new Array();
                        for (var i = 0; i < datagrid.dataview.filters.length; i++) {
                            var f1 = datagrid.dataview.filters[i];
                            var f2 = f1.filter.getfilters();
                            if (f2.length > 0) {
                                var o = {
                                    operator: f1.operator,
                                    datafield: f1.datafield,
                                    filters: f2,
                                }
                                data._filter.push(o);
                            }
                        }
                    }
                    data._sort = datagrid.getsortinformation();
                    //data._pager = datagrid.getpaginginformation();
                }
                var command = {
                };
                command[opts.settings.SelectCommand] = data;
                return {
                    str: JSON.stringify(command)
                };
            }
        }

        var options = $.extend(true, {
        }, o.options);

        for (var i = 0; i < options.columns.length; i++) {
            var col = options.columns[i];
            if (col.cellsrendered) {
                delete col.cellsrendered;
            }

            if (col.source) {
                col.source.name = col.datafield;
                source.datafields.push(col.source);
                delete col.source;
            }
        }
        for (var i = 0; i < source.datafields.length; i++) {
            var col = source.datafields[i];
            col['name'] = $.trim(col['name']);
            col['type'] = $.trim(col['type']);
        }

        options.source = new $.jqx.dataAdapter(source, settings);

        // console.log(o);
        // console.log(this[0].attributes);

        this.jqxGrid(options); $grid.grid = datagrid = this.jqxGrid("getInstance"); // init jqxGrid
        datagrid._Init(opts);

        console.log('jqxGrid init.');
        if (o.events) for (var e in o.events) this.jqxGrid().on(e, o.events[e]);

        //new $.injection(datagrid.dataview, 'refresh', datagrid._ResetEditor);

        //new $.injection(datagrid, '_render', function () {
        //    console.log('_render');
        //    datagrid.action_panel.dom_remove();
        //});
        return this;
    };

})(jqxBaseFramework);
