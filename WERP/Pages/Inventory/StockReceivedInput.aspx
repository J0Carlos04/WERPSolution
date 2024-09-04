<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StockReceivedInput.aspx.cs" Inherits="Pages_Inventory_StockReceivedInput" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>W.ERP</title>      
    <link href="../../res/css/bootstrap.min.css" rel="stylesheet" />
    <script lang ="javascript" type="text/javascript" src="../../res/js/JScript.js" ></script>

    <script src="../../res/js/jquery.slim.min.js"></script>    
    <script src="../../res/js/select2.full.min.js"></script>        
    <link rel="stylesheet" href="../../res/css/select2.min.css" />
    <link rel="stylesheet" href="../../res/css/Select2-bootstrap-5-theme.min.css" />
</head>
<body>
<form id="form1" runat="server">
<f:PageManager ID="pm" AutoSizePanelID="pContent" runat="server" Language="EN" AjaxAspnetControls="tbCodeLookup,tbDescription,tbRequester,tbProcurementType,tbPO,tbVendor,tbApprover,tbStatus,gvItems" /> 

  <f:Panel ID="pContent" runat="server" ShowBorder="false" ShowHeader="false" Layout="Fit" >
    <Toolbars>
      <f:Toolbar runat="server" ID="tlb">
        <Items>          
          <f:Button runat="server" ID="btnSubmit" Text="Submit" Icon="DatabaseSave" OnClick="Fbtn_Click" />
          <f:Button runat="server" ID="btnDelete" Text="Delete" Icon="Delete" ConfirmText="Are you sure you want to delete this Stock Received?. This action cannot be undone. Do you wish to proceed?" ConfirmIcon="Warning" OnClick="Fbtn_Click" />
          <f:Button runat="server" ID="btnCancel" Text="Cancel" Icon="Cancel" EnablePostBack="false" OnClientClick="parent.removeActiveTab();" />
          <f:ToolbarFill runat="server" ID="tf1" />
          <f:ToolbarText runat="server" ID="tfInformation" CssStyle="font-weight:bold; color:red;" />
          <f:ToolbarFill runat="server" ID="tf2" />
        </Items>
      </f:Toolbar>
    </Toolbars>
    <Items>
      <f:ContentPanel runat="server" ID="pData" ShowBorder="true" AutoScroll="true"  >
        <Content><div class="wrapper-suppress">
        
          <asp:Panel runat="server" ID="pnlCode" Visible="false">
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm" >Stock Order Code</label>
            <div class="col-sm-10"><input runat="server" id="tbCode" type="text" class="form-control form-control-sm" readonly /></div>
          </div>
          </asp:Panel>

          <asp:Panel runat="server" ID="pnlCodeLookup" Visible="false">
          <div class="form-group row">
            <asp:Label runat="server" ID="lblStockOrderId" Visible="false" />
            <label class="col-sm-2 col-form-label col-form-label-sm">Stock Order Code</label>          
            <div class="col-sm-10">
              <div class="input-group mb-3">
                <input runat="server" type="text" id="tbCodeLookup" class="form-control form-control-sm" placeholder="Select Stock Order" aria-label="select Stock Order" aria-describedby="basic-addon2" readonly />
                <div class="input-group-append"><asp:Button runat="server" ID="btnSelectStockOrder" Text="Lookup" CssClass="btn btn-primary btn-sm" /></div>
              </div>
            </div>
          </div>
          </asp:Panel>
                    
          <div class="form-group row" style="margin-top:-10px;">
            <label for="tbDescription" class="col-sm-2 col-form-label">Description</label>
            <div class="col-sm-10"><input runat="server" id="tbDescription" type="text" readonly class="form-control-plaintext" /></div>
          </div> 
          <div class="form-group row">
            <label for="tbRequester" class="col-sm-2 col-form-label">Requested</label>
            <div class="col-sm-10"><input runat="server" id="tbRequester" type="text" readonly class="form-control-plaintext" /></div>
          </div>
          <div class="form-group row">
            <label for="tbProcurementType" class="col-sm-2 col-form-label">Procurement Type</label>
            <div class="col-sm-10"><input runat="server" id="tbProcurementType" type="text" readonly class="form-control-plaintext" /></div>
          </div>                                      
          <div id="dvPOReceivedItem" class="form-group row">
            <label for="tbPO" class="col-sm-2 col-form-label">Purchase Order</label>
            <div class="col-sm-10"><input runat="server" id="tbPO" type="text" readonly class="form-control-plaintext" /></div>
          </div>           
          <div class="form-group row">
            <label for="tbVendor" class="col-sm-2 col-form-label">Vendor</label>
            <div class="col-sm-10"><input runat="server" id="tbVendor" type="text" readonly class="form-control-plaintext" /></div>
          </div> 
          <div class="form-group row">
            <label for="tbApprover" class="col-sm-2 col-form-label">Approver</label>
            <div class="col-sm-10"><input runat="server" id="tbApprover" type="text" readonly class="form-control-plaintext" /></div>
          </div> 
          <div class="form-group row">
            <label for="tbStatus" class="col-sm-2 col-form-label">Approval Status</label>
            <div class="col-sm-10"><input runat="server" id="tbStatus" type="text" readonly class="form-control-plaintext" /></div>
          </div>          
          <div>&nbsp;</div>

          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Receiver<span class="Req">*</span></label>
            <div class="col-sm-10"><asp:DropDownList runat="server" id="ddlReceiverUserId" class="form-select form-select-sm" /></div>
          </div>
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Received Date<span class="Req">*</span></label>
            <div class="col-sm-10"><input runat="server" id="tbReceivedDate" type="datetime-local" class="form-control form-control-sm" placeholder="dd/mm/yyyy" onkeydown="return false;"/></div>
          </div>
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Invoice No</label>
            <div class="col-sm-10"><input runat="server" id="tbInvoiceNo" type="text" class="form-control form-control-sm" /></div>
          </div>
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Invoice Date</label>
            <div class="col-sm-10"><input runat="server" id="tbInvoiceDate" type="date" class="form-control form-control-sm" placeholder="dd/mm/yyyy" onkeydown="return false;"/></div>
          </div>
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Invoice Attachment</label>
            <div class="col-sm-10">
              <asp:FileUpload runat="server" ID="fuInvoice" class="form-control form-control-sm" style="width:250px;display:inline-block"  />
              <asp:LinkButton runat="server" ID="lbInvoice" style="display:inline-block" />
              <asp:ImageButton runat="server" ID="imbDeleteInvoice" ImageUrl="~/res/images/btnDelete.png" OnClick="imb_Click" />                           
            </div>
          </div>
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">BAST Attachment<span class="Req">*</span></label>
            <div class="col-sm-10">
              <asp:FileUpload runat="server" ID="fuBast" class="form-control form-control-sm" style="width:200px;display:inline-block"  />
              <asp:LinkButton runat="server" ID="lbBast" style="display:inline-block" />
              <asp:ImageButton runat="server" ID="imbDeleteBast" ImageUrl="~/res/images/btnDelete.png" OnClick="imb_Click" />                           
            </div>
          </div>
          <f:Button runat="server" ID="btnDeleteItem" Text="Delete" Icon="Add" OnClick="Fbtn_Click" />
          <asp:GridView runat="server" ID="gvItems" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover" Width="100%" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" OnRowDataBound="gvItems_RowDataBound">
              <Columns>
                <asp:TemplateField HeaderText="No" HeaderStyle-Width="20px" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Right" ><ItemTemplate>
                  <asp:Literal runat="server" ID="ltrl_Id" Text='<%# Eval("Id") %>' Visible="false" />
                  <asp:Literal runat="server" ID="ltrl_StockOrderItemId" Text='<%# Eval("StockOrderItemId") %>' Visible="false" />
                  <asp:Literal runat="server" ID="ltrl_ItemId" Text='<%# Eval("ItemId") %>' Visible="false" />
                  <asp:Literal runat="server" ID="ltrl_AvailableQty" Text='<%# Eval("AvailableQty") %>' Visible="false" />

                  <asp:Literal runat="server" ID="ltrl_ReturQty" Text='<%# Eval("ReturQty") %>' Visible="false" />
                  <asp:Literal runat="server" ID="ltrl_StockOutQty" Text='<%# Eval("StockOutQty") %>' Visible="false" />
                  <asp:Literal runat="server" ID="ltrl_StockOutReturQty" Text='<%# Eval("StockOutReturQty") %>' Visible="false" />                  

                  <asp:Literal runat="server" ID="ltrl_UseSKU" Text='<%# Eval("UseSKU") %>' Visible="false" />
                  <asp:Literal runat="server" ID="ltrl_ShowFirstSKU" Text='<%# Eval("ShowFirstSKU") %>' Visible="false" />
                  <asp:Literal runat="server" ID="ltrl_No" />
                </ItemTemplate></asp:TemplateField>
                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px">
                  <HeaderTemplate><asp:CheckBox ID="cbCheckAll" runat="server" Width="16px" Height="16px" /></HeaderTemplate>
                  <ItemTemplate><asp:CheckBox ID="cb_IsChecked" runat="server" Checked='<%# (bool)Eval("IsChecked") %>' /></ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Item Code" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_ItemCode" Text='<%# Eval("ItemCode") %>' /></ItemTemplate></asp:TemplateField> 
                <asp:TemplateField HeaderText="Name">
                  <ItemTemplate><asp:Literal runat="server" ID="ltrl_ItemName" Text='<%# Eval("ItemName") %>' /></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Warehouse">
                  <ItemTemplate><asp:DropDownList runat="server" ID="ddl_WarehouseId" AutoPostBack="true" OnSelectedIndexChanged="ddl_SelectedIndexChanged" CssClass="form-select form-select-sm" /></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Rack">
                  <ItemTemplate><asp:DropDownList runat="server" ID="ddl_RackId" CssClass="form-select form-select-sm" /></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Approved&nbsp;Qty" ItemStyle-Width="80">
                  <ItemTemplate><asp:Literal runat="server" ID="ltrl_ApprovedQty" Text='<%# Eval("ApprovedQty", "{0:#,0}") %>' /></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Pending&nbsp;Qty" ItemStyle-Width="80">
                  <ItemTemplate><asp:Literal runat="server" ID="ltrl_PendingQty" Text='<%# Eval("PendingQty", "{0:#,0}") %>' /></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Received&nbsp;Qty" ItemStyle-Width="80">
                  <ItemTemplate>
                    <asp:Literal runat="server" Id="ltrlReceivedQty" Text='<%# Eval("ReceivedQty", "{0:#,0}") %>' Visible='<%# Eval("UseSKU").ToBool() %>' />
                    <asp:TextBox runat="server" ID="tb_ReceivedQty" CssClass="form-control form-control-sm" Text='<%# Eval("ReceivedQty", "{0:#,0}") %>' onkeypress="OnlyNumber(event)" onkeyup="this.value = addCommas(this.value);" Visible='<%# !Eval("UseSKU").ToBool() %>' /></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="SKU" ItemStyle-Width="150"><ItemTemplate> 
                  <div style="display:flex">
                    <asp:TextBox runat="server" ID="tb_SKU" CssClass="form-control form-control-sm" Text='<%# Eval("SKU") %>' Visible='<%# Eval("UseSKU").ToBool() %>' />
                    <div style="width:16px; padding-top:6px;"><asp:ImageButton runat="server" ID="imbSeries" OnClick="imb_Click" ImageUrl="~/res/images/ArrowDown.png" Visible='<%# Eval("ShowFirstSKU").ToBool() %>' /></div>
                  </div>                  
                </ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="Unit&nbsp;Price" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="80">
                  <ItemTemplate><asp:Literal runat="server" ID="ltrl_UnitPrice" Text='<%# Eval("UnitPrice", "{0:#,0}") %>' /></ItemTemplate>
                </asp:TemplateField>                
              </Columns>
              <HeaderStyle BackColor="#157fcc" ForeColor="White" />
            </asp:GridView>          

        </div></Content>
      </f:ContentPanel>
    </Items>
  </f:Panel>
  <f:Button runat="server" ID="btnUploadInvoice" OnClick="Fbtn_Click" Hidden="true" EnableAjax="false" />
  <f:Button runat="server" ID="btnDownloadInvoice" OnClick="Fbtn_Click" Hidden="true" EnableAjax="false" />
  <f:Button runat="server" ID="btnUploadBast" OnClick="Fbtn_Click" Hidden="true" EnableAjax="false" />
  <f:Button runat="server" ID="btnDownloadBast" OnClick="Fbtn_Click" Hidden="true" EnableAjax="false" />
  <f:Window ID="wSO" Title="Select Stock Order" Hidden="true" EnableIFrame="true" runat="server" EnableMaximize="true" EnableResize="true" Target="Top" IsModal="false" Width="830px" Height="500px" OnClose="wSO_Close"></f:Window>
</form>
</body>
</html>
