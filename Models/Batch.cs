namespace Kombucha.Models;

public class Batch {
    public long Id { get; set; }
    public DateOnly StartTime { get; set; }
    public int GramsOfSugar { get; set; }
    public int GramsOfTea { get; set; }
    public int SteepMinutes { get; set; }
    public string Description { get; set; }

    public long SugarId { get; set; }
    public virtual Sugar Sugar { get; set; }
    public long TeaId { get; set; }
    public virtual Tea Tea { get; set; }
}

public class BatchDTO {
    public long Id { get; set; }
    public DateOnly StartTime { get; set; }
    public string SugarType { get; set; }
    public int GramsOfSugar { get; set; }
    public string TeaType { get; set; }
    public int GramsOfTea { get; set; }
    public int SteepMinutes { get; set; }
    public string Description { get; set; }
}

public class BatchPostDTO {
    public DateOnly StartTime { get; set; }
    public long SugarId { get; set; }
    public int GramsOfSugar { get; set; }
    public long TeaId { get; set; }
    public int GramsOfTea { get; set; }
    public int SteepMinutes { get; set; }
    public string Description { get; set; }
}
