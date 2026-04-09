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


       
        public List<string> Sources { get; set; }

      
        public List<string> Targets { get; set; }


    
        public BackupMethod Method { get; set; }

        //CRON časování 
        public string Timing { get; set; }


      
        public BackupRetention Retention { get; set; }
    }
}
