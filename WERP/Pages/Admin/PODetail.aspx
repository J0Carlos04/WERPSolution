<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PODetail.aspx.cs" Inherits="Pages_Admin_PODetail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>W.ERP</title>      
    <link href="../../res/css/bootstrap.min.css" rel="stylesheet" />
    <script lang ="javascript" type="text/javascript" src="../../res/js/JScript.js" ></script>
</head>
<body>
<form id="form1" runat="server">
<f:PageManager ID="pm" AutoSizePanelID="pContent" runat="server" Language="EN" AjaxAspnetControls="pContent" /> 
  <f:Panel ID="pContent" runat="server" ShowBorder="false" ShowHeader="false" Layout="Fit" >
    <Toolbars>
      <f:Toolbar runat="server" ID="tlb">
        <Items>          
          <f:Button runat="server" ID="btnCancel" Text="Cancel" Icon="Cancel" EnablePostBack="false" />
        </Items>
      </f:Toolbar>
    </Toolbars>
    <Items>
      <f:ContentPanel runat="server" ID="pData" ShowBorder="true" AutoScroll="true"  >
        <Content><div class="wrapper-suppress">
        
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">PO No.</label>
            <div class="col-sm-10"><input runat="server" id="tbPONo" type="text" class="form-control form-control-sm" readonly /></div>
          </div>
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">PO Date</label>
            <div class="col-sm-10"><input runat="server" id="tbPODate" type="text" class="form-control form-control-sm" readonly /></div>
          </div>
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">PR No.</label>
            <div class="col-sm-10"><input runat="server" id="tbPRNo" type="text" class="form-control form-control-sm" readonly /></div>
          </div>
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Quot No.</label>
            <div class="col-sm-10"><input runat="server" id="tbQuotNo" type="text" class="form-control form-control-sm" readonly /></div>
          </div>                  
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Detail</label>
            <div class="col-sm-10"><textarea runat="server" id="tbDetail" class="form-control form-control-sm" rows="5" readonly /></div>
          </div>
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Attachment</label>
            <div class="col-sm-10"><asp:LinkButton runat="server" ID="lbPO" /></div>
          </div>           
        </div>

        <div style="display:none;">                    
          <asp:Label runat="server" ID="lblPO" />          
        </div>

        </div></Content>
      </f:ContentPanel>
    </Items>
  </f:Panel>  
  <f:Button runat="server" ID="btnDownload" OnClick="Fbtn_Click" Hidden="true" EnableAjax="false" />
</form>
</body>
</html>
