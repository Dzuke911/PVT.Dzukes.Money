$(document).ready(function () {

    let lastReq = 0;

    $("#CalculateTransactionButton").on("click", function (ev) {
        lastReq++;
        HideAllExcept("#TransactionWaitPlease");
        //$.post("http://localhost:50462/Home/TransactionCheck",
        //$.post("https://localhost:44330/Home/TransactionCheck",

        let path = $("#TransactionCheckAjaxPath").text();

        $.post(path,
            {
                accfrom: $("#TransactToAccount_AccountFromName").val(),
                accto: $("#TransactToAccount_AccountToName").val(),
                amount: $("#TransactToAccount_Amount").val(),
                reqnum: lastReq
            },
            function (data, status) {
                ShowTransactionCheckResult(data, lastReq);
            });
    });

    $("#TransactToAccount_Amount").on("input", function (ev) {
        HideAllExcept("#CalculateTransactionButton");
    });

    $("#TransactToAccount_AccountFromName").on("change", function (ev) {
        HideAllExcept("#CalculateTransactionButton");
    });

    $("#TransactToAccount_AccountToName").on("change", function (ev) {
        HideAllExcept("#CalculateTransactionButton");
    });
})

function HideAllExcept(stringID, stringIDsecond) {
    $("#CalculateTransactionButton").hide();
    $("#TransactionResultSameAccounts").hide();
    $("#TransactionResultCantParse").hide();
    $("#TransactionResultNotEnough").hide();
    $("#TransactionResultSuccess").hide();
    $("#TransactToAccountSubmit").hide();
    $("#TransactionWaitPlease").hide();
    $(stringID).show()
    if (stringIDsecond != undefined) {
        $(stringIDsecond).show()
    }
}


function ShowTransactionCheckResult(data, lastReq) {

    //if (data["reqNum"] < lastReq) {
    //    return;
    //}

    if (data["isSameAccs"] == true) {
        HideAllExcept("#TransactionResultSameAccounts");
        return;
    }

    if (data["isParsed"] == false) {
        HideAllExcept("#TransactionResultCantParse");
        return;
    }

    if (data["isEnough"] == false) {
        $("#TResultMaxAmount").text("The maximum allowed transaction amount is " + data["sendAmount"].formatMoney(2, ',', ' ') + " " + data["currFrom"]);
        HideAllExcept("#TransactionResultNotEnough");
        return;
    }

    let accFromName = $("#TransactToAccount_AccountFromName option:selected").attr("value");
    let accToName = $("#TransactToAccount_AccountToName option:selected").attr("value");
    $("#TResultSended").text(data["sendAmount"].formatMoney(2, ',', ' ') + " " + data["currFrom"] + " will be subtracted from '" + accFromName + "' account");
    $("#TResultCommission").text("The commision amount will be " + data["commission"].formatMoney(2, ',', ' ') + " " + data["currFrom"]);
    $("#TResultRecieve").text("'" + accToName + "' account will get " + data["receive"].formatMoney(2, ',', ' ') + " " + data["currTo"]);
    $("#TResultRest").text("The balance on '" + accFromName + "' account will be " + data["rest"].formatMoney(2, ',', ' ') + " " + data["currFrom"]);

    HideAllExcept("#TransactionResultSuccess", "#TransactToAccountSubmit");
}

Number.prototype.formatMoney = function (c, d, t) {
    var n = this,
        c = isNaN(c = Math.abs(c)) ? 2 : c,
        d = d == undefined ? "." : d,
        t = t == undefined ? "," : t,
        s = n < 0 ? "-" : "",
        i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
        j = (j = i.length) > 3 ? j % 3 : 0;
    return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? d + Math.abs(n - i).toFixed(c).slice(2) : "");
};
