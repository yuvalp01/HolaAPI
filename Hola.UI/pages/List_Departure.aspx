<%@ Page Title="Departure List" Language="C#" MasterPageFile="~/MasterHola.master" AutoEventWireup="true" CodeFile="List_Departure.aspx.cs" Inherits="pages_List_Departure" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
        <script>
        var isChrome = !!window.chrome && !!window.chrome.webstore;
        if (!isChrome) {
            window.location = "UseChrome.aspx";
        }
    </script>
    <link href="../Content/wizard.css" rel="stylesheet" />
    <style>
        tr.mark {
            background-color: #08C !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-lg-12">
            <h1 class="page-header"><i class="fa fa-sign-in fa-fw"></i>Departure List</h1>
        </div>
    </div>

    <div class="panel panel-default">

        <div class="panel-heading">
            <h3 class="panel-title">Follow the steps to create an departure list</h3>

        </div>
        <div class="panel-body">

            <div class="container" style="width: auto">
                <div class="stepwizard">
                    <div class="stepwizard-row setup-panel">
                        <div class="stepwizard-step">
                            <a href="#step-1" type="button" class="btn btn-primary btn-circle">1</a>
                            <p>Step 1</p>
                        </div>
                        <div class="stepwizard-step">
                            <a href="#step-2" type="button" class="btn btn-default btn-circle" disabled="disabled">2</a>
                            <p>Step 2</p>
                        </div>
                        <div class="stepwizard-step">
                            <a href="#step-3" type="button" class="btn btn-default btn-circle" disabled="disabled">3</a>
                            <p>Step 3</p>
                        </div>
                        <div class="stepwizard-step">
                            <a href="#step-4" type="button" class="btn btn-default btn-circle" disabled="disabled">4</a>
                            <p>Step 4</p>
                        </div>
                        <div class="stepwizard-step">
                            <a href="#step-5" type="button" class="btn btn-default btn-circle" disabled="disabled">5</a>
                            <p>Step 5</p>
                        </div>
                        <div class="stepwizard-step">
                            <a href="#step-6" type="button" class="btn btn-default btn-circle" disabled="disabled">6</a>
                            <p>Step 6</p>
                        </div>
                    </div>
                </div>
                <form id="list_wizard" role="form">
                    <div class="row setup-content" id="step-1">
                        <div class="col-xs-12">
                            <div class="col-md-12">
                                <h3>Departure Date</h3>
                                <div class="form-group">
                                    <input id="txtDateStrat" data-bind="value: date_start" required="required" type="text" class="date form-control" style="width: 250px" placeholder="Select Departure Date" />

                                </div>

                                <button class="btn btn-primary nextBtn btn-lg pull-right" type="button">Next</button>
                            </div>
                        </div>
                    </div>
                    <div class="row setup-content" id="step-2">
                        <div class="col-xs-12">
                            <div class="col-md-12">


                                <h3>Route</h3>
                                <div class="form-group">
                                    <select required="required" id="ddlActivities" style="width: 250px" class="form-control margin-bottom-14" data-bind="
    options: activities,
    optionsText: 'name',
    optionsValue: 'ID',
    value: activity_fk,
    optionsCaption: 'Select Route',
    valueAllowUnset: true
">
                                    </select>
                                </div>
                                <button class="btn btn-primary nextBtn btn-lg pull-right" type="button">Next</button>
                            </div>
                        </div>
                    </div>
                    <div class="row setup-content" id="step-3">
                        <div class="col-xs-12">


                            <div class="col-md-12">
                                <h4>Select Flights</h4>

                                <div class="form-group">
                                    <table id="tbl_flights" data-bind="visible: flights().length > 0" class="table table-striped table-bordered table-hover">
                                        <thead>
                                            <tr>
                                                <th></th>
                                                <th>Flight#</th>
                                                <th>Date</th>
                                                <th>Time</th>
                                                <th>Unassigned</th>
                                                <th>SELECT  <span title="Sum of PAX" style="margin-right:5px" class="badge" data-bind="text: total_sum"></span></th>
                                            </tr>

                                        </thead>

                                        <tbody data-bind="foreach: { data: flights, includeDestroyed: true }">
                                            <tr data-bind="click: $root.select, css: { mark: selected }">
                                                <td style="text-align: center">
                                                    <div class="form-group">
                                                        <input  type="checkbox" data-bind="checked: selected" />
                                                    </div>
                                                </td>

                                                <td data-bind="text: num"></td>
                                                <td data-bind="text: date"></td>
                                                <td data-bind="text: time"></td>
                                                <td data-bind="text: sum"></td>
                                                <td style="text-align: center">
                                                    <input type="button" value="Select" class="btn btn-info" style="background-color: #5cb85c" /></td>


                                            </tr>
                                        </tbody>
                                    </table>
                                    <div data-bind="visible: flights().length == 0">
                                        There are no unassigned passengers for <span data-bind="text: date_start"></span>for this route. <b>Choose another route or another date.</b>
                                    </div>
                                </div>

                                <button class="btn btn-primary nextBtn btn-lg pull-right" data-bind="enable: selected_flights().length != 0, click: getEarliestFlight" type="button">Next</button>
                            </div>



                        </div>
                    </div>

                    <div class="row setup-content" id="step-4">





                        <div class="col-lg-12">
                            <p>You have <span title="Sum of Selected PAX" class="badge" data-bind="text: total_sum"></span>passengers to assign to transportation.</p>

                            <div class="panel panel-info">
                                <div class="panel-heading">
                                    <i class="fa fa-bus  fa-fw"></i>
                                    <span>Create a new transportation list</span>
                                </div>
                                <!-- /.panel-heading -->
                                <div id="divCreatePlan" class="panel-body form-inline">

                                    <div class="form-group ">
                                        <label class="control-label">Driver's Details </label>
                                        <textarea class="form-control" data-bind="value: comments_trans"></textarea>
                                    </div>

                                    <%--                                    <div class="form-group ">
                                        <label class="control-label">Pickup Time </label>
                                        <input class="form-control" type="time" data-bind="value: time" />
                                    </div>--%>



                                    <div class="form-group ">
                                        <label class="control-label">Accompanied</label>

                                        <select required id="ddlGuides" class="form-control margin-bottom-14 conreq" data-bind="
    options: guides,
    optionsText: 'name',
    optionsValue: 'ID',
    value: guide_fk_selected,
    optionsCaption: 'Select Accompanied',
    valueAllowUnset: true
">
                                        </select>

                                    </div>
                                    <button data-bind="click: addEvent(), enable: count_create()==0" id="btnCreateList" class="btn btn-primary" type="button">Create</button>
                                </div>

                            </div>

                            <div data-bind="visible: events().length > 0">
                                <h4>Wait, there are existing transportation lists for the selected date and route:</h4>
                                <table id="tbl_events" class="table table-striped table-bordered table-hover">
                                    <thead>
                                        <tr>
                                            <%-- <th></th>--%>
                                            <th>ID</th>
                                            <th>Date</th>
                                            <th>Route</th>
                                            <th>Accompanied</th>
                                            <th>Time</th>
                                            <th>Passengers</th>
                                            <th>Assign</th>
                                            <%--, visible: total_sum()>0 --%>
                                        </tr>

                                    </thead>

                                    <tbody data-bind="foreach: { data: events, includeDestroyed: true }">
                                        <%--  <tr data-bind="click: $parent.select ">--%>
                                        <tr data-bind="click: $root.select, css: { mark: selected }">
                                            <%--                                                <td style="text-align: center">
                                                    <div class="form-group">
                                                    <input required="required" type="checkbox" data-bind="checked: selected" /> </div>

                                                </td>--%>

                                            <td data-bind="text: ID"></td>
                                            <td data-bind="text: date"></td>
                                            <td data-bind="text: activity_name"></td>
                                            <td data-bind="text: guide_name"></td>
                                            <td data-bind="text: time"></td>
                                            <td data-bind="text: people"></td>
                                            <td style="text-align: center">
                                                <input type="button" value="Assign" data-bind="click: $parent.AssignPassengers" class="btn btn-info" style="background-color: #5cb85c" /></td>


                                        </tr>
                                    </tbody>
                                </table>
                            </div>

                            <div class="form-group">
                                <button class="btn btn-primary nextBtn btn-lg pull-right" data-bind="disable: event_fk_selected()==0,  click: GetCreatePlan" type="button">Next</button>


                            </div>
                        </div>


                    </div>

                    <div class="row setup-content" id="step-5">
                        <div class="col-xs-12">


                            <div class="col-md-12">
                                <h3>Plan Pickup Route</h3>
                                <p>Eariest flight departs at: <span style="font-weight: bold" class="label-warning" id="lbl_earliest_time" data-bind="text: earliest_flight"></span> List # <span class="badge" data-bind="text:event_fk_selected" ></span></p>
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
                                <div class="form-group">
                                    <button class="btn btn-primary nextBtn btn-lg pull-right" data-bind="click: UpdateDepartEventTime" type="button">Next</button>


                                </div>

                            </div>
                        </div>
                    </div>



                    <div class="row setup-content" id="step-6">
                        <div class="col-xs-12">



                            <div class="col-md-12">
                                <%--     <h3>Title</h3>--%>
                                <br />



                                <p>Departure List  <b style="margin-right: 5px" data-bind="text: event_fk_selected"></b>is Ready to print.</p>
                                <iframe data-bind="attr: { src: print_url }" name="frame" id="iframe_print" style="display: none"></iframe>


                                <a class="btn btn-success btn-lg pull-right" target="_blank" href="EventsTrans.aspx">Print List</a>


                            </div>
                        </div>
                    </div>


                </form>
            </div>



        </div>
    </div>



</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="FooterScripts" runat="Server">

    <script src="../scripts/wizard.js"></script>
    <script>
        var DIRECTION = 'out';
        var _url_print = '../print/ListArrival_Print.aspx?';
    </script>
    <script src="../views_client/view_transport_wizard.js"></script>

    <script>

        var _url_activities = url_activities + '/transport/out';
        var _url_event_post = url_events;
        var _url_print = '../print/ListDeparture_Print.aspx?';

        var my = { viewModel: new FlightViewModel() };

        ko.applyBindings(my.viewModel);


        $(document).ready(function () {
            $("#txtDateStrat").datepicker({ dateFormat: 'yy-mm-dd', minDate: 0 });

            $('#btnPrint').click(function () {
                frames['frame'].print();
            });


        });


    </script>

</asp:Content>
