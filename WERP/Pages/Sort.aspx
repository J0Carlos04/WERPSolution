<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Sort.aspx.cs" Inherits="Pages_Sort" %>

<%@ Register Src="~/UserControls/Popup/ucSort.ascx" TagPrefix="uc1" TagName="ucSort" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>W.ERP</title>    
    <link href="../res/css/bootstrap.min.css" rel="stylesheet" />
    <script lang ="javascript" type="text/javascript" src="../res/js/JScript.js" ></script> 
</head>
<body>
    <form id="form1" runat="server">
        <uc1:ucSort runat="server" ID="ucSort" />
    </form>
    <script lang="javascript" type="text/javascript" src="../res/js/bootstrap.bundle.js"></script>
</body>
</html>
