using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mini_Kvantor
{
    public class RinexData
    {   
        public int SAT { get; set; }
        public int CL { get; set; }
        public int MJD { get; set; }
        public int STTIME { get; set; }
        public int TRKL { get; set; }
        public int ELV { get; set; }
        public int AZTH { get; set; }
        public int REFSV { get; set; }
        public int SRSV { get; set; }
        public int REFSYS { get; set; }
        public int SRSYS { get; set; }
        public int DSG { get; set; }
        public int IOE { get; set; }
        public int MDTR { get; set; }
        public int SMDT { get; set; }
        public int MDIO { get; set; }
        public int SMDI { get; set; }
        public int MSIO { get; set; }
        public int SMSI { get; set; }
        public int ISG { get; set; }
        public int FR { get; set; }
        public int HC { get; set; }
        public string FRC { get; set; }
        public int CK { get; set; }
        
        public RinexData(int sat,int cl,int mjd,int sttime,int trkl,int elv,int azth,
                   int refsv,int srsv,int refsys,int srsys,int dsg,int ioe,int mdtr,
                int smdt,int mdio,int smdi,int msio,int smsi,int isg,int fr,int hc,string frc,int ck)
        {
            this.SAT = sat;
            this.CL = cl;
            this.MJD = mjd;
            this.STTIME = sttime;
            this.TRKL = trkl;
            this.ELV = elv;
            this.AZTH = azth;
            this.REFSV = refsv;
            this.SRSV = srsv;
            this.REFSYS = refsys;
            this.SRSYS = srsys;
            this.DSG = dsg;
            this.IOE = ioe;
            this.MDTR = mdtr;
            this.SMDT = smdt;
            this.MDIO = mdio;
            this.SMDI = smdi;
            this.MSIO = msio;
            this.SMSI = smsi;
            this.ISG = isg;
            this.FR = fr;
            this.HC = hc;
            this.FRC = frc;
            this.CK = ck;

        }

        public RinexData() { }

        public override string ToString()
        {
            return ($"SAT:{SAT},CL:{CL},MJD:{MJD},STTIME:{STTIME},TRKL:{TRKL}," +
                $"ELV:{ELV},AZTH:{AZTH},REFSV:{REFSV},SRSV:{SRSV}," +
                $"REFSYS:{REFSYS},SRSYS:{SRSYS},DSG:{DSG},IOE:{IOE}," +
                $"MDTR:{MDTR},SMDT:{SMDT},MDIO:{MDIO},SMDI:{SMDI},MSIO:{MSIO}," +
                $" SMSI:{SMSI},ISG:{ISG},FR:{FR},HC:{HC},FRC:{FRC},CK:{CK}");
        }
    }
}
