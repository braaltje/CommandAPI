namespace CommandAPI.Tests
{
    public class CommandsControllerTests : IDisposable
    {
        Mock<ICommandAPIRepo> mockRepo;
        CommandsProfile realProfile;
        MapperConfiguration configuration;
        IMapper mapper;

        public CommandsControllerTests()
        {
            //Arrange
            //1 - create Mock object that 'points'to our repository interface
            mockRepo = new Mock<ICommandAPIRepo>();
            //  instantiate CommandsProfile
            realProfile = new CommandsProfile();
            //2 - create a Mapper instance and 'assign' CommandsProfiles
            //create MapperConfig object
            configuration = new MapperConfiguration( cfg =>
                cfg.AddProfile(realProfile));
            mapper = new Mapper(configuration);
        }

        public void Dispose()
        {
            mockRepo = null;
            realProfile = null;
            configuration = null;
            mapper = null;
        }

        [Fact]
        public void GetCommandItems_ReturnsZeroItems_WhenDBIsEmpty()
        {
            //Arrange

            //calling GetCommands() with a value of zero means the DB is emptry (no Command in List<Command>)
            mockRepo.Setup(repo => 
                repo.GetAllCommands()).Returns(GetCommands(0));

            var controller = new CommandsController(mockRepo.Object, mapper);

            //Act - get all commands
            var result = controller.GetAllCommands();

            //Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void GetAllCommands_ReturnsOneItem_WhenDBHasOneResource()
        {
            //arrange
            mockRepo.Setup(repo =>
                repo.GetAllCommands()).Returns(GetCommands(1));
            var controller = new CommandsController(mockRepo.Object, mapper);

            //act
            var result = controller.GetAllCommands();

            //assert
            //Resultset of GetAllCommands is enclosed in an Ok(). So we first need to
            //derive the result from the OkObjectResult and then get its value as a
            //List<CommandReadDto>
            var okResult = result.Result as OkObjectResult;
            var commands = okResult.Value as List<CommandReadDto>;
            Assert.Single(commands);
        }

        [Fact]
        public void GetAllCommands_Returns200OK_WhenDBHasOneResource()
        {
            //arrange
            mockRepo.Setup(repo =>
                repo.GetAllCommands()).Returns(GetCommands(1));
            var controller = new CommandsController(mockRepo.Object, mapper);

            //act
            var result = controller.GetAllCommands();

            //assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void GetAllCommands_ReturnsCorrectType_WhenDBHasOneResource()
        {
            //arrange
            mockRepo.Setup(repo =>
                repo.GetAllCommands()).Returns(GetCommands(1));
            var controller = new CommandsController(mockRepo.Object, mapper);

            //act
            var result = controller.GetAllCommands();

            //assert
            Assert.IsType<ActionResult<IEnumerable<CommandReadDto>>>(result);
        }

        [Fact]
        public void GetCommandById_Returns404NotFound_WhenIDIsNonExistent()
        {
            //arrange - setup the GetCommandById to return null when we pass 0
            mockRepo.Setup(repo => 
                repo.GetCommandById(0)).Returns( () => null);
            var controller = new CommandsController(mockRepo.Object, mapper);
            //act
            var result = controller.GetCommandById(1);
            //assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void GetCommandById_Returns200OK_WhenValidIDProvided()
        {
            //arrange
            mockRepo.Setup(repo =>
                repo.GetCommandById(1)).Returns(new Command
                    { 
                        Id = 1,
                        HowTo = "mock",
                        Platform = "mock",
                        CommandLine = "mock"
                    });
            var controller = new CommandsController(mockRepo.Object, mapper);

            //act
            var result = controller.GetCommandById(1);
            //assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void GetCommandById_ReturnsCorrectType_WhenValidIDProvided()
        {
            //arrange
            mockRepo.Setup(repo =>
                repo.GetCommandById(1)).Returns(new Command {
                    Id = 1,
                    HowTo = "mock",
                    Platform = "mock",
                    CommandLine = "mock"
                });
            var controller = new CommandsController(mockRepo.Object, mapper);
            //act
            var result = controller.GetCommandById(1);
            //assert
            Assert.IsType<ActionResult<CommandReadDto>>(result);
        }

        [Fact]
        public void CreateCommand_ReturnsCorrectType_WhenValidObjectSubmitted()
        {
            //arrange
            mockRepo.Setup(repo =>
                repo.GetCommandById(1)).Returns(new Command { 
                    Id = 1,
                    HowTo = "Mock",
                    Platform = "Mock",
                    CommandLine = "Mock"
                });
            var controller = new CommandsController(mockRepo.Object, mapper);
            //act
            var result = controller.CreateCommand(new CommandCreateDto{ });
            //assert
            Assert.IsType<ActionResult<CommandReadDto>>(result);
        }

        [Fact]
        public void CreateCommand_Returns201Created_WhenValidObjectSubmitted()
        {
            mockRepo.Setup(repo =>
                repo.GetCommandById(1)).Returns(new Command {
                    Id = 1,
                    HowTo = "Mock",
                    Platform = "Mock",
                    CommandLine = "Mock"
                });
            var controller = new CommandsController(mockRepo.Object, mapper);

            var result = controller.CreateCommand(new CommandCreateDto{ });

            Assert.IsType<CreatedAtRouteResult>(result.Result);
        }

        [Fact]
        public void UpdateCommand_Returns204NoContent_WhenValidObjectSubmitted()
        {
            //arrange
            mockRepo.Setup(repo =>
                repo.GetCommandById(1)).Returns(new Command { 
                    Id = 1,
                    HowTo = "Mock",
                    Platform = "Mock",
                    CommandLine = "Mock"
                });
            var controller = new CommandsController(mockRepo.Object, mapper);
            //act
            var result = controller.UpdateCommand(1, new CommandUpdateDto{ });
            //assert
            Assert.IsType<NoContentResult>(result.Result);
        }

        [Fact]
        public void UpdateCommand_Returns404NotFound_WhenNonExistentResourceIDSubmitted()
        {
            //arrange
            mockRepo.Setup(repo =>
                repo.GetCommandById(0)).Returns(() => null);
            var controller = new CommandsController(mockRepo.Object, mapper);
            //act
            var result = controller.UpdateCommand(0, new CommandUpdateDto{ });
            //assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void PartialCommandUpdate_Returns404NotFound_WhenNonExistentResourceIDSubmitted()
        {
            //arrange
            mockRepo.Setup(repo =>
                repo.GetCommandById(0)).Returns(() => null);
            var controller = new CommandsController(mockRepo.Object, mapper);
            //act
            var result = controller.PartialCommandUpate(0, 
                new Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<CommandUpdateDto>{ });
            //assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void DeleteCommand_Returns204NoContent_WhenValidResourceIDSubmitted()
        {
            mockRepo.Setup(repo =>
                repo.GetCommandById(1)).Returns(new Command {
                    Id = 1,
                    HowTo = "Mock",
                    Platform = "Mock",
                    CommandLine = "Mock"
                });
            var controller = new CommandsController(mockRepo.Object, mapper);

            var result = controller.DeleteCommand(1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void DeleteCommand_Returns4040NotFound_WhenNonExistentResourceIDSubmitted()
        {
            mockRepo.Setup(repo => 
                repo.GetCommandById(0)).Returns(() => null);
            var controller = new CommandsController(mockRepo.Object, mapper);

            var result = controller.DeleteCommand(0);

            Assert.IsType<NotFoundResult>(result);
        }


        //private method to return a list with 1 item or an empty list.
        private List<Command> GetCommands(int num)
        {
            var commands = new List<Command>();
            if(num > 0)
            {
                commands.Add(new Command
                {
                    Id = 0,
                    HowTo = "How to generate a migration",
                    CommandLine = "dotnet ef migrations add <Name of Migration>",
                    Platform = ".Net Core EF"
                });
            }
            return commands;
        }
    }
}