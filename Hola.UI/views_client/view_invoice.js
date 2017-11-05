
var _url_invoice_summary = url_invoice+ '/GetInvoiceSummary?';
var _url_months = '../months.ashx?';
var _url_print = '../print/Invoice_Print.aspx?';

function Flight(data) {
    this.num = ko.observable(data.num);
    var _date = new Date(data.date).yyyymmdd();
    this.date = ko.observable(_date);
    this.time = ko.observable(data.time);
    this.sum = ko.observable(data.sum);
    this.selected = ko.observable(false);
}
function Agency(data) {
    this.ID = ko.observable(data.ID);
    this.name = ko.observable(data.name);
    this.address = ko.observable(data.address);
}
function Month(data) {
    this.ID = ko.observable(data.ID);
    this.name = ko.observable(data.name);

}

function InvoiceLine(data) {
    this.date_arr = ko.observable(data.date_arr);
    this.num_arr = ko.observable(data.num_arr);
    this.product = ko.observable(data.product);
    this.people = ko.observable(data.people);
    this.rate = ko.observable(data.rate);
    this.sum = ko.observable(data.sum);

}

function InvoiceViewModel(data) {


    var self = this;
    ///

    //arrays
    self.agencies = ko.observableArray([]);
    self.months = ko.observableArray([]);
    self.years = ko.observableArray(['2016', '2017', '2018'])

    //self.invoice_lines = ko.observableArray([]);


    //Get default values
    var _date = new Date().yyyymmdd();
    var default_month = new Date().getMonth();
    var default_year = new Date().getFullYear();


    //fields
    self.agency_fk = ko.observable();
    self.month = ko.observable(default_month);
    self.year = ko.observable(default_year);

    //observable urls
    self.print_url = ko.observable();
    self.invoice_url = ko.observable();



   self.total = ko.observable();


    //self.total = ko.computed(function () {
    //    var _total = 0;
    //    for (var i = 0; i < self.invoice_lines().length; ++i) {
    //        _total += self.invoice_lines()[i].sum();
    //    }
    //    return _total;
    //});

   self.find = function (fk) {
       return ko.utils.arrayFirst(self.agencies(), function (agency) {
           return agency.agency_fk === fk;
       });
   }


    self.print_url = ko.computed(function () {
        if (self.month() && self.year().length > 0 && self.agency_fk() && self.total() > 0) {
            //var fk = self.agency_fk();
            ////var xxxx = self.find(fk);

            ////selectedId = 2;

            //var category = ko.utils.arrayFirst(self.agencies, function (agency) {
            //    return agency.agency_fk === fk;
            //});
            //debugger;
            //console.log(xxxx);
            //console.log(self.agencies()[0].name());

            return _url_print +
                 'month=' + self.month() +
                 '&year=' + self.year() +
                 '&agency_fk=' + self.agency_fk() +
                 '&total=' + self.total();
        }
    }, this);



    //get agencies from server:
    $.getJSON(url_agencies, function (allData) {
        var mappedData = $.map(allData, function (item) {
            return new Agency(item);
        });
        self.agencies(mappedData);
    });

    //get months from server:
    $.getJSON(_url_months, function (allData) {
        var mappedData = $.map(allData, function (item) {
            return new Month(item);
        });
        self.months(mappedData);
    });


    self.agency_fk.subscribe(function (new_) {
        self.show_table();

    });

    self.month.subscribe(function (new_) {
        //  self.update_print_url();
        self.show_table();

    });


    self.year.subscribe(function (new_) {
        self.show_table();
    });


    var tblInvoice;

    self.show_table = function () {

        if (self.month() && self.year().length > 0 && self.agency_fk()) {

            var _url = _url_invoice_summary +
            'month=' + self.month() +
            '&year=' + self.year() +
            '&agency_fk=' + self.agency_fk();

            if (tblInvoice) {
                tblInvoice.ajax.url(_url).load();
            }

            else {
                tblInvoice = $('#tblInvoice').DataTable({
                    //"language": {
                    //    "decimal": ",",
                    //    "thousands": "."
                    //},
                    "ajax": _url,
                    //"data": mappedData,
                    "searching": false,
                    "info": false,
                    "footer": false,
                    "paging": false,
                    "sAjaxDataProp": "",

                    "columns": [

                        { "data": "date_arr", },
                        { "data": "num_arr" },
                        { "data": "product" },
                        { "data": "people" },
                        { "data": "rate", },
                        { "data": "sum" },

                    ],
                    //"createdRow": function (row, data, index) {
                    //    alert($('td', row).eq(4).val());
                    //    var val = $('td', row).eq(4).val();
                    //    $('td', row).eq(4).val(val + '€');

                    //},
                    "columnDefs": [
                     {
                         "render": function (data, type, row) {

                             var d = new Date(data);
                             return d.yyyymmdd();
                         },
                         "targets": 0
                     },
                    ],
                    dom: 'Bfrtip',
                    language: {
                        "emptyTable": "No activity for this agency in the selected month"
                    },
                    buttons: [
                            //'copy',
                             { extend: 'excel', text: '<i class="fa fa-file-excel-o fa-fw"></i><span>Export Table</span>' },
                             //{ extend: 'print', text: '<i class="fa fa-print fa-fw"></i><span>Print Table</span>' },
                           
                            // { extend: 'pdf', text: 'Print Table' },
                            //'excel',
                            ////'csv',
                            ////'pdf',
                            //'print'
                    ]
                });


                $("div.dt-buttons").append($('#btnPrintInvoice'));
                $('#btnPrintInvoice').show();

            }


            tblInvoice.on('xhr', function () {
                var json = tblInvoice.ajax.json();
                var _total = 0;
                $.each(json, function (index, value) {
                   
                    _total = _total + value.sum;
                });
                self.total(_total)
                //alert("total= " + _total);
                //debugger;

            });

  
            tblInvoice.on('draw.dt', function () {
                var sum_column = tblInvoice.columns(5);
                if (sum_column.data().count() > 0) {
                    $(sum_column.footer()).html(self.total());
                    $("div.dt-buttons").show();
                }
                else {
                    $(sum_column.footer()).html('');
                    $("div.dt-buttons").hide();
                    //self.total(0);
                }
            });

        };

    }
}

