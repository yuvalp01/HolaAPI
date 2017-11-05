<%@ Page Title="" Language="C#" MasterPageFile="~/MasterHola.master" AutoEventWireup="true" CodeFile="Tbl_Guides.aspx.cs" Inherits="pages_Tbl_Guides" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-lg-12">
            <h1 class="page-header"><i class="fa fa-bed fa-fw"></i>Guides</h1>
        </div>
    </div>

    <div class="panel-heading">
        <h2 class="panel-title">Add a New Guide</h2>
    </div>

    <div class="panel-body">

        <div class="form-group">

            <label class="col-sm-2 control-label">Guide Name</label>
            <div class="col-sm-9">
                <input type="text" required="required" class="form-control" data-bind="value: new_name" placeholder="Name" />
            </div>
        </div>
        <div class="form-group">
            <label class="col-sm-2 control-label">Guide Phone</label>
            <div class="col-sm-9">
                <input type="tel" required="required" class="form-control" data-bind="value: new_phone" placeholder="Phone" />
            </div>
        </div>

        <div class="form-group ">
            <label class="control-label col-sm-2">
                <button type="button" data-bind="click: add_server" class="btn btn-primary">Submit</button>
            </label>
        </div>

    </div>



    <table class="table table-striped table-bordered table-hover" id="tbl">
        <thead>
            <tr>

                <th>ID               </th>
                <th>Name              </th>
                <th>Phone Number</th>


            </tr>
        </thead>
        <tbody data-bind="foreach: guides">
            <tr>
                <td data-bind="text: ID"></td>
                <td data-bind="text: name"></td>
                <td data-bind="text: phone"></td>
            </tr>
        </tbody>
    </table>


</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="FooterScripts" runat="Server">
    <script src="../scripts/knockout.mapping-latest.js"></script>
    <script src="../views_client/view_guide.js"></script>
    <%--    <%: System.Web.Optimization.Scripts.Render("~/bundles/guide") %>--%>

    <script>

        $(document).ready(function () {

        });
    </script>
</asp:Content>
