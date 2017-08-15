using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace ToDoList.Models
{
  public class Category
  {
    private int _id;
    private string _name;

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
        int CategoryId = rdr.GetInt32(0);
        string CategoryName = rdr.GetString(1);
        Category newCategory = new Category(CategoryName, CategoryId);
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
      int CategoryId = 0;
      string CategoryName = "";
      while(rdr.Read())
      {
        CategoryId = rdr.GetInt32(0);
        CategoryName = rdr.GetString(1);
      }
      Category newCategory = new Category(CategoryName, CategoryId);
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
    public List<Task> GetTasks()
    {
      List<Task> allCategoryTasks = new List<Task>();

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
        Task newTask = new Task(taskName, taskDueDate, taskCatagoryId, taskId);
        allCategoryTasks.Add(newTask);
      }
      return allCategoryTasks;
    }
  }
}
