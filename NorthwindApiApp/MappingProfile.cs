using AutoMapper;
using Northwind.DataAccess.Employees;
using Northwind.DataAccess.Products;
using Northwind.Services.Blogging;
using Northwind.Services.Employees;
using Northwind.Services.EntityFrameworkCore.Blogging.Entities;
using Northwind.Services.EntityFrameworkCore.Entities;
using Northwind.Services.Products;

namespace NorthwindApiApp
{
    /// <summary>
    /// Represents a mapping profile.
    /// </summary>
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MappingProfile"/> class.
        /// </summary>
        public MappingProfile()
        {
            this.CreateMap<EmployeeModel, EmployeeTransferObject>().ReverseMap();
            this.CreateMap<ProductModel, ProductTransferObject>().ReverseMap();
            this.CreateMap<ProductCategoryModel, ProductCategoryTransferObject>().ReverseMap();
            this.CreateMap<EmployeeModel, Employee>()
                .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ReportsToNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.EmployeeTerritories, opt => opt.Ignore())
                .ForMember(dest => dest.InverseReportsToNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.Orders, opt => opt.Ignore())
                .ReverseMap();
            this.CreateMap<ProductModel, Product>()
                .ForMember(dst => dst.ProductId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.ProductName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dst => dst.Category, opt => opt.Ignore())
                .ForMember(dst => dst.Supplier, opt => opt.Ignore())
                .ForMember(dst => dst.OrderDetails, opt => opt.Ignore())
                .ReverseMap();
            this.CreateMap<ProductCategoryModel, Category>()
                .ForMember(dst => dst.CategoryId, src => src.MapFrom(src => src.Id))
                .ForMember(dst => dst.CategoryName, src => src.MapFrom(src => src.Name))
                .ForMember(dst => dst.Products, opt => opt.Ignore())
                .ReverseMap();
            this.CreateMap<BlogArticleModel, BlogArticle>()
                .ForMember(dest => dest.BlogArticleProducts, opt => opt.Ignore())
                .ForMember(dest => dest.BlogComments, opt => opt.Ignore())
                .ReverseMap();
            this.CreateMap<BlogArticleProductModel, BlogArticleProduct>()
                .ForMember(dest => dest.BlogArticle, opt => opt.Ignore())
                .ReverseMap();
            this.CreateMap<BlogCommentModel, BlogComment>()
                .ForMember(dest => dest.BlogArticle, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
