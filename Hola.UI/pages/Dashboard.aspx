<%@ Page Title="" Language="C#" MasterPageFile="~/MasterHola.master" AutoEventWireup="true" CodeFile="Dashboard.aspx.cs" Inherits="pages_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-lg-12">
            <h1 class="page-header">Dashboard</h1>
        </div>
        <!-- /.col-lg-12 -->
    </div>

    <div class="row">
        <div class="col-lg-3 col-md-6">
            <div class="panel panel-green">
                <div class="panel-heading">
                    <div class="row">
                        <div class="col-xs-6">

                            <i class="fa fa-tasks fa-3x"></i>
                            <i class="fa fa-plane fa-rotate-90 fa-3x"></i>
                        </div>
                        <div class="col-xs-6 text-right">
                            <div data-bind="text: pax_in" class="huge"><i class="fa fa-spinner fa-spin fa-3x fa-fw margin-bottom"></i></div>
                            <div></div>
                        </div>
                    </div>
                </div>
                <a href="Search.aspx?search=<%=Today %>" target="_blank">
                    <div class="panel-footer">
                        <span class="pull-left">Today's Arrivals</span>
                        <span class="pull-right">
                            <i class="fa fa-arrow-circle-right"></i></span>
                        <div class="clearfix"></div>
                    </div>
                </a>
            </div>
        </div>
        <div class="col-lg-3 col-md-6">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <div class="row">
                        <div class="col-xs-6">
                            <i class="fa fa-tasks fa-3x"></i>
                            <i class="fa fa-plane fa-3x"></i>

                        </div>
                        <div class="col-xs-6 text-right">
                            <div data-bind="text: pax_out" class="huge"><i class="fa fa-spinner fa-spin fa-3x fa-fw margin-bottom"></i></div>
                            <div></div>
                        </div>
                    </div>
                </div>
                <a href="Search.aspx?search=<%=Today %>" target="_blank">
                    <div class="panel-footer">
                        <span class="pull-left">Today's Departures</span>
                        <span class="pull-right"><i class="fa fa-arrow-circle-right"></i></span>
                        <div class="clearfix"></div>
                    </div>
                </a>
            </div>
        </div>
        <%--        <div class="col-lg-3 col-md-6">
            <div class="panel panel-yellow">
                <div class="panel-heading">
                    <div class="row">
                        <div class="col-xs-3">
                            <i class="fa fa-plane fa-rotate-90 fa-5x"></i>
                        </div>
                        <div class="col-xs-9 text-right">
                            <div class="huge">124</div>
                            <div>Arrivals</div>
                        </div>
                    </div>
                </div>
                <a href="#">
                    <div class="panel-footer">
                        <span class="pull-left">View Details</span>
                        <span class="pull-right"><i class="fa fa-arrow-circle-right"></i></span>
                        <div class="clearfix"></div>
                    </div>
                </a>
            </div>
        </div>
        <div class="col-lg-3 col-md-6">
            <div class="panel panel-red">
                <div class="panel-heading">
                    <div class="row">
                        <div class="col-xs-3">
                            <i class="fa fa-plane  fa-5x"></i>
                        </div>
                        <div class="col-xs-9 text-right">
                            <div class="huge">13</div>
                            <div>Departures</div>
                        </div>
                    </div>
                </div>
                <a href="#">
                    <div class="panel-footer">
                        <span class="pull-left">View Details</span>
                        <span class="pull-right"><i class="fa fa-arrow-circle-right"></i></span>
                        <div class="clearfix"></div>
                    </div>
                </a>    
            </div>
        </div>--%>
    </div>

<%--    <div class="alert alert-warning alert-dismissible" role="alert">
                <button type="button" class="close" data-dismiss="alert" aria-label="Close">  <span aria-hidden="true">&times;</span></button>
     <strong></strong>There are    <a href="#" class="alert-link"><span class="badge"> 4 </span> unconfirmed flight time</a> in the next 3 days 
    </div>

    <div class="alert alert-info alert-dismissible" role="alert">
        <button type="button" class="close" data-dismiss="alert" aria-label="Close">
            <span aria-hidden="true">&times;</span></button>
        Send invoice to agencies in <span class="badge">9</span> days 
    </div>--%>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterScripts" runat="Server">
    <script>
        var today = '<%=Today%>';
        var _url = url_flights+'/GetFlightsStats/' + today;

        function ViewModel(data) {
            var self = this;

            self.in = ko.observable();
            self.out = ko.observable();
            self.pax_in = ko.observable();
            self.pax_out = ko.observable();

            $.getJSON(_url, function (result) {
                self.in(result.IN);
                self.out(result.OUT);
                self.pax_in(result.PAX_IN);
                self.pax_out(result.PAX_OUT);

            });
        }
        ko.applyBindings(new ViewModel());

        $(document).ready(function () {

        });

    </script>
</asp:Content>

