//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebAppGallineros.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class detreportegallinas
    {
        public int id { get; set; }
        public int gallinas_id { get; set; }
        public int statusgallina_id { get; set; }
        public int reporte_id { get; set; }
    
        public virtual gallinas gallinas { get; set; }
        public virtual reporte reporte { get; set; }
        public virtual statusgallina statusgallina { get; set; }
    }
}
