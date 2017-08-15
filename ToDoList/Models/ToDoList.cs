using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace ToDoList.Models
{
  public class Task
  {
    private int _id;
    private string _name;
    private DateTime _dueDate;
    private int _categoryId;

    public Task(string name, DateTime dueDate, int categoryId, int Id = 0)
    {
      _id = Id;
      _name = name;
      _dueDate = dueDate;
      _categoryId = categoryId;
    }
    public int GetId()
    {
      return _id;
    }
    public string GetName()
    {
      return _name;
    }
    public DateTime GetDueDate()
    {
      return _dueDate;
    }
    public int GetCatagoryId()
    {
      return _categoryId;
    }
    public override bool Equals(Object otherTask)
    {
     if (!(otherTask is Task))
      {
        return false;
      }
     else
      {
        Task newTask = (Task) otherTask;
        bool idEquality = (this.GetId() == newTask.GetId());
        bool nameEquality = (this.GetName() == newTask.GetName());
        bool dueDateEquality = (this.GetDueDate() == newTask.GetDueDate());
        bool categoryIdEquality = (this.GetCatagoryId() == newTask.GetCatagoryId());
        return (idEquality && nameEquality && dueDateEquality && categoryIdEquality);
      }
    }

    public static List<Task> GetAll()
    {
      List<Task> allTasks = new List<Task> {};

      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM tasks;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int taskId = rdr.GetInt32(0);
        string taskName = rdr.GetString(1);

        DateTime taskDueDate = (DateTime)rdr[2];

        int taskCatagoryId = rdr.GetInt32(3);
        Task newTask = new Task(taskName, taskDueDate, taskCatagoryId, taskId);
        allTasks.Add(newTask);
      }
      return allTasks;
    }

    public void Save()
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();

        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"INSERT INTO tasks (name, due_date, category_id) VALUES (@TaskName, @TaskDueDate, @TaskCatagoryId);";
        //for name
        MySqlParameter name = new MySqlParameter();
        name.ParameterName = "@TaskName";
        name.Value = this._name;
        cmd.Parameters.Add(name);
        //for dueDate
        MySqlParameter dueDate = new MySqlParameter();
        dueDate.ParameterName = "@TaskDueDate";
        dueDate.Value = this._dueDate;
        cmd.Parameters.Add(dueDate);
        //for categoryId
        MySqlParameter categoryId = new MySqlParameter();
        categoryId.ParameterName = "@TaskCatagoryId";
        categoryId.Value = this._categoryId;
        cmd.Parameters.Add(categoryId);

        cmd.ExecuteNonQuery();
        _id = (int) cmd.LastInsertedId;
    }

    public static void DeleteAll()
     {
       MySqlConnection conn = DB.Connection();
       conn.Open();
       var cmd = conn.CreateCommand() as MySqlCommand;
       cmd.CommandText = @"TRUNCATE tasks;";
       cmd.ExecuteNonQuery();
     }
     public static Task Find(int id)
      {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT * FROM tasks WHERE id = (@thisId);";

        MySqlParameter thisId = new MySqlParameter();
        thisId.ParameterName = "@thisId";
        thisId.Value = id;
        cmd.Parameters.Add(thisId);

        var rdr = cmd.ExecuteReader() as MySqlDataReader;

        int taskId = 0;
        string taskName = "";
        DateTime dueDate = DateTime.MinValue;
        int taskCatagoryId = 0;

        while (rdr.Read())
        {
          taskId = rdr.GetInt32(0);
          taskName = rdr.GetString(1);
          dueDate = (DateTime)rdr[2];
          taskCatagoryId = rdr.GetInt32(3);
        }
        Task foundTask = new Task(taskName, dueDate, taskCatagoryId, taskId);
        return foundTask;
      }
  }
}
