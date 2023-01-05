namespace CommandAPI.Tests
{
    public class CommandsControllerTests
    {
        [Fact]
        public void GetCommandItems_ReturnsZeroItems_WhenDBIsEmpty()
        {
            //Arrange - we need to create an instance of our CommandsController class
            //create Mock object that 'points'to our repository interface
            var mockRepo = new Mock<ICommandAPIRepo>();

            //calling GetCommands() with a value of zero means the DB is emptry (no Command in List<Command>)
            mockRepo.Setup(repo => 
                repo.GetAllCommands()).Returns(GetCommands(0));

            var controller = new CommandsController(mockRepo.Object, /* AutoMapper */);
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