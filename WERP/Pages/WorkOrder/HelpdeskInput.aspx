<%@ Page Language="C#" AutoEventWireup="true" CodeFile="HelpdeskInput.aspx.cs" Inherits="Pages_WorkOrder_HelpdeskInput" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>W.ERP</title>
    <link href="../../res/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../res/css/Collapsible.css" rel="stylesheet" />
    <script lang ="javascript" type="text/javascript" src="../../res/js/JScript.js" ></script>

    <script src="../../res/js/jquery.slim.min.js"></script>    
    <script src="../../res/js/select2.full.min.js"></script>        
    <link rel="stylesheet" href="../../res/css/select2.min.css" />
    <link rel="stylesheet" href="../../res/css/Select2-bootstrap-5-theme.min.css" />

    
</head>
<body>
<form id="form1" runat="server">
<f:PageManager ID="pm" AutoSizePanelID="pContent" runat="server" Language="EN" AjaxAspnetControls="ddlArea,tbLocation,ddlSubject,ddlRequestSource,ddlOperatorType,ddlOperator,ddlVendor,ddlStatus,
    gvCustomer,gvConductedBy,fu,gvItems,gvAttachment" /> 
  <f:Panel ID="pContent" runat="server" ShowBorder="false" ShowHeader="false" Layout="Fit" >
    <Toolbars>
      <f:Toolbar runat="server" ID="tlb">
        <Items>
          <f:Button runat="server" ID="btnSubmit" Text="Submit" Icon="DatabaseSave" OnClick="Fbtn_Click" />
          <f:Button runat="server" ID="btnDelete" Text="Delete" Icon="DatabaseDelete" OnClick="Fbtn_Click" ConfirmText="Are you sure you want to delete this Helpdesk?. This action cannot be undone. Do you wish to proceed?" ConfirmIcon="Warning"  />
          <f:Button runat="server" ID="btnCancel" Text="Cancel" Icon="Cancel" EnablePostBack="false" OnClientClick="parent.removeActiveTab();" />
          <f:Button runat="server" ID="btnViewOrder" Text="View Order" Icon="WorkOrder" OnClick="Fbtn_Click" />
        </Items>
      </f:Toolbar>
    </Toolbars>
    <Items>
      <f:ContentPanel runat="server" ID="pData" ShowBorder="true" AutoScroll="true"  >
        <Content><div class="wrapper-suppress">
          
          <div><div class="squarebox">
          <div class="squareboxgradientcaption" style="cursor: pointer;" onclick="togglePannelAnimatedStatus(this.nextElementSibling,50,50)">
            <div style="float: left;  margin-top:3px; ">General Information</div>
            <div style="float: right;  margin-top:3px;"><img src="../../res/images/collapse.png" width="16" height="16" border="0" alt="Show/Hide" title="Show/Hide" />&nbsp;</div>
          </div>
          <div class="squareboxcontent" ><div class="wrapper-suppress">          
          
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Subject<span class="Req">*</span></label>
            <div class="col-sm-10"><asp:DropDownList runat="server" id="ddlSubject" class="form-select form-select-sm" AutoPostBack="true" OnSelectedIndexChanged="ddl_SelectedIndexChanged" Width="100%" /></div>
          </div> 
          <div id="dvCust" style="margin-left:-12px; margin-right:-12px; margin-bottom:20px"><div class="squarebox">
          <div class="squareboxgradientcaption" style="cursor: pointer;"onclick="togglePannelAnimatedStatus(this.nextElementSibling,50,50)">
            <div style="float: left;  margin-top:3px; ">Customer</div>
            <div style="float: right;  margin-top:3px;"><img src="../../res/images/collapse.png" width="16" height="16" border="0" alt="Show/Hide" title="Show/Hide" />&nbsp;</div>
          </div>
          <div class="squareboxcontent" ><div class="wrapper-suppress">
            <div style="display:inline-block"><f:Button runat="server" ID="btnAddCust" Text="Add" Icon="Add" EnablePostBack="false" /></div>
            <div style="display:inline-block"><f:Button runat="server" ID="btnDeleteCust" Text="Delete" Icon="Add" OnClick="Fbtn_Click" /></div>
            <asp:GridView runat="server" ID="gvCustomer" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover" Width="100%" HeaderStyle-HorizontalAlign="Center" OnRowDataBound="gvCustomer_RowDataBound">
              <Columns>            
                <asp:TemplateField HeaderText="No" HeaderStyle-Width="20px" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Right" >
                  <ItemTemplate>
                    <asp:Literal runat="server" ID="ltrl_Id" Text='<%# Eval("Id")  %>' Visible="false" />
                    <asp:Literal runat="server" ID="ltrl_Used" Text='<%# Eval("Used")  %>' Visible="false" />
                    <asp:Literal runat="server" ID="ltrl_LocationId" Text='<%# Eval("LocationId")  %>' Visible="false" />
                    <asp:Literal runat="server" ID="ltrl_Latitude" Text='<%# Eval("Latitude")  %>' Visible="false" />
                    <asp:Literal runat="server" ID="ltrl_Longitude" Text='<%# Eval("Longitude")  %>' Visible="false" />
                    <asp:Literal runat="server" ID="ltrl_CreatedBy" Text='<%# Eval("CreatedBy")  %>' Visible="false" />
                    <asp:Literal runat="server" ID="ltrl_Created" Text='<%# Eval("Created")  %>' Visible="false" />
                    <asp:Literal runat="server" ID="ltrl_ModifiedBy" Text='<%# Eval("ModifiedBy")  %>' Visible="false" />
                    <asp:Literal runat="server" ID="ltrl_Modified" Text='<%# Eval("Modified")  %>' Visible="false" />
                    <asp:Literal runat="server" ID="ltrl_No" Text='<%# $"{Eval("No")}" == "0" ? "New" : ($"{Eval("No")}" == "-1" ? "" : $"{Eval("No")}")  %>' />
                  </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px">
                  <HeaderTemplate><asp:CheckBox ID="cbCheckAll" runat="server" Width="16px" Height="16px" /></HeaderTemplate>
                  <ItemTemplate><asp:CheckBox ID="cb_IsChecked" runat="server" Checked='<%# (bool)Eval("IsChecked") %>' /></ItemTemplate>
                </asp:TemplateField>            
    
                <asp:TemplateField HeaderText="Cust&nbsp;No" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_CustNo" Text='<%# Eval("CustNo") %>' /></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="Name" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_Name" Text='<%# Eval("Name") %>' /></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="Address" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_Address" Text='<%# Eval("Address") %>' /></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="RT" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_RT" Text='<%# Eval("RT") %>' /></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="RW" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_RW" Text='<%# Eval("RW") %>' /></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="Kelurahan" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_Kelurahan" Text='<%# Eval("Kelurahan") %>' /></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="Kecamatan" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_Kecamatan" Text='<%# Eval("Kecamatan") %>' /></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="Phone" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_Phone" Text='<%# Eval("Phone") %>' /></ItemTemplate></asp:TemplateField>                        
                <asp:TemplateField HeaderText="Email" ><ItemTemplate><asp:Literal runat="server" ID="ltrl_Email" Text='<%# Eval("Email") %>' /></ItemTemplate></asp:TemplateField>                           
            </Columns>
            <HeaderStyle BackColor="#157fcc" ForeColor="White" />
            </asp:GridView>        
          </div></div></div></div>
          <div id="dvCode" class="form-group row" style="margin-bottom:10px;">
            <label for="tbCode" class="col-sm-2 col-form-label">Code</label>
            <div class="col-sm-10"><input runat="server" id="tbCode" type="text" readonly class="form-control-plaintext" style="padding-left:10px;" /></div>
          </div>
            
          <div id="dvLocation" style="margin-bottom:-10px">
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

          <div id="dvHelpdeskConductedByCust" style="margin-left:-12px; margin-right:-12px; margin-bottom:20px"><div class="squarebox">
            <div class="squareboxgradientcaption" style="cursor: pointer;"onclick="togglePannelAnimatedStatus(this.nextElementSibling,50,50)">
              <div style="float: left;  margin-top:3px; ">Operators</div>
              <div style="float: right;  margin-top:3px;"><img src="../../res/images/collapse.png" width="16" height="16" border="0" alt="Show/Hide" title="Show/Hide" />&nbsp;</div>
            </div>
            <div class="squareboxcontent" ><div class="wrapper-suppress">
              <div style="display:inline-block"><f:Button runat="server" ID="btnAddConductedBy" Text="Add" Icon="Add" OnClick="Fbtn_Click" /></div>
              <div style="display:inline-block"><f:Button runat="server" ID="btnDeleteConductedBy" Text="Delete" Icon="Add" OnClick="Fbtn_Click" /></div>
              <asp:GridView runat="server" ID="gvConductedBy" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover" Width="100%" HeaderStyle-HorizontalAlign="Center" OnRowDataBound="gvConductedBy_RowDataBound">
                <Columns>            
                  <asp:TemplateField HeaderText="No" HeaderStyle-Width="20px" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Right" >
                    <ItemTemplate>
                      <asp:Literal runat="server" ID="ltrl_Id" Text='<%# Eval("Id")  %>' Visible="false" />                      
                      <asp:Literal runat="server" ID="ltrl_Seq" Text='<%# Eval("Seq")  %>' />
                    </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px">
                    <HeaderTemplate><asp:CheckBox ID="cbCheckAll" runat="server" Width="16px" Height="16px" /></HeaderTemplate>
                    <ItemTemplate><asp:CheckBox ID="cb_IsChecked" runat="server" Checked='<%# (bool)Eval("IsChecked") %>' /></ItemTemplate>
                  </asp:TemplateField>            
    
                  <asp:TemplateField HeaderText="Operator Type" ><ItemTemplate>
                    <asp:DropDownList runat="server" ID="ddlOperatorTypeMultiple" class="form-select form-select-sm" AutoPostBack="true" OnSelectedIndexChanged="ddl_SelectedIndexChanged" Width="100%">                      
                      <asp:ListItem>Internal</asp:ListItem>
                      <asp:ListItem>External</asp:ListItem>
                    </asp:DropDownList>
                  </ItemTemplate></asp:TemplateField>
                  <asp:TemplateField HeaderText="Vendor" ><ItemTemplate><asp:DropDownList runat="server" ID="ddlVendorMultiple" class="form-select form-select-sm" AutoPostBack="true" OnSelectedIndexChanged="ddl_SelectedIndexChanged" Width="100%" /></ItemTemplate></asp:TemplateField>
                  <asp:TemplateField HeaderText="Operator" ><ItemTemplate><asp:DropDownList runat="server" ID="ddlOperatorMultiple" class="form-select form-select-sm" Width="100%" /></ItemTemplate></asp:TemplateField>                  
              </Columns>
              <HeaderStyle BackColor="#157fcc" ForeColor="White" />
              </asp:GridView> 
              <div class="row mb-3">
                <label class="col-sm-2 col-form-label col-form-label-sm">Customers for each operator</label>
                <div class="col-sm-10"><input runat="server" id="tbNumberCust" type="number" class="form-control form-control-sm" /></div>
              </div>
            </div></div></div></div>

          <div id="dvHelpdeskConductedBy">
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
              <label class="col-sm-2 col-form-label col-form-label-sm">Operators<span class="Req">*</span></label>
              <div class="col-sm-10">
                <asp:DropDownList runat="server" ID="ddlOperator" class="form-select form-select-sm" Width="100%" />
              </div>
            </div>
            </div>

          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Work Order Type<span class="Req">*</span></label>
            <div class="col-sm-10"><asp:DropDownList runat="server" id="ddlWorkOrderType" class="form-select form-select-sm" Width="100%" /></div>
          </div>
          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Category<span class="Req">*</span></label>
            <div class="col-sm-10"><asp:DropDownList runat="server" id="ddlCategory" class="form-select form-select-sm" Width="100%" /></div>
          </div>                    

          <div class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Status</label>
            <div class="col-sm-10">
              <asp:DropDownList runat="server" id="ddlStatus" class="form-select form-select-sm" AutoPostBack="true" OnSelectedIndexChanged="ddl_SelectedIndexChanged" Width="100%" Enabled="false" >                
                <asp:ListItem>Submitted</asp:ListItem>
                <asp:ListItem Text="On Going" Value="OnGoing"></asp:ListItem>
                <asp:ListItem>Completed</asp:ListItem>
                <asp:ListItem>Pending</asp:ListItem>
                <asp:ListItem>Cancel</asp:ListItem>
              </asp:DropDownList>
            </div>
          </div>
          
          <div id="dvCompletion" class="row mb-3">
            <label class="col-sm-2 col-form-label col-form-label-sm">Completion Date Time</label>
            <div class="col-sm-10"><input runat="server" id="tbCompletion" type="datetime-local" class="form-control form-control-sm" readonly /></div>
          </div>          

          <div class="form-group row">
            <label for="tbTargetCompletionStatus" class="col-sm-2 col-form-label">Target Completion Status</label>
            <div class="col-sm-10"><input runat="server" id="tbTargetCompletionStatus" type="text" readonly class="form-control-plaintext" style="padding-left:10px;" value="Auto Generated by System" /></div>
          </div>          

          </div></div></div></div>          
          
          <div><div class="squarebox">
          <div class="squareboxgradientcaption" style="cursor: pointer;"onclick="togglePannelAnimatedStatus(this.nextElementSibling,50,50)">
            <div style="float: left;  margin-top:3px; ">Request</div>
            <div style="float: right;  margin-top:3px;"><img src="../../res/images/collapse.png" width="16" height="16" border="0" alt="Show/Hide" title="Show/Hide" />&nbsp;</div>
          </div>
          <div class="squareboxcontent" ><div class="wrapper-suppress">
            <div class="row mb-3">
              <label class="col-sm-2 col-form-label col-form-label-sm">Request Type<span class="Req">*</span></label>
              <div class="col-sm-10"><asp:DropDownList runat="server" id="ddlRequestType" class="form-select form-select-sm" Width="100%" /></div>
            </div>
            <div class="row mb-3">
              <label class="col-sm-2 col-form-label col-form-label-sm">Request Source<span class="Req">*</span></label>
              <div class="col-sm-10"><asp:DropDownList runat="server" id="ddlRequestSource" class="form-select form-select-sm" AutoPostBack="true" OnSelectedIndexChanged="ddl_SelectedIndexChanged" Width="100%" /></div></div>
            <div class="row mb-3">
              <label class="col-sm-2 col-form-label col-form-label-sm">Requester Name<span class="Req">*</span></label>
              <div class="col-sm-10"><input runat="server" id="tbRequesterName" type="text" class="form-control form-control-sm" /></div>
            </div>          
            <div class="row mb-3">
              <label class="col-sm-2 col-form-label col-form-label-sm">Requester Email</label>
              <div class="col-sm-10"><input runat="server" id="tbRequesterEmail" type="text" class="form-control form-control-sm" /></div>
            </div>
            <div class="row mb-3">
              <label class="col-sm-2 col-form-label col-form-label-sm">Requester Phone</label>
              <div class="col-sm-10"><input runat="server" id="tbRequesterPhone" type="number" class="form-control form-control-sm" /></div>
            </div>
            
            <div id="dvSocialMedia" class="row mb-3">
              <label class="col-sm-2 col-form-label col-form-label-sm">Requester Social Media<span class="Req">*</span></label>
              <div class="col-sm-10"><asp:DropDownList runat="server" id="ddlSocialMedia" class="form-select form-select-sm" Width="100%" /></div>
            </div>           

            <div class="row mb-3">
              <label class="col-sm-2 col-form-label col-form-label-sm">Request Detail<span class="Req">*</span></label>
              <div class="col-sm-10"><textarea runat="server" id="tbRequestDetail" class="form-control form-control-sm" rows="5" /></div>
            </div>
            <div class="row mb-3">
              <label class="col-sm-2 col-form-label col-form-label-sm">Requested Date Time<span class="Req">*</span></label>
              <div class="col-sm-10"><input runat="server" id="tbRequestedDateTime" type="datetime-local" class="form-control form-control-sm" /></div>
            </div>
              
            <div class="form-group row" >
              <label for="tbResponseStatus" class="col-sm-2 col-form-label">Response Status</label>
              <div class="col-sm-10"><input runat="server" id="tbResponseStatus" type="text" readonly class="form-control-plaintext" style="padding-left:10px;" value="Auto Generated By System" /></div>
            </div>            
          </div></div></div></div>

          <div id="dvItem">
            <div class="squarebox">
              <div class="squareboxgradientcaption" style="cursor: pointer;" onclick="togglePannelAnimatedStatus(this.nextElementSibling,50,50)">
                <div style="float: left; margin-top: 3px;">Work Order Item</div>
                <div style="float: right; margin-top: 3px;">
                  <img src="../../res/images/collapse.png" width="16" height="16" border="0" alt="Show/Hide" title="Show/Hide" />&nbsp;
                </div>
              </div>
              <div class="squareboxcontent">
                <div class="wrapper-suppress">
                  <div id="dvModifiedItem" class="row mb-3">
                    <div class="col-sm-10">
                      <div style="display: inline-block"><f:Button runat="server" ID="btnAddItem" Text="Add" Icon="Add" OnClick="Fbtn_Click" /></div>
                      <div style="display: inline-block"><f:Button runat="server" ID="btnDeleteItem" Text="Delete" Icon="Delete" OnClick="Fbtn_Click" /></div>
                    </div>
                  </div>
                  <asp:GridView runat="server" ID="gvItems" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover" Width="100%" HeaderStyle-HorizontalAlign="Center" OnRowDataBound="gvItems_RowDataBound">
                    <Columns>
                      <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px">
                        <HeaderTemplate>
                          <asp:CheckBox ID="cbCheckAll" runat="server" Width="16px" Height="16px" />
                        </HeaderTemplate>
                        <ItemTemplate>
                          <asp:CheckBox ID="cb_IsChecked" runat="server" Checked='<%# (bool)Eval("IsChecked") %>' />
                        </ItemTemplate>
                      </asp:TemplateField>
                      <asp:TemplateField>
                        <HeaderTemplate>Item Code<span class="Req">*</span></HeaderTemplate>
                        <ItemTemplate>
                          <asp:Literal runat="server" ID="ltrl_Id" Text='<%# Eval("Id") %>' Visible="false" />
                          <asp:Literal runat="server" ID="ltrl_Seq" Text='<%# Eval("Id") %>' Visible="false" />
                          <asp:Literal runat="server" ID="ltrl_ItemId" Text='<%# Eval("ItemId") %>' Visible="false" />
                          <asp:Literal runat="server" ID="ltrlItemCode" Text='<%# Eval("ItemCode") %>' Visible="false" />
                          <div runat="server" id="dvCode" style="display: inline-block; width: 80%;">
                            <asp:TextBox runat="server" ID="tb_ItemCode" Text='<%# Eval("ItemCode") %>' CssClass="form-control form-control-sm" ReadOnly="true" />
                          </div>
                          <div runat="server" id="dvCodeLookup" style="display: inline-block">
                            <asp:Button runat="server" ID="btnItemLookup" Text="..." CssClass="btn btn-primary btn-sm" />
                          </div>
                        </ItemTemplate>
                      </asp:TemplateField>
                      <asp:TemplateField HeaderText="Name" ItemStyle-VerticalAlign="Middle">
                        <ItemTemplate>
                          <asp:Literal runat="server" ID="ltrl_ItemName" Text='<%# Eval("ItemName") %>' />
                        </ItemTemplate>
                      </asp:TemplateField>
                      <asp:TemplateField>
                        <HeaderTemplate>Qty<span class="Req">*</span></HeaderTemplate>
                        <ItemTemplate>
                          <asp:Literal runat="server" ID="ltrlQty" Text='<%# Eval("Qty") %>' Visible="false" />
                          <asp:TextBox runat="server" ID="tb_Qty" CssClass="form-control form-control-sm" Text='<%# Eval("Qty") %>' type="number" />
                        </ItemTemplate>
                      </asp:TemplateField>
                    </Columns>
                    <HeaderStyle BackColor="#157fcc" ForeColor="White" />
                  </asp:GridView>
                </div>
              </div>
            </div>
          </div>

                                                
          <div><div class="squarebox">
          <div class="squareboxgradientcaption" style="cursor: pointer;"onclick="togglePannelAnimatedStatus(this.nextElementSibling,50,50)">
            <div style="float: left;  margin-top:3px; ">Attachment</div>
            <div style="float: right;  margin-top:3px;"><img src="../../res/images/collapse.png" width="16" height="16" border="0" alt="Show/Hide" title="Show/Hide" />&nbsp;</div>
          </div>
          <div class="squareboxcontent" ><div class="wrapper-suppress">

          <div class="row mb-3"><div class="col-sm-10">            
            <div style="display:inline-block;"><asp:FileUpload runat="server" ID="fu" class="form-control form-control-sm" style="width:300px;display:inline-block"  /></div>
            <div style="display:inline-block"><f:Button runat="server" ID="btnDeleteAttachment" Text="Delete Attachment" Icon="Delete" OnClick="Fbtn_Click" /></div>
          </div></div>
            
          <asp:GridView runat="server" ID="gvAttachment" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover" Width="100%" HeaderStyle-HorizontalAlign="Center" OnRowDataBound="gvAttachment_RowDataBound">
            <Columns>
              <asp:TemplateField ItemStyle-HorizontalAlign="Right" HeaderText="No" ItemStyle-BorderStyle="None" ItemStyle-Width="25px"><ItemTemplate>                                                                                                                       
                <asp:Literal runat="server" ID="ltrl_Seq" Text='<%# $"{Eval("Seq")}" %>' Visible='<%# $"{Eval("Seq")}" == "0" ? false : true %>' />                    
              </ItemTemplate></asp:TemplateField>
              <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px">
                <HeaderTemplate><asp:CheckBox ID="cbCheckAll" runat="server" Width="16px" Height="16px" /></HeaderTemplate>
                <ItemTemplate><asp:CheckBox ID="cb_IsChecked" runat="server" Checked='<%# (bool)Eval("IsChecked") %>' /></ItemTemplate>              
              </asp:TemplateField>
              <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderText="File Name" ItemStyle-BorderStyle="None"><ItemTemplate>
                <asp:Literal runat="server" ID="ltrl_Id" Text='<%# Eval("ID") %>' Visible="false"  />                
                <asp:Literal runat="server" ID="ltrl_OwnerID" Text='<%# Eval("OwnerID") %>' Visible="false"  /> 
                <asp:Literal runat="server" ID="ltrl_FileNameUniq" Text='<%# Eval("FileNameUniq") %>' Visible="false"  /> 
                <asp:LinkButton runat="server" ID="lb_FileName" Text='<%# Eval("FileName") %>' />
                <asp:Button runat="server" ID="btnDownload" Hidden="true" OnClick="btn_Click" EnableAjax="false" />
              </ItemTemplate></asp:TemplateField>              
            </Columns>
            <HeaderStyle BackColor="#157fcc" ForeColor="White" />
          </asp:GridView>
          
          </div>
          </div>
          </div></div>

        </div></Content>
      </f:ContentPanel>
    </Items>
  </f:Panel>
  <f:Button runat="server" ID="btnRequestedChanged" OnClick="Fbtn_Click" Hidden="true" />
  <f:Button runat="server" ID="btnUpload" OnClick="Fbtn_Click" Hidden="true" />  
  <f:Window ID="wCust" Title="Select Customer" Hidden="true" EnableIFrame="true" runat="server" EnableMaximize="true" EnableResize="true" Target="Top" IsModal="false" Width="830px" Height="500px" OnClose="wCust_Close"></f:Window>  
  <f:Window ID="wLocation" Title="Select Location" Hidden="true" EnableIFrame="true" runat="server" EnableMaximize="true" EnableResize="true" Target="Top" IsModal="false" Width="830px" Height="500px" OnClose="wLocation_Close"></f:Window>
  <f:Window ID="wItem" Title="Select Item" Hidden="true" EnableIFrame="true" runat="server" EnableMaximize="true" EnableResize="true" Target="Top" IsModal="false" Width="830px" Height="500px" OnClose="wItem_Close"></f:Window>
</form>
<script>        
    function getOrderWindowUrl(id) {
        return F.baseUrl + 'Pages/WorkOrder/WorkOrder.aspx?HelpdeskId=' + id + '&parenttabid=' + parent.getActiveTabId();
    }
    function updatePage(param1) {
        __doPostBack('', 'UpdatePage$' + param1);
    }
</script>
</body>
</html>
