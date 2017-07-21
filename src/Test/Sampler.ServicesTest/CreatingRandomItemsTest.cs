using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sampler.Test
{
    [TestClass]
    public class CreatingRandomItemsTest
    {
        private readonly GenerateRandomServices _generateRandomServices= new GenerateRandomServices();
        [TestMethod]
        public void ShouldCreateRandomInt()
        {
            int random1 = _generateRandomServices.GenerateRandomInt();
            int random2 = _generateRandomServices.GenerateRandomInt();
            Assert.IsTrue(random1 != random2);
        }

        [TestMethod]
        public void ShouldCreateRandomNullableInt()
        {
            int? random1 = _generateRandomServices.GenerateRandomNullableInt();
            int? random2 = _generateRandomServices.GenerateRandomNullableInt();
            bool result = false;
            for (int i = 0; i < 15; i++)
            {
                //This could be equal depending on the GenerateRandomBool method.
                // there is a 50/50 ish that this will return null

                result = random1 != random2;
                if (result)
                    break;
                else
                {
                    random1 = _generateRandomServices.GenerateRandomNullableInt();
                    random2 = _generateRandomServices.GenerateRandomNullableInt();
                }
            }
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ShouldCreateRandomDouble()
        {
            var random1 = _generateRandomServices.GenerateRandomDouble();
            var random2 = _generateRandomServices.GenerateRandomDouble();
            Assert.IsTrue(random1 != random2);
        }

        [TestMethod]
        public void ShouldCreateRandomByte()
        {
            var random1 = _generateRandomServices.GenerateRandomByte();
            var random2 = _generateRandomServices.GenerateRandomByte();
            Assert.IsTrue(random1 != random2);
        }

        [TestMethod]
        public void ShouldCreateRandomDecimal()
        {
            Decimal random1 = _generateRandomServices.GenerateRandomDecimal();
            Decimal random2 = _generateRandomServices.GenerateRandomDecimal();
            Assert.IsTrue(random1 != random2);
        }

        [TestMethod]
        public void ShouldCreateRandomNullableDecimal()
        {
            Decimal? random1 = _generateRandomServices.GenerateRandomNullableDecimal();
            Decimal? random2 = _generateRandomServices.GenerateRandomNullableDecimal();
            bool result = false;
            for (int i = 0; i < 5; i++)
            {
                //This could be equal depending on the GenerateRandomBool method.
                // there is a 50/50 ish that this will return null

                result = random1 != random2;
                if (result)
                    break;
                else
                {
                    random1 = _generateRandomServices.GenerateRandomNullableDecimal();
                    random2 = _generateRandomServices.GenerateRandomNullableDecimal();
                }
            }
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ShouldCreateRandomBool()
        {
            bool random1 = _generateRandomServices.GenerateRandomBool();
            bool random2 = _generateRandomServices.GenerateRandomBool();

            bool result = false;
            for (int i = 0; i < 10; i++)
            {
                //Not really sure how to test this.
                // for now I will loop until they r not equal.  if I loop 10 times and they are still equal
                //than must be something wrong or I was really unlucky

                result = random1 != random2;
                if (result)
                    break;
                else
                {
                    random1 = _generateRandomServices.GenerateRandomBool();
                    random2 = _generateRandomServices.GenerateRandomBool();
                }
            }
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CreateRandomGuid()
        {
            var random1 = _generateRandomServices.GenerateRandomGuid();
            Assert.IsTrue(random1 != Guid.Empty);
        }

        [TestMethod]
        public void CreateRandomEnum()
        {
            EventTypes random1 = (EventTypes)_generateRandomServices.GenerateRandomEnum(typeof(EventTypes));
            EventTypes random2 = (EventTypes)_generateRandomServices.GenerateRandomEnum(typeof(EventTypes));

            bool result = false;
            for (int i = 0; i < 5; i++)
            {
                result = random1 != random2;
                if (result)
                    break;
                else
                {
                    random1 = (EventTypes)_generateRandomServices.GenerateRandomEnum(typeof(EventTypes));
                    random2 = (EventTypes)_generateRandomServices.GenerateRandomEnum(typeof(EventTypes));
                }
            }
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ShouldCreateRandomParagraph()
        {
            var random1 = _generateRandomServices.GenerateRandomParagraph();
            var random2 = _generateRandomServices.GenerateRandomParagraph();
            Assert.IsTrue(random1 != random2);
        }

        [TestMethod]
        public void ShouldCreateRandomSentence()
        {
            var random1 = _generateRandomServices.GenerateRandomSentence();
            var random2 = _generateRandomServices.GenerateRandomSentence();
            Assert.IsTrue(random1 != random2);
        }

        [TestMethod]
        public void ShouldCreateRandomPhrases()
        {
            var random1 = _generateRandomServices.GenerateRandomPhrase();
            var random2 = _generateRandomServices.GenerateRandomPhrase();
            Assert.IsTrue(random1 != random2);
        }

        [TestMethod]
        public void ShouldCreateRandomWord()
        {
            var random1 = _generateRandomServices.GenerateRandomWord();
            var random2 = _generateRandomServices.GenerateRandomWord();
            Assert.IsTrue(random1 != random2);
        }

        [TestMethod]
        public void ShouldCreateNullableString()
        {
            var random1 = _generateRandomServices.GenerateRandomNullableString();
            var random2 = _generateRandomServices.GenerateRandomNullableString();

            bool result = false;
            for (int i = 0; i < 5; i++)
            {
                //This could be equal depending on the GenerateRandomBool method.
                // there is a 50/50 ish that this will return null

                result = random1 != random2;
                if (result)
                    break;
                else
                {
                    random1 = _generateRandomServices.GenerateRandomNullableString();
                    random2 = _generateRandomServices.GenerateRandomNullableString();
                }
            }
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ShouldCreateRandomDate()
        {
            var random1 = _generateRandomServices.GenerateRandomDate();
            var random2 = _generateRandomServices.GenerateRandomDate();
            Assert.IsTrue(random1 != random2);
        }

        [TestMethod]
        public void ShouldCreateRandomNullableDate()
        {
                DateTime? random1 = _generateRandomServices.GenerateRandomNullableDate();
                DateTime? random2 = _generateRandomServices.GenerateRandomNullableDate();
                bool result = false;
                for (int i = 0; i < 5; i++)
                {
                    //This could be equal depending on the GenerateRandomBool method.
                    // there is a 50/50 ish that this will return null

                    result = random1 != random2;
                    if (result)
                        break;
                    else
                    {
                        random1 = _generateRandomServices.GenerateRandomNullableDate();
                        random2 = _generateRandomServices.GenerateRandomNullableDate();
                    }
                }
                Assert.IsTrue(result);
        }
    }
}