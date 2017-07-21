using System;

namespace Sampler.Test
{
    public class Poco
    {
        public int IntProp { get; set; }
        public string StringProp { get; set; }
    }

    public class PocoPrivate
    {
        private PocoPrivate()
        {
            
        }
        public int IntProp { get; set; }
        public string StringProp { get; set; }
    }

    public class Poco2
    {
        public int IntProp { get; set; }
        public string StringProp { get; set; }
    }

    public class Poco3
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
    }
}