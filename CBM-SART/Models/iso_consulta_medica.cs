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
    
    public partial class iso_consulta_medica
    {
        public iso_consulta_medica()
        {
            this.iso_ante_familiar_consulta_m = new HashSet<iso_ante_familiar_consulta_m>();
            this.iso_ante_mujer_consulta_m = new HashSet<iso_ante_mujer_consulta_m>();
            this.iso_ante_personal_consulta_m = new HashSet<iso_ante_personal_consulta_m>();
            this.iso_exam_fp_consulta_m = new HashSet<iso_exam_fp_consulta_m>();
            this.iso_pedido_exam_consulta_m = new HashSet<iso_pedido_exam_consulta_m>();
            this.iso_sindrome_consulta_m = new HashSet<iso_sindrome_consulta_m>();
            this.iso_sintoma_consulta_m = new HashSet<iso_sintoma_consulta_m>();
            this.iso_vacuna_consulta_m = new HashSet<iso_vacuna_consulta_m>();
            this.iso_habito_consulta_m = new HashSet<iso_habito_consulta_m>();
        }
    
        public int icm_id_consulta { get; set; }
        public int icm_id_historia_clinica { get; set; }
        public int icm_id_personal { get; set; }
        public Nullable<int> icm_tipo_consulta { get; set; }
        [Display(Name = "Fecha Consulta Médica")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> icm_fecha_consulta { get; set; }
        public string icm_motivo_consulta { get; set; }
        public string icm_observaciones { get; set; }
        public string icm_nombre_medico { get; set; }
        public string icm_val_especialista { get; set; }
        public string icm_nombre_especialista { get; set; }
        public string icm_control { get; set; }
        public string icm_apto_cont_puesto { get; set; }
        public string icm_apto_cont_vmedica { get; set; }
        public string icm_requiere_cambiop { get; set; }
        public string icm_enfermedad_actual { get; set; }
        public string icm_reubicacion { get; set; }
        public string icm_tiempo_reintegro { get; set; }
        public string icm_crit_reintegro { get; set; }
        public string icm_exam_preocu { get; set; }
        public string icm_ctrl_period { get; set; }
        public string icm_cuantos_periodo { get; set; }
        public string icm_enfer_prexistentes { get; set; }
        public string icm_ert { get; set; }
        public string icm_enfer_laboral { get; set; }
        public string icm_accidente_trab { get; set; }
        public string icm_discapacidad { get; set; }
        public string icm_r1 { get; set; }
        public string icm_r2 { get; set; }
        public string icm_r3 { get; set; }
        public string icm_r4 { get; set; }
        public Nullable<System.DateTime> icm_fecha_salida { get; set; }
    
        public virtual iso_historia_clinica iso_historia_clinica { get; set; }
        public virtual iso_tipo_consulta_m iso_tipo_consulta_m { get; set; }
        public virtual ICollection<iso_ante_familiar_consulta_m> iso_ante_familiar_consulta_m { get; set; }
        public virtual ICollection<iso_ante_mujer_consulta_m> iso_ante_mujer_consulta_m { get; set; }
        public virtual ICollection<iso_ante_personal_consulta_m> iso_ante_personal_consulta_m { get; set; }
        public virtual ICollection<iso_exam_fp_consulta_m> iso_exam_fp_consulta_m { get; set; }
        public virtual ICollection<iso_pedido_exam_consulta_m> iso_pedido_exam_consulta_m { get; set; }
        public virtual ICollection<iso_sindrome_consulta_m> iso_sindrome_consulta_m { get; set; }
        public virtual ICollection<iso_sintoma_consulta_m> iso_sintoma_consulta_m { get; set; }
        public virtual ICollection<iso_vacuna_consulta_m> iso_vacuna_consulta_m { get; set; }
        public virtual ICollection<iso_habito_consulta_m> iso_habito_consulta_m { get; set; }
        public virtual ICollection<iso_permiso_medico> iso_permiso_medico { get; set; }
    }
}
