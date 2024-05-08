using FoodFeedbackForm.Models;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using System.Data.SQLite;
using Microsoft.AspNetCore.Http;
using RestSharp;

namespace FoodFeedbackForm.Controllers
{
    public class FeedbackController : Controller
    {
        private IConfiguration Configuration;
        private readonly ILogger<FeedbackController> logger;
        private readonly IWebHostEnvironment Environment;

        public FeedbackController(ILogger<FeedbackController> _logger, IConfiguration _Configuration, IWebHostEnvironment _environment)
        {
            logger = _logger;
            Configuration = _Configuration;
            Environment = _environment;
        }


        [HttpGet]
        public IActionResult Index(string preferedTimeSlotFilter, string foodLikedFilter, string visitingStatusFilter,int h)
        {
            List<Feedback> feedbacks = new List<Feedback>();
            string Conn = this.Configuration.GetConnectionString("myconn");
            using (SQLiteConnection db = new SQLiteConnection(Conn))
            {
               
                string query = "SELECT * FROM Feedback WHERE 1=1";
                DynamicParameters parameters = new DynamicParameters();

                if (!string.IsNullOrEmpty(preferedTimeSlotFilter))
                {
                    query += " AND preferedTimeSlot = @preferedTimeSlotFilter";
                    parameters.Add("@preferedTimeSlotFilter", preferedTimeSlotFilter);
                }

                if (!string.IsNullOrEmpty(foodLikedFilter))
                {
                    query += " AND foodLiked LIKE @foodLikedFilter";
                    parameters.Add("@foodLikedFilter", $"%{foodLikedFilter}%");
                }

                if (!string.IsNullOrEmpty(visitingStatusFilter))
                {
                    query += " AND visitingStatus = @visitingStatusFilter";
                    parameters.Add("@visitingStatusFilter", visitingStatusFilter);
                }

                feedbacks = db.Query<Feedback>(query, parameters).ToList();
            }

            return View(feedbacks);
        }

      

            [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Feedback feedbacks, IFormFile imageFile)
        {
            string Conn = this.Configuration.GetConnectionString("myconn");
            using (SQLiteConnection db = new SQLiteConnection(Conn))
            {
                feedbacks.id = db.ExecuteScalar<int>("SELECT COALESCE(MAX(id), 0) + 1 FROM Feedback");

                string serialNumber = GenerateSerialNumber();
                feedbacks.serialNumber = serialNumber;

                if (imageFile != null && imageFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        imageFile.CopyTo(memoryStream);
                        feedbacks.Image = memoryStream.ToArray();
                    }
                }


                var selectedFoods = Request.Form["foodLiked"].ToList();
                feedbacks.foodLiked = string.Join(",", selectedFoods);

                db.Execute("INSERT INTO Feedback (id, name, serialNumber, email, mobNo, Image, visitingStatus, foodLiked, preferedTimeSlot) " +
                           "VALUES (@id, @name, @serialNumber, @email, @mobNo, @Image, @visitingStatus, @foodLiked, @preferedTimeSlot)",
                           feedbacks);

            }

            TempData["FeedbackSaved"] = true;
            return RedirectToAction("Index", "Feedback");
        }

       
        private string GenerateSerialNumber()
        {
            string currentDate = DateTime.Now.ToString("yyyyMMdd");
            string serialNumber = "";

           
            string Conn = this.Configuration.GetConnectionString("myconn");
            using (SQLiteConnection db = new SQLiteConnection(Conn))
            {
               
                int count = db.ExecuteScalar<int>("SELECT COUNT(*) FROM Feedback WHERE serialNumber LIKE @pattern",
                    new { pattern = $"{currentDate}%" });

                serialNumber = $"{currentDate}{(count + 1).ToString("000")}";
            }

            return serialNumber;
        }

    }
}




//for api key 
//using FoodFeedbackForm.Models;
//using Microsoft.AspNetCore.Mvc;
//using Dapper;
//using System.Data.SQLite;
//using Microsoft.AspNetCore.Http;
//using RestSharp; 

//namespace FoodFeedbackForm.Controllers
//{
//    public class FeedbackController : Controller
//    {
//        private IConfiguration Configuration;
//        private readonly ILogger<FeedbackController> logger;
//        private readonly IWebHostEnvironment Environment;

//        public FeedbackController(ILogger<FeedbackController> _logger, IConfiguration _Configuration, IWebHostEnvironment _environment)
//        {
//            logger = _logger;
//            Configuration = _Configuration;
//            Environment = _environment;
//        }


//        [HttpGet]
//        public IActionResult Index(string preferedTimeSlotFilter, string foodLikedFilter, string visitingStatusFilter, int h)
//        {
//            List<Feedback> feedbacks = new List<Feedback>();
//            string Conn = this.Configuration.GetConnectionString("myconn");
//            using (SQLiteConnection db = new SQLiteConnection(Conn))
//            {

//                string query = "SELECT * FROM Feedback WHERE 1=1";
//                DynamicParameters parameters = new DynamicParameters();

//                if (!string.IsNullOrEmpty(preferedTimeSlotFilter))
//                {
//                    query += " AND preferedTimeSlot = @preferedTimeSlotFilter";
//                    parameters.Add("@preferedTimeSlotFilter", preferedTimeSlotFilter);
//                }

//                if (!string.IsNullOrEmpty(foodLikedFilter))
//                {
//                    query += " AND foodLiked LIKE @foodLikedFilter";
//                    parameters.Add("@foodLikedFilter", $"%{foodLikedFilter}%");
//                }

//                if (!string.IsNullOrEmpty(visitingStatusFilter))
//                {
//                    query += " AND visitingStatus = @visitingStatusFilter";
//                    parameters.Add("@visitingStatusFilter", visitingStatusFilter);
//                }

//                feedbacks = db.Query<Feedback>(query, parameters).ToList();
//            }

//            return View(feedbacks);
//        }



//        [HttpGet]
//        public IActionResult Create()
//        {
//            return View();
//        }
//        [HttpPost]
//        public IActionResult Create(Feedback feedbacks, IFormFile imageFile)
//        {
//            string Conn = this.Configuration.GetConnectionString("myconn");
//            using (SQLiteConnection db = new SQLiteConnection(Conn))
//            {
//                feedbacks.id = db.ExecuteScalar<int>("SELECT COALESCE(MAX(id), 0) + 1 FROM Feedback");

//                //Validate email using Email Verifier API
//                          bool isEmailValid = VerifyEmail(feedbacks.email);

//                           if (!isEmailValid)
//                                 {
//                                      ModelState.AddModelError("email", "Email is not valid.");
//                                       return View(feedbacks);
//                                     }
//                string serialNumber = GenerateSerialNumber();
//                feedbacks.serialNumber = serialNumber;

//                if (imageFile != null && imageFile.Length > 0)
//                {
//                    using (var memoryStream = new MemoryStream())
//                    {
//                        imageFile.CopyTo(memoryStream);
//                        feedbacks.Image = memoryStream.ToArray();
//                    }
//                }


//                var selectedFoods = Request.Form["foodLiked"].ToList();
//                feedbacks.foodLiked = string.Join(",", selectedFoods);

//                db.Execute("INSERT INTO Feedback (id, name, serialNumber, email, mobNo, Image, visitingStatus, foodLiked, preferedTimeSlot) " +
//                           "VALUES (@id, @name, @serialNumber, @email, @mobNo, @Image, @visitingStatus, @foodLiked, @preferedTimeSlot)",
//                           feedbacks);

//            }

//            TempData["FeedbackSaved"] = true;
//            return RedirectToAction("Index", "Feedback");
//        }


//        private string GenerateSerialNumber()
//        {
//            string currentDate = DateTime.Now.ToString("yyyyMMdd");
//            string serialNumber = "";


//            string Conn = this.Configuration.GetConnectionString("myconn");
//            using (SQLiteConnection db = new SQLiteConnection(Conn))
//            {

//                int count = db.ExecuteScalar<int>("SELECT COUNT(*) FROM Feedback WHERE serialNumber LIKE @pattern",
//                    new { pattern = $"{currentDate}%" });

//                serialNumber = $"{currentDate}{(count + 1).ToString("000")}";
//            }

//            return serialNumber;
//        }

//        //Method to verify email using Email Verifier API
//        private bool VerifyEmail(string email)
//        {
//            // Replace 'YOUR_RAPIDAPI_KEY' with your actual RapidAPI key
//            string apiKey = "YOUR_RAPIDAPI_KEY";

//            var client = new RestClient("https://mr-admin-email-verifier.p.rapidapi.com/v1/verify");
//            var request = new RestRequest(Method.POST);
//            request.AddHeader("x-rapidapi-key", apiKey);
//            request.AddHeader("x-rapidapi-host", "mr-admin-email-verifier.p.rapidapi.com");
//            request.AddHeader("Content-Type", "application/json");
//            request.AddParameter("undefined", "{\n    \"email\": \"" + email + "\"\n}", ParameterType.RequestBody);
//            IRestResponse response = client.Execute(request);

//            // Deserialize response
//            dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(response.Content);
//            bool isValid = result.result == "Valid";

//            return isValid;
//        }

//    }
//}

