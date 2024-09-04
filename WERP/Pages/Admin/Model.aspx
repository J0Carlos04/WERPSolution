<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Model.aspx.cs" Inherits="Pages_Admin_Model" %>
<%@ Register Src="~/UserControls/Form/ucMasterData.ascx" TagPrefix="uc1" TagName="ucMasterData" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>W.ERP</title>
</head>
<body>
    <form id="form1" runat="server">
        <uc1:ucMasterData runat="server" ID="ucMasterData" TableName="Model" />
    </form>
</body>
</html>
