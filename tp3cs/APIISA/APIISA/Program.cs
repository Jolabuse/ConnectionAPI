using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace APIISA
{
    class Program
    {
        private static readonly HttpClient Client = new HttpClient();
        private Root objectRes { get; set; }
        
        
        private  async void GetInfo(string currency)
        {
            objectRes = new Root();
            try
            {
                var responseBody = Client.GetAsync("https://api.lunarcrush.com/v2?data=assets&key=lnfht57eiirp715eqwevoo&symbol="+currency+"&interval=hour&data_points=24").Result;
                /*var responseBodyString = responseBody.Content.ReadAsStringAsync();
                
                
                    var res = JsonConvert.DeserializeObject(responseBodyString.Result);*/
                var res = await responseBody.Content.ReadAsStringAsync();
                objectRes = JsonConvert.DeserializeObject<Root>(res);
                if (objectRes != null)
                {
                    var data = objectRes.data;
                    foreach (var d in data)
                    {
                        Console.WriteLine("id : {0}\nname : {1}\nsymbol : {2}" +
                                          "\nprice : {3}\nprice_btc : {4}\n" +
                                          "market cap : {5}\npercent change 24h : {6}"
                            , d.id,d.name,d.symbol,d.price,d.price_btc,d.market_cap,d.percent_change_24h);
                        
                    }
                    
                }
                
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("sa a plantè");
            }
            
        }
        
        public static void Main(string[] args)
        {
            var p = new Program();
            p.GetInfo("BTC");
            double? resMax0,max,min;
            try
            {
                resMax0 = p.objectRes.data[0].timeSeries[0].high;
            
                List<Double?> Max = new List<double?>();

                foreach (var t in p.objectRes.data[0].timeSeries)
                {
                    Max.Add(t.high);
                }

                max = Max.Max();
                min = Max.Min();
            

                for (int i = 0; i < 24; i++)
                {
                    resMax0 = p.objectRes.data[0].timeSeries[i].high;
                    Console.WriteLine("{0} : {1}",i,150*resMax0/(max-min));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            

        }
        
    }
}