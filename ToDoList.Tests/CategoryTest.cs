using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToDoList.Models;
using System.Collections.Generic;
using System;

namespace ToDoList.Tests
{
  [TestClass]
  public class CategoryTest
  {
    [TestClass]
    public class CategoryTests : IDisposable
    {
      public CategoryTests()
      {
        DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=	8889;database=todo_test;";
      }

      [TestMethod]
      public void GetAll_CategoriesEmptyAtFirst_0()
      {
        //Arrange, Act
        int result = Category.GetAll().Count;

        //Assert
        Assert.AreEqual(0, result);
      }

      [TestMethod]
      public void Equals_ReturnsTrueForSameName_Category()
      {
        //Arrange, Act
        Category firstCategory = new Category("Household chores");
        Category secondCategory = new Category("Household chores");

        //Assert
        Assert.AreEqual(firstCategory, secondCategory);
      }

      [TestMethod]
      public void Save_SavesCategoryToDatabase_CategoryList()
      {
        //Arrange
        Category testCategory = new Category("Household chores");
        testCategory.Save();
        List<Category> expected = new List<Category>{testCategory};
        //Act
        List<Category> actual = Category.GetAll();
        //Assert
        CollectionAssert.AreEqual(expected, actual);
      }

      [TestMethod]
      public void Save_DatabaseAssignsIdToCategory_Id()
      {
        //Arrange
        Category testCategory = new Category("Household chores");
        testCategory.Save();
        int expected = testCategory.GetId();

        //Act
        Category savedCategory = Category.GetAll()[0];
        int actual = savedCategory.GetId();

        //Assert
        Assert.AreEqual(expected, actual);
      }

      [TestMethod]
      public void Find_FindsCategoryInDatabase_Category()
      {
        //Arrange
        Category expected = new Category("Household chores");
        expected.Save();

        //Act
        Category actual = Category.Find(expected.GetId());

        //Assert
        Assert.AreEqual(expected, actual);
      }

      [TestMethod]
      public void GetMyTasksInCategory_GetsTasksInCategory_LittOfMyTask()
      {
        //Arrange
        DateTime taskDueDate = DateTime.Parse("2017-10-15");
        MyTask task1 = new MyTask("loundary", taskDueDate, 1);
        task1.Save();
        MyTask task2 = new MyTask("loundary", taskDueDate, 2);
        task2.Save();
         List<MyTask> expected = new List<MyTask>(){task1
         };
        //Act
        List<MyTask> actual = Category.GetMyTasksInCategory(1);
        //Assert
        CollectionAssert.AreEqual(expected, actual);
      }

      [TestMethod]
      public void GetCategoryDictionary_GetsDictionaryOfIntAndTaskLit_Dictionary()
      {
        //Arrange
        Category category = new Category("laundary");
        category.Save();

        DateTime taskDueDate = DateTime.Parse("2017-10-15");
        MyTask task1 = new MyTask("loundary", taskDueDate, 1);
        task1.Save();
        List<MyTask> taskList = new List<MyTask>(){task1
        };
        Dictionary<int, List<MyTask>> expected = new Dictionary<int, List<MyTask>>();
        expected.Add(1, taskList);
        //Act
        Dictionary<int, List<MyTask>> actual = Category.GetCategoryDictionary();


        //Assert
        for (int i = 1; i<=expected.Count; i++)
        {
          List<MyTask> taskListExpected = expected[i];
          List<MyTask> taskListActual = actual[i];
          for (int j = 0; j < taskListExpected.Count; j++)
          {
            Assert.AreEqual(taskListExpected[j], taskListActual[j]);
          }
          // CollectionAssert.AreEqual(expected[i].GetId(), actual[i].GetId());
        }

      }

      public void Dispose()
      {
        MyTask.DeleteAll();
        Category.DeleteAll();
      }
    }
  }
}
