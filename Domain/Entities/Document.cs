using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Security.AccessControl;

    namespace Domain.Entities
    {
        /// <summary>
        /// Represents a document resource (pdf, docx, xlsx, txt, etc.)
        /// Inherits all Cloudinary properties from Resource
        /// </summary>
        public class Document : Resource
        {
            /// <summary>
            /// Number of pages (for PDFs)
            /// </summary>
            public int? PageCount { get; set; }
        }
    }
}
