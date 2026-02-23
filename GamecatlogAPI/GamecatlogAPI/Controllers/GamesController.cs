using Microsoft.AspNetCore.Mvc;
using GamecatalogAPI.Models.DTO;
using GamecatalogAPI.Models.Domain;
using GamecatalogAPI.Repositores;
using AutoMapper;   

namespace GamecatalogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IGamerepository gameRepository;
        private readonly IMapper mapper;
        public GamesController( IGamerepository gameRepository, IMapper mapper)
        {

            this.gameRepository = gameRepository;
            this.mapper = mapper;
        }

        //Get all games
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            //Get data from Database - domain models
            var gamesDomain = await gameRepository.GetAllAsync();
            return Ok(mapper.Map<List<GameDto>>(gamesDomain));
        }
        //get single Game by Id
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            //var game = DbContext.Game.Find(id);
            var game = await gameRepository.GetByIdAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<GameDto>(game));

        }
        // Post To create new game
        [HttpPost]
        public async Task <IActionResult> Create([FromBody] AddGameRequestDto addgameRequestDto)
        {
            // 1. Map DTO to Domain Model (Class)
            var gameDomainModel = mapper.Map<Game>(addgameRequestDto);
            gameDomainModel.Id = Guid.NewGuid();

            // Save to Database
            gameDomainModel = await gameRepository.CreateAsync(gameDomainModel);

            //sending the response back aswell to avoid the client having to make another GET request to fetch the created game details
            var gameDto = mapper.Map<GameDto>(gameDomainModel);


            return CreatedAtAction(nameof(GetById), new { id = gameDto.Id }, gameDto);
        }

        //update existing game
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task <IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateGameRequestDto updategamerequestdto)
        {
            //Map DTO to domain Model
            var gameDomainModel = mapper.Map<Game>(updategamerequestdto);

            gameDomainModel = await gameRepository.UpdateAsync(id, gameDomainModel);
            if (gameDomainModel == null)
            {
                return NotFound();
            }

            // convert the domain model to dto
            var gameDto = mapper.Map<GameDto>(gameDomainModel);

            return Ok(gameDto);
        }
        // delete existing game
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task <IActionResult> Delete([FromRoute] Guid id)
        {
            var gameDomainModel = await gameRepository.DeleteAsync(id);
            if (gameDomainModel == null)
            {
                return NotFound();
            }
            // return deleted game back
            // map domain model to DTO
            var gameDto = mapper.Map<GameDto>(gameDomainModel);
            return Ok(gameDto);
        }

    }
}
