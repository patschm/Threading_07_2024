using Feeds;
using System.Xml.Serialization;
using System.Xml;

var serializer = new XmlSerializer(typeof(Item));
var client = new HttpClient();
client.BaseAddress = new Uri("https://nu.nl");
var result = await client.GetAsync("rss");

if (result.IsSuccessStatusCode)
{
    
    await foreach(Item it in ProcessAsync(result.Content.ReadAsStreamAsync()))
    {
        ShowItem(it);
    }
}

async IAsyncEnumerable<Item> ProcessAsync(Task<Stream> streamTask)
{
    var reader = XmlReader.Create(await streamTask);
    while (reader.ReadToFollowing("item"))
    {
        var item = ProcessItem(reader.ReadSubtree());
        if (item != null) yield return item;
    }
}


Item? ProcessItem(XmlReader xmlReader)
{
    return serializer.Deserialize(xmlReader) as Item;
}

void ShowItem(Item item)
{
    Console.BackgroundColor = ConsoleColor.Yellow;
    Console.WriteLine(item.Category);
    Console.ResetColor();
    Console.BackgroundColor = ConsoleColor.Red;
    Console.WriteLine(item.Title);
    Console.ResetColor();
    Console.WriteLine(item.Description);
    Console.WriteLine();
}

