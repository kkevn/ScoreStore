# ScoreStore
 ASP.NET web application using MVC to track and visualize wins/losses for any kind of game.
 
This project is my way of refamiliarizing myself with ASP.NET after using it only a few times in university. ScoreStore aims to allow a user to input any game(s) at all of their own choosing into a global database. These games are intended to be anything that is played against a few other people such as a video game, card game, or even a sports match.

Once a game is added, the user will then need to manually update their game history as each match occurs. For example, if a user just played a game of basketball against a friend or two, the user will simply add a win or loss to their account for the basketball game they've alredy added. Then, ScoreStore will automatically update the user's win/loss ratio and other useful calcuations to the user in the form of graphs and charts so that he/she better understands their performance.

---

### Demonstration

<a href="http://www.youtube.com/watch?feature=player_embedded&v=HoI-ORoXd3Q
" target="_blank"><img src="http://img.youtube.com/vi/HoI-ORoXd3Q/0.jpg" 
alt="ScoreStore Demo" width="640" height="360" border="10" /></a>

Want to try it out? Register an account and see the live demo hosted on ~~Heroku~~ Railway: [https://scorestore.up.railway.app/](https://scorestore.up.railway.app/)

#### :exclamation: Live Demo Update :exclamation:

*As of 08/25/2022, [Heroku announced an end to all of its free hosting services](https://blog.heroku.com/next-chapter) that ScoreStore was originally using. As a result, I have migrated the live environment to Railway. Please note, Railway's free service has a monthly limit of 500 hours, and so the live environment may be unavailable at times.*

---

### Features

* Log in via a local account or social login
* Add any games you'd like to the global database
* Update your account's game wins/losses as they occur
* Graphically view your stats such as:
  * win/loss ratio
  * play count per game
  * total wins per game
  * complete win/loss history
  * win/loss streaks
* Add other users as friends
  * view their overall performance across their games
  * compare your stats against theirs for games you share

*See the [issue tracker](https://github.com/kkevn/ScoreStore/issues) for a list of planned (or completed) features and their status.*

---

### Specifications

* **C#** for program and controller logic
* **HTML/CSS** for view layouts
* **[Bootstrap 4](https://getbootstrap.com/docs/4.0/getting-started/introduction/)** for additional CSS support
* **JavaScript** for webpage logic
* **ASP.NET** for applicaiton development and MVC design
* **[Google Charts](https://developers.google.com/chart)** for rendering charts
* **[Google Sign-In](https://developers.google.com/identity/sign-in/web/sign-in)** for social login
* **[SendGrid](https://sendgrid.com/)** for mail server

#### Live Environment
* ~~**[Heroku](https://www.heroku.com/)** for free hobby-tier app and database hosting~~
* **[Railway](https://railway.app/)** for free starter-tier app and database hosting
* **PostgreSQL** integration for database (default MS SQL not supported by Heroku or Railway)
* ~~**[jincod/dotnetcore-buildpack](https://github.com/jincod/dotnetcore-buildpack/releases/tag/v3.1.11)** for dotnet deployment in Heroku~~
* **[Docker](https://www.docker.com/)** dockerfile for dotnet deployment in Railway

---

### License

```
MIT License

Copyright (c) 2021 Kevin Kowalski

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
```
