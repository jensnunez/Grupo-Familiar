1. crear una base de datos puede llevar el nombre que quieran
2. correr script api3 sobre la nueva base de datos.
3. descargar el proyecto del repositorio 
4. favor definir la ruta de conexion a la base de datos en el archivo de conexion appsettings.json
5. debe instalar las dependencias del proyecto:
NewtonsftJson , FluentValitation.aspnetcore, systemdatasqlclient, systemIdentityModel
5. ejecutar api desde visual studio 2022 el net core es 6
6. la aplicacion tiene autenticacion por jwt
7. el primer endpoint es http://localhost:5124/api/Autenticacion/Validar
este no requiere token y simplemente va a validar que el usuario este creado en la tabla usuarios
nota: para los demas endpoint es requerido el token
8. maneja el siguiente crud de usuarios:
http://localhost:5124/api/User/Lista es por get
http://localhost:5124/api/User/auditoria es por get
http://localhost:5124/api/User/Obtener/{correo{ espor get  lleva como parametro el correo
http://localhost:5124/api/User/Guardar es por post el json que se debe enviar es
{
  "username": "string",
  "password": "string",
  "email": "string"
}
http://localhost:5124/api/User/Editar es por put el json que se debe enviar es
{
  "username": "string",
  "password": "string",
  "email": "string"
}
http://localhost:5124/api/User/Eliminar/{usuario} se debe enviar el usuario como parametro

el siguiente crud es para los grupos
http://localhost:5124/api/Grupo/Lista es por get

http://localhost:5124/api/Grupo/Guardar es por post
{
  "usuario": "string",
  "cedula": "string",
  "nombres": "string",
  "apellidos": "string",
  "genero": "string",
  "parentesco": "string",
  "edad": "string",
  "fecha": "string"
}
http://localhost:5124/api/Grupo/Editar es por put
{
  "usuario": "string",
  "cedula": "string",
  "nombres": "string",
  "apellidos": "string",
  "genero": "string",
  "parentesco": "string",
  "edad": "string",
  "fecha": "string"
}
http://localhost:5124/api/Grupo/Eliminar/{usuario} es por delete recibe como parametro el usuario

La aplicacion consta de unos modelos y 3 controladores.
un controlador para validar la autenticacion
un controlador para el crud de usuarios 
un controlador para el crud grupos
la logica de la conexion la maneja directamente el controlador , el contralador inyecta las clases de los modelos
se usa la libreria FluentValitation para las reglas de los modelos.
La autenticacion por token se configura en el program.cc 

Para un detalle muy detallado del codigo favor asignarme fecha sustentacion.

