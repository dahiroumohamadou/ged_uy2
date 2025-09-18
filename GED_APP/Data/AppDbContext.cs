using Microsoft.EntityFrameworkCore;
using GED_APP.Models;
using System.Configuration;
using Microsoft.Extensions.Options;

namespace GED_APP.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
        // old
        public DbSet<User> Users { get; set; }
        public DbSet<Cycle> Cycles { get; set; }
        public DbSet<Doc> Docs { get; set; }
        public DbSet<Filiere> Filieres { get; set; }

        // new
        public DbSet<Arrete> Arretes { get; set; }
        public DbSet<Chronogramme> Chronogrammes { get; set; }
        public DbSet<CNE> CNEs { get; set; }
        public DbSet<Correspondance> Correspondances { get; set; }
        public DbSet<Dossier> Dossiers { get; set; }
        public DbSet<Examen> Examens { get; set; }
        public DbSet<Faculte> Facultes { get; set; }
        public DbSet<Filieree> Filierees { get; set; }
        public DbSet<Liste> Listes { get; set; }
        public DbSet<PvCNE> PvCNEs { get; set; }
        public DbSet<PvExamen> PvExamens { get; set; }
        public DbSet<Rapport> Rapports { get; set; }
        public DbSet<Source> Sources { get; set; }
        public DbSet<Structure> Structures { get; set; }
        public DbSet<TypeCorresp> TypeCorresps { get; set; }

       


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // old
            modelBuilder.Entity<Doc>(entity =>
            {
                entity.HasKey(d => d.Id);
                entity.Property(d => d.Numero);
                entity.Property(d => d.Source);
                entity.Property(d => d.Objet);
                entity.Property(d => d.DateSign);
                entity.Property(d => d.Signataire);
                entity.Property(d => d.AnneeAcademique);
                entity.Property(d => d.AnneeSortie);
                entity.Property(d => d.TypeDoc);
                entity.Property(d => d.Fichier);
                entity.Property(d => d.Session);
                entity.Property(d => d.Promotion);
                entity.Property(d => d.Created);
                entity.Property(d => d.Updated);
                entity.HasOne(d => d.Cycle)
                    .WithMany(c => c.Documents)
                    .HasForeignKey(d => d.CycleId)
                    .OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(d => d.Filiere)
                    .WithMany(f => f.Documents)
                    .HasForeignKey(d => d.FiliereId)
                    .OnDelete(DeleteBehavior.NoAction);
            });
            modelBuilder.Entity<Cycle>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Code);
                entity.Property(c => c.Libele);
                entity.Property(c => c.Created);
                entity.Property(c => c.Updated);
                entity.HasMany(c => c.Documents)
                    .WithOne(p => p.Cycle)
                    .HasForeignKey(p => p.CycleId)
                    .OnDelete(DeleteBehavior.NoAction);
            });
            modelBuilder.Entity<Filiere>(entity =>
            {
                entity.HasKey(f => f.Id);
                entity.Property(f => f.Libele);
                entity.Property(f => f.Created);
                entity.Property(f => f.Updated);
                entity.HasMany(f => f.Documents)
                    .WithOne(p => p.Filiere)
                    .HasForeignKey(p => p.FiliereId)
                    .OnDelete(DeleteBehavior.NoAction);
            });
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.UserName);
                entity.Property(u => u.Password);
                entity.Property(u => u.UserEmail);
                entity.Property(u => u.SaltPassword);
                entity.Property(u => u.Token);
                entity.Property(u => u.KeepLoginIn);
            });
            // new
            modelBuilder.Entity<Arrete>(entity =>
            {

            });
            modelBuilder.Entity<Chronogramme>(entity =>
            {

            });
            modelBuilder.Entity<CNE>(entity =>
            {

            });
            modelBuilder.Entity<Correspondance>(entity =>
            {

            });
            modelBuilder.Entity<Dossier>(entity =>
            {

            });
            modelBuilder.Entity<Examen>(entity =>
            {

            });
            modelBuilder.Entity<Faculte>(entity =>
            {

            });
            modelBuilder.Entity<Filieree>(entity =>
            {

            });
            modelBuilder.Entity<Liste>(entity =>
            {

            });
            modelBuilder.Entity<PvCNE>(entity =>
            {

            });
            modelBuilder.Entity<PvExamen>(entity =>
            {

            });
            modelBuilder.Entity<Rapport>(entity =>
            {

            });
            modelBuilder.Entity<Source>(entity =>
            {

            });
            modelBuilder.Entity<Structure>(entity =>
            {

            });
            modelBuilder.Entity<TypeCorresp>(entity =>
            {

            });

        }
    }
}
