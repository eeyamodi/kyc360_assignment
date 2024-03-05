using ky360datawebapi.models;

namespace ky360datawebapi
{
    public class EntityDataGenerator
    {
        private static readonly Random random = new();
        private static bool GenerateBool()
        {
            return random.Next(0, 2) == 1;
        }

        private static string GenerateGender()
        {
            string[] genders = ["Male", "Female", "Other"];
            return genders[random.Next(genders.Length)];
        }

        private static string GenerateString(int length)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private static DateTime GenerateDateTime()
        {
            DateTime start = new(1995, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(random.Next(range));
        }

        public static Address GenerateAddress()
        {

            Address address = new()
            {
                AddressLine = GenerateString(random.Next(40, 50)),
                City = GenerateString(random.Next(10, 15)),
                Country = GenerateString(random.Next(5, 30))
            };
            return address;
        }

        public static Date GenerateDate()
        {
            Date date = new()
            {
                DateType = GenerateString(random.Next(5, 10)),
                date = GenerateDateTime()
            };
            return date;
        }

        public static Name GenerateName()
        {
            Name name = new()
            {
                FirstName = GenerateString(random.Next(5, 10)),
                MiddleName = GenerateString(random.Next(5, 10)),
                Surname = GenerateString(random.Next(5, 10))
            };
            return name;
        }

        public static Entity GenerateEntity()
        {
            List<Address> addresses = new();
            for (int i = 0; i < random.Next(1, 5); i++)
            {
                addresses.Add(GenerateAddress());
            }

            List<Date> dates = new();
            for (int i = 0; i < random.Next(1, 5); i++)
            {
                dates.Add(GenerateDate());
            }

            List<Name> names = new();
            for (int i = 0; i < random.Next(1, 5); i++)
            {
                names.Add(GenerateName());
            }

            Entity entity = new()
            {
                Addresses = addresses,
                Dates = dates,
                Deceased = GenerateBool(),
                Gender = GenerateGender(),
                Id = Guid.NewGuid().ToString(),
                Names = names
            };

            return entity;
        }
    }
}