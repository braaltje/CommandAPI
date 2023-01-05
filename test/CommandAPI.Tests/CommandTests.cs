namespace ComnmandAPI.Tests
{
    public class CommandTests : IDisposable
    {
        Command testCommand;

        public CommandTests()
        {
            testCommand = new Command{                                
        
            HowTo = "Do something",
            Platform = "Some platform",
            CommandLine = "Some commandline"            
            };
        }

        public void Dispose()
        {
            testCommand = null;
        }

        [Fact]
        public void CanChangeHowTo()
        {
            //Arrange

            //Act
            testCommand.HowTo = "Execute Unit Tests";

            //Assert
            Assert.Equal("Execute Unit Tests", testCommand.HowTo);
        }

        [Fact]
        public void CanChangePlatform()
        {
            //act
            testCommand.Platform = "xUnit";
            //assert
            Assert.Equal("xUnit", testCommand.Platform);
        }

        [Fact]
        public void CanChangeCommandLine()
        {
            //act
            testCommand.CommandLine = "dotnet test";
            //assert
            Assert.Equal("dotnet test", testCommand.CommandLine);
        }

    }
}