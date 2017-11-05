<%@ Page Title="Upcoming Tours" Language="C#" MasterPageFile="~/MasterHola.master" AutoEventWireup="true" CodeFile="EventsTours.aspx.cs" Inherits="pages_TourPlan" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

        <script>
        var isChrome = !!window.chrome && !!window.chrome.webstore;
        if (!isChrome) {
            window.location = "UseChrome.aspx";
        }
    </script>
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
    0% { transform: rotate(0deg); }
    100% { transform: rotate(360deg); }
}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="row">
        <div class="col-lg-12">
            <h1 class="page-header"><i class="fa fa-calendar fa-fw"></i>Upcoming Tours</h1>
        </div>
    </div>

    <div class="panel-heading">
        <h2 class="panel-title">Add a Tour</h2>

    </div>

    <div class="panel-body">

        <div class="form-group">


            <label class="col-sm-2 control-label">Tour Name*</label>
            <div class="col-sm-10">

                <select required="required" id="ddlActivities" class="form-control margin-bottom-14" data-bind="
    options: activities,
    optionsText: 'name',
    optionsValue: 'ID',
    value: new_activity_fk,
    optionsCaption: 'Select Tour',
    valueAllowUnset: true
">
                </select>

            </div>
        </div>
        <div class="form-group">
            <label class="col-sm-2 control-label">Date*</label>
            <div class="col-sm-10">
                <input required="required" type="text" class="form-control date" data-bind="value: new_date" placeholder="Tour Date" />
            </div>
        </div>
        <div class="form-group">
            <label class="col-sm-2 control-label">Time*</label>
            <div class="col-sm-10">
                <input required="required" type="time" class="form-control" data-bind="value: new_time" />
            </div>
        </div>
        <div class="form-group">
            <label class="col-sm-2 control-label">Guide</label>
            <div class="col-sm-10">
                <select id="ddlGuides" class="form-control margin-bottom-14" data-bind="
    options: guides,
    optionsText: 'name',
    optionsValue: 'ID',
    value: new_guide_fk,
    optionsCaption: 'Select Guide',
    valueAllowUnset: true
">
                </select>
            </div>
        </div>
        <div class="form-group">


            <label class="col-sm-2 control-label">Comments</label>
            <div class="col-sm-10">
                <textarea class="form-control" data-bind="value: new_comments">

                </textarea>

            </div>


        </div>
        <div class="form-group">
            <div class="col-sm-2">
                <button type="button" data-bind="click: add_server" class="btn btn-primary">Submit</button>

            </div>
        </div>

    </div>



    <table class="table table-striped table-bordered table-hover" id="tbl">
        <thead>
            <tr>
                <th></th>
                <th>Date         </th>
                <th>Time       </th>
                <th>Tour       </th>
                <th>Guide      </th>
                <th>Comments       </th>
                <th>Update Date      </th>
            </tr>
            <tr id="trLoading" style="display:inline-grid">
                <td colspan="7" style="text-align:center;">
                        <%--<div class="loader"></div>--%>
                    Loading...</td>

            </tr>
        </thead>
        <tbody data-bind="foreach: events">


            <tr>

                <td>
                    <button data-bind="click: $parent.edit_mode, text: editBtnText, css: editBtnClass"></button>
                    <button class="btn btn-danger btn-circle" title="Delete Plan" data-bind="click: $parent.CancelEvent">Del</button>
                </td>
                <td data-bind="text: date"></td>
                <td>

                    <span data-bind="text: time, visible: !(editable())"></span>
                    <input class="form-control" type="time" data-bind="value: time, visible: editable" style="width: 120px" />

                </td>
                <td data-bind="text: activity_name"></td>


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
        var CATEGORY = 'tour';

    </script>


    <script src="../views_client/view_events.js"></script>
    <script>
        var my = { viewModel: new AppEventModel() };
        ko.applyBindings(my.viewModel);

        $(document).ready(function () {
            $(".date").datepicker({ dateFormat: 'yy-mm-dd', minDate: -30 });

        });

    </script>
</asp:Content>
