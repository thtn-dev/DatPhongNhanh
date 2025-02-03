namespace DatPhongNhanh.WebApi.Extensions
{
    public static class ConfigPipelines
    {
        public static WebApplication AddPipelines(this WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("DefaultDevCorsPolicy");

            //app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseRouting()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
            return app;
        }
    }
}
