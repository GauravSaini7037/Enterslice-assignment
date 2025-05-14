namespace TaskManagementApi.Models
{
    public class TaskModel
    {
        public int Id { get; set; }
        public string TaskTitle { get; set; }
        public string TaskDescription { get; set; }
        public DateTime TaskDueDate { get; set; }
        public string TaskStatus { get; set; }
        public string TaskRemarks { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string CreatedByName { get; set; }
        public int CreatedById { get; set; }
        public string LastUpdatedByName { get; set; }
        public int LastUpdatedById { get; set; }
    }
}
