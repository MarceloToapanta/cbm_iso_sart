//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CBM_SART.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class iso_tipo_sintoma
    {
        public iso_tipo_sintoma()
        {
            this.iso_sintoma = new HashSet<iso_sintoma>();
        }
    
        public int its_id_tipo_sintoma { get; set; }
        public string its_nombre_tipo_sintoma { get; set; }
    
        public virtual ICollection<iso_sintoma> iso_sintoma { get; set; }
    }
}
