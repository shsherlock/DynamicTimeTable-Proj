using Microsoft.AspNetCore.Mvc;
using DynamicTimeTable.Models.DynamicTimeTable; // Corrected: consistent with your folder
using System;
using System.Collections.Generic;
using System.Linq;

namespace DynamicTimeTable.Controllers
{
    public class TimeTableController : Controller
    {
        [HttpGet]
        public IActionResult TableInput()
        {
            return View(new InitialInputModel());
        }

        [HttpGet]
        public IActionResult GenrateTimeTable()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SubjectHours(InitialInputModel model)
        {
            if (!ModelState.IsValid)
                return View("InitialInput", model);

            TempData["WorkingDays"] = model.WorkingDays;
            TempData["SubjectsPerDay"] = model.SubjectsPerDay;
            TempData["TotalHours"] = model.TotalHours;
            TempData["TotalSubjects"] = model.TotalSubjects;

            ViewBag.TotalHours = model.TotalHours;
            ViewBag.TotalSubjects = model.TotalSubjects;

            // Creating an empty list of subject models
            var subjectHourList = new List<SubjectHourModel>();
            for (int i = 0; i < model.TotalSubjects; i++)
            {
                subjectHourList.Add(new SubjectHourModel());
            }

            return View("SubjectHours", subjectHourList);
        }

        [HttpPost]
        public IActionResult Generate(List<SubjectHourModel> subjects)
        {
            // TempData survives only one request, so keep it alive
            TempData.Keep();

            int totalHours = Convert.ToInt32(TempData["TotalHours"]);
            int workingDays = Convert.ToInt32(TempData["WorkingDays"]);
            int subjectsPerDay = Convert.ToInt32(TempData["SubjectsPerDay"]);

            int enteredHours = subjects.Sum(s => s.Hours);
            if (enteredHours != totalHours)
            {
                ViewBag.TotalHours = totalHours;
                ViewBag.TotalSubjects = subjects.Count;
                ViewBag.Error = "Total subject hours must match total weekly hours.";
                return View("SubjectHours", subjects);
            }

            var timetable = GenerateTimeTable(subjects, workingDays, subjectsPerDay);

            ViewBag.WorkingDays = workingDays;
            ViewBag.SubjectsPerDay = subjectsPerDay;

            return View("TimeTable", timetable);
        }

        private string[,] GenerateTimeTable(List<SubjectHourModel> subjects, int days, int slotsPerDay)
        {
            var subjectPool = new List<string>();
            foreach (var sub in subjects)
            {
                subjectPool.AddRange(Enumerable.Repeat(sub.SubjectName, sub.Hours));
            }

            var random = new Random();
            subjectPool = subjectPool.OrderBy(_ => random.Next()).ToList();

            string[,] table = new string[slotsPerDay, days];
            int indexofSub = 0;

            for (int row = 0; row < slotsPerDay; row++)
            {
                for (int col = 0; col < days; col++)
                {
                    if (indexofSub < subjectPool.Count)
                        table[row, col] = subjectPool[indexofSub++];
                    else
                        table[row, col] = "";
                }
            }

            return table;
        }
    }
}























    //public class TimeTableController : Controller
    //{
    //    [HttpGet]
    //    public IActionResult CreatTimeTable()
    //    {
    //        return View(new TimeTableViewModel());
    //    }

    //    [HttpPost]
    //    public IActionResult TimeTableForm(TimeTableViewModel model, string action)
    //    {
    //        if (action == "step1")
    //        {
    //            if (ModelState.IsValid)
    //                return View(model);

    //            // Create empty subject list for the next form
    //            model.SubjectHours = Enumerable.Range(1, model.TotalSubjects ?? 0)
    //                .Select(i => new SubjectHour()).ToList();

    //            return View(model);
    //        }
    //        else if (action == "step2")
    //        {
    //            var totalHours = model.WorkingDays.Value * model.SubjectsPerDay.Value;
    //            if (model.SubjectHours.Sum(s => s.Hours) != totalHours)
    //            {
    //                ModelState.AddModelError("", "Total subject hours must match weekly total.");
    //                return View(model);
    //            }

    //            model.TimeTable = GenerateTimeTable(model);
    //            return View(model);
    //        }

    //        return View(model);
    //    }

    //    private TimeTableModel GenerateTimeTable(TimeTableViewModel model)
    //    {
    //        var rnd = new Random();
    //        var pool = new List<string>();
    //        foreach (var subject in model.SubjectHours)
    //        {
    //            for (int i = 0; i < subject.Hours; i++)
    //                pool.Add(subject.Name);
    //        }

    //        var schedule = new List<List<string>>();
    //        for (int i = 0; i < model.SubjectsPerDay; i++)
    //        {
    //            var row = new List<string>();
    //            for (int j = 0; j < model.WorkingDays; j++)
    //            {
    //                if (pool.Count == 0) break;
    //                var idx = rnd.Next(pool.Count);
    //                row.Add(pool[idx]);
    //                pool.RemoveAt(idx);
    //            }
    //            schedule.Add(row);
    //        }

    //        return new TimeTableModel
    //        {
    //            Days = Enumerable.Range(1, model.WorkingDays.Value).Select(i => "Day " + i).ToList(),
    //            Schedule = schedule
    //        };
    //    }
    //}


