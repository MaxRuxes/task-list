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
        private readonly IUnitOfWork _database;
        private readonly IMapper _mapper;

        public ProjectService(IUnitOfWork uow)
        {
            _database = uow;
            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProjectInfoDTO, ProjectInfo>();
                cfg.CreateMap<ProjectInfo, ProjectInfoDTO>();
            }).CreateMapper();
        }

        public IEnumerable<ProjectInfoDTO> GetAllProjects()
        {
            return _mapper.Map<IEnumerable<ProjectInfo>, List<ProjectInfoDTO>>(_database.ProjectInfo.GetAll());
        }

        public ProjectInfoDTO CreateProject(ProjectInfoDTO project)
        {
            var input = _mapper.Map<ProjectInfoDTO, ProjectInfo>(project);
            var output = _database.ProjectInfo.Create(input);
            _database.Save();

            return _mapper.Map<ProjectInfo, ProjectInfoDTO>(output);
        }

        public void UpdateProject(ProjectInfoDTO projectInfo)
        {
            var input = _mapper.Map<ProjectInfoDTO, ProjectInfo>(projectInfo);
            _database.ProjectInfo.Update(input);
            _database.Save();
        }

        public void DeleteProject(int idProject)
        {
            var todoAndProjectses = _database.TodoAndProjects.Find(x => x.IdProject == idProject);
            foreach (var item in todoAndProjectses)
            {
                var usersTask = _database.TodoAndUsers.Find(x => x.IdTodo == item.IdTodo);
                foreach (var task in usersTask)
                {
                    _database.TodoAndUsers.Delete(task.TodoAndUsersId);
                }

                _database.TodoAndProjects.Delete(item.TodoAndProjectsId);

                _database.Todos.Delete(item.IdTodo);
            }

            var projects = _database.Projects.Find(x => x.IdProjectInfo == idProject);
            foreach (var project in projects)
            {
                _database.Projects.Delete(project.ProjectsId);
            }

            _database.ProjectInfo.Delete(idProject);
            _database.Save();
        }

        public int GetCostForProject(int idProject)
        {
            var todoForProject = _database.TodoAndProjects.Find(x => x.IdProject == idProject)
                .ToList();
            if (!todoForProject.Any())
            {
                return 0;
            }

            var allTodos = todoForProject.Select(x => x.IdTodo);
            var summ = 0;
            foreach (var idTodo in allTodos)
            {
                var usersWithTodo = _database.TodoAndUsers.Find(x => x.IdTodo == idTodo).FirstOrDefault();
                if (usersWithTodo == null)
                {
                    return 0;
                }

                var user = _database.Users.Get(usersWithTodo.Iduser);
                var todo = _database.Todos.Get(idTodo);

                summ += user.RatePerHour * todo.EstimatedHours;
            }

            return summ;
        }

        public int GetCountWorkersForProject(int idProject)
        {
            var project = _database.ProjectInfo.Find(x => x.ProjectInfoId == idProject).FirstOrDefault();
            if (project == null)
            {
                return 0;
            }

            var workersCount = _database.Projects.Find(x => x.IdProjectInfo == idProject).Count();
            return workersCount;
        }

        public int GetCountTodoForProject(int idProject)
        {
            var project = _database.ProjectInfo.Find(x => x.ProjectInfoId == idProject).FirstOrDefault();
            if (project == null)
            {
                return 0;
            }

            var todoCount = _database.TodoAndProjects.Find(x => x.IdProject == idProject)
                .Select(x=>x.IdTodo)
                .ToList();
            var todos = _database.Todos.Find(x => todoCount.Contains(x.TodoId)).Sum(x=>x.EstimatedHours);
            return todos;
        }

        public int GetSpentTimeTodoForProject(int idProject)
        {
            var project = _database.ProjectInfo.Find(x => x.ProjectInfoId == idProject).FirstOrDefault();
            if (project == null)
            {
                return 0;
            }

            var todoCount = _database.TodoAndProjects.Find(x => x.IdProject == idProject)
                .Select(x => x.IdTodo)
                .ToList();
            var todos = _database.Todos.Find(x => todoCount.Contains(x.TodoId)).Sum(x => x.SpentTime);
            return todos;
        }
    }
}
