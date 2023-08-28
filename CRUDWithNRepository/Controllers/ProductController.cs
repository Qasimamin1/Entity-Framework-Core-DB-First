using CRUDWithRepository.Core;
using CRUDWithRepository.Infrastructure.Implementations;
using CRUDWithRepository.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CRUDWithNRepository.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepo;

        public ProductController(IProductRepository productRepo)
        {
            _productRepo = productRepo;
        }
        public async Task<IActionResult> Index()
        {

            // read the product data 
            var products = await _productRepo.GetAll();
            return View(products);
        }
        //Adding the data 

        [HttpGet]
        public async Task <IActionResult> CreateOrEdit(int id = 0) {

            if (id == 0)
            {
                // if no id available return empty model
                return View(new Product());

            }  // if id is availble  
            else
            {
                // read the product details from model 
                try
                {
                    Product product = await _productRepo.GetById(id);
                    //get data 
                    if (product != null)
                    {

                        return View(product);
                    }
                }
                catch (Exception ex)
                {

                    TempData["errorMessage"] =ex.Message;
                    return RedirectToAction("Index");
                }
                TempData["errorMessage"]= $"Product details not found with id  { id}" ;
                return RedirectToAction("Index");
            }
            
        }


        [HttpPost]
        public async Task< IActionResult> CreateOrEdit(Product model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if(model.Id== 0) {

                        //insert the data 
                        await _productRepo.Add(model);
                        TempData["successMessage"] = "Product Created Successfully";
                        return RedirectToAction(nameof(Index));

                    }
                   else
                    {
                        //call the update method 
                        await _productRepo.Update(model);
                        TempData["successMessage"] = "Products Details updated Successfully";
                    }

                  return RedirectToAction(nameof(Index));
                }
                else
                {

                    TempData["error message"] = "Model State is Invalid";
                    return View();

                }
            }
            catch (Exception ex)
            {

                TempData["error message"] = ex.Message;
                return View();
            }


            
        }

        [HttpGet]
        public async Task <IActionResult> Delete( int id) {

            try
            {
                Product product = await _productRepo.GetById(id);
                if (product != null)
                {

                    return View(product);

                }
            }
            catch (Exception ex)
            {


                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
            TempData["errorMessage"] = $"Product details not found with id  {id}";
            return RedirectToAction("Index");
        }




        [HttpPost , ActionName("Delete")]
         // get data pass model here as a parameter 
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // pass to repository 
            try
            {
                await _productRepo.Delete(id);
                TempData["successMessage"] = "Product Deleted Successfully";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                TempData["errorMessage"] = ex.Message;
                return View();
            }

        }
    }
}
