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
    
    public partial class iso_cargo
    {
        public iso_cargo()
        {
            this.iso_puesto_trabajo = new HashSet<iso_puesto_trabajo>();
        }
    
        public int icg_id_cargo { get; set; }
        public string icg_nombre { get; set; }
        public string icg_descripcion { get; set; }
        public Nullable<int> icg_cod_clase_cargo { get; set; }
        public Nullable<int> icg_num_trabajadores { get; set; }
        public string icg_area { get; set; }
        public string icg_jefe_inmediato { get; set; }
        public string icg_edad { get; set; }
        public string icg_genero { get; set; }
        public string icg_instruccion { get; set; }
        public string icg_experiencia { get; set; }
        public string icg_conocimiento_adicional { get; set; }
    
        public virtual ICollection<iso_puesto_trabajo> iso_puesto_trabajo { get; set; }
    }
}
