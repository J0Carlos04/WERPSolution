<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Search.aspx.cs" Inherits="Pages_Search" %>

<%@ Register Src="~/UserControls/Popup/ucSearch.ascx" TagPrefix="uc1" TagName="ucSearch" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Search</title>    
    <link href="../res/css/bootstrap.min.css" rel="stylesheet" />
    <script lang ="javascript" type="text/javascript" src="../res/js/JScript.js" ></script> 
</head>
<body>
    <form id="form1" runat="server">
        <uc1:ucSearch runat="server" ID="ucSearch" />
    </form>
    <script lang="javascript" type="text/javascript" src="../res/js/bootstrap.bundle.js"></script>
</body>
</html>
