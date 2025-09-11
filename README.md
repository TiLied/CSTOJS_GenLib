# CSTOJS_GenLib
This library generate various stuff for a [CSharpToJavaScript](https://github.com/TiLied/CSharpToJavaScript).

## Generate xml documentation from mdn
- Download [MDN-content web-api](https://github.com/mdn/content/tree/main/files/en-us/web/api)
- Add in Main Method:
```csharp
GenDocs genDocs = new();
await genDocs.GenerateDocs("PATH TO A DOWNLOADED DOCS", "OUTPUT PATH");
```

## Generate c# files from webidl
- Download idls from [wpt/interfaces](https://github.com/web-platform-tests/wpt/tree/master/interfaces). See also: [webref](https://github.com/w3c/webref), [wpt](https://github.com/web-platform-tests/wpt), [reffy](https://github.com/w3c/reffy).
- Convert webidl to json files using [webidl2.js](https://github.com/w3c/webidl2.js/).
- Make sure to ignore "*.tentative.idl"!
- Make sure that json starts with :
```
{
	"TType":...
}
```
- Generate csharp files:
```csharp
GenCSharp genCSharp = new();
await genCSharp.GenerateCSFromJson("PATH TO A JSON FILE", "OUTPUT PATH");
```
