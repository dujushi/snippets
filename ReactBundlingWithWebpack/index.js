require('./style.css');
var React = require('react');
var ReactDOM = require('react-dom');
var PlaceBox = require('./components/PlaceBox');

ReactDOM.render(
  <PlaceBox />,
  document.getElementById('content')
);
