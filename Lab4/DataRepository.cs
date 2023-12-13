using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public class DataRepository
{
    public void SaveTasks(List<TaskModel> tasks, string filePath)
    {
        var json = JsonConvert.SerializeObject(tasks, Formatting.Indented);
        File.WriteAllText(filePath, json);
    }

    public List<TaskModel> LoadTasks(string filePath)
    {
        if (File.Exists(filePath))
        {
            var json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<TaskModel>>(json);
        }
        return new List<TaskModel>();
    }

    public List<TaskModel> LoadTasksFromJson(Microsoft.AspNetCore.Http.IFormFile file)
    {
        using (var reader = new StreamReader(file.OpenReadStream()))
        {
            var json = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<List<TaskModel>>(json);
        }
    }

    public List<TaskModel> LoadTasksFromXml(Microsoft.AspNetCore.Http.IFormFile file)
    {
        var serializer = new System.Xml.Serialization.XmlSerializer(typeof(List<TaskModel>));
        using (var reader = new StreamReader(file.OpenReadStream()))
        {
            return (List<TaskModel>)serializer.Deserialize(reader);
        }
    }
}