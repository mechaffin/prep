import React, { Component } from 'react';
import { ReactComponent as Bone } from './../assets/icons/bone.svg';

export class Scoreboard extends Component {
  render() {
    console.log(this.props);
    const { score } = this.props;
    const scoreDisplay = [];
    for (let i = 0; i < score; i++) {
      scoreDisplay.push(<Bone key={`bone-${i}`} className="bone" />);
    }

    return (
      <div className="scoreboard-container">
        {scoreDisplay}
      </div>
    );
  }
}
