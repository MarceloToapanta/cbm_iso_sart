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
    using System.ComponentModel.DataAnnotations;
    
    public partial class iso_matriz_mrl
    {
        public iso_matriz_mrl()
        {
            this.iso_detalle_matriz_mrl = new HashSet<iso_detalle_matriz_mrl>();
        }
    
        public int imr_id_matriz_mrl { get; set; }
        [Display(Name = "Area")]
        [Required]
        public string imr_localizacion_mrl { get; set; }
        public string imr_proceso_mrl { get; set; }
        public string imr_sub_proceso_mrl { get; set; }
        [Display(Name = "Cargo")]
        [Required]
        public string imr_cargo_mrl { get; set; }
        [Display(Name = "Puesto de Trabajo")]
        [Required]
        public string imr_puesto_trabajo_mrl { get; set; }
        [Display(Name = "Nro. Trabajadores")]
        public Nullable<long> imr_nro_tabajadores_mrl { get; set; }
        [Display(Name = "Jefe de Trabajo")]
        public string imr_jefe_area_mrl { get; set; }
        [Display(Name = "Fecha de Evaluación")]
        public string imr_fecha_evaluacion_mrl { get; set; }
        [Display(Name = "Descripción")]
        public string imr_descripcion_mrl { get; set; }
        public Nullable<long> imr_id_puesto_t_mrl { get; set; }
        public string imr_ubicacion { get; set; }
    
        public virtual ICollection<iso_detalle_matriz_mrl> iso_detalle_matriz_mrl { get; set; }
    }
}
