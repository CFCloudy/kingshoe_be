using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DAL.Models
{
    public partial class ShoeStoreContext : IdentityDbContext<ApplicationUser>
    {
        public ShoeStoreContext()
        {
        }

        public ShoeStoreContext(DbContextOptions<ShoeStoreContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Brand> Brands { get; set; } = null!;
        public virtual DbSet<Cart> Carts { get; set; } = null!;
        public virtual DbSet<CartItem> CartItems { get; set; } = null!;
        public virtual DbSet<Color> Colors { get; set; } = null!;
        public virtual DbSet<Feature> Features { get; set; } = null!;
        public virtual DbSet<Gallery> Galleries { get; set; } = null!;
        public virtual DbSet<Manufacturer> Manufacturers { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderItem> OrderItems { get; set; } = null!;
        public virtual DbSet<RequestTicket> RequestTickets { get; set; } = null!;
        public virtual DbSet<RequestTicketItem> RequestTicketItems { get; set; } = null!;
        public virtual DbSet<ResponseTicket> ResponseTickets { get; set; } = null!;
        public virtual DbSet<ResponseTicketItem> ResponseTicketItems { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<ShippingDetail> ShippingDetails { get; set; } = null!;
        public virtual DbSet<Shoe> Shoes { get; set; } = null!;
        public virtual DbSet<ShoesVariant> ShoesVariants { get; set; } = null!;
        public virtual DbSet<Size> Sizes { get; set; } = null!;
        public virtual DbSet<Style> Styles { get; set; } = null!;
        public virtual DbSet<TicketType> TicketTypes { get; set; } = null!;
        public virtual DbSet<UserAddress> UserAddresses { get; set; } = null!;
        public virtual DbSet<UserOtp> UserOtps { get; set; } = null!;
        public virtual DbSet<UserProfile> UserProfiles { get; set; } = null!;
        public virtual DbSet<Voucher> Vouchers { get; set; } = null!;
        public virtual DbSet<VoucherBank> VoucherBanks { get; set; } = null!;
        public virtual DbSet<VouchersUseLog> VouchersUseLogs { get; set; } = null!;
        public virtual DbSet<WarehouseProduct> WarehouseProducts { get; set; } = null!;
        public virtual DbSet<OrderHistoryLog> OrderHistoryLogs { get; set; } = null!;

        public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-1LNOKIQ;Initial Catalog=KingShoe;Integrated Security=True;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<UserAddress>(entity =>
            {
                entity.HasOne(x => x.UserProfiles).WithMany(x => x.UserAddresses).HasForeignKey(x => x.UserId);
            });

            modelBuilder.Entity<UserProfile>(entity => {
                entity.HasOne(x => x.ApplicationUser).WithOne(x => x.UserProfiles).HasForeignKey<UserProfile>(x => x.UserId);
            });



            modelBuilder.Entity<UserOtp>(entity => {
                entity.HasOne(x => x.ApplicationUser).WithMany(x => x.Otps).HasForeignKey(x => x.UserId);
            });

            modelBuilder.Entity<OrderHistoryLog>(entity =>
            {
                entity.HasOne(x => x.Order).WithMany(x => x.OrderHistoryLogs).HasForeignKey(x => x.OrderId);
            });
            modelBuilder.Entity<Brand>(entity =>
            {
                entity.Property(e => e.BrandName).HasMaxLength(250);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Status).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<Cart>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Status).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Status).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Cart)
                    .WithMany(p => p.CartItems)
                    .HasForeignKey(d => d.CartId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CartItems__CartI__2DE6D218");

                entity.HasOne(d => d.ProductVariant)
                    .WithMany(p => p.CartItems)
                    .HasForeignKey(d => d.ProductVariantId)
                    .HasConstraintName("FK__CartItems__Produ__31B762FC");
            });

            modelBuilder.Entity<Color>(entity =>
            {
                entity.Property(e => e.ColorName).HasMaxLength(250);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Status).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<Gallery>(entity => {
                entity.HasOne(x => x.Comments).WithMany(x => x.Attachment).HasForeignKey(x => x.CommentId);
            });

            modelBuilder.Entity<Feature>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.FeatureName).HasMaxLength(250);

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Status).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<Gallery>(entity =>
            {
                entity.ToTable("Gallery");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");


                entity.Property(e => e.Url)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Status).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<Manufacturer>(entity =>
            {
                entity.Property(e => e.Address).HasMaxLength(250);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name).HasMaxLength(250);

                entity.Property(e => e.Status).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.OrderCode).HasMaxLength(250);

                entity.Property(e => e.Status).HasDefaultValueSql("((0))");

                entity.Property(e => e.Total).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Orders__UserId__32AB8735");
            });

           

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Status).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderItem__Order__2EDAF651");

                entity.HasOne(d => d.ProductVariant)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.ProductVariantId)
                    .HasConstraintName("FK__OrderItem__Produ__30C33EC3");
            });

            modelBuilder.Entity<RequestTicket>(entity =>
            {
                entity.ToTable("RequestTicket");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Status).HasDefaultValueSql("((0))");

                entity.Property(e => e.TicketMessage).HasMaxLength(250);

                entity.HasOne(d => d.TicketTypeNavigation)
                    .WithMany(p => p.RequestTickets)
                    .HasForeignKey(d => d.TicketType)
                    .HasConstraintName("FK__RequestTi__Ticke__245D67DE");
            });

            modelBuilder.Entity<RequestTicketItem>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Status).HasDefaultValueSql("((0))");

                entity.Property(e => e.WarehouseProductId).HasColumnName("WarehouseProductID");

                entity.HasOne(d => d.Ticket)
                    .WithMany(p => p.RequestTicketItems)
                    .HasForeignKey(d => d.TicketId)
                    .HasConstraintName("FK__RequestTi__Ticke__25518C17");

                entity.HasOne(d => d.WarehouseProduct)
                    .WithMany(p => p.RequestTicketItems)
                    .HasForeignKey(d => d.WarehouseProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__RequestTi__Wareh__29221CFB");
            });

            modelBuilder.Entity<ResponseTicket>(entity =>
            {
                entity.ToTable("ResponseTicket");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Status).HasDefaultValueSql("((0))");

                entity.Property(e => e.TicketMessage).HasMaxLength(250);

                entity.HasOne(d => d.TicketTypeNavigation)
                    .WithMany(p => p.ResponseTickets)
                    .HasForeignKey(d => d.TicketType)
                    .HasConstraintName("FK__ResponseT__Ticke__2645B050");
            });

            modelBuilder.Entity<ResponseTicketItem>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Status).HasDefaultValueSql("((0))");

                entity.Property(e => e.WarehouseProductId).HasColumnName("WarehouseProductID");

                entity.HasOne(d => d.Ticket)
                    .WithMany(p => p.ResponseTicketItems)
                    .HasForeignKey(d => d.TicketId)
                    .HasConstraintName("FK__ResponseT__Ticke__2739D489");

                entity.HasOne(d => d.WarehouseProduct)
                    .WithMany(p => p.ResponseTicketItems)
                    .HasForeignKey(d => d.WarehouseProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ResponseT__Wareh__282DF8C2");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RoleName).HasMaxLength(250);

                entity.Property(e => e.RoleUrl).HasMaxLength(250);

                entity.Property(e => e.Status).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<ShippingDetail>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.OrderNote).HasMaxLength(250);

                entity.Property(e => e.ShippingAddress).HasMaxLength(250);

                entity.Property(e => e.ShippingName).HasMaxLength(250);

                entity.Property(e => e.ShippingPhone).HasMaxLength(250);

                entity.Property(e => e.Status).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.ShippingDetails)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK__ShippingD__Order__2FCF1A8A");
            });

            modelBuilder.Entity<Shoe>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DisplayPrice).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.OldPrice).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.ProductName).HasMaxLength(250);
                entity.Property(e => e.Description).HasMaxLength(250);
                entity.Property(e => e.DescriptionDetail).HasColumnType("nvarchar(max)");;

                entity.Property(e => e.Status).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.BrandGroupNavigation)
                    .WithMany(p => p.Shoes)
                    .HasForeignKey(d => d.BrandGroup)
                    .HasConstraintName("FK__Shoes__BrandGrou__1DB06A4F");

                entity.HasOne(d => d.FeatureNavigation)
                    .WithMany(p => p.Shoes)
                    .HasForeignKey(d => d.Feature)
                    .HasConstraintName("FK__Shoes__Feature__1F98B2C1");

                entity.HasOne(d => d.StyleGroupNavigation)
                    .WithMany(p => p.Shoes)
                    .HasForeignKey(d => d.StyleGroup)
                    .HasConstraintName("FK__Shoes__StyleGrou__1EA48E88");
            });

            modelBuilder.Entity<ShoesVariant>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description).HasMaxLength(250);

                entity.Property(e => e.DisplayPrice).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.OldPrice).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Status).HasDefaultValueSql("((0))");

                entity.Property(e => e.Stock).HasDefaultValueSql("((0))");

                entity.Property(e => e.VariantName).HasMaxLength(250);

                entity.HasOne(d => d.ColorNavigation)
                    .WithMany(p => p.ShoesVariants)
                    .HasForeignKey(d => d.Color)
                    .HasConstraintName("FK__ShoesVari__Color__22751F6C");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ShoesVariants)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK__ShoesVari__Produ__208CD6FA");

                entity.HasOne(d => d.SizeNavigation)
                    .WithMany(p => p.ShoesVariants)
                    .HasForeignKey(d => d.Size)
                    .HasConstraintName("FK__ShoesVaria__Size__2180FB33");
            });

            modelBuilder.Entity<Size>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Locale).HasMaxLength(250);

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Size1)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("Size");

                entity.Property(e => e.Status).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<Style>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Status).HasDefaultValueSql("((0))");

                entity.Property(e => e.StyleName).HasMaxLength(250);
            });

            modelBuilder.Entity<TicketType>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Status).HasDefaultValueSql("((0))");

                entity.Property(e => e.TypeName).HasMaxLength(250);
            });

            modelBuilder.Entity<UserAddress>(entity =>
            {
                entity.Property(e => e.AddressDetail).HasMaxLength(250);

                entity.Property(e => e.Type).HasMaxLength(250);

                entity.Property(e => e.City).HasMaxLength(250);

                entity.Property(e => e.PhoneNumber).HasMaxLength(250);

                entity.Property(e => e.District).HasMaxLength(250);

                entity.Property(e => e.Ward).HasMaxLength(250);

                entity.HasOne(d => d.UserProfiles)
                    .WithMany(p => p.UserAddresses)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__UserAddre__UserI__3493CFA7");
            });

            modelBuilder.Entity<UserOtp>(entity =>
            {
                entity.ToTable("UserOTP");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ExpireTime).HasColumnType("datetime");

                entity.Property(e => e.Code)
                    .HasMaxLength(250)
                    .HasColumnName("OTP");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.ApplicationUser)
                    .WithMany(p => p.Otps)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__UserOTP__UserID__42E1EEFE");
            });

            modelBuilder.Entity<UserProfile>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email).HasMaxLength(250);

                entity.Property(e => e.FullName).HasMaxLength(250);

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");


                entity.Property(e => e.PhoneNumber).HasMaxLength(15);

                entity.Property(e => e.Status).HasDefaultValueSql("((0))");


                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserProfiles)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__UserProfi__RoleI__339FAB6E");
            });

            modelBuilder.Entity<Voucher>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DiscountMaxValue).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.DiscountValue).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Status).HasDefaultValueSql("((0))");

                entity.Property(e => e.VoucherCode).HasMaxLength(250);

                entity.Property(e => e.VoucherContent).HasMaxLength(250);
            });

            modelBuilder.Entity<VoucherBank>(entity =>
            {
                entity.ToTable("VoucherBank");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Quantity).HasDefaultValueSql("((0))");

                //entity.HasOne(d => d.User)
                //    .WithMany(p => p.VoucherBanks)
                //    .HasForeignKey(d => d.UserId)
                //    .HasConstraintName("FK__VoucherBa__UserI__2CF2ADDF");

                //entity.HasOne(d => d.Voucher)
                //    .WithMany(p => p.VoucherBanks)
                //    .HasForeignKey(d => d.VoucherId)
                //    .HasConstraintName("FK__VoucherBa__Vouch__2BFE89A6");
            });

            modelBuilder.Entity<VouchersUseLog>(entity =>
            {
                entity.ToTable("Vouchers_UseLog");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.VoucherUserId).HasColumnName("Voucher_UserId");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.VouchersUseLogs)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK__Vouchers___Order__3587F3E0");

                //entity.HasOne(d => d.VoucherUser)
                //    .WithMany(p => p.VouchersUseLogs)
                //    .HasForeignKey(d => d.VoucherUserId)
                //    .HasConstraintName("FK__Vouchers___Vouch__2B0A656D");
            });

            modelBuilder.Entity<WarehouseProduct>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ImportPrice).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Status).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.ManufacturerNavigation)
                    .WithMany(p => p.WarehouseProducts)
                    .HasForeignKey(d => d.Manufacturer)
                    .HasConstraintName("FK__Warehouse__Manuf__236943A5");
            });

            base.OnModelCreating(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
