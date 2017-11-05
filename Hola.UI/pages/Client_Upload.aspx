<%@ Page Title="Upload File" MaintainScrollPositionOnPostback="true" Language="C#" MasterPageFile="~/MasterHola.master" AutoEventWireup="true" CodeFile="Client_Upload.aspx.cs" Inherits="pages_Client_Upload" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-lg-12">
            <h1 class="page-header">Upload File</h1>
            <asp:Panel ID="pnlFeedback" runat="server" ClientIDMode="Static" Style="display: none" CssClass="alert  alert-dismissable alert-danger">
                <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                <asp:Label ID="lblFeedback" Text="" ClientIDMode="Static" runat="server" EnableViewState="false" />


            </asp:Panel>
            <%--            <div id="message_success" style="display: none" class="alert alert-success alert-dismissable">
                
                The client: <b id="new_pnr" data-bind="text: names"></b>was successfuly added.  
                <a data-bind="    attr: { href: 'Client_Sales.aspx?search=' + PNR() }" href="Client_Sales.aspx" target="_blank" class="alert-link">Click here to add service</a>.
    
 
                           
            </div>   --%>
        </div>
        <!-- /.col-lg-12 -->
    </div>

    <form name="form1" runat="server">

        <div class="well">

            <div class="form-group">
                <label class="control-label">Choose Agency*</label>

                <asp:DropDownList ID="ddlAgencies" DataValueField="ID" ClientIDMode="Static" DataTextField="name" runat="server" required="required" CssClass="form-control">
                </asp:DropDownList>

            </div>
                        <div class="form-group">
                <label class="control-label">Choose Transportation</label>

                <asp:DropDownList Enabled="false" ID="ddlTransport" DataValueField="ID" ClientIDMode="Static" DataTextField="name" runat="server" required="required" CssClass="form-control">
                    <asp:ListItem Text="Bus: Airport --> Hotel BCN - return" />
                </asp:DropDownList>

            </div>

            <div class="form-group">
                <label class="control-label">Select csv File* </label>
                <asp:LinkButton Style="margin-left: 3px" Text="Download Example" ID="lnkDownloadExample" runat="server" OnClick="lnkDownloadExample_Click" />

                <asp:FileUpload ID="FileUpload1" accept=".csv" ClientIDMode="Static" runat="server" />

            </div>

            <div class="form-group">
                <%--                <label class="control-label">Select File*</label>--%>

                <asp:Button ID="btnUpload" CssClass="btn btn-primary" ClientIDMode="Static" runat="server" Text="Upload" OnClick="btnUpload_Click" Enabled="false" />


            </div>
        </div>


        <div>
        </div>
        <asp:GridView ID="GridView1" CssClass="table table-striped table-bordered table-hover  order-column compact" Width="1400px" Style="table-layout: fixed;" runat="server" AutoGenerateColumns="False" OnRowDataBound="GridView1_RowDataBound" OnRowCommand="GridView1_OnRowCommand">
            <Columns>


                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <asp:Button ID="btnInsert" runat="server" CssClass="form-control btn btn-primary" CausesValidation="false" CommandName="InsertClient"
                            Text="Insert"
                            CommandArgument='<%#Eval("PNR") + "~" +Eval("names")+ "~" +Eval("phone")+ "~" +Eval("date_arr")+ "~" +Eval("num_arr")+ "~" +Eval("PAX")+ "~" +Eval("date_dep")+ "~" +Eval("num_dep") + "~" +Eval("comments") %>' />
                        <%--                            CommandArgument='<%#Eval("PNR") + "~" +Eval("names")+ "~" +Eval("PAX")+ "~" +Eval("num_arr")+ "~" +Eval("date_arr")+ "~" +Eval("num_dep")+ "~" +Eval("date_dep")+ "~" +Eval("phone") + "~" +Eval("comments") %>' />--%>
                    </ItemTemplate>
                </asp:TemplateField>


                <asp:TemplateField HeaderText="Hotels" ItemStyle-Width="270px" HeaderStyle-Width="270px">
                    <ItemTemplate>
                        <asp:DropDownList CssClass="form-control" ID="ddlHotels" runat="server"></asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:BoundField DataField="hotel_name" HeaderText="hotel_name" ItemStyle-Width="200px" FooterStyle-Width="200px" />
                <asp:BoundField DataField="PNR" HeaderText="PNR" />
                <asp:BoundField DataField="names" HeaderText="names">
                    <ItemStyle Width="200px" />
                    <HeaderStyle Width="200px" />
                </asp:BoundField>
                <asp:BoundField DataField="PAX" HeaderText="PAX" />
                <asp:BoundField DataField="num_arr" HeaderText="num_arr" />
                <asp:BoundField DataField="date_arr" HeaderText="date_arr" DataFormatString="{0:yyyy-MM-dd}" />
                <asp:BoundField DataField="num_dep" HeaderText="num_dep" />
                <asp:BoundField DataField="date_dep" HeaderText="date_dep" DataFormatString="{0:yyyy-MM-dd}" />
                <asp:BoundField DataField="phone" HeaderText="phone" />
                <asp:BoundField DataField="comments" HeaderText="comments" />







            </Columns>

        </asp:GridView>

    </form>



</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="FooterScripts" runat="Server">

    <!-- Page-Level Demo Scripts - Tables - Use for reference -->
    <script>
        $(document).ready(function () {

            $('#FileUpload1').change(function () {
                validate_btn();
                //if ($('#ddlAgencies').val() == '0') {
                //    $('#lblFeedback').text('Please choose agency');
                //}
                //else {
                //    $('#lblFeedback').text('');
                //}
            });


            $('#ddlAgencies').change(function () {
                validate_btn();

            });

            function validate_btn() {

                if ($('#ddlAgencies').val() == '0' || $('#FileUpload1').val() == '') {
                    $('#btnUpload').attr('disabled', true);
                    $('#lblFeedback').text('Please choose agency and file');
                    $('#pnlFeedback').show();

                }
                else {
                    $('#btnUpload').attr('disabled', false);
                    $('#lblFeedback').text('');
                    $('#pnlFeedback').hide();

                }
            }
        });
    </script>
    <script>

    </script>
</asp:Content>
