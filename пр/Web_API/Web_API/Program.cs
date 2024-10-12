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
    // �������� ������������ �� id
    Hotel? hotel = await db.Hotel.FirstOrDefaultAsync(u => u.Id == id);

    // ���� �� ������, ���������� ��������� ��� � ��������� �� ������
    if (hotel == null) return Results.NotFound(new { message = "����� �� ������" });

    // ���� ������������ ������, ���������� ���
    return Results.Json(hotel);
});

app.MapDelete("/api/hotels/{id:int}", async (int id, ApplicationDbContext db) =>
{
    // �������� ������������ �� id
    Hotel? hotel = await db.Hotel.FirstOrDefaultAsync(u => u.Id == id);

    // ���� �� ������, ���������� ��������� ��� � ��������� �� ������
    if (hotel == null) return Results.NotFound(new { message = "����� �� ������" });

    // ���� ������������ ������, ������� ���
    db.Hotel.Remove(hotel);
    await db.SaveChangesAsync();
    return Results.Json(hotel);
});

app.MapPost("/api/hotels", async (Hotel hotel, ApplicationDbContext db) =>
{
    // ��������� ������������ � ������
    await db.Hotel.AddAsync(hotel);
    await db.SaveChangesAsync();
    return hotel;
});

app.MapPut("/api/hotels", async (Hotel HotelData, ApplicationDbContext db) =>
{
    // �������� ������������ �� id
    var hotel = await db.Hotel.FirstOrDefaultAsync(u => u.Id == HotelData.Id);

    // ���� �� ������, ���������� ��������� ��� � ��������� �� ������
    if (hotel == null) return Results.NotFound(new { message = "����� �� ������" });

    // ���� ������������ ������, �������� ��� ������ � ���������� ������� �������
    hotel.Name = HotelData.Name;
    hotel.CountOfStars = HotelData.CountOfStars;
    hotel.CountryCode = HotelData.CountryCode;
    await db.SaveChangesAsync();
    return Results.Json(hotel);
});



app.Run();
