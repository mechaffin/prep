import React, { Component } from 'react';
import { ReactComponent as Dog } from './assets/icons/dog.svg';
import { Scoreboard, Winner } from './components';
import './App.scss';

const winnerClickCount = 10;

const initialState = {
  clickCount: 0
};

export default class App extends Component {
  constructor(props) {
    super(props);

    this.state = initialState;
    this.bodyDiv = React.createRef();
  }

  onSvgClick = () => {
    const { clickCount } = this.state;
    this.changeAnimation();
    this.setState({ clickCount: clickCount + 1 });
  }

  hasWinner = () => this.state.clickCount === winnerClickCount;

  getRandomInt = (max) => Math.floor(Math.random() * Math.floor(max));

  getRandomIntInRange = (min, max) => {
    min = Math.ceil(min);
    max = Math.floor(max);
    return Math.floor(Math.random() * (max - min + 1)) + min;
  }

  changeAnimation = () => {
    //There are tons of ways to do the animation and change it up.
    //In this excercise, I elected to learn something new (to me) and use CSS variables.

    const buffer = 50;
    const svgWidth = 64;
    const svgHeight = 80;

    const maxX = (this.bodyDiv.current.clientWidth - buffer) / svgWidth * 100;
    const maxY = (this.bodyDiv.current.clientHeight - buffer) / svgHeight * 100;

    this.updateCSSVariables({
      '--move-time': this.getRandomIntInRange(3, 10) + 's',
      '--move-pct-x-1': this.getRandomInt(maxX) + '%',
      '--move-pct-x-2': this.getRandomInt(maxX) + '%',
      '--move-pct-x-3': this.getRandomInt(maxX) + '%',
      '--move-pct-y-1': this.getRandomInt(maxY) + '%',
      '--move-pct-y-2': this.getRandomInt(maxY) + '%',
      '--move-pct-y-3': this.getRandomInt(maxY) + '%',
      '--spin-degrees-1': this.getRandomInt(359) + 'deg',
      '--spin-degrees-2': '-' + this.getRandomInt(359) + 'deg',
      '--spin-degrees-3': this.getRandomInt(359) + 'deg',
    });
  }

  updateCSSVariables = (variables) => {
    Object.keys(variables).forEach((propertyName) => {
      document.documentElement.style.setProperty(propertyName, variables[propertyName]);
    });
  }

  render() {
    const { clickCount } = this.state;

    return (
      <div className="app-container">
        <div className="header">
          <div className="title">Juno's Bone Stash</div>
          <Scoreboard score={clickCount} />
          <div className="help">
            Juno loves chewing on bones so much that hoards them from her housemates Echo and Zoe.
            Each time you click Juno's image she'll get a new bone for her stash.
          </div>
        </div>
        <div ref={this.bodyDiv} className="body">
          {
            this.hasWinner()
              ? <Winner />
              : <Dog className="dog" onClick={this.onSvgClick} />
          }
        </div>
      </div>
    );
  }
}
