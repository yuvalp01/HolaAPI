
var col_names = 1;
var col_date_arr = 4;
var col_date_dep = 6;
var col_hotel = 7;
var col_agency = 8;
var col_phone = 9;




function Agency(data) {
    this.ID = ko.observable(data.ID);
    this.name = ko.observable(data.name);
    this.address = ko.observable(data.address);
}

function Product(data) {
    this.ID = ko.observable(data.ID);
    this.name = ko.observable(data.name);
}

function Hotel(data) {
    this.ID = ko.observable(data.ID);
    this.name = ko.observable(data.name);
}


function Sale(data) {
    this.PNR = ko.observable(data.PNR);
    this.product_fk = ko.observable(data.product_fk);
    this.persons = ko.observable(data.persons);
    this.price = ko.observable(data.price);
    this.sale_type = ko.observable(data.sale_type);
    this.date_sale = ko.observable(data.date_sale);
    this.date_update = ko.observable(data.date_update);
    this.paid = ko.observable(data.paid);

}


function SaleViewModel(data) {

    var self = this;
    ///

    //arrays
    self.agencies = ko.observableArray([]);
    self.hotels = ko.observableArray([]);
    // self.sales = ko.observableArray([]);


    //fields 
    self.date_arr = ko.observable();
    self.date_dep = ko.observable();
    self.agency_fk = ko.observable();
    self.product_fk = ko.observable();
    self.sale_type = ko.observable('External');

    self.PNR = ko.observable();
    self.names = ko.observable();
    self.persons = ko.observable();
    self.persons_max = ko.observable();


    self.agency_name = ko.observable();
    self.hotel_name = ko.observable();

    //fields for new record
    self.product_fk_new = ko.observable();
    self.type_sale_new = ko.observable();

    //get agencies from server:
    $.getJSON(url_agencies, function (allData) {
        var mappedData = $.map(allData, function (item) {
            return new Agency(item);
        });
        self.agencies(mappedData);
    });
    //get hotels from server:
    $.getJSON(url_hotels, function (allData) {
        var mappedData = $.map(allData, function (item) {
            return new Hotel(item);
        });
        self.hotels(mappedData);
    });


    /// Subscriptions

    self.date_arr.subscribe(function (_new) {
        //if (_new == undefined) { _new = '' }
        dataTable.column(col_date_arr).search(_new).draw();
    });

    self.date_dep.subscribe(function (_new) {
        //if (_new == undefined) { _new = '' }
        dataTable.column(col_date_dep).search(_new).draw();
    });

    self.hotel_name.subscribe(function (_new) {
        if (_new == undefined) { _new = '' }
        dataTable.column(col_hotel).search(_new).draw();
    });

    self.agency_name.subscribe(function (_new) {
        //var agency = my.viewModel.agency_name();
        //var hotel = my.viewModel.hotel_name();
        if (_new == undefined) { _new = '' }
        dataTable.column(col_agency).search(_new).draw();
    });


    self.remove_server = function (hotel) {
    }


    var tbl;
    self.show_table = function () {
        if (self.PNR()) {
            var _url = url_sales +
            '?PNR=' + self.PNR();

            if (tbl) {
                tbl.ajax.url(_url).load();
            }

            else {
                tbl = $('#tbl_sales').DataTable({
                    "ajax": _url,
                    "searching": false,
                    "info": false,
                    "footer": false,
                    "paging": false,
                    "sAjaxDataProp": "",

                    "columns": [

                        { "data": "product_name", },
                        { "data": "persons" },
                        { "data": "sale_type" },

                    ],

                    "dom": '<"toolbar">rtip',
                });
                //$("div.toolbar").append('<a class="dt-button buttons-copy buttons-html5" tabindex="0" aria-controls="tbl"><span>TEST</span></a>');

            }


            //tbl.on('xhr', function () {
            //    var json = tbl.ajax.json();
            //    debugger;

            //});

        };

    }

    dataTable = $('#tblClients').DataTable({
        "drawCallback": function (settings) {
            $('[data-toggle="popover"]').popover();
        },
        responsive: true,
        "ajax": url_clients,
        "pageLength": 10,
        "bLengthChange": false,
        "sAjaxDataProp": "",
        "autoWidth": false,
        //"columnDefs": [{
        //    "targets": col_names,
        //    "searchable": false,
        //    "sortable": false,
        //    "width": "30px"

        //}],
        "columns": [

                    { "data": "PNR", 'visible': false },//1
                    { "data": "PNR" },//1
                    { "data": "names", "width": "20px" },//2
                    { "data": "PAX", "width": "40px" },//3
                    { "data": "num_arr", "width": "100px" },//4
                    { "data": "date_arr", "width": "110px" },//5
                    { "data": "num_dep", "width": "70px" },//6
                    { "data": "date_dep", "width": "110px" },//7
                    { "data": "hotel_name", "width": "110px" },//8
                    { "data": "agency_name", "width": "110px" },//9
                    { "data": "phone", 'visible': false },//9

        ],
        "columnDefs": [
                    {
                        "targets": col_names,
                        "render": function (data, type, row) {
                            if (row.phone != "") {
                                return data + ' <a class="phone" data-toggle="popover" role="button" data-trigger="focus" tabindex="0"  title="Phone Number" data-content="' + row.phone + '"><i class="fa fa-phone-square fa-lg"></i></a>';
                            }
                            else {
                                return data;
                            }
                        },

                    },
                    {
                        "render": function (data, type, row) {
                            return new Date(data).yyyymmdd();

                        },
                        "targets": col_date_arr
                    },
                    {
                        "render": function (data, type, row) {
                            if (data) {
                                return new Date(data).yyyymmdd();
                            } else { return data; }
                        },
                        "targets": col_date_dep
                    }
        ],
        dom: 'frtipB',

        buttons: [
                //'copy',
                { extend: 'excel', text: '<i class="fa fa-file-excel-o fa-fw"></i><span>Export Table</span>' },
                //'pdf',
                //'print'
        ],

        "drawCallback": function (settings) {
            $('[data-toggle="popover"]').popover();
        },

    });



    $('[data-toggle="tooltip"]').tooltip({
        trigger: 'hover',
        container: 'body'

    });

}






