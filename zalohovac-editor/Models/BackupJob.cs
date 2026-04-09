using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace zalohovac_editor.Models
{
    public class BackupJob
    {
        //we do not have a constructor for this class, because it is used as a DTO (Data Transfer Object)
        //for serialization and deserialization of JSON data.
        //for readability

        //using lists, because we can have multiple sources and targets

       
        public List<string> Sources { get; set; }

      
        public List<string> Targets { get; set; }


    
        public BackupMethod Method { get; set; }

        //CRON časování 
        public string Timing { get; set; }


      
        public BackupRetention Retention { get; set; }
    }
}
