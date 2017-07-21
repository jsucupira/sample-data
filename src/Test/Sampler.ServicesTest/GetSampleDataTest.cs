using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sampler.Test
{
    [TestClass]
    public class GetSampleDataTest
    {
        [TestMethod]
        public void CreateSampleDataForClassTest()
        {
            List<PocoEvents> eventsList = SamplerServices<PocoEvents>.CreateSampleData(10);
            Assert.IsTrue(eventsList.Count == 10);
        }

        [TestMethod]
        public void SaveAndLoadSampleDataForClassToXmlTest()
        {
            SamplerServices<PocoEvents>.SaveToFile(SamplerServices<PocoEvents>.CreateSampleData(20));
            Assert.IsTrue(SamplerServices<PocoEvents>.LoadSavedFile().Any());
            SamplerServices<PocoEvents>.DeleteFile();
        }

        [TestMethod]
        public void DeleteSampleDataForClassToXmlTest()
        {
            SamplerServices<PocoEvents>.DeleteFile();
        }

        [TestMethod]
        public void TestCreatingUniqueGuid()
        {
            SamplerOptions samplerOptions = new SamplerOptions();
            samplerOptions.PropertyOptions.Add("UserId", SamplerOptions.Options.IsUnique);
            var result = SamplerServices<Poco3>.CreateSampleData(10, samplerOptions);
            Assert.IsTrue(result.Count == 10);
        }

        [TestMethod]
        public void AttachItemsToListTest()
        {
            List<PocoEvents> eventsList = SamplerServices<PocoEvents>.CreateSampleData(10);

            foreach (var events in eventsList)
                events.Pocos = SamplerServices<Poco>.CreateSampleData(10);

            SamplerServices<PocoEvents>.SaveToFile(eventsList);
            SamplerServices<PocoEvents>.DeleteFile();
            Assert.IsTrue(eventsList.Count == 10);
        }

        [TestMethod]
        public void CreateWithPrivateConstructor()
        {
            List<PocoPrivate> eventsList = SamplerServices<PocoPrivate>.CreateSampleData(10);
            Assert.IsTrue(eventsList.Count == 10);
        }

        [TestMethod]
        public void GetSampleWithOptionsTest()
        {
            var idList = new List<int>();
            var dates = new List<DateTime>();

            SamplerOptions options = new SamplerOptions();
            options.PropertyOptions.Add("Id", SamplerOptions.Options.IsUnique);
            options.PropertyOptions.Add("LongText", SamplerOptions.Options.Paragraph);
            options.PropertyOptions.Add("CreatedDt", SamplerOptions.Options.IsUnique);
            List<PocoEvents> eventsList = SamplerServices<PocoEvents>.CreateSampleData(100, options);

            foreach (var pocoEventse in eventsList)
            {
                idList.Add(pocoEventse.Id);
                dates.Add(pocoEventse.CreatedDt);

                Assert.IsTrue(pocoEventse.LongText.Length > 20);
            }

            Assert.IsFalse(idList.HasDuplicates());
            Assert.IsFalse(dates.HasDuplicates());
        }

        [TestMethod]
        public void GetSampleWithNullOptionsTest()
        {
            SamplerOptions options = new SamplerOptions();
            options.PropertyOptions.Add("LongText", SamplerOptions.Options.NullValue);
            List<PocoEvents> eventsList = SamplerServices<PocoEvents>.CreateSampleData(10, options);
            Assert.IsTrue(eventsList.Count == 10);

            foreach (var pocoEventse in eventsList)
                Assert.IsTrue(pocoEventse.LongText == null);
        }

        [TestMethod]
        public void GetSampleWithOptionsSequencialTest()
        {
            SamplerOptions options = new SamplerOptions();
            options.PropertyOptions.Add("EventId", SamplerOptions.Options.Sequential);
            List<PocoEvents> eventsList = SamplerServices<PocoEvents>.CreateSampleData(10, options);
            PocoEvents firstItem = eventsList.OrderBy(c => c.EventId).FirstOrDefault();
            int firstValue = 0;
            if (firstItem != null)
                firstValue = firstItem.EventId;

            foreach (var pocoEventse in eventsList.OrderBy(c => c.EventId))
            {
                Assert.IsTrue(pocoEventse.EventId == firstValue);
                firstValue++;
            }
        }

        [TestMethod]
        public void GetSampleWithOptionsDefaultValueTest()
        {
            SamplerOptions options = new SamplerOptions();
            options.PropertyDefaults.Add(new PropertiesSettings 
            {PropertyName = "UnitId", PropertyValue = "1"}, SamplerOptions.Options.DefaultValue);
            List<PocoEvents> eventsList = SamplerServices<PocoEvents>.CreateSampleData(10, options);

            foreach (var pocoEventse in eventsList.OrderBy(c => c.EventId))
            {
                Assert.IsTrue(pocoEventse.UnitId == 1);
            }
        }
        
        [TestMethod]
        public void GetSampleWithOptionsDefaultValueAndSequenceTest()
        {
            SamplerOptions options = new SamplerOptions();
            options.PropertyDefaults.Add(new PropertiesSettings 
            {PropertyName = "UnitId", PropertyValue = "1"}, SamplerOptions.Options.DefaultValue);
            options.PropertyOptions.Add("UnitId", SamplerOptions.Options.Sequential);
            List<PocoEvents> eventsList = SamplerServices<PocoEvents>.CreateSampleData(10, options);
            int currentUnit = 1;
            foreach (var pocoEventse in eventsList.OrderBy(c => c.UnitId))
            {
                Assert.IsTrue(pocoEventse.UnitId == currentUnit);
                currentUnit++;
            }
        }

        [TestMethod]
        public void GetSampleWithOptionsEnumDefaultValueTest()
        {
            SamplerOptions options = new SamplerOptions();
            options.PropertyDefaults.Add(new PropertiesSettings
                {PropertyName = "EventStatus", PropertyValue = "6"}, SamplerOptions.Options.DefaultValue);
            List<PocoEvents> eventsList = SamplerServices<PocoEvents>.CreateSampleData(10, options);

            foreach (var pocoEventse in eventsList.OrderBy(c => c.EventId))
            {
                Assert.IsTrue(pocoEventse.EventStatus == EventTypes.Deleted);
            }
        }

        [TestMethod]
        public void GetSampleWithOptionsDateTimeSequencialTest()
        {
            SamplerOptions options = new SamplerOptions();
            options.PropertyOptions.Add("CreatedDt", SamplerOptions.Options.Sequential);
            List<PocoEvents> eventsList = SamplerServices<PocoEvents>.CreateSampleData(10, options);

            PocoEvents firstItem = eventsList.OrderBy(c => c.CreatedDt).FirstOrDefault();
            DateTime firstValue = DateTime.Now;
            if (firstItem != null)
                firstValue = firstItem.CreatedDt;

            foreach (var pocoEventse in eventsList.OrderBy(c => c.CreatedDt))
            {
                Assert.IsTrue(pocoEventse.CreatedDt == firstValue);
                firstValue = firstValue.AddDays(1);
            }
        }
    }
}