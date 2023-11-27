using FruitStore.Models.Entities;

namespace FruitStore.Areas.Admin.Models
{
    public class AdminAgregarProductosViewModel
    {
        public IEnumerable<CategoriaModel>? Categorias { get; set; }
        public Productos? Producto { get; set; }
        public IFormFile? Archivo { get; set; }
    }
}
