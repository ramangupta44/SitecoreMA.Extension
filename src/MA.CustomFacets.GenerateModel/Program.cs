using MA.CustomFacets.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// install Sitecore.XConnect nuget package
// add reference to MA.CustomFacets project

namespace MA.CustomFacets.GenerateModel
{
    class Program
    {
        static void Main(string[] args)
        {
            var model = Sitecore.XConnect.Serialization.XdbModelWriter.Serialize(CustomerModel.Model);
            File.WriteAllText(CustomerModel.Model.FullName + ".json", model);
            Console.WriteLine("Completed");
            Console.Read();
        }
    }
}
