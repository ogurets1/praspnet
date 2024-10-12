using Microsoft.EntityFrameworkCore;
using Web_API.Context;
using Web_API.Models;

var builder = WebApplication.CreateBuilder(args);
string connection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connection));

var app = builder.Build();
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/api/hotels", async (ApplicationDbContext db) => await db.Hotel.ToListAsync());
app.MapGet("/api/hotels/{id:int}", async (int id, ApplicationDbContext db) =>
{
    // получаем пользователя по id
    Hotel? hotel = await db.Hotel.FirstOrDefaultAsync(u => u.Id == id);

    // если не найден, отправляем статусный код и сообщение об ошибке
    if (hotel == null) return Results.NotFound(new { message = "Отель не найден" });

    // если пользователь найден, отправляем его
    return Results.Json(hotel);
});

app.MapDelete("/api/hotels/{id:int}", async (int id, ApplicationDbContext db) =>
{
    // получаем пользователя по id
    Hotel? hotel = await db.Hotel.FirstOrDefaultAsync(u => u.Id == id);

    // если не найден, отправляем статусный код и сообщение об ошибке
    if (hotel == null) return Results.NotFound(new { message = "Отель не найден" });

    // если пользователь найден, удаляем его
    db.Hotel.Remove(hotel);
    await db.SaveChangesAsync();
    return Results.Json(hotel);
});

app.MapPost("/api/hotels", async (Hotel hotel, ApplicationDbContext db) =>
{
    // добавляем пользователя в массив
    await db.Hotel.AddAsync(hotel);
    await db.SaveChangesAsync();
    return hotel;
});

app.MapPut("/api/hotels", async (Hotel HotelData, ApplicationDbContext db) =>
{
    // получаем пользователя по id
    var hotel = await db.Hotel.FirstOrDefaultAsync(u => u.Id == HotelData.Id);

    // если не найден, отправляем статусный код и сообщение об ошибке
    if (hotel == null) return Results.NotFound(new { message = "Отель не найден" });

    // если пользователь найден, изменяем его данные и отправляем обратно клиенту
    hotel.Name = HotelData.Name;
    hotel.CountOfStars = HotelData.CountOfStars;
    hotel.CountryCode = HotelData.CountryCode;
    await db.SaveChangesAsync();
    return Results.Json(hotel);
});



app.Run();
