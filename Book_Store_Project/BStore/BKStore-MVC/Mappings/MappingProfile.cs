using AutoMapper;
using BKStore_MVC.Models;
using BKStore_MVC.ViewModel;
namespace BKStore_MVC.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Book, BookWithAuthorWithPuplisherWithCategVM>()
           .ForMember(dest => dest.BookImagePath, opt => opt.MapFrom(src => src.ImagePath))
           .ForMember(dest => dest.CategoryName, opt => opt.Ignore()); // We'll set this manually

            CreateMap<Category, BookWithAuthorWithPuplisherWithCategVM>()
                .ForMember(dest => dest.CategoryID, opt => opt.MapFrom(src => src.CategoryID))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Name));
            CreateMap<Book, BookCategVM>()
             .ForMember(dest => dest.books, opt => opt.MapFrom(src => new List<Book> { src }))
             .ForMember(dest => dest.categories, opt => opt.Ignore()) // Assuming categories are handled separately
             .ForMember(dest => dest.SearchName, opt => opt.Ignore()); // Assuming SearchName is set separately
            CreateMap<Book, BookWithAuthorWithPuplisherWithCategVM>()
                .ForMember(dest => dest.BookID, opt => opt.MapFrom(src => src.BookID))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.AuthorName))
                .ForMember(dest => dest.StockQuantity, opt => opt.MapFrom(src => src.StockQuantity))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.BookImagePath, opt => opt.MapFrom(src => src.ImagePath)) // Map ImagePath to BookImagePath
                .ForMember(dest => dest.PublisherName, opt => opt.MapFrom(src => src.PublisherName))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.CategoryID, opt => opt.MapFrom(src => src.CategoryID))
                .ForMember(dest => dest.categories, opt => opt.Ignore()) // Categories will be set separately
                .ForMember(dest => dest.ImagePath, opt => opt.Ignore()); // Ignore ImagePath

            CreateMap<BookWithAuthorWithPuplisherWithCategVM, Book>()
            .ForMember(dest => dest.BookID, opt => opt.Ignore()) // Assuming BookID is not updated
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.AuthorName))
            .ForMember(dest => dest.StockQuantity, opt => opt.MapFrom(src => src.StockQuantity))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.PublisherName, opt => opt.MapFrom(src => src.PublisherName))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src.BookImagePath))
            .ForMember(dest => dest.CategoryID, opt => opt.MapFrom(src => src.CategoryID));
            CreateMap<Category, BookWithCategoryVM>()
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryID))
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.books, opt => opt.Ignore()); // Books will be set separately

            CreateMap<Book, BookWithCategoryVM>()
                .ForMember(dest => dest.books, opt => opt.MapFrom(src => new List<Book> { src }))
                .ForMember(dest => dest.CategoryId, opt => opt.Ignore())
                .ForMember(dest => dest.CategoryName, opt => opt.Ignore());
            CreateMap<Category, Category>()
            .ForMember(dest => dest.CategoryID, opt => opt.Ignore()); // Assuming CategoryID is not updated
            
        }
    }
}
