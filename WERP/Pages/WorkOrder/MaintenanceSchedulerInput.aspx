<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MaintenanceSchedulerInput.aspx.cs" Inherits="Pages_WorkOrder_MaintenanceSchedulerInput" %>

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
<f:PageManager ID="pm" AutoSizePanelID="pContent" runat="server" Language="EN" AjaxAspnetControls="ddlSubject,ddlArea,tbLocation,ddlWorkOrderCategory,ddlOperatorType,ddlOperator,ddlVendor,gvSchedule,
  gvItems,gvAttachment" /> 
  <f:Panel ID="pContent" runat="server" ShowBorder="false" ShowHeader="false" Layout="Fit" >
    <Toolbars>
      <f:Toolbar runat="server" ID="tlb">
        <Items>
          <f:Button runat="server" ID="btnSubmit" Text="Submit" Icon="DatabaseSave" OnClick="Fbtn_Click" />
          <f:Button runat="server" ID="btnDelete" Text="Delete" Icon="DatabaseDelete" OnClick="Fbtn_Click" ConfirmText="Are you sure you want to delete this Maintenance Schedule?. This action cannot be undone. Do you wish to proceed?" ConfirmIcon="Warning"  />
          <f:Button runat="server" ID="btnCancel" Text="Cancel" Icon="Cancel" EnablePostBack="false" />          
        </Items>
      </f:Toolbar>
    </Toolbars>
    <Items>
      <f:ContentPanel runat="server" ID="pData" ShowBorder="true" AutoScroll="true"  >
        <Content><div class="wrapper-suppress"><asp:Panel runat="server" ID="pnlContent">
            
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Subject<span class="Req">*</span></label>
            <div class="col-sm-10"><asp:DropDownList runat="server" id="ddlSubject" class="form-select form-select-sm" AutoPostBack="true" OnSelectedIndexChanged="ddl_SelectedIndexChanged" Width="100%" /></div>
          </div>          
          <div id="dvCode" class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Code</label>
            <div class="col-sm-10"><input runat="server" id="tbCode" type="text" class="form-control form-control-sm" readonly /></div>
          </div>    
          
          <div style="margin-bottom:-10px">
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Area<span class="Req">*</span></label>
            <div class="col-sm-10"><asp:DropDownList runat="server" id="ddlArea" AutoPostBack="true" OnSelectedIndexChanged="ddl_SelectedIndexChanged" class="form-select form-select-sm" Width="100%" /></div>
          </div>          
          <div class="row mb-3">
            <f:HiddenField runat="server" ID="hfLocationId" />
            <label class="col-sm-2 col-form-label col-form-label-sm">Location<span class="Req">*</span></label>          
            <div class="col-sm-10">
              <div class="input-group mb-3">
                <input runat="server" type="text" id="tbLocation" class="form-control form-control-sm" placeholder="Select Location" aria-label="select Location" aria-describedby="basic-addon2" readonly />
                <div class="input-group-append"><f:Button runat="server" ID="btnSelectLocation" Text="Lookup" EnablePostBack="false" Height="30" /></div>
              </div>
            </div>
          </div>
          </div>

          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Work Order Category<span class="Req">*</span></label>
            <div class="col-sm-10"><asp:DropDownList runat="server" id="ddlWorkOrderCategory" class="form-select form-select-sm" AutoPostBack="true" OnSelectedIndexChanged="ddl_SelectedIndexChanged" Width="100%" /></div>
          </div>                      
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Work Description</label>
            <div class="col-sm-10"><textarea runat="server" id="tbWorkDescription" class="form-control form-control-sm" rows="5" /></div>
          </div> 
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Operator Type<span class="Req">*</span></label>
            <div class="col-sm-10">
              <asp:DropDownList runat="server" ID="ddlOperatorType" class="form-select form-select-sm" AutoPostBack="true" OnSelectedIndexChanged="ddl_SelectedIndexChanged" Width="100%">
                <asp:ListItem></asp:ListItem>
                <asp:ListItem>Internal</asp:ListItem>
                <asp:ListItem>External</asp:ListItem>
              </asp:DropDownList>
            </div>
          </div>
          <div id="dvVendor" class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Vendor<span class="Req">*</span></label>
            <div class="col-sm-10">
              <asp:DropDownList runat="server" ID="ddlVendor" class="form-select form-select-sm" AutoPostBack="true" OnSelectedIndexChanged="ddl_SelectedIndexChanged" Width="100%" />
            </div>
          </div>
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Conducted By<span class="Req">*</span></label>
            <div class="col-sm-10">
              <asp:DropDownList runat="server" ID="ddlOperator" class="form-select form-select-sm" Width="100%" />
            </div>
          </div>
          
          <label class="col-sm-2 col-form-label col-form-label-sm">Schedule</label>
          <asp:GridView runat="server" ID="gvSchedule" AutoGenerateColumns="false" OnRowDataBound="gvSchedule_RowDataBound" CssClass="table table-striped table-bordered table-hover" Width="100%" HeaderStyle-HorizontalAlign="Center">
            <Columns>
              <asp:TemplateField HeaderText="Start Date"><ItemTemplate>
                <asp:Literal runat="server" ID="ltrl_Id" Text='<%# Eval("Id") %>' Visible="false" />
                <asp:Literal runat="server" ID="ltrlStartDate" Text='<%# $"{Eval("StartDate")}" == "1/1/0001 12:00:00 AM" ? "" : $"{Eval("StartDate", "{0: dd-MMM-yyyy}")}" %>' Visible='<%# $"{Eval("Active")}" == "False" ? true : false %>' />
                <input runat="server" id="tb_StartDate" value='<%# Eval("StartDate") %>' type="date" class="form-control form-control-sm" Visible='<%# $"{Eval("Active")}" == "False" ? false : true %>' />
              </ItemTemplate></asp:TemplateField>
              <asp:TemplateField HeaderText="Period" ItemStyle-Width="160"><ItemTemplate>
                <asp:Literal runat="server" ID="ltrlPeriod" Text='<%# $"{Eval("Period")} {Eval("PeriodType")}" %>' Visible='<%# $"{Eval("Active")}" == "False" && $"{Eval("No")}" != "0" ? true : false %>' />
                <table><tr>
                  <td><input runat="server" id="tb_Period" value='<%# Eval("Period") %>' type="number" class="form-control form-control-sm" Visible='<%# $"{Eval("Active")}" == "False" ? false : true %>' style="width:60px" /></td>                
                  <td><asp:DropDownList runat="server" ID="ddl_PeriodType" CssClass="form-select form-select-sm" Visible='<%# $"{Eval("Active")}" == "False" ? false : true %>' Width="100px" >
                    <asp:ListItem Value="" Text=""></asp:ListItem>
                    <asp:ListItem Value="Year">Year(s)</asp:ListItem>
                    <asp:ListItem Value="Month">Month(s)</asp:ListItem>
                    <asp:ListItem Value="Day">Day(s)</asp:ListItem>
                  </asp:DropDownList></td>
                </tr></table>
              </ItemTemplate></asp:TemplateField>
              <asp:TemplateField HeaderText="End Date"><ItemTemplate>
                <asp:Literal runat="server" ID="ltrlEndDate" Text='<%# $"{Eval("EndDate")}" == "1/1/0001 12:00:00 AM" ? "" : $"{Eval("EndDate", "{0: dd-MMM-yyyy}")}" %>' Visible='<%# $"{Eval("Active")}" == "False" ? true : false %>' />
                <input runat="server" id="tb_EndDate" value='<%# Eval("EndDate") %>' type="date" class="form-control form-control-sm" Visible='<%# $"{Eval("Active")}" == "False" ? false : true %>' />
              </ItemTemplate></asp:TemplateField>
              <asp:TemplateField HeaderText="Work Order" ItemStyle-HorizontalAlign="Center" ><ItemTemplate><asp:LinkButton runat="server" ID="lbWorkOrder" Text="View" onclick='<%# GetWorkOrderUrl(Eval("Id")) %>' Visible='<%# $"{Eval("No")}" == "0" ? false : true %>' /></ItemTemplate></asp:TemplateField>
              <asp:TemplateField HeaderText="Active" ItemStyle-HorizontalAlign="Center" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_Active" Text='<%# Eval("Active") %>' /></ItemTemplate></asp:TemplateField>
            </Columns>
            <HeaderStyle BackColor="#157fcc" ForeColor="White" />
          </asp:GridView>          

          <asp:Panel runat="server" ID="pnlItem" Visible="false">
          <label class="col-sm-2 col-form-label col-form-label-sm">Item</label>
          <asp:GridView runat="server" ID="gvItems" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover" Width="100%" HeaderStyle-HorizontalAlign="Center" OnRowDataBound="gvItems_RowDataBound">
              <Columns>
                <asp:TemplateField >              
                  <HeaderTemplate>Item Code<span class="Req">*</span></HeaderTemplate>
                  <ItemTemplate>
                    <asp:Literal runat="server" ID="ltrl_Id" Text='<%# Eval("Id") %>' Visible="false" />                    
                    <asp:Literal runat="server" ID="ltrl_Seq" Text='<%# Eval("Id") %>' Visible="false" />
                    <asp:Literal runat="server" ID="ltrl_ItemId" Text='<%# Eval("ItemId") %>' Visible="false" />
                    <asp:Literal runat="server" ID="ltrlItemCode" Text='<%# Eval("ItemCode") %>' Visible='<%# $"{Eval("Mode")}" != "" ? true : false %>' />
                    <div runat="server" id="dvCode" style="display: inline-block;width:80%;" Visible='<%# $"{Eval("Mode")}" != "" ? false : true %>'><asp:TextBox runat="server" ID="tb_ItemCode" Text='<%# Eval("ItemCode") %>' CssClass="form-control form-control-sm" ReadOnly="true" /></div>
                    <div runat="server" id="dvCodeLookup" style="display: inline-block" Visible='<%# $"{Eval("Mode")}" != "" ? false : true %>'><asp:Button runat="server" ID="btnCodeLookup" Text="..." CssClass="btn btn-primary btn-sm" /></div>                                
                  </ItemTemplate>
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="Name" ItemStyle-VerticalAlign="Middle">
                  <ItemTemplate><asp:Literal runat="server" ID="ltrl_ItemName" Text='<%# Eval("ItemName") %>' /></ItemTemplate>
                </asp:TemplateField>                 
                <asp:TemplateField>
                  <HeaderTemplate>Qty<span class="Req">*</span></HeaderTemplate>
                  <ItemTemplate>
                    <asp:Literal runat="server" ID="ltrlQty" Text='<%# Eval("Qty") %>' Visible='<%# $"{Eval("Mode")}" != "" ? true : false %>' />
                    <asp:TextBox runat="server" ID="tb_Qty" CssClass="form-control form-control-sm" Text='<%# Eval("Qty") %>' type="number" Visible='<%# $"{Eval("Mode")}" != "" ? false : true %>' />
                  </ItemTemplate>
                </asp:TemplateField>                               
                <asp:TemplateField HeaderText="Del" ItemStyle-HorizontalAlign="Center"  ItemStyle-VerticalAlign="Middle"><ItemTemplate>                                   
                  <asp:ImageButton runat="server" ID="imbDelete" OnClick="imb_Click" ImageUrl="~/res/images/btnDelete.png" style="line-height:0px;" Visible='<%# $"{Eval("Mode")}" != "" ? false : true %>' />          
                </ItemTemplate></asp:TemplateField>
              </Columns>
              <HeaderStyle BackColor="#157fcc" ForeColor="White" />
            </asp:GridView>
          <div style="width:100%; text-align:right;"><asp:Button runat="server" ID="btnAdd" Text="Add" OnClick="btn_Click" CssClass="btn btn-primary btn-sm" /></div>            
          </asp:Panel>
            
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
                <asp:ImageButton runat="server" ID="imbDeleteAttachment" OnClick="imb_Click" ImageUrl="~/res/images/btnDelete.png" style="line-height:0px;" Visible='<%# $"{Eval("Seq")}" == "0" || $"{Eval("Mode")}" != "" ? false : true %>' />          
              </ItemTemplate></asp:TemplateField>
            </Columns>
            <HeaderStyle BackColor="#157fcc" ForeColor="White" />
          </asp:GridView>

        </asp:Panel></div></Content>
      </f:ContentPanel>
    </Items>
  </f:Panel>
  <f:Button runat="server" ID="btnUpload" OnClick="Fbtn_Click" Hidden="true" />    
  <f:Window ID="wLocation" Title="Select Location" Hidden="true" EnableIFrame="true" runat="server" EnableMaximize="true" EnableResize="true" Target="Top" IsModal="false" Width="830px" Height="500px" OnClose="wLocation_Close"></f:Window>
  <f:Window ID="wItem" Title="Select Item" Hidden="true" EnableIFrame="true" runat="server" EnableMaximize="true" EnableResize="true" Target="Top" IsModal="false" Width="830px" Height="500px" OnClose="wItem_Close"></f:Window>
</form>
<script>    
    function getWorkOrderWindowUrl(id) {
        return F.baseUrl + 'Pages/WorkOrder/WorkOrder.aspx?ScheduleId=' + id + '&parenttabid=' + parent.getActiveTabId();
    }
    function updatePage(param1) {
        __doPostBack('', 'UpdatePage$' + param1);
    }
</script>
</body>
</html>
