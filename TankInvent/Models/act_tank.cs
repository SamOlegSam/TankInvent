//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TankInvent.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class act_tank
    {
        public System.Guid id { get; set; }
        public System.DateTime dt { get; set; }
        public System.DateTime dtwrite { get; set; }
        public string dtsample { get; set; }
        public Nullable<int> tank { get; set; }
        public Nullable<int> unitid { get; set; }
        public Nullable<int> number { get; set; }
        public Nullable<int> qpnumber { get; set; }
        public int shift { get; set; }
        public Nullable<double> levelstart { get; set; }
        public Nullable<double> levelend { get; set; }
        public Nullable<double> wlevelstart { get; set; }
        public Nullable<double> wlevelend { get; set; }
        public Nullable<double> vol { get; set; }
        public Nullable<double> mas { get; set; }
        public Nullable<double> temp { get; set; }
        public Nullable<double> pres { get; set; }
        public Nullable<double> dens { get; set; }
        public Nullable<double> dens20 { get; set; }
        public Nullable<double> masnetto { get; set; }
        public Nullable<double> masbalast { get; set; }
        public Nullable<System.Guid> qpid { get; set; }
        public Nullable<double> maslosses { get; set; }
        public Nullable<double> masborder { get; set; }
    }
}