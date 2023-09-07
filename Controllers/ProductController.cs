using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using Newtonsoft.Json;
using ProductMVC.Models;
using System.Net.Http.Headers;

namespace ProductMVC.Controllers;

public class ProductController : Controller
{

    //Base URL to the API
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
                var ProdResponse = Res.Content.ReadAsStringAsync().Result;
                products = JsonConvert.DeserializeObject<List<Product>>(ProdResponse);
            }
            return View(products);
        }
    }

    /// <summary>
    /// This method gets 1 product from the API with the given id
    /// </summary>
    /// <param name="id">the id of the product.</param>
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
                var ProdResponse = Res.Content.ReadAsStringAsync().Result;
                product = JsonConvert.DeserializeObject<Product>(ProdResponse);
            }

            return View(product);
        }
    }

    /// <summary>
    /// This method deletes 1 product from the API with the given id
    /// </summary>
    /// <param name="id">the id of the product.</param>
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
                var ProdResponse = Res.Content.ReadAsStringAsync().Result;
            }

            return RedirectToAction("Index");
        }
    }

    public IActionResult Create()
    {
        return View();
    }

    /// <summary>
    /// This method creates 1 product from the API, called by the View
    /// </summary>
    /// <param name="product">product object to be created.</param>
    [HttpPost]
    public ActionResult Create(Product product)
    {
        using (var client = new HttpClient())
        {
            product.Id = 0;
            client.BaseAddress = new Uri($"{BaseURL}/products");

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

    /// <summary>
    /// This method gets 1 product from the API then opens Update View
    /// </summary>
    /// <param name="id">product id  to be updated.</param>
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
                var ProdResponse = Res.Content.ReadAsStringAsync().Result;
                product = JsonConvert.DeserializeObject<Product>(ProdResponse);
            }

            return View(product);
        }
    }

    /// <summary>
    /// This method updates 1 product from the API
    /// </summary>
    /// <param name="product">product object to be updated.</param>
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