import React, { Component } from 'react';
import juno from './../assets/images/juno.jpg';

export class Winner extends Component {
  render() {
    return (
      <div className="winner-container">
        <img className="hero" src={juno} alt="Juno" />
        <div className="message">
          Juno baroos that you are a WINNER-RROOO-ROO-OOOOOO! Thanks for giving her lots of bones.
        </div>
      </div>
    );
  }
}
