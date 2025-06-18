using GED_APP.Data;
using GED_APP.Models;
using GED_APP.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GED_APP.Repository.Implementations
{
    public class DocumentRepo:IDocument
    {
        private readonly AppDbContext _ctx;
        public DocumentRepo(AppDbContext ctx)
        {
            _ctx = ctx;
        }
        public int Add(Doc document)
        {
            int res = -1;
            if (document != null)
            {
                _ctx.Docs.Add(document);
                _ctx.SaveChanges();
                res = document.Id;
                //var pv = _ctx.Documents.Where(dc => ((dc.Source == document.Source) && (dc.Session == document.Session) && (dc.Promotion == document.Promotion) && (dc.AnneeSortie == document.AnneeSortie) && (dc.CycleId == document.CycleId) && (dc.FiliereId == document.FiliereId))).FirstOrDefault() ?? null;
                //var crp = _ctx.Documents.Where(dc => ((dc.Source == document.Source) && (dc.Numero == document.Numero) && (dc.DateSign == document.DateSign) && (dc.Session == document.Session) && (dc.AnneeAcademique == document.AnneeAcademique) && (dc.CycleId == document.CycleId))).FirstOrDefault() ?? null;
                //var arr = _ctx.Documents.Where(dc => ((dc.Source == document.Source) && (dc.Numero == document.Numero) && (dc.DateSign == document.DateSign) && (dc.AnneeAcademique == document.AnneeAcademique) && (dc.CycleId == document.CycleId))).FirstOrDefault() ?? null;
                //var autre = _ctx.Documents.Where(dc => ((dc.Source == document.Source) && (dc.Numero == document.Numero) && (dc.DateSign == document.DateSign))).FirstOrDefault() ?? null;
                //if ((pv != null) || (crp != null) || (arr != null)|| (autre!=null))
                //{
                //    res = 0;

                //}
                //else
                //{

                //}
            }
            return res;
        }
        public int Delete(int id)
        {
            int res = -1;
            var d = _ctx.Docs.Where(dc => dc.Id == id).FirstOrDefault() ?? null;
            if (d != null)
            {
                _ctx.Docs.Remove(d);
                _ctx.SaveChanges();
                res = d.Id;
            }
            return res;
        }

        public ICollection<Doc> GetAll()
        {
            var ds = _ctx.Docs
                .Include(d => d.Cycle)
                .Include(d => d.Filiere)
                .ToList();
            return ds;
        }
        public ICollection<Doc> GetAllByType(string type)
        {
            var ds = _ctx.Docs
                .Include(dc => dc.Cycle)
                .Include(dc => dc.Filiere)
                .Where(dc => dc.TypeDoc == type)
                .OrderByDescending(dc => dc.Created)
                .ToList();

            return ds;
        }

        public Doc GetById(int id)
        {
            var d = _ctx.Docs
                .Include(dc => dc.Cycle)
                .Where(dc => dc.Id == id).FirstOrDefault() ?? null;
            return d;
        }

        public int Update(Doc document)
        {
            int res = -1;
            var d = _ctx.Docs.Where(dc => dc.Id == document.Id).FirstOrDefault() ?? null;
            if (d != null)
            {
                d.TypeDoc = document.TypeDoc;
                d.Numero = document.Numero;
                d.Source = document.Source;
                d.Objet = document.Objet;
                d.DateSign = document.DateSign;
                d.Signataire = document.Signataire;
                d.AnneeAcademique = document.AnneeAcademique;
                d.CycleId = document.CycleId;
                d.Fichier = document.Fichier;
                d.Session = document.Session;
                d.Promotion = document.Promotion;
                d.AnneeSortie = document.AnneeSortie;
                d.FiliereId = document.FiliereId;
                d.Updated = DateTime.Now;
                _ctx.Docs.Update(d);
                _ctx.SaveChanges();
                res = d.Id;
            }
            return res;
        }
        public void Dispose()
        {
            _ctx?.Dispose();
        }
        public Doc ExisteAr(string source, string numero, string dateSign, string anneeAca, int? cycleId)
        {
            var arr = _ctx.Docs.Where(dc => (dc.Source == source && dc.Numero == numero & dc.DateSign == dateSign && dc.AnneeAcademique == anneeAca && dc.CycleId == cycleId)).FirstOrDefault() ?? null;
            return arr;
        }

        public Doc ExisteCrp(string source, string numero, string dateSign, string session, string anneeAca, int? cycleId)
        {
            var crp = _ctx.Docs.Where(dc => (dc.Source == source && dc.Numero == numero & dc.DateSign == dateSign && dc.Session == session && dc.AnneeAcademique == anneeAca && dc.CycleId == cycleId)).FirstOrDefault() ?? null;
            return crp;
        }

        public Doc ExisteOthers(string source, string numero, string dateSign)
        {
            var arr = _ctx.Docs.Where(dc => (dc.Source == source && dc.Numero == numero & dc.DateSign == dateSign)).FirstOrDefault() ?? null;
            return arr;
        }
        public Doc ExistePv(string source, string session, string promotion, string anneeSortie, int? cycleId, int? filiereId)
        {
            var pv = _ctx.Docs.Where(dc => (dc.Source == source && dc.Session == session && dc.Promotion == promotion & dc.AnneeSortie == anneeSortie && dc.CycleId == cycleId && dc.FiliereId == filiereId)).FirstOrDefault() ?? null;
            return pv;
        }
        public ICollection<Doc> GetAllByAnnee(string annee)
        {
            var docs = _ctx.Docs
                .Include(d => d.Cycle)
                .Include(d => d.Filiere)
                .Where(d => d.DateSign.EndsWith(annee) || d.AnneeSortie.EndsWith(annee));
            return docs.ToList();
        }

        public ICollection<Doc> GetAllByTypeAnnee(string? type, string? annee)
        {
            var ds = _ctx.Docs
               .Include(dc => dc.Cycle)
               .Include(dc => dc.Filiere)
               .Where(dc => dc.TypeDoc == type && (dc.DateSign.EndsWith(annee) || dc.AnneeSortie.EndsWith(annee)))
               .OrderByDescending(dc => dc.Created)
               .ToList();
            return ds;
        }
    }
}
