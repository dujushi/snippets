var React = require('react');
var Place = require('./Place');

var PlaceList = React.createClass({
  render: function() {
    var placeNodes = this.props.places.map(function(place) {
      return (
        <Place key={place.place_id} {...place} />
      );
    });
    return (
      <div className='PlaceList'>
        {placeNodes}
      </div>
    );
  }
});

module.exports = PlaceList;
