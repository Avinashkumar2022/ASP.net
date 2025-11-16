using GameStore.Api.Dtos;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

const string GetGameEndpointName= "GetGame";

List<GameDto> games=
[
    new(1, "Street Fighter II", "Fighting", 19.99M, new DateOnly(1992, 7, 15)),
    new(2, "Mortal Kombat", "Fighting", 14.99M, new DateOnly(1992, 10, 8)),
    new(3, "The Legend of Zelda: A Link to the Past", "Action-Adventure", 29.99M, new DateOnly(1991, 11, 21)),
    new(4, "Super Mario Bros.", "Platformer", 9.99M, new DateOnly(1985, 9, 13)),
    new(5, "DOOM", "First-Person Shooter", 24.99M, new DateOnly(1993, 12, 10)),
    new(6, "Half-Life", "First-Person Shooter", 39.99M, new DateOnly(1998, 11, 19))
];

//GET /games
app.MapGet("games", ()=>games);

//GET /games/1
app.MapGet("games/{id}", (int id)=> games.Find(game=>game.Id==id))
    .WithName(GetGameEndpointName);

//POST /games
app.MapPost("games",(CreateGameDto newGame)=> {
    GameDto game=new(
        games.Count + 1,
        newGame.Name,
        newGame.Genre,
        newGame.Price,
        newGame.ReleaseDate);

    games.Add(game);

    return Results.CreatedAtRoute(GetGameEndpointName, new { id= game.Id}, game);
});


//PUT /games

app.MapPut("games/{id}", (int id, UpdateGameDto updatedGame)=>
{
    var index=games.FindIndex(game=>game.Id==id);

    games[index]=new GameDto(
        id, 
        updatedGame.Name,
        updatedGame.Genre,
        updatedGame.Price,
        updatedGame.ReleaseDate
    );

    return Results.NoContent();
});

app.Run();
