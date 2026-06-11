using Microsoft.AspNetCore.Mvc.ModelBinding;
using P3AddNewFunctionalityDotNetCore.Attributes;
using System.ComponentModel.DataAnnotations;

namespace P3AddNewFunctionalityDotNetCore.Models.ViewModels
{
    public class ProductViewModel
    {
        [BindNever]
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Models.Services.ProductService), ErrorMessageResourceName = "MissingName")]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Details { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Models.Services.ProductService), ErrorMessageResourceName = "MissingStock")]
        [Integer(ErrorMessageResourceType = typeof(Resources.Models.Services.ProductService), ErrorMessageResourceName = "StockNotAnInteger")]
        [PositiveNumber(ErrorMessageResourceType = typeof(Resources.Models.Services.ProductService), ErrorMessageResourceName = "StockNotGreaterThanZero")]
        public string Stock { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Models.Services.ProductService), ErrorMessageResourceName = "MissingPrice")]
        [Float(ErrorMessageResourceType = typeof(Resources.Models.Services.ProductService), ErrorMessageResourceName = "PriceNotANumber")]
        [PositiveNumber(ErrorMessageResourceType = typeof(Resources.Models.Services.ProductService), ErrorMessageResourceName = "PriceNotGreaterThanZero")]
        public string Price { get; set; }
    }
}
