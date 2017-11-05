<%@ Page Title="Arrival/Departure Lists" Language="C#" MasterPageFile="~/MasterHola.master" AutoEventWireup="true" CodeFile="EventsTrans.aspx.cs" Inherits="pages_TourPlan" %>

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

        @keyframes spin {
            0% {
                transform: rotate(0deg);
            }

            100% {
                transform: rotate(360deg);
            }
        }

        .btn-print {
            color: #2cb63e;
            background-color: #f0e94e;
            border-color: #f0e94e;
        }

        .fa-trash {
            color: red;
        }


        .fa-print {
            color: green;
        }

        .fa-exchange {
            color: purple;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="row">
        <div class="col-lg-12">
            <h1 class="page-header"><i class="fa fa-list-alt fa-fw"></i>Arrival/Departure Lists - last 14 days</h1>
        </div>
    </div>



    <table class="table table-striped table-bordered table-hover" id="tbl">
        <thead>
            <tr>
                <th></th>
                <th>ID</th>
                <th>Date         </th>
                <th>Time       </th>
                <th>Route       </th>
                <th>P      </th>

                <th>Accompanied      </th>
                <th>Driver's Details       </th>
                <th>Update Date      </th>
            </tr>
            <tr id="trLoading" style="display: inline-grid">
                <td colspan="9" style="text-align: center;">Loading...</td>

            </tr>
        </thead>
        <tbody data-bind="foreach: events">


            <tr>

                <td>
                    <a title="Delete List" data-bind="click: $parent.CancelEvent"><i class="fa fa-trash fa-fw"></i></a>
                    <a data-bind="click: $parent.edit_mode, text: editBtnText, css: editBtnClass"></a>


                    <a title="Split List" target="_blank" data-bind="visible: people() > 0, attr: { href: split_url }"><i class="fa fa-exchange fa-fw"></i></a>
                    <a title="Print" target="_blank" data-bind="visible: people() > 0 && time, attr: { href: print_url }"><i class="fa fa-print fa-fw"></i></a>


                </td>
                <td data-bind="text: ID"></td>

                <td data-bind="text: date"></td>
                <td>

                    <!-- ko if: time()==undefined -->
                    <span style="color: red" title="Please complete the plan of the list or delete and create a new one">missing!</span>
                    <!-- /ko -->
                    <!-- ko if: time -->
                    <span data-bind="text: time, visible: !(editable()) "></span>
                    <input class="form-control" type="time" data-bind="value: time, visible: editable" style="width: 120px" />
                    <!-- /ko -->
                </td>
                <td>
                    <span data-bind="text: activity_name"></span>
                    <a  title="Plan Route" data-bind="visible: direction() == 'OUT', attr: { href: 'PlanRoute.aspx?event_fk=' + ID() }" href="PlanRoute.aspx"><i class="fa fa-clock-o"></i></a>

                </td>
                <td>
                    <!-- ko if:people()==0 -->
                    <span style="color: red; font-weight: bold" data-bind="text: people" title="List without passengers cannot be printed"></span>
                    <!-- /ko-->
                    <!-- ko if:people()>0 -->
                    <span data-bind="text: people"></span>
                    <!-- /ko-->
                </td>
                <td>
                    <span data-bind="text: guide_name, visible: !(editable())"></span>


                    <div data-bind="visible: editable()">
                        <select class="form-control margin-bottom-14" data-bind="
    options: $root.guides,
    optionsText: 'name',
    optionsValue: 'ID',
    value: $data.guide_fk,
    optionsCaption: 'Select Guide',
    valueAllowUnset: true

">
                        </select>

                    </div>

                </td>

                <td>

                    <span data-bind="text: comments, visible: !(editable())"></span>
                    <textarea class="form-control" data-bind=" value: comments, visible: editable"></textarea>
                </td>
                <td data-bind="text: date_update"></td>

            </tr>
        </tbody>
    </table>



</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="FooterScripts" runat="Server">

    <script>
        var CATEGORY = 'transport';

    </script>
    <script src="../views_client/view_events.js"></script>
    <script>
        var my = { viewModel: new AppEventModel() };
        ko.applyBindings(my.viewModel);

        $(document).ready(function () {
            //my.viewModel.GetPrintReqUrl();

        });

    </script>
</asp:Content>
