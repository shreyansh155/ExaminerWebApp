using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
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

// Add services to the container.
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddAutoMapper(typeof(ExaminerProfile));
builder.Services.AddAutoMapper(typeof(ExaminerTypeProfile));

builder.Services.AddControllersWithViews();
builder.Services.AddKendo();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddTransient<IExaminerRepository, ExaminerRepository>();
builder.Services.AddTransient<IExaminerTypeRepository, ExaminerTypeRepository>();
builder.Services.AddTransient<IApplicantRepository, ApplicantRepository>();
builder.Services.AddTransient<IApplicantTypeRepository, ApplicantTypeRepository>();
builder.Services.AddTransient<IApplicationTypeTemplateRepository, ApplicationTypeTemplateRepository>();

// Register services
builder.Services.AddScoped<IApplicantService, ApplicantService>();
builder.Services.AddScoped<IApplicantTypeService, ApplicantTypeService>();
builder.Services.AddScoped<IExaminerService, ExaminerService>();
builder.Services.AddScoped<IExaminerTypeService, ExaminerTypeService>();
builder.Services.AddScoped<IApplicationTypeService, ApplicationTypeService>();

builder.Services.AddDataProtection();
builder.Services.AddNotyf(config => { config.DurationInSeconds = 10; config.IsDismissable = true; config.Position = NotyfPosition.TopRight; config.HasRippleEffect = true; });

var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseNotyf();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
