using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace RecipeBox
{
    public class CategoryTest : IDisposable
    {
        public CategoryTest()
        {
            DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=recipe_box_test;Integrated Security=SSPI;";
        }

        [Fact]
        public void Test_CategoriesEmptyAtFirst()
        {
            //Arrange, Act
            int result = Category.GetAll().Count;

            //Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void Test_Equal_ReturnsTrueForSameName()
        {
          //Arrange, Act
         Category firstCategory = new Category("Asian");
         Category secondCategory = new Category("Asian");

          //Assert
          Assert.Equal(firstCategory, secondCategory);
        }

        [Fact]
        public void Test_Save_SavesCategoryToDatabase()
        {
          //Arrange
          Category testCategory = new Category("Asian");
          testCategory.Save();

          //Act
          List<Category> result = Category.GetAll();
          List<Category> testList = new List<Category>{testCategory};

          //Assert
          Assert.Equal(testList, result);
        }




        public void Dispose()
        {
            Category.DeleteAll();
        }
    }
}
