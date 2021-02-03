using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mini_Kvantor
{
    public  class DataName
    {
        public int PRN { get; set; }
        public int CL { get; set; }
        public int MJD { get; set; }
        public int STTIME { get; set; }
        public int TRKL { get; set; }
        public int ELV { get; set; }
        public int AZTH { get; set; }
        public int REFSV { get; set; }
        public int SRSV { get; set; }
        public int REFGPS { get; set; }
        public int SRGPS { get; set; }
        public int DSG { get; set; }
        public int IOE { get; set; }
        public int MDTR { get; set; }
        public int SMDT { get; set; }
        public int MDIO { get; set; }
        public int SMDI { get; set; }
        public int CK { get; set; }


        public DataName(int prn, int cl, int mjd, int sttime, int trkl,
                         int elv, int azth, int refsv, int srsv, int refgps,
                         int srgps, int dsg, int ioe, int mdtr, int smdt,
                         int mdio, int smdi, int ck)
        {
            this.PRN = prn;
            this.CL = cl;
            this.MJD = mjd;
            this.STTIME = sttime;
            this.TRKL = trkl;
            this.ELV = elv;
            this.AZTH = azth;
            this.REFSV = refsv;
            this.SRSV = srsv;
            this.REFGPS = refgps;
            this.SRGPS = srgps;
            this.DSG = dsg;
            this.IOE = ioe;
            this.MDTR = mdtr;
            this.SMDT = smdt;
            this.MDIO = mdio;
            this.SMDI = smdi;
            this.CK = ck;

        }
        public DataName() { }

        public override string ToString()
        {
            return ($"PRN:{PRN} CL:{CL} MJD:{MJD} STTIME:{STTIME} TRKL:{TRKL}" +
                $"ELV:{ELV} AZTH:{AZTH} REFSV:{REFSV} SRSV:{SRSV} REFGPS:{REFGPS} SRGPS:{SRGPS}  " +
                $"DSG:{DSG} IOE:{IOE} MDTR:{MDTR} SMDT:{SMDT} MDIO:{MDIO} SMDI:{SMDI} CK:{CK}");


        }
    }
}
