using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace RecipeBox
{
  public class Recipe
  {
    private int _id;
    private string _name;
    private string _ingredients;
    private string _instructions;

    public Recipe(string Name, string Ingredients, string Instructions, int Id = 0)
    {
      _id = Id;
      _name = Name;
      _ingredients = Ingredients;
      _instructions = Instructions;
    }

    public override bool Equals(System.Object otherRecipe)
    {
      if (!(otherRecipe is Recipe))
      {
        return false;
      }
      else
      {
        Recipe newRecipe = (Recipe) otherRecipe;
        bool idEquality = this.GetId() == newRecipe.GetId();
        bool nameEquality = this.GetName() == newRecipe.GetName();
        bool ingredientsEquality = this.GetIngredients() == newRecipe.GetIngredients();
        bool instructionsEquality = this.GetInstructions() == newRecipe.GetInstructions();
        return (idEquality && nameEquality && ingredientsEquality && instructionsEquality);
      }
    }
    public int GetId()
    {
      return _id;
    }
    public string GetName()
    {
      return _name;
    }
    public string GetIngredients()
    {
      return _ingredients;
    }
    public string GetInstructions()
    {
      return _instructions;
    }

    

    public static List<Recipe> GetAll()
    {
      List<Recipe> allRecipes = new List<Recipe>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM recipes;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int RecipeId = rdr.GetInt32(0);
        string RecipeName = rdr.GetString(1);
        string RecipeIngredients = rdr.GetString(2);
        string RecipeInstructions = rdr.GetString(3);
        Recipe newRecipe = new Recipe(RecipeName, RecipeIngredients, RecipeInstructions, RecipeId);
        allRecipes.Add(newRecipe);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return allRecipes;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO recipes (name, ingredients, instructions) OUTPUT INSERTED.id VALUES (@RecipeName, @RecipeIngredients, @RecipeInstructions);", conn);

      cmd.Parameters.Add(new SqlParameter("@RecipeName", this.GetName()));
      cmd.Parameters.Add(new SqlParameter("@RecipeIngredients", this.GetIngredients()));
      cmd.Parameters.Add(new SqlParameter("@RecipeInstructions", this.GetInstructions()));


      // SqlParameter nameParameter = new SqlParameter();
      // nameParameter.ParameterName = "@RecipeName";
      // nameParameter.Value = this.GetName();
      // cmd.Parameters.Add(nameParameter);
      //
      // SqlParameter ingredientsParameter = new SqlParameter();
      // ingredientsParameter.ParameterName = "@RecipeIngredients";
      // ingredientsParameter.Value = this.GetIngredients();
      // cmd.Parameters.Add(ingredientsParameter);
      //
      // SqlParameter instructionsParameter = new SqlParameter();
      // instructionsParameter.ParameterName = "@RecipeInstructions";
      // instructionsParameter.Value = this.GetInstructions();
      // cmd.Parameters.Add(instructionsParameter);

      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
    }

    public static Recipe Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM recipes WHERE id = @RecipeId;", conn);

      cmd.Parameters.Add(new SqlParameter("@RecipeId", id.ToString()));

      SqlDataReader rdr = cmd.ExecuteReader();

      int foundRecipeId = 0;
      string foundRecipeName = null;
      string foundRecipeIngredients = null;
      string foundRecipeInstructions = null;

      while(rdr.Read())
      {
        foundRecipeId = rdr.GetInt32(0);
        foundRecipeName = rdr.GetString(1);
        foundRecipeIngredients = rdr.GetString(2);
        foundRecipeInstructions = rdr.GetString(3);
      }
      Recipe foundRecipe = new Recipe(foundRecipeName, foundRecipeIngredients, foundRecipeInstructions, foundRecipeId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundRecipe;
    }

    public void AddCategory(Category newCategory)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO categories_recipes (recipe_id, category_id) VALUES (@RecipeId, @CategoryId);", conn);
      cmd.Parameters.Add(new SqlParameter("@RecipeId", this.GetId()));
      cmd.Parameters.Add(new SqlParameter("@CategoryId", newCategory.GetId()));

      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }

    public List<Category> GetCategories()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT categories.* FROM recipes JOIN categories_recipes ON (recipes.id = categories_recipes.recipe_id) JOIN categories ON (categories_recipes.category_id = categories.id) WHERE recipes.id = @RecipeId;", conn);

      cmd.Parameters.Add(new SqlParameter("@RecipeId", this.GetId().ToString()));

      SqlDataReader rdr = cmd.ExecuteReader();

      List<Category> categories = new List<Category>{};

      while(rdr.Read())
      {
        int categoryId = rdr.GetInt32(0);
        string categoryName = rdr.GetString(1);
        Category newCategory = new Category(categoryName, categoryId);
        categories.Add(newCategory);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return categories;
    }


    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM recipes WHERE id = @RecipeId; DELETE FROM categories_recipes WHERE recipe_id = @RecipeId;", conn);

      cmd.Parameters.Add(new SqlParameter("@RecipeId", this.GetId()));

      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM recipes;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }
  }
}
