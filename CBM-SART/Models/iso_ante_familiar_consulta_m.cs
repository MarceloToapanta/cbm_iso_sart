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
    public partial class iso_ante_familiar_consulta_m
    {
        public int ifc_id_consulta_medica { get; set; }
        public int ifc_id_historia_clinica { get; set; }
        public int ifc_id_personal { get; set; }
        [Display(Name = "Antecedente Familiar")]
        [Required]
        public int ifc_id_antecedente_familiar { get; set; }
        public string ifc_tipo_antecedente_f { get; set; }
        [Display(Name = "Edad")]
        [Required]
        public Nullable<short> ifc_edad { get; set; }
        [Display(Name = "Diagnóstico")]
        [Required]
        public string ifc_observacion { get; set; }
    
        public virtual iso_antecedente_familiar iso_antecedente_familiar { get; set; }
        public virtual iso_consulta_medica iso_consulta_medica { get; set; }
    }
}
