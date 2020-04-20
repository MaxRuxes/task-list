using AutoMapper;
using System.Linq;
using System.Collections.Generic;
using TaskList.BLL.DTO;
using TaskList.BLL.Interfaces;
using TaskList.DAL.Interfaces;
using TaskList.DAL.Entities;

namespace TaskList.BLL.Services
{
    public class ProjectService : IProjectService
    {
        private IUnitOfWork Database { get; set; }
        private readonly IMapper mapper;

        public ProjectService(IUnitOfWork uow)
        {
            Database = uow;
            mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProjectInfoDTO, ProjectInfo>();
                cfg.CreateMap<ProjectInfo, ProjectInfoDTO>();
            }).CreateMapper();
        }

        public IEnumerable<ProjectInfoDTO> GetAllProjects()
        {
            return mapper.Map<IEnumerable<ProjectInfo>, List<ProjectInfoDTO>>(Database.ProjectInfo.GetAll());
        }

        public ProjectInfoDTO CreateProject(ProjectInfoDTO project)
        {
            var input = mapper.Map<ProjectInfoDTO, ProjectInfo>(project);
            var output = Database.ProjectInfo.Create(input);
            Database.Save();

            return mapper.Map<ProjectInfo, ProjectInfoDTO>(output);
        }

        public void UpdateProject(ProjectInfoDTO projectInfo)
        {
            var input = mapper.Map<ProjectInfoDTO, ProjectInfo>(projectInfo);
            Database.ProjectInfo.Update(input);
            Database.Save();
        }

        public void DeleteProject(int idProject)
        {
            var todoAndProjectses = Database.TodoAndProjects.Find(x => x.IdProject == idProject);
            foreach (var item in todoAndProjectses)
            {
                var usersTask = Database.TodoAndUsers.Find(x => x.IdTodo == item.IdTodo);
                foreach (var task in usersTask)
                {
                    Database.TodoAndUsers.Delete(task.TodoAndUsersId);
                }

                Database.TodoAndProjects.Delete(item.TodoAndProjectsId);

                Database.Todos.Delete(item.IdTodo);
            }

            var projects = Database.Projects.Find(x => x.IdProjectInfo == idProject);
            foreach (var project in projects)
            {
                Database.Projects.Delete(project.ProjectsId);
            }

            Database.ProjectInfo.Delete(idProject);
            Database.Save();
        }

        public int GetCostForProject(int idProject)
        {
            var todoForProject = Database.TodoAndProjects.Find(x => x.IdProject == idProject)
                .ToList();
            if (!todoForProject.Any())
            {
                return 0;
            }

            var allTodos = todoForProject.Select(x => x.IdTodo);
            var summ = 0;
            foreach (var idTodo in allTodos)
            {
                var usersWithTodo = Database.TodoAndUsers.Find(x => x.IdTodo == idTodo).FirstOrDefault();
                if (usersWithTodo == null)
                {
                    return 0;
                }

                var user = Database.Users.Get(usersWithTodo.Iduser);
                var todo = Database.Todos.Get(idTodo);

                summ += user.RatePerHour * todo.EstimatedHours;
            }

            return summ;
        }

        public int GetCountWorkersForProject(int idProject)
        {
            var project = Database.ProjectInfo.Find(x => x.ProjectInfoId == idProject).FirstOrDefault();
            if (project == null)
            {
                return 0;
            }

            var workersCount = Database.Projects.Find(x => x.IdProjectInfo == idProject).Count();
            return workersCount;
        }

        public int GetCountTodoForProject(int idProject)
        {
            var project = Database.ProjectInfo.Find(x => x.ProjectInfoId == idProject).FirstOrDefault();
            if (project == null)
            {
                return 0;
            }

            var todoCount = Database.TodoAndProjects.Find(x => x.IdProject == idProject)
                .Select(x=>x.IdTodo)
                .ToList();
            var todos = Database.Todos.Find(x => todoCount.Contains(x.TodoId)).Sum(x=>x.EstimatedHours);
            return todos;
        }

        public int GetSpentTimeTodoForProject(int idProject)
        {
            var project = Database.ProjectInfo.Find(x => x.ProjectInfoId == idProject).FirstOrDefault();
            if (project == null)
            {
                return 0;
            }

            var todoCount = Database.TodoAndProjects.Find(x => x.IdProject == idProject)
                .Select(x => x.IdTodo)
                .ToList();
            var todos = Database.Todos.Find(x => todoCount.Contains(x.TodoId)).Sum(x => x.SpentTime);
            return todos;
        }


        public IEnumerable<ProjectInfoDTO> GetProjectsForUser(int userId)
        {
            return mapper.Map<IEnumerable<ProjectInfo>, List<ProjectInfoDTO>>(Database.Projects.Find(o => o.IdUser == userId).Select(o => o.ProjectInfo)); 
        }
    }
}
