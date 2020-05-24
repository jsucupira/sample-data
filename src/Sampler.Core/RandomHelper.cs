using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Sampler.Core
{
    public class RandomHelper
    {
        private readonly GenerateRandomServices _generateRandomServices = new GenerateRandomServices();

        public Func<object> DecideTheAction(Type type, SamplerOptions.Options samplerOptions,
                                            PropertiesSettings propertiesSettings = null)
        {
            Func<object> function = null;
            if (type == typeof(int))
            {
                if (propertiesSettings != null)
                    return () => int.Parse(propertiesSettings.PropertyValue);
                function = () => _generateRandomServices.GenerateRandomInt();
            }
            else if (type == typeof(int?))
            {
                if (propertiesSettings != null)
                    return () => int.Parse(propertiesSettings.PropertyValue);
                function = () => _generateRandomServices.GenerateRandomNullableInt();
            }
            else if (type == typeof(double))
            {
                if (propertiesSettings != null)
                    return () => double.Parse(propertiesSettings.PropertyValue);
                function = () => _generateRandomServices.GenerateRandomDouble();
            }
            else if (type == typeof(float))
            {
                if (propertiesSettings != null)
                    return () => float.Parse(propertiesSettings.PropertyValue);
                function = () => _generateRandomServices.GenerateRandomFloat();
            }
            else if (type == typeof(decimal))
            {
                if (propertiesSettings != null)
                    return () => decimal.Parse(propertiesSettings.PropertyValue);
                function = () => _generateRandomServices.GenerateRandomDecimal();
            }
            else if (type == typeof(decimal?))
            {
                if (propertiesSettings != null)
                    return () => decimal.Parse(propertiesSettings.PropertyValue);
                function = () => _generateRandomServices.GenerateRandomNullableDecimal();
            }
            else if (type == typeof(byte))
            {
                if (propertiesSettings != null)
                    return () => byte.Parse(propertiesSettings.PropertyValue);

                function = _generateRandomServices.GenerateRandomByte;
            }
            else if (type == typeof(DateTime))
            {
                if (propertiesSettings != null)
                    return () => DateTime.Parse(propertiesSettings.PropertyValue);

                function = () => _generateRandomServices.GenerateRandomDate();
            }
            else if (type == typeof(DateTimeOffset))
            {
                if (propertiesSettings != null)
                    return () => DateTimeOffset.Parse(propertiesSettings.PropertyValue);

                function = () => _generateRandomServices.GenerateRandomDate();
            }
            else if (type == typeof(DateTime?))
            {
                if (propertiesSettings != null)
                    return () => DateTime.Parse(propertiesSettings.PropertyValue);
                function = () => _generateRandomServices.GenerateRandomNullableDate();
            }
            else if (type == typeof(Guid))
            {
                if (propertiesSettings != null)
                    return () => Guid.Parse(propertiesSettings.PropertyValue);

                function = () => _generateRandomServices.GenerateRandomGuid();
            }
            else if (type == typeof(bool))
            {
                if (propertiesSettings != null)
                    return () => bool.Parse(propertiesSettings.PropertyValue);

                function = () => _generateRandomServices.GenerateRandomBool();
            }
            else if (type.BaseType == typeof(Enum))
            {
                if (propertiesSettings != null)
                    return () => int.Parse(propertiesSettings.PropertyValue);

                function = () => _generateRandomServices.GenerateRandomEnum(type);
            }
            else if (type == typeof(string))
            {
                if (propertiesSettings != null)
                    return () => propertiesSettings.PropertyValue;

                switch (samplerOptions)
                {
                    case SamplerOptions.Options.NoOptions:
                        function = _generateRandomServices.GenerateRandomWord;
                        break;
                    case SamplerOptions.Options.IsNullable:
                        function = _generateRandomServices.GenerateRandomNullableString;
                        break;
                    case SamplerOptions.Options.Paragraph:
                        function = _generateRandomServices.GenerateRandomParagraph;
                        break;
                    case SamplerOptions.Options.Sentense:
                        function = _generateRandomServices.GenerateRandomSentence;
                        break;
                    case SamplerOptions.Options.Phrase:
                        function = _generateRandomServices.GenerateRandomPhrase;
                        break;
                    case SamplerOptions.Options.OneWord:
                        function = _generateRandomServices.GenerateRandomWord;
                        break;
                    case SamplerOptions.Options.Email:
                        function = _generateRandomServices.GenerateRandomEmail;
                        break;
                    case SamplerOptions.Options.Phone:
                        function = _generateRandomServices.GenerateRandomPhone;
                        break;
                    default:
                        function = _generateRandomServices.GenerateRandomWord;
                        break;
                }
            }

            return function;
        }

        public Func<object> SetFunc(SamplerOptions samplerOptions, PropertyInfo propertyInfo)
        {
            Func<object> method;
            if (samplerOptions != null)
            {
                KeyValuePair<string, SamplerOptions.Options> value =
                    samplerOptions.PropertyOptions.FirstOrDefault(c => c.Key == propertyInfo.Name);
                if (value.Key != null)
                    method = DecideTheAction(propertyInfo.PropertyType, value.Value);
                else
                    method = DecideTheAction(propertyInfo.PropertyType, SamplerOptions.Options.NoOptions);
            }
            else
                method = DecideTheAction(propertyInfo.PropertyType, SamplerOptions.Options.NoOptions);

            return method;
        }
    }
}