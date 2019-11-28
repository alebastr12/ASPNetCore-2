using System;
using System.Collections.Generic;
using System.Text;

namespace WebStore.Domain.Models
{
    public class BreadCrumbViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public BreadCrumbType BreadCrumbType { get; set; }
    }
}
