using TankInvent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Data.OleDb;
using System.Data;
using System.Data.SqlClient;
using System.Data.Entity;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Web;
using Newtonsoft.Json;

namespace TankInvent
{
    public class Tank
    {
        public static asopnEntities   db1 = new asopnEntities();
        public static ChemLabEntities db2 = new ChemLabEntities();
        public class TankInventt
        {
            public DateTime Data { get; set; }
            public int Filial { get; set; }
            public int Rezer { get; set; }
            public int? Urov { get; set; }
            public int? UrovH2O { get; set; }
            public int? UrovNeft { get; set; }
            public double V { get; set; }
            public double VH2O { get; set; }
            public double VNeft { get; set; }
            public double Temp { get; set; }
            public double P { get; set; }
            public double MassaBrutto { get; set; }
            public double H2O { get; set; }
            public double Salt { get; set; }
            public double Meh { get; set; }
            public double BalProc { get; set; }
            public double BalTonn { get; set; }
            public double MassaNetto { get; set; }
            public double Hmin;
            public double Vmin;
            public double dMBalmin;
            public double dMNettomin;
            public double MBalTeh { get; set; }
            public double MNettoTeh { get; set; }
            public int type {get; set;}           
        
    }

    //--------------------------------------------------------------
    public class CalculatorData
    {
        /// <summary>
        /// Oil density at a temperature of 15 °C.
        /// </summary>
        public double Dens15 { get; set; }

        /// <summary>
        /// Oil density at a temperature of 20 °C.
        /// </summary>
        public double Dens20 { get; set; }

        /// <summary>
        /// Values of the coefficient K20 / K15.
        /// </summary>
        public double K20_K15 { get; set; }

        /// <summary>
        /// Values of oil expansion coefficients.
        /// </summary>
        public double Betta { get; set; }

        /// <summary>
        /// Value of oil compressibility ratios.
        /// </summary>
        public double Gamma { get; set; }

        /// <summary>
        /// Adduction density.
        /// </summary>
        public double DensTP { get; set; }
    }


    public class Density636 : ICloneable
    {
        public double DensAreom { get; set; }
        public double TempAreom { get; set; }
        public double GradAreom { get; set; }
        public double TempReal { get; set; }
        public double Pressure { get; set; }

        public Density636()
        {
            DensAreom = 0;
            TempAreom = 0;
            GradAreom = 20;
            TempReal = 0;
            Pressure = 0;
        }

        public object Clone()
        {
            return new Density636
            {
                DensAreom = this.DensAreom,
                TempAreom = this.TempAreom,
                GradAreom = this.GradAreom,
                TempReal = this.TempReal,
                Pressure = this.Pressure
            };
        }

    }
    //--------------------------------------------------------------
    
    public class Asopn
    {
        //public static asopnEntities   db1 = new asopnEntities();
        //public static ChemLabEntities db2 = new ChemLabEntities();

        public static List<LastTanksResultMoz>  LM  = new List<LastTanksResultMoz>();  //список записей из представления химанализа по Мозырю
        public static List<LastTanksResultPol>  LP  = new List<LastTanksResultPol>();  //список записей из представления по Полоцку
        public static List<LastMechanResultMoz> LMM = new List<LastMechanResultMoz>(); //список записей механических примесей в резервуарах ЛПДС Мозырь

        public static LastTanksResultMoz  LastM  = new LastTanksResultMoz();  //экземпляр класса химанализ Мозыря
        public static LastTanksResultPol  LastP  = new LastTanksResultPol();  //экземпляр класса химанализ Полоцка
        public static LastMechanResultMoz LastMM = new LastMechanResultMoz(); //экземпляр класса механических примесей Мозыря


        public static List<TankInventt> TankI = new List<TankInventt>();

        public static List<tankinfo> tankinfos = new List<tankinfo>();
        public static taginfo TI = new taginfo();

        public static trl_tank trl = new trl_tank();       

        static List<calibration> calibrationList = new List<calibration>();

        static List<TankInv> ListInv = new List<TankInv>(); 
        static List<TankInv> ListLastInv = new List<TankInv>();


            public async static Task<List<TankInventt>> Spis()
        {
            TI = db1.taginfo.OrderByDescending(h => h.dt).FirstOrDefault();
            tankinfos = db1.tankinfo.Where(j => j.dt == TI.dt).OrderBy(j => j.filialid).ThenBy(j => j.tankid).ToList();

            LP = db2.LastTanksResultPol.ToList();
            LM = db2.LastTanksResultMoz.ToList();
            LMM = db2.LastMechanResultMoz.ToList();

            ListInv = db1.TankInv.OrderByDescending(g => g.Data).ToList();
            DateTime LastDataInv = ListInv.FirstOrDefault().Data; //Получаем дату последней инвентаризации
            ListLastInv = ListInv.Where(f => f.Data == LastDataInv).ToList(); //Получаем список инвентаризации за последнюю дату;


            calibrationList = db1.calibration.OrderBy(f => f.tankid).ThenBy(h => h.oillevel).ToList();
            decimal? UrovH2O;
            double UrovNeft;
            double V;
            double VH2O;
            double VNeft;
            double Umin;
            double Umax;
            double Vmin;
            double Vmax;            
            double Upercent;
            double UminH2O;
            double UmaxH2O;
            double VminH2O;
            double VmaxH2O;
            double UpercentH2O;
            double Temp1;
            double P;
            double MassaBrutto;
            double H2O;
            double Salt;
            double Meh;
            double BalProc;
            double BalTonn;
            double MassaNetto;
            double Hmin;
            double MVmin;
            double MMin;
            double MNmin;
            int type;
            foreach (var item in tankinfos)
            {
                    UrovH2O = ListLastInv.FirstOrDefault(d => d.Filial == item.filialid & d.Rezer == item.tankid).UrovH2O;
                    //Вывод общего объема нефти и подтоварной воды согласно общего уровня-------------------------------------------------------------
                    if (db1.trl_tank.FirstOrDefault(g => g.TankID == item.tankid & g.FilialID == item.filialid) == null)
                    {
                        type = 77;
                    }
                    else
                    {
                        type = db1.trl_tank.FirstOrDefault(g => g.TankID == item.tankid & g.FilialID == item.filialid).TypeID;
                    }

                if (calibrationList.LastOrDefault(g => g.tankid == item.tankid & g.filialid ==item.filialid) == null || calibrationList.FirstOrDefault(j => j.tankid == item.tankid & j.filialid == item.filialid) == null)
                {
                    V = 0;
                    Temp1 = 0;
                }
                else
                {
                    if (calibrationList.LastOrDefault(g => g.tankid == item.tankid & g.oillevel <= item.level & g.filialid == item.filialid) == null)
                    {
                        Umin = 0;
                    }
                    else
                    {
                        Umin = calibrationList.LastOrDefault(g => g.tankid == item.tankid & g.oillevel <= item.level & g.filialid == item.filialid).oillevel;
                    }
                    Umax = calibrationList.FirstOrDefault(j => j.tankid == item.tankid & j.oillevel > item.level & j.filialid == item.filialid).oillevel;
                    if (calibrationList.LastOrDefault(g => g.tankid == item.tankid & g.oillevel <= item.level & g.filialid == item.filialid) == null)
                    {
                        Vmin = 0;
                    }
                    else
                    {
                        Vmin = calibrationList.LastOrDefault(g => g.tankid == item.tankid & g.oillevel <= item.level & g.filialid == item.filialid).oilvolume;
                    }

                    Vmax = calibrationList.FirstOrDefault(j => j.tankid == item.tankid & j.oillevel > item.level & j.filialid == item.filialid).oilvolume;
                    Upercent = (Convert.ToDouble(item.level) - Umin) / (Umax - Umin);
                    V = Vmin + (Vmax - Vmin) * Upercent;
                    Temp1 = Convert.ToDouble(item.t);           
                }

                    //-------Вывод объема подтоварной воды--------------------------------------------------------------------------------------------------------
                    //
                    if (calibrationList.LastOrDefault(g => g.tankid == item.tankid & g.filialid == item.filialid) == null || calibrationList.FirstOrDefault(j => j.tankid == item.tankid & j.filialid == item.filialid) == null || ListLastInv.FirstOrDefault(d=>d.Filial == item.filialid & d.Rezer == item.tankid).UrovH2O == 0)
                    {
                        VH2O = 0;
                    }
                    else
                    {
                        if (calibrationList.LastOrDefault(g => g.tankid == item.tankid & g.oillevel <= ListLastInv.FirstOrDefault(d => d.Filial == item.filialid & d.Rezer == item.tankid).UrovH2O & g.filialid == item.filialid) == null)
                        {
                            UminH2O = 0;
                        }
                        else
                        {
                            UminH2O = calibrationList.LastOrDefault(g => g.tankid == item.tankid & g.oillevel <= ListLastInv.FirstOrDefault(d => d.Filial == item.filialid & d.Rezer == item.tankid).UrovH2O & g.filialid == item.filialid).oillevel;
                        }
                        UmaxH2O = calibrationList.FirstOrDefault(j => j.tankid == item.tankid & j.oillevel > ListLastInv.FirstOrDefault(d => d.Filial == item.filialid & d.Rezer == item.tankid).UrovH2O & j.filialid == item.filialid).oillevel;
                        if (calibrationList.LastOrDefault(g => g.tankid == item.tankid & g.oillevel <= ListLastInv.FirstOrDefault(d => d.Filial == item.filialid & d.Rezer == item.tankid).UrovH2O & g.filialid == item.filialid) == null)
                        {
                            VminH2O = 0;
                        }
                        else
                        {
                            VminH2O = calibrationList.LastOrDefault(g => g.tankid == item.tankid & g.oillevel <= ListLastInv.FirstOrDefault(d => d.Filial == item.filialid & d.Rezer == item.tankid).UrovH2O & g.filialid == item.filialid).oilvolume;
                        }

                        VmaxH2O = calibrationList.FirstOrDefault(j => j.tankid == item.tankid & j.oillevel > ListLastInv.FirstOrDefault(d => d.Filial == item.filialid & d.Rezer == item.tankid).UrovH2O & j.filialid == item.filialid).oilvolume;
                        UpercentH2O = (Convert.ToDouble(ListLastInv.FirstOrDefault(d => d.Filial == item.filialid & d.Rezer == item.tankid).UrovH2O) - UminH2O) / (UmaxH2O - UminH2O);
                        VH2O = VminH2O + (VmaxH2O - VminH2O) * UpercentH2O;                        
                    }

                    //---------------------------------------------------------------------------------------------------------------
                    //Вывод плотности нефти при температуре 20----------------------------------------------------------------------------
                    //Temp = 0;
                    P = 0;
                H2O = 0;
                Salt = 0;
                Meh = 0;
                if (item.filialid == 1)
                {
                    foreach (var i in LM.Where(f => f.tankid == item.tankid))
                    {

                        P = i.dens20.Value;
                        if (i.water == null)
                        {
                            H2O = 0;
                        }
                        else
                        {
                            H2O = i.water.Value;
                        }
                        if (i.saltmg == null)
                        {
                            Salt = 0;
                        }
                        else
                        {
                            Salt = i.saltmg.Value;
                        }
                            // записываем значение механических примесей резервуара ЛПДС Мозырь
                            if (LMM.FirstOrDefault(g => g.tankid == item.tankid) == null)
                            {
                                Meh = 0;
                            }
                            else
                            {
                                Meh = LMM.FirstOrDefault(g => g.tankid == item.tankid).mechan.Value;
                            }
                           
                        //if (i.mechan == null)
                        //{
                        //    Meh = 0;
                        //}
                        //else
                        //{
                        //    Meh = i.mechan.Value;
                        //}
                        }
                }
                else
                {
                    foreach (var i in LP.Where(f => f.tankid == item.tankid))
                    {

                        P = i.dens20.Value;
                        if (i.water == null)
                        {
                            H2O = 0;
                        }
                        else
                        {
                            H2O = i.water.Value;
                        }
                        if (i.saltmg == null)
                        {
                            Salt = 0;
                        }
                        else
                        {
                            Salt = i.saltmg.Value;
                        }
                        if (i.mechan == null)
                        {
                            Meh = 0;
                        }
                        else
                        {
                            Meh = i.mechan.Value;
                        }
                    }
                }

                    //-----------------------------------------------------------
                    var dens636 = new Density636()
                    {
                        DensAreom = P,
                        TempAreom = 20,
                        GradAreom = 20,
                        TempReal = Temp1,
                        Pressure = 0
                    };
                    var json = JsonConvert.SerializeObject(dens636);
                    var data = new StringContent(json, Encoding.UTF8, "application/json");

                    string urlDensCalc = "http://192.168.150.144:88/denscalc";
                    var clientDensCalc = new HttpClient();

                    
                        var response = await clientDensCalc.PostAsync(urlDensCalc, data);

                        string result = response.Content.ReadAsStringAsync().Result;
                        var calculatorData = JsonConvert.DeserializeObject<CalculatorData>(result);
                    double PCalc;
                    if (calculatorData == null)
                    {
                        PCalc = 0;
                    }
                    else
                    {
                        PCalc = calculatorData.DensTP;
                    }
                    double SaltProc;
                    if (PCalc == 0)
                    { 
                        SaltProc = 0; 
                    }
                    else
                    {
                        SaltProc = Salt / PCalc / 10;
                    }
                    //-----------------------------------------------------------

                MassaBrutto = PCalc * V / 1000;
                BalProc = H2O + SaltProc + +Meh;
                BalTonn = MassaBrutto * BalProc / 100;
                MassaNetto = MassaBrutto - BalTonn;
                if (db1.trl_tank.FirstOrDefault(f => f.FilialID == item.filialid & f.TankID == item.tankid) == null)
                {
                    Hmin = 0;
                }
                else
                {
                    Hmin = Convert.ToDouble(db1.trl_tank.FirstOrDefault(f => f.FilialID == item.filialid & f.TankID == item.tankid).MinDopLevel);
                }
                if (db1.trl_tank.FirstOrDefault(f => f.FilialID == item.filialid & f.TankID == item.tankid) == null)
                {
                    MVmin = 0;
                }
                else
                {
                    MVmin = Convert.ToDouble(db1.trl_tank.FirstOrDefault(f => f.FilialID == item.filialid & f.TankID == item.tankid).MinDopVol);
                }
                    
                MMin = PCalc * MVmin / 1000;
                MNmin = MMin - (MMin * BalProc / 100);
                //MNmin = MMin * (100 - BalProc) / 100;
                    //VH2O = 0; //Задаем значение объема подтоварной воды = 0, т.к. уровень подтоварной воды нигде не указывается
                UrovNeft = Convert.ToDouble(item.level) - Convert.ToDouble(UrovH2O);
                VNeft = V - VH2O;
                    //---------------------------------------------------------------------------------------------------------------
                    TankI.Add(new TankInventt() { Data = item.dt, Filial = item.filialid, Rezer = item.tankid, Urov = item.level, UrovH2O = Convert.ToInt32(UrovH2O), UrovNeft = Convert.ToInt32(UrovNeft), V = V, P = PCalc, Temp = Temp1, MassaBrutto = MassaBrutto, H2O = H2O, Salt = SaltProc, Meh = Meh, BalProc = BalProc, BalTonn = BalTonn, MassaNetto = MassaNetto, Hmin = Hmin, Vmin = MVmin, dMBalmin = MMin, dMNettomin = MNmin, type = type, VH2O = VH2O, VNeft = VNeft }); ;
              }
            return TankI;
        }
    }

        public class Program
        {

            static async Task Main(string[] args)
            {
                string writePath = "log.txt";
                filials hhh = new filials();
                tankinfo hj = new tankinfo();
                TankInv TankinV = new TankInv();

                try
                {
                      using (StreamWriter sw = new StreamWriter(writePath, true, System.Text.Encoding.Default))
                        {
                           sw.WriteLine("Дата\t\tФилиал\tРезервуар\tУровень\tУровень воды\tУровень нефти\tОбъем\tТемпература\tПлотность\tМасса брутто\tВода\tСоль\tМехпримеси\tБаласт %\tБаласт тонн\tМасса нетто\tТип\tОбъем воды");
                        
                        foreach (var item in await Asopn.Spis())
                        //foreach (var item in Asopn.TankI)
                        {
                            //---Запись таблицы в БД---------------------------------------------------------------------

                            TankinV.Data = item.Data;
                            TankinV.Filial = item.Filial;
                            TankinV.Rezer = item.Rezer;
                            TankinV.Urov = item.Urov;
                            TankinV.UrovH2O = item.UrovH2O;
                            TankinV.UrovNeft = item.UrovNeft;
                            TankinV.V = Convert.ToDecimal(Math.Round(item.V, 3));
                            TankinV.Temp = Convert.ToDecimal(Math.Round(item.Temp, 2));
                            TankinV.P = Convert.ToDecimal(Math.Round(item.P, 2));
                            TankinV.MassaBrutto = Convert.ToDecimal(Math.Round(item.MassaBrutto, 3));
                            TankinV.H2O = Convert.ToDecimal(Math.Round(item.H2O, 4));
                            TankinV.Salt = Convert.ToDecimal(item.Salt);                            
                            TankinV.Meh = Convert.ToDecimal(Math.Round(item.Meh, 4));
                            TankinV.BalProc = Convert.ToDecimal(item.BalProc);                            
                            TankinV.BalTonn = Convert.ToDecimal(item.BalTonn);                            
                            TankinV.MassaNetto = Convert.ToDecimal(Math.Round(item.MassaNetto, 1));
                            TankinV.HMim = Convert.ToDecimal(Math.Round(item.Hmin, 0));
                            TankinV.VMin = Convert.ToDecimal(Math.Round(item.Vmin, 1));
                            TankinV.MBalMin = Convert.ToDecimal(Math.Round(item.dMBalmin, 1));
                            TankinV.MNettoMin = Convert.ToDecimal(Math.Round(item.dMNettomin, 1));
                            TankinV.type = item.type;
                            TankinV.VH2O = Convert.ToDecimal(item.VH2O);
                            TankinV.VNeft = Convert.ToDecimal(item.VNeft);

                            db1.TankInv.Add(TankinV);
                            db1.SaveChanges();

                            //----Запись таблицы в текстовый файл log.txt-------------------------------------------------------

                            object data        = item.Data;
                            object filial      = item.Filial;
                            object rezer       = item.Rezer;
                            object Urov        = item.Urov;
                            object UrovH2O     = item.UrovH2O;
                            object UrovNeft    = item.UrovNeft;
                            object V           = Math.Round(item.V, 3);
                            object Temp        = Math.Round(item.Temp, 2);
                            object P           = Math.Round(item.P, 2);
                            object MassaBrutto = Math.Round(item.MassaBrutto, 3);
                            object H2O         = Math.Round(item.H2O, 4);
                            object Salt        = Math.Round(item.Salt, 4);
                            object Meh         = Math.Round(item.Meh, 4);
                            object BalProc     = Math.Round(item.BalProc, 4);
                            object BalTonn     = Math.Round(item.BalTonn, 1);
                            object MassaNetto  = Math.Round(item.MassaNetto, 1);
                            object Hmin        = Math.Round(item.Hmin, 0);
                            object Vmin        = Math.Round(item.Vmin, 1);
                            object dMBalmin    = Math.Round(item.dMBalmin, 1);
                            object dMNettomin  = Math.Round(item.dMNettomin, 1);
                            object type        = item.type;
                            object VH2O        = Math.Round(item.VH2O, 3);
                            object VNeft       = Math.Round(item.VNeft, 3);

                            sw.WriteLine("{0} \t{1} \t{2} \t{3} \t{4} \t{5} \t{6} \t{7} \t{8} \t{9} \t{10} \t{11} \t{12} \t{13} \t{14} \t{15} \t{16} \t{17} \t{18} \t{19} \t{20}\t{21}\t{22}", data, filial, rezer, Urov, UrovH2O, UrovNeft, V, Temp, P, MassaBrutto, H2O, Salt, Meh, BalProc, BalTonn, MassaNetto, Hmin, Vmin, dMBalmin, dMNettomin, type, VH2O, VNeft);

                            sw.WriteLine("---------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                                                        
                        }
            
            Console.WriteLine("Запись успешно добавлена в БД!");
                        
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    using (StreamWriter sw = new StreamWriter(writePath, true, System.Text.Encoding.Default))
                    {
                        sw.WriteLine("Ошибка записи!!!!! Код ошибки: ", ex.ToString());
                    }
                }
                
                //using (StreamWriter sw = new StreamWriter(writePath, true, System.Text.Encoding.Default))
                //{
                //    sw.WriteLine("Дата\t\tФилиал\tРезервуар\tУровень\tУровень воды\tУровень нефти\tОбъем\tТемпература\tПлотность\tМасса брутто\tВода\tСоль\tМехпримеси\tБаласт %\tБаласт тонн\tМасса нетто");
                //    foreach (var item in Asopn.Spis())
                //    {
                //        object data = item.Data;
                //        object filial = item.Filial;
                //        object rezer = item.Rezer;
                //        object Urov = item.Urov;
                //        object UrovH2O = item.UrovH2O;
                //        object UrovNeft = item.UrovNeft;
                //        object V = Math.Round(item.V, 3);
                //        object Temp = Math.Round(item.Temp, 2);
                //        object P = Math.Round(item.P, 2);
                //        object MassaBrutto = Math.Round(item.MassaBrutto, 3);
                //        object H2O = Math.Round(item.H2O, 4);
                //        object Salt = Math.Round(item.Salt, 4);
                //        object Meh = Math.Round(item.Meh, 4);
                //        object BalProc = Math.Round(item.BalProc, 4);
                //        object BalTonn = Math.Round(item.BalTonn, 1);
                //        object MassaNetto = Math.Round(item.MassaNetto, 1);
                //        object Hmin = Math.Round(item.Hmin, 0);
                //        object Vmin = Math.Round(item.Vmin, 1);
                //        object dMBalmin = Math.Round(item.dMBalmin, 1);
                //        object dMNettomin = Math.Round(item.dMNettomin, 1);

                //        sw.WriteLine("{0} \t{1} \t{2} \t{3} \t{4} \t{5} \t{6} \t{7} \t{8} \t{9} \t{10} \t{11} \t{12} \t{13} \t{14} \t{15} \t{16} \t{17} \t{18} \t{19}", data, filial, rezer, Urov, UrovH2O, UrovNeft, V, Temp, P, MassaBrutto, H2O, Salt, Meh, BalProc, BalTonn, MassaNetto, Hmin, Vmin, dMBalmin, dMNettomin);
                //    }
                //    sw.WriteLine("---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                //}

                //using (StreamWriter sw = new StreamWriter(writePath, true, System.Text.Encoding.Default))
                //{
                //    sw.WriteLine("Дозапись");
                //    sw.Write(4.5);
                //}

            }
            
        }
    }
}
