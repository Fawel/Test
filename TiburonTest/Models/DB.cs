using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data.Sql;

namespace TiburonTest.Models
{
    public class DB
    {
        string connectionstring = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\TiburonTestDB.mdf;Integrated Security=True";
        SqlConnection dbconnect;
        public List<SmartphoneOwner> GetBrandUsers(string brand)
        {
            List<SmartphoneOwner> result = new List<SmartphoneOwner>();
            using (dbconnect = new SqlConnection(connectionstring))
            {      
                SqlCommand dbCommand = new SqlCommand(@"select Count, Genders.Gender, (Select Sum(Count) from SmartphoneOwnres inner join SmartphoneBrands on SmartphoneOwnres.BrandName = SmartphoneBrands.Id where Brand = '"+brand+@"') 
                                                        from SmartphoneOwnres inner join SmartphoneBrands on SmartphoneOwnres.BrandName = SmartphoneBrands.Id inner join Genders on SmartphoneOwnres.Gender = Genders.Id where Brand = '" + brand +"'");
                dbCommand.Connection = dbconnect;
                dbconnect.Open();
                SqlDataReader dbReader = dbCommand.ExecuteReader();
                while (dbReader.Read())
                {
                    result.Add(new SmartphoneOwner
                    {
                        Count = 100 * dbReader.GetInt32(0) / dbReader.GetInt32(2),
                        Gender = dbReader.GetString(1)
                    });
                }
            }
            return result;
            }
          

        public List<Smartphones> GetBrandList()
        {
            List<Smartphones> brandlist = new List<Smartphones>();
            using (dbconnect = new SqlConnection(connectionstring))
            {
                SqlCommand dbCommand = new SqlCommand("Select Brand  from SmartphoneBrands");
                dbCommand.Connection = dbconnect;
                dbconnect.Open();
                SqlDataReader dbReader = dbCommand.ExecuteReader();

                while (dbReader.Read())
                {
                    brandlist.Add(new Smartphones { BrandName = dbReader.GetString(0), Owned = false });
                }
                dbconnect.Close();
            }
            return brandlist;
        }

        public List<string> GetGenders()
        {
            List<string> genders = new List<string>();
            using (dbconnect = new SqlConnection(connectionstring))
            {
                SqlCommand dbCommand = new SqlCommand("Select Gender from Genders");
                dbCommand.Connection = dbconnect;
                dbconnect.Open();
                SqlDataReader dbReader = dbCommand.ExecuteReader();
                while (dbReader.Read())
                {
                    genders.Add(dbReader.GetString(0));
                }
                dbconnect.Close();
            }

            return genders;
        }
        public void UpdateCount(string brand, string gender)
        {
            using (dbconnect = new SqlConnection(connectionstring))
            {   
                SqlCommand dbCommand = new SqlCommand(@"Update SmartphoneOwnres set Count+=1 from(Select Count, Brand, Genders.Gender, Genders.Id from 
                                                        SmartphoneOwnres inner join SmartphoneBrands on BrandName = SmartphoneBrands.Id inner join Genders on SmartphoneOwnres.Gender = Genders.Id 
                                                            where Brand ='" + brand+"' and Genders.Gender ='"+gender+ "') t where SmartphoneOwnres.Count = t.Count and SmartphoneOwnres.Gender=t.Id");
                dbCommand.Connection = dbconnect;
                dbconnect.Open();
                dbCommand.ExecuteNonQuery();

                dbconnect.Close();
            }      
        }
    }
}