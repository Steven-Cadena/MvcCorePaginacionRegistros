using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCorePaginacionRegistros.Models
{
    [Table("V_DEPT_INDIVIDUAL")]
    public class VistaDepartamento
    {
        [Key]
        [Column("DEPT_NO")]
        public int IdDepartament { get; set; }
        [Column("DNOMBRE")]
        public String Nombre { get; set; }
        [Column("LOC")]
        public String Localidad { get; set; }
        [Column("POSICION")]
        public int Posicion { get; set; }
    }
}
 