using System.Linq;
using TaskList.DAL;

namespace TaskList.BLL.Services
{
    public class FunctionManagerService
    {
        private TaskListContext context;

        public FunctionManagerService(string conn)
        {
            context = new TaskListContext(conn);
        }

        public int Execute(string query)
        {
            return context.Database.SqlQuery<int>(query).FirstOrDefault();
        }
    }
}
