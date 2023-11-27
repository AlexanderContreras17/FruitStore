using System;
using System.Collections.Generic;

namespace FruitStore.Models.Entities;

public partial class Categorias
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public bool Eliminado { get; set; }

    public virtual ICollection<Productos> Productos { get; set; } = new List<Productos>();
}
