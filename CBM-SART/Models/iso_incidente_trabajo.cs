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
    
    public partial class iso_incidente_trabajo
    {
        public int iit_id_personal { get; set; }
        public int iit_id_incidente { get; set; }
        public Nullable<System.DateTime> iit_fecha_incidente { get; set; }
        public string iit_descripcion_incid { get; set; }
        public string iit_tratamiento_rec { get; set; }
        public string iit_tiempo_reposo { get; set; }
        public string iit_lugar { get; set; }
        public string iit_hora { get; set; }
    
        public virtual iso_personal iso_personal { get; set; }
    }
}
