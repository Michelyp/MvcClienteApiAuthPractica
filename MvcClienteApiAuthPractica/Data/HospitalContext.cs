using Microsoft.EntityFrameworkCore;
using MvcClienteApiAuthPractica.Models;

namespace MvcClienteApiAuthPractica.Data
{
    public class HospitalContext : DbContext
    {
        public HospitalContext(DbContextOptions<HospitalContext> options)
       : base(options) { }
        public DbSet<Empleado> Empleados { get; set; }
    }
}
