function allowDrop(ev) {
    ev.preventDefault();
};

function drag(ev) {
    let FromAccName = $(ev.target).attr("accatr");

    if (FromAccName != "Deposit-drag") {
        ev.target.parentElement.removeAttribute("ondragover");
    }

    ev.dataTransfer.setData("FromAccName", FromAccName);
    let FromParentId = ev.target.parentElement.id;
    ev.dataTransfer.setData("FromParentId", FromParentId);
};

function drop(ev) {    
    ev.preventDefault();
    let FromAccName = ev.dataTransfer.getData("FromAccName");
    let FromParentId = ev.dataTransfer.getData("FromParentId");

    let FromParentElement = document.getElementById(FromParentId);

    let ToAccElement = ev.target;

    while ($(ToAccElement).attr("accatr") == undefined) {
        ToAccElement = ToAccElement.parentElement;
    }

    let ToAccName = $(ToAccElement).attr("accatr");

    hideAllAccountForms();

    if (FromAccName == "Deposit-drag" && (ToAccName == "Withdraw-drop" || ToAccName == "Delete-drop")) {
        return;
    }

    if (FromAccName == "Deposit-drag") {
        FillDepositAcc(ToAccName);
        $("#MakeDepositAccountFormDiv").show("slow");
        return;
    }

    if (ToAccName == "Withdraw-drop") {
        FillWithdrawAcc(FromAccName);
        $("#WithdrawFromAccountFormDiv").show("slow");
        FromParentElement.setAttribute("ondragover", "allowDrop(event)");
        return;
    }

    if (ToAccName == "Delete-drop") {
        FillDeleteAcc(FromAccName);
        $("#DeleteAccountFormDiv").show("slow");
        FromParentElement.setAttribute("ondragover", "allowDrop(event)");
        return;
    }

    $("#TransactToAccountFormDiv").show("slow");
    HideAllExcept("#CalculateTransactionButton");
    FillTransactAccFrom(FromAccName);
    FillTransactAccTo(ToAccName);

    FromParentElement.setAttribute("ondragover", "allowDrop(event)");
};

function FillTransactAccFrom(accName) {

    $('#TransactToAccount_AccountFromName').val(accName);

    let accID = "#" + accName + "_acc";

    let html = $(accID).html();

    $('#TransactionFromAcc').html(html);
};

function FillTransactAccTo(accName) {

    $('#TransactToAccount_AccountToName').val(accName);

    let accID = "#" + accName + "_acc";

    let html = $(accID).html();

    $('#TransactionToAcc').html(html);
};

function FillDepositAcc(accName) {

    $('#DepositAccountName').val(accName);

    let accID = "#" + accName + "_acc";

    let html = $(accID).html();

    $('#DepositToAcc').html(html);
};

function FillWithdrawAcc(accName) {

    $('#Withdraw_AccountName').val(accName);

    let accID = "#" + accName + "_acc";

    let html = $(accID).html();

    $('#WithdrawFromAcc').html(html);
};

function FillDeleteAcc(accName) {

    $('#DeleteAccountName').val(accName);

    let accID = "#" + accName + "_acc";

    let buffer = $(accID).find("h4").text();
    let accCurr = $(accID).find("kbd").text();
    let accAmount = buffer.replace(accCurr,'');

    $('#AccToDeleteName').text(accName);
    $('#AccToDeleteAmount').text(accAmount);
    $('#AccToDeleteCurr').text(accCurr);
};