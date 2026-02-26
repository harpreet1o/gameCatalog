using Moq;
using Microsoft.AspNetCore.Mvc;
using GamecatalogAPI.Controllers;
using GamecatalogAPI.Repositores;
using GamecatalogAPI.Models.Domain;
using GamecatalogAPI.Models.DTO;
using AutoMapper;

namespace GamecatalogAPI.Tests
{
    public class GamesControllerTests
    {
        // 1. Declare these at the class level
        private readonly Mock<IGamerepository> mockRepo;
        private readonly Mock<IMapper> mockMapper;
        private readonly GamesController controller;

        public GamesControllerTests()
        {
            // repeat this setup for each test, or do it once in the constructor if it's the same for all tests
            mockRepo = new Mock<IGamerepository>();
            mockMapper = new Mock<IMapper>();

            // Pass the .Object of the mocks into the controller
            controller = new GamesController(mockRepo.Object, mockMapper.Object);
        }
            [Fact]
            public async Task Create_IdealApproach()
            //Just one for the create cause testing the logic here it is working and no if statements to test different branches of logic, thinking of testing for missing details but it would be in grey zone due to the validation happening in the dto another place
            {
                // 1. ARRANGE
  
                var requestDto = new AddGameRequestDto("Halo", "Sci-fi", 50.00m, "FPS", null);
            // so when we map it with the game we create the domain model though guid is created seperated but fine for now
            var domainGame = new Game{ Id = Guid.NewGuid(),Name = "Halo", Description = "A great sci-fi shooter", Genre = "FPS",Price = 50.00m};
          
            // final expected response is like this    
            var responseDto = new GameDto(Guid.NewGuid(), "Halo", "Sci-fi", 50, "FPS", null);

            mockMapper.Setup(m => m.Map<Game>(requestDto)).Returns(domainGame);
            mockRepo.Setup(r => r.CreateAsync(It.IsAny<Game>())).ReturnsAsync((Game g) => g);

            // Setup the "Output" mapping (The part you were missing!)
            mockMapper.Setup(m => m.Map<GameDto>(It.IsAny<Game>())).Returns(responseDto);

            // 2. ACT
            var result = await controller.Create(requestDto);

            // 3. ASSERT
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);

            Assert.Equal(responseDto, createdResult.Value);
        }
        // this is for post and here two things can happen so testing them both one match one not match
        [Fact]
            public async Task Update_ReturnsOk_WhenGameExists()
            {
                // 1. ARRANGE

                var gameId = Guid.NewGuid();
                var updateDto = new UpdateGameRequestDto("Modern Warfare", "Shooter", 60.00m, "FPS", null);
                var domainGame = new Game { Id = gameId, Name = "Modern Warfare" };
                var responseDto = new GameDto(gameId, "Modern Warfare", "Shooter", 60, "FPS", null);

                // Mocking the chain
                mockMapper.Setup(m => m.Map<Game>(updateDto)).Returns(domainGame);
                mockRepo.Setup(r => r.UpdateAsync(gameId, domainGame)).ReturnsAsync(domainGame);
                mockMapper.Setup(m => m.Map<GameDto>(domainGame)).Returns(responseDto);

                // 2. ACT
                var result = await controller.Update(gameId, updateDto);

                // 3. ASSERT
                var okResult = Assert.IsType<OkObjectResult>(result);
                Assert.Equal(responseDto, okResult.Value);
            }
        [Fact]
        public async Task Update_ReturnsNotFound_WhenGameDoesNotExist()
        {
            // 1. ARRANGE
            var fakeId = Guid.NewGuid();
            var updateDto = new UpdateGameRequestDto("Fake", "val", 21, "Adventure", null);
            //mapping
            mockMapper.Setup(m => m.Map<Game>(updateDto)).Returns(new Game());

            mockRepo.Setup(r => r.UpdateAsync(fakeId, It.IsAny<Game>()))
                     .ReturnsAsync((Game)null);

            // 2. ACT
            var result = await controller.Update(fakeId, updateDto);

            // 3. ASSERT
            // This checks if the controller actually executed "return NotFound();"
            Assert.IsType<NotFoundResult>(result);

        }
        [Fact]
        public async Task Delete_ReturnsOk_WhenGameExists()
        {
            // 1. ARRANGE
            var gameId = Guid.NewGuid();
            var domainGame = new Game { Id = gameId, Name = "Halo" };
            var responseDto = new GameDto(gameId, "Halo", "Sci-fi", 50, "FPS", null);
            // Tell the Repo to return the game as if it successfully deleted it
            mockRepo.Setup(r => r.DeleteAsync(gameId)).ReturnsAsync(domainGame);

            mockMapper.Setup(m => m.Map<GameDto>(domainGame)).Returns(responseDto);

            // 2. ACT
            var result = await controller.Delete(gameId);

            // 3. ASSERT
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(responseDto, okResult.Value);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenGameDoesNotExist()
        {
            // 1. ARRANGE
            var fakeId = Guid.NewGuid();

            // The Repository returns NULL because the game isn't there
            mockRepo.Setup(r => r.DeleteAsync(fakeId)).ReturnsAsync((Game)null);

            // 2. ACT
            var result = await controller.Delete(fakeId);

            // 3. ASSERT
            Assert.IsType<NotFoundResult>(result);
        }
        [Fact]
        public async Task GetById_ReturnsOk_WhenGameExists()
        {
            // 1. ARRANGE
            var gameId = Guid.NewGuid();
            // as the domaingame is class here thogh the properties are required to be set but we can just set the ones we need for the test and ignore the rest as they won't be used in the mapping or the response
            var domainGame = new Game { Id = gameId, Name = "Halo" };
            var responseDto = new GameDto(gameId, "Halo", "Sci-fi", 50, "FPS", null);

            mockRepo.Setup(r => r.GetByIdAsync(gameId)).ReturnsAsync(domainGame);
            mockMapper.Setup(m => m.Map<GameDto>(domainGame)).Returns(responseDto);

            // 2. ACT
            var result = await controller.GetById(gameId);

            // 3. ASSERT
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(responseDto, okResult.Value);
        }
    }
    }
