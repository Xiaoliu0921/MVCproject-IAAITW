using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace IAAI.Models
{
    public partial class DbModel : DbContext
    {
        public DbModel()
            : base("name=DbModel")
        {
        }

        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<CertifiedMember> CertifiedMembers { get; set; }
        public virtual DbSet<Expert> Experts { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<ForumPost> ForumPosts { get; set; }
        public virtual DbSet<ForumReply> ForumReplys { get; set; }
        public virtual DbSet<Knowledge> Knowledge { get; set; }

        // DbModelBuilder，EF自動提供給OnModelCreating方法的物件
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // 直接設置所有BaseEntity類的屬性
            // 也就是不用每個子類都要寫modelBuilder.Entity<T>
            // 最後使用Ignore()，才不會在資料庫建立BaseEntity表資料表
            modelBuilder.Types<BaseEntity>()
                .Configure(entity =>
                {
                    entity.HasKey(e => e.Id);
                    entity.Property(e => e.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                });

            modelBuilder.Ignore<BaseEntity>();

            // ForumReplies -> Members (回覆的發表者)
            modelBuilder.Entity<ForumReply>()
                .HasRequired(reply => reply.MyMember)
                .WithMany() // 如果有 Member -> Replies 的導航屬性，則填入
                .HasForeignKey(reply => reply.MemberId)
                .WillCascadeOnDelete(false); // 禁用連鎖刪除

            // ForumReplies -> ForumPosts (回覆的文章)
            modelBuilder.Entity<ForumReply>()
                .HasRequired(reply => reply.MyForumPost)
                .WithMany(post => post.MyReplies) // 假設有 ForumPost -> Replies 的導航屬性
                .HasForeignKey(reply => reply.ForumPostId)
                .WillCascadeOnDelete(false); // 禁用連鎖刪除

            // 只是擴充方法，但是原有的方法邏輯還是需要，所以要呼叫base執行
            base.OnModelCreating(modelBuilder);
        }

        //// 回傳值為int，表示成功的CRUD的數量
        //public override int SaveChanges()
        //{
        //    // 追蹤繼承BaseEntity類的實體
        //    var entries = ChangeTracker.Entries<BaseEntity>();

        //    // entry，物件紀錄，包含物件狀態以及實體
        //    // entry.State : 物件狀態，entry.Entity : 物件實體
        //    foreach (var entry in entries)
        //    {
        //        if (entry.State == EntityState.Added)
        //        {
        //            entry.Entity.CreatedTime = DateTime.Now;
        //        }
        //        else if (entry.State == EntityState.Modified)
        //        {
        //            entry.Entity.UpdatedTime = DateTime.Now;
        //        }
        //    }
        //    return base.SaveChanges();
        //}


    }
}
