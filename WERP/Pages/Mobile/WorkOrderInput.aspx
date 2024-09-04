<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WorkOrderInput.aspx.cs" Inherits="Pages_Mobile_WorkOrderInput" %>

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
    <input runat="server" id="tbWorkDescription" class="form-control form-control-sm" placeholder="Work Description" readonly />
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

  <asp:Panel runat="server" ID="pnlVendor">
  <div class="form-floating mb-2">
    <input runat="server" id="tbVendor" class="form-control form-control-sm" placeholder="Vendor" readonly />
    <label for="tbVendor">Vendor</label>
  </div>
  </asp:Panel>

  <div class="form-floating mb-2">
    <input runat="server" id="tbOperator" class="form-control form-control-sm" placeholder="Operator" readonly />
    <label for="tbOperator">Operator</label>
  </div>

  <div class="form-floating mb-2">
    <input runat="server" id="tbStartDate" class="form-control form-control-sm" placeholder="Start Date" type="datetime-local" />
    <label for="tbStartDate">Start Date</label>
  </div>

  <div class="form-floating mb-2">
    <input runat="server" id="tbWorkDuration" class="form-control form-control-sm" type="number" />
    <label for="tbWorkDuration">Work Duration</label>
  </div>

  <div class="form-floating mb-2">
    <asp:DropDownList runat="server" id="ddlWorkDurationType" class="form-select form-select-sm" Width="100%" >
      <asp:ListItem></asp:ListItem>
      <asp:ListItem Text="Hours" Value="Hour"></asp:ListItem>
      <asp:ListItem Text="Days" Value="Day"></asp:ListItem>
      <asp:ListItem Text="Months" Value="Month"></asp:ListItem>
      <asp:ListItem Text="Years" Value="Year"></asp:ListItem>
    </asp:DropDownList>
    <label for="ddlWorkDurationType">Work Duration Type</label>
  </div>

  <div class="form-floating mb-2">
    <input runat="server" id="tbTargetCompletion" class="form-control form-control-sm" placeholder="Target Completion" value="Auto Generated by System" />
    <label for="tbTargetCompletion">Target Completion</label>
  </div>

  <div class="form-floating mb-2">
    <asp:DropDownList runat="server" id="ddlStatus" class="form-select form-select-sm" Width="100%" >
      <asp:ListItem></asp:ListItem>
      <asp:ListItem Text="Hours" Value="Hour"></asp:ListItem>
      <asp:ListItem Text="Days" Value="Day"></asp:ListItem>
      <asp:ListItem Text="Months" Value="Month"></asp:ListItem>
      <asp:ListItem Text="Years" Value="Year"></asp:ListItem>
    </asp:DropDownList>
    <label for="ddlStatus">Status</label>
  </div>

  <div class="form-floating mb-2">
    <asp:DropDownList runat="server" id="ddlResult" class="form-select form-select-sm" Width="100%" >
      <asp:ListItem></asp:ListItem>
      <asp:ListItem Text="Hours" Value="Hour"></asp:ListItem>
      <asp:ListItem Text="Days" Value="Day"></asp:ListItem>
      <asp:ListItem Text="Months" Value="Month"></asp:ListItem>
      <asp:ListItem Text="Years" Value="Year"></asp:ListItem>
    </asp:DropDownList>
    <label for="ddlResult">Result</label>
  </div>

  <div class="form-floating mb-2">
    <textarea runat="server" id="tbRemarks" class="form-control form-control-sm" rows="5" style="height:100px;" />
    <label for="tbRemarks">Remarks</label>
  </div>

  <div class="form-floating mb-2">
    <input runat="server" id="tbCloseDate" class="form-control form-control-sm" type="datetime-local" />
    <label for="tbCloseDate">Close Date</label>
  </div>

  <div class="form-floating mb-2">
    <input runat="server" id="tbActualAchievement" class="form-control form-control-sm" value="Auto Generated by System" readonly />
    <label for="tbActualAchievement">Actual Achievement</label>
  </div>

  <asp:Panel runat="server" ID="pnlAchievement" Visible="false">
    <div class="form-floating mb-2">
      <asp:DropDownList runat="server" id="ddlAchievement" class="form-select form-select-sm" Width="100%" >
        <asp:ListItem></asp:ListItem>
        <asp:ListItem>Achieved</asp:ListItem>               
        <asp:ListItem>Not Achieved</asp:ListItem>                
      </asp:DropDownList>
    <label for="ddlResult">Result</label>
  </div>
  </asp:Panel>    
  
</div></form>
</body>
</html>
