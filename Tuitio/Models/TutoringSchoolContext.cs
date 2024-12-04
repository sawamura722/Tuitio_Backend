using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Tuitio.Models;

public partial class TutoringSchoolContext : DbContext
{
    public TutoringSchoolContext()
    {
    }

    public TutoringSchoolContext(DbContextOptions<TutoringSchoolContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Faq> Faqs { get; set; }

    public virtual DbSet<Lesson> Lessons { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Registration> Registrations { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<School> Schools { get; set; }

    public virtual DbSet<Topic> Topics { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-G74O92U;Database=TutoringSchool;Trusted_Connection=True; Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("PK__Carts__2EF52A27BAD87F24");

            entity.Property(e => e.CartId).HasColumnName("cart_id");
            entity.Property(e => e.StudentId).HasColumnName("student_id");

            entity.HasOne(d => d.Student).WithMany(p => p.Carts)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Carts__student_i__4D94879B");
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.CartItemId).HasName("PK__CartItem__5D9A6C6EE3E566B1");

            entity.Property(e => e.CartItemId).HasColumnName("cart_item_id");
            entity.Property(e => e.CartId).HasColumnName("cart_id");
            entity.Property(e => e.CourseId).HasColumnName("course_id");

            entity.HasOne(d => d.Cart).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.CartId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CartItems__cart___52593CB8");

            entity.HasOne(d => d.Course).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CartItems__cours__534D60F1");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__19093A0BE812425E");

            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(100)
                .HasColumnName("category_name");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PK__Courses__8F1EF7AEF34EA60D");

            entity.Property(e => e.CourseId).HasColumnName("course_id");
            entity.Property(e => e.CourseDescription)
                .HasColumnType("text")
                .HasColumnName("course_description");
            entity.Property(e => e.CourseTitle)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("course_title");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.EndDateAt).HasColumnName("end_date_at");
            entity.Property(e => e.EndTimeAt).HasColumnName("end_time_at");
            entity.Property(e => e.Friday).HasDefaultValue(false);
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsOnline).HasColumnName("isOnline");
            entity.Property(e => e.Location)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Monday).HasDefaultValue(false);
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.Saturday).HasDefaultValue(false);
            entity.Property(e => e.StartDateAt).HasColumnName("start_date_at");
            entity.Property(e => e.StartTimeAt).HasColumnName("start_time_at");
            entity.Property(e => e.Sunday).HasDefaultValue(false);
            entity.Property(e => e.TeacherId).HasColumnName("teacher_id");
            entity.Property(e => e.Thumbnail)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("thumbnail");
            entity.Property(e => e.Thursday).HasDefaultValue(false);
            entity.Property(e => e.Tuesday).HasDefaultValue(false);
            entity.Property(e => e.Wednesday).HasDefaultValue(false);

            entity.HasOne(d => d.Teacher).WithMany(p => p.Courses)
                .HasForeignKey(d => d.TeacherId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Courses__teacher__412EB0B6");

            entity.HasMany(d => d.Categories).WithMany(p => p.Courses)
                .UsingEntity<Dictionary<string, object>>(
                    "CourseCategory",
                    r => r.HasOne<Category>().WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__CourseCat__categ__70DDC3D8"),
                    l => l.HasOne<Course>().WithMany()
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__CourseCat__cours__6FE99F9F"),
                    j =>
                    {
                        j.HasKey("CourseId", "CategoryId").HasName("PK__CourseCa__D24A19353BAF3D7D");
                        j.ToTable("CourseCategories");
                        j.IndexerProperty<int>("CourseId").HasColumnName("course_id");
                        j.IndexerProperty<int>("CategoryId").HasColumnName("category_id");
                    });
        });

        modelBuilder.Entity<Faq>(entity =>
        {
            entity.HasKey(e => e.FaqId).HasName("PK__FAQ__66734BAF08DDBE24");

            entity.ToTable("FAQ");

            entity.Property(e => e.FaqId).HasColumnName("faq_id");
            entity.Property(e => e.Answer)
                .HasColumnType("text")
                .HasColumnName("answer");
            entity.Property(e => e.Question)
                .HasColumnType("text")
                .HasColumnName("question");
        });

        modelBuilder.Entity<Lesson>(entity =>
        {
            entity.HasKey(e => e.LessonId).HasName("PK__Lessons__6421F7BE99382761");

            entity.Property(e => e.LessonId).HasColumnName("lesson_id");
            entity.Property(e => e.LessonContent)
                .HasColumnType("text")
                .HasColumnName("lesson_content");
            entity.Property(e => e.LessonTitle)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("lesson_title");
            entity.Property(e => e.Thumbnail)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("thumbnail");
            entity.Property(e => e.TopicId).HasColumnName("topic_id");
            entity.Property(e => e.VideoUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("video_url");

            entity.HasOne(d => d.Topic).WithMany(p => p.Lessons)
                .HasForeignKey(d => d.TopicId)
                .HasConstraintName("FK__Lessons__topic_i__07C12930");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__46596229F21413F9");

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.StudentId).HasColumnName("student_id");
            entity.Property(e => e.TotalPrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("total_price");

            entity.HasOne(d => d.Student).WithMany(p => p.Orders)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orders__student___571DF1D5");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.OrderdetailId).HasName("PK__OrderDet__59AE74111E7453AC");

            entity.Property(e => e.OrderdetailId).HasColumnName("orderdetail_id");
            entity.Property(e => e.CourseId).HasColumnName("course_id");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("price");

            entity.HasOne(d => d.Course).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDetails_Courses");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDetails_Orders");
        });

        modelBuilder.Entity<Registration>(entity =>
        {
            entity.HasKey(e => e.RegistrationId).HasName("PK__Registra__22A298F6B8E3AE63");

            entity.Property(e => e.RegistrationId).HasColumnName("registration_id");
            entity.Property(e => e.CourseId).HasColumnName("course_id");
            entity.Property(e => e.RegistrationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("registration_date");
            entity.Property(e => e.StudentId).HasColumnName("student_id");

            entity.HasOne(d => d.Course).WithMany(p => p.Registrations)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Registrat__cours__49C3F6B7");

            entity.HasOne(d => d.Student).WithMany(p => p.Registrations)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Registrat__stude__48CFD27E");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PK__Reviews__60883D9015FAE952");

            entity.Property(e => e.ReviewId).HasColumnName("review_id");
            entity.Property(e => e.Comment)
                .HasColumnType("text")
                .HasColumnName("comment");
            entity.Property(e => e.CourseId).HasColumnName("course_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.StudentId).HasColumnName("student_id");

            entity.HasOne(d => d.Course).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reviews__course___5FB337D6");

            entity.HasOne(d => d.Student).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reviews__student__5EBF139D");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__760965CC55D84965");

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<School>(entity =>
        {
            entity.HasKey(e => e.SchoolId).HasName("PK__Schools__3DA4675BA6A1C200");

            entity.Property(e => e.Location).HasMaxLength(255);
            entity.Property(e => e.SchoolTitle).HasMaxLength(100);
            entity.Property(e => e.Telephone).HasMaxLength(20);
        });

        modelBuilder.Entity<Topic>(entity =>
        {
            entity.HasKey(e => e.TopicId).HasName("PK__Topics__D5DAA3E9E9B9BF7B");

            entity.Property(e => e.TopicId).HasColumnName("topic_id");
            entity.Property(e => e.CourseId).HasColumnName("course_id");
            entity.Property(e => e.TopicTitle)
                .HasMaxLength(255)
                .HasColumnName("topic_title");

            entity.HasOne(d => d.Course).WithMany(p => p.Topics)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Topics__course_i__06CD04F7");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__B9BE370F1C02AA99");

            entity.HasIndex(e => e.Email, "UQ__Users__AB6E61645D63D7BC").IsUnique();

            entity.HasIndex(e => e.Username, "UQ__Users__F3DBC57290556208").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("full_name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.ProfileImage)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("profile_image");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("username");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Users__role_id__3C69FB99");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
