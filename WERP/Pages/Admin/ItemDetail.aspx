<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ItemDetail.aspx.cs" Inherits="Pages_Admin_ItemDetail" %>

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
            <label class="col-sm-2 col-form-label col-form-label-sm">Code</label>
            <div class="col-sm-10"><input runat="server" id="tbCode" type="text" class="form-control form-control-sm" readonly /></div>
          </div>
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Name</label>
            <div class="col-sm-10"><input runat="server" id="tbName" type="text" class="form-control form-control-sm" readonly /></div>
          </div>
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Description</label>
            <div class="col-sm-10"><textarea runat="server" id="tbDescription" class="form-control form-control-sm" rows="3" readonly /></div>
          </div>
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Category</label>
            <div class="col-sm-10"><input runat="server" id="tbCategory" type="text" class="form-control form-control-sm" readonly /></div>
          </div>
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Group</label>
            <div class="col-sm-10"><input runat="server" id="tbGroup" type="text" class="form-control form-control-sm" readonly /></div>
          </div>          
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Brand</label>
            <div class="col-sm-10"><input runat="server" id="tbBrand" type="text" class="form-control form-control-sm" readonly /></div>
          </div>
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Model</label>
            <div class="col-sm-10"><input runat="server" id="tbModel" type="text" class="form-control form-control-sm" readonly /></div>
          </div>
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Material</label>
            <div class="col-sm-10"><input runat="server" id="tbMaterial" type="text" class="form-control form-control-sm" readonly /></div>
          </div>
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Specs</label>
            <div class="col-sm-10"><input runat="server" id="tbSpecs" type="text" class="form-control form-control-sm" readonly /></div>
          </div>
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">UOM</label>
            <div class="col-sm-10"><input runat="server" id="tbUOM" type="text" class="form-control form-control-sm" readonly /></div>
          </div>
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Size</label>
            <div class="col-sm-10"><input runat="server" id="tbSize" type="number" class="form-control form-control-sm" readonly /></div>
          </div>
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Threshold</label>
            <div class="col-sm-10"><input runat="server" id="tbThreshold" type="number" class="form-control form-control-sm" readonly /></div>
          </div>
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Active</label>
            <div class="col-sm-10"><input runat="server" id="tbActive" type="text" class="form-control form-control-sm" readonly /></div>
          </div>                  
        
        </div></Content>
      </f:ContentPanel>
    </Items>
  </f:Panel>
</form>
<script lang="javascript" type="text/javascript" src="../../res/js/bootstrap.bundle.js"></script>
</body>
</html>
