function Product(data) {
    this.ID = ko.observable(data.ID);
    this.name = ko.observable(data.name);
    this.code = ko.observable(data.code);
    this.type = ko.observable(data.type);
    this.rate = ko.observable(data.rate);
    this.capacity = ko.observable(data.capacity);

}

function Flight(data) {
    this.num = ko.observable(data.num);
    this.date = ko.observable(Date(data.date).yyyymmdd());
    this.time = ko.observable(data.time);
    this.direction = ko.observable(data.direction);
    this.destination = ko.observable(data.destination);
    this.time_approved = ko.observable(data.time_approved);
    this.date_update = ko.observable(data.date_update);

    this.details = ko.dependentObservable(function () {
        return (this.date() + ' - ' + this.num() + ' - ' + this.time())
    }, this);
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

function TourPlan(data) {

    this.ID = ko.observable(data.ID);
    var d = new Date(data.date);
    this.date = ko.observable(d.yyyymmdd());
    this.date = Date(data.date).yyyymmdd();
    this.time = ko.observable(data.time);
    this.product_fk = ko.observable(data.product_fk);
    this.guide_fk = ko.observable(data.guide_fk);
    this.comments = ko.observable(data.comments);
    var d_u = new Date(data.date_update);
    this.date_update = ko.observable(d_u.yyyymmdd());
}

function Guide(data) {
    //this.ID = ko.observable(data.ID);
    //this.name = ko.observable(data.name);
}