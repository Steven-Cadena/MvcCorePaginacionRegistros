using Microsoft.EntityFrameworkCore;
using MvcCorePaginacionRegistros.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCorePaginacionRegistros.Data
{
    public class HospitalContext:DbContext
    {
        public HospitalContext(DbContextOptions<HospitalContext> options)
            : base(options) { }

        public DbSet<VistaDepartamento> VistaDepartamentos { get; set; }
        public DbSet<Departamento> Departamentos { get; set; }
    }
}
