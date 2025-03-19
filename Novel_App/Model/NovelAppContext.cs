using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Novel_App.Model;

public partial class NovelAppContext : DbContext
{
    public NovelAppContext()
    {
    }

    public NovelAppContext(DbContextOptions<NovelAppContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Chapter> Chapters { get; set; }

    public virtual DbSet<ChapterSubmission> ChapterSubmissions { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Favorite> Favorites { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<LockAccountLog> LockAccountLogs { get; set; }

    public virtual DbSet<LockChapterLog> LockChapterLogs { get; set; }

    public virtual DbSet<LockNovelLog> LockNovelLogs { get; set; }

    public virtual DbSet<ManagerAccount> ManagerAccounts { get; set; }

    public virtual DbSet<Novel> Novels { get; set; }

    public virtual DbSet<NovelSubmission> NovelSubmissions { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<ReadingHistory> ReadingHistories { get; set; }

    public virtual DbSet<UserAccount> UserAccounts { get; set; }

    public virtual DbSet<Viewing> Viewings { get; set; }

    private string GetConnectionString()
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .Build();
        var connectionstring = configuration["ConnectionStrings:DefaultConnection"];
        return connectionstring;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(GetConnectionString());


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Chapter>(entity =>
        {
            entity.HasKey(e => e.ChapterId).HasName("PK__Chapter__05D716EF5A734D35");

            entity.ToTable("Chapter");

            entity.Property(e => e.ChapterId).HasColumnName("chapterID");
            entity.Property(e => e.ChapterName)
                .HasMaxLength(255)
                .HasColumnName("chapterName");
            entity.Property(e => e.ChapterNumber).HasColumnName("chapterNumber");
            entity.Property(e => e.ChapterStatus)
                .HasMaxLength(10)
                .HasColumnName("chapterStatus");
            entity.Property(e => e.FileUrl)
                .HasMaxLength(200)
                .HasColumnName("fileURL");
            entity.Property(e => e.NovelId).HasColumnName("novelID");
            entity.Property(e => e.PublishedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("publishedDate");

            entity.HasOne(d => d.Novel).WithMany(p => p.Chapters)
                .HasForeignKey(d => d.NovelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Chapter__novelID__02FC7413");
        });

        modelBuilder.Entity<ChapterSubmission>(entity =>
        {
            entity.HasKey(e => e.SubmissionCid).HasName("PK__ChapterS__83560145A3AB9E67");

            entity.ToTable("ChapterSubmission");

            entity.Property(e => e.SubmissionCid).HasColumnName("submissionCID");
            entity.Property(e => e.ApprovalDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("approvalDate");
            entity.Property(e => e.ChapterId).HasColumnName("chapterID");
            entity.Property(e => e.DraftId).HasColumnName("draftID");
            entity.Property(e => e.ManagerId).HasColumnName("managerID");
            entity.Property(e => e.ReasonRejected)
                .HasMaxLength(200)
                .HasColumnName("reasonRejected");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("status");
            entity.Property(e => e.SubmissionDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("submissionDate");
            entity.Property(e => e.Type)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("post")
                .HasColumnName("type");
            entity.Property(e => e.UserId).HasColumnName("userID");

            entity.HasOne(d => d.Chapter).WithMany(p => p.ChapterSubmissions)
                .HasForeignKey(d => d.ChapterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChapterSu__chapt__03F0984C");

            entity.HasOne(d => d.Draft).WithMany(p => p.ChapterSubmissions)
                .HasForeignKey(d => d.DraftId)
                .HasConstraintName("FK__ChapterSu__draft__06CD04F7");

            entity.HasOne(d => d.Manager).WithMany(p => p.ChapterSubmissions)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("FK__ChapterSu__manag__05D8E0BE");

            entity.HasOne(d => d.User).WithMany(p => p.ChapterSubmissions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChapterSu__userI__04E4BC85");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PK__Comment__CDDE91BDEFFF41C7");

            entity.ToTable("Comment");

            entity.Property(e => e.CommentId).HasColumnName("commentID");
            entity.Property(e => e.CommentContent).HasColumnName("commentContent");
            entity.Property(e => e.CommentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("commentDate");
            entity.Property(e => e.NovelId).HasColumnName("novelID");
            entity.Property(e => e.UserId).HasColumnName("userID");

            entity.HasOne(d => d.Novel).WithMany(p => p.Comments)
                .HasForeignKey(d => d.NovelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comment__novelID__08B54D69");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comment__userID__07C12930");
        });

        modelBuilder.Entity<Favorite>(entity =>
        {
            entity.HasKey(e => e.FavoriteId).HasName("PK__Favorite__876A6735C683FAA2");

            entity.ToTable("Favorite");

            entity.Property(e => e.FavoriteId).HasColumnName("favoriteID");
            entity.Property(e => e.IsFavorite)
                .HasDefaultValue(false)
                .HasColumnName("isFavorite");
            entity.Property(e => e.NovelId).HasColumnName("novelID");
            entity.Property(e => e.UserId).HasColumnName("userID");

            entity.HasOne(d => d.Novel).WithMany(p => p.Favorites)
                .HasForeignKey(d => d.NovelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Favorite__novelI__0A9D95DB");

            entity.HasOne(d => d.User).WithMany(p => p.Favorites)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Favorite__userID__09A971A2");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.GenreId).HasName("PK__Genre__3C5476A2A82BA172");

            entity.ToTable("Genre");

            entity.HasIndex(e => e.GenreName, "UQ__Genre__9E262151D3CE8DEB").IsUnique();

            entity.Property(e => e.GenreId).HasColumnName("genreID");
            entity.Property(e => e.GenreName)
                .HasMaxLength(50)
                .HasColumnName("genreName");

            entity.HasMany(d => d.Novels).WithMany(p => p.Genres)
                .UsingEntity<Dictionary<string, object>>(
                    "GenreNovel",
                    r => r.HasOne<Novel>().WithMany()
                        .HasForeignKey("NovelId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Genre_Nov__novel__0C85DE4D"),
                    l => l.HasOne<Genre>().WithMany()
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Genre_Nov__genre__0B91BA14"),
                    j =>
                    {
                        j.HasKey("GenreId", "NovelId").HasName("PK__Genre_No__89E3DA5D50C582DA");
                        j.ToTable("Genre_Novel");
                        j.IndexerProperty<int>("GenreId").HasColumnName("genreID");
                        j.IndexerProperty<int>("NovelId").HasColumnName("novelID");
                    });
        });

        modelBuilder.Entity<LockAccountLog>(entity =>
        {
            entity.HasKey(e => e.LogAid).HasName("PK__LockAcco__96E77FC446A7462F");

            entity.ToTable("LockAccountLog");

            entity.Property(e => e.LogAid).HasColumnName("logAID");
            entity.Property(e => e.Action)
                .HasMaxLength(10)
                .HasColumnName("action");
            entity.Property(e => e.Datetime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("datetime");
            entity.Property(e => e.LockReason)
                .HasMaxLength(255)
                .HasColumnName("lockReason");
            entity.Property(e => e.ManagerId).HasColumnName("managerID");
            entity.Property(e => e.UserId).HasColumnName("userID");

            entity.HasOne(d => d.Manager).WithMany(p => p.LockAccountLogs)
                .HasForeignKey(d => d.ManagerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LockAccou__manag__0D7A0286");

            entity.HasOne(d => d.User).WithMany(p => p.LockAccountLogs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LockAccou__userI__0E6E26BF");
        });

        modelBuilder.Entity<LockChapterLog>(entity =>
        {
            entity.HasKey(e => e.LogCid).HasName("PK__LockChap__894BABBDB63365ED");

            entity.ToTable("LockChapterLog");

            entity.Property(e => e.LogCid).HasColumnName("logCID");
            entity.Property(e => e.Action)
                .HasMaxLength(10)
                .HasColumnName("action");
            entity.Property(e => e.ChapterId).HasColumnName("chapterID");
            entity.Property(e => e.Datetime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("datetime");
            entity.Property(e => e.LockReason)
                .HasMaxLength(255)
                .HasColumnName("lockReason");
            entity.Property(e => e.ManagerId).HasColumnName("managerID");

            entity.HasOne(d => d.Chapter).WithMany(p => p.LockChapterLogs)
                .HasForeignKey(d => d.ChapterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LockChapt__chapt__10566F31");

            entity.HasOne(d => d.Manager).WithMany(p => p.LockChapterLogs)
                .HasForeignKey(d => d.ManagerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LockChapt__manag__0F624AF8");
        });

        modelBuilder.Entity<LockNovelLog>(entity =>
        {
            entity.HasKey(e => e.LogNid).HasName("PK__LockNove__2BC2197C18DBAA30");

            entity.ToTable("LockNovelLog");

            entity.Property(e => e.LogNid).HasColumnName("logNID");
            entity.Property(e => e.Action)
                .HasMaxLength(10)
                .HasColumnName("action");
            entity.Property(e => e.Datetime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("datetime");
            entity.Property(e => e.LockReason)
                .HasMaxLength(255)
                .HasColumnName("lockReason");
            entity.Property(e => e.ManagerId).HasColumnName("managerID");
            entity.Property(e => e.NovelId).HasColumnName("novelID");

            entity.HasOne(d => d.Manager).WithMany(p => p.LockNovelLogs)
                .HasForeignKey(d => d.ManagerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LockNovel__manag__114A936A");

            entity.HasOne(d => d.Novel).WithMany(p => p.LockNovelLogs)
                .HasForeignKey(d => d.NovelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LockNovel__novel__123EB7A3");
        });

        modelBuilder.Entity<ManagerAccount>(entity =>
        {
            entity.HasKey(e => e.ManagerId).HasName("PK__ManagerA__47E0147FCFD969EE");

            entity.ToTable("ManagerAccount");

            entity.HasIndex(e => e.Email, "UQ__ManagerA__AB6E616417348A73").IsUnique();

            entity.HasIndex(e => e.Username, "UQ__ManagerA__F3DBC5723165F761").IsUnique();

            entity.Property(e => e.ManagerId).HasColumnName("managerID");
            entity.Property(e => e.CanApprove).HasColumnName("canApprove");
            entity.Property(e => e.CanLock).HasColumnName("canLock");
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("creationDate");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .HasColumnName("fullName");
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .HasColumnName("gender");
            entity.Property(e => e.NumberPhone)
                .HasMaxLength(15)
                .HasColumnName("numberPhone");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.Role)
                .HasMaxLength(10)
                .HasDefaultValue("Staff")
                .HasColumnName("role");
            entity.Property(e => e.Status)
                .HasDefaultValue(false)
                .HasColumnName("status");
            entity.Property(e => e.Type)
                .HasMaxLength(10)
                .HasDefaultValue("normal")
                .HasColumnName("type");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");
        });

        modelBuilder.Entity<Novel>(entity =>
        {
            entity.HasKey(e => e.NovelId).HasName("PK__Novel__5B7ACFFAE5C3E27B");

            entity.ToTable("Novel");

            entity.Property(e => e.NovelId).HasColumnName("novelID");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .HasColumnName("imageURL");
            entity.Property(e => e.NovelDescription).HasColumnName("novelDescription");
            entity.Property(e => e.NovelName)
                .HasMaxLength(255)
                .HasColumnName("novelName");
            entity.Property(e => e.NovelStatus)
                .HasMaxLength(10)
                .HasColumnName("novelStatus");
            entity.Property(e => e.PublishedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("publishedDate");
            entity.Property(e => e.TotalChapter)
                .HasDefaultValue(0)
                .HasColumnName("totalChapter");
            entity.Property(e => e.UserId).HasColumnName("userID");

            entity.HasOne(d => d.User).WithMany(p => p.Novels)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Novel__userID__1332DBDC");
        });

        modelBuilder.Entity<NovelSubmission>(entity =>
        {
            entity.HasKey(e => e.SubmissionNid).HasName("PK__NovelSub__EE51D30BD7C0A65C");

            entity.ToTable("NovelSubmission");

            entity.Property(e => e.SubmissionNid).HasColumnName("submissionNID");
            entity.Property(e => e.ApprovalDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("approvalDate");
            entity.Property(e => e.DraftId).HasColumnName("draftID");
            entity.Property(e => e.ManagerId).HasColumnName("managerID");
            entity.Property(e => e.NovelId).HasColumnName("novelID");
            entity.Property(e => e.ReasonRejected)
                .HasMaxLength(200)
                .HasColumnName("reasonRejected");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("status");
            entity.Property(e => e.SubmissionDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("submissionDate");
            entity.Property(e => e.Type)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("post")
                .HasColumnName("type");
            entity.Property(e => e.UserId).HasColumnName("userID");

            entity.HasOne(d => d.Draft).WithMany(p => p.NovelSubmissionDrafts)
                .HasForeignKey(d => d.DraftId)
                .HasConstraintName("FK__NovelSubm__draft__17036CC0");

            entity.HasOne(d => d.Manager).WithMany(p => p.NovelSubmissions)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("FK__NovelSubm__manag__160F4887");

            entity.HasOne(d => d.Novel).WithMany(p => p.NovelSubmissionNovels)
                .HasForeignKey(d => d.NovelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__NovelSubm__novel__14270015");

            entity.HasOne(d => d.User).WithMany(p => p.NovelSubmissions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__NovelSubm__userI__151B244E");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(e => e.RatingId).HasName("PK__Rating__2D290D4959DD8736");

            entity.ToTable("Rating");

            entity.HasIndex(e => new { e.UserId, e.NovelId }, "UQ_User_Novel").IsUnique();

            entity.Property(e => e.RatingId).HasColumnName("ratingID");
            entity.Property(e => e.NovelId).HasColumnName("novelID");
            entity.Property(e => e.Score).HasColumnName("score");
            entity.Property(e => e.UserId).HasColumnName("userID");

            entity.HasOne(d => d.Novel).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.NovelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Rating__novelID__18EBB532");

            entity.HasOne(d => d.User).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Rating__userID__17F790F9");
        });

        modelBuilder.Entity<ReadingHistory>(entity =>
        {
            entity.HasKey(e => e.ReadingId).HasName("PK__ReadingH__D66D552BEEDB1CD7");

            entity.ToTable("ReadingHistory");

            entity.Property(e => e.ReadingId).HasColumnName("readingID");
            entity.Property(e => e.ChapterId).HasColumnName("chapterID");
            entity.Property(e => e.LastReadDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("lastReadDate");
            entity.Property(e => e.NovelId).HasColumnName("novelID");
            entity.Property(e => e.UserId).HasColumnName("userID");

            entity.HasOne(d => d.Chapter).WithMany(p => p.ReadingHistories)
                .HasForeignKey(d => d.ChapterId)
                .HasConstraintName("FK__ReadingHi__chapt__1BC821DD");

            entity.HasOne(d => d.Novel).WithMany(p => p.ReadingHistories)
                .HasForeignKey(d => d.NovelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ReadingHi__novel__1AD3FDA4");

            entity.HasOne(d => d.User).WithMany(p => p.ReadingHistories)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ReadingHi__userI__19DFD96B");
        });

        modelBuilder.Entity<UserAccount>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__UserAcco__CB9A1CDF854A8A6D");

            entity.ToTable("UserAccount");

            entity.HasIndex(e => e.UserName, "UQ__UserAcco__66DCF95CFF954808").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__UserAcco__AB6E6164BB275E60").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("userID");
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("creationDate");
            entity.Property(e => e.DateOfBirth).HasColumnName("dateOfBirth");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .HasColumnName("fullName");
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .HasColumnName("gender");
            entity.Property(e => e.ImageUml)
                .HasMaxLength(255)
                .HasColumnName("imageUML");
            entity.Property(e => e.NumberPhone)
                .HasMaxLength(15)
                .HasColumnName("numberPhone");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.Status)
                .HasDefaultValue(false)
                .HasColumnName("status");
            entity.Property(e => e.Type)
                .HasMaxLength(10)
                .HasDefaultValue("normal")
                .HasColumnName("type");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .HasColumnName("userName");
        });

        modelBuilder.Entity<Viewing>(entity =>
        {
            entity.HasKey(e => e.ViewId).HasName("PK__Viewing__2D7A9BAFB3E73ED5");

            entity.ToTable("Viewing");

            entity.Property(e => e.ViewId).HasColumnName("viewID");
            entity.Property(e => e.NovelId).HasColumnName("novelID");

            entity.HasOne(d => d.Novel).WithMany(p => p.Viewings)
                .HasForeignKey(d => d.NovelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Viewing__novelID__1CBC4616");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
