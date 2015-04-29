using System;
using System.Diagnostics;
using System.Linq;

namespace DALSample
{
    class Program
    {
        //execute the db.sql first and edit the connection string in App.config
        static void Main()
        {
            //edit number f5
            const int number = 10;

            //PropertyInfosStorage.RegisterActionForEachType(HyperTypeDescriptionProvider.Add);

            IHumanRepo repo = new HumanRepo(new ConnectionFactory());
            repo.DeleteAll();
 
            var w = new Stopwatch();
            w.Start();
            for (var i = 0; i < number; i++)
            {
                repo.Insert(new Human { FirstName = "Danny", LastName = "Ocean", DateRegistered = DateTime.Today.AddDays(-2342) });
                repo.Insert(new Human { FirstName = "Rusty", LastName = "Ryan", DateRegistered = DateTime.Today.AddDays(-3423) });
                repo.Insert(new Human { FirstName = "Frank", LastName = "Catton", DateRegistered = DateTime.Today.AddDays(-132) });
                repo.Insert(new Human { FirstName = "Reuben", LastName = "Tishkoff", DateRegistered = DateTime.Today.AddDays(-1231) });
                repo.Insert(new Human { FirstName = "Virgil", LastName = "Malloy", DateRegistered = DateTime.Today.AddDays(-123) });
                repo.Insert(new Human { FirstName = "Turk", LastName = "Malloy", DateRegistered = DateTime.Today.AddDays(-123) });
                repo.Insert(new Human { FirstName = "Livingston", LastName = "Dell", DateRegistered = DateTime.Today.AddDays(-123) });
                repo.Insert(new Human { FirstName = "Basher", LastName = "Tarr", DateRegistered = DateTime.Today.AddDays(-123) });
                repo.Insert(new Human { FirstName = "Yen", LastName = "The Amazing", DateRegistered = DateTime.Today.AddDays(-123) });
                repo.Insert(new Human { FirstName = "Saul", LastName = "Bloom", DateRegistered = DateTime.Today.AddDays(-123) });
                repo.Insert(new Human { FirstName = "Linus", LastName = "Caldwell", DateRegistered = DateTime.Today.AddDays(-123) });

            }
            w.Stop();
            Console.WriteLine("inserted {0} rows in Db:", 11 * number);
            Console.WriteLine("inserts duration: {0}", w.Elapsed);
            w.Reset();
            w.Start();
            var all = repo.GetAll().ToList();
            w.Stop();
            Console.WriteLine("get all duration {0}", w.Elapsed);
            Console.ReadKey();

            foreach (var h in all)
                Write(h);


            Console.WriteLine("");
            Console.WriteLine("get where LastName = Ocean and FirstName = Danny");
            Console.ReadKey();

            foreach (var human in repo.GetWhere(new { LastName = "Ocean", FirstName = "Danny" }))
                Write(human);

            Console.WriteLine("");
            Console.WriteLine("update FirstName = The Idea and LastName = Man where LastName = Ocean and FirstName = Danny");
            Console.ReadKey();
            Console.WriteLine("rows affected {0}",
                repo.UpdateWhatWhere(new { LastName = "Man", FirstName = "The Idea" }, new { LastName = "Ocean", FirstName = "Danny" }));

            Console.WriteLine("Count where LastName = Malloy");
            Console.ReadKey();

            Console.WriteLine("result: {0}", repo.CountWhere(new { LastName = "Malloy" }));

            Console.WriteLine("get paged:");
            Console.ReadKey();
            var pageable = repo.GetPageable(1, 5);

            for (var i = 1; i <= pageable.PageCount; i++)
            {
                Console.WriteLine("page " + i);
                foreach (var h in repo.GetPageable(i, 5).Page) Write(h);
                Console.ReadKey();
            }
        }

        private static void Write(Human h)
        {
            Console.WriteLine("{0,-3} {1,-12} {2,-12} {3}", h.Id, h.FirstName, h.LastName,
                                      h.DateRegistered.ToShortDateString());
        }
    }
}
