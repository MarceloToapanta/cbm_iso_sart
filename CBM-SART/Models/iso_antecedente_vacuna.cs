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
    
    public partial class iso_antecedente_vacuna
    {
        public iso_antecedente_vacuna()
        {
            this.iso_vacuna_consulta_m = new HashSet<iso_vacuna_consulta_m>();
        }
    
        public int iav_id_vacuna { get; set; }
        public string iav_nombre_vacuna { get; set; }
    
        public virtual ICollection<iso_vacuna_consulta_m> iso_vacuna_consulta_m { get; set; }
    }
}
