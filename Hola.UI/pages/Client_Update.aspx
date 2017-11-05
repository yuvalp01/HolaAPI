<%@ Page Title="Update Reservation" Language="C#" MasterPageFile="~/MasterHola.master" AutoEventWireup="true" CodeFile="Client_Update.aspx.cs" Inherits="pages_Client_Update" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        #tblClients_filter {
            display: none;
        }

        .ui-dialog-titlebar, ui-widget-header {
            background-color: #d9edf7 !important;
            background-image: none !important;
        }

        img.edit {
            margin-right: 6px;
            margin-left: 3px;
        }

        img.trans {
            margin-right: 10px;
        }

        #tbl_sales_wrapper {
            margin: 3px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-lg-12">
            <h1 class="page-header"><i class="fa fa-pencil  fa-fw"></i>Update Reservation</h1>
        </div>
    </div>

    <div class="row">

        <div id="modal_edit" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-admin " role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span>&times;</span></button>
                        <h4 class="modal-title"></h4>
                    </div>
                    <div class="modal-body">
                        <div class="panel panel-default">
                            <div class="panel-body">
                                <div class="row">
                                    <div class="col-lg-8" style="padding-left: 0px;">
                                        <div class="col-lg-12">
                                            <div class="panel panel-info">
                                                <div class="panel-heading">
                                                    <i class="fa fa-pencil-square-o  fa-fw"></i>
                                                    General Info
                                                </div>
                                                <!-- /.panel-heading -->
                                                <div class="panel-body form-inline">

                                                    <div class="form-group ">
                                                        <label class="control-label">Agency*</label>
                                                        <select disabled="disabled" class="form-control margin-bottom-14" required="required" data-bind="
    options: agencies,
    optionsText: 'name',
    optionsValue: 'ID',
    value: agency_fk,
    optionsCaption: 'Select Agency',
    valueAllowUnset: true
">
                                                        </select>

                                                    </div>


                                                    <div class="form-group">
                                                        <label class="control-label">Hotel*</label>
                                                        <select class="form-control margin-bottom-14" required="required" data-bind="
    options: hotels,
    optionsText: 'name',
    optionsValue: 'ID',
    value: hotel_fk,
    optionsCaption: 'Select Hotel',
    valueAllowUnset: true
">
                                                        </select>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div id="personal_info" class="col-lg-7">
                                            <div class="panel panel-info">
                                                <div class="panel-heading">
                                                    <i class="fa fa-user fa-fw"></i>
                                                    Personal Info
                       
                                                </div>

                                                <div class="panel-body ">

                                                    <div class="form-group">
                                                        <label class="control-label">Client Names*</label>
                                                        <input data-bind="value: names" id="txtNames" required="required" class="form-control" placeholder="Client Names" />

                                                    </div>

                                                    <div class="form-group">
                                                        <label class="control-label">PNR*</label>

                                                        <input data-bind="value: PNR" id="txtPNR" readonly="readonly" required="required" class="form-control" placeholder="PNR" />
                                                    </div>

                                                    <div class="form-group ">
                                                        <label class="control-label">PAX*</label>

                                                        <input type="number" data-bind="value: PAX" style="width: 50px;" min="1" max="99" id="txtPAX" required="required" class="form-control" />

                                                    </div>

                                                    <div class="form-group">
                                                        <label class="control-label">Phone Number</label>
                                                        <input type="tel" data-bind="value: phone" id="txtPhone" class="form-control" placeholder="Phone Number" />

                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div id="flight_info" class="col-lg-5">
                                            <div class="panel panel-info">
                                                <div class="panel-heading">
                                                    <i class="fa fa-plane fa-fw"></i>
                                                    Flights Info
                       
                                                </div>
                                                <!-- /.panel-heading -->
                                                <div class="panel-body ">

                                                    <div class="form-group">
                                                        <label class="control-label">Arrival Date*</label>
                                                        <%--                                                        <label style="font-weight: normal; margin-left: 5px">

                                                            <input data-bind="checked: oneway" id="cbIsOw" type="checkbox" />
                                                            One Way
                                                        </label>--%>

                                                        <input id="txtDateArr" required="required" class="form-control date" data-bind="value: date_arr" placeholder="Pick Arrival Date" />
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="control-label">Arrival Flight*</label>

                                                        <select id="ddlFlightArr" required="required" class="form-control margin-bottom-14" data-bind=" click: function () { $root.arrival_validation(); },

    options: flights_filter_arr,
    optionsText: 'details',
    optionsValue: 'num',
    value: num_arr,
    optionsCaption: 'Select Arrival Date',
    valueAllowUnset: true
">
                                                        </select>

                                                    </div>
                                                    <div id="div_dep">
                                                        <div class="form-group">
                                                            <label class="control-label">Departure Date*</label>
                                                            <input id="txtDateDep" required="required" class="form-control date" data-bind="value: date_dep" placeholder="Pick Departure Date" />
                                                        </div>
                                                        <div class="form-group">
                                                            <label class="control-label">Departure Flight*</label>


                                                            <select id="ddlFlightDep" required="required" class="form-control margin-bottom-14" data-bind="click: function () { $root.departure_validation(); },

    options: flights_filter_dep,
    optionsText: 'details',
    optionsValue: 'num',
    value: num_dep,
    optionsCaption: 'Select Departure Date',
    valueAllowUnset: true
">
                                                            </select>
                                                        </div>

                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-lg-12">

                                            <div class="form-group">
                                                <label>Comments</label>

                                                <textarea data-bind="value: comments" id="txtComments" class="form-control"></textarea>
                                            </div>
                                            <div class="form-group">
                                                <button data-bind="click: $root.UpdateClient" id="btn_update" class="btn btn-primary" type="button">Save</button>
                                                <button data-bind="click: $root.cancel_client" id="btn_delete" class="btn btn-danger" type="button">Delete</button>
                                                <button id="btn_close" class="btn btn-default" type="button">Close</button>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-4">
                                        <div class="panel panel-default">
                                            <div class="panel-heading">
                                                System Feedback
                       
                                            </div>
                                            <!-- /.panel-heading -->
                                            <div id="system_feedback" class="panel-body">
                                                <div id="message_info" style="display: none;" class="alert alert-info  alert-dismissable">
                                                    <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                                                    <%--                                            Lorem ipsum dolor sit amet, consectetur adipisicing elit. <a href="#" class="alert-link">Alert Link</a>.--%>
                                                </div>
                                                <div id="message_success" style="display: none;" class="alert alert-success alert-dismissable">
                                                    <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>

                                                </div>
                                                <div id="message_warning" style="display: none" class="alert alert-warning alert-dismissable">
                                                    <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>


                                                </div>
                                                <div id="message_danger" style="display: none" class="alert alert-danger alert-dismissable">
                                                    <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>


                                                </div>
                                            </div>
                                            <!-- .panel-body -->
                                        </div>
                                        <!-- /.panel -->
                                    </div>

                                    <!-- /.col-lg-6 (nested) -->
                                </div>
                                <!-- /.row (nested) -->
                            </div>
                            <!-- /.panel-body -->
                        </div>
                    </div>

                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>



        <div id="modal_trans" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title"></h4>
                    </div>
                    <div class="modal-body">
                        <form class="form-inline panel panel-default" style="padding: 5px">

                            <div class="row">






                                <%--                                <div class="col-lg-6" data-bind="foreach: { data: products_trans }">
                                    <div class="input-group col-lg-12">
                                        <span class="input-group-addon">
                                            <input type="radio" data-bind="attr: { id: 'rb_' + ID(), value: ID, 'data-rate': rate }, checked: $root.trans_product_fk" name="trans_type" />

                                        </span>
                                        <input data-bind="value: name() + ' (Rate:' + rate() + ')'" readonly="readonly" class="form-control" />
                                    </div>
                                </div>--%>


                                <div class="col-lg-6 ">
                                    <div class="form-group">
                                        <select id="ddlTrans" required="required" class="form-control margin-bottom-14" style="display: inline; width: 555px" data-bind="
    options: products_trans,
    optionsText: 'name',
    optionsValue: 'ID',
    value: trans_product_fk,
    optionsCaption: 'Select Transportation',
    valueAllowUnset: true
    ">
                                        </select>
                                    </div>



                                    <%--                                    <div class="form-group">
                                        <label class="control-label">Current Price</label>
                                        <input class="form-control" type="number" style="width: 70px" data-bind="value: trans_remained_pay" min="0" step="1" />
                                    </div>--%>

                                    <div class="form-group" style="width: auto">
                                        <label class="control-label">Comments</label>
                                        <br />
                                        <textarea class="form-control" style="width: 555px" data-bind="value: trans_comments"></textarea>
                                    </div>

                                    <div>
                                        <span id="lblFeedback_trans" data-bind="text: feedback_trans" style="display: block; color: red" class="has-warning"></span>
                                    </div>

                                </div>
                            </div>

                        </form>
                    </div>

                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                        <input class="btn btn-primary" data-bind="click: update_transport" type="button" value="Update" />
                    </div>
                </div>
            </div>
        </div>




        <div id="modal_sale" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title"></h4>
                    </div>
                    <div class="modal-body">
                        <form class="form-inline panel panel-default  well" style="padding: 5px; margin-bottom: 0px;">
                            <div class="form-group">
                                <label class="control-label">Tour:</label>
                                <select id="ddlTours" required="required" class="form-control margin-bottom-14" style="display: inline; width: 200px" data-bind="
    options: products_tour,
    optionsText: 'name',
    optionsValue: 'ID',
    value: product_fk,
    optionsCaption: 'Select Tour',
    valueAllowUnset: true
">
                                </select>
                            </div>
                            <div class="form-group">
                                <label class="control-label">People</label>
                                <input type="number" style="width: 60px" class="form-control" data-bind="value: persons, attr: { max: persons_max }" min="1" id="txtP" />
                            </div>
                            <div id="rblType" class="form-group">

                                <label class="radio-inline" data-container="body" data-toggle="tooltip" data-placement="top" title="External - pre-order by agency">
                                    <input type="radio" name="xxx" required data-bind="checked: sale_type" value="external" />External</label>
                                <label class="radio-inline" data-container="body" data-toggle="tooltip" data-placement="top" title="Internal - order directly by the client">
                                    <input type="radio" name="xxx" required data-bind="checked: sale_type" value="internal" />Internal</label>
                            </div>

                            <div id="divPayment" class="collapse" style="margin-top: 5px">
                                <div class="form-group ">
                                    <label class="control-label">Total Price:</label>
                                    <input type="number" step="any" min="0" required="required" style="width: 70px" class="form-control" data-bind="value: calc_total" />

                                    <span data-bind="visible: rate">Price per Person  <span data-bind="    text: rate" class="badge"></span></span>



                                </div>

                            </div>

                            <div class="form-group">

                                <textarea placeholder="Comments" style="width: 555px; margin-top: 5px; margin-bottom: -5px" data-bind="textInput: sale_comments" class="form-control"></textarea>



                            </div>

                            <div class="form-group has-error has-feedback">
                                <div>
                                    <span id="lblFeedback" data-bind="text: feedback_sale" style="display: block; color: red" class="has-warning"></span>
                                </div>
                            </div>
                        </form>
                    </div>

                    <div data-bind="visible: sales().length == 0" class="alert alert-info" role="alert" style="margin-left: 14px; margin-right: 14px;">
                        There are no tours reserved for this reservation
                    </div>


                    <table data-bind="visible: sales().length > 0" id="tbl_sales" class="table table-striped table-bordered table-hover order-column compact" style="width: 98%; margin-left: 5px;">
                        <thead>
                            <tr>
                                <th>Tour</th>
                                <th>Persons</th>
                                <th>Type</th>
                                <%-- <th style="width: 100px" title="Balance Remaining of the client to Hola Shalom">Remained</th>--%>
                                <th style="width: 200px">Comments</th>
                                <th style="width: 100px"></th>

                            </tr>
                        </thead>

                        <tbody data-bind="foreach: { data: sales }">
                            <tr>
                                <td data-bind="text: product_name"></td>
                                <td>
                                    <span data-bind="text: persons, visible: !(editable())"></span>
                                    <input class="form-control" type="number" step="any" min="1" data-bind="attr: { 'max': $parent.persons }, value: persons, visible: editable" style="width: 60px" />

                                </td>
                                <td data-bind="text: sale_type"></td>
                                <%--                                <td>
                                    <span data-bind="text: remained_pay() + ' €', visible: !(editable())"></span>
                                    <input class="form-control" type="number" step="any" min="0" data-bind="value: remained_pay, visible: editable" style="width: 70px" />
                                </td>--%>
                                <td>
                                    <span data-bind="text: comments, visible: !(editable())"></span>
                                    <textarea class="form-control" data-bind=" value: comments, visible: editable"></textarea>
                                </td>
                                <td>
                                    <button data-bind="click: $parent.edit_mode, text: editBtnText, css: editBtnClass"></button>
                                    <button class="btn btn-danger btn-circle" title="Delete Sale" data-bind="click: $parent.cancel">Del</button>
                                </td>


                            </tr>
                        </tbody>
                    </table>




                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                        <input id="btnAdd" class="btn btn-primary" data-bind="click: add" type="button" value="Add" />
                    </div>
                </div>
            </div>
        </div>
    </div>


    <div class="row">


        <%--        <div id="div_advanced" style="display: block" class="well">
            <label>Search Options:</label>

            <label>
                <input id="txtSearchBox" data-bind="textInput: search_term_filter" type="search" class="form-control  margin-bottom-14" placeholder="Search" />
            </label>
            <select id="ddlAgency" class="form-control margin-bottom-14" style="display: inline; width: 180px" data-bind="


    options: agencies,
    optionsText: 'name',
    optionsValue: 'ID',
    value: agency_fk_filter,
    optionsCaption: 'Show All Agencies',
    valueAllowUnset: true
">
            </select>

            <input placeholder="Arrival Date" data-bind="value: date_arr_filter" type="text" style="display: inline; width: 200px" class="date form-control margin-bottom-14" />
            <input placeholder="Departure Date" data-bind="value: date_dep_filter" type="text" style="display: none; width: 200px" class="date form-control margin-bottom-14" />


        </div>
        --%>


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
    value: agency_name_search,
    optionsCaption: 'Show All Agencies',
    valueAllowUnset: true
">
            </select>

            <%--            <input id="txtSearchBox" type="text" style="display: inline; width: 200px" class="form-control margin-bottom-14" placeholder="Free text" />--%>

            <select id="ddlHotel" class="form-control margin-bottom-14" style="display: inline; width: 180px" data-bind="


    options: hotels,
    optionsText: 'name',
    optionsValue: 'name',
    value: hotel_name_search,
    optionsCaption: 'Show All Hotels',
    valueAllowUnset: true
">
            </select>
            <input id="txtDateArrSearch" type="text" style="display: inline; width: 200px" class="form-control margin-bottom-14 date" placeholder="Arrival Date" data-bind="value: date_arr_search" />
            <input id="txtDateDepSearch" type="text" style="display: inline; width: 200px" class="form-control margin-bottom-14 date" placeholder="Departure Date" data-bind="value: date_dep_search" />


        </div>


        <table class="table table-striped table-bordered table-hover order-column compact" id="tblClients">
            <thead>
                <tr>
                    <th style="width: 110px">Action</th>
                    <th>PNR</th>
                    <th>Names</th>
                    <th>PAX</th>
                    <th>Arrival#</th>
                    <th>Arrival Date</th>
                    <th>Dep.#</th>
                    <th>Dep. Date</th>
                    <th>Hotel</th>
                    <th>Agency</th>
                    <th>Phone</th>
                </tr>
                <%--                <tr id="trLoading" style="display: inline-grid">
                    <td colspan="7" style="text-align: center;">Loading...</td>

                </tr>--%>
            </thead>

            <tbody data-bind="foreach: { data: clients }">
                <tr>
                    <td>
                        <img class="edit" style="cursor: pointer" title="Edit Reservation" src="../icons/fa-pencil.png" />
                        <img class="trans" style="cursor: pointer" title="Transportation" src="../icons/fa-train.png" />
                        <img class="sale" style="cursor: pointer" title="Tours" src="../icons/tourguide.png" />

                    </td>
                    <td data-bind="text: PNR"></td>
                    <td data-bind="text: names"></td>
                    <td data-bind="text: PAX"></td>
                    <td data-bind="text: num_arr"></td>
                    <td data-bind="text: date_arr"></td>
                    <td data-bind="text: num_dep"></td>
                    <td data-bind="text: date_dep"></td>
                    <td data-bind="text: hotel_name"></td>
                    <td data-bind="text: agency_name"></td>
                    <td data-bind="text: phone"></td>

                </tr>
            </tbody>
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

    <script src="../views_client/view_client_update.js"></script>

    <script>

        var my = { viewModel: new SaleViewModel() };
        ko.applyBindings(my.viewModel);
        var dataTable;
        var search = '<%=Search%>';
        $(document).ready(function () {
            my.viewModel.search_term_filter(search);

            $(".date").datepicker({ dateFormat: 'yy-mm-dd' });

            $('#lnkAdvanced').click(function myfunction() {

                $('#div_advanced').toggle('slow');

            });
            my.viewModel.get_clients();

            //dataTable = $('#tblClients').DataTable({
            //    "drawCallback": function (settings) {
            //        $('[data-toggle="popover"]').popover();
            //    },
            //    responsive: true,
            //    //"ajax": url_clients,
            //    "data": my.viewModel.clients(),
            //    "pageLength": 10,
            //    "bLengthChange": false,
            //    "sAjaxDataProp": "",
            //    "autoWidth": false,
            //    //"columnDefs": [{
            //    //    "targets": col_names,
            //    //    "searchable": false,
            //    //    "sortable": false,
            //    //    "width": "30px"
            //    //}],
            //    "columns": [
            //                { "data": "PNR", 'visible':false },//1
            //                { "data": "PNR" },//1
            //                { "data": "names", "width": "20px" },//2
            //                { "data": "PAX", "width": "40px" },//3
            //                { "data": "num_arr", "width": "100px" },//4
            //                { "data": "date_arr", "width": "110px" },//5
            //                { "data": "num_dep", "width": "70px" },//6
            //                { "data": "date_dep", "width": "110px" },//7
            //                { "data": "hotel_name", "width": "110px" },//8
            //                { "data": "agency_name", "width": "110px" },//9
            //                { "data": "phone", 'visible': false },//9
            //    ],
            //    "columnDefs": [
            //                {
            //                    "targets": col_names,
            //                    "render": function (data, type, row) {
            //                        if (row.phone != "") {
            //                            return data + ' <a class="phone" data-toggle="popover" role="button" data-trigger="focus" tabindex="0"  title="Phone Number" data-content="' + row.phone + '"><i class="fa fa-phone-square fa-lg"></i></a>';
            //                        }
            //                        else {
            //                            return data;
            //                        }
            //                    },
            //                },
            //                {
            //                    "render": function (data, type, row) {
            //                        return new Date(data).yyyymmdd();
            //                    },
            //                    "targets": col_date_arr
            //                },
            //                {
            //                    "render": function (data, type, row) {
            //                        if (data) {
            //                            return new Date(data).yyyymmdd();
            //                        } else { return data; }
            //                    },
            //                    "targets": col_date_dep
            //                }
            //    ],
            //    dom: 'frtipB',
            //    buttons: [
            //            //'copy',
            //            { extend: 'excel', text: '<i class="fa fa-file-excel-o fa-fw"></i><span>Export Table</span>' },
            //            //'pdf',
            //            //'print'
            //    ],
            //    "drawCallback": function (settings) {
            //        $('[data-toggle="popover"]').popover();
            //    },
            //});


            $("#txtSearchBox").on("keyup search input paste cut", function () {
                dataTable.search(this.value).draw();
            });

            if (search != '') {
                $('#txtSearchBox').val(search);
                dataTable.search(search).draw();
            }


            $('#btn_close').click(function () {

                $('#modal_edit').modal('hide');
            });


            $('#tblClients tbody').on('click', '.sale', function () {
                $(".form-group").removeClass("has-error");
                $('#modal_sale').modal('show');

                var client = ko.dataFor(this);
                my.viewModel.PNR(client.PNR());
                my.viewModel.agency_fk(client.agency_fk());
                my.viewModel.persons(client.PAX());
                my.viewModel.persons_max(client.PAX());
                /////TODO: new version uncomment:
                //my.viewModel.sale_type('');
                /////
                my.viewModel.load_sales();
                $('.modal-title').text("[" + client.PNR() + "] " + client.names())
            });

            $('#tblClients tbody').on('click', '.trans', function () {
                $(".form-group").removeClass("has-error");
                $('#modal_trans').modal('show');

                var client = ko.dataFor(this);
                my.viewModel.PNR(client.PNR());
                my.viewModel.agency_fk(client.agency_fk());

                my.viewModel.load_trans();
                $('.modal-title').text("[" + client.PNR() + "] " + client.names())
            });



            $('#tblClients tbody').on('click', '.edit', function () {
                $(".form-group").removeClass("has-error");
                $('#btn_update').show();
                $('#btn_delete').show();
                $('#modal_edit').modal('show');
                //var tr = $(this).closest('tr');
                var client = ko.dataFor(this);
                my.viewModel.load_client(client);

                if (client.oneway()) {
                    $('#div_dep').slideUp();
                    $('#txtDateDep').removeAttr('required', 'required');
                    $('#ddlFlightDep').removeAttr('required', 'required');
                }
                $('.modal-title').text("[" + client.PNR() + "] " + client.names())
            });

        });


    </script>
</asp:Content>
