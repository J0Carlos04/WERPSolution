function activeTabAndUpdate(tabId, param1) {
    var mainTabStrip = F(mainTabStripClientID);
    var targetTab = mainTabStrip.getTab(tabId);
    var oldActiveTabId = getActiveTabId();

    if (targetTab) {
        mainTabStrip.setActiveTab(targetTab);
        // 通过jQuery查找 iframe 节点，并调用 iframe 内的页面的自定义JS函数 updatePage
        $(targetTab.el.dom).find('iframe')[0].contentWindow.updatePage(param1);

        // 删除之前的激活选项卡
        mainTabStrip.removeTab(oldActiveTabId);
    }
}     
function InitSelect2StockOrderInput() {      
    $(document).ready(function () {
        $("#pContent_pData_ddlRequester").select2({ placeholder: "Select Requester", allowClear: true, theme: 'bootstrap-5' });
        $("#pContent_pData_ddlApprover").select2({ placeholder: "Select Approver", allowClear: true, theme: 'bootstrap-5' });
        $("#pContent_pData_ddlVendor").select2({ placeholder: "Select Vendor", allowClear: true, theme: 'bootstrap-5' });
    });    
}
function InitSelect2StockReceivedInput() {
    $(document).ready(function () {
        $("#pContent_pData_ddlReceiverUserId").select2({ placeholder: "Select Receiver", allowClear: true, theme: 'bootstrap-5' });        
    });
}
function InitSelect2StockReceivedReturInput() {
    $(document).ready(function () {
        $("#pContent_pData_ddlRequester").select2({ placeholder: "Select Requester", allowClear: true, theme: 'bootstrap-5' });
    });
}
function InitSelect2StockOutReturInput() {
    $(document).ready(function () {
        $("#pContent_pData_ddlRequester").select2({ placeholder: "Select Receiver", allowClear: true, theme: 'bootstrap-5' });
    });
}
function InitSelect2MaintenanceSchedulerInput() {
    $(document).ready(function () {
        $("#pContent_pData_ddlArea").select2({ placeholder: "Select Area", allowClear: true, theme: 'bootstrap-5' });
        $("#pContent_pData_ddlLocation").select2({ placeholder: "Select Location", allowClear: true, theme: 'bootstrap-5' });
        $("#pContent_pData_ddlWorkOrderCategory").select2({ placeholder: "Select Order Category", allowClear: true, theme: 'bootstrap-5' });
        $("#pContent_pData_ddlSubject").select2({ placeholder: "Select Subject", allowClear: true, theme: 'bootstrap-5' });
    });
}
function InitSelect2HelpdeskInput() {
    $(document).ready(function () {
        $("#pContent_pData_ddlWorkOrderType").select2({ placeholder: "Select Work Order Type", allowClear: true, theme: 'bootstrap-5' });
        $("#pContent_pData_ddlCategory").select2({ placeholder: "Select Category", allowClear: true, theme: 'bootstrap-5' });
        $("#pContent_pData_ddlArea").select2({ placeholder: "Select Area", allowClear: true, theme: 'bootstrap-5' });
        $("#pContent_pData_ddlLocation").select2({ placeholder: "Select Location", allowClear: true, theme: 'bootstrap-5' });
        $("#pContent_pData_ddlWorkOrderCategory").select2({ placeholder: "Select Order Category", allowClear: true, theme: 'bootstrap-5' });
        $("#pContent_pData_ddlSubject").select2({ placeholder: "Select Subject", allowClear: true, theme: 'bootstrap-5' });
        $("#pContent_pData_ddlVendor").select2({ placeholder: "Select Vendor", allowClear: true, theme: 'bootstrap-5' });
    });
}
function cancelBack() {
    if (event.srcElement.className != "select2-search__field" && (event.keyCode == 8 || (event.keyCode == 37 && event.altKey) || (event.keyCode == 39 && event.altKey)) && (event.srcElement.form == null || event.srcElement.isTextEdit == false)) {
        event.cancelBubble = true;
        event.preventDefault();
        event.returnValue = false;
        return false;
    }
} 
function pnlEmployedClicked() {
    var evt = (evt) ? evt : ((event) ? event : null);
    evt.cancelBubble = true;
}
function pnlNameClicked() {
    var evt = (evt) ? evt : ((event) ? event : null);
    if (event.srcElement.id.indexOf("btnOk") == -1) 
        evt.cancelBubble = true;             
}
function ShowDDE() {
    var dde = $find("ContentPlaceHolder1_ucNew1_ddeEmployee");
    if (dde != null)
        dde.show();
}
function HideDDE() {
    var dde = $find("ContentPlaceHolder1_ucNew1_ddeEmployee");
    if (dde != null)
        dde.hide();
}
function EnterToCallButton(btn) {    
    if (window.event.keyCode == 13) {
        var btnClientChanged = document.getElementById(btn);
        btnClientChanged.click();
    }
    return false;
}
function ClientChanged(btn) {    
    var btnClientChanged = document.getElementById(btn);
    btnClientChanged.click();
}
function getLocation(lblLatitude, lblLongitude, hfLatitude, hfLongitude) {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function (position) {
            var latitude = position.coords.latitude;
            var longitude = position.coords.longitude;

            document.getElementById(hfLatitude.id).value = latitude;
            document.getElementById(hfLongitude.id).value = longitude;
            
            document.getElementById(lblLatitude.id).innerHTML = latitude;
            document.getElementById(lblLongitude.id).innerHTML = longitude;
            
            
        });
    } else {
        alert("Geolocation is not supported by this browser.");
    }
}
function SubmitClick(sender, btn) {    
    var btnClientChanged = document.getElementById(btn);
    btnClientChanged.click();    
}
function ShowConfirm(Total, btn) {
    if (confirm('Total Row: ' + Total + '. Are you sure you want to export to excel?')) {
        var btnClientChanged = document.getElementById(btn);
        btnClientChanged.click();
    } else {
        return false;
    }
}
function ShowConfirmRecall(Total, btn) {
    if (confirm('Total Row: ' + Total + '. Are you sure you want to recall export to excel?')) {
        var btnClientChanged = document.getElementById(btn);
        btnClientChanged.click();
    } else {
        return false;
    }
}
function timedRefresh(timeoutPeriod) {
    setTimeout("location.reload(true);", timeoutPeriod);
}
function sortData(btn, hfSortField, sortField) {
    var btnSort = document.getElementById(btn);
    var hfSort = document.getElementById(hfSortField);
    hfSort.value = sortField;
    btnSort.click();
}
function DataSelection(btn, Value, Text, tbFromName, tbFromIDName) {
    var btnDataSelection = document.getElementById(btn);
    var tbFrom = document.getElementById(tbFromName);
    var tbFromID = document.getElementById(tbFromIDName);

    tbFrom.value = Text;
    tbFromID.value = Value;
    btnDataSelection.click();
}
function SelectAll(CheckBoxControl, GridName) {
    var cb = document.getElementById(CheckBoxControl);
    var i;
    for (i = 0; i < document.forms[0].elements.length; i++) {
        if ((document.forms[0].elements[i].type == 'checkbox') && ((document.forms[0].elements[i].name.indexOf(GridName) > -1) || (document.forms[0].elements[i].id.indexOf(GridName) > -1))) {
            if (cb.checked == true)
                document.forms[0].elements[i].checked = true;
            else
                document.forms[0].elements[i].checked = false;
        }
    }
}
function SelectAllApprovalList(CheckBoxControl, GridName) {
    var cb = document.getElementById(CheckBoxControl);
    var i;
    for (i = 0; i < document.forms[0].elements.length; i++) {
        if ((document.forms[0].elements[i].type == 'checkbox') && ((document.forms[0].elements[i].name.indexOf(GridName) > -1) || (document.forms[0].elements[i].id.indexOf(GridName) > -1))) {
            if (document.forms[0].elements[i].id.indexOf("Budget") == -1) {
                if (cb.checked == true)
                    document.forms[0].elements[i].checked = true;
                else
                    document.forms[0].elements[i].checked = false;
            }
        }
    }
}
function isNumber(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}
function CheckInOutSpan(sender, args) {
    var btnID = null;
    if (sender._id.indexOf('ceCheckIn') > -1) {
        btnID = sender._id.replace("ceCheckIn", "btnChangedDateAccommodation");        
    }
    else {
        btnID = sender._id.replace("ceCheckOut", "btnChangedDateAccommodation");        
    }
    var btn = document.getElementById(btnID);   
    btn.click();
}
function ChangedDateAccommodation(tbID) {
    var btn = document.getElementById(tbID);
    btn.click();
}
function NoSpace(event) {
    //handling ie and other browser keycode 
    var keyPressed = event.which || event.keyCode;

    //Handling whitespace
    //keycode of space is 32
    if (keyPressed == 32) {
        event.preventDefault();
        event.stopPropagation();
    }
}
function OnlyNumber(evt) {
    var theEvent = evt || window.event;

    // Handle paste
    if (theEvent.type === 'paste') {
        key = event.clipboardData.getData('text/plain');
    } else {
        // Handle key press
        var key = theEvent.keyCode || theEvent.which;
        key = String.fromCharCode(key);
    }
    var regex = /[0-9]|\./;
    if (!regex.test(key)) {
        theEvent.returnValue = false;
        if (theEvent.preventDefault) theEvent.preventDefault();
    }

    if (theEvent.keyCode == 13) {
        theEvent.returnValue = false;
        theEvent.preventDefault();
    }
}

function addCommas(Num) {
    Num += '';
    Num = Num.replace(',', ''); Num = Num.replace(',', ''); Num = Num.replace(',', '');
    Num = Num.replace(',', ''); Num = Num.replace(',', ''); Num = Num.replace(',', '');
    x = Num.split('.');
    x1 = x[0];
    x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1))
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    return x1 + x2;
}
function ClickButtonForEnter(evt, btnID) {
    ohevt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode == 13) {
        var btn = document.getElementById(btnID);
        btn.click();
        return false;
    }
}
// current animated collapsible panel content
var currentContent = null;
function togglePannelAnimatedStatus(content, interval, step) {
    // wait for another animated expand/collapse action to end
    if (currentContent == null) {
        currentContent = content;
        var expand = (content.style.display == "none");
        if (expand)
            content.style.display = "block";
        var max_height = content.offsetHeight;

        var step_height = step + (expand ? 0 : -max_height);
        toggleChevronIcon(content);

        // schedule first animated collapse/expand event
        content.style.height = Math.abs(step_height) + "px";
        setTimeout("togglePannelAnimatingStatus("
            + interval + "," + step
            + "," + max_height + "," + step_height + ")", interval);
    }
}

function togglePannelAnimatingStatus(interval,
    step, max_height, step_height) {
    var step_height_abs = Math.abs(step_height);

    // schedule next animated collapse/expand event
    if (step_height_abs >= step && step_height_abs <= (max_height - step)) {
        step_height += step;
        currentContent.style.height = Math.abs(step_height) + "px";
        setTimeout("togglePannelAnimatingStatus("
            + interval + "," + step
            + "," + max_height + "," + step_height + ")", interval);
    }
    // animated expand/collapse done
    else {
        if (step_height_abs < step)
            currentContent.style.display = "none";
        currentContent.style.height = "";
        currentContent = null;
    }
}

// change chevron icon into either collapse or expand
function toggleChevronIcon(content) {
    //var chevron = content.parentNode.firstChild.childNodes[1].childNodes[0];
    var chevron = content.parentNode.firstElementChild.childNodes[3].childNodes[0];
    var expand = (chevron.src.indexOf("expand.png") > 0);
    chevron.src = chevron.src
        .split(expand ? "expand.png" : "collapse.png")
        .join(expand ? "collapse.png" : "expand.png");
}

function SetTotalHour() {
    var tbStartDate = document.getElementById("pContent_tlbTop_cpStart_tbStart");
    var tbEndDate = document.getElementById("pContent_tlbTop_cpEnd_tbEnd");
    var ttTotal = document.getElementById("pContent_tlbTop_ttTotal");
    if (tbStartDate.value == "" || tbEndDate.value == "")
        return;
    var dtStart = new Date(tbStartDate.value);
    var dtEnd = new Date(tbEndDate.value);
    var diff = (dtEnd.getTime() - dtStart.getTime()) / 1000;
    diff /= (60 * 60);
    var Day = 0;
    var Hour = Math.abs(Math.round(diff));
    if (Hour > 24) {
        Day = parseInt(Hour / 24);
        Hour = parseInt(Hour % 24);
    }
    ttTotal.innerHTML = Day + " Days" + Hour + " Hours";
}
function AddClassToClass(SourceClass, AddedClass) {
    const elements = document.querySelectorAll(SourceClass);

    elements.forEach((element) => {
        element.classList.add(AddedClass);
    });
}