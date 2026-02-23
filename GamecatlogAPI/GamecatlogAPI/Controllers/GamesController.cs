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
            var game = await gameRepository.GetByIdAsync(id);
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
            var gameDomainModel = new Game
            {
                Id = Guid.NewGuid(),
                Name = addgameDto.Name,
                Description = addgameDto.Description,
                Price = addgameDto.Price,
                Genre = addgameDto.Genre,
                GameImageURL = addgameDto.GameImageURL
            };

            // Save to Database
            gameDomainModel = await gameRepository.CreateAsync(gameDomainModel);

            //sending the response back aswell to avoid the client having to make another GET request to fetch the created game details
            var gameDto = new GameDto(
                gameDomainModel.Id,
                gameDomainModel.Name,
                gameDomainModel.Description,
                gameDomainModel.Price,
                gameDomainModel.Genre,
                gameDomainModel.GameImageURL
            );


            return CreatedAtAction(nameof(GetById), new { id = gameDto.Id }, gameDto);
        }

        //update existing game
        [HttpPut]
        [Route("id:guid")]
        public async Task <IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateGameRequestDto updategamerequestdto)
        {
            //Map DTO to domain Model
            var gameDomainModel = new Game
            {
                Name = updategamerequestdto.Name,
                Description = updategamerequestdto.Description,
                Price = updategamerequestdto.Price,
                Genre = updategamerequestdto.Genre,
                GameImageURL = updategamerequestdto.GameImageURL
            };
            gameDomainModel = await gameRepository.UpdateAsync(id, gameDomainModel);
            if (gameDomainModel == null)
            {
                return NotFound();
            }
          
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
            var gameDomainModel = await gameRepository.DeleteAsync(id);
            if (gameDomainModel == null)
            {
                return NotFound();
            }
            // return deleted game back
            // map domain model to DTO
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

    }
}
