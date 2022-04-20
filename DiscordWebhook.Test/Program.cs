using DiscordWebhook;
using Newtonsoft.Json;

//Get webhook credentials.
string[] webookCredentials = (await File.ReadAllTextAsync("./resources/WebhookCredentials.txt")).Split('/'); //Format is '{id}/{token}'
long id = long.Parse(webookCredentials[0]); //Assume what will be entered is a valid long.
string token = webookCredentials[1];

//Create a new webhook instance with your webhooks id and token. Obtained from '.../webhooks/{webhook.id}/{webhook.token}'.
Webhook webhook = new Webhook(id, token);

//Attatch some files (returns the file ID) and then get the attatchment URL -> attachment://{id}.
string discordLogoFileAttatchmentURL = Webhook.GetAttatchmentURLForFile(await webhook.AddFile("./resources/DiscordLogo.png"));
string discordFileAttatchmentURL = Webhook.GetAttatchmentURLForFile(await webhook.AddFile("./resources/Discord.png"));

//Override the server set webhook avatar and username.
//webhook.avatarUrl = discordLogoFileAttatchmentURL; //Cannot be an attatchment URL.
webhook.username = "A discord webhook.";

//Set some plain text content and enable TTS.
webhook.content = "A webhook message @everyone.";
webhook.tts = true;

//Add a mention parser, in this example we will parse the {webhook.content} for @everyone.
Mention mention = new Mention();
mention.AddParse(Mention.EParseOptions.everyone);
webhook.allowedMentions = mention;

//Add an embed.
webhook.AddEmbed(new Embed
{
    title = "An embed title.",
    description = "An embed description.",
    color = 5793266,
    footer = new Footer("An embed footer.") { iconUrl = discordLogoFileAttatchmentURL },
    image = new Media(discordFileAttatchmentURL),
    author = new Author("An author."),
    fields = new List<Field> { new Field("An embed field.", "An embed field value.") }
});

//Post the webhook and wait for the response.
HttpResponseMessage response = await webhook.Send();
Console.WriteLine(response.StatusCode);
//Format the JSON response.
Console.WriteLine(JsonConvert.SerializeObject(JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result), Formatting.Indented));
