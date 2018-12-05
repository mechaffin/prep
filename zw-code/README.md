# zw-code-exercise
ZW Code Exercise - React Game

## Setup
* Run `npm install`
* Run `npm start`
* Next navigate to `http://localhost:8080`

## Objective

You will be making a game in React. Feel free to be creative. We have provided an SVG example and an icon, but you may use whatever you like. Click on a target to get a point. After 10 successful clicks, the player wins.

### Requirements
- [ ] Fork this repo and submit a PR when done
    -- By permission, this project was bootstrapped with [Create React App](https://github.com/facebook/create-react-app).
- [x] No external libs
    -- Only libraries/packages added by create-react-app are included.
- [ ] Put code in `/src/spas/home`
    -- By permission, bootstrapped with create-react-app so the code is all under `/src`
- [x] Must use SVG asset for click target
- [x] You may use `scss` or `css`
- [x] Must be responsive and mobile friendly
- [x] Shoud work in all modern web browsers
- [x] Must have a winner UI

### Things I'd like to do differently
- [ ] State management (didn't have time to look more into React Context API, & it was against the rules to add MobX or Redux).
- [ ] Better factoring so the game logic isn't in App (easier with decent state management).
- [ ] Better factoring of utility type code (getRandomInt, updateCssVariables, etc)
- [ ] Better user experience ("new game", change animation trigger, etc)
- [ ] Probably pick a different animation scheme.
    - This way makes it jump to a different spot when the animation changes. Though, I decided to leave it this way for now because when you click, Juno "runs away" to find another bone. Haha!
    - Also, CSS variables wasn't something I had worked with before so it was good to get an understanding of them.
