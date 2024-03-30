using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = int.MaxValue; // or your desired limit
});

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

app.UseRouting();

app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.MapControllerRoute(
    name: "CreateAvatar",
    pattern: "Avatar/CreateAvatarAsync",
    defaults: new {controller = "Avatar", action = "CreateAvatarAsync" });


app.Run();
