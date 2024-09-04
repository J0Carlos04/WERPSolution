<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ErrorList.aspx.cs" Inherits="Pages_ErrorList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Error List</title>
    <link href="../res/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:GridView runat="server" ID="gvData" BorderWidth="0px" Width="100%" CssClass="table table-striped table-bordered table-hover" >          
        </asp:GridView>
    </form>
</body>
</html>
