using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zalohovac_editor.Models
{
    public class BackupRetention
    {
        public int Count { get; set; }
        public int Size { get; set; }
    }
}
// U třídy neděláme konstruktory, protože se používají jako DTO(Data Transfer Objekt
// - Objekt, který jen „přenáší“ data z místa na místo) pro serializaci a deserializaci JSON dat.
//„Knihovna System.Text.Json potřebuje bezparametrický konstruktor,
//aby mohla vytvořit instanci třídy dříve, než do ní začne vkládat data. S vlastím konstruktorem by to hodilo chybu