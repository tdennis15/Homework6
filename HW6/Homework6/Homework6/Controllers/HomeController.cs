using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Homework6.Models;
using System.Net;

namespace Homework6.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Creating a context instance in so that we can access the database inside our controller
        /// </summary>
        private AdventureWorksContext db = new AdventureWorksContext();



        /// <summary>
        /// Standard index action handler. This will handle the landing page and the home page
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int? img = 123)
        {
            return View();
        }

        /// <summary>
        /// Dynamic Page built based off of what category was selected on the home page
        /// This will use the model property of sub categories to query the Db looking for
        /// each item related to the search field
        /// 
        /// @param: default value just in case something weird...
        /// </summary>
        /// <returns> returns a view containing the subcategories based upon the category (actionlinked) </returns>
        [HttpGet]
        public ActionResult Categories(string category = "Bikes")
        {
            //@catID category id viewbag so we can display this to the user on the view.
            ViewBag.catID = category;

            //searching the database via category (string) and return all that match, then ordering by alphabet
            return View(db.ProductSubcategories
                        .Where(v => v.ProductCategory.Name.Contains(category))
                        .OrderBy(p => p.ProductCategoryID));
        }

        /// <summary>
        /// Returning the products that belong to a subcategory. 
        /// </summary>
     
        /// <returns>returns a view of the products of the subcategory in a list format, default id=1 to avoid crash.</returns>
        [HttpGet]
        public ActionResult Products(int ID = 1)
        {

            //function to search by sub category id, and return products
          return View(db.Products
                        .Where(catID => catID.ProductSubcategoryID == ID).ToList());
        }


        /// <summary>
        /// This actionresult brings everything together for 1 item that has been selected.
        /// First we find the product and make sure that it is in our DB, 
        /// Then we try to find the associated photo to return with the view
        /// then we ship out the product in the view.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ProductDetails(int? ID = 1)
        {
            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var product = db.Products.Find(ID);

                //finding the photo was difficult because the table names didn't seem to work correctly.
                var photo = (from item in db.ProductPhotoes
                             where item.ProductProductPhotoes.Any(p => p.ProductID == ID)
                             select item.LargePhoto).FirstOrDefault();

                ViewBag.pPhoto = photo;

                return View(product);
            }
        }


        // POST: Reviews that get submitted are handled here
        // this request comes after submitting the add review form
        [HttpPost]
        public ActionResult Product(int? id, string name, string email, int? rating, string comments)
        {
            // Initialize an object of ProductReview from the DB
            ProductReview review = db.ProductReviews.Create();

            //If any of the fields are null then we have to stop, collaborate and listen.
            if (id == null || id.Equals("") || name == null || name.Equals("") || email == null || email.Equals("") || rating == null || rating.Equals("") || comments == null || comments.Equals(""))
            { //reloads the page
                return View(ProductDetails(id));
            }

            // add all the attributes of a Product Review to add it to the db
            review.Comments = comments;
            review.EmailAddress = email;
            review.ModifiedDate = DateTime.Now; // correct the date/time format
            review.ProductID = (int)id;
            review.Rating = (int)rating;
            review.ReviewDate = DateTime.Now;
            review.ReviewerName = name;

            if (ModelState.IsValid) //error checking to make sure that everything was passed correctly and all the datatypes match
            {
                db.ProductReviews.Add(review); //add it to the database
                db.SaveChanges(); //save the database
                return View(ProductDetails(id)); //reload the page
            }

            return RedirectToAction("Index"); //if it fails reload the home page
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}