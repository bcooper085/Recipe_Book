using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace RecipeBox
{
    public class Category
    {
        private int _id;
        private string _name;

        public Category(string Name, int Id = 0)
        {
            _id = Id;
            _name = Name;
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
                bool idEquality = this.GetId() == newCategory.GetId();
                bool nameEquality = this.GetName() == newCategory.GetName();
                return (idEquality && nameEquality);
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
        public static List<Category> GetAll()
        {
          List<Category> allCategories = new List<Category>{};

          SqlConnection conn = DB.Connection();
          conn.Open();

          SqlCommand cmd = new SqlCommand("SELECT * FROM categories;", conn);
          SqlDataReader rdr = cmd.ExecuteReader();

          while(rdr.Read())
          {
            int CategoryId = rdr.GetInt32(0);
            string CategoryName = rdr.GetString(1);
            Category newCategory = new Category(CategoryName, CategoryId);
            allCategories.Add(newCategory);
          }

          if (rdr != null)
          {
            rdr.Close();
          }
          if (conn != null)
          {
            conn.Close();
          }

          return allCategories;
        }

        public void Save()
        {
          SqlConnection conn = DB.Connection();
          conn.Open();

          SqlCommand cmd = new SqlCommand("INSERT INTO categories (name) OUTPUT INSERTED.id VALUES (@CategoryName);", conn);

          SqlParameter nameParameter = new SqlParameter();
          nameParameter.ParameterName = "@CategoryName";
          nameParameter.Value = this.GetName();
          cmd.Parameters.Add(nameParameter);
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




        public static void DeleteAll()
        {
          SqlConnection conn = DB.Connection();
          conn.Open();
          SqlCommand cmd = new SqlCommand("DELETE FROM categories;", conn);
          cmd.ExecuteNonQuery();
          conn.Close();
        }

    }
}
