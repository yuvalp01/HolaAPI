

function Product(data) {
    this.ID = ko.observable(data.ID);
    this.name = ko.observable(data.name);
    this.code = ko.observable(data.code);
    this.type = ko.observable(data.type);
    this.rate = ko.observable(data.rate);
    this.capacity = ko.observable(data.capacity);

}




function AppViewModel(data) {
    var self = this;

    self.products = ko.observableArray([]);

    self.new_ID = ko.observable();
    self.new_name = ko.observable();
    self.new_code = ko.observable();
    self.new_type = ko.observable();
    self.new_rate = ko.observable();
    self.new_capacity = ko.observable();

    self.type_options= ["Transport", "Tour", "Other"];


    $.getJSON(url_products, function (allData) {
        var mappedData = $.map(allData, function (item) {
            return new Product(item);
        });
        self.products(mappedData);
    });

    //// Operations
    self.add_server = function () {

        if (isValid()) {

            var _product = {
                name: self.new_name(),
                code: self.new_code(),
                type: self.new_type(),
                rate: self.new_rate(),
                capacity: self.new_capacity()


            };
            $.post(url_products, _product, function (product_from_server) {
                _product.ID = product_from_server.ID;
                self.products.unshift(new Product(_product));
                self.new_name('');
                self.new_code('');
                self.new_type('');
                self.new_rate('');
                self.new_capacity('')

            });
        }
    };

}

// Activates knockout.js
ko.applyBindings(new AppViewModel());






