<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StockOutInput.aspx.cs" Inherits="Pages_Inventory_StockOutInput" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>WERP</title>      
    <link href="../../res/css/bootstrap.min.css" rel="stylesheet" />
    <script lang ="javascript" type="text/javascript" src="../../res/js/JScript.js" ></script>
</head>
<body>
<form id="form1" runat="server">
<f:PageManager ID="pm" AutoSizePanelID="pContent" runat="server" Language="EN" AjaxAspnetControls="tbWorkOrderCode,ddlStockOutType,tbWorkOrderCodeLookup,tbWODescription,gvItems" /> 

  <f:Panel ID="pContent" runat="server" ShowBorder="false" ShowHeader="false" Layout="Fit" >
    <Toolbars>
      <f:Toolbar runat="server" ID="tlb">
        <Items>          
          <f:Button runat="server" ID="btnSubmit" Text="Submit" Icon="DatabaseSave" OnClick="Fbtn_Click" />
          <f:Button runat="server" ID="btnDelete" Text="Delete" Icon="DatabaseDelete" OnClick="Fbtn_Click" ConfirmText="Are you sure you want to delete this Stock Out?. This action cannot be undone. Do you wish to proceed?" ConfirmIcon="Warning"  />
          <f:Button runat="server" ID="btnCancel" Text="Cancel" Icon="Cancel" EnablePostBack="false" />
        </Items>
      </f:Toolbar>
    </Toolbars>
    <Items>
      <f:ContentPanel runat="server" ID="pData" ShowBorder="true" AutoScroll="true"  >
        <Content><div class="wrapper-suppress">
                                    
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Code<span class="Req">*</span></label>
            <div class="col-sm-10"><input runat="server" id="tbCode" type="text" class="form-control form-control-sm" readonly /></div>
          </div>
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Description</label>
            <div class="col-sm-10"><textarea runat="server" id="tbDescription" class="form-control form-control-sm" rows="5" /></div>
          </div>
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Out Date<span class="Req">*</span></label>
            <div class="col-sm-10"><input runat="server" id="tbOutDate" type="datetime-local" class="form-control form-control-sm" placeholder="dd/mm/yyyy" onkeydown="return false;"/></div>
          </div>
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Takes Out By<span class="Req">*</span></label>
            <div class="col-sm-10"><asp:DropDownList runat="server" ID="ddlTakesOutUserId" class="form-select form-select-sm" /></div>
          </div> 

          <div id="dvCode" class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm"><asp:Label runat="server" ID="lblCodeTitle" /><span class="Req">*</span></label>
            <div class="col-sm-10"><input runat="server" id="tbWorkOrderCode" type="text" class="form-control form-control-sm" readonly /></div>
          </div>                             
                    
          <div id="dvCodeLookup" class="row mb-3">
            <f:HiddenField runat="server" ID="hfHelpdeskId" />
            <f:HiddenField runat="server" ID="hfWorkOrderId" />
            <asp:DropDownList runat="server" ID="ddlStockOutType" AutoPostBack="true" OnSelectedIndexChanged="ddl_SelectedIndexChanged" class="col-sm-2 col-form-label col-form-label-sm" style="border:1px solid #ced4da; height:50%" >
              <asp:ListItem Text="Work Order" Value="Workorder" />
              <asp:ListItem Text="Helpdesk" Value="Helpdesk" />
            </asp:DropDownList>                      
            <div class="col-sm-10">
              <div class="input-group mb-3">
                <input runat="server" type="text" id="tbWorkOrderCodeLookup" class="form-control form-control-sm" placeholder="Select Helpdesk / Work Order" aria-label="Select Helpdesk / Work Order" aria-describedby="basic-addon2" readonly />
                <div class="input-group-append"><f:Button runat="server" ID="btnSelectWorkOrder" Text="Lookup" CssClass="btn btn-primary btn-sm" Height="30px" /></div>
              </div>
            </div>
          </div>
            
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm"><asp:Label runat="server" ID="lblDescriptionTitle" /></label>
            <div class="col-sm-10"><textarea runat="server" id="tbWODescription" class="form-control form-control-sm" rows="5" /></div>
          </div>

          <asp:GridView runat="server" ID="gvItems" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover" Width="100%" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" OnRowDataBound="gvItems_RowDataBound" OnDataBound="gvItems_DataBound">
              <Columns>
                <asp:TemplateField HeaderText="No" ItemStyle-HorizontalAlign="Right"><ItemTemplate><asp:Literal runat="server" ID="ltrl_No" /> </ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="Work Order"><ItemTemplate><asp:Literal runat="server" ID="ltrl_WorkOrderCode" Text='<%# Eval("WorkOrderCode") %>' /></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="Item Code" >              
                  <ItemTemplate>
                    <asp:Literal runat="server" ID="ltrl_Id" Text='<%# Eval("Id") %>' Visible="false" />                    
                    <asp:Literal runat="server" ID="ltrl_ItemId" Text='<%# Eval("ItemId") %>' Visible="false" />                   
                    <asp:Literal runat="server" ID="lttl_StockReceivedItemId" Text='<%# Eval("StockReceivedItemId") %>' Visible="false" />
                    <asp:Literal runat="server" ID="ltrl_UseSKU" Text='<%# Eval("UseSKU") %>' Visible="false" />
                    <asp:Literal runat="server" ID="ltrl_WarehouseId" Text='<%# Eval("WarehouseId") %>' Visible="false" />
                    <asp:Literal runat="server" ID="ltrl_RackId" Text='<%# Eval("RackId") %>' Visible="false" />                    
                    <asp:Literal runat="server" ID="ltrl_ItemCode" Text='<%# Eval("ItemCode") %>' />
                  </ItemTemplate>
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="Name">
                  <ItemTemplate><asp:Literal runat="server" ID="ltrl_ItemName" Text='<%# Eval("ItemName") %>' /></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Warehouse">
                  <ItemTemplate><asp:Literal runat="server" ID="ltrl_Warehouse" Text='<%# Eval("Warehouse") %>' /></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Rack">
                  <ItemTemplate><asp:Literal runat="server" ID="ltrl_Rack" Text='<%# Eval("Rack") %>' /></ItemTemplate>
                </asp:TemplateField>                                            
                <asp:TemplateField HeaderText="Qty" ItemStyle-HorizontalAlign="Right">
                  <ItemTemplate><asp:Literal runat="server" ID="ltrl_Qty" Text='<%# Eval("Qty") %>' /></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Unused&nbsp;Qty" ItemStyle-HorizontalAlign="Right">
                  <ItemTemplate><asp:Literal runat="server" ID="ltrl_UnusedQty" Text='<%# Eval("UnusedQty") %>' /></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Used&nbsp;Qty" ItemStyle-HorizontalAlign="Right">
                  <ItemTemplate><asp:Literal runat="server" ID="ltrl_UsedQty" Text='<%# Eval("UsedQty") %>' /></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="SKU">
                  <ItemTemplate>
                    <asp:Literal runat="server" ID="ltrlSKU" Text='<%# Eval("SKU") %>' Visible='<%# Eval("StockReceivedItemId").ToInt().IsNotZero() %>' />
                    <asp:TextBox runat="server" ID="tb_SKU" Text='<%# Eval("SKU") %>' class="form-control form-control-sm" ReadOnly='<%# Eval("Qty") == Eval("UsedQty") ? true : false %>' Visible='<%# Eval("StockReceivedItemId").ToInt().IsZero() %>' /></ItemTemplate>
                </asp:TemplateField>
              </Columns>
              <HeaderStyle BackColor="#157fcc" ForeColor="White" />
            </asp:GridView>          

        </div></Content>
      </f:ContentPanel>
    </Items>
  </f:Panel>  
  <f:Window ID="wo" Title="Select Work Order" Hidden="true" EnableIFrame="true" runat="server" EnableMaximize="true" EnableResize="true" Target="Top" IsModal="false" Width="830px" Height="500px" OnClose="wo_Close"></f:Window>
</form>    
</body>
</html>
