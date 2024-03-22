// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
/*
** ===========================================================================
** A collection of typescript static date functions as follows:
** . nsg_getTruncTime		zero out time part of a date
** . nsg_getYYYYMMDD		return string of YYYY-MM-DD format of a date
** . nsg_isWeekEnding		return this data a Saturday
** . nsg_getWeekEnding		return the next Saturday of a date
** . nsg_isWeekStarting		return this data a Sunday
** . nsg_getWeekStarting	return the next Sunday of a date
** . nsg_getAddDate			add n # of days to a date
** . nsg_nearestMinute		round to the nearest minute
**
** Includes various constants:
** . nsg_milliInDay			number of milliseconds in a day
** . nsg_milliInMinute		number of milliseconds in a minute
** . nsg_weekEndingDOW		the day-of-week that is week ending (Saturday)
** . nsg_timeDayOnlyString	string part to force to have no time part
** . nsg_initialDate		smallest/earliest/initial Javascript date
*/
var nsg_milliInDay = 86400000;
var nsg_milliInMinute = 60000; // 1000 * 60
var nsg_weekStartingDOW = 0;
var nsg_weekEndingDOW = 6;
var nsg_timeDayOnlyString = 'T00:00:00';
var nsg_initialDate = new Date('1970-01-01T00:00:00');
/**
** Remove/truncate the time part of a date value.
** This will not effect the passed value.
*/
function nsg_getTruncTime(dt) {
	return new Date(dt.getFullYear(), dt.getMonth(), dt.getDate());
}
/**
** Return the string of date in the format of YYYY-MM-DD.
*/
function nsg_getYYYYMMDD(dt) {
	return dt.toLocaleDateString('fr-CA');
}
/**
** Check if date is week ending date.
*/
function nsg_isWeekEnding(dt) {
	if (dt.getDay() === nsg_weekEndingDOW) {
		return true;
	}
	return false;
}
/**
** Check if date is week starting date.
*/
function nsg_isWeekStarting(dt) {
	if (dt.getDay() === nsg_weekStartingDOW) {
		return true;
	}
	return false;
}
/**
** Convert a date to the week ending date.
*/
function nsg_getWeekEnding(dt) {
	let ret = nsg_getTruncTime(dt);
	const _dow = ret.getDay();
	if (_dow !== nsg_weekEndingDOW) {
		ret = new Date(ret.setDate(ret.getDate() + (nsg_weekEndingDOW - _dow)));
	}
	return ret;
}
/**
** Convert a date to the week strating date.
*/
function nsg_getWeekStarting(dt) {
	let ret = nsg_getTruncTime(dt);
	const _dow = ret.getDay();
	if (_dow !== nsg_weekStartingDOW) {
		ret = new Date(ret.setDate(ret.getDate() + (nsg_weekStartingDOW - _dow)));
	}
	return ret;
}
/**
** add/subtract # of days from passed date.
** This will not effect the passed value.
*/
function nsg_getAddDate(dt, days) {
	return new Date(dt.getTime() + days * nsg_milliInDay);
}
/**
** Round up/down to the nearest minute.
** This will not effect the passed value.
*/
function nsg_nearestMinute(dt) {
	return new Date(Math.round(
		dt.getTime() / nsg_milliInMinute) * nsg_milliInMinute);
}
/**
** Check if the date (dt) is in the middle of DST.
** @param {any} dt
*/
function nsg_isDST(dt) {
	let yr = dt.getFullYear();
	let jan = new Date(yr, 0, 1).getTimezoneOffset();
	let jul = new Date(yr, 6, 1).getTimezoneOffset();
	return Math.max(jan, jul) !== dt.getTimezoneOffset();
}
//
// ===========================================================================
