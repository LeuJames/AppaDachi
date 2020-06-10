using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using AppaDachi.Models;

namespace AppaDachi.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {

          // Initialize
          if (HttpContext.Session.GetInt32("Status") == null)
          {
            HttpContext.Session.SetString("Status", "Playing");
            HttpContext.Session.SetInt32("Meals", 3);
            HttpContext.Session.SetInt32("Happiness", 20);
            HttpContext.Session.SetInt32("Fullness", 20);
            HttpContext.Session.SetInt32("Energy", 50);

            TempData["Image"] = "normal.jpg";
            TempData["Message"] = "Welcome to Appa-Dachi! Please take good care of your Appa!";
          }

          // Lose Condition
          else if(HttpContext.Session.GetInt32("Fullness") <= 0 || HttpContext.Session.GetInt32("Happiness") <= 0 )
          {
            HttpContext.Session.SetString("Status", "Lose");
            TempData["Image"] = "lose.jpg";
            TempData["Message"] = "You have mistreated Appa and now he is dead. You lose :(";
          }

          // Win Condition
          else if(HttpContext.Session.GetInt32("Fullness") >= 100 && HttpContext.Session.GetInt32("Happiness") >= 100 && HttpContext.Session.GetInt32("Energy") >= 100)
          {
            HttpContext.Session.SetString("Status", "Win");
            TempData["Image"] = "win.jpg";
            TempData["Message"] = "You have treated Appa great! You win!!!";
          }

          ViewBag.Status = HttpContext.Session.GetString("Status");
          ViewBag.Fullness = HttpContext.Session.GetInt32("Fullness");
          ViewBag.Happiness = HttpContext.Session.GetInt32("Happiness");
          ViewBag.Energy = HttpContext.Session.GetInt32("Energy");
          ViewBag.Meals = HttpContext.Session.GetInt32("Meals");
          ViewBag.Image = TempData["Image"];
          ViewBag.Message = TempData["Message"];

          
          return View();
        }

        [HttpGet("/feed")]
        public IActionResult Feed()
        {
            int? meals = HttpContext.Session.GetInt32("Meals");
            if(meals <= 0)
            {
              TempData["Image"] ="normal.jpg";
              TempData["Message"] = "Uh oh, you don't have enough meals to feed Appa!";
            }
            else
            {
              meals--;
              HttpContext.Session.SetInt32("Meals", (int) meals);

              int? fullness = HttpContext.Session.GetInt32("Fullness");
              Random rand = new Random();
              int luck = rand.Next(1,5);
              if( luck == 1)
              {
                TempData["Image"] ="displeased.png";
                TempData["Message"] = $"Appa ate 1 meal but he didn't like it! No increase in fullness!";
              }
              else
              {
                int incFullness = rand.Next(5, 11);
                fullness += incFullness;
                HttpContext.Session.SetInt32("Fullness", (int) fullness);

                TempData["Image"] ="eating.jpg";
                TempData["Message"] = $"Appa ate 1 meal and gained {incFullness} fullness!";
              }

            }

            return RedirectToAction("Index");
        }

        [HttpGet("/play")]
        public IActionResult Play()
        {
            int? energy = HttpContext.Session.GetInt32("Energy");
            if(energy <= 0)
            {
              TempData["Image"] ="normal.jpg";
              TempData["Message"] = "Uh oh, Appa doesn't have enough energy to play right now!";
            }
            else
            {
              energy -= 5;
              HttpContext.Session.SetInt32("Energy", (int) energy);

              int? happiness = HttpContext.Session.GetInt32("Happiness");
              Random rand = new Random();
              int luck = rand.Next(1,5);
              if( luck == 1)
              {
                TempData["Image"] ="displeased.png";
                TempData["Message"] = "You played with Appa but he didn't like it!! No increase in happiness!";
              }
              else
              {
                int incHappiness = rand.Next(5, 11);
                happiness += incHappiness;
                HttpContext.Session.SetInt32("Happiness", (int) happiness);

                TempData["Image"] ="playing.png";
                TempData["Message"] = $"You played with Appa and he gained {incHappiness} happiness!";
              }

            }

            return RedirectToAction("Index");
        }

                
        [HttpGet("/work")]
        public IActionResult Work()
        {
            int? energy = HttpContext.Session.GetInt32("Energy");
            if(energy <= 0)
            {
              TempData["Image"] ="normal.jpg";
              TempData["Message"] = "Uh oh, Appa doesn't have enough energy to work right now!";
            }
            else
            {
              energy -= 5;
              HttpContext.Session.SetInt32("Energy", (int) energy);

              int? meals = HttpContext.Session.GetInt32("Meals");

              Random rand =  new Random();
              int incMeals = rand.Next(1, 4);
              meals += incMeals;
              HttpContext.Session.SetInt32("Meals", (int) meals);

              TempData["Image"] ="working.jpg";
              TempData["Message"] = $"Yip Yip!! Appa worked and he gained {incMeals} meals!";

            }

            return RedirectToAction("Index");
        }

        [HttpGet("/rest")]
        public IActionResult Rest()
        {
            int? fullness = HttpContext.Session.GetInt32("Fullness");
            if(fullness <= 0)
            {
              TempData["Image"] ="normal.jpg";
              TempData["Message"] = "Uh oh, Appa is too hungry to sleep right now!";
            }
                        
            int? happiness = HttpContext.Session.GetInt32("Happiness");
            if(happiness <= 0)
            {
              TempData["Image"] ="normal.jpg";
              TempData["Message"] = "Uh oh, Appa is too grumpy to sleep right now!";
            }
            
            else
            {
              fullness -= 5;
              HttpContext.Session.SetInt32("Fullness", (int) fullness);

              happiness -= 5;
              HttpContext.Session.SetInt32("Hapiness", (int) happiness);

              int? energy = HttpContext.Session.GetInt32("Energy");
              energy += 15;
              HttpContext.Session.SetInt32("Energy", (int) energy);

              TempData["Image"] ="sleeping.gif";
              TempData["Message"] = "Do not disturb!! Appa rested and gained 15 energy!";

            }

            return RedirectToAction("Index");
        }

        [HttpGet("/reset")]
        public IActionResult Reset()
        {
            HttpContext.Session.SetString("Status", "Playing");
            HttpContext.Session.SetInt32("Meals", 3);
            HttpContext.Session.SetInt32("Happiness", 20);
            HttpContext.Session.SetInt32("Fullness", 20);
            HttpContext.Session.SetInt32("Energy", 50);

            TempData["Image"] = "normal.jpg";
            TempData["Message"] = "Welcome to Appa-Dachi! Please take good care of your Appa!";
            return RedirectToAction("Index");
        }



    }
}
