<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserManagementInput.aspx.cs" Inherits="Pages_SuperAdmin_UserManagementInput" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../res/css/bootstrap.min.css" rel="stylesheet" />
    <script lang ="javascript" type="text/javascript" src="../../res/js/JScript.js" ></script>
</head>
<body>
<form id="form1" runat="server">
<f:PageManager ID="pm" AutoSizePanelID="pContent" runat="server" Language="EN" AjaxAspnetControls="gvUsers,gvOperators,gvRole,gvModule" /> 
  <f:Panel ID="pContent" runat="server" ShowBorder="false" ShowHeader="false" Layout="Fit" >
    <Toolbars>
      <f:Toolbar runat="server" ID="tlb">
        <Items>
          <f:Button runat="server" ID="btnSubmit" Text="Submit" Icon="DatabaseSave" OnClick="btn_Click" />
          <f:Button runat="server" ID="btnDelete" Text="Delete" Icon="DatabaseDelete" OnClick="btn_Click" ConfirmText="Are you sure you want to delete this User Management?. This action cannot be undone. Do you wish to proceed?" ConfirmIcon="Warning" />
          <f:Button runat="server" ID="btnCancel" Text="Cancel" Icon="Cancel" EnablePostBack="false" OnClientClick="parent.removeActiveTab();" />
        </Items>
      </f:Toolbar>
    </Toolbars>
    <Items><f:ContentPanel runat="server" ID="pData" ShowBorder="true" AutoScroll="true" ><div class="wrapper-suppress">
      
      <div class="row mb-3">
        <label class="col-sm-2 col-form-label col-form-label-sm">Users<span class="Req">*</span></label>
        <div class="col-sm-10">
          <asp:DropDownList runat="server" id="ddlAllUsers" class="form-select form-select-sm" AutoPostBack="true" OnSelectedIndexChanged="ddl_SelectedIndexChanged" >            
            <asp:ListItem Text="Selected Users"  Value="False"></asp:ListItem>
            <asp:ListItem Text="All Users" Value="True"></asp:ListItem>            
          </asp:DropDownList>          
          <div id="dvSelectedUsers" style="margin-top:2px;">
          <div style="display:inline-block;"><f:Button runat="server" ID="btnSelectUser" Text="Select User" Icon="Add" OnClick="btn_Click" /></div>
          <div style="display:inline-block;"><f:Button runat="server" ID="btnDeleteUser" Text="Delete User" Icon="Delete" OnClick="btn_Click" /></div>
          <asp:GridView runat="server" ID="gvUsers" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover" Width="100%" HeaderStyle-HorizontalAlign="Center" OnRowDataBound="gvUsers_RowDataBound">
            <Columns>
              <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px">
                <HeaderTemplate><asp:CheckBox ID="cbCheckAll" runat="server" Width="16px" Height="16px" /></HeaderTemplate>
                <ItemTemplate><asp:CheckBox ID="cb_IsChecked" runat="server" Checked='<%# (bool)Eval("IsChecked") %>' /></ItemTemplate>              
              </asp:TemplateField>
              <asp:TemplateField HeaderText="No" ItemStyle-Width="20" ItemStyle-HorizontalAlign="Right"><ItemTemplate><asp:Literal runat="server" ID="ltrl_Id" Text='<%# Eval("Id") %>' Visible="false" /><asp:Literal runat="server" ID="ltrl_No" Text='<%# Eval("No") %>' Visible='<%# Eval("Name").IsEmpty() ? false : true %>' /></ItemTemplate></asp:TemplateField>
              <asp:TemplateField HeaderText="Name"><ItemTemplate><asp:Literal runat="server" ID="ltrl_Name" Text='<%# Eval("Name") %>' /></ItemTemplate></asp:TemplateField>
            </Columns>            
          </asp:GridView>
          </div>
        </div>
      </div>

      <div class="row mb-3">
        <label class="col-sm-2 col-form-label col-form-label-sm">Operators</label>
        <div class="col-sm-10">
          <asp:DropDownList runat="server" id="ddlAllOperators" class="form-select form-select-sm" AutoPostBack="true" OnSelectedIndexChanged="ddl_SelectedIndexChanged" >            
            <asp:ListItem Text="Selected Operators"  Value="False"></asp:ListItem>
            <asp:ListItem Text="All Operators" Value="True"></asp:ListItem>            
          </asp:DropDownList>
          <div id="dvSelectedOperators" style="margin-top:2px;">
          <div style="display:inline-block;"><f:Button runat="server" ID="btnSelectOperator" Text="Select Operator" Icon="Add" OnClick="btn_Click" /></div>
          <div style="display:inline-block;"><f:Button runat="server" ID="btnDeleteOperator" Text="Delete Operator" Icon="Delete" OnClick="btn_Click" /></div>
          <asp:GridView runat="server" ID="gvOperators" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover" Width="100%" HeaderStyle-HorizontalAlign="Center" OnRowDataBound="gvOperators_RowDataBound">
            <Columns>
              <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px">
                <HeaderTemplate><asp:CheckBox ID="cbCheckAll" runat="server" Width="16px" Height="16px" /></HeaderTemplate>
                <ItemTemplate><asp:CheckBox ID="cb_IsChecked" runat="server" Checked='<%# (bool)Eval("IsChecked") %>' /></ItemTemplate>              
              </asp:TemplateField>
              <asp:TemplateField HeaderText="No" ItemStyle-Width="20" ItemStyle-HorizontalAlign="Right"><ItemTemplate><asp:Literal runat="server" ID="ltrl_Id" Text='<%# Eval("Id") %>' Visible="false" /><asp:Literal runat="server" ID="ltrl_No" Text='<%# Eval("No") %>' Visible='<%# Eval("Name").IsEmpty() ? false : true %>' /></ItemTemplate></asp:TemplateField>
              <asp:TemplateField HeaderText="Name"><ItemTemplate><asp:Literal runat="server" ID="ltrl_Name" Text='<%# Eval("Name") %>' /></ItemTemplate></asp:TemplateField>
            </Columns>            
          </asp:GridView>
          </div>
        </div>
      </div>

      <div class="row mb-3">
        <label class="col-sm-2 col-form-label col-form-label-sm">Roles</label>
        <div class="col-sm-10">
          <div style="display:inline-block;"><f:Button runat="server" ID="btnSelectRole" Text="Select Role" Icon="Add" OnClick="btn_Click" /></div>
          <div style="display:inline-block;"><f:Button runat="server" ID="btnDeleteRole" Text="Delete Role" Icon="Delete" OnClick="btn_Click" /></div>
          <asp:GridView runat="server" ID="gvRole" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover" Width="100%" HeaderStyle-HorizontalAlign="Center" OnRowDataBound="gvRole_RowDataBound">
            <Columns>
              <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px">
                <HeaderTemplate><asp:CheckBox ID="cbCheckAll" runat="server" Width="16px" Height="16px" /></HeaderTemplate>
                <ItemTemplate><asp:CheckBox ID="cb_IsChecked" runat="server" Checked='<%# (bool)Eval("IsChecked") %>' /></ItemTemplate>              
              </asp:TemplateField>
              <asp:TemplateField HeaderText="No" ItemStyle-Width="20" ItemStyle-HorizontalAlign="Right"><ItemTemplate><asp:Literal runat="server" ID="ltrl_Id" Text='<%# Eval("Id") %>' Visible="false" /><asp:Literal runat="server" ID="ltrl_No" Text='<%# Eval("No") %>' Visible='<%# Eval("Name").IsEmpty() ? false : true %>' /></ItemTemplate></asp:TemplateField>
              <asp:TemplateField HeaderText="Name"><ItemTemplate><asp:Literal runat="server" ID="ltrl_Name" Text='<%# Eval("Name") %>' /></ItemTemplate></asp:TemplateField>
            </Columns>            
          </asp:GridView>
        </div>
      </div>

      <div class="row mb-3">
        <label class="col-sm-2 col-form-label col-form-label-sm">Modules</label>
        <div class="col-sm-10">
          <asp:DropDownList runat="server" id="ddlAllModules" class="form-select form-select-sm" AutoPostBack="true" OnSelectedIndexChanged="ddl_SelectedIndexChanged" >            
           <asp:ListItem Text="Selected Modules"  Value="False"></asp:ListItem>
           <asp:ListItem Text="All Modules" Value="True"></asp:ListItem>            
          </asp:DropDownList>
          <div id="dvSelectedModules" style="margin-top:2px;">
          <div style="display:inline-block;"><f:Button runat="server" ID="btnSelectModule" Text="Select Module" Icon="Add" OnClick="btn_Click" /></div>
          <div style="display:inline-block;"><f:Button runat="server" ID="btnDeleteModule" Text="Delete Module" Icon="Delete" OnClick="btn_Click" /></div>
          <asp:GridView runat="server" ID="gvModule" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover" Width="100%" HeaderStyle-HorizontalAlign="Center" OnRowDataBound="gvModule_RowDataBound">
            <Columns>
              <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px">
                <HeaderTemplate><asp:CheckBox ID="cbCheckAll" runat="server" Width="16px" Height="16px" /></HeaderTemplate>
                <ItemTemplate><asp:CheckBox ID="cb_IsChecked" runat="server" Checked='<%# (bool)Eval("IsChecked") %>' /></ItemTemplate>              
              </asp:TemplateField>
              <asp:TemplateField HeaderText="No" ItemStyle-Width="20" ItemStyle-HorizontalAlign="Right"><ItemTemplate><asp:Literal runat="server" ID="ltrl_Id" Text='<%# Eval("Id") %>' Visible="false" /><asp:Literal runat="server" ID="ltrl_No" Text='<%# Eval("No") %>' Visible='<%# Eval("Name").IsEmpty() ? false : true %>' /></ItemTemplate></asp:TemplateField>
              <asp:TemplateField HeaderText="Name"><ItemTemplate><asp:Literal runat="server" ID="ltrl_Name" Text='<%# Eval("Name") %>' /></ItemTemplate></asp:TemplateField>
            </Columns>            
          </asp:GridView>
          </div>
        </div>
      </div>
      
      <div class="row mb-3">
        <label class="col-sm-2 col-form-label col-form-label-sm">Create<span class="Req">*</span></label>
        <div class="col-sm-10">
          <asp:DropDownList runat="server" id="ddlCreate" class="form-select form-select-sm" >
            <asp:ListItem></asp:ListItem>
            <asp:ListItem>True</asp:ListItem>
            <asp:ListItem>False</asp:ListItem>
          </asp:DropDownList>
        </div>
      </div>

      <div class="row mb-3">
        <label class="col-sm-2 col-form-label col-form-label-sm">View Page<span class="Req">*</span></label>
        <div class="col-sm-10">
          <asp:DropDownList runat="server" id="ddlRead" class="form-select form-select-sm" >
            <asp:ListItem></asp:ListItem>
            <asp:ListItem>True</asp:ListItem>
            <asp:ListItem>False</asp:ListItem>
          </asp:DropDownList>
        </div>
      </div>

      <div class="row mb-3">
        <label class="col-sm-2 col-form-label col-form-label-sm">Update<span class="Req">*</span></label>
        <div class="col-sm-10">
          <asp:DropDownList runat="server" id="ddlUpdate" class="form-select form-select-sm" >
            <asp:ListItem></asp:ListItem>
            <asp:ListItem>True</asp:ListItem>
            <asp:ListItem>False</asp:ListItem>
          </asp:DropDownList>
        </div>
      </div>

      <div class="row mb-3">
        <label class="col-sm-2 col-form-label col-form-label-sm">Delete<span class="Req">*</span></label>
        <div class="col-sm-10">
          <asp:DropDownList runat="server" id="ddlDelete" class="form-select form-select-sm" >
            <asp:ListItem></asp:ListItem>
            <asp:ListItem>True</asp:ListItem>
            <asp:ListItem>False</asp:ListItem>
          </asp:DropDownList>
        </div>
      </div>

      <div class="row mb-3">
        <label class="col-sm-2 col-form-label col-form-label-sm">View All Data<span class="Req">*</span></label>
        <div class="col-sm-10">
          <asp:DropDownList runat="server" id="ddlViewAllData" class="form-select form-select-sm" >
            <asp:ListItem></asp:ListItem>
            <asp:ListItem>True</asp:ListItem>
            <asp:ListItem>False</asp:ListItem>
          </asp:DropDownList>
        </div>
      </div>

      <div class="row mb-3">
        <label class="col-sm-2 col-form-label col-form-label-sm">Deviation<span class="Req">*</span></label>
        <div class="col-sm-10">
          <asp:DropDownList runat="server" id="ddlDeviation" class="form-select form-select-sm" >
            <asp:ListItem></asp:ListItem>
            <asp:ListItem>True</asp:ListItem>
            <asp:ListItem>False</asp:ListItem>
          </asp:DropDownList>
        </div>
      </div>
      
    </div></f:ContentPanel></Items>
  </f:Panel>
  <f:Window ID="wUsers" Title="Select Users" Hidden="true" EnableIFrame="true" runat="server" EnableMaximize="true" EnableResize="true" Target="Top" IsModal="false" Width="830px" Height="500px" OnClose="wUsers_Close"></f:Window>  
  <f:Window ID="wOperators" Title="Select Operators" Hidden="true" EnableIFrame="true" runat="server" EnableMaximize="true" EnableResize="true" Target="Top" IsModal="false" Width="830px" Height="500px" OnClose="wOperators_Close"></f:Window>
  <f:Window ID="wRole" Title="Select Roles" Hidden="true" EnableIFrame="true" runat="server" EnableMaximize="true" EnableResize="true" Target="Top" IsModal="false" Width="830px" Height="500px" OnClose="wRole_Close"></f:Window>
  <f:Window ID="wModule" Title="Select Modules" Hidden="true" EnableIFrame="true" runat="server" EnableMaximize="true" EnableResize="true" Target="Top" IsModal="false" Width="830px" Height="500px" OnClose="wModule_Close"></f:Window>
</form>
</body>
</html>
