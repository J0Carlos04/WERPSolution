<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Parameters.aspx.cs" Inherits="Pages_Admin_Parameters" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>W.ERP</title>      
    <link href="../../res/css/bootstrap.min.css" rel="stylesheet" />
    <script lang ="javascript" type="text/javascript" src="../../res/js/JScript.js" ></script>
</head>
<body>
<form id="form1" runat="server">
<f:PageManager ID="pm" AutoSizePanelID="pContent" runat="server" Language="EN" AjaxAspnetControls="pnlContent" /> 
  <f:Panel ID="pContent" runat="server" ShowBorder="false" ShowHeader="false" Layout="Fit" >
    <Toolbars>
      <f:Toolbar runat="server" ID="tlb">
        <Items>
          <f:Button runat="server" ID="btnEdit" Text="Edit" Icon="ApplicationEdit" OnClick="btn_Click"  />
          <f:Button runat="server" ID="btnEditSave" Text="Save" Icon="DatabaseSave" OnClick="btn_Click" Hidden="true"  />           
          <f:Button runat="server" ID="btnEditCancel" Text="Cancel" Icon="Cancel" OnClick="btn_Click" Hidden="true" />
          <f:Button runat="server" ID="btnSubmit" Text="Submit" Icon="DatabaseSave" OnClick="btn_Click" />          
          <f:Button runat="server" ID="btnCancel" Text="Cancel" Icon="Cancel" EnablePostBack="false" OnClientClick="parent.removeActiveTab();" />
          <f:Button runat="server" ID="btnSynchCustomer" Text="Synch Customer" Icon="DatabaseSave" OnClick="btn_Click" />
        </Items>
      </f:Toolbar>
    </Toolbars>
    <Items>
      <f:ContentPanel runat="server" ID="pData" ShowBorder="true" AutoScroll="true"  >
        <Content><div class="wrapper-suppress"><asp:Panel runat="server" ID="pnlContent">

          <asp:GridView runat="server" ID="gvData" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover" OnRowDataBound="gvData_RowDataBound" Width="100%" HeaderStyle-HorizontalAlign="Center">
            <Columns>
              <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px">
                <HeaderTemplate><asp:CheckBox ID="cbCheckAll" runat="server" Width="16px" Height="16px" /></HeaderTemplate>
                <ItemTemplate><asp:CheckBox ID="cb_IsChecked" runat="server" Checked='<%# (bool)Eval("IsChecked") %>' /></ItemTemplate>              
              </asp:TemplateField>
              <asp:TemplateField HeaderText="No" HeaderStyle-Width="20px" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Right" >
                <ItemTemplate>
                  <asp:Literal runat="server" ID="ltrl_Mode" Text='<%# Eval("Mode")  %>' Visible="false" />
                  <asp:Literal runat="server" ID="ltrl_Id" Text='<%# Eval("Id")  %>' Visible="false" />
                  <asp:Literal runat="server" ID="ltrl_No" Text='<%# $"{Eval("No")}" == "0" ? "New" : ($"{Eval("No")}" == "-1" ? "" : $"{Eval("No")}")  %>' />
                </ItemTemplate>
              </asp:TemplateField>
    
              <asp:TemplateField HeaderText="Key" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_Key" Text='<%# Eval("Key") %>' /></ItemTemplate></asp:TemplateField> 
              <asp:TemplateField HeaderText="Text" >              
                <ItemTemplate>		    
                  <asp:Literal runat="server" ID="ltrlText" Text='<%# Eval("Text") %>' Visible='<%# Eval("Mode").IsEmpty() %>' />                        
                  <asp:TextBox runat="server" ID="tb_Text" Text='<%# Eval("Text") %>' Visible='<%# !Eval("Mode").IsEmpty() %>' CssClass="form-control form-control-sm" />
                </ItemTemplate>
              </asp:TemplateField>                         
          </Columns>        
          </asp:GridView>
        
          <div class="mb-3">
            <label class="form-label">Work Order Performance Deviation Day (in days)<span class="Req">*</span></label>
            <input runat="server" id="tbWoDeviationDays" type="text" class="form-control form-control-sm" />
          </div>
          <div class="mb-3">
            <label class="form-label">Work Order Performance Deviation Month (in days)<span class="Req">*</span></label>
            <input runat="server" id="tbWoDeviationMonths" type="text" class="form-control form-control-sm" />
          </div>
          <div class="mb-3">
            <label class="form-label">Work Order Performance Deviation Year (in days)<span class="Req">*</span></label>
            <input runat="server" id="tbWoDeviationYears" type="text" class="form-control form-control-sm" />
          </div> 
          <div class="mb-3">
            <label class="form-label">Latitude<span class="Req">*</span></label>
            <input runat="server" id="tbLatitude" type="text" class="form-control form-control-sm" />
          </div>
          <div class="mb-3">
            <label class="form-label">Longitude<span class="Req">*</span></label>
            <input runat="server" id="tbLongitude" type="text" class="form-control form-control-sm" />
          </div>
          <div class="mb-3">
            <label class="form-label">Version<span class="Req">*</span></label>
            <input runat="server" id="tbVersion" type="text" class="form-control form-control-sm" />
          </div>

        </asp:Panel></div></Content>
      </f:ContentPanel>
    </Items>
  </f:Panel>  
</form>
</body>
</html>
