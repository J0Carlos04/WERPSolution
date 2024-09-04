<%@ Page Language="C#" AutoEventWireup="true" CodeFile="HelpdeskCategory.aspx.cs" Inherits="Pages_Admin_Helpdesk_HelpdeskCategory" %>
<%@ Register Src="~/UserControls/Form/ucMasterData.ascx" TagPrefix="uc1" TagName="ucMasterData" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../../res/css/bootstrap.min.css" rel="stylesheet" />
    <script lang ="javascript" type="text/javascript" src="../../../res/js/JScript.js" ></script>
</head>
<body>
    <form id="form1" runat="server">
        <uc1:ucMasterData runat="server" ID="ucMasterData" TableName="HelpdeskCategory" />
    </form>
</body>
</html>
