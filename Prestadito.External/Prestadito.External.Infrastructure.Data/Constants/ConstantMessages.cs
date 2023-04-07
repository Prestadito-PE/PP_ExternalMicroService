using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prestadito.External.Infrastructure.Data.Constants
{
    public class ConstantMessages
    {
        #region Parametros Email
        public class Email
        {
            public class ASUNTO
            {
                public const string BIENVENIDA = "Confirmación de Correo Electrónico";
                public const string CAMBIAR_CLAVE = "Cambio de contraseña";
                public const string RECUPERAR_CLAVE = "Recuperación de contraseña";
            }

            public class PLANTILLA
            {
                public const string BIENVENIDA = @"Content\Bienvenida.html";
                public const string CAMBIAR_CLAVE = @"Content\CambiarClave.html";
                public const string RECUPERAR_CLAVE = @"Content\RecuperarClave.html";
            }
        }
        #endregion
    }
}
