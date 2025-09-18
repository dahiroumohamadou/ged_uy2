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
        // UYII
        public DbSet<_Arrete> _Arretes { get; set; }
        public DbSet<_Attestation> _Attestations { get; set; }
        public DbSet<_Certificat> _Certificats { get; set; }
        public DbSet<_Communique> _Communiques { get; set; }
        public DbSet<_Contrat> _Contrats { get; set; }
        public DbSet<_Correspondance> _Correspondances { get; set; }
        public DbSet<_Decision> _Decisions { get; set; }
        public DbSet<_Decret> _Decrets { get; set; }
        public DbSet<_EtatPaiement> _EtatPaiements { get; set; }
        public DbSet<_NoteService> _NoteServices { get; set; }
        // old
        public DbSet<User> Users { get; set; }
        public DbSet<Cycle> Cycles { get; set; }
        public DbSet<Doc> Docs { get; set; }
        public DbSet<Filiere> Filieres { get; set; }

        // new
        public DbSet<Arrete> Arretes { get; set; }
        public DbSet<Chronogramme> Chronogrammes { get; set; }
        public DbSet<Correspondance> Correspondances { get; set; }
        public DbSet<Dossier> Dossiers { get; set; }
        public DbSet<Examen> Examens { get; set; }
        public DbSet<Faculte> Facultes { get; set; }
        public DbSet<Filieree> Filierees { get; set; }
        public DbSet<Liste> Listes { get; set; }
        public DbSet<PvCNE> PvCNEs { get; set; }
        public DbSet<PvExamen> PvExamens { get; set; }
        public DbSet<Rapport> Rapports { get; set; }
        public DbSet<Decharge> Decharges { get; set; }
        public DbSet<Source> Sources { get; set; }
        public DbSet<Structure> Structures { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // UYII
            modelBuilder.Entity<_Arrete>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.Property(a => a.Numero);
                entity.Property(a=> a.Objet);
                entity.Property(a => a.Signataire);
                entity.Property(a=> a.DateSign);
                entity.Property(a => a.Status);
                entity.Property(a => a.Created);
                entity.Property(a => a.Updated);
            });
            modelBuilder.Entity<_Attestation>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.Property(a => a.Numero);
                entity.Property(a => a.Type);
                entity.Property(a => a.Destinataire);
                entity.Property(a => a.Signataire);
                entity.Property(a => a.DateSign);
                entity.Property(a => a.Status);
                entity.Property(a => a.Created);
                entity.Property(a => a.Updated);

            });
            modelBuilder.Entity<_Certificat>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Numero);
                entity.Property(c => c.Type);
                entity.Property(c => c.Destinataire);
                entity.Property(c => c.Signataire);
                entity.Property(c => c.DateSign);
                entity.Property(c => c.Status);
                entity.Property(c => c.Created);
                entity.Property(c => c.Updated);

            });
            modelBuilder.Entity<_Communique>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Numero);
                entity.Property(c => c.Objet);
                entity.Property(c => c.Signataire);
                entity.Property(c => c.DateSign);
                entity.Property(c => c.Status);
                entity.Property(c => c.Created);
                entity.Property(c => c.Updated);

            });
            modelBuilder.Entity<_Contrat>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Numero);
                entity.Property(c => c.Type);
                entity.Property(c => c.Beneficiaire);
                entity.Property(c => c.Signataire);
                entity.Property(c => c.DateSign);
                entity.Property(c => c.Status);
                entity.Property(c => c.Created);
                entity.Property(c => c.Updated);

            });
            modelBuilder.Entity<_Correspondance>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Reference);
                entity.Property(c => c.Objet);
                entity.Property(c => c.Emetteur);
                entity.Property(c => c.Recepteur);
                entity.Property(c => c.Signataire);
                entity.Property(c => c.DateSign);
                entity.Property(c => c.Status);
                entity.Property(c => c.Created);
                entity.Property(c => c.Updated);

            });
            modelBuilder.Entity<_Decision>(entity =>
            {
                entity.HasKey(d => d.Id);
                entity.Property(d => d.Type);
                entity.Property(d => d.Objet);
                entity.Property(d => d.Signataire);
                entity.Property(d => d.DateSign);
                entity.Property(d => d.Status);
                entity.Property(d => d.Created);
                entity.Property(d => d.Updated);

            });
            modelBuilder.Entity<_Decret>(entity =>
            {
                entity.HasKey(d => d.Id);
                entity.Property(d => d.Numero);
                entity.Property(d=>d.Objet);
                entity.Property(d => d.Signataire);
                entity.Property(d => d.DateSign);
                entity.Property(d => d.Status);
                entity.Property(d => d.Created);
                entity.Property(d => d.Updated);

            });
            modelBuilder.Entity<_EtatPaiement>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Numero);
                entity.Property(e=>e.Objet);
                entity.Property(e => e.Signataire);
                entity.Property(e => e.DateSign);
                entity.Property(e => e.Status);
                entity.Property(e => e.Created);
                entity.Property(e => e.Updated);

            });
            modelBuilder.Entity<_NoteService>(entity =>
            {
                entity.HasKey(n => n.Id);
                entity.Property(n => n.Numero);
                entity.Property(n=>n.Objet);
                entity.Property(n => n.Signataire);
                entity.Property(n => n.DateSign);
                entity.Property(n => n.Status);
                entity.Property(n => n.Created);
                entity.Property(n => n.Updated);

            });
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
                entity.Property(u => u.Service);
                entity.Property(u => u.SaltPassword);
                entity.Property(u => u.Token);
                entity.Property(u => u.KeepLoginIn);
            });
            // new
            modelBuilder.Entity<Arrete>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.Property(a=>a.Numero);
                entity.Property(a => a.Origine);
                entity.Property(a => a.NumeroCNE);
                entity.Property(a => a.DateCne);
                entity.Property(a => a.Objet);
                entity.Property(a=>a.Status);
                entity.Property(a=>a.Created);
                entity.Property(a=>a.Updated);
                entity.HasOne(p => p.Structure)
                   .WithMany(s => s.Arretes)
                   .HasForeignKey(p => p.StructureId)
                   .OnDelete(DeleteBehavior.NoAction);


            });
            modelBuilder.Entity<Chronogramme>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c=>c.NumeroPTA);
                entity.Property(c=>c.Libele);
                entity.Property(c=>c.Annee);
                entity.Property(c => c.Origine);
                entity.Property(c=>c.Status);
                entity.Property(c=>c.Created);
                entity.Property(c => c.Updated);
                entity.HasOne(s=>s.Structure)
                    .WithMany(c=>c.Chronos)
                    .HasForeignKey(s=>s.StructureId)
                    .OnDelete(DeleteBehavior.NoAction);

            });
            modelBuilder.Entity<Correspondance>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c=>c.Objet);
                entity.Property(c => c.Date);
                entity.Property(c => c.Type);
                entity.Property(c => c.Origine);
                entity.Property(c => c.Initiateur);
                entity.Property(c=>c.Status);
                entity.Property(c=>c.Created);
                entity.Property(c=>c.Updated);
                
                entity.HasOne(s=>s.Structure)
                    .WithMany(c=>c.Correspondances)
                    .HasForeignKey(s=>s.StructureId)
                    .OnDelete(DeleteBehavior.NoAction);

            });
            modelBuilder.Entity<Dossier>(entity =>
            {
                entity.HasKey(d => d.Id);
                entity.Property(d=>d.Objet);
                entity.Property(d=>d.Numero);
                entity.Property(d => d.Origine);
                entity.Property(d=>d.Demandeur);
                entity.Property(d => d.DateEntree);
                entity.Property(d => d.DateSortie);
                entity.Property(d => d.PersonneTraite);
                entity.Property(d=>d.Status);
                entity.Property(d => d.Created);
                entity.Property(d => d.Updated);
                entity.HasOne(d => d.Structure)
                    .WithMany(s => s.Dossiers)
                    .HasForeignKey(d => d.StructureId)
                    .OnDelete(DeleteBehavior.NoAction);

            });
            modelBuilder.Entity<Examen>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nom);
                entity.Property(e => e.Code);
                entity.Property(e => e.Created);
                entity.Property(e => e.Updated);
                entity.HasMany(e => e.Pvs)
                 .WithOne(p => p.Examen)
                 .HasForeignKey(p => p.ExamenId)
                 .OnDelete(DeleteBehavior.NoAction);

            });
            modelBuilder.Entity<Faculte>(entity =>
            {
                entity.HasKey(f => f.Id);
                entity.Property(f => f.Code);
                entity.Property(f => f.Libele);
                entity.Property(f=>f.Created);
                entity.Property(f=>f.Updated);
                entity.HasMany(f => f.PvExamens)
                   .WithOne(p => p.Faculte)
                   .HasForeignKey(p => p.FaculteId)
                   .OnDelete(DeleteBehavior.NoAction);

            });
            modelBuilder.Entity<Filieree>(entity =>
            {
                entity.HasKey(f => f.Id);
                entity.Property(f => f.Code);
                entity.Property(f => f.Created);
                entity.Property(f=>f.Updated);
                entity.HasMany(f => f.PvExamens)
                    .WithOne(p => p.Filiere)
                    .HasForeignKey(p => p.FiliereId)
                    .OnDelete(DeleteBehavior.NoAction);

            });
            modelBuilder.Entity<Liste>(entity =>
            {
                entity.HasKey(l => l.Id);
                entity.Property(l => l.Origine);
                entity.Property(l => l.Numero);
                entity.Property(l => l.DateSign);
                //entity.Property(l=>l.Type);
                entity.Property(l => l.NumeroCne);
                entity.Property(l=>l.DateCne);
                entity.Property(l => l.Status);
                entity.Property(l=>l.Descritpion);
                entity.Property(l=>l.Created);
                entity.Property(l=>l.Updated);
                entity.HasOne(l => l.Structure)
                    .WithMany(s => s.Listes)
                    .HasForeignKey(l => l.StructureId)
                    .OnDelete(DeleteBehavior.NoAction);

            });
            modelBuilder.Entity<PvCNE>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Description);
                entity.Property(p => p.Origine);
                entity.Property(p => p.NumeroCne);
                entity.Property(p => p.DateCne);
                entity.Property(p => p.Status);
                entity.Property(p => p.Created);
                entity.Property(p => p.Updated);
                entity.HasOne(p=>p.Structure)
                    .WithMany(s=>s.PvCNEs)
                    .HasForeignKey(p=>p.StructureId)
                    .OnDelete(DeleteBehavior.NoAction);

            });
            modelBuilder.Entity<PvExamen>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Session);
                entity.Property(p=>p.Status);
                entity.Property(p=>p.Created);
                entity.Property(p => p.Updated);
                entity.HasOne(p=>p.Source)
                    .WithMany(s=>s.PvExamens)
                    .HasForeignKey(p=>p.SourceId)
                    .OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(p=>p.Filiere)
                    .WithMany(f=>f.PvExamens)
                    .HasForeignKey(p=>p.FiliereId)
                    .OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(p=>p.Faculte)
                    .WithMany(f=>f.PvExamens)
                    .HasForeignKey(p=>p.FaculteId)
                    .OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(p => p.Examen)
                    .WithMany(e => e.Pvs)
                    .HasForeignKey(p => p.ExamenId)
                    .OnDelete(DeleteBehavior.NoAction);

            });
            modelBuilder.Entity<Rapport>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Status);
                entity.Property(r => r.Debut);
                entity.Property(r => r.Fin);
                entity.Property(r => r.Origine);
                entity.Property(r=>r.Description);
                entity.Property(r => r.Created);
                entity.Property(r=>r.Updated);
                entity.HasOne(r=>r.Structure)
                    .WithMany(s=>s.Rapports)
                    .HasForeignKey(r=>r.StructureId)
                    .OnDelete(DeleteBehavior.NoAction);

            });
            modelBuilder.Entity<Decharge>(entity =>
            {
                entity.HasKey(d => d.Id);
                entity.Property(d => d.Date);
                entity.Property(d => d.Origine);
                entity.Property(d => d.Description);
                entity.Property(d => d.Status);
                entity.Property(r => r.Created);
                entity.Property(r => r.Updated);
            });
            modelBuilder.Entity<Source>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Code);
                entity.Property(s => s.Libele);
                entity.Property(s => s.Created);
                entity.Property(s => s.Updated);
                entity.HasMany(s=>s.PvExamens)
                    .WithOne(p=>p.Source)
                    .HasForeignKey(p=>p.SourceId)
                    .OnDelete(DeleteBehavior.NoAction);
            });
            modelBuilder.Entity<Structure>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Code);
                entity.Property(s => s.Libele);
                entity.Property(s => s.Created);
                entity.Property(s => s.Updated);
                entity.HasMany(s=>s.Chronos)
                    .WithOne(c=>c.Structure)
                    .HasForeignKey(c=>c.StructureId)
                    .OnDelete(DeleteBehavior.NoAction);
                entity.HasMany(s => s.Rapports)
                    .WithOne(r => r.Structure)
                    .HasForeignKey(r => r.StructureId)
                    .OnDelete(DeleteBehavior.NoAction);
                entity.HasMany(s=>s.Arretes)
                    .WithOne(a=>a.Structure)
                    .HasForeignKey(a=>a.StructureId)
                    .OnDelete(DeleteBehavior.NoAction);
                entity.HasMany(s => s.PvCNEs)
                    .WithOne(p => p.Structure)
                    .HasForeignKey(p => p.StructureId)
                    .OnDelete(DeleteBehavior.NoAction);
                entity.HasMany(s=>s.PvExamens)
                    .WithOne(p=>p.Structure)
                    .HasForeignKey(p=>p.StructureId)
                    .OnDelete(DeleteBehavior.NoAction);
                entity.HasMany(s => s.Correspondances)
                    .WithOne(c => c.Structure)
                    .HasForeignKey(c => c.StructureId)
                    .OnDelete(DeleteBehavior.NoAction);
                entity.HasMany(s => s.Dossiers)
                     .WithOne(d => d.Structure)
                     .HasForeignKey(d => d.StructureId)
                     .OnDelete(DeleteBehavior.NoAction);
                entity.HasMany(s => s.Listes)
                    .WithOne(l => l.Structure)
                    .HasForeignKey(l => l.StructureId)
                    .OnDelete(DeleteBehavior.NoAction);
                entity.HasMany(s => s.Decharges)
                .WithOne(d => d.Structure)
                .HasForeignKey(d => d.StructureId)
                .OnDelete(DeleteBehavior.NoAction);


            });
           
        }
    }
}
