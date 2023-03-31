using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


using RESTAPI_CORE.Modelos;

using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace RESTAPI_CORE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticacionController : ControllerBase
    {
        private readonly string secretKey;
        private readonly string cadenaSQL;
        public AutenticacionController(IConfiguration config) {
            secretKey = config.GetSection("settings").GetSection("secretKey").ToString();
            cadenaSQL = config.GetConnectionString("CadenaSQL");
        }

      


        [HttpPost]
        [Route("Validar")]
        public IActionResult Validar([FromBody] Login request) {

         
           
                //if (request != null && !string.IsNullOrWhiteSpace(request.correo) && !string.IsNullOrWhiteSpace(request.clave))
                //{
                    using (var conexion = new SqlConnection(cadenaSQL))
                    {
                        conexion.Open();
                        SqlCommand cmd = new SqlCommand("[sp_login_usuario]", conexion);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@usuario", request.username);
                        cmd.Parameters.AddWithValue("@clave", request.password);                      
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            var keyBytes = Encoding.ASCII.GetBytes(secretKey);
                            var claims = new ClaimsIdentity();
                            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, request.username));

                            var tokenDescriptor = new SecurityTokenDescriptor
                            {
                                Subject = claims,
                                Expires = DateTime.UtcNow.AddMinutes(5000),
                                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
                            };

                            var tokenHandler = new JwtSecurityTokenHandler();
                            var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

                            string tokencreado = tokenHandler.WriteToken(tokenConfig);


                            return StatusCode(StatusCodes.Status200OK, new { token = tokencreado });

                        }
                        else
                        {
                            return StatusCode(StatusCodes.Status401Unauthorized, new { token = "" });
                        }
                    }
                //}
                
            
            

            //






            
            //if (request.correo == "c@gmail.com" && request.clave == "123")
            //{

            //    var keyBytes = Encoding.ASCII.GetBytes(secretKey);
            //    var claims = new ClaimsIdentity();
            //    claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, request.correo));

            //    var tokenDescriptor = new SecurityTokenDescriptor
            //    {
            //        Subject = claims,
            //        Expires = DateTime.UtcNow.AddMinutes(5),
            //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
            //    };

            //    var tokenHandler = new JwtSecurityTokenHandler();
            //    var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

            //    string tokencreado = tokenHandler.WriteToken(tokenConfig);


            //    return StatusCode(StatusCodes.Status200OK, new { token = tokencreado });

            //}
            //else {

            //    return StatusCode(StatusCodes.Status401Unauthorized, new { token = "" });
            //}

            
            
        }

        

    }
}
