using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MvcClienteApiAuthPractica.Models;
using MvcClienteApiAuthPractica.Repositories;
using Newtonsoft.Json;
using System.Security.Claims;

namespace MvcClienteApiAuthPractica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadosController : ControllerBase
    {
        private RepositoryEmpleados repo;

        public EmpleadosController(RepositoryEmpleados repo)
        {
            this.repo = repo;
        }

        // GET: api/empleados
        /// <summary>
        /// Obtiene el conjunto de Empleados, tabla EMP.
        /// </summary>
        /// <remarks>
        /// Método para devolver todos los empleados de la BBDD
        /// </remarks>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>  
        [HttpGet]
        public async Task<ActionResult<List<Empleado>>>
            GetEmpleados()
        {
            return await this.repo.GetEmpleadosAsync();
        }

        /// <summary>
        /// Obtiene una Empleado por su Id, tabla EMP.
        /// </summary>
        /// <remarks>
        /// Permite buscar un objeto Empleado por su ID
        /// </remarks>
        /// <param name="idempleado">Id (GUID) del objeto Empleado.</param>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>        
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response> 
        [HttpGet("{id}")]
        public async Task<ActionResult<Empleado>>
            FindEmpleado(int idempleado)
        {
            Empleado empleado =
                await this.repo.FindEmpleadoAsync(idempleado);
            if (empleado == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(empleado);
            }
        }
        //Este es con token 
        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<Empleado>>
            PerfilEmpleado()
        {
            //INTERNAMENTE, CUANDO RECIBIMOS EL TOKEN 
            //EL USUARIO ES VALIDADO Y ALMACENA DATOS 
            //COMO HttpContext.User.Identity.IsAuthenticated
            //COMO HEMOS INCLUIDO LA KEY DE LOS Claims, 
            //AUTOMATICAMENTE TAMBIEN TENEMOS DICHOS CLAIMS
            //COMO EN LAS APLICACIONES MVC
            Claim claim = HttpContext.User
                .FindFirst(x => x.Type == "UserData");
            //RECUPERAMOS EL JSON DEL EMPLEADO
            string jsonEmpleado = claim.Value;
            Empleado empleado =
                JsonConvert.DeserializeObject<Empleado>(jsonEmpleado);
            return empleado;
        }



        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<List<string>>>
            Oficios()
        {
            return await this.repo.GetOficiosAsync();
        }

        //?oficio=dato&oficio=data2
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<List<Empleado>>>
            EmpleadosOficios([FromQuery] List<string> oficio)
        {
            return await this.repo.GetEmpleadosOficiosAsync(oficio);
        }

    }
}