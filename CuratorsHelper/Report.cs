//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CuratorsHelper
{
    using System;
    using System.Collections.Generic;
    
    public partial class Report
    {
        public int id_report { get; set; }
        public Nullable<int> id_group { get; set; }
        public Nullable<int> mounth { get; set; }
        public Nullable<System.DateTime> date { get; set; }
        public string text_report { get; set; }
        public Nullable<double> hours_week { get; set; }
        public Nullable<double> hours_day { get; set; }
        public string check_end { get; set; }
    
        public virtual Groups Groups { get; set; }
        public virtual Mounth Mounth1 { get; set; }
    }
}