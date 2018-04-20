$(document).ready(function () {

    $("#HistoryPage").ready(function () {
        $('#HistoryTable').DataTable({
            "order": [[1, "desc"], [2, "desc"]],
            "columnDefs": [{ "orderData": [1, 2], "targets": 1 }]
        });
    });
});