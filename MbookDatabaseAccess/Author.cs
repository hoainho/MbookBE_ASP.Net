//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MbookDatabaseAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class Author
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Author()
        {
            this.Products = new HashSet<Product>();
        }
    
        public System.Guid idAuthor { get; set; }
        public string name { get; set; }
        public string age { get; set; }
        public string slogan { get; set; }
        public string image { get; set; }
        public string description { get; set; }
        public string createdby { get; set; }
        public Nullable<System.DateTime> createddate { get; set; }
        public string modifiedby { get; set; }
        public Nullable<System.DateTime> modifieddate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product> Products { get; set; }
    }
}
