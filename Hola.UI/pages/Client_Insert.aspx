<%@ Page Title="Insert a New Reservation" Language="C#" MasterPageFile="~/MasterHola.master" AutoEventWireup="true" CodeFile="Client_Insert.aspx.cs" Inherits="pages_Client_Insert" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-lg-12">
            <h1 class="page-header"><i id="test" class="fa fa-plus-circle fa-fw"></i>Insert a New Reservation</h1>
        </div>
        <!-- /.col-lg-12 -->
    </div>
    <form id="form_client" name="form1" runat="server">
        <div class="row">
            <div class="col-lg-12">
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
                                                <select class="form-control margin-bottom-14" required="required" data-bind="
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

                                            <div class="form-group " <%-- data-bind="css: { 'has-warning': PNR() == '' } "--%>>
                                                <label class="control-label">PNR*</label>

                                                <input data-bind="value: PNR" id="txtPNR" required="required" class="form-control" placeholder="PNR" />
                                            </div>

                                            <div class="form-group ">
                                                <label class="control-label">PAX*</label>
                                                <input type="number" data-bind="value: PAX" style="width: 60px;" min="1" max="99" id="txtPAX" required="required" class="form-control" />

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
                                                <%--                                                <label style="font-weight: normal; margin-left: 5px">

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

                                        <textarea data-bind="text: comments" id="txtComments" form="form_client" class="form-control">
                                        </textarea>
                                    </div>



                                </div>


                                <div class="col-lg-12">
                                    <div class="panel panel-info">
                                        <div class="panel-heading">
                                            <i class="fa fa-bus  fa-fw"></i>
                                            Transportation
                                        </div>
                                        <!-- /.panel-heading -->
                                        <div class="panel-body form-inline">

                                            <div class="form-group ">
                                                <label class="control-label">Transportation*</label>
                                                <select class="form-control margin-bottom-14" required="required" data-bind="
    options: products_trans,
    optionsText: 'name',
    optionsValue: 'ID',
    value: trans_product_fk,
    optionsCaption: 'Select Transportation',
    valueAllowUnset: true
">
                                                </select>

                                            </div>



                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <button id="btn_insert" data-bind="click: insert_client" class="btn btn-primary" type="button">Insert</button>
                                        <button id="btn_reset" type="reset" class="btn btn-default">Reset</button>
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
                                        <div id="message_success" style="display: none" class="alert alert-success alert-dismissable">
                                            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                                            The client: <b id="new_pnr" data-bind="text: names"></b> &nbsp;  was successfuly added.  <a data-bind="    attr: { href: 'Client_Update.aspx?search=' + PNR() }" href="Client_Sales.aspx" target="_blank" class="alert-link">Click here to add service</a>.
                           
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
                <!-- /.panel -->
            </div>
            <!-- /.col-lg-12 -->
        </div>

    </form>




</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="FooterScripts" runat="Server">


    <script src="../views_client/view_client_insert.js"></script>


    <script type="text/javascript">
        var my = { viewModel: new ReservationViewModel() };
        ko.applyBindings(my.viewModel);

        $(document).ready(function () {

            $(".date").datepicker({ dateFormat: 'yy-mm-dd', minDate: -30 });

            //$('#cbIsOw').change(function () {
            //    if (this.checked) {
            //        $('#div_dep').slideUp();
            //        $('#txtDateDep').removeAttr('required', 'required');
            //        $('#ddlFlightDep').removeAttr('required', 'required');

            //    }
            //    else {
            //        $('#div_dep').slideDown();
            //        $('#txtDateDep').attr('required', 'required');
            //        $('#ddlFlightDep').attr('required', 'required');
            //    }
            //});

            //$('#form_client').validate();

            $('#btn_reset').click(function () {
                $('#div_dep').slideDown();
                $('#txtComments').text('');
            });




            $('#ddlFlightArr').change(function () {
                var selected_arr = $('option:selected', this).text();
                $('#hdnSelectedArr').val(selected_arr);
            });

            $('#ddlFlightDep').change(function () {
                var selected_arr = $('option:selected', this).text();
                $('#hdnSelectedDep').val(selected_arr);
            });

            $('._date').change(function () {



                var filter = $(this).val();
                var count = 0;
                var original_div = $(this).parent();
                var new_div = original_div.siblings().first();
                var select_flight = new_div.children('select');
                var ddlID = select_flight.attr('id');

                count_ready++;
                count_array[ddlID]++;
                if (count_array[ddlID] > 1) {
                    $('select#' + ddlID).append($(cache_array[ddlID]));

                }


                $('option', select_flight).each(function (i) {

                    if (i != 0) {

                        var op_val = ($(this).val());
                        var op_date = new Date(op_val);
                        var _date = $.datepicker.formatDate('yy-mm-dd', op_date);

                        if (_date == filter) {
                            count++;
                        } else {
                            $(this).remove();
                        }
                    }
                });

                if (count == 0) {

                    $(select_flight).attr('disabled', 'disabled');
                    $(original_div).addClass('has-warning');
                    $('#div_warning').show('highlight');

                    $('select#' + ddlID + ' option').first().text('NO FLIGHTS ON ' + filter);
                }
                else {
                    $(select_flight).removeAttr('disabled');
                    $(original_div).removeClass('has-warning');
                    $('#div_warning').hide('highlight');

                    $('select#' + ddlID + ' option').first().text('SELECT FLIGHT');
                    $('select#' + ddlID).val('0');
                }

            });

            var count = 0;
            $('#test').click(function () {
                count++;
                if (count % 3 == 0) {
                    var pnr = Math.floor((Math.random() * 10000));
                    var pax = Math.floor((Math.random() * 5) + 1);
                    var hotel = Math.floor((Math.random() * 5) + 1);
                    var agency = Math.floor((Math.random() * 4) + 1);
                    var names = 'moshe';

                    my.viewModel.PNR(pnr);
                    my.viewModel.names(names);
                    my.viewModel.PAX(pax);
                    my.viewModel.phone('123456');
                    my.viewModel.num_arr('LY101');
                    my.viewModel.date_arr('2016-10-01');
                    my.viewModel.num_dep('LY201');
                    my.viewModel.date_dep('2016-10-05');
                    my.viewModel.hotel_fk(hotel);
                    my.viewModel.agency_fk(agency);
                    my.viewModel.trans_product_fk(1);
                    my.viewModel.comments('bla bla bla');

                }
            });

        });




    </script>

</asp:Content>
