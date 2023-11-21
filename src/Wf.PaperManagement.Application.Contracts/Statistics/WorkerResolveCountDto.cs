namespace csuwf.PaperManagement.Statistics;

public class WorkerResolveCountDto
{
    public string WorkerName { get; set; } = null!;
    public int WorkerId { get; set; }
    public int ResolveCount { get; set; }
}