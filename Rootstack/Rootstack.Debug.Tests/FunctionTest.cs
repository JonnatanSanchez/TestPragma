using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.TestUtilities;
using Newtonsoft.Json;
using GoogleScraping.Lambda;
using Xunit;

namespace GoogleScraping.Debug.Tests
{
    public class FunctionTest
    {
        [Fact]
        public void Categorias()
        {
            InvokeAPIGatewayRequest();
        }

        [Fact]
        public void Scrapear()
        {
            InvokeAPIGatewayRequest();
        }

        [Fact]
        public void PorCategoria()
        {
            InvokeAPIGatewayRequest();
        }

        [Fact]
        public void PorSubCategoria()
        {
            InvokeAPIGatewayRequest();
        }

        [Fact]
        public void GetCategoriesDB()
        {
            InvokeAPIGatewayRequest();
        }
        
        private APIGatewayProxyResponse InvokeAPIGatewayRequest()
        {
            StackTrace stackTrace = new StackTrace();

            string TestCase = stackTrace.GetFrame(1).GetMethod().Name;
            TestLambdaContext context = new TestLambdaContext();
            var lambdaFunction = new Function();
            var filePath = Path.Combine(Path.GetDirectoryName(this.GetType().GetTypeInfo().Assembly.Location), "JsonTest\\TestData.json");
            var requestStr = File.ReadAllText(filePath);

            var request = JsonConvert.DeserializeObject<Dictionary<string, APIGatewayProxyRequest>>(requestStr);
            APIGatewayProxyRequest reques = request.Where(x => x.Key == TestCase).FirstOrDefault().Value;

            return lambdaFunction.FunctionHandler(reques, context);
        }
    }
}
