using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Security.Claims;
using TO_DO.Dtos;
using TO_DO.Models;
using Task = TO_DO.Models.Tasks;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ToDo_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly TododbContext _context;

        public TasksController(TododbContext _context)
        {
            this._context = _context;
        }

        [HttpGet]
        public IActionResult GetTasks()
        {
            try
            {
                List<Tasks> ListTasks = _context.Tasks.ToList();

                if (ListTasks != null)
                {
                    return Ok(ListTasks);
                }
                return Ok("there are no tasks");
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Creating Tasks
        //[Authorize]
        [HttpPost("AddTasks")]

        public async Task<IActionResult> createTask([FromBody] TasksDto tasks)
        {

            try
            {
                if (tasks == null)
                {
                    return BadRequest("Task is invalid");
                }
               
                var newTask = new Task
                {
                    TaskTitle = tasks.TaskTitle,
                    TaskDetails = tasks.TaskDetails,
                    DateAdded = DateTime.Now
                };

                _context.Tasks.Add(newTask);
                await _context.SaveChangesAsync();

                return Ok("New Task Created");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        //Updating Tasks
        [HttpPut("updateTasks")]
        public async Task<IActionResult> updateTask([FromBody] TasksDto tasks)
        {
            try
            {
#pragma warning disable CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
                if (tasks == null || tasks.TaskID == null || tasks.TaskID <= 0)
                {
                    return BadRequest("Task is invalid");
                }
#pragma warning restore CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'

                var dbTask = _context.Tasks.Where(t => t.TaskID == tasks.TaskID).FirstOrDefault();
                if (dbTask == null)
                {
                    return NotFound("Task does not exist!");
                }

                // Update fields
                dbTask.TaskTitle = tasks.TaskTitle;
                dbTask.TaskDetails = tasks.TaskDetails;

                _context.Entry(dbTask).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok("Task Updated");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        //Get Task By ID
        [HttpGet("getTaskbyID")]
        public async Task<IActionResult> getTaskbyID(int id)
        {
            try
            {
                if(id == null || id <= 0)
                {
                    return BadRequest("Task does not exists");
                }

                var tasks = _context.Tasks.Select(t => new
                {
                    t.TaskID,
                    t.TaskTitle,
                    t.TaskDetails,
                    t.DateAdded
                }).Where(t => t.TaskID == id).FirstOrDefault();

                return Ok(tasks);
            }

            catch (Exception)
            {
                return BadRequest("No task available");
            }
        }

        [HttpDelete("deletetask/{id}")]
        public async Task<IActionResult> deleteTaskbyID(int id)
        {
            try
            {
                if (id == null || id <= 0)
                {
                    return BadRequest("Task does not exists");
                }

                var tasks = _context.Tasks.Where(t => t.TaskID == id).FirstOrDefault();

                if (tasks == null)
                {
                    return NotFound("ID does not belong to any task");
                }

                _context.Tasks.Remove(tasks);

                await _context.SaveChangesAsync();

                return Ok("Task Deleted");
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
