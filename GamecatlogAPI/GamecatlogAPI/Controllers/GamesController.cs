using GamecatalogAPI.Data;
using Microsoft.AspNetCore.Mvc;
using GamecatalogAPI.Models.DTO;
using GamecatalogAPI.Models.Domain;
using System.Reflection.Metadata.Ecma335;

namespace GamecatalogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        public readonly GamesDBContext DbContext;
        public GamesController(GamesDBContext dbContext)
        {
            this.DbContext = dbContext;
        }

        //Get all games
        [HttpGet]
        public IActionResult GetAll()
        {
            //Get data from Database - domain models
            var games = DbContext.Game.ToList();
            // map domain models to DTOs
            //return DTOs
            var gameDtos = games.Select(game => new GameDto
            (
                game.Id,
                game.Name,
                game.Description,
                game.Price,
                game.Genre,
                game.GameImageURL
            )).ToList();
            return Ok(gameDtos);
        }
        //get single Game by Id
        [HttpGet]
        [Route("id:Guid")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            //var game = DbContext.Game.Find(id);
            var game = DbContext.Game.FirstOrDefault(x => x.Id == id);
            if (game == null)
            {
                return NotFound();
            }

            var gameDto = new GameDto
            (
                game.Id,
                game.Name,
                game.Description,
                game.Price,
                game.Genre,
                game.GameImageURL
            );
            return Ok(gameDto);

        }
        // Post To create new game
        [HttpPost]
        public IActionResult Create([FromBody] AddGameRequestDto addgameDto)
        {
            // 1. Map DTO to Domain Model (Class)
            var game = new Game
            {
                Id = Guid.NewGuid(),
                Name = addgameDto.Name,
                Description = addgameDto.Description,
                Price = addgameDto.Price,
                Genre = addgameDto.Genre,
                GameImageURL = addgameDto.GameImageURL
            };

            // Save to Database
            DbContext.Game.Add(game);
            DbContext.SaveChanges();

            //sending the response back aswell to avoid the client having to make another GET request to fetch the created game details
            var gameDto = new GameDto(
                game.Id,
                game.Name,
                game.Description,
                game.Price,
                game.Genre,
                game.GameImageURL
            );


            return CreatedAtAction(nameof(GetById), new { id = gameDto.Id }, gameDto);
        }

        //update existing game
        [HttpPut]
        [Route("id:guid")]
        public IActionResult Update([FromRoute] Guid id, [FromBody] UpdateGameRequestDto updategamerequestdto)
        {
            var gameDomainModel = DbContext.Game.FirstOrDefault(x => x.Id == id);
            if (gameDomainModel == null)
            {
                return NotFound();
            }
            //update the properties of the existing game with the new values from the DTO
            gameDomainModel.Name = updategamerequestdto.Name;
            gameDomainModel.Description = updategamerequestdto.Description;
            gameDomainModel.Price = updategamerequestdto.Price;
            gameDomainModel.Genre = updategamerequestdto.Genre;
            gameDomainModel.GameImageURL = updategamerequestdto.GameImageURL;
            //save changes to database
            DbContext.SaveChanges();
            // convert the domain model to dto
            var gameDto = new GameDto
                (
                    gameDomainModel.Id,
                    gameDomainModel.Name,
                    gameDomainModel.Description,
                    gameDomainModel.Price,
                    gameDomainModel.Genre,
                    gameDomainModel.GameImageURL
                );

            return Ok(gameDto);
        }
        // delete existing game
        [HttpDelete]
        [Route("id:guid")]
        public IActionResult Delete([FromRoute] Guid id)
        {
            var gameDomainModel = DbContext.Game.FirstOrDefault(x => x.Id == id);
            if (gameDomainModel == null)
            {
                return NotFound();
            }
            //remove the game from database
            DbContext.Game.Remove(gameDomainModel);
            DbContext.SaveChanges();
            // return deleted game back
            // map domain model to DTO
            var regionDto = new GameDto
                (
                    gameDomainModel.Id,
                    gameDomainModel.Name,
                    gameDomainModel.Description,
                    gameDomainModel.Price,
                    gameDomainModel.Genre,
                    gameDomainModel.GameImageURL
                );
            return Ok();
        }

    }
}
