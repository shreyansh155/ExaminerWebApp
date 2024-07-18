using ExaminerWebApp.Composition.MappingProfile;
using ExaminerWebApp.Repository.DataContext;
using ExaminerWebApp.Repository.Implementation;
using ExaminerWebApp.Repository.Interface;
using ExaminerWebApp.Service.Implementation;
using ExaminerWebApp.Service.Interface;
using Microsoft.EntityFrameworkCore;
using PracticeWebApp.Composition.MappingProfile;
using PracticeWebApp.Service.Implementation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddAutoMapper(typeof(ApplicantProfile));
builder.Services.AddAutoMapper(typeof(ExaminerProfile));
builder.Services.AddAutoMapper(typeof(ExaminerTypeProfile));

builder.Services.AddControllersWithViews().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
builder.Services.AddKendo();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//Registered Repositories
builder.Services.AddTransient<IExaminerRepository, ExaminerRepository>();
builder.Services.AddTransient<IExaminerTypeRepository, ExaminerTypeRepository>();
builder.Services.AddTransient<IApplicantRepository, ApplicantRepository>();
builder.Services.AddTransient<IApplicantTypeRepository, ApplicantTypeRepository>();
builder.Services.AddTransient<IApplicationTypeTemplateRepository, ApplicationTypeTemplateRepository>();
builder.Services.AddTransient<IPhaseRepository, PhaseRepository>();
builder.Services.AddTransient<IStepRepository, StepRepository>();
builder.Services.AddTransient<ITemplatePhaseRepository, TemplatePhaseRepository>();
builder.Services.AddTransient<IPhaseStepRepository, PhaseStepRepository>();

// Registered services
builder.Services.AddScoped<IApplicantService, ApplicantService>();
builder.Services.AddScoped<IApplicantTypeService, ApplicantTypeService>();
builder.Services.AddScoped<IExaminerService, ExaminerService>();
builder.Services.AddScoped<IExaminerTypeService, ExaminerTypeService>();
builder.Services.AddScoped<IApplicationTypeService, ApplicationTypeService>();
builder.Services.AddScoped<IPhaseService, PhaseService>();
builder.Services.AddScoped<IStepService, StepService>();
builder.Services.AddScoped<ITemplatePhaseService, TemplatePhaseService>();

builder.Services.AddDataProtection();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();