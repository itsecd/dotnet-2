namespace PPTaskList.Controllers.Model
{
    public class Executor
    {
        public int ExecutorId { get; set; }

        public string Name { get; set; }

        public Executor(int id, string name)
        {
            ExecutorId = id;
            Name = name;
        }
    }
}
