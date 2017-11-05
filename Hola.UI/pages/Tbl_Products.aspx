<%@ Page Title="" Language="C#" MasterPageFile="~/MasterHola.master" AutoEventWireup="true" CodeFile="Tbl_Products.aspx.cs" Inherits="pages_Tbl_Products" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-lg-12">
            <h1 class="page-header"><i class="fa fa-shopping-cart  fa-fw"></i>Products</h1>
        </div>
    </div>

    <div class="panel-heading">
        <h2 class="panel-title">Add New Product</h2>
    </div>

    <div class="panel-body">

        <div class="form-group">


            <label class="col-sm-2 control-label">Name</label>
            <div class="col-sm-9">
                <input type="text" required="required" class="form-control" data-bind="value: new_name" placeholder="Name" />
            </div>
        </div>
        <div class="form-group">

            <label class="col-sm-2 control-label">Code</label>
            <div class="col-sm-9">
                <input type="text" required="required" class="form-control" data-bind="value: new_code" placeholder="Code" />
            </div>
        </div>
        <div class="form-group">
            <label class="col-sm-2 control-label">Type</label>
            <div class="col-sm-9">
                <select id="ddlType" required="required" class="form-control margin-bottom-14" data-bind="
    options: type_options,
    value: new_type,
    optionsCaption: 'Select Type'">
                </select>

            </div>
        </div>
        <div class="form-group">

            <label class="col-sm-2 control-label">Rate</label>
            <div class="col-sm-9">
                <input type="text" required="required" class="form-control" data-bind="value: new_rate" placeholder="Rate" />
            </div>
        </div>
        <div class="form-group">

            <label class="col-sm-2 control-label">Capacity</label>
            <div class="col-sm-9">
                <input type="text" class="form-control" data-bind="value: new_capacity" placeholder="Capacity" />
            </div>

        </div>
        <div class="col-sm-2">
            <button type="button" data-bind="click: add_server" class="btn btn-primary">Submit</button>
        </div>
    </div>



    <table class="table table-striped table-bordered table-hover" id="tbl">
        <thead>
            <tr>

                <th>ID               </th>
                <th>Name              </th>
                <th>Node           </th>
                <th>Type           </th>
                <th>Rate           </th>
                <th>Capacity           </th>

            </tr>
        </thead>
        <tbody data-bind="foreach: products">
            <tr>
                <td data-bind="text: ID"></td>
                <td data-bind="text: name"></td>
                <td data-bind="text: code"></td>
                <td data-bind="text: type"></td>
                <td data-bind="text: rate"></td>
                <td data-bind="text: capacity"></td>
            </tr>
        </tbody>
    </table>


</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="FooterScripts" runat="Server">

    <%--<%: System.Web.Optimization.Scripts.Render("~/bundles/product") %>--%>
    <script src="../views_client/view_product.js"></script>
    <script>


        $(document).ready(function () {

        });
    </script>
</asp:Content>
