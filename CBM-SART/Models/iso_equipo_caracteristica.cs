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
    
    public partial class iso_equipo_caracteristica
    {
        public int iec_id_equipo { get; set; }
        public int iec_id_caracteristica { get; set; }
    
        public virtual iso_equipo_proteccion iso_equipo_proteccion { get; set; }
    }
}
