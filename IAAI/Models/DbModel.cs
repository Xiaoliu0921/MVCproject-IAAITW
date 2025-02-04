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

        // DbModelBuilder�AEF�۰ʴ��ѵ�OnModelCreating��k������
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // �����]�m�Ҧ�BaseEntity�����ݩ�
            // �]�N�O���ΨC�Ӥl�����n�gmodelBuilder.Entity<T>
            // �̫�ϥ�Ignore()�A�~���|�b��Ʈw�إ�BaseEntity���ƪ�
            modelBuilder.Types<BaseEntity>()
                .Configure(entity =>
                {
                    entity.HasKey(e => e.Id);
                    entity.Property(e => e.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                });

            modelBuilder.Ignore<BaseEntity>();

            // ForumReplies -> Members (�^�Ъ��o���)
            modelBuilder.Entity<ForumReply>()
                .HasRequired(reply => reply.MyMember)
                .WithMany() // �p�G�� Member -> Replies ���ɯ��ݩʡA�h��J
                .HasForeignKey(reply => reply.MemberId)
                .WillCascadeOnDelete(false); // �T�γs��R��

            // ForumReplies -> ForumPosts (�^�Ъ��峹)
            modelBuilder.Entity<ForumReply>()
                .HasRequired(reply => reply.MyForumPost)
                .WithMany(post => post.MyReplies) // ���]�� ForumPost -> Replies ���ɯ��ݩ�
                .HasForeignKey(reply => reply.ForumPostId)
                .WillCascadeOnDelete(false); // �T�γs��R��

            // �u�O�X�R��k�A���O�즳����k�޿��٬O�ݭn�A�ҥH�n�I�sbase����
            base.OnModelCreating(modelBuilder);
        }

        //// �^�ǭȬ�int�A��ܦ��\��CRUD���ƶq
        //public override int SaveChanges()
        //{
        //    // �l���~��BaseEntity��������
        //    var entries = ChangeTracker.Entries<BaseEntity>();

        //    // entry�A��������A�]�t���󪬺A�H�ι���
        //    // entry.State : ���󪬺A�Aentry.Entity : �������
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
