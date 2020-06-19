using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppGallineros.ViewModels
{
    public class ProduccionVM
    {
        public string granja { get; set; }

        public string granjero { get; set; }        
               
        public string gallinero { get; set; }

        public int qty { get; set; }

        public string folio { get; set; }
    }
}