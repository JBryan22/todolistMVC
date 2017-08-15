using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToDoList.Models;
using System.Collections.Generic;
using System;

namespace ToDoList.Tests
{

    [TestClass]
    public class TaskTests : IDisposable
    {
      public void Dispose()
      {
        Task.DeleteAll();
        Category.DeleteAll();
      }
      public TaskTests()
      {
        DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=	8889;database=todo_test;";
      }

      [TestMethod]
      public void Equals_OverrideTrueForSameDescription_Task()
      {
        //Arrange
        DateTime due_date = DateTime.Parse("2017-10-15");
        Task firstTask = new Task("Mow the lawn", due_date, 1);
        Task secondTask = new Task("Mow the lawn", due_date, 1);
        //Act
        //Assert
        Assert.AreEqual(firstTask, secondTask);
      }

      [TestMethod]
      public void Save_SavesTaskToDatabase_TaskList()
      {
        //Arrange
        DateTime due_date = DateTime.Parse("2017-10-15");
        Task testTask = new Task("Mow the lawn", due_date, 1);
        testTask.Save();
        List<Task> expected = new List<Task>{testTask};
        //Act
        List<Task> actual = Task.GetAll();

        //Assert
        CollectionAssert.AreEqual(expected, actual);
      }

      [TestMethod]
      public void Save_DatabaseAssignsIdToObject_Id()
      {
        //Arrange
        DateTime due_date = DateTime.Parse("2017-10-15");

        Task testTask = new Task("Mow the lawn", due_date , 1);
        testTask.Save();
        int expected = testTask.GetId();
        //Act
        Task savedTask = Task.GetAll()[0];
        int actual = savedTask.GetId();

        //Assert
        Assert.AreEqual(expected, actual);
      }


      [TestMethod]
      public void Find_FindsTaskInDatabase_Task()
      {
        //Arrange
        DateTime due_date = DateTime.Parse("2017-10-15");

        Task expected = new Task("Mow the lawn", due_date, 1);
        expected.Save();

        //Act
        Task actual = Task.Find(expected.GetId());

        //Assert
        Assert.AreEqual(expected, actual);
      }
    }
}
