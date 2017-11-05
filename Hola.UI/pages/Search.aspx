<%@ Page Title="Search Reservations" Language="C#" MasterPageFile="~/MasterHola.master" AutoEventWireup="true" CodeFile="Search.aspx.cs" Inherits="pages_Search" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        #tblSearch_filter {
            display: none;
        }

        .buttons-excel {
            background-image: -webkit-linear-gradient(top, white 0%, #5cb85c 100%) !important;
        }

        /*td.details-control {
            background: url('../icons/fa-edit.png') no-repeat center center;
            cursor: pointer;
        }


        tr.details td.details-control {
            background: url('../icons/fa-edit.png') no-repeat center center;
        }*/

        td.edit {
            background: url('../icons/fa-edit.png') no-repeat center center;
            cursor: pointer;
        }

        .phone {
            /*background: url('../icons/phone.png') no-repeat  ;*/
            cursor: pointer;
            margin: 3px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-lg-12">
            <h1 class="page-header"><i class="fa fa-search  fa-fw"></i>Search Reservations
            </h1>
        </div>
        <!-- /.col-lg-12 -->
    </div>

<%--    <!-- Modal -->
    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="myModalLabel">Modal title</h4>
                </div>
                <div class="modal-body">
                    ...
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    <button type="button" class="btn btn-primary">Save changes</button>
                </div>
            </div>
        </div>
    </div>--%>
    <div class="row">
        <label>
            <input id="txtSearchBox" type="search" class="form-control input-sm" placeholder="Search" aria-controls="tblSearch" />
            <a id="lnkAdvanced" href="#">Advanced Search </a>
        </label>
        <div id="div_advanced" style="display: none" class="well">
            <label>Search Options:</label>


            <select id="ddlAgency" class="form-control margin-bottom-14" style="display: inline; width: 180px" data-bind="


    options: agencies,
    optionsText: 'name',
    optionsValue: 'name',
    value: agency_name,
    optionsCaption: 'Show All Agencies',
    valueAllowUnset: true
">
            </select>

            <%--            <input id="txtSearchBox" type="text" style="display: inline; width: 200px" class="form-control margin-bottom-14" placeholder="Free text" />--%>

            <select id="ddlHotel" class="form-control margin-bottom-14" style="display: inline; width: 180px" data-bind="


    options: hotels,
    optionsText: 'name',
    optionsValue: 'name',
    value: hotel_name,
    optionsCaption: 'Show All Hotels',
    valueAllowUnset: true
">
            </select>
            <input id="txtDateArr" type="text" style="display: inline; width: 200px" class="form-control margin-bottom-14" placeholder="Arrival Date" data-bind="value: date_arr" />
            <input id="txtDateDep" type="text" style="display: inline; width: 200px" class="form-control margin-bottom-14" placeholder="Departure Date" data-bind="value: date_dep" />


        </div>

        <table class="table table-striped table-bordered table-hover  order-column compact" id="tblSearch">
            <thead>
                <tr>
                    <%--                    <th></th>--%>
                    <th>PNR</th>
                    <th>Names</th>
                    <th>PAX</th>
                    <th>Arrival#</th>
                    <th>Arrival Date</th>
                    <th>Dep.#</th>
                    <th>Dep. Date</th>
                    <th>Hotel</th>
                    <th>Agency</th>

                </tr>
            </thead>
        </table>
    </div>


</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="FooterScripts" runat="Server">


    <link href="../Content/DataTables/css/buttons.dataTables.css" rel="stylesheet" />
    <script src="../scripts/DataTables/dataTables.buttons.js"></script>
    <script src="../scripts/jszip.min.js"></script>
    <script src="../scripts/pdfmake.min.js"></script>
    <script src="../scripts/vfs_fonts.js"></script>
    <script src="../scripts/DataTables/buttons.html5.js"></script>
    <script src="../scripts/DataTables/buttons.print.js"></script>

    <%--<script src="../scripts/knockout-3.4.0.js"></script>--%>
    <script src="../views_client/view_search.js"></script>


    <script>
        var my = { viewModel: new SaleViewModel() };
        ko.applyBindings(my.viewModel);
        var dataTable;
        var search = '<%=Search%>';
        $(document).ready(function () {


            $("#txtDateArr").datepicker({ dateFormat: 'yy-mm-dd' });
            $("#txtDateDep").datepicker({ dateFormat: 'yy-mm-dd' });



            dataTable = $('#tblSearch').DataTable({
                "drawCallback": function (settings) {
                    $('[data-toggle="popover"]').popover();
                },
                responsive: true,
                "ajax": url_clients,
                "pageLength": 10,
                "bLengthChange": false,
                //"sDom": '<"top"i>rt<"bottom"flp><"clear">',
                "sAjaxDataProp": "",
                "autoWidth": false,
                "columnDefs": [{
                    "targets": col_names,
                    "searchable": false,
                    "sortable": false,
                    "width": "30px"

                }],
                "columns": [
                            //{
                            //    //"class": "details-control",
                            //    "class": "edit",
                            //    "orderable": false,
                            //    "data": null,
                            //    "defaultContent": "",
                            //    "width": "10px"
                            //},//0
                            { "data": "PNR" },//1
                            { "data": "names", "width": "20px" },//2
                            { "data": "PAX", "width": "40px" },//3
                            { "data": "num_arr", "width": "100px" },//4
                            { "data": "date_arr", "width": "110px" },//5
                            { "data": "num_dep", "width": "70px" },//6
                            { "data": "date_dep", "width": "110px" },//7
                            { "data": "hotel_name" },//8
                            { "data": "agency_name", "width": "110px" },//9
                            { "data": "phone", 'visible': false },//9
                            ////{ "data": "phone", "width": "70px" },//9
                            //{
                            //    "class": "phone",
                            //    "orderable": false,
                            //    "data": null,
                            //    "defaultContent": ""
                            //},//0
                ],
                "columnDefs": [
                            {
                                "render": function (data, type, row) {

                                    if (row.phone != "") {
                                        return data + ' <a class="phone" data-toggle="popover" role="button" data-trigger="focus" tabindex="0"  title="Phone Number" data-content="' + row.phone + '"><i class="fa fa-phone-square fa-lg"></i></a>';
                                    }
                                    else {
                                        return data;
                                    }

                                },
                                "targets": col_names
                            },
                            {
                                "render": function (data, type, row) {
                                    return new Date(data).yyyymmdd();

                                },
                                "targets": col_date_arr
                            },
                            {
                                "render": function (data, type, row) {
                                    if (data) {
                                        return new Date(data).yyyymmdd();
                                    } else { return data; }
                                },
                                "targets": col_date_dep
                            }
                ],
                dom: 'frtipB',

                buttons: [
                        //'copy',
                        { extend: 'excel', text: '<i class="fa fa-file-excel-o fa-fw"></i><span>Export Table</span>' },
                        //'pdf',
                        //'print'
                ],
                "rowCallback": function (row, data, index) {
                    if (data.phone != "") {
                        // alert(data.phone);
                        ////  $('td:eq(2)', row).append('<img src="../icons/phone.png" />');
                        ////<a href="Client_Insert.aspx"><i class="fa fa-plus-circle fa-fw"></i>Insert a New Client</a>
                        //$('td:eq(' + col_names + ')', row).append('<a class="phone" data-toggle="popover" role="button" data-trigger="focus" tabindex="0"  title="Phone Number" data-content="' + data.phone + '"><i class="fa fa-phone-square fa-lg"></i></a>');
                        //// $('td:eq(2)', row).append('<button type="button" class="btn btn-lg btn-danger phone" data-toggle="popover" title="Popover title" data-content="And heres some amazing content. Its very engaging. Right?">Click to toggle popover</button>');

                    }
                },
                "drawCallback": function (settings) {
                    $('[data-toggle="popover"]').popover();
                },

            });




            $('#lnkAdvanced').click(function myfunction() {

                $('#div_advanced').toggle('slow');

            });


            $('#tblSearch').on('page.dt', function () {
                //alert();
                //var xxx = $('[data-toggle="popover"]');
                //debugger;
                //$('[data-toggle="popover"]').popover();
            });



            //$('#tblSearch tbody').on('click', '.phone', function () {
            //    // $(this).popover();
            //    // $('[data-toggle="popover"]').popover();

            //    //var tr = $(this).closest('tr');
            //    //var row = dataTable.row(tr);
            //    //alert(row.data().phone);

            //});

            $('#tblSearch tbody').on('click', 'tr td.edit', function () {
                //var tr = $(this).closest('tr');
                //var row = dataTable.row(tr);
                ////var idx = $.inArray(tr.attr('id'), detailRows);
                //$('#myModal').modal();
                //alert(row.data().PNR);
            });



            $("#txtSearchBox").on("keyup search input paste cut", function () {
                dataTable.search(this.value).draw();
            });

            if (search != '') {
                $('#txtSearchBox').val(search);
                dataTable.search(search).draw();
            }

        });


    </script>


    <script>

        //var detailRows = [];

        //$('#tblSearch tbody').on('click', 'tr td.details-control', function () {

        //    var tr = $(this).closest('tr');
        //    var row = dataTable.row(tr);
        //    var idx = $.inArray(tr.attr('id'), detailRows);



        //    if (row.child.isShown()) {
        //        tr.removeClass('details');
        //        $(tr).css('color', 'black')
        //        row.child.hide();

        //        // Remove from the 'open' array
        //        detailRows.splice(idx, 1);
        //    }
        //    else {
        //        tr.addClass('details');
        //        // row.child(row.data().phone).show();
        //        $(tr).css('color', 'red');

        //        row.child('Are you sure you want to cancel?').show();
        //        var text = $('<b>').html('Are you sure you want to cancel?')
        //        row.child(text);


        //        // Add to the 'open' array
        //        if (idx === -1) {
        //            detailRows.push(tr.attr('id'));
        //        }
        //    }
        //});

        // On each draw, loop over the `detailRows` array and show any child rows
        //dataTable.on('draw', function () {
        //    $.each(detailRows, function (i, id) {
        //        $('#' + id + ' td.details-control').trigger('click');
        //    });
        //});


        //function format(d) {
        //    return 'Full name: ' + d.first_name + ' ' + d.last_name + '<br>' +
        //        'Salary: ' + d.salary + '<br>' +
        //        'The child row can contain any data you wish, including links, images, inner tables etc.';
        //}

    </script>
</asp:Content>
