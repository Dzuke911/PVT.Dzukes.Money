$(document).ready(function () {

    $("#SelectedUser").change(function () {
        var role = $("#SelectedUser option:selected").attr("roleAttr");
        deselectNewRoles();
        $("#NewRole [value='" + role + "']").attr("selected", "selected");
    });

    $("#SelectedRole").change(function () {
        fillPermissionsColumns();
        fillNewPermissions();
    });

    $("#RolesManagementPage").ready(function () {
        fillPermissionsColumns();
        fillNewPermissions();
    });

    $("div[attrused]").click(function () {
        var element = this;
        var used = $(element).attr("attrused");
        if (used == "false") {
            $(element).detach();
            $(element).appendTo($("#addedPermissions"));
            $(element).attr("attrused", "true");
            fillNewPermissions();
        }
        if (used == "true") {
            $(element).detach();
            $(element).appendTo($("#removedPermissions"));
            $(element).attr("attrUsed", "false");
            fillNewPermissions();
        }
    });

    $("#UsersManagementPage").ready(function () {
        if ($("#UsersManagementMessage").text() != undefined) {
            $("#UsersManagementMessage").fadeOut(3000);
        }
    });

    $("#EditPersonalDataButton").click(function () {
        $("#PersonalDataBodyShow").hide("slow");
        $("#PersonalDataBodyEdit").show("slow");
        $('#Login').focus();
        $('#Login').blur();
        $('#Email').focus();
        $('#Email').blur();
    });

    $("#PersonalDataBodyEdit").find("#Login").on("input", function (ev) {
        var login = $(this).val();
        var attr = $("#Login").attr("attrbaselogin");
        if (login == attr) {
            $("#SignOutWarning").hide();
        }
        else {
            $("#SignOutWarning").show();
        }
    });

    //
    //  Index view functions
    //
    $("#CreateAccountButton").click(function () {
        hideAllAccountForms();
        $("#CreateAccountFormDiv").show("slow");
    });

    //$(".accountwell").click(function () {
    //    var element = this;
    //    var childrens = $(element).children();
    //    var name = childrens[0].innerHTML;
    //    deselectAllAccounts();
    //    element.classList.add("accountwellselected");        
    //    $("#DepositAccountName").attr("value", name);
    //    $("#Withdraw_AccountName").attr("value", name);
    //});

});

//
//For SignalR
//
/*
$(document).ready(function () {
    let hubUrl = 'http://localhost:50462/MoneyHub';
    let httpConnection = new signalR.HttpConnection(hubUrl);
    let hubConnection = new signalR.HubConnection(httpConnection);
    hubConnection.on("Send", function (message) {
        window.alert(message);
    });
    hubConnection.start();
    SendMessage(hubConnection);
});

function SendMessage(hubConnection) {
    setTimeout(function () { hubConnection.invoke("Receive", "TestMessage")},12000);   
}*/

//
//For MenuItemSelection
//
$(document).ready(function () {
    var item = $("#SelectedMenuItem").attr("value");
    $(item).addClass("active");
});

//
//
//
function deselectAllAccounts() {
    $(".accountwellselected").each(function () { this.classList.remove("accountwellselected") });
}

function hideAllAccountForms() {
    $("#AccountFormsDiv").children("div").hide("slow");
}

function deselectNewRoles() {
    $('#NewRole option:selected').each(function () { this.removeAttribute("selected") });
}

function fillNewPermissions() {
    var array = $('#addedPermissions').find("div");
    var usedPermissionsArray = [];

    for (a = 0; a < array.length; a++) {
        usedPermissionsArray.push($(array[a]).attr("value"));
    }

    $("#NewPermissions").attr("value", usedPermissionsArray.toString());
}

function fillPermissionsColumns() {

    var selectedRolePermissions = $("#SelectedRole option:selected").attr("attrPerms");
    if (selectedRolePermissions == undefined) {
        selectedRolePermissions = "";
    }
    var rolePermissionsArray = selectedRolePermissions.split(",");
    var allPermissionsArray = [];
    var isUsed;

    var array = $('#addedPermissions').find("div");

    for (a = 0; a < array.length; a++) {
        allPermissionsArray.push($(array[a]).attr("value"));
    }

    array = $('#removedPermissions').find("div");

    for (b = 0; b < array.length; b++) {
        allPermissionsArray.push($(array[b]).attr("value"));
    }

    for (i = 0; i < allPermissionsArray.length; i++) {
        var isUsed = false;
        for (j = 0; j < rolePermissionsArray.length; j++) {
            if (allPermissionsArray[i] === rolePermissionsArray[j]) {
                isUsed = true;
                continue;
            }
        }

        if (isUsed === true) {
            $("#" + allPermissionsArray[i]).appendTo("#addedPermissions");
            $("#" + allPermissionsArray[i]).attr("attrUsed", "true");
        }
        else {
            $("#" + allPermissionsArray[i]).appendTo("#removedPermissions");
            $("#" + allPermissionsArray[i]).attr("attrUsed", "false");
        }
    }
}