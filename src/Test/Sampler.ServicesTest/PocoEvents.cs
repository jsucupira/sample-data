using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Sampler.Test
{
    public class PocoEvents
    {
        public PocoEvents()
        {
            PocoEnumerableForTest = new List<Poco>();
            Pocos = new List<Poco>();
        }

        public int Id { get; set; }
        public string EmployeeNumber { get; set; }
        public string EmployeeFirstName { get; set; }
        public string EmployeeLastName { get; set; }
        public string TypeOfTransaction { get; set; }
        public int CensusId { get; set; }
        public DateTime? ModifiedDt { get; set; }
        public DateTime CreatedDt { get; set; }
        public EventTypes EventStatus { get; set; }
        public int EventId { get; set; }
        public int UnitId { get; set; }
        public TimeSpan ExpectedThresholdTimeInQueue { get; set; }
        public ICollection<Poco> Pocos { get; set; }
        public IEnumerable<Poco> PocoEnumerableForTest { get; set; }
        public float FloatTest { get; set; }
        public Poco2 Poco2 { get; set; }
        public string LongText { get; set; }

        public string EmployeeFullName
        {
            get { return string.Format("{0} {1}", EmployeeFirstName, EmployeeLastName); }
        }
    }
}