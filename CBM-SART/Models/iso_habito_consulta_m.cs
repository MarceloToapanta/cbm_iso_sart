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
    
    public partial class iso_habito_consulta_m
    {
        public int ihc_id_consulta_medica { get; set; }
        public int ihc_id_historia_clinica { get; set; }
        public int ihc_id_personal { get; set; }
        public int ihc_id_habito { get; set; }
        public string ihc_caracteristica { get; set; }
        public string ihc_cantidad { get; set; }
        public string ihc_frecuencia { get; set; }
        public string ihc_factor_riesgo { get; set; }
        public string ihc_descripcion { get; set; }
        public string ihc_observacion { get; set; }
    
        public virtual iso_consulta_medica iso_consulta_medica { get; set; }
        public virtual iso_habitos iso_habitos { get; set; }
    }
}