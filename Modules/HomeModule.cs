using System;
using System.Collections.Generic;
using Nancy;
using Nancy.ViewEngines.Razor;

namespace RecipeBox
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = _ => {
                List<Category> AllCategories = Category.GetAll();
                return View["index.cshtml", AllCategories];
            };

            Get["/recipe/new"] = _ => {
                return View["recipe_form.cshtml"];
            };

            Post["/recipe/new"] = _ => {
                Category newCategory = new Category(Request.Form["category-name"]);
                newCategory.Save();
                Recipe newRecipe = new Recipe(Request.Form["recipe-name"], Request.Form["recipe-ingredients"], Request.Form["recipe-instructions"]);
                newRecipe.Save();
                newCategory.AddRecipe(newRecipe);
                return View["recipe_form.cshtml"];
            };

            Get["/category/{id}"] = parameters => {
                Dictionary<string, object> model = new Dictionary<string, object>();
                Category SelectedCategory = Category.Find(parameters.id);
                List<Recipe> RecipeList = SelectedCategory.GetRecipes();
                model.Add("category", SelectedCategory);
                model.Add("recipes", RecipeList);

                return View["category_recipes.cshtml", model];
            };

            Get["/recipe/{id}"] = parameters => {
                Dictionary<string, object> model = new Dictionary<string, object>();
                Recipe SelectedRecipe = Recipe.Find(parameters.id);
                List<Category> RecipeCategory = SelectedRecipe.GetCategories();
                model.Add("recipe", SelectedRecipe);
                return View["index.cshtml"];

            };

        }


    }
}
