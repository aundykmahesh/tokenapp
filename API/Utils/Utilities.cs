using Microsoft.Extensions.Configuration;

namespace API.Utils
{
    public static class Utilities
    {
        public static string GetTokenKey(IConfiguration config=null){
                //todo take from azure keyvalult
                if (config==null)
                {
                    return config["tokenkey"];    

                }
                return "return from secured location";
        }
    }
}