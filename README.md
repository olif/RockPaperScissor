# RockPaperScissors
A poco RockPaperScissors api implementation written in C# (written in Spacemacs).

## Problem description
Create a simple REST api that let's programmers solve their disputes by means
of the classical game Rock Paper and Scissors. The rules are simple, the winner
of 1 match takes all the glory.

* There is no requirement for a re-match mechanism.
* A draw result counts as a result. This means that you do not need to 
restart the game, or let the players choose another move in case of a draw.
* No persistance mechanism is required (e.g a database).
* The application should allow multiple games to be played in parallel.
* No client implementation is required but the application should be thoroughly 
tested.

### Example of game flow
1. Player 1 makes a request to the api, creating a new game
2. Player 1 sends the game Id to Player 2 via mail or slack.
3. Player 2 joins the game with help of the game Id.
4. Player 1 makes a move (Rock).
5. Player 2 makes a move (Scissor).
6. Player 1 polls the status of the game and discovers that he/she won.
7. Player 2 polls the status of the game and discovers that he/she lost.

## How to run
Run the application with

    $> dotnet run


## Endpoints
The application exposes a set of endpoints:

GET /api/games/{id}?playerName={string}
POST /api/games/new
POST /api/games/{id}/join
POST /api/games/{id}move

where id is a global unique identifier (GUID). All endpoints returns the 
current games state:

```
{
   Id: "79aa1a95-f453-44f2-ba92-52e9744bc51b"
   CurrentPlayer: "Niklas"
   Winner: "Niklas"
   Opponent: "Arne"
   GameFinished: true,
   CurrentPlayerMove: "Rock",
   OpponentPlayerMove: "Scissor"
}
```

### GET /api/games/{id}?playerName={string}_
Returns the current game state given a player name active in the game.

### POST /api/games/new
Creates a new game given the initial player.

Request body example:
```
{
    Name: "Niklas"
}

```

### POST /api/games/{id}/join
Joins an existing game given a game id.

Request body example:
```
{
    Name: "Arne"
}
```

### POST api/games/{id}/move
Makes a move.

Request body example:
```
{
    Name: "Niklas",
    Move: "Rock"
}
```




