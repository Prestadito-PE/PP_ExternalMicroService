namespace Prestadito.External.API.Endpoints
{
    public static class EndPoints
    {
        readonly static string basePath = "/api";
        public static WebApplication UseExternalEndpoints(this WebApplication app)
        {
            app.UseEmailEndpoints(basePath);

            return app;
        }
    }
}