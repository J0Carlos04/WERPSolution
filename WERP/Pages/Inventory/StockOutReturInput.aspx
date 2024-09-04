<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StockOutReturInput.aspx.cs" Inherits="Pages_Inventory_StockOutReturInput" %>

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
          <f:Button runat="server" ID="btnSubmit" Text="Submit" Icon="DatabaseSave" OnClick="Fbtn_Click" />
          <f:Button runat="server" ID="btnDelete" Text="Delete" Icon="DatabaseDelete" OnClick="Fbtn_Click" ConfirmText="Are you sure you want to delete this Stock Out Retur?. This action cannot be undone. Do you wish to proceed?" ConfirmIcon="Warning"  />
          <f:Button runat="server" ID="btnCancel" Text="Cancel" Icon="Cancel" EnablePostBack="false" />
        </Items>
      </f:Toolbar>
    </Toolbars>
    <Items>
      <f:ContentPanel runat="server" ID="pData" ShowBorder="true" AutoScroll="true"  >
        <Content><div class="wrapper-suppress"><asp:Panel runat="server" ID="pnlContent">
        
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Code<span class="Req">*</span></label>
            <div class="col-sm-10"><input runat="server" id="tbCode" type="text" class="form-control form-control-sm" /></div>
          </div>
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Reason<span class="Req">*</span></label>
            <div class="col-sm-10"><textarea runat="server" id="tbReason" class="form-control form-control-sm" rows="5" /></div>
          </div>
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Receiver<span class="Req">*</span></label>
            <div class="col-sm-10"><asp:DropDownList runat="server" id="ddlReceiver" class="form-select form-select-sm" /></div>
          </div>
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Retur Date<span class="Req">*</span></label>
            <div class="col-sm-10"><input runat="server" id="tbReturDate" type="datetime-local" class="form-control form-control-sm" placeholder="dd/mm/yyyy" onkeydown="return false;"/></div>
          </div>
          
          <asp:Panel runat="server" ID="pnlWorkOrder" Visible="false">
            <div class="row mb-3">
              <label class="col-sm-2 col-form-label col-form-label-sm">Work Order<span class="Req">*</span></label>
              <div class="col-sm-10"><input runat="server" id="tbWorkOrder" type="text" class="form-control form-control-sm" readonly /></div>
            </div>
            </asp:Panel> 

          <asp:Panel runat="server" ID="pnlWorkOrderLookup" Visible="false">
            <div class="row mb-3">            
              <label class="col-sm-2 col-form-label col-form-label-sm">Work Order</label>          
              <div class="col-sm-10">
                <div class="input-group mb-3">
                  <input runat="server" type="text" id="tbWorkOrderLookup" class="form-control form-control-sm" placeholder="Select Work Order" aria-label="Select Work Order" aria-describedby="basic-addon2" readonly />
                  <div class="input-group-append"><asp:Button runat="server" ID="btnWorkOrderLookup" Text="Lookup" CssClass="btn btn-primary btn-sm" OnClick="btn_Click" /></div>
                </div>
              </div>
            </div>
          </asp:Panel>

          <asp:Panel runat="server" ID="pnlCode" Visible="false" style="margin-top:-20px;">
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Item Code<span class="Req">*</span></label>
            <div class="col-sm-10"><input runat="server" id="tbItemCode" type="text" class="form-control form-control-sm" readonly /></div>
          </div>
          </asp:Panel>          

          <asp:Panel runat="server" ID="pnlCodeLookup" Visible="false" style="margin-top:-20px;">
          <div class="row mb-3">            
            <label class="col-sm-2 col-form-label col-form-label-sm">Item Code</label>          
            <div class="col-sm-10">
              <div class="input-group mb-3">
                <input runat="server" type="text" id="tbItemCodeLookup" class="form-control form-control-sm" placeholder="Select Stock Order" aria-label="select Stock Order" aria-describedby="basic-addon2" readonly />
                <div class="input-group-append"><asp:Button runat="server" ID="btnSelectStockOutItemCode" Text="Lookup" CssClass="btn btn-primary btn-sm" OnClick="btn_Click" /></div>
              </div>
            </div>
          </div>
          </asp:Panel>

          <div class="form-group row">
            <label for="tbItemName" class="col-sm-2 col-form-label">Item Name</label>
            <div class="col-sm-10"><input runat="server" id="tbItemName" type="text" readonly class="form-control-plaintext" style="padding-left:10px;" /></div>
          </div>
          <div class="form-group row">
            <label for="tbItemName" class="col-sm-2 col-form-label">SKU</label>
            <div class="col-sm-10"><input runat="server" id="tbSKU" class="form-control-plaintext" style="padding-left:10px;" /></div>
          </div>
          <div class="form-group row">
            <label for="tbUnusedQty" class="col-sm-2 col-form-label">Unused Qty</label>
            <div class="col-sm-10"><input runat="server" id="tbUnusedQty" type="text" readonly class="form-control-plaintext" style="padding-left:10px;" /></div>
          </div>                    
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Qty<span class="Req">*</span></label>
            <div class="col-sm-10"><input runat="server" id="tbQty" type="number" class="form-control form-control-sm" /></div>
          </div>          
            
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Attachment<span class="Req">*</span></label>
            <div class="col-sm-10"><asp:FileUpload runat="server" ID="fu" class="form-control form-control-sm" style="width:200px;display:inline-block"  /></div>
          </div>
            
          <asp:GridView runat="server" ID="gvAttachment" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover" Width="100%" HeaderStyle-HorizontalAlign="Center" OnRowDataBound="gvAttachment_RowDataBound">
            <Columns>
              <asp:TemplateField ItemStyle-HorizontalAlign="Right" HeaderText="No" ItemStyle-BorderStyle="None" ItemStyle-Width="25px"><ItemTemplate>                                                                                                                       
                <asp:Literal runat="server" ID="ltrl_Seq" Text='<%# $"{Eval("Seq")}" %>' Visible='<%# $"{Eval("Seq")}" == "0" ? false : true %>' />                    
              </ItemTemplate></asp:TemplateField>
              <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderText="File Name" ItemStyle-BorderStyle="None"><ItemTemplate>
                <asp:Literal runat="server" ID="ltrl_Id" Text='<%# Eval("ID") %>' Visible="false"  />                
                <asp:Literal runat="server" ID="ltrl_OwnerID" Text='<%# Eval("OwnerID") %>' Visible="false"  /> 
                <asp:Literal runat="server" ID="ltrl_FileNameUniq" Text='<%# Eval("FileNameUniq") %>' Visible="false"  /> 
                <asp:LinkButton runat="server" ID="lb_FileName" Text='<%# Eval("FileName") %>' />
                <asp:Button runat="server" ID="btnDownload" Hidden="true" OnClick="Download" EnableAjax="false" />
              </ItemTemplate></asp:TemplateField>
              <asp:TemplateField HeaderText="Del" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="25px"><ItemTemplate>                                   
                <asp:ImageButton runat="server" ID="imbDeleteAttachment" OnClick="imb_Click" ImageUrl="~/res/images/btnDelete.png" style="line-height:0px;" Visible='<%# $"{Eval("Seq")}" == "0" ? false : true %>' />          
              </ItemTemplate></asp:TemplateField>
            </Columns>
            <HeaderStyle BackColor="#157fcc" ForeColor="White" />
            </asp:GridView>

        </asp:Panel></div></Content>
      </f:ContentPanel>
    </Items>
  </f:Panel>
  <f:Button runat="server" ID="btnUpload" OnClick="Fbtn_Click" Hidden="true" />
  <f:Window ID="wWorkOrder" Title="Select Work Order" Hidden="true" EnableIFrame="true" runat="server" EnableMaximize="true" EnableResize="true" Target="Top" IsModal="false" Width="830px" Height="500px" OnClose="wWorkOrder_Close"></f:Window>  
  <f:Window ID="wSR" Title="Select Stock Out Item" Hidden="true" EnableIFrame="true" runat="server" EnableMaximize="true" EnableResize="true" Target="Top" IsModal="false" Width="830px" Height="500px" OnClose="wSR_Close"></f:Window>  
</form>
</body>
</html>
