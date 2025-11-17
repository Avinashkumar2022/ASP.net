using System;
using GameStore.Api.Dtos;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
    const string GetGameEndpointName= "GetGame";

    private static readonly List<GameDto> games=
    [
        new(1, "Street Fighter II", "Fighting", 19.99M, new DateOnly(1992, 7, 15)),
        new(2, "Mortal Kombat", "Fighting", 14.99M, new DateOnly(1992, 10, 8)),
        new(3, "The Legend of Zelda: A Link to the Past", "Action-Adventure", 29.99M, new DateOnly(1991, 11, 21)),
        new(4, "Super Mario Bros.", "Platformer", 9.99M, new DateOnly(1985, 9, 13)),
        new(5, "DOOM", "First-Person Shooter", 24.99M, new DateOnly(1993, 12, 10)),
        new(6, "Half-Life", "First-Person Shooter", 39.99M, new DateOnly(1998, 11, 19))
    ];

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {

        var group= app.MapGroup("games").WithParameterValidation();
        
        //GET /games
        group.MapGet("/", ()=>games);

        //GET /games/1
        group.MapGet("/{id}", (int id)=> 
        {
            GameDto? game=games.Find(game=>game.Id==id);

            return game is null ? Results.NotFound() : Results.Ok(game);
        })
        .WithName(GetGameEndpointName);

        //POST /games

        group.MapPost("/",(CreateGameDto newGame)=> {

            // if(string.IsNullOrEmpty(newGame.Name))
            // {
            //     return Results.BadRequest("Name is Required");
            // }


            GameDto game=new(
                games.Count + 1,
                newGame.Name,
                newGame.Genre,
                newGame.Price,
                newGame.ReleaseDate);

            games.Add(game);

            return Results.CreatedAtRoute(GetGameEndpointName, new { id= game.Id}, game);
        }).WithParameterValidation();


        //PUT /games

        group.MapPut("/{id}", (int id, UpdateGameDto updatedGame)=>
        {
            var index=games.FindIndex(game=>game.Id==id);

            if(index == -1)
            {
                return Results.NotFound();
            }

            games[index]=new GameDto(
                id, 
                updatedGame.Name,
                updatedGame.Genre,
                updatedGame.Price,
                updatedGame.ReleaseDate
            );

            return Results.NoContent();
        });

        //DELETE /games/id

        group.MapDelete("/{id}", (int id)=> 
        {
            games.RemoveAll(game=>game.Id==id);

            return Results.NoContent();
        });

        return group;
    }
}
