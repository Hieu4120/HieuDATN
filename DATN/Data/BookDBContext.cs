using Microsoft.EntityFrameworkCore;
using DATN.Model;

namespace DATN.Data
{
    public class BookDBContext : DbContext
    {
        public DbSet<m_account> m_accounts { get; set; }
        public DbSet<m_book> m_books { get; set; }
        public DbSet<m_cart> m_carts { get; set; }
        public DbSet<m_feedback> m_feedbacks { get; set; }
        public DbSet<m_genre> m_genres { get; set; }
        public DbSet<m_import_order> m_import_orders { get; set; }
        public DbSet<m_import_order_detail> m_import_order_details { get; set; }
        public DbSet<m_carosel> m_carosels { get; set; }
        public DbSet<m_sale_order> m_sale_orders { get; set; }
        public DbSet<m_sale_order_detail> m_sale_order_details { get; set; }
        public DbSet<m_supplier> m_suppliers { get; set; }
        public DbSet<m_review> m_reviews { get; set; }

        public BookDBContext(DbContextOptions<BookDBContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<m_account>().HasKey(table => new
            {
                table.customer_id,
            });
            builder.Entity<m_book>().HasKey(table => new
            {
                table.book_id,
                table.genre_id,
                table.supplier_id
            });
            builder.Entity<m_cart>().HasKey(table => new
            {
                table.cart_id,
                table.book_id,
                table.customer_id
            });          
            builder.Entity<m_feedback>().HasKey(table => new
            {
                table.feedback_id,
                table.customer_id
            });
            builder.Entity<m_genre>().HasKey(table => new
            {
                table.genre_id              
            });
            builder.Entity<m_import_order>().HasKey(table => new
            {
                table.import_order_id,
                table.supplier_id
            });
            builder.Entity<m_import_order_detail>().HasKey(table => new
            {
                table.import_order_detail_id,
                table.import_order_id,
                table.book_id,
                table.supplier_id,
                table.genre_id
            });
            builder.Entity<m_carosel>().HasKey(table => new
            {
                table.carosel_id,            
            });
            builder.Entity<m_sale_order>().HasKey(table => new
            {
                table.sale_order_id
            });
            builder.Entity<m_sale_order_detail>().HasKey(table => new
            {
                table.sale_order_detail_id
            });
            builder.Entity<m_supplier>().HasKey(table => new
            {
                table.supplier_id
            });
            builder.Entity<m_review>().HasKey(table => new
            {
                table.review_id,
                table.book_id
            });

        }
    }
}
