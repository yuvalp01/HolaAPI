
//var _url = api_url + '/api/hotels';


function Hotel(data) {
    this.ID = ko.observable(data.ID);
    this.name = ko.observable(data.name);
    this.address = ko.observable(data.address);
}
function AppViewModel(data) {
    var self = this;

    self.hotels = ko.observableArray([]);
    self.new_ID = ko.observable();
    self.new_name = ko.observable();
    self.new_address = ko.observable();


    $.getJSON(url_hotels, function (allData) {
        var mappedData = $.map(allData, function (item) {
            return new Hotel(item);
        });
        self.hotels(mappedData);
    });

    //// Operations
    self.add_server = function () {
        if (isValid()) {
            var _hotel = { name: self.new_name(), address: self.new_address() };
            $.post(url_hotels, _hotel, function (hotel_from_server) {
                _hotel.ID = hotel_from_server.ID;
                self.hotels.unshift(new Hotel(_hotel));
                self.new_name('');
                self.new_address('');
            });
        }

    };

    self.remove_server = function (hotel) {

        $.ajax({
            url: url_hotels + '/' + hotel.ID(),
            type: 'DELETE',
        }).done(function () {
            self.hotels.remove(hotel)
        }).fail(function (error) {
            alert("error");
        });
    }
}

// Activates knockout.js
ko.applyBindings(new AppViewModel());






