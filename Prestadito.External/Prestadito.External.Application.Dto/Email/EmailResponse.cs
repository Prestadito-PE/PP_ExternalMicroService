using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prestadito.External.Application.Dto.Email
{
    public class EmailResponse
    {
        public List<string> Destinatarios { get; set; }
        public List<string> DestinatariosCC { get; set; }
        public List<string> DestinatariosCCO { get; set; }
        public string Asunto { get; set; }
        public string Correo { get; set; }
        public string Contrasena { get; set; }
        public string DisplayName { get; set; }
        public Dictionary<string, string> Parametros { get; set; }
        public Dictionary<string, List<string>> ListadoColumnas { get; set; }
        public Dictionary<string, List<List<string>>> ListadoFilas { get; set; }
        public string Plantilla { get; set; }
        public Dictionary<string, byte[]> Attachments { get; set; }
    }
}
