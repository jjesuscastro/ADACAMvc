using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using Newtonsoft.Json;
using ProductMVC.Models;
using System.Net.Http.Headers;

namespace ProductMVC.Controllers;

public class ProductController : Controller
{
    // 
    // GET: /HelloWorld/

    string BaseURL = "http://localhost:5292";
    public async Task<ActionResult> Index()
    {
        List<Product> products = new List<Product>();

        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(BaseURL);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage Res = await client.GetAsync("products");

            if (Res.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api
                var ProdResponse = Res.Content.ReadAsStringAsync().Result;
                //Deserializing the response recieved from web api and storing into the Employee list
                products = JsonConvert.DeserializeObject<List<Product>>(ProdResponse);
            }
            //returning the employee list to view
            return View(products);
        }
    }

    public async Task<ActionResult> Details(int id)
    {
        Product product = new Product();

        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(BaseURL);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage Res = await client.GetAsync($"products/{id}");

            if (Res.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api
                var ProdResponse = Res.Content.ReadAsStringAsync().Result;
                //Deserializing the response recieved from web api and storing into the Employee list
                product = JsonConvert.DeserializeObject<Product>(ProdResponse);
            }

            return View(product);
        }
    }

    public async Task<ActionResult> Delete(int id)
    {
        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(BaseURL);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage Res = await client.DeleteAsync($"products/{id}");

            if (Res.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api
                var ProdResponse = Res.Content.ReadAsStringAsync().Result;
            }

            return RedirectToAction("Index");
        }
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Create(Product product)
    {
        using (var client = new HttpClient())
        {
            product.Id = 0;
            client.BaseAddress = new Uri($"{BaseURL}/products");

            //HTTP POST
            var postTask = client.PostAsJsonAsync<Product>("products", product);
            postTask.Wait();

            var result = postTask.Result;
            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
        }

        ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

        return View(product);
    }

    public async Task<ActionResult> Update(int id)
    {
        Product product = new Product();

        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(BaseURL);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage Res = await client.GetAsync($"products/{id}");

            if (Res.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api
                var ProdResponse = Res.Content.ReadAsStringAsync().Result;
                //Deserializing the response recieved from web api and storing into the Employee list
                product = JsonConvert.DeserializeObject<Product>(ProdResponse);
            }

            return View(product);
        }
    }

    [HttpPost]
    public ActionResult Update(Product product)
    {
        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri($"{BaseURL}/products/{product.Id}");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var postTask = client.PutAsJsonAsync<Product>($"products", product);
            postTask.Wait();

            var result = postTask.Result;
            Console.Write(result);
            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
        }

        ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

        return View(product);
    }

}