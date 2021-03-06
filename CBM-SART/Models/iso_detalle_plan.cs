//------------------------------------------------------------------------------
// <auto-generated>
//     Este cÃ³digo se generÃ³ a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicaciÃ³n.
//     Los cambios manuales en este archivo se sobrescribirÃ¡n si se regenera el cÃ³digo.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CBM_SART.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class iso_detalle_plan
    {
        public iso_detalle_plan()
        {
            this.iso_avance_plan = new HashSet<iso_avance_plan>();
        }

        public int idp_id_detalle_plan { get; set; }
        [Display(Name = "Plan/Programa")]
        [Required]
        public int idp_id_plan { get; set; }
        [Display(Name = "Número Tarea")]
        [Required]
        public int idp_numero_plan { get; set; }
        [Display(Name = "Tarea")]
        [Required]
        public string idp_tarea { get; set; }
        public string idp_observacion { get; set; }
        [Display(Name = "Fecha Inicio")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Required]
        public Nullable<System.DateTime> idp_fecha_comienzo { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Fecha Fin")]
        [Required]
        public Nullable<System.DateTime> idp_fecha_fin { get; set; }
        public string idp_duracion { get; set; }
        public string idp_costo { get; set; }
        public string idp_porcentaje { get; set; }
        [Display(Name = "Estado")]
        public string idp_estado { get; set; }
        public Nullable<int> idp_id_plan_padre { get; set; }
        [Display(Name = "Descripción")]
        public string idp_descripcion { get; set; }
        public Nullable<System.DateTime> idp_fecha_creacion { get; set; }
        public Nullable<System.DateTime> idp_fecha_modificacion { get; set; }
        public string idp_verificar_accion { get; set; }
        public string idp_unidad { get; set; }
        [Display(Name = "Cantidad")]
        public string idp_cantidad { get; set; }
        public string idp_tiempoded { get; set; }
        public string idp_tiemponeto { get; set; }
        public string idp_avancep { get; set; }
        public string idp_objetivo_estrategico { get; set; }
        public string idp_entregable { get; set; }
        public Nullable<int> idp_id_departamento { get; set; }
        public string idp_costo_ejecutado { get; set; }
        public Nullable<int> idp_id_proceso { get; set; }
        public string idp_peso { get; set; }

        public virtual ICollection<iso_avance_plan> iso_avance_plan { get; set; }
        public virtual iso_plan iso_plan { get; set; }
    }
}