using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToDoList.Models;
using System.Collections.Generic;
using System;

namespace ToDoList.Tests
{
    [TestClass]
    public class MyTaskTests : IDisposable
    {
      public void Dispose()
      {
        MyTask.DeleteAll();
        Category.DeleteAll();
      }
      public MyTaskTests()
      {
        DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=	8889;database=todo_test;";
      }

      [TestMethod]
      public void Equals_OverrideTrueForSameDescription_MyTask()
      {
        //Arrange
        DateTime due_date = DateTime.Parse("2017-10-15");
        MyTask firstMyTask = new MyTask("Mow the lawn", due_date, 1);
        MyTask secondMyTask = new MyTask("Mow the lawn", due_date, 1);
        //Act
        //Assert
        Assert.AreEqual(firstMyTask, secondMyTask);
      }

      [TestMethod]
      public void Save_SavesMyTaskToDatabase_MyTaskList()
      {
        //Arrange
        DateTime due_date = DateTime.Parse("2017-10-15");
        MyTask testMyTask = new MyTask("Mow the lawn", due_date, 1);
        testMyTask.Save();
        List<MyTask> expected = new List<MyTask>{testMyTask};
        //Act
        List<MyTask> actual = MyTask.GetAll();

        //Assert
        CollectionAssert.AreEqual(expected, actual);
      }

      [TestMethod]
      public void Save_DatabaseAssignsIdToObject_Id()
      {
        //Arrange
        DateTime due_date = DateTime.Parse("2017-10-15");

        MyTask testMyTask = new MyTask("Mow the lawn", due_date , 1);
        testMyTask.Save();
        int expected = testMyTask.GetId();
        //Act
        MyTask savedMyTask = MyTask.GetAll()[0];
        int actual = savedMyTask.GetId();

        //Assert
        Assert.AreEqual(expected, actual);
      }

      [TestMethod]
      public void Find_FindsMyTaskInDatabase_MyTask()
      {
        //Arrange
        DateTime due_date = DateTime.Parse("2017-10-15");

        MyTask expected = new MyTask("Mow the lawn", due_date, 1);
        expected.Save();

        //Act
        MyTask actual = MyTask.Find(expected.GetId());

        //Assert
        Assert.AreEqual(expected, actual);
      }
    }
}
