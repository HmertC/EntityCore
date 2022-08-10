using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace EntyProj
{
    public class ShopContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public static readonly ILoggerFactory MyLogger = LoggerFactory.Create(bulilder => {bulilder.AddConsole(); });

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           optionsBuilder
                .UseLoggerFactory(MyLogger)
                .UseSqlServer(@"Data Source =DESKTOP-C0V3RSM\SQLEXPRESS; Initial Catalog = ShopDb; Integrated Security=true;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductCategory>().HasKey(t => new { t.ProductId, t.CategoryId });

            modelBuilder.Entity<ProductCategory>()
                .HasOne(pc => pc.Product)
                .WithMany(p => p.ProductCategories)
                .HasForeignKey(pc => pc.ProductId);

            modelBuilder.Entity<ProductCategory>()
               .HasOne(pc => pc.Category)
               .WithMany(c => c.ProductCategories)
               .HasForeignKey(pc => pc.CategoryId);


        }
    }

    public class User
    {
        public User()
        {
            this.Addresses = new List<Address>();
        }
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public Customer Customer { get; set; }
        public List<Address> Addresses { get; set; }
    }

    public class Customer
    {
        public int Id { get; set; }
        public string IdentityNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
    }

    public class Supplier
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string TaxNumber { get; set; }
        
    }

    public class Address
    {
        public int Id { get; set; }
        public string Fullname { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }

        public User User { get; set; }
        public int UserId { get; set; }
    }
         
    public class Product
    {

        public int Id { get; set; }
        //[MaxLength(100)]
        //[Required]
        public string Name { get; set; }
        public decimal Price { get; set; }

        //public int CategoryId { get; set; }
        public List<ProductCategory> ProductCategories { get; set; }
    }

    public class Category
    {
        
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ProductCategory> ProductCategories { get; set; }
    }

    public class ProductCategory
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
    public class Order
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public DateTime DateAdded { get; set; }

    }

    class Program
    {
        static void Main(string[] args)
        {

            using (var db = new ShopContext())
            {
                var products = new List<Product>()
                {
                    new Product(){Name =" Samsung M51",Price = 5000},
                    new Product(){Name =" Samsung M52",Price = 5500},
                    new Product(){Name =" Samsung M53",Price = 6000},
                    new Product(){Name =" Samsung M54",Price = 6500},

                };

                db.Products.AddRange(products);

                var categotie = new List<Category>()
                {
                    new Category(){Name="Telefon"},
                    new Category(){Name="Bilgisayar"},
                    new Category(){Name="Elektironik"}
                };

                db.Categories.AddRange(categotie);

                db.SaveChanges();


            }


            //using (var db = new ShopContext())
            //{

            //    //var customer = new Customer()
            //    //{
            //    //    IdentityNumber = "123654",
            //    //    FirstName = "Haydar",
            //    //    LastName = "Haydari",
            //    //    User= db.Users.FirstOrDefault(i => i.Id == 3)
            //    //};

            //    //db.Customers.Add(customer);
            //    //db.SaveChanges();

            //    var user = new User()
            //    {
            //        Username = "Deneme",
            //        Email = "deeneme@gmail.com",
            //        Customer = new Customer()
            //        {
            //            FirstName = "Deneme",
            //            LastName = "Deneme",
            //            IdentityNumber = "12364"
            //        }
            //    };

            //    db.Users.Add(user);
            //    db.SaveChanges();

            //}



            //InsertAddresses();

            //using (var db = new ShopContext())
            //{
            //    var user = db.Users.FirstOrDefault(i => i.Username == "Cabbar");

            //    if (user!=null)
            //    {
            //        user.Addresses = new List<Address>();
            //        user.Addresses.AddRange(
            //            new List<Address>() {
            //            new Address() { Fullname = "Cabbar", Title = "İş Adresi", Body = "Kocaeli" },
            //            new Address() { Fullname = "Cabbar", Title = "İş Adresi 2", Body = "Kocaeli"},
            //            new Address() { Fullname = "Cabbar", Title = "İş Adresi 3", Body = "Kocaeli"}
            //            });
            //    }
            //    db.SaveChanges();
            //}

            

        }

        static void InsertUsers()
        {
            var users = new List<User>()
            {
                new User(){Username="HmertC", Email = "hmc@gmail.com"},
                new User(){Username="Haydar", Email = "haydar@gmail.com"},
                new User(){Username="Cabbar", Email = "cbr@gmail.com"},
                new User(){Username="Rezzak", Email = "rzk@gmail.com"},
            };

            using (var db = new ShopContext())
            {
                db.Users.AddRange(users);
                db.SaveChanges();
            }
        }

        static void InsertAddresses()
        {
            var addresses = new List<Address>()
            {
                new Address(){Fullname="HmertC", Title = "Memleket",Body = "Mardin",UserId = 1},
                new Address(){Fullname="Haydar", Title = "Ev adresi",Body = "Mersin",UserId = 3},
                new Address(){Fullname="Cabbar", Title = "İş Adresi",Body = "Kocaeli",UserId = 2},
                new Address(){Fullname="Samet", Title = "Dükkan",Body = "Maraş",UserId = 4},

            };

            using (var db = new ShopContext())
            {
                db.Addresses.AddRange(addresses);
                db.SaveChanges();
            }
        }


        //static void DeleteProduct(int id)
        //{
        //    using(var db = new ShopContext())
        //    {

        //        var p = new Product() { Id = 3};

        //        //db.Products.Remove(p);
        //        db.Entry(p).State = EntityState.Deleted;
        //        db.SaveChanges();

        //        //var p = db.Products.FirstOrDefault(i => i.Id == id);

        //        //if (p!=null)
        //        //{
        //        //    db.Products.Remove(p);
        //        //    db.SaveChanges();

        //        //    Console.WriteLine("Veri Silindi");
        //        //}
        //    }
        //}
        //static void UpdateProduct()
        //{

        //    using (var db = new ShopContext())
        //    {
        //        var p = db.Products.Where(i => i.Id == 1).FirstOrDefault();

        //        if (p!=null)
        //        {
        //            p.Price = 2400;
        //            db.Products.Update(p);
        //            db.SaveChanges();
        //        }
        //    }


        ////    using(var db = new ShopContext())
        ////    {
        ////        var entity = new Product() { Id = 1 };
        ////        db.Products.Attach(entity);

        ////        entity.Price = 3000;

        ////        db.SaveChanges();
        ////    }




        //            //{
        //    //    var p = db
        //    //        .Products
        //    //        //.AsNoTracking()
        //    //        .Where(i => i.Id == 1)
        //    //        .FirstOrDefault();
        //    //    if (p!=null)
        //    //    {
        //    //        p.Price *= 1.2m;

        //    //        db.SaveChanges();

        //    //        Console.WriteLine("Güncelleme Yapıldı");
        //    //    }
        //    //}
        //}
        //static void GetPrductByName(string name)
        //{
        //    using (var context = new ShopContext())
        //    {
        //        var products = context
        //            .Products.Where(p => p.Name.ToLower().Contains(name.ToLower()))
        //            .Select(p =>
        //            new
        //            {
        //                p.Name,
        //                p.Price
        //            })
        //            .ToList();


        //        foreach (var p in products)
        //        {
        //            Console.WriteLine($"name : {p.Name} price  : {p.Price}");
        //        }



        //    }
        //}
        //static void GetPrductById(int id)
        //{
        //    using (var context = new ShopContext())
        //    {
        //        var result = context
        //            .Products.Where(p => p.Id == id)
        //            .Select(p => 
        //            new
        //            {
        //                p.Name,
        //                p.Price
        //            })
        //            .FirstOrDefault();


        //            Console.WriteLine($"name : {result.Name} price  : {result.Price}");



        //    }
        //}

        //static void GetAllProducts()
        //{
        //    using(var context = new ShopContext())
        //    {
        //        var products = context
        //            .Products
        //            .Select(p => 
        //            new {
        //                p.Name,
        //                p.Price
        //            })
        //            .ToList();

        //        foreach (var p in products)
        //        {
        //            Console.WriteLine($"name : {p.Name} price  : {p.Price}");
        //        }


        //    }
        //}

        //static void AddProducts()
        //{
        //    using (var db = new ShopContext())
        //    {
        //        var products = new List<Product>()
        //        {
        //            new Product {Name = "Samsung M52", Price =6250},
        //            new Product {Name = "Samsung M53", Price =7250},
        //             new Product {Name = "Samsung M54", Price =8250},
        //            new Product {Name = "Samsung M55", Price =9250}
        //        };


        //        //foreach (var p in products)
        //        //{
        //        //    db.Products.Add(p);
        //        //}

        //        //alternatif olarak
        //        db.Products.AddRange(products);

        //        db.SaveChanges();

        //        Console.WriteLine("Veriler Eklendi");
        //    }
        //}

        //static void AddProduct()
        //{
        //    using (var db = new ShopContext())
        //    {

        //        var p = new Product { Name = "Samsung M56", Price = 5500 };
        //        //foreach (var p in products)
        //        //{
        //        //    db.Products.Add(p);
        //        //}

        //        //alternatif olarak
        //        db.Products.Add(p);

        //        db.SaveChanges();

        //        Console.WriteLine("Veriler Eklendi");
        //    }
        //}

    }
}
