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

    public class UserController : ControllerBase
    {
        private readonly string cadenaSQL;
        public UserController(IConfiguration config)
        {
            cadenaSQL = config.GetConnectionString("CadenaSQL");
        }


        [HttpPost]
        [Route("Lista")]
        public IActionResult Lista()
        {
            List<Usuario> lista = new List<Usuario>();
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_listado_usuarios", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            lista.Add(new Usuario
                            {
                                email = rd["correo"].ToString(),
                                username = rd["usuario"].ToString(), 
                                password = rd["clave"].ToString(),
                            });
                        }
                    }
                }
                return StatusCode(StatusCodes.Status200OK, JsonConvert.SerializeObject(lista) );
            }
            catch (Exception error)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, response = lista });

            }
        }


        [HttpPost]
        [Route("auditoria")]
        public IActionResult Auditoria()
        {
            List<Auditoria> lista = new List<Auditoria>();
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_lista_auditoria", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            lista.Add(new Auditoria
                            {
                                usuario = rd["usuario"].ToString(),
                                fecha = rd["fecha"].ToString(),
                                descripcion = rd["descripcion"].ToString(),
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



        // obtener por id

        [HttpGet]
    
    [Route("Obtener/{correo}")]
    public IActionResult Obtener(string correo)
    {
        List<Usuario> lista = new List<Usuario>();
        Usuario oUsuario = new Usuario();
        try
        {
            using (var conexion = new SqlConnection(cadenaSQL))
            {
                conexion.Open();
                var cmd = new SqlCommand("sp_lista_usuarios", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        lista.Add(new Usuario
                        {
                            email = rd["email"].ToString(),
                            username = rd["username"].ToString(),
                            password = rd["password"].ToString(),
                        });
                    }

                }
            }

                oUsuario = lista.Where(item => item.email == correo).FirstOrDefault();

            return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = oUsuario });
        }
        catch (Exception error)
        {

            return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, response = oUsuario });

        }
    }
        // guardar


        [HttpPost]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody] Usuario objeto)
        {
            try
            {
                int filas = 0;
                using (var conexion = new SqlConnection(cadenaSQL))
                {                   
                    conexion.Open();
                    var cmd = new SqlCommand("[sp_guardar_usuarios]", conexion);
                    cmd.Parameters.AddWithValue("correo", objeto.email);
                    cmd.Parameters.AddWithValue("clave", objeto.password);
                    cmd.Parameters.AddWithValue("usuario", objeto.username);

                    cmd.CommandType = CommandType.StoredProcedure;
                    filas = cmd.ExecuteNonQuery();
                }
                if (filas > 1 )
                {
                    return StatusCode(StatusCodes.Status201Created, new { mensaje = "agregado" });
                } else
                {
                    return StatusCode(StatusCodes.Status208AlreadyReported, new { mensaje = "Este usuario ya ha sido creado" });
                }

               
            }
            catch (Exception error)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });

            }
        }

        [HttpPut]
        [Route("Editar")]
        public IActionResult Editar([FromBody] Usuario objeto)
        {
            try
            {
                int filas = 0;
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("[sp_editar_usuarios]", conexion);
                    cmd.Parameters.AddWithValue("usuario", objeto.username is null ? DBNull.Value : objeto.username);
                    cmd.Parameters.AddWithValue("correo", objeto.email is null ? DBNull.Value : objeto.email);
                    cmd.Parameters.AddWithValue("clave", objeto.password is null ? DBNull.Value : objeto.password);                  
                    cmd.CommandType = CommandType.StoredProcedure;
                    filas = cmd.ExecuteNonQuery();
                }

                if (filas > 1)
                {
                    return StatusCode(StatusCodes.Status201Created, new { mensaje = "actualizado" });
                }
                else
                {
                    return StatusCode(StatusCodes.Status302Found, new { mensaje = "Este usuario no ha sido creado" });
                }
            }
            catch (Exception error)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });

            }
        }

        // eliminar

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
                    var cmd = new SqlCommand("[sp_eliminar_usuarios]", conexion);
                    cmd.Parameters.AddWithValue("usuario", usuario);
                    cmd.CommandType = CommandType.StoredProcedure;
                    filas = cmd.ExecuteNonQuery();
                }
                if (filas > 1)
                {
                    return StatusCode(StatusCodes.Status200OK, new { mensaje = "eliminado" });
                } else
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
