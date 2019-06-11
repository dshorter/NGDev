﻿using System;
using Microsoft.Win32.TaskScheduler;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using DaysOfWeel = Microsoft.Win32.TaskScheduler;

namespace TaskScheduler
{
    public class TaskWrapper
    {
        //private ITask task;
        private string name;
        private static Guid CLSID_Ctask = new Guid("{148BD520-A2AB-11CE-B11F-00AA00530503}");
        private static Guid IID_Task = new Guid("{148BD524-A2AB-11CE-B11F-00AA00530503}");

        protected TaskWrapper()
        {
        }

        public TaskWrapper(/*ITask t,*/string n)
        {
            //task = t;
            name = n;
        }

        public static Task Find(string TaskName)
        {
            TaskService taskService = new TaskService();
            try
            {
                return taskService.GetTask(TaskName);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static bool Exists(string TaskName)
        {
            TaskService taskService = new TaskService();
            try
            {
                Task task = taskService.GetTask(TaskName);
                return (task != null);
            }
            catch (Exception)
            {
                return false;
            }
        }

        //public static Task Get(string TaskName)
        //{
        //    /*ITask t = Find(TaskName);
        //    if (t == null) return null;*/
        //    return new Task(/*t,*/ TaskName,);
        //}

        public static bool Delete(string TaskName)
        {
            using (TaskService taskService = new TaskService())
            {
                try
                {
                    taskService.RootFolder.DeleteTask(TaskName);
                    return true;
                }
                catch (FileNotFoundException)
                {
                    return false;
                }

            }
        }

        private static Task AddTask(
            string taskName,
            string appPath,
            string parameters,
            string workingFolder,
            Trigger[] triggers)
        {
            Delete(taskName);
            using (TaskService taskService = new TaskService())
            {
                try
                {
                    TaskDefinition taskDef = taskService.NewTask();
                    taskDef.RegistrationInfo.Description = taskName;
                    taskDef.Actions.Add(new ExecAction(appPath, parameters, workingFolder));
                    foreach (var trigger in triggers)
                        taskDef.Triggers.Add(trigger);
                    return taskService.RootFolder.RegisterTaskDefinition(taskName, taskDef);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
        public static Task Create(
            string TaskName,
            string Application,
            string Parameters,
            string WorkingFolder,
            DateTime start,
            double HourInterval,
            int duration = 0)
        {

            TimeTrigger t = new TimeTrigger();
            t.StartBoundary = start;
            t.Repetition.Interval = TimeSpan.FromHours(HourInterval);
            t.Repetition.Duration = duration == 0 ? TimeSpan.Zero : TimeSpan.FromDays(duration);
            return AddTask(TaskName, Application, Parameters, WorkingFolder, new Trigger[] { t });
        }

        public static Task CreateDaily(
            string TaskName,
            string Application,
            string Parameters,
            string WorkingFolder,
            TimeSpan beginTime,
            double HourInterval,
            int duration = 0)
        {
            if ((HourInterval < 0) || (HourInterval > 12)) return null;
            DailyTrigger t = new DailyTrigger();
            t.StartBoundary = DateTime.Today + beginTime;
            t.DaysInterval = 1;
            t.Repetition.Interval = TimeSpan.FromHours(HourInterval);
            t.Repetition.Duration = duration == 0 ? TimeSpan.Zero : TimeSpan.FromDays(duration);
            return AddTask(TaskName, Application, Parameters, WorkingFolder, new Trigger[] { t });
        }

        /// <summary>
        /// Gets day of week to create weekly task running at specfied day.
        /// </summary>
        /// <param name="dayOfWeekNumber">Number of week day (1 = Mon, 2 = Tue, 3 = Wed, 4 = Thu, 5 = Fri, 6 = Sat, 7 = Sun)</param>
        /// <returns>Returns int value</returns>
        private static int GetDayOfWeekForFirstMonday
        (
            int dayOfWeekNumber
        )
        {
            int firstDaySundayDayOfWeekNumber = dayOfWeekNumber;
            if ((firstDaySundayDayOfWeekNumber > 0) && ((firstDaySundayDayOfWeekNumber < 7)))
            {
                firstDaySundayDayOfWeekNumber = firstDaySundayDayOfWeekNumber + 1;
            }
            else
            {
                firstDaySundayDayOfWeekNumber = 1;
            }

            return (firstDaySundayDayOfWeekNumber);
        }


        public static Task CreateWeekly(
            string TaskName,
            string Application,
            string Parameters,
            string WorkingFolder,
            DateTime start,
            int days,
            TimeSpan beginTime,
            double hourInterval,
            int duration = 0)
        {
            List<Trigger> triggers = new List<Trigger>();
            WeeklyTrigger t = new WeeklyTrigger();
            t.StartBoundary = DateTime.Today + beginTime;
            t.DaysOfWeek = (DaysOfTheWeek)days;
            t.WeeksInterval = 1;
            t.Repetition.Interval = TimeSpan.FromHours(hourInterval);
            t.Repetition.Duration = duration == 0 ? TimeSpan.Zero : TimeSpan.FromDays(duration);
            return AddTask(TaskName, Application, Parameters, WorkingFolder, new Trigger[] { t });
        }

        public static Task CreateWeekly(
            string TaskName,
            string Application,
            string Parameters,
            string WorkingFolder,
            DateTime start,
            int beginDay,
            TimeSpan beginTime,
            double hourInterval,
            bool useMondayAsFirstDayOfWeek,
            int duration = 0)
        {
            Task task = useMondayAsFirstDayOfWeek == true ? 
                        CreateWeekly(TaskName, Application, Parameters, WorkingFolder, start, GetDayOfWeekForFirstMonday(beginDay), beginTime, hourInterval, duration) :
                        CreateWeekly(TaskName, Application, Parameters, WorkingFolder, start, beginDay, beginTime, hourInterval, duration);
            return (task);
        }


        public static bool IsRunning(string taskName)
        {
            using (TaskService taskService = new TaskService())
            {
                Task task = taskService.FindTask(taskName);
                if (task != null)
                    return task.State == TaskState.Running;
                return false;
            }
        }

        public static void Run(string taskName)
        {
            using (TaskService taskService = new TaskService())
            {
                Task task = taskService.FindTask(taskName);
                if (task != null)
                {
                    DateTime begin = task.LastRunTime;
                    if (task.State == TaskState.Running)
                        return;
                    task.Run();
                    DateTime end;
                    do
                    {
                        Thread.Sleep(100);
                        end = task.LastRunTime;
                    }
                    while (begin >= end);
                }
            }
        }

        public static int GetLastError(string taskName)
        {
            using (TaskService taskService = new TaskService())
            {
                Task task = taskService.FindTask(taskName);
                if (task != null)
                {
                    return task.LastTaskResult;
                }
            }
            return -1;
        }

        public static DateTime GetLastRunTime(string taskName)
        {
            using (TaskService taskService = new TaskService())
            {
                Task task = taskService.FindTask(taskName);
                if (task != null)
                    return task.LastRunTime;
                return DateTime.MinValue;
            }
        }


        public static void SetAccount(string taskName, string username, string password)
        {
            using (TaskService taskService = new TaskService())
            {
                Task task = taskService.FindTask(taskName);
                if (task != null)
                {
                    taskService.RootFolder.RegisterTaskDefinition(taskName, task.Definition, TaskCreation.Update, username, password);
                }
            }
        }

        //protected static DateTime GetLastRunTime(ITask task)
        //{
        //    _SYSTEMTIME st;
        //    task.GetMostRecentRunTime(out st);
        //    if (st.wYear == 0 && st.wMonth == 0 && st.wDay == 0) return DateTime.MinValue;
        //    return new DateTime(st.wYear, st.wMonth, st.wDay, st.wHour, st.wMinute, st.wSecond);
        //}

        //public DateTime GetLastRunTime()
        //{
        //    ITask task = Find(name);
        //    try
        //    {
        //        return GetLastRunTime(task);
        //    }
        //    finally
        //    {
        //        int count;
        //        if (task != null)
        //            count = System.Runtime.InteropServices.Marshal.ReleaseComObject(task);
        //    }
        //}

        public string Name
        {
            get
            {
                return name;
            }
        }
    }
}
