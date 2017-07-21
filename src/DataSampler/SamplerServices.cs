using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

namespace Sampler
{
    /// <summary>
    /// If you would like to have a location where your sample files will be housed create a entry in appsettings with the key of SampleDataLocation
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SamplerServices<T>
    {
        private static readonly string _saveLocation = ConfigurationManager.AppSettings["SampleDataLocation"];

        /// <summary>
        /// Creates the sample data.
        /// </summary>
        /// <param name="size">The size exists.</param>
        /// <param name="samplerOptions">The options.</param>
        /// <returns>Sample List.</returns>
        public static List<T> CreateSampleData(int size, SamplerOptions samplerOptions = null)
        {
            List<T> objectList = Activator.CreateInstance<List<T>>();
            RandomHelper helper = new RandomHelper();
            for (int i = 0; i < size; i++)
            {
                object instance = CreateInstance();


                IEnumerable<PropertyInfo> properties = typeof(T).GetProperties();
                foreach (PropertyInfo propertyInfo in properties)
                {
                    Func<object> method = helper.SetFunc(samplerOptions, propertyInfo);
                    bool isUnique = false;
                    bool isSequential = false;
                    bool isNull = false;
                    if (samplerOptions != null)
                    {
                        KeyValuePair<string, SamplerOptions.Options> value = samplerOptions.PropertyOptions.FirstOrDefault(c => c.Key == propertyInfo.Name);
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
                            KeyValuePair<PropertiesSettings, SamplerOptions.Options> defaultValue =
                                samplerOptions.PropertyDefaults.FirstOrDefault(
                                    c => c.Key.PropertyName == propertyInfo.Name);

                            if (defaultValue.Key != null)
                            {
                                PropertiesSettings defaul = defaultValue.Key;
                                method = helper.DecideTheAction(propertyInfo.PropertyType, defaultValue.Value, defaul);
                            }
                        }
                    }

                    if (propertyInfo.CanWrite)
                    {
                        if (method != null)
                        {
                            object value = method.Invoke();
                            IQueryable<T> queryable = objectList.AsQueryable();
                            if (isSequential)
                            {
                                T lastItem = queryable.OrderBy(propertyInfo.Name + " descending").FirstOrDefault();
                                if (propertyInfo.PropertyType == typeof(int))
                                {
                                    if (lastItem != null)
                                    {
                                        object props =
                                            lastItem.GetType().GetProperty(propertyInfo.Name).GetValue(lastItem,
                                                                                                       null);
                                        int currentCount = int.Parse(props.ToString());
                                        value = currentCount + 1;
                                    }
                                }
                                else if (propertyInfo.PropertyType == typeof(DateTime))
                                {
                                    if (lastItem != null)
                                    {
                                        object props = lastItem.GetType().GetProperty(propertyInfo.Name).GetValue(lastItem, null);
                                        DateTime currentDt = DateTime.Parse(props.ToString());
                                        value = currentDt.AddDays(1);
                                    }
                                }
                            }
                            else if (isUnique)
                            {
                                object[] array = new[] { value };
                                bool alreadyExists;
                                if (propertyInfo.PropertyType == typeof(Guid))
                                    alreadyExists = CheckForUniqueGuid(objectList, propertyInfo, value);
                                else
                                    alreadyExists =
                                        queryable.Where(string.Format("{0} = @0", propertyInfo.Name), array).Any();
                                while (alreadyExists)
                                {
                                    value = method.Invoke();
                                    array = new[] { value };
                                    if (propertyInfo.PropertyType == typeof(Guid))
                                        alreadyExists = CheckForUniqueGuid(objectList, propertyInfo, value);
                                    else
                                        alreadyExists =
                                            queryable.Where(string.Format("{0} = @0", propertyInfo.Name), array).Any();

                                }
                            }
                            else if (isNull)
                                value = null;

                            propertyInfo.SetValue(instance, value, null);
                        }
                        else
                        {
                            if (propertyInfo.PropertyType.BaseType == typeof(object))
                            {
                                if (propertyInfo.PropertyType.IsClass) // Not really sure if the IsClass is necessary
                                    propertyInfo.SetValue(instance, CreateSingleItem(propertyInfo.PropertyType, helper), null);
                            }
                        }
                    }
                }
                objectList.Add((T)instance);
            }

            return objectList;
        }

        private static object CreateInstance()
        {
            object instance = null;
            try
            {
                instance = Activator.CreateInstance<T>();
            }
            catch (MissingMethodException)
            {
                ConstructorInfo ctor = typeof(T).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)[0];
                instance = ctor.Invoke(null);
            }
            return instance;
        }

        /// <summary>
        /// Save the List of objects into a file as a json object.  If the key SampleDataLocation doesn't exists within the AppSettings 
        /// with the value of where you want to save the file the application will save the to the location where 
        /// the assemblies are running
        /// </summary>
        /// <param name="objectList">List of T</param>
        public static void SaveToFile(List<T> objectList)
        {
            string fileName = String.Format("{0}.txt", Activator.CreateInstance<T>().GetType());

            StringBuilder location = new StringBuilder();
            if (string.IsNullOrEmpty(_saveLocation))
            {
                location.Append(string.Format("{0}\\{1}",
                                              Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase),
                                              fileName));
                location.Replace(@"file:\", "");
            }
            else
            {
                location.Append(string.Format("{0}\\{1}", _saveLocation, fileName));
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
                if (fs != null)
                    fs.Dispose();
            }
        }

        /// <summary>
        /// Deletes the file.
        /// </summary>
        public static void DeleteFile()
        {
            string fileName = String.Format("{0}.txt", Activator.CreateInstance<T>().GetType());

            StringBuilder location = new StringBuilder();
            if (string.IsNullOrEmpty(_saveLocation))
            {
                location.Append(string.Format("{0}\\{1}",
                                              Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase),
                                              fileName));
                location.Replace(@"file:\", "");
            }
            else
            {
                location.Append(string.Format("{0}\\{1}", _saveLocation, fileName));
            }

            if (File.Exists(location.ToString()))
                File.Delete(location.ToString());
        }

        /// <summary>
        /// Loads the saved file.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns>List{`0}.</returns>
        public static List<T> LoadSavedFile(SamplerOptions options = null)
        {
            string fileName = String.Format("{0}.txt", Activator.CreateInstance<T>().GetType());

            StringBuilder location = new StringBuilder();
            if (string.IsNullOrEmpty(_saveLocation))
            {
                location.Append(string.Format("{0}\\{1}",
                                              Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase),
                                              fileName));
                location.Replace(@"file:\", "");
            }
            else
            {
                location.Append(string.Format("{0}\\{1}", _saveLocation, fileName));
            }

            if (!File.Exists(location.ToString()))
                return null;

            List<T> list = JsonConvert.DeserializeObject<List<T>>(File.ReadAllText(location.ToString()));

            return list;
        }

        private static object CreateSingleItem(Type type, RandomHelper helper)
        {
            object item = Activator.CreateInstance(type);
            IEnumerable<PropertyInfo> properties = type.GetProperties();
            foreach (PropertyInfo propertyInfo in properties)
            {
                Func<object> method = helper.DecideTheAction(propertyInfo.PropertyType, SamplerOptions.Options.NoOptions);
                if (method != null)
                {
                    Type classType = item.GetType();
                    PropertyInfo temp = classType.GetProperty(propertyInfo.Name);
                    if (temp.CanWrite)
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
                var propValue = propInfo.GetValue(item, null);
                if (propValue.ToString() == value.ToString())
                    return true;
            }
            return false;
        }
    }
}