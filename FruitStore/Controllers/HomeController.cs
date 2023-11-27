using FruitStore.Models.Entities;
using FruitStore.Models.ViewModels;
using FruitStore.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FruitStore.Controllers
{
    public class HomeController : Controller
    {
        public ProductosRepository Repository { get; set; }
        public HomeController(ProductosRepository repository) 
        { 
            Repository = repository;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Productos(string Id) //Id es el nombre de la categoria
        {
            Id = Id.Replace("-", " ");
            ProductosViewModel vm = new()
            {
                Categoria = Id,
                Productos= Repository.GetProductosByCategoria(Id)
                .Select(x=> new ProductosModel
                {
                    Id=x.Id,
                    Nombre=x.Nombre?? "",
                    Precio=x.Precio??0 ,
                    FechaModificacion= new FileInfo($"wwwroot/img_frutas/{x.Id}.jpg").LastWriteTime.ToString("yyyyMMddhhmm")
                })
            };

            return View(vm);
        }

        public IActionResult Ver(string Id)
        {
            Id = Id.Replace("-", "");
            var producto=Repository.GetByNombre(Id);
            if (producto == null)
            {
                return RedirectToAction("Index");
            }
            VerProductoViewModel vm = new()
            {
                Id = producto.Id,
                Categoria = producto.IdCategoriaNavigation?.Nombre ?? "",
                Descripcion = producto.Descripcion ?? "",
                Precio = producto.Precio ?? 0,
                UnidadMedida = producto.UnidadMedida ?? "",
                Nombre = producto.Nombre ?? ""
            };
            return View(vm);
        }

    }
}
