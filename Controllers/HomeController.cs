using DatabaseDesignProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Data;

using DatabaseDesignProject.Models;
using Microsoft.Extensions.Configuration;
using System.Diagnostics.Eventing.Reader;
using System.Security.Cryptography;

namespace DatabaseDesignProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public string connectionString = "Server=tcp:dbdesignfinalprojectdbsrv.database.windows.net,1433;Initial Catalog=DBDesignFinalProject_db;Persist Security Info=False;User ID=ShoeStoredbAppUsr;Password=ShoeApp$123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30; ";
        //public string connectionString = "Server=tcp:dbdesignfinalprojectdbsrv.database.windows.net,1433;Initial Catalog=DBDesignFinalProject_db;Persist Security Info=False;User ID=COP4710sa;Password=ShoeStore$123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30; ";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {

            return View();
        }

        [HttpGet]
        public IActionResult ShoeList()
        {

            /*
             
            TODO  place code for user authentication  here

             */

            List<Shoe> listShoes = new List<Shoe>();

            try
            {
                //string connectionString = "Data Source=Joseph-HP\\SQLEXPRESS;Initial Catalog=testStore;Integrated Security=True";

                using (SqlConnection connection0 = new SqlConnection(connectionString))
                {
                    connection0.Open();
                    string sql_text = @"SELECT  [shoeID]
                                          ,[brand]           
                                          ,[shoe_name]		 
                                          ,[price]			 
                                          ,[color]			 
                                          ,[shoe_description]
                                        FROM [SHOES]";
                    using (SqlCommand command0 = new SqlCommand(sql_text, connection0))
                    {
                        using (SqlDataReader reader0 = command0.ExecuteReader())
                        {
                            while (reader0.Read())
                            {
                                Shoe shoe = new Shoe();

                                //shoe.shoeID = reader.GetInt32(0);
                                //shoe.brand = reader.GetString(1);
                                //shoe.shoe_name = reader.GetString(2);
                                //shoe.price = reader.GetDecimal(3);
                                //shoe.color = reader.GetString(4);
                                //shoe.shoe_description = reader.GetString(5);

                                shoe.shoeID = Convert.ToInt32(reader0["shoeID"]);
                                shoe.brand_name = reader0["brand"].ToString();
                                shoe.shoe_name = reader0["shoe_name"].ToString();
                                shoe.shoe_price = Convert.ToDecimal(reader0["price"]);
                                shoe.shoe_color = reader0["color"].ToString();
                                shoe.shoe_description = reader0["shoe_description"].ToString();

                                listShoes.Add(shoe);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                //todo future log error message in database
            }
            return View(listShoes);
        }

        [HttpGet]
        public IActionResult AddShoe()
        {

            return View();
        }
        [HttpPost]
        public IActionResult AddShoe(Shoe aShoe)
        {
            //string aBrand = aShoe.brand_name;
            //string ashoe_name = aShoe.shoe_name;

            //string aBrand2 = Request.Form["brand_name"];
            //string ashoe_name2 = Request.Form["shoe_name"];

            string shoeID = String.Empty;

            try
            {
                using (SqlConnection con1 = new SqlConnection(connectionString))
                {
                    con1.Open();

                    string sql_text = String.Empty;
                    //Write info to the database
                    sql_text = @"INSERT INTO [dbo].[SHOES]  " +
                                         " ([brand],[shoe_name],[price], " +
                                         "  [color], [shoe_description]) " +
                                         " VALUES(@brand_name, @shoe_name, @shoe_price, " +
                                         "  @shoe_color, @shoe_description);";
                    using (var cmd1 = new SqlCommand(sql_text, con1))
                    {
                        try
                        {
                            cmd1.Parameters.Add("@brand_name", SqlDbType.NVarChar);
                            if (aShoe.brand_name == null) { aShoe.brand_name = String.Empty; }
                            if (aShoe.brand_name.Length > 50)
                            {
                                cmd1.Parameters["@brand_name"].Value = aShoe.brand_name.Substring(0, 50);
                            }
                            else
                            {
                                cmd1.Parameters["@brand_name"].Value = aShoe.brand_name;
                            }


                            cmd1.Parameters.Add("@shoe_name", SqlDbType.NVarChar);
                            if (aShoe.shoe_name.Length > 50)
                            {
                                cmd1.Parameters["@shoe_name"].Value = aShoe.shoe_name.Substring(0, 50);
                            }
                            else
                            {
                                cmd1.Parameters["@shoe_name"].Value = aShoe.shoe_name;
                            }


                            cmd1.Parameters.Add("@shoe_price", SqlDbType.SmallMoney);
                            cmd1.Parameters["@shoe_price"].Value = aShoe.shoe_price;

                            cmd1.Parameters.Add("@shoe_color", SqlDbType.NVarChar);
                            if (aShoe.shoe_color.Length > 25)
                            {
                                //string color_test = aShoe.shoe_color.Substring(0, 25);
                                cmd1.Parameters["@shoe_color"].Value = aShoe.shoe_color.Substring(0, 25);

                            }
                            else
                            {
                                cmd1.Parameters["@shoe_color"].Value = aShoe.shoe_color;
                            }


                            cmd1.Parameters.Add("@shoe_description", SqlDbType.NVarChar);
                            if (aShoe.shoe_description.Length > 1000)
                            {
                                cmd1.Parameters["@shoe_description"].Value = aShoe.shoe_description.Substring(0, 1000);
                            }
                            else
                            {
                                cmd1.Parameters["@shoe_description"].Value = aShoe.shoe_description;
                            }


                            cmd1.ExecuteScalar();

                        }
                        catch (Exception Ex)
                        {
                            string msg = Ex.Message.ToString();
                            throw;
                        }
                        cmd1.Dispose();
                        con1.Close();
                    }

                }
            }
            catch (Exception)
            {
                throw;
            }


            //return View();
            return RedirectToAction("ShoeList");
        }

        public IActionResult AssignmentDesc()
        {
            return View();
        }

       
        [HttpGet]
        public IActionResult EditShoe(int shoeID)
        {
            Shoe singleShoe = new Shoe();

            using (var con2 = new SqlConnection(connectionString))
            {
                con2.Open();

                using (var cmd2 = new SqlCommand("[dbo].[USP_GET_SHOE_BY_ID]", con2))
                {
                    try
                    {
                        cmd2.CommandType = CommandType.StoredProcedure;

                        cmd2.Parameters.Add("@SearchByShoeId", SqlDbType.Int);
                        cmd2.Parameters["@SearchByShoeId"].Value = shoeID;

                        SqlDataReader rdr2 = cmd2.ExecuteReader();

                        if (rdr2.HasRows)
                        {
                            while (rdr2.Read())
                            {
                                singleShoe.shoeID = Convert.ToInt32(rdr2["shoeID"]);
                                singleShoe.brand_name = Convert.ToString(rdr2["brand"]);
                                singleShoe.shoe_name = Convert.ToString(rdr2["shoe_name"]);
                                singleShoe.shoe_price = Convert.ToDecimal(rdr2["price"]);
                                singleShoe.shoe_color = Convert.ToString(rdr2["color"]);
                                singleShoe.shoe_description = Convert.ToString(rdr2["shoe_description"]);
                            }
                        }
                        rdr2.Close();
                    }
                    catch (Exception Ex)
                    {
                        throw;
                    }
                    
                    cmd2.Dispose();
                    con2.Close();
                }
                //tranScopeInsrtUpdt.Complete();

            }

            return View(singleShoe);
        }

        [HttpPost]
        public IActionResult EditShoe(Shoe editedShoe)
        {

            string shoeID = String.Empty;

            try
            {
                using (SqlConnection con3 = new SqlConnection(connectionString))
                {
                    con3.Open();

                    string sql_text = String.Empty;
                    //Write info to the database
                    sql_text = @"UPDATE [dbo].[SHOES] " +
                                         " SET [brand] = @brand_name, " +
                                         "[shoe_name] = @shoe_name, " +
                                         "[price] = @shoe_price, " +
                                         "[color] = @shoe_color, " +
                                         "[shoe_description] = @shoe_description " +
                                         " WHERE [shoeID] = @shoeID ";
                    using (var cmd3 = new SqlCommand(sql_text, con3))
                    {
                        try
                        {
                            cmd3.Parameters.Add("@shoeID", SqlDbType.Int);
                            cmd3.Parameters["@shoeID"].Value = editedShoe.shoeID;

                            cmd3.Parameters.Add("@brand_name", SqlDbType.NVarChar);
                            if (editedShoe.brand_name == null) { editedShoe.brand_name = String.Empty; }
                            if (editedShoe.brand_name.Length > 50)
                            {
                                cmd3.Parameters["@brand_name"].Value = editedShoe.brand_name.Substring(0, 50);
                            }
                            else
                            {
                                cmd3.Parameters["@brand_name"].Value = editedShoe.brand_name;
                            }


                            cmd3.Parameters.Add("@shoe_name", SqlDbType.NVarChar);
                            if (editedShoe.shoe_name.Length > 50)
                            {
                                cmd3.Parameters["@shoe_name"].Value = editedShoe.shoe_name.Substring(0, 50);
                            }
                            else
                            {
                                cmd3.Parameters["@shoe_name"].Value = editedShoe.shoe_name;
                            }


                            cmd3.Parameters.Add("@shoe_price", SqlDbType.SmallMoney);
                            cmd3.Parameters["@shoe_price"].Value = editedShoe.shoe_price;

                            cmd3.Parameters.Add("@shoe_color", SqlDbType.NVarChar);
                            if (editedShoe.shoe_color.Length > 25)
                            {
                                //string color_test = aShoe.shoe_color.Substring(0, 25);
                                cmd3.Parameters["@shoe_color"].Value = editedShoe.shoe_color.Substring(0, 25);

                            }
                            else
                            {
                                cmd3.Parameters["@shoe_color"].Value = editedShoe.shoe_color;
                            }


                            cmd3.Parameters.Add("@shoe_description", SqlDbType.NVarChar);
                            if (editedShoe.shoe_description.Length > 1000)
                            {
                                cmd3.Parameters["@shoe_description"].Value = editedShoe.shoe_description.Substring(0, 1000);
                            }
                            else
                            {
                                cmd3.Parameters["@shoe_description"].Value = editedShoe.shoe_description;
                            }


                            cmd3.ExecuteNonQuery();

                        }
                        catch (Exception Ex)
                        {
                            string msg = Ex.Message.ToString();
                            throw;
                        }
                        cmd3.Dispose();
                        con3.Close();
                    }

                }
            }
            catch (Exception)
            {
                throw;
            }



            //return View();
            return RedirectToAction("ShoeList");
        }


        [HttpGet]
        public IActionResult ViewShoe(int shoeID)
        {
            List<Review> listReviews = new List<Review>();
            Shoe singleShoe = new Shoe();

            using (var con4 = new SqlConnection(connectionString))
            {
                con4.Open();

                //Retrieving Info Displayed at the Top
                using (var cmd4 = new SqlCommand("[dbo].[USP_GET_SHOE_BY_ID]", con4))
                {
                    try
                    {
                        
                        cmd4.CommandType = CommandType.StoredProcedure;

                        cmd4.Parameters.Add("@SearchByShoeId", SqlDbType.Int);
                        cmd4.Parameters["@SearchByShoeId"].Value = shoeID;

                        SqlDataReader rdr4 = cmd4.ExecuteReader();


                        if (rdr4.HasRows)
                        {
                            while (rdr4.Read())
                            {
                                singleShoe.shoeID = Convert.ToInt32(rdr4["shoeID"]);
                                singleShoe.brand_name = Convert.ToString(rdr4["brand"]);
                                singleShoe.shoe_name = Convert.ToString(rdr4["shoe_name"]);
                                singleShoe.shoe_price = Convert.ToDecimal(rdr4["price"]);
                                singleShoe.shoe_color = Convert.ToString(rdr4["color"]);
                                singleShoe.shoe_description = Convert.ToString(rdr4["shoe_description"]);
                            }
                        }

            
                        rdr4.Close();
                    }
                    catch (Exception Ex)
                    {
                        throw;
                    }

                    cmd4.Dispose();
                }

                //Retrieving info for Reviews
                string sql_get_reviews = @"SELECT [reviewer]
                                          ,[rating]           
                                          ,[rating_description]		 
                                    FROM [REVIEWS]
                                    WHERE [reviewed_shoe] = @shoeID";

                //using (SqlCommand cmd5 = new SqlCommand("[dbo].[USP_GET_SHOE_BY_ID]", con4))
                using (SqlCommand cmd5 = new SqlCommand(sql_get_reviews, con4))
                {
                    cmd5.Parameters.Add("@ShoeID", SqlDbType.Int);
                    cmd5.Parameters["@ShoeID"].Value = shoeID;

                    //cmd5.CommandType = CommandType.StoredProcedure; //(not a stored procedure right now)
                    using (SqlDataReader rdr5 = cmd5.ExecuteReader())
                    {
                        while (rdr5.Read())
                        {
                            Review review = new Review();

                            review.review_shoeID = shoeID;
                            review.reviewer = rdr5["reviewer"].ToString();
                            review.rating = Convert.ToInt32(rdr5["rating"]);
                            review.rating_description = rdr5["rating_description"].ToString();

                            listReviews.Add(review);
                        }

                    }
                    cmd5.Parameters.Add("@shoeID", SqlDbType.Int);
                    cmd5.Parameters["@shoeID"].Value = shoeID;
                }

                con4.Close();
                //tranScopeInsrtUpdt.Complete();

            }

            ViewShoeViewModel finalItem = new ViewShoeViewModel();
            finalItem.current_shoe = singleShoe;
            finalItem.current_shoe_reviews = listReviews;

            return View(finalItem);
        }


        [HttpPost]
        public IActionResult CheckLoginBeforeReview(int shoeID, string username, string password)
        {
            string aTest = "this is a test";
            try
            {
                //string connectionString = "Data Source=Joseph-HP\\SQLEXPRESS;Initial Catalog=testStore;Integrated Security=True";

                using (SqlConnection con6 = new SqlConnection(connectionString))
                {
                    con6.Open();
                    string sql_login_text = @"SELECT [username],
                                                     [password]           
                                              FROM [USERS]
                                              WHERE [username] = @loginUsername AND [password] = @loginPassword";

                    using (SqlCommand cmd6 = new SqlCommand(sql_login_text, con6))
                    {

                        cmd6.Parameters.Add("@loginUsername", SqlDbType.NVarChar);
                        cmd6.Parameters["@loginUsername"].Value = username;
                        cmd6.Parameters.Add("@loginPassword", SqlDbType.NVarChar);
                        cmd6.Parameters["@loginPassword"].Value = password;

                        using (SqlDataReader rdr6 = cmd6.ExecuteReader())
                        {
                            if (rdr6.HasRows)
                            {
                                ShoeIdAndUser id_and_user = new ShoeIdAndUser();
                                id_and_user.shoeID_to_review = shoeID;
                                id_and_user.username_reviewing = username;
                                return RedirectToAction("ReviewShoe", id_and_user);
                            }
                            else
                            {
                                return RedirectToAction("ViewShoe", new { shoeID = shoeID });
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                //todo future log error message in database
            }
            //go to database to check user table with form values


            //if   //username does not exist
            //return RedirectToAction("NotAuthorizedToView");

            //else if //password doesnt match

            //return RedirectToAction("NotAuthorizedToView");  //.return RedirectToAction("Action", new { id = 99 });

            //else if
            //return RedirectToAction("LeaveReview");
            //return View();
            return RedirectToAction("ViewShoe", new { returnShoeID = shoeID });
        }
        [HttpGet]
        public IActionResult CheckLoginBeforeReview(int shoeID)
        {
            string aTest = "this is a test";
            int i = shoeID;
            return View(shoeID);
        }

        [HttpGet]
        public IActionResult ReviewShoe(ShoeIdAndUser id_and_user)
        {
            Review newReview = new Review();
            newReview.review_shoeID = id_and_user.shoeID_to_review;
            newReview.reviewer = id_and_user.username_reviewing;

            return View(newReview);
        }
        [HttpPost]
        public IActionResult ReviewShoe(Review newReviewParam)
        {

            string shoeID = String.Empty;

            try
            {
                using (SqlConnection con7 = new SqlConnection(connectionString))
                {
                    con7.Open();

                    string sql_text = String.Empty;
                    //Write info to the database
                    sql_text = @"INSERT INTO [dbo].[REVIEWS]  " +
                                         " ([reviewed_shoe],[reviewer],[rating], " +
                                         "  [rating_description]) " +
                                         " VALUES(@reviewed_shoe, @reviewer, @rating, " +
                                         "  @rating_description);";
                    using (var cmd7 = new SqlCommand(sql_text, con7))
                    {
                        try
                        {
                            cmd7.Parameters.Add("@reviewed_shoe", SqlDbType.Int);
                            cmd7.Parameters["@reviewed_shoe"].Value = newReviewParam.review_shoeID;

                            cmd7.Parameters.Add("@reviewer", SqlDbType.NVarChar);
                            cmd7.Parameters["@reviewer"].Value = newReviewParam.reviewer;

                            cmd7.Parameters.Add("@rating", SqlDbType.Int);
                            cmd7.Parameters["@rating"].Value = newReviewParam.rating;

                            cmd7.Parameters.Add("@rating_description", SqlDbType.NVarChar);
                            if (newReviewParam.rating_description == null) { newReviewParam.rating_description = String.Empty; }
                            if (newReviewParam.rating_description.Length > 1000)
                            {
                                cmd7.Parameters["@rating_description"].Value = newReviewParam.rating_description.Substring(0, 1000);
                            }
                            else
                            {
                                cmd7.Parameters["@rating_description"].Value = newReviewParam.rating_description;
                            }

                            cmd7.ExecuteScalar();

                        }
                        catch (Exception Ex)
                        {
                            string msg = Ex.Message.ToString();
                            throw;
                        }
                        cmd7.Dispose();
                        con7.Close();
                    }

                }
            }
            catch (Exception)
            {
                throw;
            }


            //return View();
            return RedirectToAction("ViewShoe", new { shoeID = newReviewParam.review_shoeID });
        }


        [HttpGet]
        public IActionResult AddUser()
        {

            return View();
        }
        [HttpPost]
        public IActionResult AddUser(User aUser)
        {

            try
            {
                using (SqlConnection con8 = new SqlConnection(connectionString))
                {
                    con8.Open();

                    string sql_text = String.Empty;
                    //Write info to the database
                    sql_text = @"INSERT INTO [dbo].[USERS]  " +
                                         " ([username],[password], [role]) " +
                                         " VALUES(@username, @password, 'appuser');";
                    using (var cmd8 = new SqlCommand(sql_text, con8))
                    {
                        try
                        {
                            cmd8.Parameters.Add("@username", SqlDbType.NVarChar);
                            if (aUser.username.Length > 50)
                            {
                                cmd8.Parameters["@username"].Value = aUser.username.Substring(0, 50);
                            }
                            else
                            {
                                cmd8.Parameters["@username"].Value = aUser.username;
                            }


                            cmd8.Parameters.Add("@password", SqlDbType.NVarChar);
                            if (aUser.password.Length > 50)
                            {
                                cmd8.Parameters["@password"].Value = aUser.password.Substring(0, 50);
                            }
                            else
                            {
                                cmd8.Parameters["@password"].Value = aUser.password;
                            }

                            cmd8.ExecuteScalar();

                        }
                        catch (Exception Ex)
                        {
                            string msg = Ex.Message.ToString();
                            throw;
                        }
                        cmd8.Dispose();
                        con8.Close();
                    }

                }
            }
            catch (Exception)
            {
                throw;
            }


            //return View();
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult DeleteShoeConfirm(int shoeID)
        {
            Shoe singleShoe = new Shoe();

            using (var con9 = new SqlConnection(connectionString))
            {
                con9.Open();

                using (var cmd9 = new SqlCommand("[dbo].[USP_GET_SHOE_BY_ID]", con9))
                {
                    try
                    {
                        cmd9.CommandType = CommandType.StoredProcedure;

                        cmd9.Parameters.Add("@SearchByShoeId", SqlDbType.Int);
                        cmd9.Parameters["@SearchByShoeId"].Value = shoeID;

                        SqlDataReader rdr9 = cmd9.ExecuteReader();

                        if (rdr9.HasRows)
                        {
                            while (rdr9.Read())
                            {
                                singleShoe.shoeID = Convert.ToInt32(rdr9["shoeID"]);
                                singleShoe.brand_name = Convert.ToString(rdr9["brand"]);
                                singleShoe.shoe_name = Convert.ToString(rdr9["shoe_name"]);
                                singleShoe.shoe_price = Convert.ToDecimal(rdr9["price"]);
                                singleShoe.shoe_color = Convert.ToString(rdr9["color"]);
                                singleShoe.shoe_description = Convert.ToString(rdr9["shoe_description"]);
                            }
                        }
                        rdr9.Close();
                    }
                    catch (Exception Ex)
                    {
                        throw;
                    }

                    cmd9.Dispose();
                    con9.Close();
                }
                //tranScopeInsrtUpdt.Complete();

            }

            return View(singleShoe);
        }

        [HttpPost]
        public IActionResult DeleteShoeConfirm(Shoe deleteShoe)
        {

            string shoeID = String.Empty;

            try
            {
                using (SqlConnection con10 = new SqlConnection(connectionString))
                {
                    con10.Open();

                    string sql_text = String.Empty;
                    //Write info to the database
                    sql_text = @"DELETE FROM [SHOES] WHERE [shoeID] = @shoeID";
                    using (var cmd10 = new SqlCommand(sql_text, con10))
                    {
                        try
                        {
                            cmd10.Parameters.Add("@shoeID", SqlDbType.Int);
                            cmd10.Parameters["@shoeID"].Value = deleteShoe.shoeID;

                            cmd10.ExecuteNonQuery();

                        }
                        catch (Exception Ex)
                        {
                            string msg = Ex.Message.ToString();
                            throw;
                        }
                        cmd10.Dispose();
                        con10.Close();
                    }

                }
            }
            catch (Exception)
            {
                throw;
            }



            //return View();
            return RedirectToAction("ShoeList");
        }


        [HttpGet]
        public IActionResult CheckLoginBeforeAddToCart(int shoeID)
        {

            return View(shoeID);
        }

        [HttpPost]
        public IActionResult CheckLoginBeforeAddToCart(int shoeID, string username, string password)
        {
            
            try
            {
                //string connectionString = "Data Source=Joseph-HP\\SQLEXPRESS;Initial Catalog=testStore;Integrated Security=True";

                using (SqlConnection con11 = new SqlConnection(connectionString))
                {
                    con11.Open();
                    string sql_login_text = @"SELECT [username],
                                                     [password]           
                                              FROM [USERS]
                                              WHERE [username] = @loginUsername AND [password] = @loginPassword";

                    using (SqlCommand cmd11 = new SqlCommand(sql_login_text, con11))
                    {

                        cmd11.Parameters.Add("@loginUsername", SqlDbType.NVarChar);
                        cmd11.Parameters["@loginUsername"].Value = username;
                        cmd11.Parameters.Add("@loginPassword", SqlDbType.NVarChar);
                        cmd11.Parameters["@loginPassword"].Value = password;

                        using (SqlDataReader rdr11 = cmd11.ExecuteReader())
                        {
                            if (rdr11.HasRows)
                            {
                                ShoeIdAndUser id_and_user = new ShoeIdAndUser();
                                id_and_user.shoeID_to_review = shoeID;
                                id_and_user.username_reviewing = username;


                                //KEEP GOING HEREEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE
                                string sql_text2 = @"INSERT INTO [dbo].[SHOE_IN_CART]  " +
                                         " ([username],[shoeID]) " +
                                         " VALUES(@username, @shoeID);";
                                using (SqlCommand cmd12 = new SqlCommand(sql_text2, con11))
                                {

                                    cmd12.Parameters.Add("@username", SqlDbType.NVarChar);
                                    if (username.Length > 50)
                                    {
                                        cmd12.Parameters["@username"].Value = username.Substring(0, 50);
                                    }
                                    else
                                    {
                                        cmd12.Parameters["@username"].Value = username;
                                    }


                                    cmd12.Parameters.Add("@shoeID", SqlDbType.Int);
                                    cmd12.Parameters["@shoeID"].Value = shoeID;

                                    rdr11.Close();
                                    cmd12.ExecuteScalar();



                                    cmd12.Dispose();
                                    con11.Close();


                                }

                                return RedirectToAction("ShoeList");

                            }
                            else
                            {
                                rdr11.Close();
                                con11.Close();
                                return RedirectToAction("ViewShoe", new { shoeID = shoeID });
                            }
                        }
                    }
                   
                    
                }

            }
            catch (Exception ex)
            {
                //todo future log error message in database
            }
            
            return RedirectToAction("ViewShoe", new { returnShoeID = shoeID });
        }


        [HttpGet]
        public IActionResult CheckLoginBeforeViewCart(int shoeID)
        {
            return View(shoeID);
        }
        [HttpPost]
        public IActionResult CheckLoginBeforeViewCart(int shoeID, string username, string password)
        {
            string aTest = "this is a test";
            try
            {
                //string connectionString = "Data Source=Joseph-HP\\SQLEXPRESS;Initial Catalog=testStore;Integrated Security=True";

                using (SqlConnection con13 = new SqlConnection(connectionString))
                {
                    con13.Open();
                    string sql_login_text = @"SELECT [username],
                                                     [password]           
                                              FROM [USERS]
                                              WHERE [username] = @loginUsername AND [password] = @loginPassword";

                    using (SqlCommand cmd13 = new SqlCommand(sql_login_text, con13))
                    {

                        cmd13.Parameters.Add("@loginUsername", SqlDbType.NVarChar);
                        cmd13.Parameters["@loginUsername"].Value = username;
                        cmd13.Parameters.Add("@loginPassword", SqlDbType.NVarChar);
                        cmd13.Parameters["@loginPassword"].Value = password;

                        using (SqlDataReader rdr6 = cmd13.ExecuteReader())
                        {
                            if (rdr6.HasRows)
                            {
                                ShoeIdAndUser id_and_user = new ShoeIdAndUser();
                                id_and_user.shoeID_to_review = shoeID;
                                id_and_user.username_reviewing = username;
                                return RedirectToAction("ViewCart", new { username = username });
                            }
                            else
                            {
                                return RedirectToAction("Index");
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                //todo future log error message in database
            }
            //go to database to check user table with form values


            //if   //username does not exist
            //return RedirectToAction("NotAuthorizedToView");

            //else if //password doesnt match

            //return RedirectToAction("NotAuthorizedToView");  //.return RedirectToAction("Action", new { id = 99 });

            //else if
            //return RedirectToAction("LeaveReview");
            //return View();
            return RedirectToAction("ViewCart", new { username = username });
        }
      


        [HttpGet]
        public IActionResult ViewCart(string username)
        {

            List<Shoe> listCartShoes = new List<Shoe>();
            List<int> listCartShoeID = new List<int>();

            try
            {
                //string connectionString = "Data Source=Joseph-HP\\SQLEXPRESS;Initial Catalog=testStore;Integrated Security=True";

                using (SqlConnection con14 = new SqlConnection(connectionString))
                {
                    con14.Open();
                    string sql_text = @"SELECT  [shoeID]
                                        FROM [SHOE_IN_CART]
                                        WHERE [username] = @currentCartUsername";
                    using (SqlCommand cmd14 = new SqlCommand(sql_text, con14))
                    {
                        cmd14.Parameters.Add("@currentCartUsername", SqlDbType.NVarChar);
                        cmd14.Parameters["@currentCartUsername"].Value = username;
                        using (SqlDataReader rdr14 = cmd14.ExecuteReader())
                        {
                            while (rdr14.Read())
                            {
                                int current_cart_shoe_id = Convert.ToInt32(rdr14["shoeID"]);
                                listCartShoeID.Add(current_cart_shoe_id);
                                
                            }
                            rdr14.Close();
                        }

                        string sql_text2 = @"SELECT  [shoeID]
                                          ,[brand]           
                                          ,[shoe_name]		 
                                          ,[price]			 
                                          ,[color]			 
                                          ,[shoe_description]
                                        FROM [SHOES]
                                        WHERE [shoeID] = @currentCartShoeID";
                        using (SqlCommand cmd15 = new SqlCommand(sql_text2, con14))
                        {
                            cmd15.Parameters.Add("@currentCartShoeID", SqlDbType.Int);
                            foreach (int i in listCartShoeID)
                            {
                                
                                cmd15.Parameters["@currentCartShoeID"].Value = i;

                                using (SqlDataReader rdr15 = cmd15.ExecuteReader())
                                {
                                    while (rdr15.Read())
                                    {
                                        Shoe shoe = new Shoe();

                                        //shoe.shoeID = reader.GetInt32(0);
                                        //shoe.brand = reader.GetString(1);
                                        //shoe.shoe_name = reader.GetString(2);
                                        //shoe.price = reader.GetDecimal(3);
                                        //shoe.color = reader.GetString(4);
                                        //shoe.shoe_description = reader.GetString(5);

                                        shoe.shoeID = Convert.ToInt32(rdr15["shoeID"]);
                                        shoe.brand_name = rdr15["brand"].ToString();
                                        shoe.shoe_name = rdr15["shoe_name"].ToString();
                                        shoe.shoe_price = Convert.ToDecimal(rdr15["price"]);
                                        shoe.shoe_color = rdr15["color"].ToString();
                                        shoe.shoe_description = rdr15["shoe_description"].ToString();

                                        listCartShoes.Add(shoe);
                                    }
                                }
                            }
                            
                            
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                //todo future log error message in database
            }
            ViewCartModel current_cart_info = new ViewCartModel();
            current_cart_info.cart_user = username;
            current_cart_info.shoes_in_cart = listCartShoes;
            return View(current_cart_info);
        }

        //test

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}