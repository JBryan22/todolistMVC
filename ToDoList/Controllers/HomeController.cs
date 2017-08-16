
using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;
using System;
using System.Collections.Generic;

namespace ToDoList.Controllers
{
  public class HomeController : Controller
  {
    [HttpGet("/")]
    public ActionResult Index ()
    {
      return View();
    }

    [HttpPost("/addMyTask")]
    public ActionResult AddMyTask()
    {
      string taskName = Request.Form["taskName"];
      DateTime taskDueDate = DateTime.Parse( Request.Form["taskDate"]);
      string categoryName = Request.Form["category"];

      Category newCategory = new Category(categoryName);
      newCategory.Save();

      int categoryId = newCategory.GetId();
      MyTask newMyTask = new MyTask(taskName, taskDueDate,categoryId);
      newMyTask.Save();

      // List<MyTask> allMyTasks = MyTask.GetAll();
      // return View(allMyTasks);
      return RedirectToAction("GetAll", "Home");
    }

    [HttpGet("/getAllMyTask")]
    public ActionResult GetAll()
    {
      List<MyTask> allMyTasks = MyTask.GetAll();
      return View(allMyTasks);
    }
    [HttpPost("/showAll")]
    public ActionResult ShowAll()
    {
      return RedirectToAction("GetAll", "Home");
    }

    [HttpPost("/clearAll")]
    public ActionResult ClearAll()
    {
      MyTask.DeleteAll();
      return View();
    }

    [HttpGet("/showCategory")]
    public ActionResult ShowCategoryList()
    {
      List<Category> categoryList = Category.GetAll();
      return View(categoryList);
    }

    [HttpPost("/showTasksInCategory")]
    public ActionResult ShowTasksInCategory(int categoryId)
    {
      Console.WriteLine("CATEGORY ID" + categoryId);
      List<MyTask> taskList = MyTask.FindTasksByCategory(categoryId);
      return View(taskList);
    }
  }
}
