**Project Description**
Data Sampler is a .Net library intended to help developers to quick create sample data for unit test purpose.

*This was imported from my codeplex project.*

**Features**
- Ability to quickly create dummy data for your classes
- Ability to save a set of data to a file, so that data can be re-used for your unit tests

**Information**
This is a .Net 4.0 library.

**This dll has dependencies on Newtonsoft.Json**

**Create sample 10 sample data for the class poco:**
```c#
List<PocoEvents> EventsList = SamplerServices<PocoEvents>.CreateSampleData(10);
```

**Save sample data for later use.**
```c#
SamplerServices<PocoEvents>.SaveToFile(EventsList);
```

**Load the saved sample data**
```c#
var eventList = SamplerServices<PocoEvents>.LoadSavedFile();
```

**Create sample data with options**
```c#
SamplerOptions options = new SamplerOptions();
options.PropertyOptions.Add("Id", SamplerOptions.Options.IsUnique);
options.PropertyOptions.Add("LongText", SamplerOptions.Options.Paragraph);
options.PropertyOptions.Add("CreatedDt", SamplerOptions.Options.IsUnique);
List<PocoEvents> EventsList = SamplerServices<PocoEvents>.CreateSampleData(100, options);
```

**Here is a sample of creating a list of object and saving to a file so the Unit Test always runs against this data sample**
```c#
public void CreateSample()
{
    SamplerOptions options = new SamplerOptions();
    options.PropertyDefaults.Add(new PropertiesSettings { PropertyName = "Id", PropertyValue = "1" }, SamplerOptions.Options.DefaultValue);
    options.PropertyOptions.Add("Id", SamplerOptions.Options.Sequential);
    List<Menu> menues = SamplerServices<Menu>.CreateSampleData(15, options);
    List<Category> categories = SamplerServices<Category>.CreateSampleData(3, options);
    categories[0](0).Name = "Appetizer";
    categories[1](1).Name = "Entree";
    categories[2](2).Name = "Desert";

    for (int i = 0; i < 5; i++)
    {
        CreateMenuItem(menues, i);
        menues[i](i).Category = categories[0](0);
        menues[i](i).CategoryId = categories[0](0).Id;
    }
    for (int i = 5; i < 10; i++)
    {
        CreateMenuItem(menues, i);
        menues[i](i).Category = categories[0](0);
        menues[i](i).CategoryId = categories[1](1).Id;
    }
    for (int i = 10; i < 15; i++)
    {
        CreateMenuItem(menues, i);
        menues[i](i).Category = categories[0](0);
        menues[i](i).CategoryId = categories[2](2).Id;
    }
    SamplerServices<Menu>.SaveToFile(menues);
}

private static void CreateMenuItem(IList<Menu> menues, int currentCount)
{
    foreach (var menuItem in SamplerServices<MenuItem>.CreateSampleData(_random.Next(2, 8)))
    {
        menuItem.MenuId = menues[currentCount](currentCount).Id;
        foreach (var nutrionInformation in SamplerServices<NutrionInformation>.CreateSampleData(_random.Next(3, 5)))
        {
            nutrionInformation.MenuItemId = menuItem.Id;
            menuItem.NutriotionInformations.Add(nutrionInformation);
        }
        menues[currentCount](currentCount).MenuItems.Add(menuItem);
    }
}
```