using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace Sampler.Core.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder().AddInMemoryCollection(
                new Dictionary<string, string> {{ "SampleDataLocation", @"c:\Temp\" } }
            ).Build();

            var samplerServices = new SamplerServices<CheckoutVM>(configuration);

            var visitSettings = new PropertiesSettings
            {
                PropertyName = nameof(CheckoutVM.VisitDate),
                PropertyValue = DateTimeOffset.UtcNow.AddDays(-1).ToString()
            };

            var location = new PropertiesSettings
            {
                PropertyName = nameof(CheckoutVM.LocationId),
                PropertyValue = "1"
            };

            var TestId = new PropertiesSettings
            {
                PropertyName = nameof(CheckoutVM.TestId),
                PropertyValue = "1"
            };

            var city = new PropertiesSettings
            {
                PropertyName = nameof(CheckoutVM.City),
                PropertyValue = "Tampa"
            };

            var state = new PropertiesSettings
            {
                PropertyName = nameof(CheckoutVM.State),
                PropertyValue = "FL"
            };

            var options = new SamplerOptions();
            options.PropertyOptions.Add(nameof(CheckoutVM.FirstName), SamplerOptions.Options.OneWord);
            options.PropertyOptions.Add(nameof(CheckoutVM.LastName), SamplerOptions.Options.OneWord);
            options.PropertyOptions.Add(nameof(CheckoutVM.EMail), SamplerOptions.Options.Email);
            options.PropertyOptions.Add(nameof(CheckoutVM.PhoneNumber), SamplerOptions.Options.Phone);
            options.PropertyOptions.Add(nameof(CheckoutVM.Address), SamplerOptions.Options.Phrase);

            options.PropertyDefaults.Add(visitSettings, SamplerOptions.Options.DefaultValue);
            options.PropertyDefaults.Add(location, SamplerOptions.Options.DefaultValue);
            options.PropertyDefaults.Add(TestId, SamplerOptions.Options.DefaultValue);
            options.PropertyDefaults.Add(city, SamplerOptions.Options.DefaultValue);
            options.PropertyDefaults.Add(state, SamplerOptions.Options.DefaultValue);

            var checkouts = SamplerServices<CheckoutVM>.CreateSampleData(20, options);
            visitSettings.PropertyValue = DateTimeOffset.UtcNow.ToString();
            checkouts.AddRange(SamplerServices<CheckoutVM>.CreateSampleData(25, options));
            visitSettings.PropertyValue = DateTimeOffset.UtcNow.AddDays(1).ToString();
            checkouts.AddRange(SamplerServices<CheckoutVM>.CreateSampleData(30, options));

            var random = new Random(1);

            foreach (var item in checkouts)
            {
                item.Sex = random.Next(2) % 2 == 0 ? "Male" : "Female";
                item.DateOfBirth = item.DateOfBirth.AddYears(-random.Next(50));
                item.VisitDate = item.VisitDate.AddHours(random.Next(48));
                item.VisitDate = item.VisitDate.AddMinutes(random.Next(400));
            }

            samplerServices.SaveToFile(checkouts);
        }
    }

    public class CheckoutVM
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EMail { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTimeOffset VisitDate { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Sex { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public int Zip { get; set; }
        public string PhoneNumber { get; set; }
        public string Notes { get; set; }
        public int LocationId { get; set; }
        public int TestId { get; set; }
    }
}
