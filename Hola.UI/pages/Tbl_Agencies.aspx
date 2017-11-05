<%@ Page Title="" Language="C#" MasterPageFile="~/MasterHola.master" AutoEventWireup="true" CodeFile="Tbl_Agencies.aspx.cs" Inherits="pages_Tbl_Agencies" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-lg-12">
            <h1 class="page-header"><i class="fa fa-briefcase  fa-fw"></i>Agencies</h1>
        </div>
    </div>

    <div class="panel-heading">
        <h2 class="panel-title">Add New Agancy</h2>
    </div>

    <div class="panel-body">


        <div class="form-group   has-feedback">
            <%--has-warning--%>
            <label class="control-label col-sm-2" for="txtName">Agency Name </label>
            <div class="col-sm-9">
                <input type="text" id="txtName" required="required" class="form-control" data-bind="value: new_name" placeholder="Name" />
                <span class="glyphicon  form-control-feedback"></span><%--glyphicon-ok || glyphicon-warning-sign ||glyphicon-remove --%>
            </div>
        </div>
        <div class="form-group   has-feedback">
            <%--has-warning--%>
            <label class="control-label col-sm-2" for="txtAddress">Agency Address </label>
            <div class="col-sm-9">
                <input type="text" id="txtAddress" required="required" class="form-control" data-bind="value: new_address" placeholder="Address" />
                <span class="glyphicon  form-control-feedback"></span><%--glyphicon-ok || glyphicon-warning-sign ||glyphicon-remove --%>
            </div>
        </div>

        <div class="col-sm-2">

            <button type="button" data-bind="click: add_server" class="btn btn-primary">Submit</button>

        </div>
    </div>




    <table class="table table-striped table-bordered table-hover" id="tbl">
        <thead>
            <tr>

                <th>ID               </th>
                <th>Name              </th>
                <th>Address</th>


            </tr>
        </thead>
        <tbody data-bind="foreach: agencies">
            <tr>
                <td data-bind="text: ID"></td>
                <td data-bind="text: name"></td>
                <td data-bind="text: address"></td>
            </tr>
        </tbody>
    </table>


</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="FooterScripts" runat="Server">

    <script src="../views_client/view_agency.js"></script>

    <style>
        .dataTables_empty {
            display: none;
        }
    </style>

    <script>




        $(document).ready(function () {

            var tbl = $('#tbl').DataTable({
                //responsive: true,
                "paging": false,
                "info": false,
                "searching": false,
                "ordering": false,
                data: self.agencies,
                "sAjaxDataProp": "",
                "columns": [
                            { "data": "ID" },
                            { "data": "name" },
                            { "data": "address" },


                ]
            });
            $('#tbl tbody').on('click', 'tr', function () {

                if ($(this).hasClass('selected')) {
                    $(this).removeClass('selected');
                }
                else {
                    tbl.$('tr.selected').removeClass('selected');
                    $(this).addClass('selected');
                }
            });
        });




    </script>
    <script>
        //Date.prototype.yyyymmdd = function () {
        //    var yyyy = this.getFullYear().toString();
        //    var mm = (this.getMonth() + 1).toString(); // getMonth() is zero-based
        //    var dd = this.getDate().toString();
        //    return yyyy + '-' + (mm[1] ? mm : "0" + mm[0]) + '-' + (dd[1] ? dd : "0" + dd[0]); // padding
        //};

    </script>
</asp:Content>
