using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace BulkInsertDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new DemoContext())
            {
                var testUsersPackCount = 10;
                var addRangeList = new List<User>();

                Console.WriteLine($"Users, which were added by AddRange: 0 - 9");

                for (var i = 0; i < testUsersPackCount; i++)
                {
                    addRangeList.Add(new User {Name = $"name_{i}", Surname = $"surname_{i}", Country = $"country_{i}", Address = $"address_{i}", City = $"city_{i}" });
                }

                context.Users.AddRange(addRangeList);
                context.SaveChanges();


                var bulkInsertedList = new List<User>();

                Console.WriteLine($"Users, which were added by BulkInsert: 10 - 19");

                for (var i = testUsersPackCount; i < testUsersPackCount + testUsersPackCount; i++)
                {
                    bulkInsertedList.Add(new User { Name = $"name_{i}", Surname = $"surname_{i}", Country = $"country_{i}", Address = $"address_{i}", City = $"city_{i}" });
                }

                context.BulkInsert(bulkInsertedList, SqlBulkCopyOptions.Default);
                context.SaveChanges();

                Console.WriteLine();

                Console.WriteLine("Users, found by context.Users.ToList():");
                var efFetchedDbList = context.Users.ToList();
                efFetchedDbList.ForEach(i => Console.WriteLine($"User Data: {i.Name} {i.Surname} {i.Country ?? "NULL"} {i.City ?? "NULL"} {i.Address ?? "NULL"}"));
                Console.WriteLine();

                Console.WriteLine("What actually is in database (Data, fetcehd from Users and UserAddresses separately and combined together):");
                var userTableResults = context.Database.SqlQuery<UserPart>("select * from part.Users").ToList<UserPart>();
                var userAddressesTableResults = context.Database.SqlQuery<UserAddressPart>("select * from part.UserAddresses").ToList<UserAddressPart>();

                var actualDbList = new List<User>();
                userTableResults.ForEach(user => 
                {
                    var appropriateAddress = userAddressesTableResults.FirstOrDefault(a => a.Id == user.Id);

                    actualDbList.Add(new User
                    { 
                        Id = user.Id,
                        Name = user.Name,
                        Surname = user.Surname,
                        Address = appropriateAddress?.Address ?? null,
                        City = appropriateAddress?.City ?? null,
                        Country = appropriateAddress?.Country ?? null,
                    });
                });

                actualDbList.ForEach(i => Console.WriteLine($"User Data: {i.Name} {i.Surname} {i.Country ?? "NULL"} {i.City ?? "NULL"} {i.Address ?? "NULL"}"));


                context.Users.RemoveRange(efFetchedDbList);
                context.SaveChanges();
                context.Database.ExecuteSqlCommand("delete from part.Users");
            }
        }
    }
}
