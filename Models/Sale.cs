//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HolaAPI.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Sale
    {
        public string PNR { get; set; }
        public int product_fk { get; set; }
        public int persons { get; set; }
        public decimal price { get; set; }
        public string sale_type { get; set; }
        public System.DateTime date_sale { get; set; }
        public System.DateTime date_update { get; set; }
        public decimal paid { get; set; }
        public bool canceled { get; set; }
        public bool deleted { get; set; }
    }
}