namespace HelloWorld.Tasks
{
    public interface ITaskDelayConfiguration
    {
        int ForceDelay { get; set; }
    }

    public class TaskDelayConfiguration : ITaskDelayConfiguration
    {
        public int ForceDelay { get; set; }
    }
}