<%@ Page Title="Flights" Language="C#" MasterPageFile="~/MasterHola.master" AutoEventWireup="true" CodeFile="Flights.aspx.cs" Inherits="pages_Flights" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .form-control-feedback {
            width: 70px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-lg-12">
            <h1 class="page-header"><i class="fa fa-plane fa-fw"></i>Flights</h1>
        </div>
    </div>

    <div class="panel-heading">
        <h2 class="panel-title">Add a New Flight</h2>
    </div>

    <div class="panel-body">

        <div class="form-group">
            <label class="col-sm-2 control-label">Flight #</label>
            <div class="col-sm-10">
                <input type="text" required="required" class="form-control" data-bind="value: new_num" placeholder="e.g. VY7844" />

            </div>
        </div>
        <div class="form-group has-feedback">
            <label class="col-sm-2 control-label">Date</label>
            <div class="col-sm-10">
                <input type="text" required="required" class="form-control date" data-bind="value: new_date" placeholder="Date" />
                <span class="glyphicon  form-control-feedback" title="Please use only the format yyyy-MM-dd"></span>

            </div>

        </div>
        <div class="form-group">
            <label class="col-sm-2 control-label">Time</label>
            <div class="col-sm-10">
                <input type="time" required="required" class="form-control" data-bind="value: new_time" placeholder="Time" />
            </div>
        </div>
        <div class="form-group">
            <label class="col-sm-2 control-label">Destination</label>
            <div class="col-sm-10">
                <label class="radio-inline">
                    <input required="required" type="radio" value="BCN" data-bind="checked: new_destination" name="optradio" />Barcelona</label>
                <label class="radio-inline">
                    <input required="required" type="radio" value="TLV" data-bind="checked: new_destination" name="optradio" />Tel Aviv</label>
                <label class="radio-inline">
                    <input required="required" type="radio" value="Valencia" data-bind="checked: new_destination" name="optradio" />Valencia</label>
            </div>
        </div>
        <div class="form-group">
            <label class="col-sm-2 control-label" style="clear: both">Direction</label>
            <div class="col-sm-10">
                <input type="text" class="form-control" disabled="disabled" data-bind="value: new_direction" placeholder="Direction" />
            </div>
            <div class="col-sm-10" style="margin-top: 10px; margin-bottom: 10px;">
                <button type="button" data-bind="click: add_server" class="btn btn-primary">Submit</button>
            </div>
        </div>

    </div>

    <table class="table table-striped table-bordered table-hover" id="tbl">
        <thead>
            <tr>
                <th style="width: 100px"></th>
                <th>Flight #</th>
                <th>Date</th>
                <th>Time</th>
                <th>Destination</th>
            </tr>
            <tr id="trLoading" style="display: inline-grid">
                <td colspan="7" style="text-align: center;">Loading...</td>
            </tr>
        </thead>
        <tbody data-bind="foreach: flights">
            <tr>
                <td>
                    <button data-bind="click: $parent.edit_mode, text: editBtnText, css: editBtnClass"></button>
                    <button class="btn btn-danger btn-circle" title="Delete Plan" data-bind="click: $parent.DeleteFlight">Del</button>
                </td>
                <td data-bind="text: num"></td>
                <td data-bind="text: date"></td>
                <td>
                
                    <span data-bind="text: time, visible: !(editable())"></span>
                    <input class="form-control" type="time" data-bind="value: time, visible: editable" style="width: 120px" />
                </td>
                <td data-bind="text: destination"></td>
            </tr>
        </tbody>
    </table>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="FooterScripts" runat="Server">


    <script src="../views_client/view_flight.js"></script>


    <script>

        $(document).ready(function () {
            $(".date").datepicker({ dateFormat: 'yy-mm-dd', minDate: 0 });


            //$('#tbl').DataTable({

            //    //"columnDefs": [
            //    //    { "visible": false, "targets": 0 }
            //    //],


            //    "paging": false,
            //    "sorting": true,
            //    "searching": true,
            //    "info": false,
            //    //dom: 'Bfrtip',
            //    //buttons: [
            //    //    'excel','copy', 'csv', 'pdf',  'print'
            //    //]
            //});
        });













    </script>
</asp:Content>
