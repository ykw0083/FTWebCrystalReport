<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CrLayouts.aspx.cs" Inherits="FTWebCrystalReport.CrLayouts" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/css/style.css" rel="stylesheet" type="text/css" />
    <link href="/css/animate.css" rel="stylesheet" type="text/css" />
    <link href="/css/toastr.css" rel="stylesheet" type="text/css" />
    <link href="/css/jquery-ui-1.12.1.css" rel="stylesheet" />
    <script src="/Scripts/jquery-3.2.1.min.js"></script>
<%--<script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.11.0/umd/popper.min.js" ></script>
<script src="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/js/bootstrap.min.js"></script>--%>

    <script src="/scripts/popper.min.js" ></script>
    <script src="/Scripts/bootstrap.min.js" ></script>

    <script src="/Scripts/jquery.dataTables.min.js"></script>
    <script src="/Scripts/dataTables.bootstrap.min.js"></script>
    <script src="/Scripts/jquery-1.12.1-ui.js"></script>
    <script src="/Scripts/toastr.min.js"></script>

    <style>
        #itemdetails th:nth-child(6), #itemdetails td:nth-child(6) {
        display:none;
        }
    .modal-footer {
        display:block;
    }
    .bootbox-body {
         font-size:16px;
        }
    .modal-footer button {
      float:right;
      margin-left: 15px;
    }
       
        .modal .show {
        display:block important;
        
        }
    </style>
</head>
<body>
    <input type="text" class="form-control" id="txtID" style="display:none;" />
    <input type="text" class="form-control" id="txtLineID" style="display:none;" />
    <input type="text" class="form-control" id="txtuserid" style="display:none;" />

<script>
    var id = $("#txtID");
    var oid = $("#txtLineID");
    var userid = $("#txtuserid");

    function getURLParameters(url) {

        var result = {};
        var searchIndex = url.indexOf("?");
        if (searchIndex == -1) return result;
        var sPageURL = url.substring(searchIndex + 1);
        var sURLVariables = sPageURL.split('&');
        for (var i = 0; i < sURLVariables.length; i++) {
            var sParameterName = sURLVariables[i].split('=');
            result[sParameterName[0]] = sParameterName[1];
        }
        return result;
    }

    function get_hostname(url) {
        var m = url.match(/^http:\/\/[^/]+/);
        return m ? m[0] : null;
    }
    $(document).ready(function () {
        var url = window.location.href;
        //var domain_url = "http://localhost:50600";
        var domain_url = get_hostname(url)
        var report_app = ""; //"/FTWebCrystalReport";
        console.log(get_hostname(url));
        var api_path = domain_url + report_app + "/api";

        var params = getURLParameters(url);
        if (Object.keys(params).length) {
            var report_id = params["id"];
            id.val(report_id);
            var report_oid = params["oid"];
            oid.val(report_oid);
            var report_userid = params["userid"];
            userid.val(report_userid);

            $.ajax({
                url: api_path + "/GenLayout/" + id.val() + "/" + oid.val() + "/" + userid.val(),
                method: "GET",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: {},
                success: function (result) {
                    console.log(result);
                    //downloadFile("pathinfo.pdfpath + "PR/" + result, result);
                    toastr.success("Report generated.");
                    //window.open(pathinfo.pdfpath + datainfo.username + "/" + result, '_blank', '');
                    //window.open(domain_url + report_app + "/pdf/" + report_id + "/" + result, '_blank', '');
                    window.open(domain_url + report_app + "/pdf/" + report_id + "/" + result, '_self');
                    setTimeout(function () {
                        window.close();
                    }, 500);
                },
                error: function (res, d) {
                    //toastr.error(jQuery.parseJSON(res.responseText).Message);
                    toastr.error(res.responseText);
                }
            })
        }

    });
</script>
</body>
</html>