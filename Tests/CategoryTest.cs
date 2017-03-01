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

        [Fact]
        public void Test_Save_AssignsIdToCategoryObject()
        {
            //Arrange
            Category testCategory = new Category("Asian");
            testCategory.Save();

            //Act
            Category savedCategory = Category.GetAll()[0];

            int result = savedCategory.GetId();
            int testId = testCategory.GetId();

            //Assert
            Assert.Equal(testId, result);
        }

        [Fact]
        public void Test_Find_FindsCategoryInDatabase()
        {
            //Arrange
            Category testCategory = new Category("Asian");
            testCategory.Save();

            //Act
            Category foundCategory = Category.Find(testCategory.GetId());

            //Assert
            Assert.Equal(testCategory, foundCategory);
        }


        [Fact]
        public void Test_GetRecipes_RetrievesAllRecipesInCategory()
        {
            //Arrange
            Category testCategory = new Category("Asian");
            testCategory.Save();
            Recipe firstRecipe = new Recipe("Spicy Yaki Soba", "Spicy, Soba", "Pour Spicy into Soba");
            firstRecipe.Save();
            Recipe secondRecipe = new Recipe("Spicy Yaki Soba", "Spicy, Soba", "Pour Spicy into Soba");
            secondRecipe.Save();

            //Act
            testCategory.AddRecipe(firstRecipe);
            testCategory.AddRecipe(secondRecipe);
            List<Recipe> testRecipeList = new List<Recipe> {firstRecipe, secondRecipe};
            List<Recipe> resultRecipeList = testCategory.GetRecipes();

            //Assert
            Assert.Equal(testRecipeList, resultRecipeList);
        }


        [Fact]
        public void Test_Delete_DeletesCategoryAssociationsFromDatabase()
        {
            //Arrange
            Recipe testRecipe = new Recipe("Spicy Yaki Soba", "Spicy, Soba", "Pour Spicy into Soba");
            testRecipe.Save();

            string testName = "Asian";
            Category testCategory = new Category(testName);
            testCategory.Save();

            //Act
            testCategory.AddRecipe(testRecipe);
            testCategory.Delete();

            List<Category> resultRecipeCategories = testRecipe.GetCategories();
            List<Category> testRecipeCategories = new List<Category>{};

            //Assert
            Assert.Equal(testRecipeCategories, resultRecipeCategories);
        }





        public void Dispose()
        {
            Category.DeleteAll();
            Recipe.DeleteAll();
        }
    }
}
