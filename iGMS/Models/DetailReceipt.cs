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
    
    public partial class DetailReceipt
    {
        public int Id { get; set; }
        public string IdReceipt { get; set; }
        public Nullable<int> Amount { get; set; }
        public string idGood { get; set; }
        public Nullable<bool> Status { get; set; }
    
        public virtual Good Good { get; set; }
        public virtual Receipt Receipt { get; set; }
    }
}