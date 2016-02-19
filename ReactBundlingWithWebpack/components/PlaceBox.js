var React = require('react');
var PlaceList = require('./PlaceList');
var PlaceForm = require('./PlaceForm');

var PlaceBox = React.createClass({
  getInitialState: function() {
    return {places: []};
  },
  render: function() {
    return (
      <div className='PlaceBox'>
        <h1>Travel Plan</h1>
        <PlaceList places={this.state.places}/>
        <PlaceForm onPlaceSelect={this.handlePlaceSelect}/>
      </div>
    );
  },
  handlePlaceSelect: function(place) {
    var places = this.state.places;
    // check if place_id already exists
    var found = places.find(function(element, index, array) {
      return element.place_id == place.place_id;
    });
    if (found === undefined) {
      // calculate distance and duration from last stop
      if (places.length > 0) {
        var lastPlace = places[places.length - 1];
        lastPlace.distance = 'Calculating';
        lastPlace.duration = 'Calculating';
        places[places.length - 1] = lastPlace;

        this.calculateDistanceMatrix(lastPlace, place);
      }
      places.push(place);
      this.setState({places: places});
    }
  },
  calculateDistanceMatrix: function(originPlace, destinationPlace) {
    var service = new google.maps.DistanceMatrixService;
    service.getDistanceMatrix({
      origins: [originPlace.description],
      destinations: [destinationPlace.description],
      travelMode: google.maps.TravelMode.DRIVING,
      unitSystem: google.maps.UnitSystem.METRIC,
      avoidHighways: false,
      avoidTolls: false
    }, function(response, status) {
      if (status !== google.maps.DistanceMatrixStatus.OK) {
        alert('Error! DistanceMatrixStatus: ' + status);
      } else {
        var results = response.rows[0].elements;
        var status = results[0].status;
        if (status == google.maps.DistanceMatrixElementStatus.OK) {
          originPlace.distance = results[0].distance.text;
          originPlace.duration = results[0].duration.text;
        } else {
          originPlace.distance = status;
          originPlace.duration = status;
        }
        var places = this.state.places;
        // find the right place to update distance and duration
        var found = places.find(function(element, index, array) {
          return element.place_id == originPlace.place_id;
        });
        if (found != undefined) {
          places[found] = originPlace;
          this.setState({places: places});
        }
      }
    }.bind(this));
  }
});

module.exports = PlaceBox;
