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
    
    public partial class iso_habitos
    {
        public iso_habitos()
        {
            this.iso_habito_consulta_m = new HashSet<iso_habito_consulta_m>();
        }
    
        public int iht_id_habito { get; set; }
        public string iht_nombre_habito { get; set; }
    
        public virtual ICollection<iso_habito_consulta_m> iso_habito_consulta_m { get; set; }
    }
}
