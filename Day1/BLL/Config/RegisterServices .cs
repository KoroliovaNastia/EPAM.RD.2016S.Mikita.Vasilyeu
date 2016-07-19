using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Config
{
    public class RegisterServices : ConfigurationSection
    {
        [ConfigurationProperty("Services")]
        public ServiceCollection ServicesItems => ((ServiceCollection)(base["Services"]));

        public static RegisterServices GetConfig()
        {
            return (RegisterServices)ConfigurationManager.GetSection("RegisterServices") ?? new RegisterServices();
        }
    }
}
