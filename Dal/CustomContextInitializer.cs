using NLog;
using System;
using System.Data.Entity;
using Dal.Model;

namespace Dal
{
    public class CustomContextInitializer : DropCreateDatabaseIfModelChanges<EfDbContext>
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        protected override void Seed(EfDbContext context)
        {
            CustomContextInitializer.logger.Info("Initializing database for first use...");
            Role entity1 = new Role() { Name = "Admin" };
            Role entity2 = new Role()
            {
                Name = "ProductAdmin"
            };
            Role entity3 = new Role() { Name = "MemberAdmin" };
            Role entity4 = new Role() { Name = "Bartender" };
            context.Roles.Add(entity1);
            context.Roles.Add(entity2);
            context.Roles.Add(entity3);
            context.Roles.Add(entity4);
            Member entity5 = new Member()
            {
                FirstName = "Stijn",
                LastName = "De Potter",
                BirthDate = new DateTime?(DateTime.ParseExact("25/05/1988", "dd/MM/yyyy", (IFormatProvider)null)),
                City = "Kontich",
                ZipCode = "2550",
                Address = "Beemdenlaan 27 b4",
                Email = "stijn.depotter@gmail.com",
                TelephoneNr = "0486912764",
                Country = "BE"
            };
            entity5.Roles.Add(entity1);
            entity5.Roles.Add(entity2);
            entity5.Roles.Add(entity3);
            entity5.Roles.Add(entity4);
            MemberCard memberCard = new MemberCard()
            {
                ActiveMember = true,
                Blocked = false,
                CreationTime = DateTime.Now,
                ExpireDate = DateTime.Now + TimeSpan.FromDays(365.0),
                Printed = false,
                SmartCardId = "44aa052f9000"
            };
            entity5.MemberCards.Add(memberCard);
            context.Members.Add(entity5);
            Category entity6 = new Category()
            {
                ImagePath = "drinks.png",
                Name = "Frisdranken",
                Order = 1
            };
            Category entity7 = new Category()
            {
                ImagePath = "beer.png",
                Name = "Bieren",
                Order = 0
            };
            Category entity8 = new Category()
            {
                ImagePath = "food.png",
                Name = "Eten",
                Order = 2
            };
            Category entity9 = new Category()
            {
                ImagePath = "star.png",
                Name = "Andere",
                Order = 3
            };
            context.Categories.Add(entity6);
            context.Categories.Add(entity7);
            context.Categories.Add(entity8);
            context.Categories.Add(entity9);
            TaxCategory entity10 = new TaxCategory()
            {
                Name = "None (VZW)",
                TaxPercentage = 0.0
            };
            context.TaxCategories.Add(entity10);
            Product entity11 = new Product()
            {
                AddedBy = entity5,
                Category = entity6,
                DateAdded = DateTime.Now,
                IsHidden = false,
                TaxCategory = entity10,
                Name = "Coca-Cola",
                PicturePath = "cola.png",
                Price = 1.0,
                Xpos = 0,
                YPos = 0
            };
            Product entity12 = new Product()
            {
                AddedBy = entity5,
                Category = entity6,
                DateAdded = DateTime.Now,
                IsHidden = false,
                TaxCategory = entity10,
                Name = "Coca-Cola Light",
                PicturePath = "colalight.png",
                Price = 1.0,
                Xpos = 0,
                YPos = 1
            };
            Product entity13 = new Product()
            {
                AddedBy = entity5,
                Category = entity6,
                DateAdded = DateTime.Now,
                IsHidden = false,
                TaxCategory = entity10,
                Name = "Coca-Cola Zero",
                PicturePath = "colazero.png",
                Price = 1.0,
                Xpos = 0,
                YPos = 2
            };
            Product entity14 = new Product()
            {
                AddedBy = entity5,
                Category = entity6,
                DateAdded = DateTime.Now,
                IsHidden = false,
                TaxCategory = entity10,
                Name = "Fanta",
                PicturePath = "fanta.png",
                Price = 1.0,
                Xpos = 1,
                YPos = 0
            };
            Product entity15 = new Product()
            {
                AddedBy = entity5,
                Category = entity6,
                DateAdded = DateTime.Now,
                IsHidden = false,
                TaxCategory = entity10,
                Name = "Sprite",
                PicturePath = "sprite.png",
                Price = 1.0,
                Xpos = 1,
                YPos = 1
            };
            Product entity16 = new Product()
            {
                AddedBy = entity5,
                Category = entity7,
                DateAdded = DateTime.Now,
                IsHidden = false,
                TaxCategory = entity10,
                Name = "Maes 25cc",
                PicturePath = "maes.png",
                Price = 1.2,
                Xpos = 0,
                YPos = 0
            };
            Product entity17 = new Product()
            {
                AddedBy = entity5,
                Category = entity7,
                DateAdded = DateTime.Now,
                IsHidden = false,
                TaxCategory = entity10,
                Name = "Maes 33cc",
                PicturePath = "maes.png",
                Price = 1.5,
                Xpos = 0,
                YPos = 1
            };
            Product entity18 = new Product()
            {
                AddedBy = entity5,
                Category = entity7,
                DateAdded = DateTime.Now,
                IsHidden = false,
                TaxCategory = entity10,
                Name = "Maes 50cc",
                PicturePath = "maes.png",
                Price = 2.0,
                Xpos = 0,
                YPos = 2
            };
            Product entity19 = new Product()
            {
                AddedBy = entity5,
                Category = entity7,
                DateAdded = DateTime.Now,
                IsHidden = false,
                TaxCategory = entity10,
                Name = "Duvel",
                PicturePath = "duvel.png",
                Price = 2.0,
                Xpos = 1,
                YPos = 0
            };
            Product entity20 = new Product()
            {
                AddedBy = entity5,
                Category = entity7,
                DateAdded = DateTime.Now,
                IsHidden = false,
                TaxCategory = entity10,
                Name = "Duvel Triple Hop",
                PicturePath = "duveltriplehop.png",
                Price = 2.0,
                Xpos = 1,
                YPos = 1
            };
            Product entity21 = new Product()
            {
                AddedBy = entity5,
                Category = entity7,
                DateAdded = DateTime.Now,
                IsHidden = false,
                TaxCategory = entity10,
                Name = "Hopus",
                PicturePath = "hopus.png",
                Price = 2.0,
                Xpos = 2,
                YPos = 0
            };
            Product entity22 = new Product()
            {
                AddedBy = entity5,
                Category = entity7,
                DateAdded = DateTime.Now,
                IsHidden = false,
                TaxCategory = entity10,
                Name = "Salitos",
                PicturePath = "salitos.png",
                Price = 2.0,
                Xpos = 2,
                YPos = 1
            };
            Product entity23 = new Product()
            {
                AddedBy = entity5,
                Category = entity8,
                DateAdded = DateTime.Now,
                IsHidden = false,
                TaxCategory = entity10,
                Name = "Lays Paprika",
                PicturePath = "layspaprika.png",
                Price = 1.0,
                Xpos = 0,
                YPos = 0
            };
            Product entity24 = new Product()
            {
                AddedBy = entity5,
                Category = entity8,
                DateAdded = DateTime.Now,
                IsHidden = false,
                TaxCategory = entity10,
                Name = "Lays Naturel",
                PicturePath = "laysnatural.png",
                Price = 1.0,
                Xpos = 0,
                YPos = 1
            };
            Product entity25 = new Product()
            {
                AddedBy = entity5,
                Category = entity8,
                DateAdded = DateTime.Now,
                IsHidden = false,
                TaxCategory = entity10,
                Name = "Lays Pickles",
                PicturePath = "layspickles.png",
                Price = 1.0,
                Xpos = 0,
                YPos = 2
            };
            Product entity26 = new Product()
            {
                AddedBy = entity5,
                Category = entity8,
                DateAdded = DateTime.Now,
                IsHidden = false,
                TaxCategory = entity10,
                Name = "Lays Salt & Pepper",
                PicturePath = "layssaltpepper.png",
                Price = 1.0,
                Xpos = 0,
                YPos = 3
            };
            Product entity27 = new Product()
            {
                AddedBy = entity5,
                Category = entity8,
                DateAdded = DateTime.Now,
                IsHidden = false,
                TaxCategory = entity10,
                Name = "Lays Grills",
                PicturePath = "laysgrills.png",
                Price = 1.0,
                Xpos = 0,
                YPos = 4
            };
            Product entity28 = new Product()
            {
                AddedBy = entity5,
                Category = entity8,
                DateAdded = DateTime.Now,
                IsHidden = false,
                TaxCategory = entity10,
                Name = "Aiki Spicy",
                PicturePath = "aikispicy.png",
                Price = 1.0,
                Xpos = 1,
                YPos = 0
            };
            Product entity29 = new Product()
            {
                AddedBy = entity5,
                Category = entity8,
                DateAdded = DateTime.Now,
                IsHidden = false,
                TaxCategory = entity10,
                Name = "Aiki BBQ",
                PicturePath = "aikibbq.png",
                Price = 1.0,
                Xpos = 1,
                YPos = 1
            };
            Product entity30 = new Product()
            {
                AddedBy = entity5,
                Category = entity8,
                DateAdded = DateTime.Now,
                IsHidden = false,
                TaxCategory = entity10,
                Name = "Aiki Chicken",
                PicturePath = "aikichicken.png",
                Price = 1.0,
                Xpos = 1,
                YPos = 2
            };
            Product entity31 = new Product()
            {
                AddedBy = entity5,
                Category = entity8,
                DateAdded = DateTime.Now,
                IsHidden = false,
                TaxCategory = entity10,
                Name = "Aiki Chinese",
                PicturePath = "aikichinese.png",
                Price = 1.0,
                Xpos = 1,
                YPos = 3
            };
            Product entity32 = new Product()
            {
                AddedBy = entity5,
                Category = entity8,
                DateAdded = DateTime.Now,
                IsHidden = false,
                TaxCategory = entity10,
                Name = "Aiki Curry",
                PicturePath = "aikicurry.png",
                Price = 1.0,
                Xpos = 1,
                YPos = 4
            };
            Product entity33 = new Product()
            {
                AddedBy = entity5,
                Category = entity8,
                DateAdded = DateTime.Now,
                IsHidden = false,
                TaxCategory = entity10,
                Name = "CracANut",
                PicturePath = "cracanut.png",
                Price = 1.0,
                Xpos = 2,
                YPos = 0
            };
            Product entity34 = new Product()
            {
                AddedBy = entity5,
                Category = entity9,
                DateAdded = DateTime.Now,
                IsHidden = false,
                TaxCategory = entity10,
                Name = "Steunkaart",
                PicturePath = (string)null,
                Price = 5.0,
                Xpos = 0,
                YPos = 0
            };
            context.Products.Add(entity11);
            context.Products.Add(entity12);
            context.Products.Add(entity13);
            context.Products.Add(entity14);
            context.Products.Add(entity15);
            context.Products.Add(entity16);
            context.Products.Add(entity17);
            context.Products.Add(entity18);
            context.Products.Add(entity19);
            context.Products.Add(entity20);
            context.Products.Add(entity21);
            context.Products.Add(entity22);
            context.Products.Add(entity23);
            context.Products.Add(entity24);
            context.Products.Add(entity25);
            context.Products.Add(entity26);
            context.Products.Add(entity27);
            context.Products.Add(entity28);
            context.Products.Add(entity29);
            context.Products.Add(entity30);
            context.Products.Add(entity31);
            context.Products.Add(entity32);
            context.Products.Add(entity33);
            context.Products.Add(entity34);
            CustomContextInitializer.logger.Info("Initializing database completed");
        }
    }
}