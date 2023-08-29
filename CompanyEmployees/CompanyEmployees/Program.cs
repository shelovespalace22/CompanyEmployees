using CompanyEmployees.Extensions;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureCors(); 
builder.Services.ConfigureIISIntegration();



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment()) 
    app.UseDeveloperExceptionPage(); 
else 
    app.UseHsts();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles(); 
app.UseForwardedHeaders(new ForwardedHeadersOptions
{ 
    ForwardedHeaders = ForwardedHeaders.All 
});
app.UseCors("CorsPolicy");

app.UseAuthorization();

//app.Use(async (context, next) => 
//{
//    Console.WriteLine($"Logic before executing the next delegate in the Use method");
    
//    await next.Invoke();
    
//    Console.WriteLine($"Logic after executing the next delegate in the Use method"); 
//});

//app.Run(async context => {

//    Console.WriteLine($"Writing the response to the client in the Run method");

//    await context.Response.WriteAsync("Hello from the middleware component.");

//});

app.MapControllers();

app.Run();
