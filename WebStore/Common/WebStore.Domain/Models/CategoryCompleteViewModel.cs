using System;
using System.Collections.Generic;
using System.Text;

namespace WebStore.Domain.Models
{
    public class CategoryCompleteViewModel
    {
        public IEnumerable<CategoryViewModel> Categories { get; set; }

        public int? CurrentParrentCategory { get; set; }

        public int? CurrentcategoryId { get; set; }
    }
}
