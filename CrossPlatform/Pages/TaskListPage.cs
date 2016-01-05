﻿using System;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;
using System.Linq;

namespace CrossPlatform
{
    public class TaskListPage : BasePage
    {
        readonly string AddTaskButton;
        readonly Func<AppQuery, AppQuery> FirstTask;

        public TaskListPage()
            : base("menu_add_task", "Tasky")
        {
            if (OnAndroid)
            {
                AddTaskButton = "menu_add_task";
                FirstTask = x => x.Id("lstTasks").Child(0);
            }

            if (OniOS)
            {
                AddTaskButton = "Add";
                FirstTask = x => x.Class("UITableViewWrapperView").Child(0);
            }
        }

        public void GoToAddTask()
        {
            app.Tap(AddTaskButton);
        }

        public void SelectTask(string name = null)
        {
            app.WaitFor(() => app.Query(FirstTask).Any());

            if (name == null)
                app.Tap(FirstTask);
            else
                app.Tap(name);
        }

        public TaskListPage VerifyTaskExists(string name)
        {
            app.WaitForElement(name);
            app.Screenshot("Task exists");

            return this;
        }

        public TaskListPage VerifyTaskDoesNotExist(string name)
        {
            app.WaitForNoElement(name);
            app.Screenshot("Task deleted");

            return this;
        }

        public TaskListPage VerifyTaskDone(string name = null, bool done = true)
        {
            // Method not applicable to iOS
            if (OnAndroid)
            {
                Func<AppQuery, AppQuery> checkMark;
                
                if (name == null)
                    checkMark = x => x.Id("lstTasks").Child().Child().Id("checkMark");
                else
                    checkMark = x => x.Marked(name).Parent().Sibling().Id("checkMark");
                
                if (done)
                    app.WaitForElement(checkMark);
                else
                    app.WaitForNoElement(checkMark);

                app.Screenshot("Task is done");
            }

            return this;
        }

        public TaskListPage DeleteTask(string name)
        {
            // Method not applicable to Android
            if (OniOS)
            {
                app.SwipeLeftOnElement(app.Query(e => e.Marked(name).Parent(0))[0]);
                app.Tap("Delete");
            }

            return this;
        }
    }
}
