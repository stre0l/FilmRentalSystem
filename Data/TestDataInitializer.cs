using System;
using System.Linq;
using FilmRentalSystem.Models;

namespace FilmRentalSystem.Data
{
    public static class TestDataInitializer
    {
        public static void InitializeTestData(AppDbContext context)
        {
            if (context.Cinemas.Any()) return;

            var supplier1 = new Supplier
            {
                Name = "Кинопрокат ООО",
                LegalAddress = "г. Москва, ул. Киношная, д. 1",
                Bank = "Сбербанк",
                AccountNumber = "40702810500000012345",
                INN = "7712345678"
            };

            var supplier2 = new Supplier
            {
                Name = "ФильмДистрибьютор",
                LegalAddress = "г. Санкт-Петербург, Невский пр., д. 100",
                Bank = "ВТБ",
                AccountNumber = "40702810600000054321",
                INN = "7812345678"
            };

            context.Suppliers.AddRange(supplier1, supplier2);
            context.SaveChanges();

            var cinema1 = new Cinema
            {
                Name = "Киномакс",
                Address = "г. Москва, ул. Пушкина, д. 10",
                Phone = "+7(495)123-45-67",
                SeatCount = 300,
                Director = "Иванов И.И.",
                Owner = "ООО Киносети",
                Bank = "Альфа-Банк",
                AccountNumber = "40817810500000011111",
                INN = "7701234567"
            };

            var cinema2 = new Cinema
            {
                Name = "Синема Парк",
                Address = "г. Москва, ТРЦ Авиапарк",
                Phone = "+7(495)765-43-21",
                SeatCount = 500,
                Director = "Петров П.П.",
                Owner = "Петров П.П.",
                Bank = "Тинькофф",
                AccountNumber = "40817810500000022222",
                INN = "7707654321"
            };

            context.Cinemas.AddRange(cinema1, cinema2);
            context.SaveChanges();

            var movie1 = new Movie
            {
                Title = "Матрица",
                Category = "Фантастика",
                Screenwriter = "Лана и Лилли Вачовски",
                Director = "Лана и Лилли Вачовски",
                ProductionCompany = "Warner Bros.",
                ReleaseYear = 1999,
                PurchaseCost = 50000,
                SupplierId = supplier1.Id
            };

            var movie2 = new Movie
            {
                Title = "Крепкий орешек",
                Category = "Боевик",
                Screenwriter = "Джон МакТирнан",
                Director = "Джон МакТирнан",
                ProductionCompany = "20th Century Fox",
                ReleaseYear = 1988,
                PurchaseCost = 30000,
                SupplierId = supplier1.Id
            };

            var movie3 = new Movie
            {
                Title = "Иван Васильевич меняет профессию",
                Category = "Комедия",
                Screenwriter = "Леонид Гайдай",
                Director = "Леонид Гайдай",
                ProductionCompany = "Мосфильм",
                ReleaseYear = 1973,
                PurchaseCost = 20000,
                SupplierId = supplier2.Id
            };

            context.Movies.AddRange(movie1, movie2, movie3);
            context.SaveChanges();

            var rental1 = new Rental
            {
                MovieId = movie1.Id,
                CinemaId = cinema1.Id,
                StartDate = DateTime.Now.AddDays(-10),
                EndDate = DateTime.Now.AddDays(5),
                RentalFee = 15000,
                PenaltyFee = 0,
                IsReturned = false
            };

            var rental2 = new Rental
            {
                MovieId = movie2.Id,
                CinemaId = cinema2.Id,
                StartDate = DateTime.Now.AddDays(-20),
                EndDate = DateTime.Now.AddDays(-5),
                RentalFee = 10000,
                PenaltyFee = 500,
                IsReturned = true
            };

            var rental3 = new Rental
            {
                MovieId = movie3.Id,
                CinemaId = cinema1.Id,
                StartDate = DateTime.Now.AddDays(-2),
                EndDate = DateTime.Now.AddDays(12),
                RentalFee = 8000,
                PenaltyFee = 0,
                IsReturned = false
            };

            context.Rentals.AddRange(rental1, rental2, rental3);
            context.SaveChanges();
        }
    }
}
