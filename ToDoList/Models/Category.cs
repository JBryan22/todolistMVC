using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace ToDoList.Models
{
  public class Category
  {
    private int _id;
    private string _name;

    private static Dictionary<int, List<MyTask>> _categoryDictionary = new Dictionary<int, List<MyTask>>();

    public Category(string name, int id = 0)
    {
      _id = id;
      _name = name;
    }
    public int GetId()
    {
      return _id;
    }
    public string GetName()
    {
      return _name;
    }
    public override bool Equals(System.Object otherCategory)
    {
      if (!(otherCategory is Category))
      {
        return false;
      }
      else
      {
        Category newCategory = (Category) otherCategory;
        return this.GetId().Equals(newCategory.GetId());
      }
    }
    public override int GetHashCode()
    {
      return this.GetId().GetHashCode();
    }

    public void Save()
    {
      MySqlConnection mySqlConnection = DB.Connection();
      mySqlConnection.Open();

      var cmd = mySqlConnection.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO categories (name) VALUES (@name);";

      MySqlParameter name = new MySqlParameter();
      name.ParameterName = "@name";
      name.Value = this._name;
      cmd.Parameters.Add(name);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
    }

    public static List<Category> GetAll()
    {
      List<Category> allCategories = new List<Category> {};

      MySqlConnection mySqlConnection = DB.Connection();
      mySqlConnection.Open();

      var cmd = mySqlConnection.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM categories;";

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int categoryId = rdr.GetInt32(0);
        string categoryName = "";
        if (! DBNull.Value.Equals(rdr[1]))
        {
          categoryName = (string)rdr[1];
        }
        else
        {
          categoryName = "";
        }
        Category newCategory = new Category(categoryName, categoryId);
        allCategories.Add(newCategory);
      }
      return allCategories;
    }
    public static Category Find(int id)
    {
      MySqlConnection mySqlConnection = DB.Connection();
      mySqlConnection.Open();

      var cmd = mySqlConnection.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM categories WHERE id = @searchId;";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int categoryId = 0;
      string categoryName = "";
      while(rdr.Read())
      {
        categoryId = rdr.GetInt32(0);
        categoryName = rdr.GetString(1);
      }
      Category newCategory = new Category(categoryName, categoryId);
      return newCategory;
    }
    public static void DeleteAll()
    {
      MySqlConnection mySqlConnection = DB.Connection();
      mySqlConnection.Open();
      var cmd = mySqlConnection.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"TRUNCATE categories;";
      cmd.ExecuteNonQuery();
    }
    public List<MyTask> GetMyTasks()
    {
      List<MyTask> allCategoryMyTasks = new List<MyTask>();

      MySqlConnection mySqlConnection = DB.Connection();
      mySqlConnection.Open();

      var cmd = mySqlConnection.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM tasks WHERE category_id = @category_id;";

      MySqlParameter categoryId = new MySqlParameter();
      categoryId.ParameterName = "@category_id";
      categoryId.Value = this._id;
      cmd.Parameters.Add(categoryId);


      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int taskId = rdr.GetInt32(0);
        string taskName = rdr.GetString(1);
        DateTime taskDueDate = DateTime.Parse(rdr.GetString(2));
        int taskCatagoryId = rdr.GetInt32(3);
        MyTask newMyTask = new MyTask(taskName, taskDueDate, taskCatagoryId, taskId);
        allCategoryMyTasks.Add(newMyTask);
      }
      return allCategoryMyTasks;
    }
    public static Dictionary<int, List<MyTask>> GetCategoryDictionary()
    {

      List<Category> categories = GetAll();
      foreach(Category category in categories)
      {
        int categoryId = category.GetId();
        List<MyTask> tasksInThisCategory = GetMyTasksInCategory(categoryId);
        _categoryDictionary.Add(categoryId, tasksInThisCategory);
      }
      return _categoryDictionary;
    }
    public static List<MyTask> GetMyTasksInCategory(int categoryId)
    {
      List<MyTask> tasksInCategory = new List<MyTask>();
      MySqlConnection mySqlConnection = DB.Connection();
      mySqlConnection.Open();

      MySqlCommand cmd = mySqlConnection.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM tasks WHERE category_id = @category_id;";

      MySqlParameter categoryIdParameter = new MySqlParameter();
      categoryIdParameter.ParameterName = "@category_id";
      categoryIdParameter.Value = categoryId;
      cmd.Parameters.Add(categoryIdParameter);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int taskId = rdr.GetInt32(0);
        string taskName = "";
        if(! DBNull.Value.Equals(rdr[1]))
        {
          taskName = (string)rdr[1];
        }
        DateTime taskDueDate = DateTime.Parse(rdr[2].ToString());
        int taskCategoryId = rdr.GetInt32(3);
        MyTask newTask = new MyTask(taskName, taskDueDate, taskCategoryId, taskId);
        tasksInCategory.Add(newTask);
      }
      return tasksInCategory;
    }
  }
}
