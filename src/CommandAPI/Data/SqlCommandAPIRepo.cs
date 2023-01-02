using System.Linq;
using CommandAPI.Models;

namespace CommandAPI.Data
{
    public class SqlCommandAPIRepo : ICommandAPIRepo
    {
        private readonly CommandContext _dbContext;

        public SqlCommandAPIRepo(CommandContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void CreateCommand(Command cmd)
        {
            throw new NotImplementedException();
        }

        public void Delete(Command cmd)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Command> GetAllCommands()
        {
            return _dbContext.CommandItems.ToList();
        }

        public Command GetCommandById(int id)
        {
            return _dbContext.CommandItems.FirstOrDefault(p => p.Id == id);
        }

        public bool SaveChanges()
        {
            throw new NotImplementedException();
        }

        public void UpdateCommand(Command cmd)
        {
            throw new NotImplementedException();
        }
    }
}