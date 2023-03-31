using System.ComponentModel.DataAnnotations;

namespace RESTAPI_CORE.Modelos
{
    public class GuardarGrupo
    {
        [Required]
        public string usuario { get; set; }
        [Required]
        public string cedula { get; set; }
        [Required]
        public string nombres { get; set; }
        [Required]
        public string apellidos { get; set; }
        public string genero { get; set; }
        public string parentesco { get; set; }
        [Required]
        public string edad { get; set; }

        
        public string fecha { 
            get; set; 
        }

    }
}
