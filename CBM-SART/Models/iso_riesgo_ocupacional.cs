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
    
    public partial class iso_riesgo_ocupacional
    {
        public iso_riesgo_ocupacional()
        {
            this.iso_detalle_matriz_ocupacional = new HashSet<iso_detalle_matriz_ocupacional>();
        }
    
        public int irg_id_riesgo { get; set; }
        public int irg_tipo_riesgo { get; set; }
        public string irg_nombre_riesgo { get; set; }
    
        public virtual ICollection<iso_detalle_matriz_ocupacional> iso_detalle_matriz_ocupacional { get; set; }
        public virtual iso_tipo_riesgo_ocupacional iso_tipo_riesgo_ocupacional { get; set; }
    }
}
