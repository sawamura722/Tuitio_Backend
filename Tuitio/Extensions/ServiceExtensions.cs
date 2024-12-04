using Tuitio.Services.IService;
using Tuitio.Services.ServiceImpl;

namespace Tuitio.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddProjectServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<ILessonService, LessonService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IRegistrationService, RegistrationService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<IFaqService, FaqService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITopicService, TopicService>();
            services.AddScoped<ISchoolService, SchoolService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<PaymentService>();
        }
    }
}
