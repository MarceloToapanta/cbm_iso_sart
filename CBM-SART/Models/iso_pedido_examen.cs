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
    
    public partial class iso_pedido_examen
    {
        public iso_pedido_examen()
        {
            this.iso_pedido_exam_consulta_m = new HashSet<iso_pedido_exam_consulta_m>();
        }
    
        public int ipe_id_tipo_pexamen { get; set; }
        public int ipe_id_pexamen { get; set; }
        public string ipe_nombre_pexamen { get; set; }
    
        public virtual ICollection<iso_pedido_exam_consulta_m> iso_pedido_exam_consulta_m { get; set; }
        public virtual iso_tipo_pedido_exam iso_tipo_pedido_exam { get; set; }
    }
}