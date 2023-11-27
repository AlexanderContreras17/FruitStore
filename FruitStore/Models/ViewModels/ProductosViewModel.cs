namespace FruitStore.Models.ViewModels
{
	public class ProductosViewModel
	{
		//agregacion = a una clase se le agregan datos de otra clase
		//agregacion, composicion, herencia, asociacion
		public string Categoria { get; set; } = null!;
		public IEnumerable<ProductosModel> Productos { get; set; } = null!;
	}
	public class ProductosModel
	{
		public int Id { get; set; }
		public string Nombre { get; set;} = null!;
		public decimal Precio {  get; set; }
        public string Categoria { get; internal set; }
		public string FechaModificacion { get; set; } = null!;		
    }
}
