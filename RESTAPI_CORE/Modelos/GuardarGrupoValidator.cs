using FluentValidation;
using System.Xml.Linq;

namespace RESTAPI_CORE.Modelos
{
    public class GuardarGrupoValidator : AbstractValidator<GuardarGrupo>
    {
        public GuardarGrupoValidator()
        {

            RuleFor(guardarGrupo => guardarGrupo.cedula).NotNull().NotEmpty();
            RuleFor(guardarGrupo => guardarGrupo.nombres).NotNull().NotEmpty();
            RuleFor(guardarGrupo => guardarGrupo.apellidos).NotNull().NotEmpty().WithMessage("campo requerido por favor");
            RuleFor(guardarGrupo => guardarGrupo.edad).NotNull().NotEmpty().WithMessage("campo requerido por favor");
            RuleFor(guardarGrupo => guardarGrupo.genero).NotNull().NotEmpty();
            RuleFor(guardarGrupo => guardarGrupo.parentesco).NotNull().NotEmpty();
            RuleFor(guardarGrupo => guardarGrupo.usuario).NotNull().NotEmpty();            
            RuleFor(guardarGrupo => guardarGrupo.fecha).NotEmpty().When(guardarGrupo => int.Parse(guardarGrupo.edad) < 18);
        }

        private string IsOver18(string edad)
        {
            if (int.Parse(edad) < 18)
            {
                return "debe registrar la fecha de nacimiento";
            } else
            {
                return "";
            }
            
        }

    }
}