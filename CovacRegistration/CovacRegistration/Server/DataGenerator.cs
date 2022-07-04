using CovacRegistration.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace CovacRegistration.Server
{
    public class DataGenerator
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new CovacRegistrationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<CovacRegistrationDbContext>>()))
            {
                // Look for any vaccines.
                if (context.Vaccines.Any())
                {
                    return; // Data was already seeded
                }

                context.Vaccines.AddRange(
                    new Vaccine()
                    {
                        CountryOfOrigin = "United Kingdom",
                        WeeksInterval = 6,
                        Name = "AstraZeneca"
                    },
                    new Vaccine()
                    {
                        CountryOfOrigin = "Germany",
                        WeeksInterval = 3,
                        Name = "Pfizer-BioNTech"
                    },
                    new Vaccine()
                    {
                        CountryOfOrigin = "United States",
                        WeeksInterval = 5,
                        Name = "Moderna"
                    },
                    new Vaccine()
                    {
                        CountryOfOrigin = "Belgium",
                        WeeksInterval = 0,
                        Name = "J&J Janssen"
                    },
                    new Vaccine()
                    {
                        CountryOfOrigin = "China",
                        WeeksInterval = 5,
                        Name = "Sinovac"
                    },
                    new Vaccine()
                    {
                        CountryOfOrigin = "Russia",
                        WeeksInterval = 6,
                        Name = "Sputnik V"
                    }
                );

                context.SaveChanges();
            }
        }
    }
}
