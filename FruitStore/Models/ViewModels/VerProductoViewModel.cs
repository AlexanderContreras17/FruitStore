namespace FruitStore.Models.ViewModels
{
    public class VerProductoViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Categoria { get; set; }=null!;
        public decimal Precio { get; set; }
        public string UnidadMedida { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
    }
}   
