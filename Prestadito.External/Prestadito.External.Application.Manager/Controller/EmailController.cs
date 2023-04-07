using Microsoft.Extensions.Logging;
using Prestadito.External.Application.Dto.Email;
using Prestadito.External.Application.Manager.Interfaces;
using Prestadito.External.Application.Manager.Utilities;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Http;
using Prestadito.External.Infrastructure.Data.Constants;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

namespace Prestadito.External.Application.Manager.Controller
{
    public class EmailController : IEmailController
    {
        private readonly ILogger<EmailController> _logger;
        private readonly IConfiguration _configuration;

        public EmailController(ILogger<EmailController> logger, IServiceScopeFactory factory)
        {
            _logger = logger;
            _configuration = factory.CreateScope().ServiceProvider.GetRequiredService<IConfiguration>();
        }

        public async ValueTask<IResult> SendEmail(EmailRequest request)
        {
            ResponseModel responseModel = new();
            try
            {
                try
                {
                    EmailResponse oEmailResponse = new EmailResponse();
                    oEmailResponse.Destinatarios = new List<string>();
                    oEmailResponse.DestinatariosCC = new List<string>();
                    oEmailResponse.DestinatariosCCO = new List<string>();
                    oEmailResponse.Parametros = new Dictionary<string, string>();
                    oEmailResponse.ListadoColumnas = new Dictionary<string, List<string>>();
                    oEmailResponse.ListadoFilas = new Dictionary<string, List<List<string>>>();

                    string[] correos = request.correo.Split(';');
                    string[] correosCC = request.correocc.Split(';');
                    string[] correosCCO = request.correocco.Split(';');

                    foreach (var item in correos)
                        oEmailResponse.Destinatarios.Add(item);

                    foreach (var item in correosCC)
                        oEmailResponse.DestinatariosCC.Add(item);

                    foreach (var item in correosCCO)
                        oEmailResponse.DestinatariosCCO.Add(item);

                    oEmailResponse.Parametros = request.parametros;

                    try
                    {
                        MailMessage? oMail = new();
                        SmtpClient? oSMTP = new();
                        string error = string.Empty;
                        string resultado = string.Empty;
                        try
                        {
                            //Correos Destino
                            foreach (string item in oEmailResponse.Destinatarios)
                            {
                                try
                                {
                                    if (!string.IsNullOrEmpty(item))
                                    {
                                        var oMailAddres = new MailAddress(item);
                                        oMail.To.Add(oMailAddres);
                                    }
                                }
                                catch (Exception e)
                                {
                                    responseModel.Error = true;
                                    responseModel.Message = $"Error: {e.Message}";
                                }
                            }

                            //Correos CC
                            foreach (string item in oEmailResponse.DestinatariosCC)
                            {
                                try
                                {
                                    if (!string.IsNullOrEmpty(item))
                                    {
                                        var oMailAddres = new MailAddress(item);
                                        oMail.CC.Add(oMailAddres);
                                    }
                                }
                                catch (Exception e)
                                {
                                    responseModel.Error = true;
                                    responseModel.Message = $"Error: {e.Message}";
                                }
                            }

                            //Correos Ocultos
                            foreach (string item in oEmailResponse.DestinatariosCCO)
                            {
                                try
                                {
                                    if (!string.IsNullOrEmpty(item))
                                    {
                                        var oMailAddres = new MailAddress(item);
                                        oMail.Bcc.Add(oMailAddres);
                                    }
                                }
                                catch (Exception e)
                                {
                                    responseModel.Error = true;
                                    responseModel.Message = $"Error: {e.Message}";
                                }
                            }

                            #region Armado de request para envio de correos

                            string bodyMessage = string.Empty;
                            string plantilla = string.Empty;

                            switch (request.tipo)
                            {
                                case 1:
                                    oMail.Subject = ConstantMessages.Email.ASUNTO.BIENVENIDA;
                                    plantilla = ConstantMessages.Email.PLANTILLA.BIENVENIDA;
                                    break;
                                case 2:
                                    oMail.Subject = ConstantMessages.Email.ASUNTO.CAMBIAR_CLAVE;
                                    plantilla = ConstantMessages.Email.PLANTILLA.CAMBIAR_CLAVE;
                                    break;
                                case 3:
                                    oMail.Subject = ConstantMessages.Email.ASUNTO.RECUPERAR_CLAVE;
                                    plantilla = ConstantMessages.Email.PLANTILLA.RECUPERAR_CLAVE;
                                    break;
                            }

                            using (var client = new WebClient())
                            {
                                client.Encoding = System.Text.Encoding.UTF8;
                                bodyMessage = client.DownloadString($"{plantilla}");
                            }
                            string cMessage = string.Empty;
                            if (request.parametros != null)
                                bodyMessage = TagReplace(request.parametros, bodyMessage);

                            oMail.From = new MailAddress(request.correo, _configuration.GetSection("EmailSettings").GetSection("DisplayName").Value);
                            oMail.Body = bodyMessage;
                            oMail.IsBodyHtml = true;
                            oSMTP.Port = Convert.ToInt32(_configuration.GetSection("EmailSettings").GetSection("Port").Value);
                            oSMTP.UseDefaultCredentials = false;
                            oSMTP.Credentials = new NetworkCredential(_configuration.GetSection("EmailSettings").GetSection("CorreoEnvio").Value, _configuration.GetSection("EmailSettings").GetSection("SecureKey").Value);
                            oSMTP.Host = _configuration.GetSection("EmailSettings").GetSection("Host").Value;
                            oSMTP.EnableSsl = Convert.ToBoolean(_configuration.GetSection("EmailSettings").GetSection("SSLEmail").Value);
                            oSMTP.DeliveryMethod = SmtpDeliveryMethod.Network;
                            oSMTP.Timeout = 20000;
                            await oSMTP.SendMailAsync(oMail);
                            responseModel.Error = true;
                            responseModel.Message = "Correo enviado satisfactoriamente";
                            #endregion
                        }
                        catch (Exception e)
                        {
                            _logger.LogError($"Error en método EnviarCorreo: {e}"); 
                            responseModel.Error = true;
                            responseModel.Message = $"Error: {e.Message}";
                        }
                        finally
                        {
                            oMail = null;
                            oSMTP = null;
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.LogError($"Error en método EnviarCorreo: {e}"); 
                        responseModel.Error = true;
                        responseModel.Message = $"Error: {e.Message}";
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError($"Error en método EnviarCorreo: {e}"); responseModel.Error = true;
                    responseModel.Error = true;
                    responseModel.Message = $"Error: {e.Message}";
                }
                _logger.LogInformation("Correo enviado satisfactoriamente");
            }
            catch (Exception e)
            {
                _logger.LogError($"Error en método SendEmail de EmailController: {e}");
                responseModel.Error = true;
                responseModel.Message = $"Error: {e.Message}";
            }
            return Results.Json(responseModel);
        }

        private static string TagReplace(Dictionary<string, string> parametros, string bodyMessage)
        {
            foreach (var replacement in parametros)
            {
                bodyMessage = bodyMessage.Replace($"{{{replacement.Key}}}", replacement.Value);
            }
            return bodyMessage;
        }
    }
}