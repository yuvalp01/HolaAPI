<%@ Page Title="" Language="C#" MasterPageFile="~/MasterHola.master" AutoEventWireup="true" CodeFile="EventSplit.aspx.cs" Inherits="pages_EventSplit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
<%--    <script src="https://ajax.googleapis.com/ajax/libs/angularjs/1.5.7/angular.min.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/angular_material/1.1.0-rc.5/angular-material.min.js"></script>--%>

    <script src="../scripts/angular.min.js"></script>

    <script>
        var ORIGINAL_LIST = '<%=ORIGINAL_LIST  %>';
    </script>
    <script src="../angularjs/mainApp.js"></script>
    <script src="../angularjs/passengersController.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div ng-app="mainApp" ng-controller="passengersController" ng-init="init()" class="row">
        <div class="col-lg-12">
            <h1 class="page-header"><i id="test" class="fa fa-exchange fa-fw"></i>Split Lists</h1>
            <a href="eventsTrans.aspx"><i class="fa fa-reply  fa-fw"></i>Return to Lists</a>
        </div>
        <!-- /.col-lg-12 -->

        <div class="col-lg-12">
            <div class="panel panel-default">
                <div class="panel-body">
                    <div class="row">
                        <div class="col-lg-6" style="padding-left: 0px;">
                            <div class="col-lg-12">
                                <div class="panel panel-info">
                                    <div class="panel-heading">
                                        <i class="fa fa-list-alt  fa-fw"></i>

                                        Origin List <span ng-bind="'#' + event_original.ID"></span>
                                        <i class="fa fa-male"></i>
                                        <span class="badge" ng-bind="sum_original"></span>
                                    </div>
                                    <div class="panel-body form-inline">
                                        <div class="well" >

                                            <table style="width: 100%;min-height:150px ">
                                                <tr>
                                                    <td><i class="fa fa-calendar"></i>
                                                        <span ng-bind="event_original.date  | date:'yyyy-MM-dd'"></span></td>
                                                    <td><i class="fa fa-clock-o"></i>
                                                        <span ng-bind="event_original.time | date:'hh:mm'"></span></td>
                                                </tr>
                                                <tr>
                                                    <td><i class="fa fa-bus"></i>
                                                        <span ng-bind="event_original.activity_name"></span></td>
                                                    <td><i class="fa fa-user"></i>
                                                        <span ng-bind="event_original.guide_name"></span></td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2"><i class="fa fa-info-circle"></i>
                                                        <span ng-bind="event_original.comments"></span></td>

                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <button style="width: 100%" id="btnDuplicate" class="btn btn-warning" ng-click="duplicate(event_original.ID)" type="button"><i class="glyphicon glyphicon-duplicate"></i>Duplicate List</button>
                                                    </td>
                                                </tr>
                                            </table>


                                        </div>


                                        <div class="alert alert-info" ng-repeat="passenger in passengers_original">
                                            <span class="label label-default" ng-bind="passenger.hotel_name"></span>
                                            <span ng-attr-title="{{passenger.names}}">{{ '[' + passenger.PNR + '] ' }}   {{ passenger.names | limitTo: 15 }}{{passenger.names.length > 15 ? '...' : ''}}
                                            </span>

                                            <button style="display: none" ng-click="movePassenger(passenger, $index, 'right')" class="btn btn-default  btn-circle shootRight"><i class="fa fa fa-long-arrow-right "></i></button>


                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-6" style="padding-left: 0px;">
                            <div class="col-lg-12">
                                <div id="newList_div" style="display:none" class="panel panel-success">
                                    <div class="panel-heading">
                                        <i class="fa fa-list-alt  fa-fw"></i>
                                        New List
                                           <span ng-bind="'#'+event_new.ID"></span>
                                        <span id="div_sum" style="display: none">

                                            <i class="fa fa-male"></i>
                                            <span class="badge" ng-bind="sum_new"></span>
                                        </span>


                                    </div>
                                    <div   class="panel-body form-inline">
                                        <div class="well">


                                            <table style="width: 100%;min-height:150px ">
                                                <tr>
                                                    <td><i class="fa fa-calendar"></i>
                                                        <span ng-bind="event_new.date  | date:'yyyy-MM-dd'"></span></td>
                                                    <td><i class="fa fa-clock-o"></i>
                                                        <span ng-bind="event_new.time | date:'hh:mm'"></span></td>
                                                </tr>
                                                <tr>
                                                    <td><i class="fa fa-bus"></i>
                                                        <span ng-bind="event_new.activity_name"></span></td>
                                                    <td><i class="fa fa-user"></i>
                                                        <%--<label class="control-label">Accompanied*</label>--%>

                                                        <select ng-model="event_new.guide_fk" class="form-control margin-bottom-14" required="required">

                                                            <option ng-repeat="guide in guides" ng-value="guide.ID">{{guide.name}}</option>
                                                        </select>


                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2"><i class="fa fa-info-circle"></i>
                                                        <input style="width:93%"   ng-model="event_new.comments" required="required" class="form-control" placeholder="Driver Details" />




                                                    </td>

                                                </tr>
                                                <tr>
                                                    <td colspan="2">
  <button style="width:100%; margin-top:5px" class="btn btn-primary" type="button" ng-click="UpdateEvent()">Save</button>

                                                    </td>
                                                </tr>
                                            </table>





                                            <%--                                            <select ng-model="event_new.guide_fk" ng-options="guide as guide.name for guide in guides track by guide.ID" >

                                                <option value="0">Select Accompanied</option>
                                            </select>--%>






                                          

                                        </div>



                                        <div class="alert alert-success" ng-repeat="passenger in passengers_new">
                                            <button ng-click="movePassenger(passenger, $index, 'left')" class="btn btn-default  btn-circle"><i class="fa fa fa-long-arrow-left"></i></button>

                                            <span class="label label-default" ng-bind="passenger.hotel_name"></span>
                                            <span ng-attr-title="{{passenger.names}}">{{ '[' + passenger.PNR + '] ' }}   {{ passenger.names | limitTo: 15 }}{{passenger.names.length > 15 ? '...' : ''}}
                                            </span>



                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>


        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterScripts" runat="Server">
    <script>

    </script>
</asp:Content>

