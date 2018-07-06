// JavaScript Document
function WarningBox()
{
	this.show = function(a)
	{
		if(document.getElementById(a).getAttribute("class").slice(" off"))
		{
			document.getElementById(a).setAttribute("class",document.getElementById(a).getAttribute("class").replace(" off"," on"));
		}
	};
	this.hide = function(a)
	{
		if(document.getElementById(a).getAttribute("class").slice(" on"))
		{
			document.getElementById(a).setAttribute("class",document.getElementById(a).getAttribute("class").replace(" on"," off"));
		}
	}
}