using Microsoft.AspNetCore.Mvc;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/reserve/add", ( [FromBody] ReserveRoom reserve ) => {
    // body ‚©‚ç json •¶š—ñ‚ğ“Ç‚İ‚Ş
    Console.WriteLine("called /reserve/add");
    return $"{reserve.date} ‚Ì‰ï‹cº‚ğ—\–ñ‚µ‚Ü‚µ‚½B";

})
.WithName("ReserveRoom")
.WithOpenApi();

app.Run();

class ReserveRoom
{
    public DateTime date { get; set; }
    public string start_time { get; set; }
    public string end_time { get; set; }
    public string room { get; set; }
    public string user { get; set; }

}
