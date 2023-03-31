using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using RESTAPI_CORE.Modelos;
using System.Data;
using System.Data.SqlClient;
using System.Text.Json;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Diagnostics;
using MoreLinq.Extensions;


namespace RESTAPI_CORE.Controllers
{
    [EnableCors("ReglasCors")]
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]

    public class GrupoController : ControllerBase
    {
        private readonly string cadenaSQL;
        public GrupoController(IConfiguration config)
        {
            cadenaSQL = config.GetConnectionString("CadenaSQL");
        }


        [HttpGet]
        [Route("Lista")]
        public IActionResult Lista()
        {
            List<Grupo> lista = new List<Grupo>();
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_listado_grupos", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            lista.Add(new Grupo
                            {
                                usuario = rd["usuario"].ToString(),
                                cedula = rd["cedula"].ToString(),
                                nombres = rd["nombres"].ToString(),
                                apellidos = rd["apellidos"].ToString(),
                                genero = rd["genero"].ToString(),
                                parentesco = rd["parentesco"].ToString(),
                                edad = rd["edad"].ToString(),
                                menoredad = rd["menoredad"].ToString(),
                                fecha = rd["fechanacimiento"].ToString(),

                            });
                        }
                    }
                }
                return StatusCode(StatusCodes.Status200OK, JsonConvert.SerializeObject(lista));
            }
            catch (Exception error)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, response = lista });

            }
        }


        [HttpGet]
        [Route("Obtener/{user}")]
        public IActionResult Obtener(string user)
        {
            List<Grupo> lista = new List<Grupo>();
            Grupo oPost = new Grupo();
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_listado_grupos", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            lista.Add(new Grupo
                            {
                                usuario = rd["usuario"].ToString(),
                                cedula = rd["cedula"].ToString(),
                                nombres = rd["nombres"].ToString(),
                                apellidos = rd["apellidos"].ToString(),
                                genero = rd["genero"].ToString(),
                                parentesco = rd["parentesco"].ToString(),
                                edad = rd["edad"].ToString(),
                                fecha = rd["fechanacimiento"].ToString(),
                                menoredad = rd["menoredad"].ToString(),
                            });
                        }
                    }
                }
                oPost = lista.Where(item => item.usuario == user).FirstOrDefault();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = oPost });
            }
            catch (Exception error)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, response = oPost });

            }
        }



        [HttpPost]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody] GuardarGrupo objeto)
        {
            try
            {
               
                int filas = 0;
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("[sp_guardar_grupos]", conexion);
                    cmd.Parameters.AddWithValue("usuario", objeto.usuario);
                    cmd.Parameters.AddWithValue("cedula", objeto.cedula);
                    cmd.Parameters.AddWithValue("nombres", objeto.nombres);

                    cmd.Parameters.AddWithValue("apellidos", objeto.apellidos);
                    cmd.Parameters.AddWithValue("genero", objeto.genero);
                    cmd.Parameters.AddWithValue("parentesco", objeto.parentesco);
                    cmd.Parameters.AddWithValue("fecha", objeto.fecha);
                    cmd.Parameters.AddWithValue("edad", objeto.edad);
                

                    cmd.CommandType = CommandType.StoredProcedure;
                    filas = cmd.ExecuteNonQuery();
                }
                if (filas > 1)
                {
                    return StatusCode(StatusCodes.Status201Created, new { mensaje = "agregado" });
                }
                else
                {
                    return StatusCode(StatusCodes.Status208AlreadyReported, new { mensaje = "Este Grupo ya ha sido creado" });
                }


            }
            catch (Exception error)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });

            }
        }

        [HttpPut]
        [Route("Editar")]
        public IActionResult Editar([FromBody] GuardarGrupo objeto)
        {
            try
            {
                int filas = 0;
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("[sp_editar_grupos]", conexion);
                    cmd.Parameters.AddWithValue("usuario", objeto.usuario);
                    cmd.Parameters.AddWithValue("cedula", objeto.cedula);
                    cmd.Parameters.AddWithValue("nombres", objeto.nombres);

                    cmd.Parameters.AddWithValue("apellidos", objeto.apellidos);
                    cmd.Parameters.AddWithValue("genero", objeto.genero);
                    cmd.Parameters.AddWithValue("parentesco", objeto.parentesco);
                    cmd.Parameters.AddWithValue("fecha", objeto.fecha);
                    cmd.Parameters.AddWithValue("edad", objeto.edad);

                    cmd.CommandType = CommandType.StoredProcedure;
                    filas = cmd.ExecuteNonQuery();
                }
                if (filas > 1)
                {
                    return StatusCode(StatusCodes.Status201Created, new { mensaje = "agregado" });
                }
                else
                {
                    return StatusCode(StatusCodes.Status208AlreadyReported, new { mensaje = "Este Grupo ya ha sido creado" });
                }
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });

            }
        }


        [HttpDelete]
        [Route("Eliminar/{usuario}")]
        public IActionResult Eliminar(string usuario)
        {
            try
            {
                int filas = 0;
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("[sp_eliminar_grupos]", conexion);
                    cmd.Parameters.AddWithValue("usuario", usuario);
                    cmd.CommandType = CommandType.StoredProcedure;
                    filas = cmd.ExecuteNonQuery();
                }
                if (filas > 1)
                {
                    return StatusCode(StatusCodes.Status200OK, new { mensaje = "eliminado" });
                }
                else
                {
                    return StatusCode(StatusCodes.Status302Found, new { mensaje = "Este usuario no existe" });
                }
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
                            }
        }
    }
}
