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
    
    public partial class iso_tipo_riesgo_ocupacional
    {
        public iso_tipo_riesgo_ocupacional()
        {
            this.iso_riesgo_mrl = new HashSet<iso_riesgo_mrl>();
            this.iso_riesgo_ocupacional = new HashSet<iso_riesgo_ocupacional>();
        }
    
        public int ito_id_tipo_riesgo { get; set; }
        public string ito_nombre_tipo { get; set; }
    
        public virtual ICollection<iso_riesgo_mrl> iso_riesgo_mrl { get; set; }
        public virtual ICollection<iso_riesgo_ocupacional> iso_riesgo_ocupacional { get; set; }
    }
}