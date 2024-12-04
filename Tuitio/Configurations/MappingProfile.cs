using AutoMapper;
using Tuitio.DTOs;
using Tuitio.Models;

namespace Tuitio.Configurations
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Course, CourseDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, UpdateUserDTO>().ReverseMap();
            CreateMap<Lesson, LessonDTO>().ReverseMap();
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Registration, RegistrationDTO>().ReverseMap();
            CreateMap<Role, RoleDTO>().ReverseMap();


            CreateMap<Order, OrderDTO>()
                .ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(src => src.OrderDetails));
            CreateMap<OrderDetail, OrderDetailDTO>()
                .ForMember(dest => dest.CourseTitle, opt => opt.MapFrom(src => src.Course.CourseTitle));


            CreateMap<Review, ReviewDTO>()
                .ForMember(dest => dest.Student, opt => opt.MapFrom(src => new StudentDTO
                {
                    Username = src.Student.Username,
                    Email = src.Student.Email,
                    FullName = src.Student.FullName,
                    ProfileImage = src.Student.ProfileImage
                }))
                .ReverseMap();

            // Add mapping for CreateReviewDTO
            CreateMap<CreateReviewDTO, Review>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.StudentId, opt => opt.MapFrom(src => src.StudentId))
                .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.CourseId))
                .ForMember(dest => dest.Comment, opt => opt.MapFrom(src => src.Comment))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating));

            CreateMap<Review, CreateReviewDTO>().ReverseMap();

            CreateMap<Topic, TopicDTO>().ReverseMap();
            CreateMap<User, StudentDTO>().ReverseMap();

            // Cart Mapping
            CreateMap<Cart, CartDTO>()
                .ForMember(dest => dest.CartItems, opt => opt.MapFrom(src => src.CartItems));

            CreateMap<CartDTO, Cart>()
                .ForMember(dest => dest.CartItems, opt => opt.MapFrom(src => src.CartItems));

            // CartItem Mapping
            CreateMap<CartItem, CartItemDTO>()
                .ForMember(dest => dest.CourseTitle, opt => opt.MapFrom(src => src.Course.CourseTitle))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Course.Price))
                .ForMember(dest => dest.Thumbnail, opt => opt.MapFrom(src => src.Course.Thumbnail));

            CreateMap<CartItemDTO, CartItem>()
                .ForMember(dest => dest.Course, opt => opt.Ignore());
        }
    }
}
