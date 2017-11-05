<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ListTrans_IN_Print.aspx.cs" Inherits="ListArrival_Print" %>

<!DOCTYPE html>





<html xmlns="http://www.w3.org/1999/xhtml">


<head runat="server">
    <style>
        .panel-body, .panel-heading {
            padding: 5px !important;
        }



        @media all {
            .page-break {
                display: none;
            }

            .page-break-web {
                display: block;
                border: dashed;
                border-width: thin;
                width: 100%;
                margin-left: auto;
                margin-right: auto;
                text-align: center;
            }
        }




        @media print {
            .page-break {
                display: block;
                page-break-before: always;
            }

            .page-break-web {
                display: none;
                page-break-before: always;
            }

            .btn-group {
                display: none !important;
            }
        }

        .container {
            max-width: none !important;
            width: 970px;
        }

        table.fixed {
            table-layout: fixed;
        }

            table.fixed td {
                overflow: hidden;
            }

        tr.group,
        tr.group:hover {
            background-color: #ddd !important;
            font-weight: bold;
        }

        td {
            /*line-height: 10px !important;
   min-height: 10px !important;
   height: 10px !important;*/
            padding: 1px !important;
        }

        th {
            padding: 3px !important;
            font-size:small ;
        }

        /*.fa-bus {
            color: #337ab7 !important;
        }*/

        @media print {
            .badge {
                padding: 2px 6px;
                border: 1px solid #000;
            }

            .label-default {
                background-color: #777 !important;
                color: white !important;
                border-style: none !important;
            }
        }
    </style>
    <title>Arrival List for <%=DateStart %></title>
</head>
<body>

    <div style="text-align: center;">
        <div class="col-xs-12">
            <h3 style="margin-left: 5px">

                <i class="fa fa-plane fa-rotate-90 fa-1x "></i>
                Arrival List
                <span style="margin-left: 5px; margin-right: 5px" class="label label-default" data-bind="text: date"></span>
            </h3>
        </div>
    </div>
    <div style="text-align: left; position: absolute; top: 0; right: 0">
        <img src="../images/holaShalom_small.png" />
        <%--        <img title="Transportation" src="../icons/fa-train.png" /><span style="margin-left: 5px; margin-right: 5px" class="label label-default" data-bind="text: route"></span>--%>
        <%--        <img title="Pickup Date" src="../icons/fa-calendar.png" />--%>
        <%--        <span style="margin-left: 5px; margin-right: 5px" class="label label-default" data-bind="text: date"></span>--%>
        <%--         <img src="../images/AviationLinks.png" title="Aviation Links" alt="Aviation Links" />
        Aviation Links
                 <img src="../images/AviationLinks.png" title="Aviation Links" alt="Aviation Links" />
        Aviation Links--%>
    </div>

    <hr />
    <br />
    <br />
    <br />




    <div class="col-lg-12">
        <div class="panel panel-default">
            <div class="panel-heading">
                <i class="fa fa-bus  fa-fw "></i>
                <span data-bind="text: route"></span><i class="fa fa-male  fa-fw "></i><span class="badge"><%=TotalP %></span>
            </div>
            <!-- /.panel-heading -->
            <div class="panel-body form-inline">
                <h4>Pickup Time: <span class="label label-default" data-bind="text:time"></span>
                    By:  <span class="label label-default" data-bind="text:guide_name"></span>
                    Driver's Details:  <span class="label label-default" data-bind="text:comments"></span>

                </h4>
            </div>
        </div>
    </div>
    <div class="col-lg-12">
        <div class="panel panel-default">
            <div class="panel-heading">
                <i class="fa fa-plane fa-rotate-90  fa-fw"></i>
                Flights
            </div>
            <!-- /.panel-heading -->
            <div class="panel-body form-inline">


                <table id="tbl_flights" style="margin: 0" class="table table-striped table-bordered table-hover compact table-sm">
                    <thead>
                        <tr>

                            <th>Flight#</th>
                            <th>Date</th>
                            <th>Time</th>

                        </tr>

                    </thead>

                    <tbody data-bind="foreach: { data: flights }">
                        <tr>


                            <td data-bind="text: num"></td>
                            <td data-bind="text: date"></td>
                            <td data-bind="text: time"></td>
                        </tr>
                    </tbody>
                </table>

            </div>
        </div>
    </div>



    <div style="clear: both">
        <div class="page-break-web" style="margin-bottom:5px; padding-bottom:5px; background-color:gainsboro">
            <p>Print configuration</p>
            <div class="btn-group" role="group">

                <button type="button" data-bind="click: function(){ toggleLabel('Flight')}" class="btn btn-primary"><i id="iFlight" data-status="hide" class="fa fa-eye-slash">Hide Flight</i></button>
                <button type="button" data-bind="click: function(){ toggleLabel('Agency')}" class="btn btn-primary"><i id="iAgency" data-status="hide" class="fa fa-eye-slash">Hide Agency</i></button>
                <button type="button" data-bind="click: function(){ toggleLabel('Days')}" class="btn btn-primary"><i id="iDays" data-status="hide" class="fa fa-eye-slash">Hide Days</i></button>
                <button type="button" data-bind="click: function(){ compress()}" class="btn btn-primary"><i id="iCompress" data-status="compress" class="fa fa-compress">Compress</i></button>
                <button type="button" data-bind="click: function(){ togglePage('Break')}" class="btn btn-primary"><i id="iBreak" data-status="hide" class="fa fa-chain">Connect Pages</i></button>

            </div>
        </div>
<%--        <div class="page-break"></div>--%>

    </div>

    <div class="Break" style="clear: both">
        <div class="page-break-web">
            <p style="margin-bottom:0">Page Break</p>
        </div>
        <div class="page-break"></div>
    </div>




    <div id="secondTitle" style="text-align: center;">
        <div class="col-xs-12">
            <h3 style="margin-left: 5px">

                <i class="fa fa-plane fa-rotate-90 fa-1x "></i>
                Arrival List
                <span style="margin-left: 5px; margin-right: 5px" class="label label-default" data-bind="text: date"></span>
            </h3>
        </div>
    </div>
    <table style="width: 100%" class="table table-striped table-bordered table-hover order-column compact table-sm " id="tblArrival">
        <thead>
            <tr>
                <th>Hotel</th>
                <%--                <th style="width: 20px">PNR</th>--%>
                <th class="col-md-6 ">Names  </th>
                <th class="col-sm-1">P</th>

                <th class="col-md-3">Phone</th>

                <!-- ko foreach: tour_names-->
                <td class="col-md-2" style="width: 10px" data-bind="text: $data"></td>
                <!-- /ko-->
            </tr>
            <%--            <tr id="trLoading" style="display: inline-grid">
                <td colspan="7" style="text-align: center;">Loading...</td>
            </tr>--%>
        </thead>

        <tbody data-bind="foreach: { data: passengers }">
            <tr>

                <td data-bind="text: hotel_name"></td>
                <td>
                    <span data-bind="text: names" class="names" style="margin-right: 2px"></span>
                    <span class="badge Agency" data-bind="text: agency_name"></span>
                    <%--                    <img src="../images/AviationLinks.png" title="Aviation Links" alt="Aviation Links" />--%>
                    <span class="badge Flight" data-bind="text: num_arr"></span>
                    <span class="badge Days" data-bind="text: days() + 'd'"></span>
                </td>
                <td data-bind="text: PAX"></td>

                <td data-bind="text: phone"></td>

                <!-- ko foreach: activities-->
                <td data-bind="text: sum"></td>
                <!-- /ko-->
            </tr>
        </tbody>
    </table>

    <h3>Tour plan for this week</h3>

    <table class="table table-striped table-bordered table-hover order-column compact  ">
        <thead>
            <tr>
                <th style="width: 20px"></th>
                <th>Name</th>
                <th>Date</th>
                <th>Time</th>
                <th>Comments</th>
            </tr>
            <tr id="trLoading_plan" style="display: inline-grid">
                <td colspan="7" style="text-align: center;">Loading...</td>
            </tr>
        </thead>

        <tbody data-bind="foreach: { data: tour_plan }">
            <tr>


                <td style="width: 20px" data-bind="text: subcat"></td>
                <td data-bind="text: activity_name"></td>
                <td data-bind="text: date"></td>
                <td data-bind="text: time"></td>
                <td data-bind="text: comments"></td>
            </tr>
        </tbody>
    </table>



</body>
</html>



<!-- Bootstrap Core CSS -->
<link href="../Content/bootstrap.min.css" rel="stylesheet" />
<!-- Bootstrap datatables CSS -->
<link href="../Content/DataTables/css/dataTables.bootstrap.min.css" rel="stylesheet" />
<%--<link href="Content/DataTables/css/jquery.dataTables.css" rel="stylesheet" />--%>
<!-- jq UI -->
<link href="../components/jquery-ui/jquery-ui.min.css" rel="stylesheet" />
<!-- MetisMenu CSS -->
<link href="../components/metisMenu/dist/metisMenu.min.css" rel="stylesheet" />
<!-- Custom CSS -->
<link href="../Content/sb-admin-2.css" rel="stylesheet" />
<!-- Custom Fonts -->
<link href="../fonts/font-awesome-4.5.0/css/font-awesome.min.css" rel="stylesheet" />

<!-- jQuery -->
<script src="../scripts/jquery-2.2.1.min.js"></script>
<!-- jQuery datatables -->
<script src="../scripts/DataTables/jquery.dataTables.min.js"></script>
<script src="../scripts/DataTables/dataTables.bootstrap.min.js"></script>
<!-- jQuery UI -->
<script src="../components/jquery-ui/jquery-ui.min.js"></script>
<!-- Bootstrap Core JavaScript -->
<script src="../scripts/bootstrap.min.js"></script>
<!-- Metis Menu Plugin JavaScript -->
<script src="../components/metisMenu/dist/metisMenu.min.js"></script>

<!-- Custom Theme JavaScript -->
<script src="../scripts/sb-admin-2.js"></script>

<script src="../scripts/knockout-3.4.0.js"></script>
<script>
    var api_url = '<%=ConfigurationManager.AppSettings["api_url"] %>'
</script>

<script src="../views_client/common.js"></script>

<script type="text/javascript">

    var date_start = '<%=DateStart%>';
    var date_end = '<%=DateEnd%>';
    var url_list = '<%=UrlList%>';
    var url_tour_plan = '<%=UrlPlan%>';
    // var url_tour_plan = '<%=UrlPlan%>';



</script>
<script>


    function Passenger(data) {
        this.hotel_name = ko.observable(data.hotel_name);
        this.PNR = ko.observable(data.PNR);
        this.names = ko.observable('['+data.PNR + '] ' + data.names );
        this.phone =  ko.observable(data.phone); 
        this.PAX =  ko.observable(data.PAX); 
        this.agency_name =  ko.observable(data.agency_name); 
        this.num_arr =  ko.observable(data.num_arr); 
        this.days =  ko.observable(data.days); 
        this.activities =  ko.observableArray([]);

        var mappedData = $.map(data.activities, function (item) {
            return new ActivityPair(item);
        });

        this.activities(mappedData);
    }

    function ActivityPair(data) {
        this.activity_fk =  ko.observable(data.activity_fk); 

        this.sum =  ko.observable(data.sum==0?'':data.sum); 
    }

    function Event(data) {

        var d = new Date(data.date);
        this.date = ko.observable(d.yyyymmdd());

        var _t;
        if (data.time) {
            _t = data.time.HHmm();
        }
        this.time = ko.observable(_t);

        this.activity_name = ko.observable(data.activity_name);
        this.guide_name = ko.observable(data.guide_name);
        this.comments = ko.observable(data.comments);
        this.subcat = ko.observable(data.subcat);
    }

    //Constructor for an object with two properties
    function Flight(data) {

        this.num = ko.observable(data.num);
        var d = new Date(data.date);
        this.date = ko.observable(d.yyyymmdd());
        var t = data.time;
        this.time = ko.observable(t.HHmm());
 
        //this.destination = ko.observable( data.destination);
        //this.direction = ko.observable(data.direction);
        //this.date_update = ko.observable(data.date_update);
    };


    function AppViewModel(data) {
        var self = this;
        self.passengers = ko.observableArray([]);
        //self.activities = ko.observableArray([]);
        self.tour_plan = ko.observableArray([]);
        self.tour_names = ko.observableArray([]);
        self.flights = ko.observableArray([]);

        self.date = ko.observable();
        self.time = ko.observable();
        self.route = ko.observable();
        self.comments = ko.observable();
        self.guide_name = ko.observable();


        var Passengers = <%=Passengers %>;
        var TourNames = <%=TourNames %>;
        var TourPlan = <%=TourPlan %>;
        var Flights = <%=Flights %>;

        var DATA = <%=DATA %>;
  
        self.route(DATA.activity_name)
        var d = new Date(DATA.date);
        self.date(d.yyyymmdd());

        self.time(DATA.pickup_time.HHmm());
        self.comments(DATA.comments_trans);
        self.guide_name(DATA.guide_name);

        var _passengers = $.map(Passengers, function (item) {
            return new Passenger(item);
        });
        $('#trLoading').hide();
        self.passengers(_passengers);

        var _events = $.map(TourPlan, function (item) {
            return new Event(item);
        });
        $('#trLoading_plan').hide();
        self.tour_plan(_events);

        var _flights = $.map(Flights, function (item) {
            return new Flight(item);
        });

        self.flights(_flights);

        self.tour_names(TourNames);

        self.toggleLabel = function (itemName) {

            if ($('#i'+itemName).attr('data-status')=='hide') {
                $('#i'+itemName).attr('data-status','show');
                $('#i'+itemName).removeClass('fa-eye-slash').addClass('fa-eye');
                $('#i'+itemName).text('Show '+ itemName);
                $('#i'+itemName).parent().removeClass('btn-primary').addClass('btn-warning');
                $('.' + itemName).hide('fast');

            }
            else
            {
                $('#i'+itemName).attr('data-status','hide');
                $('#i'+itemName).removeClass('fa-eye').addClass('fa-eye-slash');
                $('#i'+itemName).text('Hide '+itemName);
                $('#i'+itemName).parent().removeClass('btn-warning').addClass('btn-primary');
                $('.' + itemName).show('fast');
            }

        }

        
        self.togglePage = function (itemName) {


            if ($('#i'+itemName).attr('data-status')=='hide') {
                $('#i'+itemName).attr('data-status','show');
                $('#i'+itemName).removeClass('fa-chain').addClass('fa-chain-broken');
                $('#i'+itemName).text('Break Page');
                $('#i'+itemName).parent().removeClass('btn-primary').addClass('btn-warning');
                $('#secondTitle').hide('fast');               
                $('.' + itemName).hide('fast');

            }
            else
            {
                $('#i'+itemName).attr('data-status','hide');
                $('#i'+itemName).removeClass('fa-chain-broken').addClass('fa-chain');
                $('#i'+itemName).text('Connect Pages');
                $('#i'+itemName).parent().removeClass('btn-warning').addClass('btn-primary');
                $('#secondTitle').show('fast');   
                $('.' + itemName).show('fast');
            }
        }


        




        self.compress = function () {


            if ($('#iCompress').attr('data-status')=='compress') {

                $('#iCompress').attr('data-status','expand');
                $('#iCompress').removeClass('fa-compress').addClass('fa-expand');
                $('#iCompress').text('Expand');
                $('#iCompress').parent().removeClass('btn-primary').addClass('btn-warning');
                $('td span, td, th').css('font-size','x-small');

                $('h4').each(function() {
                    $(this).replaceWith('<h7>'+$(this).html()+'</h7>');
                });

            }
            else
            {

                $('#iCompress').attr('data-status','compress');
                $('#iCompress').removeClass('fa-expand').addClass('fa-compress');
                $('#iCompress').text('Compress');
                $('#iCompress').parent().removeClass('btn-warning').addClass('btn-primary');
                $('td span, td, th').css('font-size','small');

                
                $('h7').each(function() {
                    $(this).replaceWith('<h4>'+$(this).html()+'</h4>');
                });

            }

        }
    }
    // Activates knockout.js
    ko.applyBindings(new AppViewModel());

</script>


<script>

    $(document).ready(function() {
  
        $('#tblArrival').DataTable( {

            "columnDefs": [
                { "visible": false, "targets": 0}
            ],

            "drawCallback": function ( settings ) {
                var api = this.api();
                var rows = api.rows( {page:'current'} ).nodes();
                var last=null;

                var subTotal = new Array();
                var grandTotal = new Array();
                var groupID = -1;

                api.column(0, {page:'current'} ).data().each( function ( group, i ) {

                    if ( last !== group ) {
                        groupID++;

                        $(rows).eq(i).before("<tr class='group'><td  class='groupTitle'>"+group+"</td><td class='groupSum' colspan='20' id='" + groupID + "'></td></tr>");
                        last = group;
                    }
                    //Sub-total of each column within the same grouping
                    var val = api.row(api.row($(rows).eq(i)).index()).data(); //Current order index
                    if (typeof subTotal[groupID] == 'undefined') {
                        subTotal[groupID] = new Array();
                    }
                    if (typeof subTotal[groupID][2] == 'undefined') {
                        subTotal[groupID][2] = 0;
                    }
                    if (typeof grandTotal[2] == 'undefined') {
                        grandTotal[2] = 0;
                    }
                    value = parseInt( val[2])
                    subTotal[groupID][2] += value;
                    grandTotal[2] += value;
 
                });            

                for (var i = 0; i < subTotal.length; i++) {
                    var www =   $('tbody').find('td#'+i).text(subTotal[i][2]);
                }

                //$('tbody').find('td.groupSum').each(function (i, v) {
                //var rowCount = $(this).nextUntil('.group').length;
                //    var subTotalInfo = "";
                //    for (var a = 4; a <= 8; a++) {
                //        subTotalInfo += "<td class='groupTD'>" + subTotal[i][a] + "/" + grandTotal[a] + "</td>";
                //    }
                //    $(this).append(subTotalInfo);
                //});
            },
            "paging":   false,
            "sorting": false,
            "searching": false,
            "info": false,
            "bSort": false
        } );
    } );


</script>
