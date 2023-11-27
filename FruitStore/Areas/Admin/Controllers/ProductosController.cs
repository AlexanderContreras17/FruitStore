using FruitStore.Areas.Admin.Models;
using FruitStore.Models.Entities;
using FruitStore.Repositories;
using Microsoft.AspNetCore.Mvc;
using static FruitStore.Areas.Admin.Models.AdminProductosViewModel;

namespace FruitStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductosController : Controller
    {
        public ProductosController(ProductosRepository productosRepository, Repository<Categorias> categoriaRepository)
        {
            ProductosRepository = productosRepository;
            CategoriaRepository = categoriaRepository;
        }

        public ProductosRepository ProductosRepository { get; }
        public Repository<Categorias> CategoriaRepository { get; }

        [HttpGet]
        [HttpPost]
        public IActionResult Index(AdminProductosViewModel vm)
        {
            vm.Categorias = CategoriaRepository.GetAll().OrderBy(x => x.Nombre)
                .Select(x => new CategoriaModel
                {
                    Id = x.Id,
                    Nombre = x.Nombre ?? ""
                });

            if (vm.IdCategoriaSeleccionada == 0)
            {
                vm.Productos = ProductosRepository
                    .GetAll()
                    .Select(x => new ProductoModel
                    {
                        Id = x.Id,
                        Nombre = x.Nombre ?? "",
                        Categoria = x.IdCategoriaNavigation?.Nombre ?? ""
                    });

            }
            else
            {
                vm.Productos = ProductosRepository
                    .GetProductosByCategoria(vm.IdCategoriaSeleccionada)
                    .Select(x => new ProductoModel
                    {
                        Id = x.Id,
                        Nombre = x.Nombre ?? "",
                        Categoria = x.IdCategoriaNavigation?.Nombre ?? ""
                    });
            }
            return View(vm);
        }

        public IActionResult Agregar()
        {
            AdminAgregarProductosViewModel vm = new();
            vm.Categorias = CategoriaRepository.GetAll().OrderBy(x => x.Nombre)
                .Select(x => new CategoriaModel
                {
                    Id = x.Id,
                    Nombre = x.Nombre ?? ""
                });
            return View(vm);
        }
        [HttpPost]
        public IActionResult Agregar(AdminAgregarProductosViewModel vm)
        {
            if (vm.Archivo != null) //Si selecciono un archivo
            {
                if (vm.Archivo.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("", "Solo se permite imagenes JPEG");
                }
                if (vm.Archivo.Length > 500 * 1024)
                {
                    ModelState.AddModelError("", "Solo se permite archivos no mayores a 500Kb");

                }

            }

            //Validar
            if (ModelState.IsValid)
            {
                //Guardar
                ProductosRepository.Insert(vm.Producto);

                if (vm.Archivo == null) //No eligio archivo
                {
                    //Obterner id del archivo
                    //Copiar el archivo llamado nodisponible.jpg y cambiarle el nombre por el id
                    System.IO.File.Copy("wwwroot/img_frutas/0.jpg", $"wwwroot/img_frutas/{vm.Producto.Id}.jpg");
                }
                else
                {
                    System.IO.FileStream fs = System.IO.File.Create($"wwwroot/img_frutas/{vm.Producto.Id}.jpg");
                    vm.Archivo.CopyTo(fs);
                    fs.Close();
                }
                return RedirectToAction("Index");
            }
            vm.Categorias = CategoriaRepository.GetAll().OrderBy(x => x.Nombre)
                .Select(x => new CategoriaModel
                {
                    Id = x.Id,
                    Nombre = x.Nombre ?? ""
                });
            return View(vm);
        }

        public IActionResult Editar(int id)
        {
            var producto = ProductosRepository.Get(id);
            if (producto == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                AdminAgregarProductosViewModel vm = new();
                vm.Producto = producto;
                vm.Categorias = CategoriaRepository.GetAll().OrderBy(x => x.Nombre)
                .Select(x => new CategoriaModel
                {
                    Id = x.Id,
                    Nombre = x.Nombre ?? ""
                });

            return View(vm);
            }
        }
        [HttpPost]
        public IActionResult Editar(AdminAgregarProductosViewModel vm)
        {
            //validar
            if (vm.Archivo != null) //Si selecciono un archivo
            {
                if (vm.Archivo.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("", "Solo se permite imagenes JPEG");
                }
                if (vm.Archivo.Length > 500 * 1024)
                {
                    ModelState.AddModelError("", "Solo se permite archivos no mayores a 500Kb");

                }

            }
            if (ModelState.IsValid)
            {
                var producto = ProductosRepository.Get(vm.Producto.Id); //obtiene la entidad que corresponde al producto del vm
                if(producto == null) 
                { 
                    return RedirectToAction("Index");
                }
                producto.Nombre = vm.Producto.Nombre;
                producto.Precio= vm.Producto.Precio;
                producto.Descripcion= vm.Producto.Descripcion;
                producto.UnidadMedida = vm.Producto.UnidadMedida;
                producto.Precio=vm.Producto.Precio;
                producto.IdCategoria=vm.Producto.IdCategoria;

                ProductosRepository.Update(producto);

                //editar la foto
                 if(vm.Archivo != null)
                {
                    System.IO.FileStream fs = System.IO.File.Create($"wwwroot/img_frutas/{vm.Producto.Id}.jpg");
                    vm.Archivo.CopyTo(fs);
                    fs.Close();
                }
                return RedirectToAction("Index");
            }
            vm.Categorias = CategoriaRepository.GetAll().OrderBy(x => x.Nombre)
              .Select(x => new CategoriaModel
              {
                  Id = x.Id,
                  Nombre = x.Nombre ?? ""
              });
            return View(vm);
        }
        public IActionResult Eliminar(int id)
        {
            var producto = ProductosRepository.Get(id);
            if (producto == null)
            {
                return RedirectToAction("Index");
            }
                return View(producto);
            
        }

        [HttpPost]
        public IActionResult Eliminar(Productos p)
        {
            var producto = ProductosRepository.Get(p.Id);
            if (producto == null)
            {
                return RedirectToAction("Index");
            }
            ProductosRepository.Delete(producto);
            var ruta = $"wwwroot/img_frutas/{p.Id}jpg";
            if (System.IO.File.Exists(ruta))
            {
                System.IO.File.Delete(ruta);
            }
            return RedirectToAction("Index");
        }
    }
}
