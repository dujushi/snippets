var React = require('react');
var Place = React.createClass({
  render: function() {
    var distanceMatrixNode = '';
    var isCalculating = this.props.distance == 'Calculating';
    if (this.props.distance != "") {
      distanceMatrixNode = (
        <div className='DistanceMatrix'>
          <span>{!isCalculating ? this.props.distance : <i className='fa fa-spinner fa-spin'></i>}</span>
          <i className='fa fa-long-arrow-down ToNextStop'></i>
          <span>{!isCalculating ? this.props.duration : <i className='fa fa-spinner fa-spin'></i>}</span>
        </div>
      );
    }
    return (
      <div className='Place'>
        <h2 className='Stop'>
          <i className='fa fa-car'></i>
          <span>{this.props.description}</span>
        </h2>
        {distanceMatrixNode}
      </div>
    );
  }
});

module.exports = Place;
