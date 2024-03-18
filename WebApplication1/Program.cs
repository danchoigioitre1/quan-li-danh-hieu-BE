using MISA.FresherWeb032023.Controllers.Middlewares;
using MISA.FRESHERWEB032023.BL.Services.EmulationService;
using MISA.FRESHERWEB032023.BL.Services.ExportAndImportService;
using MISA.FRESHERWEB032023.DL.Repository.Emulation;
using MISA.FRESHERWEB032023.DL.Repository.EmulationRepo;
using MISA.FRESHERWEB032023.DL.Repository.ExportImportRepo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(option => option.JsonSerializerOptions.PropertyNamingPolicy = null);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IEmulationRepository, EmulationRepository>();
builder.Services.AddScoped<IEmulationService, EmulationService>();

builder.Services.AddScoped<IExportRepository, ExportRepository>();
builder.Services.AddScoped<IEmulationExportService, EmulationExportService>();

builder.Services.AddScoped<IImportRepository, ImportRepository>();
builder.Services.AddScoped<IEmulationImportService,EmulationImportService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<ExceptionMiddleware>();

app.Run();
