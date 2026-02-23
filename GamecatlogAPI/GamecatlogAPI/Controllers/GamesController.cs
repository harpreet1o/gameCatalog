using GamecatalogAPI.Data;
using Microsoft.AspNetCore.Mvc;
using GamecatalogAPI.Models.DTO;
using GamecatalogAPI.Models.Domain;
using Microsoft.EntityFrameworkCore;
using GamecatalogAPI.Repositores;

namespace GamecatalogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly GamesDBContext DbContext;
        private readonly IGamerepository gameRepository;
        public GamesController(GamesDBContext dbContext, IGamerepository gameRepository)
        {
            this.DbContext = dbContext;
            this.gameRepository = gameRepository;
        }

        //Get all games
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            //Get data from Database - domain models
            var games = await gameRepository.GetAllAsync();
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
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            //var game = DbContext.Game.Find(id);
            var game = await DbContext.Game.FirstOrDefaultAsync(x => x.Id == id);
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
        public async Task <IActionResult> Create([FromBody] AddGameRequestDto addgameDto)
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
            await DbContext.Game.AddAsync(game);
            await DbContext.SaveChangesAsync();

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
        public async Task <IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateGameRequestDto updategamerequestdto)
        {
            var gameDomainModel =await DbContext.Game.FirstOrDefaultAsync(x => x.Id == id);
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
            await DbContext.SaveChangesAsync();
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
        public async Task <IActionResult> Delete([FromRoute] Guid id)
        {
            var gameDomainModel = await DbContext.Game.FirstOrDefaultAsync(x => x.Id == id);
            if (gameDomainModel == null)
            {
                return NotFound();
            }
            //remove the game from database and remove doesn't have an async method 
            DbContext.Game.Remove(gameDomainModel);
            await DbContext.SaveChangesAsync();
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
