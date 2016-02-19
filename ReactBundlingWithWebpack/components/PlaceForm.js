var React = require('react');
var PlaceForm = React.createClass({
  defaultState: {keywords: '', predictions: [], error: ''},
  getInitialState: function() {
    return this.defaultState;
  },
  render: function() {
    var predictionNodes = this.state.predictions.map(function(prediction) {
      return (
        <li key={prediction.place_id}
            onClick={this.handlePlaceSelect.bind(this, prediction.place_id, prediction.description)}
            >
          {prediction.description}
        </li>
      );
    }.bind(this));
    return (
      <div>
        <input value={this.state.keywords} onChange={this.handleKeywordsChange} placeholder='Enter your next stop' />
        {this.state.error != "" ? <div className='Error'>{this.state.error}</div> : null}
        {this.state.predictions.length == 0 ? null : <ul>{predictionNodes}</ul>}
      </div>
    );
  },
  handleKeywordsChange: function(e) {
    var keywords = e.target.value;
    this.setState({keywords: keywords});

    // no query if keywords has fewer than 5 characters
    if (keywords.length < 5) {
      this.setState({predictions: []});
    } else {
      var service = new google.maps.places.AutocompleteService();
      service.getQueryPredictions({input: keywords}, function(predictions, status) {
        // result may come back in different order.
        // only show results for current keywords.
        if (keywords != this.state.keywords) return;

        switch (status) {
          case google.maps.places.PlacesServiceStatus.OK:
            this.setState({predictions: predictions});
            break;
          case google.maps.places.PlacesServiceStatus.ZERO_RESULTS:
            this.setState({predictions: []});
            break;
          default:
            this.setState({error: status, predictions: []});
            break;
        }
      }.bind(this));
    }
  },
  handlePlaceSelect: function(place_id, description, e) {
    this.setState(this.defaultState);
    this.props.onPlaceSelect({place_id: place_id, description: description, duration: '', distance: ''});
  }
});

module.exports = PlaceForm;
