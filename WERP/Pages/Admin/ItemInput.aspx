<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ItemInput.aspx.cs" Inherits="Pages_ItemInput" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>WERP</title>  
    <meta name="sourcefiles" content="~/Pages/ItemInput.aspx" />
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
      $("#pContent_pData_ddlCategory").select2({ placeholder: "Select Category", allowClear: true, theme: 'bootstrap-5' });      
      $("#pContent_pData_ddlGroup").select2({ placeholder: "Select Group", allowClear: true, theme: 'bootstrap-5' }); 
      $("#pContent_pData_ddlBrand").select2({ placeholder: "Select Brand", allowClear: true, theme: 'bootstrap-5' }); 
      $("#pContent_pData_ddlModel").select2({ placeholder: "Select Model", allowClear: true, theme: 'bootstrap-5' }); 
      $("#pContent_pData_ddlMaterial").select2({ placeholder: "Select Material", allowClear: true, theme: 'bootstrap-5' }); 
      $("#pContent_pData_ddlSpecs").select2({ placeholder: "Select Spec", allowClear: true, theme: 'bootstrap-5' }); 
      $("#pContent_pData_ddlUOM").select2({ placeholder: "Select UOM", allowClear: true, theme: 'bootstrap-5' }); 
  });   
</script>
  <f:Panel ID="pContent" runat="server" ShowBorder="false" ShowHeader="false" Layout="Fit" >
    <Toolbars>
      <f:Toolbar runat="server" ID="tlb">
        <Items>
          <f:Button runat="server" ID="btnSubmit" Text="Submit" Icon="DatabaseSave" OnClick="Fbtn_Click"  />
          <f:Button runat="server" ID="btnDelete" Text="Delete" Icon="DatabaseDelete" OnClick="Fbtn_Click" ConfirmText="Are you sure you want to delete this item?. This action cannot be undone. Do you wish to proceed?" ConfirmIcon="Warning"  />
          <f:Button runat="server" ID="btnCancel" Text="Cancel" Icon="Cancel" EnablePostBack="false" />
        </Items>
      </f:Toolbar>
    </Toolbars>
    <Items>
      <f:ContentPanel runat="server" ID="pData" ShowBorder="true" AutoScroll="true"  >
        <Content><div class="wrapper-suppress">                                
          
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Code</label>
            <div class="col-sm-10"><input runat="server" id="tbCode" type="text" class="form-control form-control-sm" maxlength="30" /></div>
          </div>
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Name</label>
            <div class="col-sm-10"><input runat="server" id="tbName" type="text" class="form-control form-control-sm" /></div>
          </div>
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Description</label>
            <div class="col-sm-10"><textarea runat="server" id="tbDescription" class="form-control form-control-sm" rows="3" /></div>
          </div>
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Category</label>
            <div class="col-sm-10"><asp:DropDownList runat="server" id="ddlCategory" class="form-select form-select-sm" /></div>
          </div>
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Group</label>
            <div class="col-sm-10"><asp:DropDownList runat="server" id="ddlGroup" class="form-select form-select-sm" /></div>
          </div>          
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Brand</label>
            <div class="col-sm-10"><asp:DropDownList runat="server" id="ddlBrand" class="form-select form-select-sm" /></div>
          </div>
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Model</label>
            <div class="col-sm-10"><asp:DropDownList runat="server" id="ddlModel" class="form-select form-select-sm" /></div>
          </div>
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Material</label>
            <div class="col-sm-10"><asp:DropDownList runat="server" id="ddlMaterial" class="form-select form-select-sm" /></div>
          </div>
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Specs</label>
            <div class="col-sm-10"><asp:DropDownList runat="server" id="ddlSpecs" class="form-select form-select-sm" /></div>
          </div>
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">UOM</label>
            <div class="col-sm-10"><asp:DropDownList runat="server" id="ddlUOM" class="form-select form-select-sm" /></div>
          </div>
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Size</label>
            <div class="col-sm-10"><input runat="server" id="tbSize" type="text" class="form-control form-control-sm" /></div>
          </div>
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Threshold</label>
            <div class="col-sm-10"><input runat="server" id="tbThreshold" type="number" class="form-control form-control-sm" /></div>
          </div>
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Use SKU</label>
            <div class="col-sm-10"><asp:CheckBox runat="server" ID="cbUseSKU" CssClass="cb" /></div>
          </div>
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Active</label>
            <div class="col-sm-10">
              <asp:DropDownList runat="server" id="ddlActive" class="form-select form-select-sm" >
                <asp:ListItem>True</asp:ListItem>
                <asp:ListItem>False</asp:ListItem>
              </asp:DropDownList>
            </div>
          </div>                  
        
        </div></Content>
      </f:ContentPanel>
    </Items>
  </f:Panel>
</form>
<script lang="javascript" type="text/javascript" src="../../res/js/bootstrap.bundle.js"></script>
</body>
</html>
