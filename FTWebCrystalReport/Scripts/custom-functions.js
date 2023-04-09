﻿ function ButtonLoading(btn) {
    btn.attr('disabled', true);
    btn.html("Loading...");
}
function ButtonReset(btn, text) {
    btn.html(text);
    btn.attr('disabled', false);
}
function formatDate(x) {
    var formattedDate = new Date(x);
    var d = formattedDate.getDate();
    var m = formattedDate.getMonth();
    m += 1;  // JavaScript months are 0-11
    var mName = "";
    switch (m)
    {
        case 1: mName = "JAN";
            break;
        case 2: mName = "FEB"
            break;
        case 3: mName = "MAR"
            break;
        case 4: mName = "APR"
            break;
        case 5: mName = "MAY"
            break;
        case 6: mName = "JUN"
            break;
        case 7: mName = "JUL"
            break;
        case 8: mName = "AUG"
            break;
        case 9: mName = "SEP"
            break;
        case 10: mName = "OCT"
            break;
        case 11: mName = "NOV"
            break;
        case 12: mName = "DEC"
            break;
    }
    var y = formattedDate.getFullYear();

    return ('0' + d).slice(-2) + "-" + mName + "-" + y;
}
function formatTime(x) {
    var formattedDate = new Date(x);
    var h = formattedDate.getHours();
    var m = formattedDate.getMinutes();
    var s = formattedDate.getSeconds();

    return ('0' + h).slice(-2) + ":" + ('0' + m).slice(-2) + ":" + ('0' + s).slice(-2);
}
function downloadFile(path, filename) {
    var anchor = document.createElement('a');
    anchor.href = path;
    anchor.target = '_blank';
    anchor.download = filename;
    anchor.click();
}
function loaddropdown() {
    $.ajax({
        type: "get",
        url: "/api/medivestowhs",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        data: {},
        success: function (result) {
            $('#ddlWarehouse').html("");
            //$('#ddlWarehouse').children('option').remove();
            $.each(result, function (i) {
                $('#ddlWarehouse').append($('<option></option>').val(result[i]["whscode"]).html(result[i]["whsname"]));
            });
            $("#ddlWarehouse").prepend("<option value='' selected='selected'></option>");
        },
        failure: function () {
            alert("Error");
        }
    });
}

