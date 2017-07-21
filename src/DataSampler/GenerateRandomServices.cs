using System;
using System.Collections.Generic;
using System.Linq;

namespace Sampler
{
    public class GenerateRandomServices
    {
        private const int MAX = 299999;
        private readonly Random _random;

        public GenerateRandomServices()
        {
            _random = new Random();
        }

        public int GenerateRandomInt()
        {
            return _random.Next(MAX);
        }

        public int? GenerateRandomNullableInt()
        {
            if (GenerateRandomBool())
                return GenerateRandomInt();

            return null;
        }

        public double GenerateRandomDouble()
        {
            double dob = GenerateRandomInt();
            return dob + _random.NextDouble();
        }

        public float GenerateRandomFloat()
        {
            double dob = GenerateRandomInt();
            return Convert.ToSingle(dob + _random.NextDouble());
        }

        public Byte[] GenerateRandomByte()
        {
            Byte[] byteArray = new Byte[GenerateRandomInt()];
            _random.NextBytes(byteArray);
            return byteArray;
        }

        public DateTime GenerateRandomDate()
        {
            DateTime current = DateTime.Now;
            int randomNumber = _random.Next(365);
            return new DateTime(current.Year, 1, 1).AddDays(randomNumber);
        }

        public DateTime? GenerateRandomNullableDate()
        {
            if (GenerateRandomBool())
                return GenerateRandomDate();
            else
                return null;
        }

        public Guid GenerateRandomGuid()
        {
            return Guid.NewGuid();
        }

        public bool GenerateRandomBool()
        {
            int randomNumber = _random.Next(2);
            return randomNumber == 1;
        }

        public decimal GenerateRandomDecimal()
        {
            decimal randomDecimal = GenerateRandomInt();
            return randomDecimal;
        }

        public decimal? GenerateRandomNullableDecimal()
        {
            if (GenerateRandomBool())
                return GenerateRandomDecimal();

            return null;
        }

        public object GenerateRandomEnum(Type type)
        {
            List<object> types = new List<object>();
            foreach (object item in Enum.GetValues(type))
                types.Add(item);

            int size = types.Count - 1;
            return types[_random.Next(size)];
        }

        public string GenerateRandomPhrase()
        {
            string sentence = GenerateRandomSentence();
            List<string> phrases = sentence.Split(',').ToList();
            int size = phrases.Count - 1;
            if (size < 0)
                GenerateRandomPhrase();

            string result = phrases[_random.Next(size)].Trim();
            if (result.Length == 0)
                GenerateRandomPhrase();

            return result;
        }

        public string GenerateRandomWord()
        {
            string phrase = GenerateRandomPhrase();
            List<string> word = phrase.Split(' ').ToList();
            int size = word.Count - 1;
            return word[_random.Next(size)].Trim();
        }

        public string GenerateRandomSentence()
        {
            string paraGraph = GenerateRandomParagraph();
            List<string> allSentences = paraGraph.Split('.').ToList();
            int size = allSentences.Count() - 1;
            string result = allSentences[_random.Next(size)].Trim();

            if (result.Length == 0)
                GenerateRandomSentence();

            return result.Trim();
        }

        public string GenerateRandomParagraph()
        {
            const int paraSize = 27;
            return LoremIpsum.LOREMIPSUM.Split('|')[_random.Next(paraSize)].Trim();
        }

        public string GenerateRandomNullableString()
        {
            int randomNumber = _random.Next(2);

            if (randomNumber == 1)
                return GenerateRandomWord();

            return null;
        }
    }
}