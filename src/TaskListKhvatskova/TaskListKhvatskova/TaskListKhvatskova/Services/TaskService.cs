using Grpc.Core;
using System.Threading.Tasks;
using TaskListKhvatskova.Models;
using TaskListKhvatskova.Repositories;
using System.Linq;

namespace TaskListKhvatskova.Services
{
    public class TaskService : tasksList.tasksListBase
    {
        private readonly IExecutorRepository _executorRepository;
        private readonly ITagRepository _tagRepository;
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository, ITagRepository tagRepository, IExecutorRepository executorRepository)
        {
            _taskRepository = taskRepository;       
            _tagRepository = tagRepository;
            _executorRepository = executorRepository;
        }

        public override Task<AllTaskReply> GetAllTasks(NullRequest request, ServerCallContext context)
        {
            AllTaskReply allTaskReply = new();
            foreach(MyTask task in _taskRepository.GetTasks())
            {
                TaskReply taskReply = new();
                taskReply.TaskId = task.TaskId;
                taskReply.Name = task.Name;
                taskReply.Description = task.Description;
                taskReply.TaskState = task.TaskState;
                taskReply.ExecutorId = task.ExecutorId;
                foreach(int tag in task.TagsId)
                {
                    taskReply.TagsId.Add(tag);
                }
                allTaskReply.Tasks.Add(taskReply);
            }
            return Task.FromResult(allTaskReply);
        }

        public override Task<TaskReply> GetTask(TaskRequest request, ServerCallContext context)
        {
            TaskReply taskReply = new();
            MyTask task = _taskRepository.Get(request.TaskId);
            if(task is not null)
            {
                taskReply.TaskId = task.TaskId;
                taskReply.Name = task.Name;
                taskReply.Description = task.Description;
                taskReply.TaskState = task.TaskState;
                taskReply.ExecutorId = task.ExecutorId;
                foreach (int tag in task.TagsId)
                {
                    taskReply.TagsId.Add(tag);
                }
                return Task.FromResult(taskReply);
            }
            return Task.FromResult(new TaskReply { ExaminationReply = new ExaminationReply {NotFoundException = true } });
        }

        public override Task<TaskReply> AddTask(TaskRequest request, ServerCallContext context)
        {
            var tags = request.TagsId.ToList();
            return Task.FromResult(new TaskReply
            {
                TaskId = _taskRepository.AddTask(new MyTask(request.TaskId, request.Name, request.Description, request.TaskState, request.ExecutorId, tags))
            });
        }

        public override Task<TaskReply> UpdateTask(TaskRequest request, ServerCallContext context)
        {
            var tags = request.TagsId.ToList();
            return Task.FromResult(new TaskReply
            {
                TaskId = _taskRepository.UpdateTask(request.TaskId, new MyTask(request.TaskId, request.Name, request.Description, request.TaskState, request.ExecutorId, tags))
            });
        }

        public override Task<TaskReply> UpdateTaskState(TaskRequest request, ServerCallContext context)
        {
            return Task.FromResult(new TaskReply
            {
                TaskId = _taskRepository.UpdateTaskState(request.TaskId, request.TaskState)
            });
        }

        public override Task<TaskReply> RemoveTask(TaskRequest request, ServerCallContext context)
        {
            return Task.FromResult(new TaskReply
            {
                TaskId = _taskRepository.RemoveTask(request.TaskId)
            });
        }


        public override Task<AllExecutorReply> GetAllExecutors(NullRequest request, ServerCallContext context)
        {
            AllExecutorReply reply = new();
            foreach(Executor executor in _executorRepository.GetExecutors())
            {
                reply.Executors.Add(new ExecutorReply
                {
                    ExecutorId = executor.ExecutorId,
                    Name = executor.Name,
                    Surname = executor.Surname
                });
            }
            return Task.FromResult(reply);
        }

        public override Task<ExecutorReply> GetExecutor(ExecutorRequest request, ServerCallContext context)
        {
            Executor executor = _executorRepository.Get(request.ExecutorId);
            return Task.FromResult(new ExecutorReply
            {
                ExecutorId = executor.ExecutorId,
                Name = request.Name,
                Surname = request.Surname
            });
        }

        public override Task<ExecutorReply> AddExecutor(ExecutorRequest request, ServerCallContext context)
        {
            return Task.FromResult(new ExecutorReply
            {
                ExecutorId = _executorRepository.AddExecutor(new Executor(request.Name, request.Surname))
            });
        }

        public override Task<ExecutorReply> UpdateExecutor(ExecutorRequest request, ServerCallContext context)
        {
            return Task.FromResult(new ExecutorReply
            {
                ExecutorId = _executorRepository.UpdateExecutor(request.ExecutorId, new Executor(request.ExecutorId, request.Name, request.Surname))
            });
        }

        public override Task<ExecutorReply> RemoveExecutor(ExecutorRequest request, ServerCallContext context)
        {
            return Task.FromResult(new ExecutorReply
            {
                ExecutorId = _executorRepository.RemoveExecutor(request.ExecutorId)
            });
        }


        public override Task<AllTagReply> GetAllTags(NullRequest request, ServerCallContext context)
        {
            AllTagReply allTagReply = new();
            foreach(Tags tag in _tagRepository.GetTags())
            {
                allTagReply.Tags.Add(new TagReply
                {
                    TagId = tag.TagId,
                    Name = tag.Name,
                    Color = tag.Color,
                });
            }
            return Task.FromResult(allTagReply);
        }

        public override Task<TagReply> GetTag(TagRequest request, ServerCallContext context)
        {
            Tags tags = _tagRepository.Get(request.TagId);
            return Task.FromResult(new TagReply
            {
                TagId = tags.TagId,
                Name = tags.Name,
                Color = tags.Color,
            });
        }

        public override Task<TagReply> AddTag(TagRequest request, ServerCallContext context)
        {
            return Task.FromResult(new TagReply
            {
                TagId = _tagRepository.AddTag(new Tags(request.Name, request.Color))
            });
        }

        public override Task<TagReply> UpdateTag(TagRequest request, ServerCallContext context)
        {
            return Task.FromResult(new TagReply
            {
                TagId = _tagRepository.UpdateTag(request.TagId, new Tags(request.TagId, request.Name, request.Color))
            });
        }

        public override Task<TagReply> RemoveTag(TagRequest request, ServerCallContext context)
        {
            return Task.FromResult(new TagReply
            {
                TagId = _tagRepository.RemoveTag(request.TagId)
            });
        }

        public override Task<NullRequest> RemoveAllTags(NullRequest request, ServerCallContext context)
        {
            _tagRepository.RemoveAllTags();
            return Task.FromResult( new NullRequest() );
        }
    }
}
