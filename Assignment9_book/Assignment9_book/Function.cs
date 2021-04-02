using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Lambda.APIGatewayEvents;
using System.Net.Http;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Assignment9_book
{
    public class Function
    {

        public static readonly HttpClient client = new HttpClient();
        public async Task<ExpandoObject> FunctionHandler(APIGatewayProxyRequest input, ILambdaContext context)
        {
            string list = "";
            Dictionary<string, string> dict = (Dictionary<string, string>)input.QueryStringParameters;
            dict.TryGetValue("list", out list);

            HttpResponseMessage httpResponse = await client.GetAsync("https://api.nytimes.com/svc/books/v3/lists/"+ list + ".json?api-key=buM7NOsyelhN3OAGrt3oGViXQjIyajlR");
            httpResponse.EnsureSuccessStatusCode();
            string responseBody = await httpResponse.Content.ReadAsStringAsync();

            Document myDoc = Document.FromJson(responseBody);
            dynamic nyObject = JsonConvert.DeserializeObject<ExpandoObject>(myDoc.ToJson());
            return nyObject;
        }
    }
}
