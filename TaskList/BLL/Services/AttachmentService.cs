using System.Collections.Generic;
using System.Linq;
using TaskList.BLL.DTO;
using TaskList.BLL.Interfaces;
using AutoMapper;
using TaskList.DAL.Interfaces;
using TaskList.DAL.Models;
using TaskList.BLL.Infrastructure;

namespace TaskList.BLL.Services
{
    public class AttachmentService : IAttachmentService
    {
        private IUnitOfWork Database { get; set; }
        private IMapper mapper;

        public AttachmentService(IUnitOfWork uow)
        {
            Database = uow;
            mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TodoDTO, Todo>();
                cfg.CreateMap<AttachmentsDTO, Attachments>();
                cfg.CreateMap<Attachments, AttachmentsDTO>();
            }).CreateMapper();
        }

        /// <summary>
        /// Добавляет вложение к определенному todo
        /// </summary>
        /// <param name="todo"></param>
        /// <param name="attachments"></param>
        public void AddAttachment(TodoDTO todo, AttachmentsDTO attachments)
        {
            // Если вложение существует -- то набрасываем и убегаем
            var itemAttach = Database.Attachments.Get(attachments.AttachmentsId);
            if (itemAttach != null)
            {
                throw new ValidationException("Attach already exists", "");
            }
            Database.Attachments.Create(mapper.Map<AttachmentsDTO, Attachments>(attachments));

            // Если таски не существует -- добавляем
            var findingTodoItem = Database.Todos.Get(todo.TodoId);
            if (findingTodoItem == null)
            {
                Database.Todos.Create(mapper.Map<TodoDTO, Todo>(todo));
            }

            // добавляем данные в связующую таблицу
            var todoAndAttaches = new TodoAndAttaches
            {
                IdTodo = todo.TodoId,
                IdAttach = attachments.AttachmentsId
            };

            Database.TodoAndAttaches.Create(todoAndAttaches);

            Database.Save();
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        public AttachmentsDTO GetAttachment(int? todoId, int? idAttach)
        {
            var res = Database.TodoAndAttaches
                .GetAll()
                .Where(o => o.IdTodo == todoId && o.IdAttach == idAttach)
                .ToList().
                FirstOrDefault();

            return mapper.Map<Attachments, AttachmentsDTO>(res.Attachment);
        }

        public void UpdateAttachment(AttachmentsDTO attachments)
        {
            Database.Attachments.Update(mapper.Map<AttachmentsDTO, Attachments>(attachments));
        }

        /// <summary>
        /// Открепляет todo от указанного todo
        /// </summary>
        /// <param name="todo"></param>
        /// <param name="attachments"></param>
        public void DeleteAttachment(TodoDTO todo, AttachmentsDTO attachments)
        {
            // ищем в базе таску
            var todoItem = Database.Todos.Get(todo.TodoId);
            if (todoItem != null)
            {
                // выбираем все записи с данным аттачем и таской
                var listAttaches = Database.TodoAndAttaches.GetAll()
                    .Where(o => o.IdTodo == todo.TodoId && o.IdAttach == attachments.AttachmentsId)
                    .ToList();
                if (listAttaches != null)
                {
                    // удаляем эти записи
                    foreach (var item in listAttaches)
                    {
                        Database.TodoAndAttaches.Delete(item.TodoAndAttachesId);
                    }
                }
            }
        }

        public IEnumerable<AttachmentsDTO> GetAllAttachments(int? todoId)
        {
            var res = Database.TodoAndAttaches
                .GetAll()
                .Where(o => o.IdTodo == todoId)
                .Select(o => o.Attachment)
                .ToList();

            return mapper.Map<List<Attachments>, IEnumerable<AttachmentsDTO>>(res);
        }
    }
}
