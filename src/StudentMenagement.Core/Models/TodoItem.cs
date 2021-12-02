using System.ComponentModel.DataAnnotations;

namespace StudentMenagement.Models
{
    public class TodoItem
    {
        [Key]
        public long Id { get; set; }

        public string Name { get; set; }
        public bool IsComplete { get; set; }
    }
}
