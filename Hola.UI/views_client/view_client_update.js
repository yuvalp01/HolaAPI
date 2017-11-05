
var col_names = 2;
var col_date_arr = 5;
var col_date_dep = 7;
var col_hotel = 8;
var col_agency = 9;
var col_phone = 10;


function Client(data) {
    this.PNR = ko.observable(data.PNR);
    this.names = ko.observable(data.names);
    this.PAX = ko.observable(data.PAX);
    this.num_arr = ko.observable(data.num_arr);
    var da = new Date(data.date_arr);
    this.date_arr = ko.observable(da.yyyymmdd());
    this.num_dep = ko.observable(data.num_dep);
    var dd = new Date(data.date_dep);
    this.date_dep = ko.observable(dd.yyyymmdd());
    this.phone = ko.observable(data.phone);
    this.hotel_fk = ko.observable(data.hotel_fk);
    this.hotel_name = ko.observable(data.hotel_name);
    this.agency_fk = ko.observable(data.agency_fk);
    this.agency_name = ko.observable(data.agency_name);
    this.oneway = ko.observable(data.oneway);
    this.comments = ko.observable(data.comments);
    this.canceled = ko.observable(data.canceled);

}


function Hotel(data) {
    this.ID = ko.observable(data.ID);
    this.name = ko.observable(data.name);
    this.address = ko.observable(data.address);
}
function Agency(data) {
    this.ID = ko.observable(data.ID);
    this.name = ko.observable(data.name);
    this.address = ko.observable(data.address);
}

function Product(data) {
    this.ID = ko.observable(data.ID);
    this.name = ko.observable(data.name);
    this.rate = ko.observable(data.rate);
    this.category = ko.observable(data.category);
    this.subcat = ko.observable(data.subcat);
}
function Flight(data) {
    this.num = ko.observable(data.num);
    var _date = new Date(data.date).yyyymmdd();
    this.date = ko.observable(_date);
    var t = data.time;
    this.time = ko.observable(t.HHmm());
    this.direction = ko.observable(data.direction);

    this.details = ko.dependentObservable(function () {
        return (this.date() + ' - ' + this.num() + ' - ' + this.time())
    }, this);
}


function Sale(data, isEditMode) {
    this.ID = ko.observable(data.ID);
    this.PNR = ko.observable(data.PNR);
    this.agency_fk = ko.observable(data.agency_fk);
    this.product_fk = ko.observable(data.product_fk);
    this.product_name = ko.observable(data.product_name);

    this.persons = ko.observable(data.persons);
    this.remained_pay = ko.observable(data.remained_pay);
    this.sale_type = ko.observable(data.sale_type);
    this.date_sale = ko.observable(data.date_sale);
    this.date_update = ko.observable(data.date_update);
    this.canceled = ko.observable(data.canceled);
    this.comments = ko.observable(data.comments);
    this.editable = ko.observable(isEditMode);
    this.editBtnClass = ko.observable('btn btn-circle btn-info');
    this.editBtnText = ko.observable('Edit');

}


function SaleViewModel(data) {

    var self = this;

    //arrays
    self.clients = ko.observableArray([]);
    self.agencies = ko.observableArray([]);
    self.products = ko.observableArray([]);
    self.sales = ko.observableArray([]);
    self.trans = ko.observableArray([]);
    self.editable = ko.observable(false);
    self.hotels = ko.observableArray([]);
    self.flights = ko.observableArray([]);

    self.clients_filter = ko.observableArray([]);

    //fields - common
    self.PNR = ko.observable();
    self.names = ko.observable();
    self.agency_fk = ko.observable();
    self.product_fk = ko.observable();

    self.rate = ko.observable();
    self.tour_subcat = ko.observable();

    //Client search

    self.agency_name_search = ko.observable();
    self.hotel_name_search = ko.observable();
    self.date_arr_search = ko.observable();
    self.date_dep_search = ko.observable();

    self.date_arr_search.subscribe(function (_new) {

        dataTable.column(col_date_arr).search(_new).draw();

    });

    self.date_dep_search.subscribe(function (_new) {
        //if (_new == undefined) { _new = '' }
        dataTable.column(col_date_dep).search(_new).draw();
    });

    self.hotel_name_search.subscribe(function (_new) {
        if (_new == undefined) { _new = '' }
        dataTable.column(col_hotel).search(_new).draw();
    });

    self.agency_name_search.subscribe(function (_new) {
        if (_new == undefined) { _new = '' }
        dataTable.column(col_agency).search(_new).draw();
    });

    //End client search



    //fields - client 
    self.hotel_fk = ko.observable();
    self.PAX = ko.observable();
    self.phone = ko.observable();
    self.oneway = ko.observable(false);
    self.comments = ko.observable('');
    self.date_arr = ko.observable()
    self.date_dep = ko.observable()
    self.num_arr = ko.observable();
    self.num_dep = ko.observable();

    //field - transportation
    self.trans_product_fk = ko.observable('1');
    self.trans_comments = ko.observable();
    self.trans_remained_pay = ko.observable();
    self.trans_ID = ko.observable();

    //fileds - sale
    self.sale_type = ko.observable('external');
    self.sale_comments = ko.observable();


    self.persons = ko.observable();
    self.persons_max = ko.observable();
    self.feedback_sale = ko.observable();
    self.feedback_trans = ko.observable();


    self.agency_fk_filter = ko.observable();
    self.date_arr_filter = ko.observable();
    self.date_dep_filter = ko.observable();
    self.search_term_filter = ko.observable();

    self.remained_pay = ko.observable();
    self.calc_total = ko.computed({

        read: function () {
            if (self.sale_type() == 'external') {
                self.remained_pay(0);
                return 0;
            }
            else {
                if (!self.product_fk()) {
                    self.remained_pay('');
                    return '';
                }
                else {
                    var _calc = self.persons() * self.rate();
                    self.calc_total(_calc);
                    return _calc;

                }
            }
        },
        write: function (value) {
            console.log(value);
            self.remained_pay(value);
            return value;
        },
        owner: this
    });

    //UX
    self.editBtnClass = ko.observable('btn btn-circle btn-info');
    self.editBtnText = ko.observable('Edit');


    ////flights arrival + date filter:
    //self.clients_filter = ko.computed(function () {
    //    return ko.utils.arrayFilter(self.clients(), function (client) {
    //        var agency_fk_filter = self.agency_fk_filter();
    //        var date_arr_filter = self.date_arr_filter();
    //        var search_term_filter = self.search_term_filter();

    //        //no filter - return all
    //        if (!agency_fk_filter && !date_arr_filter && !search_term_filter) {
    //            return true;
    //        }
    //        //only agency
    //        if (agency_fk_filter && !date_arr_filter && !search_term_filter) {
    //            return client.agency_fk() == agency_fk_filter;
    //        }
    //        //only arrival
    //        if (!agency_fk_filter && date_arr_filter && !search_term_filter) {
    //            return client.date_arr() == date_arr_filter;
    //        }
    //        //only search term
    //        if (!agency_fk_filter && !date_arr_filter && search_term_filter) {
    //            search_term_filter = search_term_filter.toLowerCase();
    //            var names = client.names().toLowerCase();
    //            var PNR = client.PNR().toLowerCase();
    //            if (names.indexOf(search_term_filter) != -1 || PNR.indexOf(search_term_filter) != -1) {
    //                return true;
    //            }
    //        }
    //        //agency and arrival
    //        if (agency_fk_filter && date_arr_filter && !search_term_filter) {
    //            return client.agency_fk() == agency_fk_filter && client.date_arr() == date_arr_filter;
    //        }
    //        //agency and search term
    //        if (agency_fk_filter && !date_arr_filter && search_term_filter) {
    //            search_term_filter = search_term_filter.toLowerCase();
    //            var names = client.names().toLowerCase();
    //            var PNR = client.PNR().toLowerCase();
    //            if ((names.indexOf(search_term_filter) != -1 || PNR.indexOf(search_term_filter) != -1) && client.agency_fk() == agency_fk_filter) {
    //                return true;
    //            }
    //        }
    //        //arrival and search term
    //        if (!agency_fk_filter && date_arr_filter && search_term_filter) {
    //            search_term_filter = search_term_filter.toLowerCase();
    //            var names = client.names().toLowerCase();
    //            var PNR = client.PNR().toLowerCase();
    //            if ((names.indexOf(search_term_filter) != -1 || PNR.indexOf(search_term_filter) != -1) && client.date_arr() == date_arr_filter) {
    //                return true;
    //            }
    //        }
    //        //all filters
    //        if (agency_fk_filter && date_arr_filter && search_term_filter) {

    //            search_term_filter = search_term_filter.toLowerCase();
    //            var names = client.names().toLowerCase();
    //            var PNR = client.PNR().toLowerCase();

    //            if ((names.indexOf(search_term_filter) != -1 || PNR.indexOf(search_term_filter) != -1) && client.date_arr() == date_arr_filter && client.agency_fk() == agency_fk_filter) {
    //                return true;
    //            }
    //        }
    //    });
    //});
    ////};



    //get clients from server:
    self.get_clients = function () {
        $.getJSON(url_clients, function (allData) {
            var mappedData = $.map(allData, function (item) {
                return new Client(item);
            });

            $('#trLoading').hide();
            self.clients(mappedData);
            if (dataTable) {
                dataTable.destroy();
                $('.phone').remove();
            }

            dataTable = $('#tblClients').DataTable({
                "bLengthChange": false,
                "columnDefs": [
                            {

                                "targets": col_phone,
                                "visible": false
                            },
                            {
                                "render": function (data, type, row) {
                                    if (row[col_phone] != "") {
                           
                                        return data + ' <a class="phone" data-toggle="popover" role="button" data-trigger="focus" tabindex="0"  title="Phone Number" data-content="' + row[col_phone] + '"><i class="fa fa-phone-square fa-lg"></i></a>';
                                    }
                                    else {
                                        return data;
                                    }

                                },
                                "targets": col_names
                            },

                ],
                "drawCallback": function (settings) {
                    $('[data-toggle="popover"]').popover();
                },
            });

        });

    };


    //get agencies from server:
    $.getJSON(url_agencies, function (allData) {
        var mappedData = $.map(allData, function (item) {
            return new Agency(item);
        });
        self.agencies(mappedData);
    });
    //get agencies from server:
    $.getJSON(url_hotels, function (allData) {
        var mappedData = $.map(allData, function (item) {
            return new Hotel(item);
        });
        self.hotels(mappedData);
    });
    //get products from server:
    $.getJSON(url_products, function (allData) {
        var mappedData = $.map(allData, function (item) {
            return new Product(item);
        });
        self.products(mappedData);
    });

    //get flights from server:
    $.getJSON(url_flights, function (allData) {
        var mappedData = $.map(allData, function (item) {
            return new Flight(item);
        });
        self.flights(mappedData);
    });

    //flights arrival + date filter:
    self.flights_filter_arr = ko.computed(function () {
        return ko.utils.arrayFilter(self.flights(), function (flight) {
            return flight.date() == self.date_arr() && flight.direction() == 'IN';
        });
    });

    //flights departure + date filter:
    self.flights_filter_dep = ko.computed(function () {
        return ko.utils.arrayFilter(self.flights(), function (flight) {
            return flight.date() == self.date_dep() && flight.direction() == 'OUT';
        });
    });

    //get services from server:
    $.getJSON(url_products, function (allData) {

        var mappedData = $.map(allData, function (item) {
            return new Product(item);
        });
        self.products(mappedData);


    });

    self.products_tour = ko.computed(function () {
        return ko.utils.arrayFilter(self.products(), function (product) {
            return product.category() == 'tour';
        });
    });

    self.products_trans = ko.computed(function () {
        return ko.utils.arrayFilter(self.products(), function (product) {
            return product.category() == 'transport';
        });
    });




    self.product_fk.subscribe(function (_new) {

        var match = ko.utils.arrayFirst(self.products(), function (item) {
            return item.ID() === _new;
        });
        if (match) {
            self.rate(match.rate());
            self.tour_subcat(match.subcat());
        }
        else {
            self.rate('');
        }
    });

    ///Sales

    self.load_sales = function () {

        self.feedback_sale('');
        var _url = url_sales + "/getSales/" + self.agency_fk() + '/' + self.PNR() + "/tour,other";
        $.getJSON(_url, function (allData) {
            var mappedData = $.map(allData, function (item) {
                return new Sale(item);
            });
            self.sales(mappedData);
        });
    }


    self.add = function () {

        if (isValidContainer('modal_sale')) {
            //check if already exists.
            var match_product = ko.utils.arrayFirst(self.sales(), function (item) {
                return item.product_fk() === self.product_fk();
            });

            if (match_product && self.tour_subcat() != 'other') {
                self.feedback_sale('Product already exists');
                return;
            }

            var _sale = {
                PNR: self.PNR(),
                agency_fk: self.agency_fk(),
                product_fk: self.product_fk(),
                sale_type: self.sale_type(),
                persons: self.persons(),
                comments: self.sale_comments(),
                //remained_pay: self.remained_pay(),
                //TODO: comment in/out to price model
                remained_pay: 0,
            };

            $.post(url_sales, _sale, function (sale_from_server) {
                self.sales.push(new Sale(_sale));
                self.product_fk('');
                self.persons(self.persons_max());
                self.sale_comments('');
                //self.remained_pay('');

                self.load_sales();

            }).done(function (result) {
                self.feedback_sale('');
            })
            .fail(function (error) {
                self.feedback_sale(error.responseText);
            });
        }

    };

    //self.edit_mode = function () {

    //    this.editable(!this.editable());
    //};


    self.edit_mode = function () {
        //If edite mode, we want to save it
        if (this.editable() == true) {

            self.UpdateSale(this);
            this.editBtnText('Edit');
            this.editBtnClass('btn btn-circle btn-info');
        }
        else {
            this.editable(true);
            this.editBtnText('Save');
            this.editBtnClass('btn btn-circle btn-success');
        }
    };


    self.UpdateSale = function (item) {

        var _url = url_sales + '/UpdateSale/' + item.ID();
        $.ajax({
            method: "PUT",
            url: _url,
            data: item,
            dataType: "json",
        }).done(function (result) {
            item.editable(false);
            self.feedback_sale('');
        }).fail(function (error) {
            self.feedback_sale(error.responseText)
        });
    }

    self.cancel = function () {

        if (confirm('Are you sure you want to delete this sale?')) {
            var item = this;
            var _url = url_sales + '/CancelSale/' + item.ID();
            $.ajax({
                method: "PUT",
                url: _url,
                //data: item,
                dataType: "json",
            }).done(function (result) {
                self.sales.remove(item);
                self.feedback_sale('');
            }).fail(function (error) {
                //alert(error.responseText);
                self.feedback_sale(error.responseText);
            })
        }
    }

    self.load_trans = function () {

        self.feedback_trans('');
        var _url = url_sales + "/getSales/" + '/' + self.agency_fk() + '/' + self.PNR() + "/transport";
        $.getJSON(_url, function (allData) {
            self.trans_ID(allData[0].ID);
            self.trans_product_fk(allData[0].product_fk.toString());
            self.trans_comments(allData[0].comments);
            self.trans_remained_pay(allData[0].remained_pay);
        });

    }




    self.update_transport = function () {

        if (isValidContainer('modal_trans')) {


            var _sale = {
                PNR: self.PNR(),
                agency_fk: self.agency_fk(),
                product_fk: self.trans_product_fk(),
                comments: self.trans_comments(),
                //remained_pay: self.trans_remained_pay(),
                //TODO: Comment in/out prices for next version
                remained_pay: 0
            };

            var _url = url_sales + '/UpdateTransport/' + self.trans_ID();
            $.ajax({
                method: "PUT",
                url: _url,
                data: _sale,
                dataType: "json",
            }).done(function (result) {
                $('#modal_trans').modal('hide');
            }).fail(function (error) {
                self.feedback_trans(error.responseText);
            });
        }
        else {
            popMessage('danger', 'Please fill out all required fields.')

        }
    }

    self.trans_product_fk.subscribe(function (_new) {

        var id = '#rb_' + _new;
        var rate = $(id).attr('data-rate');
        self.trans_remained_pay(rate);

    });

    var _last_pnr = '';
    self.load_client = function (client) {
        if (_last_pnr != client.PNR()) {
            //Clear previous messages
            clearMessages();

            self.PNR(client.PNR());
            self.names(client.names());
            self.PAX(client.PAX());
            self.phone(client.phone());
            self.oneway(client.oneway());
            self.comments(client.comments());
            self.num_arr(client.num_arr());
            self.date_arr(client.date_arr());
            self.num_dep(client.num_dep());

            if (client.date_dep() == '1970-01-01') {
                self.date_dep('');
            }
            else {
                self.date_dep(client.date_dep());
            }

            self.agency_fk(client.agency_fk());
            self.hotel_fk(client.hotel_fk());
            _last_pnr = client.PNR();
        }
        else {
            console.log('same PNR: ' + _last_pnr)
        }

    };

    self.PAX.subscribe(function (_new) {

        if (_last_pnr == self.PNR()) {
            popMessage('warning', 'Note that Changing the PAX will also set the same number of people at all the services (e.g. bus, tours etc.)');

        }

    });

    ////    //TODO: next version uncomment:
    //self.sale_type.subscribe(function (_new) {
    //    if (_new == "Internal") {
    //        //$('#collapseExample').show('slow');
    //        $('#divPayment').collapse('show');
    //    }
    //    else {
    //        //$('#divPayment').hide('slow');
    //        $('#divPayment').collapse('hide');
    //    }
    //});
    //////




    self.UpdateClient = function () {
        if (isValidContainer('modal_edit')) {
            var _client_update = {
                PNR: self.PNR(),
                agency_fk: self.agency_fk(),
                names: self.names(),
                PAX: self.PAX(),
                phone: self.phone(),
                oneway: self.oneway(),
                comments: self.comments(),
                date_arr: self.date_arr(),
                date_dep: self.date_dep(),
                num_arr: self.num_arr(),
                num_dep: self.num_dep(),
                num_arr: self.num_arr(),
                hotel_fk: self.hotel_fk(),
            };

            var _url = url_clients + '/UpdateClient/' + self.agency_fk() + '/' + self.PNR() + '';
            $.ajax({
                method: "PUT",
                url: _url,
                data: _client_update,
                dataType: "json",
            }).done(function (result) {
                popMessage('success', 'The reservation for : <b> ' + self.names() + "</b>" + ' was successfuly updated. Close the window to continue.');
                // The client: <b id="new_pnr" data-bind="text: names"></b>was successfuly updated.  
                self.get_clients();
            }).fail(function (error) {
                popMessage('danger', error.responseText);
            });
        }
        else {
            popMessage('danger', 'Please fill out all required fields.')

        }

    }



    self.cancel_client = function () {

        if (confirm('Are you sure you want to delete this reservation?')) {
            //var item = self;
            var _url = url_clients + '/CancelClient/' + self.agency_fk() + '/' + self.PNR();
            $.ajax({
                method: "PUT",
                url: _url,
                //data: item,
                dataType: "json",
            }).done(function (result) {
                popMessage('success', 'The reservation for: <b> ' + self.names() + "</b>" + ' was successfuly <span style="color:red; font-weight:bold">DELETED</span>. Close the window to continue.');
                self.get_clients();
                $('#btn_update').hide();
                $('#btn_delete').hide();

                //$('#modal_edit').modal('hide');
            }).fail(function (error) {
                popMessage('danger', error.responseText);
            })
        }
    }





    self.arrival_validation = function () {

        if (!self.date_arr()) {
            popMessage('info', 'Please choose <b>Arrival Date</b> first');
        }
        else if (self.flights_filter_arr().length == 0) {
            popMessage('warning', 'There are no arrival flights on the selected date. Please choose another date or <a target="_blank" href="../pages/flights.aspx">click here to add a new flight</a>');
        }

    };


    self.departure_validation = function () {

        if (!self.date_dep()) {
            popMessage('info', 'Please choose <b>Departure Date</b> first');
        }
        else if (self.flights_filter_dep().length == 0) {
            popMessage('warning', 'There are no departure flights on the selected date. Please choose another date or <a target="_blank" href="../pages/flights.aspx">click here to add a new flight</a>');
        }

    };

    function clearMessages() {
        //var old_messages = $("div#system_feedback").children('div [name=clone]');
        var old_messages = $("div#system_feedback").children('div');
        $.each(old_messages, function (index, value) {
            if ($(this).attr('id') == '') {
                $(this).remove();
            }
        });
    };

    function popMessage(severity, html) {
        var div; var clone;
        switch (severity) {
            case 'success':
                div = $("#system_feedback").children("div#message_success");
                break;
            case 'info':
                div = $("#system_feedback").children("div#message_info");
                break;
            case 'warning':
                div = $("#system_feedback").children("div#message_warning");
                break;
            case 'danger':
                div = $("#system_feedback").children("div#message_danger");
                break;
            default:
                break;
        }

        var old_messages = $("div#system_feedback").children('div');

        $.each(old_messages, function (index, value) {
            if ($(this).attr('id') == '') {
                $(this).css('opacity', '0.3');
                // $(this).attr('name', 'clone');


            }

        });

        var clone = $(div).clone().attr('id', '').show('fast');
        if (html != undefined) {
            $(clone).append(html);
        }
        $("#system_feedback").prepend(clone);

    }


}



