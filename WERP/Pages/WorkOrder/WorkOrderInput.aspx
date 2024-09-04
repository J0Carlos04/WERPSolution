<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WorkOrderInput.aspx.cs" Inherits="Pages_WorkOrder_WorkOrderInput" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title>WERP</title>
  <link href="../../res/css/bootstrap.min.css" rel="stylesheet" />
  <link href="../../res/css/Collapsible.css" rel="stylesheet" />
  <script lang="javascript" type="text/javascript" src="../../res/js/JScript.js"></script>

  <script src="../../res/js/jquery.slim.min.js"></script>
  <script src="../../res/js/select2.full.min.js"></script>
  <link rel="stylesheet" href="../../res/css/select2.min.css" />
  <link rel="stylesheet" href="../../res/css/Select2-bootstrap-5-theme.min.css" />
</head>
<body>
  <form id="form1" runat="server">
    <f:PageManager ID="pm" AutoSizePanelID="pContent" runat="server" Language="EN" AjaxAspnetControls="ddlOperatorType,ddlVendor,ddlOperator,gvItems
    ,tbStartDate,tbWorkDuration,tbTargetCompletion,ddlStatus,gvWorkUpdate,ddlOperatorType,ddlVendor,gvWOAttachment" />
    <f:Panel ID="pContent" runat="server" ShowBorder="false" ShowHeader="false" Layout="Fit">
      <Toolbars>
        <f:Toolbar runat="server" ID="tlb">
          <Items>
            <f:Button runat="server" ID="btnSubmit" Text="Submit" Icon="DatabaseSave" OnClick="Fbtn_Click" />
            <f:Button runat="server" ID="btnSubmitAchievement" Text="Submit Achievement" Icon="DatabaseSave" OnClick="Fbtn_Click" Hidden="true" />
            <f:Button runat="server" ID="btnCancel" Text="Cancel" Icon="Cancel" EnablePostBack="false" OnClientClick="parent.removeActiveTab();" />
          </Items>
        </f:Toolbar>
      </Toolbars>
      <Items>
        <f:ContentPanel runat="server" ID="pData" ShowBorder="true" AutoScroll="true">
          <Content>
            <div class="wrapper-suppress">
              <asp:Panel runat="server" ID="pnlContent">

                <div>
                  <div class="squarebox">
                    <div class="squareboxgradientcaption" style="cursor: pointer;" onclick="togglePannelAnimatedStatus(this.nextElementSibling,50,50)">
                      <div style="float: left; margin-top: 3px;">
                        <asp:Label runat="server" ID="lblParentTitle" />
                      </div>
                      <div style="float: right; margin-top: 3px;">
                        <img src="../../res/images/collapse.png" width="16" height="16" border="0" alt="Show/Hide" title="Show/Hide" />&nbsp;
                      </div>
                    </div>
                    <div class="squareboxcontent" id="sbcParent">
                      <div class="wrapper-suppress">
                         <div class="form-group row" style="margin-bottom: 10px;">
                          <label for="tbLocation" class="col-sm-2 col-form-label">Code</label>
                          <div class="col-sm-10">
                            <input runat="server" id="tbHelpdeskCode" type="text" readonly class="form-control-plaintext" style="padding-left: 10px;" />
                          </div>
                        </div>
                        <div class="form-group row" style="margin-bottom: 10px;">
                          <label for="tbArea" class="col-sm-2 col-form-label">Area</label>
                          <div class="col-sm-10">
                            <input runat="server" id="tbArea" type="text" readonly class="form-control-plaintext" style="padding-left: 10px;" />
                          </div>
                        </div>                       
                        <div class="form-group row" style="margin-bottom: 10px;">
                          <label for="tbLocation" class="col-sm-2 col-form-label">Location</label>
                          <div class="col-sm-10">
                            <input runat="server" id="tbLocation" type="text" readonly class="form-control-plaintext" style="padding-left: 10px;" />
                          </div>
                        </div>
                        <div class="form-group row" style="margin-bottom: 10px;">
                          <label for="tbCategory" class="col-sm-2 col-form-label">Order Category</label>
                          <div class="col-sm-10">
                            <input runat="server" id="tbCategory" type="text" readonly class="form-control-plaintext" style="padding-left: 10px;" />
                          </div>
                        </div>
                        <div class="form-group row" style="margin-bottom: 10px;">
                          <label for="tbSubject" class="col-sm-2 col-form-label">Subject</label>
                          <div class="col-sm-10">
                            <input runat="server" id="tbSubject" type="text" readonly class="form-control-plaintext" style="padding-left: 10px;" />
                          </div>
                        </div>

                        <div id="dvCustomerWorkOrder">
                        <div class="form-group row">
                          <label for="tbCustomerNo" class="col-sm-2 col-form-label">Customer No</label>
                          <div class="col-sm-10">
                            <input runat="server" id="tbCustomerNo" type="text" readonly class="form-control-plaintext" style="padding-left: 10px;" />
                          </div>
                        </div>
                        <div class="form-group row">
                          <label for="tbCustomer" class="col-sm-2 col-form-label">Customer</label>
                          <div class="col-sm-10">
                            <input runat="server" id="tbCustomer" type="text" readonly class="form-control-plaintext" style="padding-left: 10px;" />
                          </div>
                        </div>
                        </div>

                        <div class="form-group row" style="margin-bottom: 10px;">
                          <label for="tbWorkDescription" class="col-sm-2 col-form-label">
                            <asp:Literal runat="server" ID="ltrlDescTitle" Text="Work Description / Request Detail" /></label>
                          <div class="col-sm-10">
                            <textarea runat="server" id="tbWorkDescription" type="text" readonly class="form-control-plaintext" style="padding-left: 10px;" />
                          </div>
                        </div>                        

                        <div id="dvScheduler">
                          <fieldset class="border rounded-3 p-3">
                            <legend class="float-none w-auto px-3 col-sm-2 col-form-label col-form-label-sm">Schedule</legend>
                            <asp:GridView runat="server" ID="gvSchedule" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover" Width="100%" HeaderStyle-HorizontalAlign="Center">
                              <Columns>
                                <asp:TemplateField HeaderText="Start Date">
                                  <ItemTemplate>
                                    <asp:Literal runat="server" ID="ltrl_Id" Text='<%# Eval("Id") %>' Visible="false" />
                                    <asp:Literal runat="server" ID="ltrlStartDate" Text='<%# $"{Eval("StartDate")}" == "1/1/0001 12:00:00 AM" ? "" : $"{Eval("StartDate", "{0: dd-MMM-yyyy}")}" %>' />
                                  </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Period" ItemStyle-Width="160">
                                  <ItemTemplate>
                                    <asp:Literal runat="server" ID="ltrlPeriod" Text='<%# $"{Eval("Period")} {Eval("PeriodType")}" %>' />
                                  </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="End Date">
                                  <ItemTemplate>
                                    <asp:Literal runat="server" ID="ltrlEndDate" Text='<%# $"{Eval("EndDate")}" == "1/1/0001 12:00:00 AM" ? "" : $"{Eval("EndDate", "{0: dd-MMM-yyyy}")}" %>' />
                                  </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Order Date">
                                  <ItemTemplate>
                                    <asp:Literal runat="server" ID="ltrlOrderDate" Text='<%# $"{Eval("OrderDate")}" == "1/1/0001 12:00:00 AM" ? "" : $"{Eval("OrderDate", "{0: dd-MMM-yyyy}")}" %>' />
                                  </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Earliest Start Date">
                                  <ItemTemplate>
                                    <asp:Literal runat="server" ID="ltrlEarliestStartDate" Text='<%# $"{Eval("EarliestStartDate")}" == "1/1/0001 12:00:00 AM" ? "" : $"{Eval("EarliestStartDate", "{0: dd-MMM-yyyy}")}" %>' />
                                  </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Lastest Start Date">
                                  <ItemTemplate>
                                    <asp:Literal runat="server" ID="ltrlLatestStartDate" Text='<%# $"{Eval("LatestStartDate")}" == "1/1/0001 12:00:00 AM" ? "" : $"{Eval("LatestStartDate", "{0: dd-MMM-yyyy}")}" %>' />
                                  </ItemTemplate>
                                </asp:TemplateField>
                              </Columns>
                            </asp:GridView>
                          </fieldset>
                        </div>

                        <fieldset class="border rounded-3 p-3">
                          <legend class="float-none w-auto px-3 col-sm-2 col-form-label col-form-label-sm">Attachment</legend>
                          <asp:GridView runat="server" ID="gvScheduleAttachment" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover" Width="100%" HeaderStyle-HorizontalAlign="Center" OnRowDataBound="gvAttachment_RowDataBound">
                            <Columns>
                              <asp:TemplateField ItemStyle-HorizontalAlign="Right" HeaderText="No" ItemStyle-BorderStyle="None" ItemStyle-Width="25px">
                                <ItemTemplate>
                                  <asp:Literal runat="server" ID="ltrl_Seq" Text='<%# $"{Eval("Seq")}" %>' Visible='<%# $"{Eval("Seq")}" == "0" ? false : true %>' />
                                </ItemTemplate>
                              </asp:TemplateField>
                              <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderText="File Name" ItemStyle-BorderStyle="None">
                                <ItemTemplate>
                                  <asp:Literal runat="server" ID="ltrl_Id" Text='<%# Eval("ID") %>' Visible="false" />
                                  <asp:Literal runat="server" ID="ltrl_OwnerID" Text='<%# Eval("OwnerID") %>' Visible="false" />
                                  <asp:Literal runat="server" ID="ltrl_FileNameUniq" Text='<%# Eval("FileNameUniq") %>' Visible="false" />
                                  <asp:LinkButton runat="server" ID="lb_FileName" Text='<%# Eval("FileName") %>' />
                                  <asp:Button runat="server" ID="btnDownload" Hidden="true" OnClick="btn_Click" EnableAjax="false" />
                                </ItemTemplate>
                              </asp:TemplateField>
                            </Columns>
                            <HeaderStyle BackColor="#157fcc" ForeColor="White" />
                          </asp:GridView>
                        </fieldset>

                      </div>
                    </div>
                  </div>
                </div>

                <div>
                  <div class="squarebox">
                    <div class="squareboxgradientcaption" style="cursor: pointer;" onclick="togglePannelAnimatedStatus(this.nextElementSibling,50,50)">
                      <div style="float: left; margin-top: 3px;">Work Order</div>
                      <div style="float: right; margin-top: 3px;">
                        <img src="../../res/images/collapse.png" width="16" height="16" border="0" alt="Show/Hide" title="Show/Hide" />&nbsp;
                      </div>
                    </div>
                    <div class="squareboxcontent" id="sbcWorkOrder">
                      <div class="wrapper-suppress">

                        <div class="form-group row" style="margin-bottom: 10px;">
                          <label for="tbCode" class="col-sm-2 col-form-label">Code</label>
                          <div class="col-sm-10">
                            <input runat="server" id="tbCode" type="text" readonly class="form-control-plaintext" style="padding-left: 10px;" />
                          </div>
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

                        <div class="row mb-3">
                          <label class="col-sm-2 col-form-label col-form-label-sm">Status<span class="Req">*</span></label>
                          <div class="col-sm-10">
                            <asp:DropDownList runat="server" ID="ddlStatus" class="form-select form-select-sm" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddl_SelectedIndexChanged">
                              <asp:ListItem>Assignment</asp:ListItem>
                              <asp:ListItem>Preparation</asp:ListItem>
                              <asp:ListItem>StockOut</asp:ListItem>
                              <asp:ListItem>Started</asp:ListItem>
                              <asp:ListItem>Inprogress</asp:ListItem>
                              <asp:ListItem>Completed</asp:ListItem>
                              <asp:ListItem>Pending</asp:ListItem>
                              <asp:ListItem>Cancel</asp:ListItem>
                            </asp:DropDownList>
                          </div>
                        </div>

                        <div id="dvProgress">
                          <div class="row mb-3">
                            <label class="col-sm-2 col-form-label col-form-label-sm">Start Date<span class="Req">*</span></label>
                            <div class="col-sm-10">
                              <input runat="server" id="tbStartDate" type="datetime-local" class="form-control form-control-sm" />
                            </div>
                          </div>
                          <div class="row mb-3">
                            <label class="col-sm-2 col-form-label col-form-label-sm">Work Duration<span class="Req">*</span></label>
                            <div class="col-sm-10">
                              <input runat="server" id="tbWorkDuration" type="number" class="form-control form-control-sm" />
                            </div>
                          </div>
                          <div class="row mb-3">
                            <label class="col-sm-2 col-form-label col-form-label-sm">Work Duration Type<span class="Req">*</span></label>
                            <div class="col-sm-10">
                              <asp:DropDownList runat="server" ID="ddlWorkDurationType" class="form-select form-select-sm" Width="100%">
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem Text="Hours" Value="Hour"></asp:ListItem>
                                <asp:ListItem Text="Days" Value="Day"></asp:ListItem>
                                <asp:ListItem Text="Months" Value="Month"></asp:ListItem>
                                <asp:ListItem Text="Years" Value="Year"></asp:ListItem>
                              </asp:DropDownList>
                            </div>
                          </div>
                          <div class="form-group row" style="margin-bottom: 10px;">
                            <label for="tbTargetCompletion" class="col-sm-2 col-form-label">Target Completion</label>
                            <div class="col-sm-10"><input runat="server" id="tbTargetCompletion" type="text" readonly class="form-control-plaintext" style="padding-left: 10px;" value="Auto Generated by System" /></div>
                          </div>
                        </div>
                        <div id="dvCompletion">
                          <div class="row mb-3">
                            <label class="col-sm-2 col-form-label col-form-label-sm">Closed Date<span class="Req">*</span></label>
                            <div class="col-sm-10"><input runat="server" id="tbCloseDate" type="datetime-local" class="form-control form-control-sm" /></div>
                          </div>

                          <div id="dvMeterCondition">
                            
                            <div class="row mb-3">
                              <label class="col-sm-2 col-form-label col-form-label-sm">Seal Meter Condition<span class="Req">*</span></label>
                              <div class="col-sm-10">
                                <asp:DropDownList runat="server" ID="ddlSealMeterCondition" class="form-select form-select-sm" Width="100%">
                                  <asp:ListItem></asp:ListItem>
                                  <asp:ListItem>Available</asp:ListItem>
                                  <asp:ListItem>Not Available</asp:ListItem>
                                  <asp:ListItem>Broken</asp:ListItem>
                                </asp:DropDownList>
                              </div>
                            </div>

                            <div class="row mb-3">
                              <label class="col-sm-2 col-form-label col-form-label-sm">Body Meter Condition<span class="Req">*</span></label>
                              <div class="col-sm-10">
                                <asp:DropDownList runat="server" ID="ddlBodyMeterCondition" class="form-select form-select-sm" Width="100%">
                                  <asp:ListItem></asp:ListItem>
                                  <asp:ListItem>Good</asp:ListItem>
                                  <asp:ListItem>Broken</asp:ListItem>
                                </asp:DropDownList>
                              </div>
                            </div>

                            <div class="row mb-3">
                              <label class="col-sm-2 col-form-label col-form-label-sm">Cover Meter Condition<span class="Req">*</span></label>
                              <div class="col-sm-10">
                                <asp:DropDownList runat="server" ID="ddlCoverMeterCondition" class="form-select form-select-sm" Width="100%">
                                  <asp:ListItem></asp:ListItem>
                                  <asp:ListItem>Available</asp:ListItem>
                                  <asp:ListItem>Not Available</asp:ListItem>
                                  <asp:ListItem>Broken</asp:ListItem>
                                </asp:DropDownList>
                              </div>
                            </div>

                            <div class="row mb-3">
                              <label class="col-sm-2 col-form-label col-form-label-sm">Glass Meter Condition<span class="Req">*</span></label>
                              <div class="col-sm-10">
                                <asp:DropDownList runat="server" ID="ddlGlassMeterCondition" class="form-select form-select-sm" Width="100%">
                                  <asp:ListItem></asp:ListItem>
                                  <asp:ListItem>Good</asp:ListItem>
                                  <asp:ListItem>Blur</asp:ListItem>
                                  <asp:ListItem>Cracked</asp:ListItem>
                                </asp:DropDownList>
                              </div>
                            </div>
                            <div class="row mb-3">
                              <label class="col-sm-2 col-form-label col-form-label-sm">Machine Meter Condition<span class="Req">*</span></label>
                              <div class="col-sm-10">
                                <asp:DropDownList runat="server" ID="ddlMachiveMeterCondition" class="form-select form-select-sm" Width="100%">
                                  <asp:ListItem></asp:ListItem>
                                  <asp:ListItem>Good</asp:ListItem>
                                  <asp:ListItem>Off</asp:ListItem>
                                  <asp:ListItem>Broken</asp:ListItem>
                                </asp:DropDownList>
                              </div>
                            </div>
                          </div>

                          <div class="row mb-3">
                            <label class="col-sm-2 col-form-label col-form-label-sm">Result</label>
                            <div class="col-sm-10">
                              <asp:DropDownList runat="server" ID="ddlResult" class="form-select form-select-sm" Width="100%">
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem>Good</asp:ListItem>
                                <asp:ListItem Text="Good with Note" Value="GoodNote"></asp:ListItem>
                                <asp:ListItem>Finding</asp:ListItem>
                              </asp:DropDownList>
                            </div>
                          </div>
                          <div class="row mb-3">
                            <label class="col-sm-2 col-form-label col-form-label-sm">Remarks</label>
                            <div class="col-sm-10"><textarea runat="server" id="tbRemarks" class="form-control form-control-sm" rows="5" /> </div>
                          </div>
                          <div class="form-group row" style="margin-bottom: 10px;">
                            <label for="tbActualAchievement" class="col-sm-2 col-form-label">Actual Achievement</label>
                            <div class="col-sm-10">
                              <input runat="server" id="tbActualAchievement" type="text" readonly class="form-control-plaintext" style="padding-left: 10px;" value="Auto Generated by System" />
                            </div>
                          </div>
                          <asp:Panel runat="server" ID="pnlAchievement" Visible="false">
                            <div class="row mb-3">
                              <label class="col-sm-2 col-form-label col-form-label-sm">Achievement</label>
                              <div class="col-sm-10">
                                <asp:DropDownList runat="server" ID="ddlAchievement" class="form-select form-select-sm" Width="100%">
                                  <asp:ListItem></asp:ListItem>
                                  <asp:ListItem>Achieved</asp:ListItem>
                                  <asp:ListItem>Not Achieved</asp:ListItem>
                                </asp:DropDownList>
                              </div>
                            </div>
                          </asp:Panel>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>

                <div id="dvWorkUpdate">
                  <div class="squarebox">
                    <div class="squareboxgradientcaption" style="cursor: pointer;" onclick="togglePannelAnimatedStatus(this.nextElementSibling,50,50)">
                      <div style="float: left; margin-top: 3px;">Work Update</div>
                      <div style="float: right; margin-top: 3px;">
                        <img src="../../res/images/collapse.png" width="16" height="16" border="0" alt="Show/Hide" title="Show/Hide" />&nbsp;
                      </div>
                    </div>
                    <div class="squareboxcontent">
                      <div class="wrapper-suppress">
                        <div class="row mb-3">
                          <div class="col-sm-10">
                            <div style="display: inline-block"><f:Button runat="server" ID="btnAddWorkUpdate" Text="Add" Icon="Add" OnClick="Fbtn_Click" /></div>
                            <div style="display: inline-block"><f:Button runat="server" ID="btnDeleteWorkUpdate" Text="Delete" Icon="Delete" OnClick="Fbtn_Click" /></div>
                          </div>
                        </div>
                        <asp:GridView runat="server" ID="gvWorkUpdate" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover" Width="100%" HeaderStyle-HorizontalAlign="Center" OnRowDataBound="gvWorkUpdate_RowDataBound">
                          <Columns>
                            <asp:TemplateField HeaderText="No" ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="50">
                              <ItemTemplate>
                                <asp:Literal runat="server" ID="ltrl_Id" Text='<%# Eval("Id") %>' Visible="false" />
                                <asp:Literal runat="server" ID="ltrl_WorkOrderId" Text='<%# Eval("WorkOrderId") %>' Visible="false" />
                                <asp:Literal runat="server" ID="ltrl_WorkOrderReferenceId" Text='<%# Eval("WorkOrderReferenceId") %>' Visible="false" />
                                <asp:Literal runat="server" ID="ltrl_StockOutId" Text='<%# Eval("StockOutId") %>' Visible="false" />
                                <asp:Literal runat="server" ID="ltrl_StockOutReturId" Text='<%# Eval("StockOutReturId") %>' Visible="false" />
                                <asp:Literal runat="server" ID="ltrl_No" Text='<%# Eval("No") %>' />
                              </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px">
                              <HeaderTemplate><asp:CheckBox ID="cbCheckAll" runat="server" Width="16px" Height="16px" /></HeaderTemplate>
                              <ItemTemplate><asp:CheckBox ID="cb_IsChecked" runat="server" Checked='<%# (bool)Eval("IsChecked") %>' /></ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Date & Work Type" ItemStyle-VerticalAlign="Middle" HeaderStyle-Width="180px">
                              <ItemTemplate>
                                <div style="width: 200px;"><input runat="server" id="tbDate" type="datetime-local" class="form-control form-control-sm" /></div>
                                <asp:DropDownList runat="server" ID="ddl_WorkType" class="form-select form-select-sm" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddl_SelectedIndexChanged">
                                  <asp:ListItem Text="Non-Inventory" Value="NonInventory"></asp:ListItem>
                                  <asp:ListItem>Installation</asp:ListItem>
                                  <asp:ListItem>Disposal</asp:ListItem>
                                  <asp:ListItem>Retur</asp:ListItem>
                                </asp:DropDownList>
                                <div runat="server" id="dvWO" class="col-sm-10" style="width: 100%;" visible='<%# Eval("WorkType").Is(CNT.Retur) %>'>
                                  <div class="input-group mb-3" style="width: 100%;">
                                    <asp:TextBox runat="server" ID="tb_WoCode" class="form-control form-control-sm" placeholder="Select Work Order" aria-label="select Purchase Order" aria-describedby="basic-addon2" ReadOnly="true" Text='<%# Eval("WoCode") %>' />
                                    <div class="input-group-append"><asp:Button runat="server" ID="btnLookupWO" Text="Work Order" CssClass="btn btn-primary btn-sm" /></div>
                                  </div>
                                </div>
                              </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Work Detail" ItemStyle-VerticalAlign="Middle">
                              <ItemTemplate><asp:TextBox runat="server" ID="tbWorkDetail" class="form-control form-control-sm" TextMode="MultiLine" Rows="5" Width="100%" /></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-VerticalAlign="Middle">
                              <HeaderTemplate>Items<span class="Req">*</span></HeaderTemplate>
                              <ItemTemplate>
                                <div runat="server" id="dvItemWO"><div>
                                  Add&nbsp;Item
                                  <asp:Button runat="server" ID="btnCodeLookup" Text="..." CssClass="btn btn-primary btn-sm" OnClick="btn_Click" />
                                </div>
                                <div>
                                  <asp:GridView runat="server" ID="gvUsedItem" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover" Width="100%" HeaderStyle-HorizontalAlign="Center" OnRowDataBound="gvUsedItem_RowDataBound">
                                    <Columns>
                                      <asp:TemplateField HeaderText="No" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                          <asp:Literal runat="server" ID="ltrl_Id" Text='<%# Eval("Id") %>' Visible="false" />
                                          <asp:Literal runat="server" ID="ltrl_ReferenceId" Text='<%# Eval("ReferenceId") %>' Visible="false" />
                                          <asp:Literal runat="server" ID="ltrl_ItemId" Text='<%# Eval("ItemId") %>' Visible="false" />
                                          <asp:Literal runat="server" ID="ltrl_StockOutItemId" Text='<%# Eval("StockOutItemId") %>' Visible="false" />
                                          <asp:Literal runat="server" ID="ltrl_No" Text='<%# Eval("No") %>' />
                                        </ItemTemplate>
                                      </asp:TemplateField>
                                      <asp:TemplateField HeaderText="ItemCode">
                                        <ItemTemplate><asp:Literal runat="server" ID="ltrl_ItemCode" Text='<%# Eval("ItemCode") %>' /></ItemTemplate>
                                      </asp:TemplateField>
                                      <asp:TemplateField HeaderText="ItemName">
                                        <ItemTemplate><asp:Literal runat="server" ID="ltrl_ItemName" Text='<%# Eval("ItemName") %>' /></ItemTemplate>
                                      </asp:TemplateField>
                                      <asp:TemplateField HeaderText="Qty" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate><asp:Literal runat="server" ID="ltrl_Qty" Text='<%# Eval("Qty") %>' /></ItemTemplate>
                                      </asp:TemplateField>
                                      <asp:TemplateField HeaderText="Retur&nbsp;Qty" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate><asp:Literal runat="server" ID="ltrl_ReturQty" Text='<%# Eval("ReturQty") %>' /></ItemTemplate>
                                      </asp:TemplateField>
                                      <asp:TemplateField HeaderText="Disposal&nbsp;Qty" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate><asp:Literal runat="server" ID="ltrl_DisposalQty" Text='<%# Eval("DisposalQty") %>' /></ItemTemplate>
                                      </asp:TemplateField>
                                      <asp:TemplateField HeaderText="SKU" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate><asp:Literal runat="server" ID="ltrl_SKU" Text='<%# Eval("SKU") %>' /></ItemTemplate>
                                      </asp:TemplateField>
                                      <asp:TemplateField HeaderText="Del" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="25px">
                                        <ItemTemplate><asp:ImageButton runat="server" ID="imbDelWorkUpdateItem" OnClick="imb_Click" ImageUrl="~/res/images/btnDelete.png" Style="line-height: 0px;" /></ItemTemplate>
                                      </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle BackColor="#157fcc" ForeColor="White" />
                                  </asp:GridView>
                                </div></div>
                              </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-VerticalAlign="Middle" HeaderText="Photo">
                              <ItemTemplate>
                                <div><asp:FileUpload runat="server" ID="fuWorkUpdate" class="form-control form-control-sm" Style="width: 200px; display: inline-block" accept=".jpg, .jpeg, .png, .gif, .bmp" /></div>
                                <div style="display: none;"><asp:Button runat="server" ID="btnUploadWorkUpdate" OnClick="btn_Click" /></div>
                                <div>
                                  <asp:GridView runat="server" ID="gvAttachmentWorkUpdate" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover" Width="100%" HeaderStyle-HorizontalAlign="Center" OnRowDataBound="gvAttachment_RowDataBound">
                                    <Columns>
                                      <asp:TemplateField ItemStyle-HorizontalAlign="Right" HeaderText="No" ItemStyle-BorderStyle="None" ItemStyle-Width="25px">
                                        <ItemTemplate>
                                          <asp:Literal runat="server" ID="ltrl_Seq" Text='<%# $"{Eval("Seq")}" %>' Visible='<%# $"{Eval("Seq")}" == "0" ? false : true %>' />
                                        </ItemTemplate>
                                      </asp:TemplateField>
                                      <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderText="File Name" ItemStyle-BorderStyle="None">
                                        <ItemTemplate>
                                          <asp:Literal runat="server" ID="ltrl_Id" Text='<%# Eval("ID") %>' Visible="false" />
                                          <asp:Literal runat="server" ID="ltrl_OwnerID" Text='<%# Eval("OwnerID") %>' Visible="false" />
                                          <asp:Literal runat="server" ID="ltrl_FileNameUniq" Text='<%# Eval("FileNameUniq") %>' Visible="false" />
                                          
                                          <asp:HiddenField runat="server" ID="hf_Latitude" Value='<%# Eval("Latitude") %>' />
                                          <asp:HiddenField runat="server" ID="hf_Longitude" Value='<%# Eval("Longitude") %>' />
                                          <div>Latitude&nbsp;:&nbsp;<asp:Label runat="server" ID="lblLatitude" Text='<%# Eval("Latitude") %>' /></div>
                                          <div>Longitude&nbsp;:&nbsp;<asp:Label runat="server" ID="lblLongitude" Text='<%# Eval("Longitude") %>' /></div>                                                                                       
                                          <div><asp:LinkButton runat="server" ID="lb_FileName" Text='<%# Eval("FileName") %>' /></div>
                                          
                                          <asp:Button runat="server" ID="btnDownload" Hidden="true" OnClick="btn_Click" EnableAjax="false" />
                                        </ItemTemplate>
                                      </asp:TemplateField>
                                      <asp:TemplateField HeaderText="Del" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="25px">
                                        <ItemTemplate>
                                          <asp:ImageButton runat="server" ID="imbDelAttachWorkUpdate" OnClick="imb_Click" ImageUrl="~/res/images/btnDelete.png" Style="line-height: 0px;" Visible='<%# $"{Eval("Seq")}" == "0" || $"{Eval("Mode")}" != "" ? false : true %>' />
                                        </ItemTemplate>
                                      </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle BackColor="#157fcc" ForeColor="White" />
                                  </asp:GridView>
                                </div>
                              </ItemTemplate>
                            </asp:TemplateField>
                          </Columns>
                          <HeaderStyle BackColor="#157fcc" ForeColor="White" />
                        </asp:GridView>
                      </div>
                    </div>
                  </div>
                </div>

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
                            <div style="display: inline-block">
                              <f:Button runat="server" ID="btnAddItem" Text="Add" Icon="Add" OnClick="Fbtn_Click" />
                            </div>
                            <div style="display: inline-block">
                              <f:Button runat="server" ID="btnDeleteItem" Text="Delete" Icon="Delete" OnClick="Fbtn_Click" />
                            </div>
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

                <div id="dvAttachment">
                  <div class="squarebox">
                    <div class="squareboxgradientcaption" style="cursor: pointer;" onclick="togglePannelAnimatedStatus(this.nextElementSibling,50,50)">
                      <div style="float: left; margin-top: 3px;">Work Order Attachment</div>
                      <div style="float: right; margin-top: 3px;">
                        <img src="../../res/images/collapse.png" width="16" height="16" border="0" alt="Show/Hide" title="Show/Hide" />&nbsp;
                      </div>
                    </div>
                    <div class="squareboxcontent" id="sbcWorkOrderAttachment">
                      <div class="wrapper-suppress">
                        <div class="row mb-3">
                          <div class="col-sm-10">
                            <asp:FileUpload runat="server" ID="fuWorkOrder" class="form-control form-control-sm" Style="width: 200px; display: inline-block" />
                          </div>
                        </div>
                        <asp:GridView runat="server" ID="gvWOAttachment" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover" Width="100%" HeaderStyle-HorizontalAlign="Center" OnRowDataBound="gvAttachment_RowDataBound">
                          <Columns>
                            <asp:TemplateField ItemStyle-HorizontalAlign="Right" HeaderText="No" ItemStyle-BorderStyle="None" ItemStyle-Width="25px">
                              <ItemTemplate>
                                <asp:Literal runat="server" ID="ltrl_Seq" Text='<%# $"{Eval("Seq")}" %>' Visible='<%# $"{Eval("Seq")}" == "0" ? false : true %>' />
                              </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderText="File Name" ItemStyle-BorderStyle="None">
                              <ItemTemplate>
                                <asp:Literal runat="server" ID="ltrl_Id" Text='<%# Eval("ID") %>' Visible="false" />
                                <asp:Literal runat="server" ID="ltrl_OwnerID" Text='<%# Eval("OwnerID") %>' Visible="false" />
                                <asp:Literal runat="server" ID="ltrl_FileNameUniq" Text='<%# Eval("FileNameUniq") %>' Visible="false" />
                                <asp:LinkButton runat="server" ID="lb_FileName" Text='<%# Eval("FileName") %>' />
                                <asp:Button runat="server" ID="btnDownload" Hidden="true" OnClick="btn_Click" EnableAjax="false" />
                              </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Del" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="25px">
                              <ItemTemplate>
                                <asp:ImageButton runat="server" ID="imbDelAttachWorkOrder" OnClick="imb_Click" ImageUrl="~/res/images/btnDelete.png" Style="line-height: 0px;" Visible='<%# $"{Eval("Seq")}" == "0" || $"{Eval("Mode")}" != "" ? false : true %>' />
                              </ItemTemplate>
                            </asp:TemplateField>
                          </Columns>
                          <HeaderStyle BackColor="#157fcc" ForeColor="White" />
                        </asp:GridView>

                      </div>
                    </div>
                  </div>
                </div>

              </asp:Panel>
            </div>
          </Content>
        </f:ContentPanel>
      </Items>
    </f:Panel>
    <f:Button runat="server" ID="btnStartDateChanged" OnClick="Fbtn_Click" Hidden="true" />
    <f:Button runat="server" ID="btnUpload" OnClick="Fbtn_Click" Hidden="true" />
    <f:Window ID="wWo" Title="Select Work Order" Hidden="true" EnableIFrame="true" runat="server" EnableMaximize="true" EnableResize="true" Target="Top" IsModal="false" Width="830px" Height="500px" OnClose="wWo_Close"></f:Window>
    <f:Window ID="wWorkUpdateItem" Title="Select Item" Hidden="true" EnableIFrame="true" runat="server" EnableMaximize="true" EnableResize="true" Target="Top" IsModal="false" Width="830px" Height="500px" OnClose="wWorkUpdateItem_Close"></f:Window>
    <f:Window ID="wWorkUpdateItemReference" Title="Select Item" Hidden="true" EnableIFrame="true" runat="server" EnableMaximize="true" EnableResize="true" Target="Top" IsModal="false" Width="830px" Height="500px" OnClose="wWorkUpdateItemReference_Close"></f:Window>
    <f:Window ID="wItem" Title="Select Item" Hidden="true" EnableIFrame="true" runat="server" EnableMaximize="true" EnableResize="true" Target="Top" IsModal="false" Width="830px" Height="500px" OnClose="wItem_Close"></f:Window>
  </form>
</body>
</html>
