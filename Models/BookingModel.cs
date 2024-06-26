﻿
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MovieMate.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace MovieMate.Models
{
    public class BookingModel
    {
        [Key]
        public int BookingId { get; set; }

        public DateTime? BookedAt { get; set; } = DateTime.Now;

        public virtual ShowModel Show { get; set; }
        [ForeignKey("Show")] public int ShowId { get; set; }




        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }





    }

    public static class BookingModelEndpoints
    {


        [Authorize(Roles = "Admin,Customer")]
        public static void MapBookingModelEndpoints(this IEndpointRouteBuilder routes)
        {
            routes.MapGet("/api/BookingModel", async (ApplicationDbContext db) =>
            {
                return await db.Bookings.ToListAsync();
            })
            .WithName("GetAllBookingModels");

            routes.MapGet("/api/BookingModel/{id}", async (int BookingId, ApplicationDbContext db) =>
            {
                return await db.Bookings.FindAsync(BookingId)
                    is BookingModel model
                        ? Results.Ok(model)
                        : Results.NotFound();
            })
            .WithName("GetBookingModelById");

            routes.MapPut("/api/BookingModel/{id}", async (int BookingId, BookingModel bookingModel, ApplicationDbContext db) =>
            {
                var foundModel = await db.Bookings.FindAsync(BookingId);

                if (foundModel is null)
                {
                    return Results.NotFound();
                }

                db.Update(bookingModel);

                await db.SaveChangesAsync();

                return Results.NoContent();
            })
            .WithName("UpdateBookingModel");

            routes.MapPost("/api/BookingModel/", async (BookingModel bookingModel, ApplicationDbContext db) =>
            {

                // Get data from Identity duynamically
                // bookingModel.UserId = this.User.GetUserId();

                db.Bookings.Add(bookingModel);
                await db.SaveChangesAsync();
                return Results.Created($"/BookingModels/{bookingModel.BookingId}", bookingModel);
            })
            .WithName("CreateBookingModel");


            routes.MapDelete("/api/BookingModel/{id}", async (int BookingId, ApplicationDbContext db) =>
            {
                Console.WriteLine("Deleting"+BookingId);
                if (await db.Bookings.FindAsync(BookingId) is BookingModel bookingModel)
                {
                    db.Bookings.Remove(bookingModel);
                    await db.SaveChangesAsync();
                    return Results.Ok(bookingModel);
                }

                Console.WriteLine("Booking not found");
                return Results.NotFound();
            })
            .WithName("DeleteBookingModel");
        }
    }



}