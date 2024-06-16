using Microsoft.AspNetCore.Mvc;
using OpenAIDemo.Services;
using System.Text;

namespace OpenAIDemo.Controllers;

[ApiController]
[Route("[controller]")]
public class DemoController : ControllerBase
{
    private readonly IOpeAiService _opeAiService;

    public DemoController(IOpeAiService opeAiService)
    {
        _opeAiService=opeAiService;
    }
private static readonly HttpClient client = new HttpClient();
    [HttpPost()]
    [Route("CompleteSentence")]
    public async Task<IActionResult> CompleteSentence( string prompt)
    {
        string apiKey = "xxx";
        string endpoint = "https://api.openai.com/v1/chat/completions";
        
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
        
         var requestData = new
        {
            model = "gpt-3.5-turbo",
            messages = new[]
            {
                new { role = "system", content = @"You will transform text into json
Input Example:

'Give me the list of members whose account is Alabama and whose first name contains Mr.'

Desired Output Example:
{
ACCT.State is AL
AND MBR.First Name is containing Mr
}
Instructions:

1. Convert the state name to its two-letter abbreviation.
2. Use the format ACCT.State is [State Abbreviation].
3. Use the format MBR.First Name is containing [String] for the name condition.
4. Combine conditions with AND.
Additional Examples:

Input: 'List members from Texas whose last name contains 'Smith'.'
Output:
{
ACCT.State is TX
AND MBR.Last Name is containing Smith
}
Input: 'Show me members with accounts in California and first names containing 'Ann'.'
Output:
{
ACCT.State is CA
AND MBR.First Name is containing Ann
}
Input: 'Give me members whose account state is New York and last name includes 'Doe'.'
Output:
{
ACCT.State is NY
AND MBR.Last Name is containing Doe
}
User Input: 'Give me the list of members whose account is Alabama and whose first name contains Mr.'

Output:
{
ACCT.State is AL
AND MBR.First Name is containing Mr
}" },
                new { role = "user", content = prompt }
            },
            max_tokens = 500
        };

        var json = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await client.PostAsync(endpoint, content);

        
        if (response.IsSuccessStatusCode)
        {
            string responseString = await response.Content.ReadAsStringAsync();
            return Ok(responseString);
            Console.WriteLine(responseString);
        }
        else
        {
            Console.WriteLine($"Error: {response.StatusCode}");
            Console.WriteLine(await response.Content.ReadAsStringAsync());
            return Ok(await response.Content.ReadAsStringAsync());
        }
        // return Ok(prompt);
        // var result= await _opeAiService.GetCompletion(prompt);
        // return Ok(result);
    }

    // [HttpGet]
    // public string Get()
    // {
    //     return "Hello World";
    // }
}