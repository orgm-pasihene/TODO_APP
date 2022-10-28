using Microsoft.AspNetCore.Components;
using static TODO_UI.Pages.Tasks;

namespace TODO_UI.Services
{
    public class Services : ComponentBase
    {
        public IEnumerable<TasksClass> tasks = Array.Empty<TasksClass>();

        public string TaskIDFilter = "";
        public string TaskTitleFilter = "";

        public IEnumerable<TasksClass> tasksWithoutFilter = Array.Empty<TasksClass>();

        //Filter Function
        public void FilterFn()
        {
            tasks = tasksWithoutFilter.Where(
                c => c.TaskID.ToString().Contains(TaskIDFilter) &&
                c.TaskTitle.ToLower().Contains(TaskTitleFilter.ToLower()));
        }

        //Sort Funtion
        public void SortFn(string property, string asc_desc)
        {
            if (property == "TaskID")
            {
                if (asc_desc == "asc")
                {
                    tasks = tasksWithoutFilter.OrderBy(c => c.TaskID);
                }
                else
                {
                    tasks = tasksWithoutFilter.OrderByDescending(c => c.TaskID);
                }
            }
            else
            {
                if (asc_desc == "asc")
                {
                    tasks = tasksWithoutFilter.OrderBy(c => c.TaskTitle);
                }
                else
                {
                    tasks = tasksWithoutFilter.OrderByDescending(c => c.TaskTitle);
                }
            }
        }

        public string modalTitle;
        public int TaskID;
        public string TaskTitle;
        public string TaskDetails;
        public DateTime DateAdded;

        //Add Click
        public void addClick()
        {
            modalTitle = "Add Task";
            TaskID = 0;
            TaskTitle = "";
            TaskDetails = "";
            DateAdded = DateTime.Now;
        }

        //Edit Click
        public void editClick(TasksClass tsk)
        {
            modalTitle = "Edit Task";
            TaskID = tsk.TaskID;
            TaskTitle = tsk.TaskTitle;
            TaskDetails = tsk.TaskDetails;
            DateAdded = Convert.ToDateTime(tsk.DateAdded);
        }

    }

}
