using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DWClient.Models
{
    public class Measurement
    {
        public DatabaseObject<int> sifTablica { get; set; } = new DatabaseObject<int>("tablica.sifTablica");
        public DatabaseObject<int> rbrAtrib { get; set; } = new DatabaseObject<int>("tabAtribut.rbrAtrib");
        public DatabaseObject<string> imeSQLAtrib { get; set; } = new DatabaseObject<string>("tabAtribut.imeSQLAtrib");
        public DatabaseObject<int> sifTipAtrib { get; set; } = new DatabaseObject<int>("tabAtribut.sifTipAtrib");
        public DatabaseObject<string> imeAtrib { get; set; } = new DatabaseObject<string>("tabAtribut.imeAtrib");
        public DatabaseObject<int> sifAgrFun { get; set; } = new DatabaseObject<int>("agrFun.sifAgrFun");
        public DatabaseObject<string> nazAgrFun { get; set; } = new DatabaseObject<string>("agrFun.nazAgrFun");
        public DatabaseObject<string> nazTablica { get; set; } = new DatabaseObject<string>("tablica.nazTablica");
        public DatabaseObject<string> nazSQLTablica { get; set; } = new DatabaseObject<string>("tablica.nazSQLTablica");
        public DatabaseObject<int> sifTipTablica { get; set; } = new DatabaseObject<int>("tablica.sifTipTablica");
        public DatabaseObject<string> imeAtribAgrFun { get; set; } = new DatabaseObject<string>("tabAtributAgrFun.imeAtrib");

        public Measurement()
        {

        }

        public Measurement(List<string> result)
        {
            sifTablica.Value = int.Parse(result[0]);
            rbrAtrib.Value = int.Parse(result[1]);
            imeSQLAtrib.Value = result[2];
            sifTipAtrib.Value = int.Parse(result[3]);
            imeAtrib.Value = result[4];
            sifAgrFun.Value = int.Parse(result[5]);
            nazAgrFun.Value = result[6];
            nazTablica.Value = result[8];
            nazSQLTablica.Value = result[9];
            sifTipTablica.Value = int.Parse(result[10]);
            imeAtribAgrFun.Value = result[14];
        }
    }
}
