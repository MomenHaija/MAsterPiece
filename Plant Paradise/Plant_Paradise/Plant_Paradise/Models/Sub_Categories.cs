//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Plant_Paradise.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Sub_Categories
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Sub_Categories()
        {
            this.Products = new HashSet<Product>();
        }
    
        public int SubCategory_id { get; set; }
        public Nullable<int> Category_id { get; set; }
        public string SubCategory_image { get; set; }
        public string SubCategory_Name { get; set; }
        public string SubCategory_Description { get; set; }
    
        public virtual Category Category { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product> Products { get; set; }
    }
}
