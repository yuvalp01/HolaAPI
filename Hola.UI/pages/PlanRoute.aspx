<%@ Page Title="Plan Departure Pickup Route" Language="C#" MasterPageFile="~/MasterHola.master" AutoEventWireup="true" CodeFile="PlanRoute.aspx.cs" Inherits="pages_PlanRoute" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .loader {
            border: 6px solid #f3f3f3;
            border-radius: 50%;
            border-top: 6px solid #3498db;
            width: 40px;
            height: 40px;
            -webkit-animation: spin 2s linear infinite;
            animation: spin 2s linear infinite;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="row">
        <div class="col-lg-12">
            <h1 class="page-header"><i class="fa fa-clock-o fa-fw"></i>Plan Departure Pickup Route</h1>
        </div>
    </div>


    <div class="col-md-12">
        <h3>List <span class="badge" data-bind="    text: event_fk_selected"></span></h3>

        <table id="tbl_plans" class="table table-striped table-bordered table-hover">
            <thead>
                <tr>
                    <th>Hotel</th>
                    <th>PAX</th>
                    <th>Time <a href="#" id="lnk_sort" class="btn-circle" style="display: none; width: 80px" data-bind="click: GetDepartPlan">Sort By Time</a></th>

                </tr>
            </thead>

            <tbody data-bind="foreach: { data: plans }">
                <tr>
                    <td data-bind="text: hotel_name"></td>
                    <td data-bind="text: PAX"></td>
                    <td>
                        <span data-bind="text: time, visible: !(editable())"></span>
                        <div class="form-group">
                            <input required="required" class="form-control" type="time" data-bind="value: time, visible: editable, event: { blur: $parent.updateTime }" />
                        </div>
                        <button class="btn btn-circle btn-info" data-bind="click: $parent.edit_mode, visible: !(editable())">Edit</button>

                    </td>
                </tr>
            </tbody>
        </table>

        <div id="divSuccess" data-bind="visible: result() == 'success'" class="alert alert-success" role="alert">
            The plan has been uccessfully saved.   <a href="EventsTrans.aspx">Return</a>

        </div>
        <div class="form-group">
            <button id="btnSave" class="btn btn-primary nextBtn btn-lg pull-right" data-bind="click: UpdateDepartEventTime" type="button">Save </button>

        </div>



    </div>




</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="FooterScripts" runat="Server">

    <script>
        var DIRECTION = 'OUT';

    </script>
    <script src="../views_client/view_transport_wizard.js"></script>
    <script>
        var my = { viewModel: new FlightViewModel() };
        ko.applyBindings(my.viewModel);
        my.viewModel.event_fk_selected(<%=event_fk%>);
        my.viewModel.GetDepartPlan();




        $(document).ready(function () {
            $(".date").datepicker({ dateFormat: 'yy-mm-dd', minDate: -30 });


        });

    </script>
</asp:Content>
