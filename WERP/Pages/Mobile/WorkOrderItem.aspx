<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WorkOrderItem.aspx.cs" Inherits="Pages_Mobile_WorkOrderItem" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script lang ="javascript" type="text/javascript" src="../../res/js/JScript.js" ></script>     
    <link rel="stylesheet" href="../../res/css/bootstrap.min.css" />
</head>
<body>
<form id="form1" runat="server"><div class="wrapper-suppress">
  <div class="form-floating mb-2">
    <input runat="server" id="tbCode" class="form-control form-control-sm" placeholder="Work Order Code" readonly />
    <label for="tbCode">Work Order Code</label>
  </div>
  <div class="form-floating mb-2">
    <input runat="server" id="tbArea" class="form-control form-control-sm" placeholder="Area" readonly />
    <label for="tbArea">Area</label>
  </div>
  <div class="form-floating mb-2">
    <input runat="server" id="tbLocation" class="form-control form-control-sm" placeholder="Location" readonly />
    <label for="tbLocation">Location</label>
  </div>
  <div class="form-floating mb-2">
    <input runat="server" id="tbCategory" class="form-control form-control-sm" placeholder="Order Category" readonly />
    <label for="tbCategory">Order Category</label>
  </div>
  <div class="form-floating mb-2">
    <input runat="server" id="tbSubject" class="form-control form-control-sm" placeholder="Subject" readonly />
    <label for="tbSubject">Subject</label>
  </div>
  <div class="form-floating mb-2">
    <textarea runat="server" id="tbWorkDescription" class="form-control form-control-sm" placeholder="Work Description" readonly style="min-height:100px;" />
    <label runat="server" id="lblTitleWorkDescription" for="tbWorkDescription">Work Description</label>
  </div>

  <asp:GridView runat="server" ID="gvSchedule" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover" Width="100%" HeaderStyle-HorizontalAlign="Center">
      <Columns>
        <asp:TemplateField HeaderText="Schedule"><ItemTemplate>
          <asp:Literal runat="server" ID="ltrl_Id" Text='<%# Eval("Id") %>' Visible="false" />
          <div>Start Date : <asp:Literal runat="server" ID="ltrlStartDate" Text='<%# $"{Eval("StartDate")}" == "1/1/0001 12:00:00 AM" ? "" : $"{Eval("StartDate", "{0: dd-MMM-yyyy}")}" %>' /></div>
          <div>Period : <asp:Literal runat="server" ID="ltrlPeriod" Text='<%# $"{Eval("Period")} {Eval("PeriodType")}" %>' /></div>
          <div>End Date : <asp:Literal runat="server" ID="ltrlEndDate" Text='<%# $"{Eval("EndDate")}" == "1/1/0001 12:00:00 AM" ? "" : $"{Eval("EndDate", "{0: dd-MMM-yyyy}")}" %>' /></div>
          <div>Order Date : <asp:Literal runat="server" ID="ltrlOrderDate" Text='<%# $"{Eval("OrderDate")}" == "1/1/0001 12:00:00 AM" ? "" : $"{Eval("OrderDate", "{0: dd-MMM-yyyy}")}" %>' /></div>
          <div>Earliest Start Date : <asp:Literal runat="server" ID="ltrlEarliestStartDate" Text='<%# $"{Eval("EarliestStartDate")}" == "1/1/0001 12:00:00 AM" ? "" : $"{Eval("EarliestStartDate", "{0: dd-MMM-yyyy}")}" %>' /></div>
          <div>Lastest Start Date : <asp:Literal runat="server" ID="ltrlLatestStartDate" Text='<%# $"{Eval("LatestStartDate")}" == "1/1/0001 12:00:00 AM" ? "" : $"{Eval("LatestStartDate", "{0: dd-MMM-yyyy}")}" %>' /></div>                                    
        </ItemTemplate></asp:TemplateField>                
      </Columns>
      <HeaderStyle BackColor="#157fcc" ForeColor="White" />
    </asp:GridView>
  
  <asp:GridView runat="server" ID="gvScheduleAttachment" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover" Width="100%" HeaderStyle-HorizontalAlign="Center" OnRowDataBound="gvAttachment_RowDataBound">
    <Columns>      
      <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderText="Attachment" ItemStyle-BorderStyle="None"><ItemTemplate>
        <asp:Literal runat="server" ID="ltrl_Id" Text='<%# Eval("ID") %>' Visible="false"  />                
        <asp:Literal runat="server" ID="ltrl_OwnerID" Text='<%# Eval("OwnerID") %>' Visible="false"  /> 
        <asp:Literal runat="server" ID="ltrl_FileNameUniq" Text='<%# Eval("FileNameUniq") %>' Visible="false"  />
        <div style="display:inline-block"><asp:Literal runat="server" ID="ltrl_Seq" Text='<%# $"{Eval("Seq")}. " %>' Visible='<%# $"{Eval("Seq")}" == "0" ? false : true %>' /></div>
        <div style="display:inline-block"><asp:LinkButton runat="server" ID="lb_FileName" Text='<%# Eval("FileName") %>' /></div>        
        <asp:Button runat="server" ID="btnDownload" Hidden="true" OnClick="btn_Click" />
      </ItemTemplate></asp:TemplateField>                 
    </Columns>
    <HeaderStyle BackColor="#157fcc" ForeColor="White" />
    </asp:GridView> 

  <asp:Panel runat="server" ID="pnlItems">
  <div class="row mb-3">            
    <div class="col-sm-10">
      <div style="display:inline-block"><asp:Button runat="server" ID="btnAddItem" Text="Add" OnClick="btn_Click" CssClass="btn btn-primary btn-sm" /></div>
      <div style="display:inline-block"><asp:Button runat="server" ID="btnDeleteItem" Text="Delete" OnClick="btn_Click" CssClass="btn btn-primary btn-sm" /></div>              
    </div>
  </div>
  <asp:GridView runat="server" ID="gvItems" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover" Width="100%" HeaderStyle-HorizontalAlign="Center" OnRowDataBound="gvItems_RowDataBound">
    <Columns>
        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px">
            <HeaderTemplate><asp:CheckBox ID="cbCheckAll" runat="server" Width="16px" Height="16px" /></HeaderTemplate>
            <ItemTemplate><asp:CheckBox ID="cb_IsChecked" runat="server" Checked='<%# (bool)Eval("IsChecked") %>' /></ItemTemplate>              
        </asp:TemplateField>
        <asp:TemplateField ItemStyle-Width="75px" >              
          <HeaderTemplate>Item Code<span class="Req">*</span></HeaderTemplate>
          <ItemTemplate>
            <asp:Literal runat="server" ID="ltrl_Id" Text='<%# Eval("Id") %>' Visible="false" />                    
            <asp:Literal runat="server" ID="ltrl_Seq" Text='<%# Eval("Id") %>' Visible="false" />
            <asp:Literal runat="server" ID="ltrl_ItemId" Text='<%# Eval("ItemId") %>' Visible="false" />
            <asp:Literal runat="server" ID="ltrlItemCode" Text='<%# Eval("ItemCode") %>' Visible="false" />
            <div style="display:flex;">
              <asp:TextBox runat="server" ID="tb_ItemCode" Text='<%# Eval("ItemCode") %>' CssClass="form-control form-control-sm" style="width:75px;" />
              <asp:Button runat="server" ID="btnCodeLookup" Text="..." CssClass="btn btn-primary btn-sm" OnClick="btn_Click" />
            </div>                                                                
          </ItemTemplate>
        </asp:TemplateField> 
        <asp:TemplateField HeaderText="Name" ItemStyle-VerticalAlign="Middle">
            <ItemTemplate><asp:Literal runat="server" ID="ltrl_ItemName" Text='<%# Eval("ItemName") %>' /></ItemTemplate>
        </asp:TemplateField>                 
        <asp:TemplateField ItemStyle-Width="65">
            <HeaderTemplate>Qty<span class="Req">*</span></HeaderTemplate>
            <ItemTemplate>
            <asp:Literal runat="server" ID="ltrlQty" Text='<%# Eval("Qty") %>' Visible="false" />
            <asp:TextBox runat="server" ID="tb_Qty" CssClass="form-control form-control-sm" Text='<%# Eval("Qty") %>' type="number" />
            </ItemTemplate>
        </asp:TemplateField>                
    </Columns>
    <HeaderStyle BackColor="#157fcc" ForeColor="White" />
  </asp:GridView> 
  </asp:Panel>
    
  <asp:Panel runat="server" ID="pnlLookupItems" Visible="false">
    <div class="row mb-3"><div class="col-sm-10">
      <div style="display:flex;">
        <asp:TextBox runat="server" ID="tbSearch" CssClass="form-control form-control-sm" />
        <asp:Button runat="server" ID="btnSearch" Text="Search" CssClass="btn btn-primary btn-sm" OnClick="btn_Click" />
      </div>
    </div></div>
    <asp:GridView runat="server" ID="gvData" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover" Width="100%" HeaderStyle-HorizontalAlign="Center" OnRowDataBound="gvData_RowDataBound">
      <Columns>            
        <asp:TemplateField HeaderText="No" HeaderStyle-Width="20px" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Right" >
            <ItemTemplate>
            <asp:Literal runat="server" ID="ltrl_Id" Text='<%# Eval("Id")  %>' Visible="false" />
            <asp:Literal runat="server" ID="ltrl_No" Text='<%# $"{Eval("No")}" == "0" ? "New" : ($"{Eval("No")}" == "-1" ? "" : $"{Eval("No")}")  %>' />
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField><ItemTemplate><asp:RadioButton runat="server" ID="rb" OnCheckedChanged="rb_CheckedChanged" AutoPostBack="true" /></ItemTemplate></asp:TemplateField>
            
        <asp:TemplateField HeaderText="Code" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_Code" Text='<%# Eval("Code") %>' /></ItemTemplate></asp:TemplateField>
        <asp:TemplateField HeaderText="Name" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_Name" Text='<%# Eval("Name") %>' /></ItemTemplate></asp:TemplateField>
        <asp:TemplateField HeaderText="Description" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_Description" Text='<%# Eval("Description") %>' /></ItemTemplate></asp:TemplateField>
        <asp:TemplateField HeaderText="Category" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_Category" Text='<%# Eval("Category") %>' /></ItemTemplate></asp:TemplateField>
        <asp:TemplateField HeaderText="Group" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_ItemGroup" Text='<%# Eval("ItemGroup") %>' /></ItemTemplate></asp:TemplateField>
        <asp:TemplateField HeaderText="Brand" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_Brand" Text='<%# Eval("Brand") %>' /></ItemTemplate></asp:TemplateField>
        <asp:TemplateField HeaderText="Model" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_Model" Text='<%# Eval("Model") %>' /></ItemTemplate></asp:TemplateField>
        <asp:TemplateField HeaderText="Material" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_Material" Text='<%# Eval("Material") %>' /></ItemTemplate></asp:TemplateField>
        <asp:TemplateField HeaderText="Specs" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_Specs" Text='<%# Eval("Specs") %>' /></ItemTemplate></asp:TemplateField>
        <asp:TemplateField HeaderText="UOM" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_UOM" Text='<%# Eval("UOM") %>' /></ItemTemplate></asp:TemplateField>
        <asp:TemplateField HeaderText="Size" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_Size" Text='<%# Eval("Size") %>' /></ItemTemplate></asp:TemplateField>
        <asp:TemplateField HeaderText="Threshold" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_Threshold" Text='<%# Eval("Threshold") %>' /></ItemTemplate></asp:TemplateField>
            
        <asp:TemplateField HeaderText="Active" ><ItemTemplate><asp:Literal runat="server" ID="ltrlActive" Text='<%# $"{Eval("No")}" == "-1" ? "" : $"{Eval("Active")}" %>' /></ItemTemplate></asp:TemplateField>
      </Columns>        
    </asp:GridView>
  </asp:Panel>
</div></form>
</body>
</html>
