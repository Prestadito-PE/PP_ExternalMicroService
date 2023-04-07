namespace Prestadito.External.Application.Dto.Email
{
    public class EmailRequest
    {
        public string correo { get; set; }
        public string correocc { get; set; }
        public string correocco { get; set; }
        public Dictionary<string, string> parametros { get; set; }
        public int tipo { get; set; }                   //1 = Confirmar Correo en registro      //2 = Cambio de Contraseña       3 = Recuperar Contraseña
    }
}