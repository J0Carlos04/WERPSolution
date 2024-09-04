<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StockOrderDetail.aspx.cs" Inherits="Pages_Inventory_StockOrderDetail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>W.ERP</title>      
    <link href="../../res/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
<form id="form1" runat="server">
<f:PageManager ID="pm" AutoSizePanelID="pContent" runat="server" Language="EN" AjaxAspnetControls="pnlContent" /> 
  <f:Panel ID="pContent" runat="server" ShowBorder="false" ShowHeader="false" Layout="Fit" >
    <Toolbars>
      <f:Toolbar runat="server" ID="tlb">
        <Items>          
          <f:Button runat="server" ID="btnCancel" Text="Cancel" Icon="Cancel" EnablePostBack="false" OnClientClick="parent.removeActiveTab();" />
        </Items>
      </f:Toolbar>
    </Toolbars>
    <Items>
      <f:ContentPanel runat="server" ID="pData" ShowBorder="true" AutoScroll="true"  >
        <Content><div class="wrapper-suppress"><asp:Panel runat="server" ID="pnlContent">
        
          <div class="form-group row">
            <label for="tbCode" class="col-sm-2 col-form-label">Code</label>
            <div class="col-sm-10"><input runat="server" id="tbCode" type="text" readonly class="form-control-plaintext" /></div>
          </div>
          <asp:Panel runat="server" ID="pnlDescription">
          <div id="dvDescription" class="form-group row">
            <label for="tbDescription" class="col-sm-2 col-form-label">Description</label>
            <div class="col-sm-10"><input runat="server" id="tbDescription" type="text" readonly class="form-control-plaintext" /></div>
          </div>
          </asp:Panel>
          <div class="form-group row">
            <label for="tbRequester" class="col-sm-2 col-form-label">Requested</label>
            <div class="col-sm-10"><input runat="server" id="tbRequester" type="text" readonly class="form-control-plaintext" /></div>
          </div>           
          <div class="form-group row">
            <label for="tbProcurementType" class="col-sm-2 col-form-label">Procurement Type</label>
            <div class="col-sm-10"><input runat="server" id="tbProcurementType" type="text" readonly class="form-control-plaintext" /></div>
          </div>

          <asp:Panel runat="server" ID="pnlPO">
          <div class="form-group row">
            <label for="tbPO" class="col-sm-2 col-form-label">Purchase Order</label>
            <div class="col-sm-10"><input runat="server" id="tbPO" type="text" readonly class="form-control-plaintext" /></div>
          </div>          
          </asp:Panel>

          <div class="form-group row">
            <label for="tbVendor" class="col-sm-2 col-form-label">Vendor</label>
            <div class="col-sm-10"><input runat="server" id="tbVendor" type="text" readonly class="form-control-plaintext" /></div>
          </div> 
          <div class="form-group row">
            <label for="tbApprover" class="col-sm-2 col-form-label">Approver</label>
            <div class="col-sm-10"><input runat="server" id="tbApprover" type="text" readonly class="form-control-plaintext" /></div>
          </div> 
          <div class="form-group row">
            <label for="tbStatus" class="col-sm-2 col-form-label">Status</label>
            <div class="col-sm-10"><input runat="server" id="tbStatus" type="text" readonly class="form-control-plaintext" /></div>
          </div>                  
            
          <asp:GridView runat="server" ID="gvItems" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover" Width="100%" HeaderStyle-HorizontalAlign="Center" OnRowDataBound="gvItems_RowDataBound">
              <Columns>
                <asp:TemplateField HeaderText="Item Code" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_Code" Text='<%# Eval("Code") %>' /></ItemTemplate></asp:TemplateField>
                    <asp:TemplateField HeaderText="Item Name" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_Name" Text='<%# Eval("Name") %>' /></ItemTemplate></asp:TemplateField>                    
                    <asp:TemplateField HeaderText="Qty" ItemStyle-HorizontalAlign="Right" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_Qty" Text='<%# Eval("Qty", "{0:#,0}") %>' /></ItemTemplate></asp:TemplateField>                    
                    <asp:TemplateField HeaderText="Approved Qty" ItemStyle-HorizontalAlign="Right" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_ApprovedQty" Text='<%# Eval("ApprovedQty", "{0:#,0}") %>' /></ItemTemplate></asp:TemplateField>
                    <asp:TemplateField HeaderText="Pending Qty" ItemStyle-HorizontalAlign="Right" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_PendingQty" Text='<%# Eval("PendingQty", "{0:#,0}") %>' /></ItemTemplate></asp:TemplateField>                    
                    <asp:TemplateField HeaderText="Received Qty" ItemStyle-HorizontalAlign="Right" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_ReceivedQty" Text='<%# Eval("ReceivedQty", "{0:#,0}") %>' /></ItemTemplate></asp:TemplateField>                    
                    <asp:TemplateField HeaderText="Retur Qty" ItemStyle-HorizontalAlign="Right" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_ReturQty" Text='<%# Eval("ReturQty", "{0:#,0}") %>' /></ItemTemplate></asp:TemplateField>
                    <asp:TemplateField HeaderText="Unit Price" ItemStyle-HorizontalAlign="Right" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_UnitPrice" Text='<%# Eval("UnitPrice", "{0:#,0}") %>' /></ItemTemplate></asp:TemplateField>                    
                    <asp:TemplateField HeaderText="Status" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_Status" Text='<%# Eval("Status") %>' /></ItemTemplate></asp:TemplateField>                                                   
              </Columns>
              <HeaderStyle BackColor="#157fcc" ForeColor="White" />
            </asp:GridView>          

        </asp:Panel></div></Content>
      </f:ContentPanel>
    </Items>
  </f:Panel>  
</form>
</body>
</html>
