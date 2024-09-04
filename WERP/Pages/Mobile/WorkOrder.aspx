<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WorkOrder.aspx.cs" Inherits="Pages_Mobile_WorkOrder" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>    
    <link rel="stylesheet" href="../../res/css/bootstrap.min.css" /> 
    <script src="../../res/js/bootstrap.bundle.js"></script> 
    <style>
      .grid{
          border:0px none;
      }
      .grid td {       
          border:0px none;      
      }
    </style>
</head>
<body>
<form id="form1" runat="server"><div class="wrapper-suppress">
        
<ul class="nav nav-pills" style="margin-bottom:5px;">
    <li class="nav-item">
        <a href="#All" class="nav-link active" data-bs-toggle="tab">All</a>
    </li>
    <li class="nav-item">
        <a href="#Preparation" class="nav-link" data-bs-toggle="tab">Preparation</a>
    </li>
    <li class="nav-item">
        <a href="#StockReceived" class="nav-link" data-bs-toggle="tab">Stock Received</a>
    </li>     
</ul>

<div class="tab-content">

<div class="tab-pane fade show active" id="All">
<input type="file" capture="[environment | user | camera]" accept="image/*" id="takePictureField" />
<asp:GridView runat="server" ID="gvWorkOrder" ShowHeader="false" AutoGenerateColumns="false" BorderWidth="0px" Width="100%" CssClass="grid" OnRowDataBound="gvWorkOrder_RowDataBound" >
    <Columns>
        <asp:TemplateField><ItemTemplate>
        <asp:Literal runat="server" ID="ltrl_Id" Text='<%# Eval("Id") %>' Visible="false" />
        <div class="card" style="margin-bottom:10px;" >
          <div class="card-header card-link"><asp:LinkButton runat="server" ID="lb_Subject" Text='<%# Eval("Subject") %>' OnClick="lb_Click" /></div>
          <div class="card-body"><asp:Literal runat="server" ID="ltrl_WorkDescription" Text='<%# Eval("WorkDescription") %>' /></div>
          <div runat="server" id="dvFooter" class="card-footer">
            <asp:Button runat="server" ID="btnItem" Text="Items" class="btn btn-primary btn-sm" OnClick="btn_Click" />
            <asp:Button runat="server" ID="btnWorkUpdate" Text="Work Update" class="btn btn-primary btn-sm" OnClick="btn_Click" />
            <asp:Button runat="server" ID="btnAttachment" Text="Attachment" class="btn btn-primary btn-sm" OnClick="btn_Click" />
          </div>
          <div style="display:none;"><asp:Button runat="server" ID="btnSelect" OnClick="btn_Click" /></div>
        </div>
        </ItemTemplate></asp:TemplateField>
    </Columns>
</asp:GridView>
</div>

<div class="tab-pane fade" id="Preparation">
    <p>Preparation tab content ...</p>
</div>

<div class="tab-pane fade" id="StockReceived">
    <p>StockReceived tab content ...</p>
</div>
</div>

</div></form>
      
    
</body>
</html>
