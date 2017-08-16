using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace ToDoList.Models
{
  public class MyTask
  {
    private int _id;
    private string _name;
    private DateTime _dueDate;
    private int _categoryId;

    public MyTask(string name, DateTime dueDate, int categoryId, int Id = 0)
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
    public override bool Equals(Object otherMyTask)
    {
     if (!(otherMyTask is MyTask))
      {
        return false;
      }
     else
      {
        MyTask newMyTask = (MyTask) otherMyTask;
        bool idEquality = (this.GetId() == newMyTask.GetId());
        bool nameEquality = (this.GetName() == newMyTask.GetName());
        bool dueDateEquality = (this.GetDueDate() == newMyTask.GetDueDate());
        bool categoryIdEquality = (this.GetCatagoryId() == newMyTask.GetCatagoryId());
        return (idEquality && nameEquality && dueDateEquality && categoryIdEquality);
      }
    }

    public static List<MyTask> GetAll()
    {
      List<MyTask> allMyTasks = new List<MyTask> {};

      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM tasks;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int taskId = rdr.GetInt32(0);
        string taskName = "";
        if (! DBNull.Value.Equals(rdr[1]))
        {
          taskName = (string)rdr[1];
        }
        else
        {
          taskName = String.Empty;
        }
        DateTime taskDueDate = (DateTime)rdr[2];

        int taskCatagoryId = rdr.GetInt32(3);
        MyTask newMyTask = new MyTask(taskName, taskDueDate, taskCatagoryId, taskId);
        allMyTasks.Add(newMyTask);
      }
      return allMyTasks;
    }

    public void Save()
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();

        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"INSERT INTO tasks (name, due_date, category_id) VALUES (@MyTaskName, @MyTaskDueDate, @MyTaskCatagoryId);";
        //for name
        MySqlParameter name = new MySqlParameter();
        name.ParameterName = "@MyTaskName";
        name.Value = this._name;
        cmd.Parameters.Add(name);
        //for dueDate
        MySqlParameter dueDate = new MySqlParameter();
        dueDate.ParameterName = "@MyTaskDueDate";
        dueDate.Value = this._dueDate;
        cmd.Parameters.Add(dueDate);
        //for categoryId
        MySqlParameter categoryId = new MySqlParameter();
        categoryId.ParameterName = "@MyTaskCatagoryId";
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
    public static MyTask Find(int id)
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
      MyTask foundMyTask = new MyTask(taskName, dueDate, taskCatagoryId, taskId);
      return foundMyTask;
    }
    public static List<MyTask> FindTasksByCategory(int categoryId)
    {
      List<MyTask> taskList = new List<MyTask>();
      MySqlConnection mySqlConnection = DB.Connection();
      mySqlConnection.Open();
      MySqlCommand cmd = mySqlConnection.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM tasks WHERE category_id = @categoryId;";

      MySqlParameter idParameter = new MySqlParameter();
      idParameter.ParameterName = "categoryId";
      idParameter.Value = categoryId;
      cmd.Parameters.Add(idParameter);

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while (rdr.Read())
      {
        int taskId = rdr.GetInt32(0);
        string taskName = "";
        if (! DBNull.Value.Equals(rdr[1]))
        {
          taskName = (string)rdr[1];
        }

        DateTime taskDueDate = DateTime.MinValue;
        if (! DBNull.Value.Equals(rdr[2]))
        {
          taskDueDate = DateTime.Parse(rdr[2].ToString());
        }

        int taskCategoryId = rdr.GetInt32(3);
        MyTask newTask = new MyTask(taskName, taskDueDate, taskCategoryId, taskId);
        taskList.Add(newTask);
      }
      return taskList;
    }
  }
}
