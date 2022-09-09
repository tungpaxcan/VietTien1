//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace iGMS.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Customer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Customer()
        {
            this.Bills = new HashSet<Bill>();
        }
    
        public string Id { get; set; }
        public Nullable<int> IdGroupGoods { get; set; }
        public string IdGeneral { get; set; }
        public string IdInternal { get; set; }
        public string Name { get; set; }
        public string NameTransaction { get; set; }
        public Nullable<double> Money { get; set; }
        public Nullable<double> Point { get; set; }
        public string AddRess { get; set; }
        public Nullable<int> TaxCode { get; set; }
        public string Fax { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Represent { get; set; }
        public string Position { get; set; }
        public string Website { get; set; }
        public Nullable<int> STK { get; set; }
        public string Bank { get; set; }
        public Nullable<double> DebtFrom { get; set; }
        public Nullable<double> DebtTo { get; set; }
        public Nullable<double> Deposit { get; set; }
        public Nullable<System.DateTime> DatePay { get; set; }
        public Nullable<double> Discount { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public string ModifyBy { get; set; }
        public Nullable<bool> Status { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Bill> Bills { get; set; }
        public virtual GeneralAccounting GeneralAccounting { get; set; }
        public virtual GroupGood GroupGood { get; set; }
        public virtual InternalAccounting InternalAccounting { get; set; }
    }
}
