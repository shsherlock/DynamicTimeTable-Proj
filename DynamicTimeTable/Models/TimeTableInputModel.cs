using System.ComponentModel.DataAnnotations;

namespace DynamicTimeTable.Models
{

    //public class InputModel
    //{
    //    [Required]
    //    [Range(1, 7, ErrorMessage = "Working days must be between 1 and 7")]
    //    public int WorkingDays { get; set; }

    //    [Required]
    //    [Range(1, 8)]
    //    public int SubjectsPerDay { get; set; }

    //    [Required]
    //    [Range(1, int.MaxValue)]
    //    public int TotalSubjects { get; set; }

    //    public int TotalHours => WorkingDays * SubjectsPerDay;
    //}

    #region TimeTable Input fields View
    public class TimeTableViewModel
    {
        // Step 1 inputs
        [Required]
        [Range(1, 7)]
        public int? WorkingDays { get; set; }

        //[Required]
        //[Range(1, 8)]
        public int? SubjectsPerDay { get; set; }

        //[Required]
        //[Range(1, int.MaxValue)]
        public int? TotalSubjects { get; set; }

        public int TotalHours => (WorkingDays ?? 0) * (SubjectsPerDay ?? 0);

        // Step 2: Subject list
        public List<SubjectHour> SubjectHours { get; set; } = new List<SubjectHour>();

        // Step 3: Generated timetable
        public TimeTableModel TimeTable { get; set; }
    }

    #endregion

    #region Subject and Hour 
    public class SubjectHourModel
    {
        public int TotalHours { get; set; }

        public List<SubjectHour> Subjects { get; set; }
    }

    public class SubjectHour
    {
        public string Name { get; set; }
        public int Hours { get; set; }
    }
    #endregion


    #region TimeTableModelClass 
    public class TimeTableModel
    {
        public List<string> Days { get; set; }
        public List<List<string>> Schedule { get; set; }
    }
    #endregion
}
