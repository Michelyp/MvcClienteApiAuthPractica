using Microsoft.EntityFrameworkCore;
using MvcClienteApiAuthPractica.Data;
using MvcClienteApiAuthPractica.Models;

namespace MvcClienteApiAuthPractica.Repositories
{
    public class RepositoryEmpleados
    {
        private HospitalContext context;

        public RepositoryEmpleados(HospitalContext context)
        {
            this.context = context;
        }

        public async Task<List<Empleado>> GetEmpleadosAsync()
        {
            return await this.context.Empleados.ToListAsync();
        }

        public async Task<Empleado> FindEmpleadoAsync(int idEmpleado)
        {
            return await this.context.Empleados
                .FirstOrDefaultAsync(x => x.IdEmpleado == idEmpleado);
        }
        private int GetMaxIdEmpleado()
        {
            return this.context.Empleados.Max(x => x.IdEmpleado) + 1;
        }
        public async Task<List<string>> GetOficiosAsync()
        {
            var consulta = (from datos in this.context.Empleados
                            select datos.Oficio).Distinct();
            return await consulta.ToListAsync();
        }
        public async Task InsertEmpleadoAsync(string nombre, string oficio, int salario, int iddepartamento)
        {
            Empleado emp = new Empleado();
            emp.IdEmpleado = this.GetMaxIdEmpleado();
            emp.Apellido = nombre;
            emp.Oficio = oficio;
            emp.Salario = salario;
            emp.IdDepartamento = iddepartamento;
            await this.context.Empleados.AddAsync(emp);
            await this.context.SaveChangesAsync();
        }
        public async Task<List<Empleado>>
           GetEmpleadosOficiosAsync(List<string> oficios)
        {
            var consulta = from datos in this.context.Empleados
                           where oficios.Contains(datos.Oficio)
                           select datos;
            return await consulta.ToListAsync();
        }
        public async Task<Empleado>
           LogInEmpleadoAsync(string apellido, int idEmpleado)
        {
            return await this.context.Empleados
                .Where(x => x.Apellido == apellido
                && x.IdEmpleado == idEmpleado)
                .FirstOrDefaultAsync();
        }
        public async Task<List<Empleado>> OficioAsync(string oficio)
        {
            return await this.context.Empleados.Where(x => x.Oficio == oficio).ToListAsync();
        }
        public async Task<List<Empleado>> SalariosAsync(int id)
        {
            return await this.context.Empleados.Where(x => x.Salario == id).ToListAsync();
        }

    }
}