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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Sale()
        {
            this.Payments = new HashSet<Payment>();
        }
    
        public int ID { get; set; }
        public string PNR { get; set; }
        public int agency_fk { get; set; }
        public int product_fk { get; set; }
        public Nullable<int> plan_fk { get; set; }
        public int persons { get; set; }
        public decimal remained_pay { get; set; }
        public string sale_type { get; set; }
        public System.DateTime date_sale { get; set; }
        public System.DateTime date_update { get; set; }
        public bool canceled { get; set; }
        public string comments { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual Sale Sales1 { get; set; }
        public virtual Sale Sale1 { get; set; }
    }
}
