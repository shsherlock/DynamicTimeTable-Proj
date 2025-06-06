using System.ComponentModel.DataAnnotations;

namespace DynamicTimeTable.Models.DynamicTimeTable
{
    public class SubjectHourModel
    {
        public string SubjectName { get; set; }
        public int Hours { get; set; }
    }
}
