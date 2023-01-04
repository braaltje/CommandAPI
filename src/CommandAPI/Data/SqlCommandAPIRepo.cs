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
            if(cmd == null)
            {
                throw new ArgumentNullException(nameof(cmd));
            }
            _dbContext.CommandItems.Add(cmd);
        }

        public void DeleteCommand(Command cmd)
        {
            if(cmd == null)
            {
                throw new ArgumentNullException(nameof(cmd));
            }
            _dbContext.CommandItems.Remove(cmd);
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
            //checks whether and how many changes to the db were detected and returns true when > 0
            return (_dbContext.SaveChanges() >= 0);
        }

        public void UpdateCommand(Command cmd)
        {
            //we don't need to do anything here
        }
    }
}