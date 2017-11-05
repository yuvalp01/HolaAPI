<%@ Page Title="Invoices" Language="C#" MasterPageFile="~/MasterHola.master" AutoEventWireup="true" CodeFile="Invoices.aspx.cs" Inherits="pages_Invoices" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        tr.mark {
            background-color: #08C !important;
        }
        .buttons-excel {
                background-image: -webkit-linear-gradient(top, white 0%, #5cb85c 100%) !important;
       
        }

                .buttons-print {
                background-image: -webkit-linear-gradient(top, white 0%, #337ab7 100%) !important;     
        }
           
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <div class="row">
        <div class="col-lg-12">
            <h1 class="page-header"><i class="fa fa-euro fa-fw"></i>Invoices</h1>
        </div>
    </div>

    <div class="panel panel-default">

        <div class="panel-heading">
            <h3 class="panel-title">Create and print an invoice</h3>

        </div>
        <div class="panel-body">
            <a id="btnPrintInvoice" class="dt-button buttons-print" style="display: none"
                tabindex="0" data-bind="enable: total() > 0" aria-controls="tblInvoice"><i class="fa fa-print fa-fw"></i><span>Print Invoice</span></a>

            <div class="form-group ">
                <label>Agency</label>
                <select class="form-control margin-bottom-14" data-bind="
    options: agencies,
    optionsText: 'name',
    optionsValue: 'ID',
    value: agency_fk,
    optionsCaption: 'Select Agency',
    valueAllowUnset: true
">
                </select>

            </div>
            <div class="form-group ">
                <label>Month</label>
                <select class="form-control margin-bottom-14" data-bind="
    options: months,
    optionsText: 'name',
    optionsValue: 'ID',
    value: month,
    optionsCaption: 'Select Month',
    valueAllowUnset: true
">
                </select>
            </div>
            <div class="form-group ">
                <label>Year</label>
                <select class="form-control margin-bottom-14" data-bind="options: years, value: year"></select>
            </div>

            <table id="tblInvoice" data-bind="visible: total() > 0" class="table table-striped table-bordered table-hover" style="width: 100%">
                <thead>
                    <tr>

                        <th>Date</th>
                        <th>Flight</th>
                        <th>Service</th>
                        <th>People</th>
                        <th>Rate</th>
                        <th>Sum</th>

                    </tr>
                </thead>
                <tfoot>
                    <tr>
                        <th></th>
                        <th></th>
                        <th></th>
                        <th></th>
                        <th></th>
                        <th></th>
                    </tr>
                </tfoot>

            </table>

            <iframe data-bind="attr: { src: print_url }" name="frame" id="iframe_print" style="display: none"></iframe>



        </div>
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

    <script src="../views_client/view_invoice.js"></script>


    <script>
        //ko.options.useOnlyNativeEvents = true;
        var my = { viewModel: new InvoiceViewModel() };

        // my.viewModel.months(months_json);

        ko.applyBindings(my.viewModel);

        $(document).ready(function () {

            $('#btnPrintInvoice').click(function () {
                frames['frame'].print();

            });
        });

    </script>

</asp:Content>
