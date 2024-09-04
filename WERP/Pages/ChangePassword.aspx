<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChangePassword.aspx.cs" Inherits="Pages_ChangePassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>W.ERP</title>      
    <link href="../res/css/bootstrap.min.css" rel="stylesheet" />
    <script lang ="javascript" type="text/javascript" src="../res/js/JScript.js" ></script> 
    <script>
        function TogglePassword() {
          var x = document.getElementById("pContent_pData_tbCurrentPassword");
          var y = document.getElementById("pContent_pData_tbNewPassword");
          var z = document.getElementById("pContent_pData_tbRepeatPassword");
            if (x.type === "password") {
              x.type = "text";
              y.type = "text";
              z.type = "text";
            } else {
              x.type = "password";
              y.type = "password";
              z.type = "password";
            }
		}        
    </script>
</head>
<body>
  <form id="form1" runat="server">
       <f:PageManager ID="pm" AutoSizePanelID="pContent" runat="server" Language="EN" AjaxAspnetControls="pnlContent" /> 
    <f:Panel ID="pContent" runat="server" ShowBorder="false" ShowHeader="false" Layout="Fit" >
    <Toolbars>
      <f:Toolbar runat="server" ID="tlb">
        <Items>          
          <f:Button runat="server" ID="btnSubmit" Text="Submit" Icon="DatabaseSave" OnClick="btnSubmit_Click" ConfirmText="Are you sure want to change your password ?" ConfirmIcon="Question" />          
          <f:Button runat="server" ID="btnLogout" Text="Logout" OnClientClick="window.location='Logout.aspx'" />           
          <f:ToolbarFill runat="server" ID="tf1" />
          <f:ToolbarText runat="server" ID="tfInformation" CssStyle="font-weight:bold; color:red;" />
          <f:ToolbarFill runat="server" ID="tf2" />
        </Items>
      </f:Toolbar>
    </Toolbars>
    <Items>
      <f:ContentPanel runat="server" ID="pData" ShowBorder="true" AutoScroll="true"  >
        <Content><div class="wrapper-suppress"><asp:Panel runat="server" ID="pnlContent">
        
          <div style="text-align:right; width:100%;"><asp:CheckBox runat="server" ID="cbShowPassword" Text="Show Password" onclick="TogglePassword()" CssClass="cbText" /></div>
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm" >Current Password</label>
            <div class="col-sm-10"><input runat="server" id="tbCurrentPassword" class="form-control form-control-sm" type="password" /></div>
          </div>
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm" >New Password</label>
            <div class="col-sm-10"><input runat="server" id="tbNewPassword" class="form-control form-control-sm" type="password" /></div>
          </div>
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm" >Repeat New Password</label>
            <div class="col-sm-10"><input runat="server" id="tbRepeatPassword" class="form-control form-control-sm" type="password" /></div>
          </div>
          

        </asp:Panel></div></Content>
      </f:ContentPanel>
    </Items>
  </f:Panel>
  </form>
</body>
</html>
