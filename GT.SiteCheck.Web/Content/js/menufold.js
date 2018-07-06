// JavaScript Document
function menuFold()
{
	//fold
	var unfold = document.getElementById("menu").getElementsByClassName("unfold");
	var unfoldLength = unfold.length;

	for(i=0;i<unfoldLength;i++)
	{
		unfold.item(i).onclick = function()
		{
			console.log(this.parentNode.getElementsByTagName("ul"));
			this.className = "fold";
			this.parentNode.getElementsByTagName("ul").item(0).style.display = "none";
			menuFold();
		}
	}
	//unfold
	var fold = document.getElementById("menu").getElementsByClassName("fold");
	var foldLength = fold.length;

	for(i=0;i<foldLength;i++)
	{
		fold.item(i).onclick = function()
		{
			console.log(this.parentNode.getElementsByTagName("ul"));
			this.className = "unfold";
			this.parentNode.getElementsByTagName("ul").item(0).style.display = "";
			menuFold();
		}
	}
}