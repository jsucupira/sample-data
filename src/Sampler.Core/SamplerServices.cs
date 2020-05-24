using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Sampler.Core
{
    /// <summary>
    ///     If you would like to have a location where your sample files will be housed create a entry in appsettings with the
    ///     key of SampleDataLocation
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SamplerServices<T>
    {
        private readonly string _saveLocation;

        public SamplerServices(IConfiguration configuration)
        {
            var section = configuration.GetSection("SampleDataLocation");
            if (section != null)
                _saveLocation = section.Value;
        }

        /// <summary>
        /// Creates the sample data.
        /// </summary>
        /// <param name="size">The size exists.</param>
        /// <param name="samplerOptions">The options.</param>
        /// <returns>Sample List.</returns>
        public static List<T> CreateSampleData(int size, SamplerOptions samplerOptions = null)
        {
            var objectList = Activator.CreateInstance<List<T>>();
            var helper = new RandomHelper();
            for (var i = 0; i < size; i++)
            {
                var instance = CreateInstance();

                IEnumerable<PropertyInfo> properties = typeof(T).GetProperties();
                foreach (var propertyInfo in properties)
                {
                    var method = helper.SetFunc(samplerOptions, propertyInfo);
                    var isUnique = false;
                    var isSequential = false;
                    var isNull = false;;
                    if (samplerOptions != null)
                    {
                        var value = samplerOptions.PropertyOptions.FirstOrDefault(c => c.Key == propertyInfo.Name);
                        if (value.Key != null)
                        {
                            method = helper.DecideTheAction(propertyInfo.PropertyType, value.Value);
                            if (value.Value == SamplerOptions.Options.IsUnique)
                                isUnique = true;
                            else if (value.Value == SamplerOptions.Options.Sequential)
                                isSequential = true;
                            else if (value.Value == SamplerOptions.Options.NullValue)
                                isNull = true;
                        }

                        if (samplerOptions.PropertyDefaults.Any())
                        {
                            var defaultValue = samplerOptions.PropertyDefaults.FirstOrDefault(c => c.Key.PropertyName == propertyInfo.Name);

                            if (defaultValue.Key != null)
                            {
                                var @default = defaultValue.Key;
                                method = helper.DecideTheAction(propertyInfo.PropertyType, defaultValue.Value, @default);
                            }
                        }
                    }

                    if (propertyInfo.CanWrite)
                    {
                        if (method != null)
                        {
                            var value = method.Invoke();
                            var queryable = objectList.AsQueryable();
                            if (isSequential)
                            {
                                var lastItem = queryable.OrderBy(propertyInfo.Name + " descending").FirstOrDefault();
                                if (propertyInfo.PropertyType == typeof(int))
                                {
                                    if (lastItem != null)
                                    {
                                        var props =
                                            lastItem.GetType().GetProperty(propertyInfo.Name)
                                                ?.GetValue(lastItem,
                                                null);
                                        if (props != null)
                                        {
                                            var currentCount = int.Parse(props.ToString());
                                            value = currentCount + 1;
                                        }
                                    }
                                }
                                else if (propertyInfo.PropertyType == typeof(DateTime))
                                {
                                    if (lastItem != null)
                                    {
                                        var props = lastItem.GetType().GetProperty(propertyInfo.Name)
                                            ?.GetValue(lastItem, null);
                                        if (props != null)
                                        {
                                            var currentDt = DateTime.Parse(props.ToString());
                                            value = currentDt.AddDays(1);
                                        }
                                    }
                                }
                            }
                            else if (isUnique)
                            {
                                object[] array = {value};
                                bool alreadyExists;
                                alreadyExists = propertyInfo.PropertyType == typeof(Guid) ? CheckForUniqueGuid(objectList, propertyInfo, value) : queryable.Where($"{propertyInfo.Name} = @0", array).Any();
                                while (alreadyExists)
                                {
                                    value = method.Invoke();
                                    array = new[] {value};
                                    alreadyExists = propertyInfo.PropertyType == typeof(Guid) ? CheckForUniqueGuid(objectList, propertyInfo, value) : queryable.Where($"{propertyInfo.Name} = @0", array).Any();
                                }
                            }
                            else if (isNull)
                            {
                                value = null;
                            }

                            propertyInfo.SetValue(instance, value, null);
                        }
                        else
                        {
                            if (propertyInfo.PropertyType.BaseType == typeof(object))
                                if (propertyInfo.PropertyType.IsClass) // Not really sure if the IsClass is necessary
                                    propertyInfo.SetValue(instance, CreateSingleItem(propertyInfo.PropertyType, helper),
                                        null);
                        }
                    }
                }

                objectList.Add((T) instance);
            }

            return objectList;
        }

        private static object CreateInstance()
        {
            object instance;
            try
            {
                instance = Activator.CreateInstance<T>();
            }
            catch (MissingMethodException)
            {
                var ctor = typeof(T).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)[0];
                instance = ctor.Invoke(null);
            }

            return instance;
        }

        /// <summary>
        ///     Save the List of objects into a file as a json object.  If the key SampleDataLocation doesn't exists within the
        ///     AppSettings
        ///     with the value of where you want to save the file the application will save the to the location where
        ///     the assemblies are running
        /// </summary>
        /// <param name="objectList">List of T</param>
        public void SaveToFile(List<T> objectList)
        {
            var fileName = $"{Activator.CreateInstance<T>().GetType()}.json";

            var location = new StringBuilder();
            if (string.IsNullOrEmpty(_saveLocation))
            {
                location.Append($"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase)}\\{fileName}");
                location.Replace(@"file:\", "");
            }
            else
            {
                location.Append($"{_saveLocation}\\{fileName}");
            }

            if (File.Exists(location.ToString()))
                File.Delete(location.ToString());

            FileStream fs = null;
            try
            {
                fs = new FileStream(location.ToString(), FileMode.Create, FileAccess.Write);

                using (var sw = new StreamWriter(fs))
                {
                    fs = null;
                    var jsonString = JsonConvert.SerializeObject(objectList);
                    sw.Write(jsonString);
                }
            }
            finally
            {
                fs?.Dispose();
            }
        }

        /// <summary>
        ///     Deletes the file.
        /// </summary>
        public void DeleteFile()
        {
            var fileName = $"{Activator.CreateInstance<T>().GetType()}.txt";

            var location = new StringBuilder();
            if (string.IsNullOrEmpty(_saveLocation))
            {
                location.Append(
                    $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase)}\\{fileName}");
                location.Replace(@"file:\", "");
            }
            else
            {
                location.Append($"{_saveLocation}\\{fileName}");
            }

            if (File.Exists(location.ToString()))
                File.Delete(location.ToString());
        }

        /// <summary>
        ///     Loads the saved file.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns>List{`0}.</returns>
        public List<T> LoadSavedFile(SamplerOptions options = null)
        {
            var fileName = $"{Activator.CreateInstance<T>().GetType()}.txt";

            var location = new StringBuilder();
            if (string.IsNullOrEmpty(_saveLocation))
            {
                location.Append(
                    $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase)}\\{fileName}");
                location.Replace(@"file:\", "");
            }
            else
            {
                location.Append($"{_saveLocation}\\{fileName}");
            }

            if (!File.Exists(location.ToString()))
                return null;

            var list = JsonConvert.DeserializeObject<List<T>>(File.ReadAllText(location.ToString()));

            return list;
        }

        private static object CreateSingleItem(Type type, RandomHelper helper)
        {
            var item = Activator.CreateInstance(type);
            IEnumerable<PropertyInfo> properties = type.GetProperties();
            foreach (var propertyInfo in properties)
            {
                var method = helper.DecideTheAction(propertyInfo.PropertyType, SamplerOptions.Options.NoOptions);
                if (method != null)
                {
                    var classType = item.GetType();
                    var temp = classType.GetProperty(propertyInfo.Name);
                    if (temp != null && temp.CanWrite)
                        temp.SetValue(item, method.Invoke(), null);
                }
            }

            return item;
        }

        private static bool CheckForUniqueGuid(IEnumerable<T> objectList, PropertyInfo propertyInfo, object value)
        {
            foreach (var item in objectList)
            {
                var propInfo = typeof(T).GetProperty(propertyInfo.Name);
                if (propInfo != null)
                {
                    var propValue = propInfo.GetValue(item, null);
                    if (propValue.ToString() == value.ToString())
                        return true;
                }
            }

            return false;
        }
    }
}