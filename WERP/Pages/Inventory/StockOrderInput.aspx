<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StockOrderInput.aspx.cs" Inherits="Pages_Inventory_StockOrderInput" %>

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
<f:PageManager ID="pm" AutoSizePanelID="pContent" runat="server" Language="EN" AjaxAspnetControls="pnlContent" /> 
  <f:Panel ID="pContent" runat="server" ShowBorder="false" ShowHeader="false" Layout="Fit" >
    <Toolbars>
      <f:Toolbar runat="server" ID="tlb">
        <Items>
          <f:Button runat="server" ID="btnSubmit" Text="Submit" Icon="DatabaseSave" OnClick="Fbtn_Click" ConfirmText="Are you sure want to submit this Stock Order ?"  />
          <f:Button runat="server" ID="btnDelete" Text="Delete" Icon="DatabaseDelete" OnClick="Fbtn_Click" ConfirmText="Are you sure you want to delete this Stock Order?. This action cannot be undone. Do you wish to proceed?" ConfirmIcon="Warning"  />
          <f:Button runat="server" ID="btnCancel" Text="Cancel" Icon="Cancel" EnablePostBack="false" OnClientClick="parent.removeActiveTab();" />
        </Items>
      </f:Toolbar>
    </Toolbars>
    <Items>
      <f:ContentPanel runat="server" ID="pData" ShowBorder="true" AutoScroll="true"  >
        <Content><div class="wrapper-suppress"><asp:Panel runat="server" ID="pnlContent">
        
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Code<span class="Req">*</span></label>
            <div class="col-sm-10"><input runat="server" id="tbCode" type="text" class="form-control form-control-sm" onkeypress="return NoSpace(event);" /></div>
          </div>
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Description</label>
            <div class="col-sm-10"><textarea runat="server" id="tbDescription" class="form-control form-control-sm" rows="5" /></div>
          </div>
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Requester<span class="Req">*</span></label>
            <div class="col-sm-10"><asp:DropDownList runat="server" id="ddlRequester" class="form-select form-select-sm" /></div>
          </div>
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Procurement Type</label>
            <div class="col-sm-10">
              <asp:DropDownList runat="server" id="ddlProcurementType" class="form-select form-select-sm" AutoPostBack="true" OnSelectedIndexChanged="ddl_SelectedIndexChanged" >
                <asp:ListItem>Regular</asp:ListItem>
                <asp:ListItem>Direct</asp:ListItem>
              </asp:DropDownList>
            </div>
          </div>          

          <asp:Panel runat="server" ID="pnlPO">
          <div class="row mb-3">
            <asp:Label runat="server" ID="lblPOId" Visible="false" />
            <label class="col-sm-2 col-form-label col-form-label-sm">Purchase Order (PO)<span class="Req">*</span></label>          
            <div class="col-sm-10">
              <div class="input-group mb-3">
                <input runat="server" type="text" id="tbPONo" class="form-control form-control-sm" placeholder="Select Purchase Order" aria-label="select Purchase Order" aria-describedby="basic-addon2" readonly />
                <div class="input-group-append"><asp:Button runat="server" ID="btnSelectPO" Text="Lookup" CssClass="btn btn-primary btn-sm" /></div>
              </div>
            </div>
          </div>
          </asp:Panel>

          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Vendor<span class="Req">*</span></label>
            <div class="col-sm-10"><asp:DropDownList runat="server" id="ddlVendor" class="form-select form-select-sm" /></div>
          </div>
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Approver<span class="Req">*</span></label>
            <div class="col-sm-10"><asp:DropDownList runat="server" id="ddlApprover" class="form-select form-select-sm" /></div>
          </div>
          <div class="form-group row" style="margin-top:-10px;">
            <label for="tbStatus" class="col-sm-2 col-form-label">Status</label>
            <div class="col-sm-10"><input runat="server" id="tbStatus" type="text" readonly class="form-control-plaintext" /></div>
          </div>                  
            
          <asp:GridView runat="server" ID="gvItems" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover" Width="100%" HeaderStyle-HorizontalAlign="Center" OnRowDataBound="gvItems_RowDataBound">
              <Columns>
                <asp:TemplateField >              
                  <HeaderTemplate>Item Code<span class="Req">*</span></HeaderTemplate>
                  <ItemTemplate>
                    <asp:Literal runat="server" ID="ltrl_Id" Text='<%# Eval("Id") %>' Visible="false" />
                    <asp:Literal runat="server" ID="ltrl_StockOrderId" Text='<%# Eval("StockOrderId") %>' Visible="false" />
                    <asp:Literal runat="server" ID="ltrl_ItemId" Text='<%# Eval("ItemId") %>' Visible="false" />
                    <div runat="server" id="dvCode" style="display: inline-block;width:80%;"><asp:TextBox runat="server" ID="tb_Code" Text='<%# Eval("Code") %>' CssClass="form-control form-control-sm" ReadOnly="true" /></div>
                    <div runat="server" id="dvCodeLookup" style="display: inline-block"><asp:Button runat="server" ID="btnCodeLookup" Text="..." CssClass="btn btn-primary btn-sm" /></div>                                
                  </ItemTemplate>
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="Name" ItemStyle-VerticalAlign="Middle">
                  <ItemTemplate><asp:Literal runat="server" ID="ltrl_Name" Text='<%# Eval("Name") %>' /></ItemTemplate>
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="Reject&nbsp;Reason" ItemStyle-VerticalAlign="Middle" Visible="false">
                  <ItemTemplate><asp:Literal runat="server" ID="ltrl_RejectReason" Text='<%# Eval("RejectReason") %>' /></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                  <HeaderTemplate>Qty<span class="Req">*</span></HeaderTemplate>
                  <ItemTemplate><asp:TextBox runat="server" ID="tb_Qty" CssClass="form-control form-control-sm" Text='<%# Eval("Qty", "{0:#,0}") %>' onkeypress="OnlyNumber(event)" onkeyup="this.value = addCommas(this.value);" /></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                  <HeaderTemplate>Unit Price<span class="Req">*</span></HeaderTemplate>
                  <ItemTemplate><asp:TextBox runat="server" ID="tb_UnitPrice" CssClass="form-control form-control-sm" Text='<%# Eval("UnitPrice", "{0:#,0}") %>' onkeypress="OnlyNumber(event)" onkeyup="this.value = addCommas(this.value);" /></ItemTemplate>
                </asp:TemplateField>                
                <asp:TemplateField HeaderText="Del" ItemStyle-HorizontalAlign="Center"  ItemStyle-VerticalAlign="Middle"><ItemTemplate>                                   
                  <asp:ImageButton runat="server" ID="imbDelete" OnClick="imb_Click" ImageUrl="~/res/images/btnDelete.png" style="line-height:0px;" />          
                </ItemTemplate></asp:TemplateField>
              </Columns>
              <HeaderStyle BackColor="#157fcc" ForeColor="White" />
            </asp:GridView>
          <div style="width:100%; text-align:right;"><asp:Button runat="server" ID="btnAdd" Text="Add" OnClick="btn_Click" CssClass="btn btn-primary btn-sm" /></div>            

        </asp:Panel></div></Content>
      </f:ContentPanel>
    </Items>
  </f:Panel>
  <f:Window ID="wPO" Title="Select PO" Hidden="true" EnableIFrame="true" runat="server" EnableMaximize="true" EnableResize="true" Target="Top" IsModal="false" Width="830px" Height="500px" OnClose="wPO_Close"></f:Window>
  <f:Window ID="wItem" Title="Select Item" Hidden="true" EnableIFrame="true" runat="server" EnableMaximize="true" EnableResize="true" Target="Top" IsModal="false" Width="830px" Height="500px" OnClose="wItem_Close"></f:Window>
</form>
</body>
</html>
