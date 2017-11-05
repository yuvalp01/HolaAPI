<%@ Page Async="true" Language="C#" AutoEventWireup="true" Culture="es-ES" CodeFile="Invoice_Print.aspx.cs" Inherits="Invoice_Print" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        tr.row_bottom_solid > td {
            border-bottom: solid;
        }

        tr.row_bottom_double > td {
            border-top: double;
        }

        tr.row_no_border > td {
            border: none;
        }

        .auto-style1 {
            width: 100%;
        }

        .auto-style3 {
            text-align: right;
        }

        .auto-style4 {
            height: 23px;
        }

        .auto-style5 {
            width: 8px;
        }

        .auto-style6 {
            width: 80px;
        }

        .auto-style7 {
            height: 44px;
        }

        .auto-style8 {
            width: 80px;
            height: 44px;
        }

        .auto-style9 {
            height: 44px;
            width: 117px;
        }

        .auto-style10 {
            width: 117px;
        }

        .auto-style11 {
            width: 317px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            &nbsp;<br />
            <br />
            <br />
            <table class="auto-style1" border="0">
                <tr>
                    <td colspan="7" style="text-align: center">
                        <img src="../images/holaShalom.png" /></td>
                </tr>
                <tr class="row_bottom_double">
                    <td colspan="2">
                        <asp:Label ID="Label1" runat="server" Text="Turismo España"></asp:Label>
                    </td>
                    <td class="auto-style3" colspan="5">
                        <asp:Label ID="Label2" runat="server" Text="תיירות והדרכה ברצלונה-קאטלוניה"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">&nbsp;</td>
                    <td class="auto-style3" colspan="5">&nbsp;</td>
                </tr>
                <tr>
                    <td>HOLASHALOM ESPAÑA CIF-B65031320</td>
                    <td colspan="4" rowspan="8">&nbsp;</td>
                    <td>bengalco@holashalom.com</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>PASSATGE MAIOL 8 3-2 08013 BARCELONA</td>
                    <td>(34) 686.686.544</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>Agencia: GC002763</td>
                    <td>(34) 686.179.923</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>FACTURA:<asp:Label ID="lblFactura" runat="server"></asp:Label>
                    </td>
                    <td>
                        <%-- <asp:Label ID="lblOperator" runat="server"></asp:Label>--%>



                        <span data-bind="text: agency_name"></span>




                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>FECHA:
                    <asp:Label ID="lblFecha" runat="server"></asp:Label>
                    </td>
                    <td>
                        <%--                        <asp:Label ID="lblAddress" runat="server"></asp:Label>--%>


                        <span data-bind="text: agency_address"></span>


                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>MES:<asp:Label ID="lblMonth" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label3" runat="server" Text="Israel"></asp:Label>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr class="row_bottom_solid">
                    <td colspan="7">&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="6">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="6">
                        <table border="1" class="auto-style1">
                            <tr>
                                <td>Cant.</td>
                                <td>Concepto</td>
                                <td>PVP/UNI (IVA incluido)</td>
                                <td>IVA % aplicado</td>
                                <td>Base imponible</td>
                            </tr>
                            <tr>
                                <td>1</td>
                                <td>Client <%=Month %></td>
                                <td>
                                    <%=Sum %>
                                </td>
                                <td>10%
                                </td>
                                <td>
                                    <%=Base %>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td class="auto-style5">&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="6" class="auto-style4">
                        <table class="auto-style1">
                            <tr>
                                <td class="auto-style11">
                                    <table border="1" class="auto-style1">
                                        <tr>
                                            <td class="auto-style9">base imponible</td>
                                            <td class="auto-style8">% imp.</td>
                                            <td class="auto-style7">Tasa IVA</td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style10">0,00€</td>
                                            <td class="auto-style6">21%</td>
                                            <td>0,00€</td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style10"><%=Base %></td>
                                            <td class="auto-style6">
                                                <asp:Label ID="lblVAT0" runat="server" Text="10%"></asp:Label>
                                            </td>
                                            <td><%=VAT %></td>
                                        </tr>
                                    </table>
                                </td>
                                <td rowspan="3">&nbsp;</td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="auto-style11">&nbsp;</td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="auto-style11">&nbsp;</td>
                                <td></td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td></td>
                            </tr>
                            <tr>
                                <td class="auto-style11">
                                    <asp:Label ID="Label4" runat="server" Text="TOTAL FACTURA:" Font-Bold="True"></asp:Label>
                                    <span style="font-weight: bold; font-size: 22px; color: green"><%=Sum %></span>
                                </td>
                                <td></td>
                                <td></td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="auto-style11">

                                    <asp:Label ID="Label5" runat="server" Text="Invoice Due Date: "></asp:Label>
                                    <%=DueDate %>
                                </td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>
                        </table>
                    </td>
                    <td class="auto-style4"></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td class="auto-style5">&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <table class="auto-style1">
                            <tr>
                                <td>
                                    <asp:Label ID="Label6" runat="server" Text="Bank details:" Font-Underline="True"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label7" runat="server" Text="IBAN: ES31 0182 2383 5802 0153 0836"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label8" runat="server" Text="SWIFT: BBVAESMMXX"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label9" runat="server" Text="Bank adress: c/ Arago 406-408 Barcelona"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td class="auto-style5">&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td class="auto-style5">&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
            </table>
            <br />
            <br />


        </div>
    </form>

    <script src="../scripts/jquery-2.2.1.min.js"></script>
    <script src="../scripts/knockout-3.4.0.js"></script>


    <script type="text/javascript">



    </script>

    <script>

        //function Agency(data) {
        //    this.ID = ko.observable(data.ID);
        //    this.name = ko.observable(data.name);
        //    this.address = ko.observable(data.address);
        //}

        var api_url = '<%=ConfigurationManager.AppSettings["api_url"] %>';
        var agency_fk = '<%=Agency_fk%>';
        var _url_agency = api_url + '/api/agencies/' + agency_fk;

        function AgencyViewModel(data) {

            var self = this;

            self.agency_name = ko.observable();
            self.agency_address = ko.observable();


            $.getJSON(_url_agency, function (allData) {
                self.agency_name(allData.name);
                self.agency_address(allData.address);
            });

        }
        // Activates knockout.js
        ko.applyBindings(new AgencyViewModel());

        //$(document).ready(function () {

        //});

    </script>

</body>

</html>
