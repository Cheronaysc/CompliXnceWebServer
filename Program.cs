using AutoGovernance9Web.Backend.Data;
using AutoGovernance9Web.Backend.Models;
using AutoGovernance9Web.Backend.Dtos;
using AutoGovernance9Web.Backend.Services.UserServices;
using AutoGovernance9Web.Components;
using Microsoft.AspNetCore.Connections;
using AutoGovernance9Web.Backend.Services.AssesmentServices;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddScoped<IDbConnectionInterface, SqlConnectionInterface>();
builder.Services.AddScoped<AuthenticationService>();
builder.Services.AddScoped<UserSession>();
builder.Services.AddScoped<AdminService>();
builder.Services.AddScoped<QuestionService>();
builder.Services.AddScoped<AssessmentService>();
builder.Services.AddScoped<EmployeeService>();
builder.Services.AddScoped<AssessmentService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
