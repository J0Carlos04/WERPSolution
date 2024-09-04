<%@ Page Language="C#" AutoEventWireup="true" CodeFile="POInput.aspx.cs" Inherits="Pages_Admin_POInput" %>

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
<f:PageManager ID="pm" AutoSizePanelID="pContent" runat="server" Language="EN" AjaxAspnetControls="pContent" /> 
<script type="text/javascript">
$(document).ready(function () {
    $("#pContent_pData_ddlVendor").select2({ placeholder: "Select Vendor", allowClear: true, theme: 'bootstrap-5' });        
});   
</script>
  <f:Panel ID="pContent" runat="server" ShowBorder="false" ShowHeader="false" Layout="Fit" >
    <Toolbars>
      <f:Toolbar runat="server" ID="tlb">
        <Items>
          <f:Button runat="server" ID="btnSubmit" Text="Submit" Icon="DatabaseSave" OnClick="Fbtn_Click"  />
          <f:Button runat="server" ID="btnDelete" Text="Delete" Icon="DatabaseDelete" OnClick="Fbtn_Click" ConfirmText="Are you sure you want to delete this Purchase Order?. This action cannot be undone. Do you wish to proceed?" ConfirmIcon="Warning"  />
          <f:Button runat="server" ID="btnCancel" Text="Cancel" Icon="Cancel" EnablePostBack="false" />
        </Items>
      </f:Toolbar>
    </Toolbars>
    <Items>
      <f:ContentPanel runat="server" ID="pData" ShowBorder="true" AutoScroll="true"  >
        <Content><div class="wrapper-suppress">
        
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">PO No.</label>
            <div class="col-sm-10"><input runat="server" id="tbPONo" type="text" class="form-control form-control-sm" /></div>
          </div>
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">PO Date</label>
            <div class="col-sm-10"><input runat="server" id="tbPODate" type="date" class="form-control form-control-sm" /></div>
          </div>
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">PR No.</label>
            <div class="col-sm-10"><input runat="server" id="tbPRNo" type="text" class="form-control form-control-sm" /></div>
          </div>
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Quot No.</label>
            <div class="col-sm-10"><input runat="server" id="tbQuotNo" type="text" class="form-control form-control-sm" /></div>
          </div>                  
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Detail</label>
            <div class="col-sm-10"><textarea runat="server" id="tbDetail" class="form-control form-control-sm" rows="5" /></div>
          </div>
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Attachment</label>
            <div class="col-sm-10">
              <asp:FileUpload runat="server" ID="fuPO" style="display:inline-block;" />
              <asp:LinkButton runat="server" ID="lbPO" />
              <f:Button runat="server" ID="btnDownload" OnClick="Fbtn_Click" Hidden="true" EnableAjax="false" />
            </div>
          </div>           
        </div>

        <div style="display:none;">
          <f:Button runat="server" ID="btnPO" OnClick="Fbtn_Click" />          
          <asp:Label runat="server" ID="lblPO" />          
        </div>

        </div></Content>
      </f:ContentPanel>
    </Items>
  </f:Panel>
</form>
</body>
</html>
