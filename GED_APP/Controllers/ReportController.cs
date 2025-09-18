using GED_APP.Reports;
using GED_APP.Repository.Interfaces;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.AspNetCore.Mvc;
using GED_APP.Models;
using AspNetCoreGeneratedDocument;
using GED_APP.Repository.Implementations;

namespace GED_APP.Controllers
{
    //[Authorize]
    public class ReportController : Controller
    {
        private readonly _ICorrespondance _corpRepo;
        private readonly _IContrat _contrRepo;
        private readonly _IDecision _decisRepo;
        private readonly _ICommunique _commRepo;
        private readonly _IAttestation _attestRepo;
        private readonly _IArrete _arretRepo;
        private readonly _IDecret _decretRepo;
        private readonly _IEtatPaiement _etatPaieRepo;
        private readonly _ICertificat _certfRepo;
        private readonly _INoteService _noteRepo;
        

        /*
        private readonly IStructure _structRepo;
        private readonly IArrete _arreteRepo;
        private readonly IChronogramme _chronoRepo;
        private readonly ICorrespondance _correspRepo;
        private readonly IDecharge _dechargeRepo;
        private readonly IDossier _dosRepo;
        private readonly IListe _listeRepo;
        private readonly IPvCne _pvCneRepo;
        private readonly IPvExamen _pvExamRepo;
        private readonly IRapport _rapportRepo;*/
       

        // variables config reports
        int _totalColumn = 3;
        Document _pdoc;
        BaseColor bgcolor = BaseColor.LIGHT_GRAY;

        Font _fontStyleEntete = new Font(Font.FontFamily.HELVETICA, 7f, Font.BOLD);
        Font _fontStyleEnteteSmall = new Font(Font.FontFamily.HELVETICA, 5f, Font.BOLD);
        // font style info
        Font _fontStyleInfo = new Font(Font.FontFamily.HELVETICA, 10f, Font.BOLD);
        Font _fontStyleInfolIatlic = new Font(Font.FontFamily.HELVETICA, 7f, Font.BOLDITALIC);
        Font _fontStyleInfoItalicSmall = new Font(Font.FontFamily.HELVETICA, 5f, Font.BOLDITALIC);

        Font _fontStyleTitre = new Font(Font.FontFamily.HELVETICA, 12f, Font.BOLD);
        Font _fontStyleSousTitreSmall = new Font(Font.FontFamily.HELVETICA, 10f, Font.BOLD);
        PdfPTable _tableau = new PdfPTable(3);
        MemoryStream _memoryStream = new MemoryStream();
        public ReportController(_ICorrespondance corspRepo, _IContrat contrRepo, _IDecision decsRepo, _ICommunique comRepo, _IAttestation attestRepo, _IArrete arretRepo, _IDecret decretRepo, _IEtatPaiement etatRepo, _ICertificat certRepo, _INoteService noteRepo)
        {
            _corpRepo = corspRepo;
            _contrRepo = contrRepo;
            _decisRepo= decsRepo;
            _commRepo = comRepo;
            _attestRepo= attestRepo;
            _arretRepo= arretRepo;
            _decretRepo = decretRepo;
            _noteRepo= noteRepo;
            _etatPaieRepo= etatRepo;
            _certfRepo= certRepo;

        }
       
        // GET: ReportController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ReportController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ReportController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ReportController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult print(string annee)
        {
            try
            {
                //DocumentReport docReport = new DocumentReport();
                byte[] bytes = _prepareReportDocByYear(annee);
                return File(bytes, "application/pdf");
            }
            catch(Exception e)
            {
                TempData["AlertMessage"] = "ERROR....."+e.Message;
                return RedirectToAction(nameof(Index));
            }
        }
        [HttpPost, ActionName("test")]
        [ValidateAntiForgeryToken]
        public IActionResult test()
        {


            TempData["AlertMessage"] = "bonjour admin";

            return RedirectToAction(nameof(Index));
        }
        // GET: ReportController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ReportController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ReportController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ReportController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        //[NonAction]
        public byte[] _prepareReportDocByYear(string annee)
        {
           
            // iTextSharp.text.Image logoM=default(iTextSharp.text.Image);
            //  logoM = iTextSharp.text.Image.GetInstance("./wwwroot/img/images/logo.png");
            Image logo = Image.GetInstance("./wwwroot/img/images/logo_uy2.png");

            _pdoc = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _pdoc.SetPageSize(PageSize.A4) ;
            _pdoc.SetMargins(10f, 10f, 10f, 10f);

            PdfWriter.GetInstance(_pdoc, _memoryStream);
            _pdoc.Open();
            // Paragraph p = new Paragraph("Hello wold : " + membre.Noms + DateTime.Today.Year);
            PdfPTable tableEntete = new PdfPTable(3);
            PdfPCell cell = new PdfPCell(new Phrase("REPUBLIQUE DU CAMEROUN", _fontStyleEntete));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.Border = Rectangle.NO_BORDER;
            tableEntete.AddCell(cell);
            cell = new PdfPCell();
            cell.Rowspan = 10;
            //logo
            //Image logo = Image.GetInstance("./wwwroot/img/images/logo.png");
            logo.Alignment = Element.ALIGN_CENTER;
            logo.ScaleAbsolute(70, 50);
            logo.Border = Rectangle.BOX;
            cell.AddElement(logo);
            cell.Border = Rectangle.NO_BORDER;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            tableEntete.AddCell(cell);
            // end logo
            cell = new PdfPCell(new Phrase("REPUBLIC OF CAMEROUN", _fontStyleEntete));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.Border = Rectangle.NO_BORDER;
            tableEntete.AddCell(cell);

            cell = new PdfPCell(new Phrase("Paix-Travail-Patrie", _fontStyleEnteteSmall));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.Border = Rectangle.NO_BORDER;
            tableEntete.AddCell(cell);

            cell = new PdfPCell(new Phrase("Peace-Work-Fatherland", _fontStyleEnteteSmall));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.Border = Rectangle.NO_BORDER;
            tableEntete.AddCell(cell);

            cell = new PdfPCell(new Phrase("MINISTERE DE L'ENSEIGNEMENT SUPERIEUR", _fontStyleEntete));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.Border = Rectangle.NO_BORDER;
            tableEntete.AddCell(cell);
            cell = new PdfPCell(new Phrase("MINISTRY OF HIGHER EDUCATION", _fontStyleEntete));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.Border = Rectangle.NO_BORDER;
            tableEntete.AddCell(cell);

            cell = new PdfPCell(new Phrase("-------------", _fontStyleEnteteSmall));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.Border = Rectangle.NO_BORDER;
            tableEntete.AddCell(cell);
            cell = new PdfPCell(new Phrase("-------------", _fontStyleEnteteSmall));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.Border = Rectangle.NO_BORDER;
            tableEntete.AddCell(cell);

            cell = new PdfPCell(new Phrase("UNIVERSITE DE YAOUNDE II SOA", _fontStyleEntete));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.Border = Rectangle.NO_BORDER;
            tableEntete.AddCell(cell);
            cell = new PdfPCell(new Phrase("UNIVERSITY OF YAOUNDE II SOA", _fontStyleEntete));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.Border = Rectangle.NO_BORDER;
            tableEntete.AddCell(cell);

            cell = new PdfPCell(new Phrase("-------------", _fontStyleEnteteSmall));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.Border = Rectangle.NO_BORDER;
            tableEntete.AddCell(cell);
            cell = new PdfPCell(new Phrase("-------------", _fontStyleEnteteSmall));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.Border = Rectangle.NO_BORDER;
            tableEntete.AddCell(cell);
/*
            cell = new PdfPCell(new Phrase("DEPARTEMENT DES ETUDES", _fontStyleEntete));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.Border = Rectangle.NO_BORDER;
            tableEntete.AddCell(cell);
            cell = new PdfPCell(new Phrase("DEPARTEMNENT OF STUDIES", _fontStyleEntete));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.Border = Rectangle.NO_BORDER;
            tableEntete.AddCell(cell);

            cell = new PdfPCell(new Phrase("-------------", _fontStyleEnteteSmall));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.Border = Rectangle.NO_BORDER;
            tableEntete.AddCell(cell);
            cell = new PdfPCell(new Phrase("-------------", _fontStyleEnteteSmall));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.Border = Rectangle.NO_BORDER;
            tableEntete.AddCell(cell);

            cell = new PdfPCell(new Phrase("SERVICE DE LA SCOLARITE", _fontStyleEntete));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.Border = Rectangle.NO_BORDER;
            tableEntete.AddCell(cell);
            cell = new PdfPCell(new Phrase("SCHOOLING SERVICE", _fontStyleEntete));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.Border = Rectangle.NO_BORDER;
            tableEntete.AddCell(cell);*/
            _pdoc.Add(tableEntete);

            Paragraph p = new Paragraph(new Chunk(" ", _fontStyleSousTitreSmall));
            _pdoc.Add(p);
            // titre Fiche
            PdfPTable tableTitreFiche = new PdfPTable(1);

            cell = new PdfPCell(new Phrase("LISTE DES DOCUMENTS ", _fontStyleTitre));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.Border = Rectangle.NO_BORDER;
            cell.BackgroundColor = bgcolor;
            cell.BackgroundColor = bgcolor;
            tableTitreFiche.AddCell(cell);
            cell = new PdfPCell(new Phrase("LIST OF DOCUMENT ", _fontStyleSousTitreSmall));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.Border = Rectangle.NO_BORDER;
            cell.BackgroundColor = bgcolor;
            tableTitreFiche.AddCell(cell);

            cell = new PdfPCell(new Phrase("Pour le compte de l'année :  " + annee, _fontStyleSousTitreSmall));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.Border = Rectangle.NO_BORDER;
            cell.BackgroundColor = bgcolor;
            tableTitreFiche.AddCell(cell);

            cell = new PdfPCell(new Phrase(" ", _fontStyleEnteteSmall));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.Border = Rectangle.NO_BORDER;
            cell.BackgroundColor = bgcolor;
            tableTitreFiche.AddCell(cell);
            /*
            cell = new PdfPCell(new Phrase("("+s.Code+")", _fontStyleTitre));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.Border = Rectangle.NO_BORDER;
            cell.BackgroundColor = bgcolor;
            cell.BackgroundColor = bgcolor;
            tableTitreFiche.AddCell(cell);*/

            cell = new PdfPCell(new Phrase(" ", _fontStyleEnteteSmall));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.Border = Rectangle.NO_BORDER;
            cell.BackgroundColor = bgcolor;
            cell.BackgroundColor = bgcolor;
            tableTitreFiche.AddCell(cell);

            _pdoc.Add(tableTitreFiche);

            p = new Paragraph(new Chunk(" ", _fontStyleEnteteSmall));
            _pdoc.Add(p);

            if (annee != null)
            {

               ICollection<_Correspondance> corresps =_corpRepo.GetAllByAnnee(annee);
               ICollection<_Contrat> contrs =_contrRepo.GetAllByAnnee(annee);
               ICollection<_Decision> decs =_decisRepo.GetAllByAnnee(annee);
               ICollection<_Communique> communiqs =_commRepo.GetAllByAnnee(annee);
               ICollection<_Attestation> atts =_attestRepo.GetAllByAnnee(annee);
               ICollection<_Arrete> arrets =_arretRepo.GetAllByAnnee(annee);
               ICollection<_Decret> decrts =_decretRepo.GetAllByAnnee(annee);
               ICollection<_EtatPaiement> etps =_etatPaieRepo.GetAllByAnnee(annee);
               ICollection<_Certificat> crts =_certfRepo.GetAllByAnnee(annee);
               ICollection<_NoteService> nts =_noteRepo.GetAllByAnnee(annee);
                //***************************************************************
                //*************************** entete correspondance*********************
                // titre Correspondance
                PdfPTable tableTitreCorresp = new PdfPTable(1);

                        cell = new PdfPCell(new Phrase("I.  LA LISTE DES CORRESPONDANCES", _fontStyleTitre));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = Rectangle.NO_BORDER;
                    //cell.BackgroundColor = bgcolor;
                       tableTitreCorresp.AddCell(cell);
                        _pdoc.Add(tableTitreCorresp);

                        // ENTETE TABLEAU PVS
                        float[] colwidthtableCorresp = { 0.5f, 2f, 2f, 1f, 1f, 1f, 1f };
                        PdfPTable tableHistCorresp = new PdfPTable(colwidthtableCorresp);
                        tableHistCorresp.WidthPercentage = 95;
                        cell = new PdfPCell(new Phrase("N°", _fontStyleInfo));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.BackgroundColor = bgcolor;
                        tableHistCorresp.AddCell(cell);
                        cell = new PdfPCell(new Phrase("Numero", _fontStyleInfo));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.BackgroundColor = bgcolor;
                        tableHistCorresp.AddCell(cell);
                        cell = new PdfPCell(new Phrase("Objet", _fontStyleInfo));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.BackgroundColor = bgcolor;
                         tableHistCorresp.AddCell(cell);
                        cell = new PdfPCell(new Phrase("Emmeteur", _fontStyleInfo));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.BackgroundColor = bgcolor;
                        tableHistCorresp.AddCell(cell);
                        cell = new PdfPCell(new Phrase("Destinataire", _fontStyleInfo));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.BackgroundColor = bgcolor;
                        tableHistCorresp.AddCell(cell);
                        cell = new PdfPCell(new Phrase("Date Signature", _fontStyleInfo));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.BackgroundColor = bgcolor;
                        tableHistCorresp.AddCell(cell);
                       
                        cell = new PdfPCell(new Phrase("Numérisé ?", _fontStyleInfo));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.BackgroundColor = bgcolor;
                        tableHistCorresp.AddCell(cell);
                        //fin entete

                        int cpt = 0;
                        foreach (var d in corresps)
                        {
                            cpt++;
                            cell = new PdfPCell(new Phrase("" + cpt, _fontStyleInfolIatlic));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //cell.BackgroundColor = bgcolor;
                            tableHistCorresp.AddCell(cell);
                            cell = new PdfPCell(new Phrase("" + d.Reference, _fontStyleInfolIatlic));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //cell.BackgroundColor = bgcolor;
                            tableHistCorresp.AddCell(cell);
                            cell = new PdfPCell(new Phrase("" + d.Objet, _fontStyleInfolIatlic));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //cell.BackgroundColor = bgcolor;
                            tableHistCorresp.AddCell(cell);
                            cell = new PdfPCell(new Phrase("" + d.Emetteur, _fontStyleInfolIatlic));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //cell.BackgroundColor = bgcolor;
                            tableHistCorresp.AddCell(cell);
                            cell = new PdfPCell(new Phrase("" + d.Recepteur, _fontStyleInfolIatlic));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //cell.BackgroundColor = bgcolor;
                            tableHistCorresp.AddCell(cell);
                            cell = new PdfPCell(new Phrase("" + d.DateSign, _fontStyleInfolIatlic));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //cell.BackgroundColor = bgcolor;
                            tableHistCorresp.AddCell(cell);
                            if (d.Status == 1)
                            {
                                cell = new PdfPCell(new Phrase("Oui", _fontStyleInfolIatlic));
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        //cell.BackgroundColor = bgcolor;
                                tableHistCorresp.AddCell(cell);
                            }
                            else
                            {
                                cell = new PdfPCell(new Phrase("Non", _fontStyleInfolIatlic));
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        //cell.BackgroundColor = bgcolor;
                                 tableHistCorresp.AddCell(cell);
                            }


                        }
                        _pdoc.Add(tableHistCorresp);


                //***************************************************************
                //*************************** entete contrat*********************
                        float[] colwidthtableContr = { 0.5f, 2f, 1f, 1f, 1f, 1f };
                        PdfPTable tableCntr = new PdfPTable(colwidthtableContr);
                       tableCntr.WidthPercentage = 95;
                        if (contrs != null)
                        {
                            // titre CONTRAT
                            PdfPTable tableTitreCntr = new PdfPTable(1);

                            cell = new PdfPCell(new Phrase("II.  LA LISTE DES CONTRATS ", _fontStyleTitre));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.Border = Rectangle.NO_BORDER;
                    //cell.BackgroundColor = bgcolor;
                            tableTitreCntr.AddCell(cell);
                            _pdoc.Add(tableTitreCntr);

                            // ENTETE TABLEAU CONTRAT

                            cell = new PdfPCell(new Phrase("N°", _fontStyleInfo));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.BackgroundColor = bgcolor;
                            tableCntr.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Numero", _fontStyleInfo));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.BackgroundColor = bgcolor;
                            tableCntr.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Type", _fontStyleInfo));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.BackgroundColor = bgcolor;
                            tableCntr.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Béneficiaire", _fontStyleInfo));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.BackgroundColor = bgcolor;
                            tableCntr.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Date Singature", _fontStyleInfo));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.BackgroundColor = bgcolor;
                            tableCntr.AddCell(cell);
                            
                            cell = new PdfPCell(new Phrase("Numérisé ?", _fontStyleInfo));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.BackgroundColor = bgcolor;
                            tableCntr.AddCell(cell);
                            //fin entete
                            int i = 0;
                            foreach (var r in contrs)
                            {
                                i++;
                                cell = new PdfPCell(new Phrase("" + i, _fontStyleInfolIatlic));
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                //cell.BackgroundColor = bgcolor;
                                tableCntr.AddCell(cell);
                                cell = new PdfPCell(new Phrase("" + r.Numero, _fontStyleInfolIatlic));
                                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            //cell.BackgroundColor = bgcolor;
                                tableCntr.AddCell(cell);
                                cell = new PdfPCell(new Phrase("" + r.Type, _fontStyleInfolIatlic));
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                //cell.BackgroundColor = bgcolor;
                                tableCntr.AddCell(cell);
                                cell = new PdfPCell(new Phrase("" + r.Beneficiaire, _fontStyleInfolIatlic));
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                //cell.BackgroundColor = bgcolor;
                                tableCntr.AddCell(cell);
                                cell = new PdfPCell(new Phrase("" + r.DateSign, _fontStyleInfolIatlic));
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                //cell.BackgroundColor = bgcolor;
                                tableCntr.AddCell(cell);

                                
                                if (r.Status == 1)
                                {
                                    cell = new PdfPCell(new Phrase("Oui", _fontStyleInfolIatlic));
                                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                    //cell.BackgroundColor = bgcolor;
                                    tableCntr.AddCell(cell);
                                }
                                else
                                {
                                    cell = new PdfPCell(new Phrase("Non", _fontStyleInfolIatlic));
                                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                    //cell.BackgroundColor = bgcolor;
                                    tableCntr.AddCell(cell);
                                }
                            }
                            _pdoc.Add(tableCntr);

                        }

                //***************************************************************
                //*************************** entete decisions*********************
                float[] colwidthtableDecs = { 0.5f, 2f, 2f, 1f, 1f, 1f};
                        PdfPTable tableDecs = new PdfPTable(colwidthtableDecs);
                        tableDecs.WidthPercentage = 95;
                        if (decs != null)
                        {
                            // titre PV
                            PdfPTable tableTitreDecs = new PdfPTable(1);

                            cell = new PdfPCell(new Phrase("III.  LA LISTE DES DECISIONS", _fontStyleTitre));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.Border = Rectangle.NO_BORDER;
                            //cell.BackgroundColor = bgcolor;
                            tableTitreDecs.AddCell(cell);
                            _pdoc.Add(tableTitreDecs);
                            // ENTETE TABLEAU CRP

                            cell = new PdfPCell(new Phrase("N°", _fontStyleInfo));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.BackgroundColor = bgcolor;
                            tableDecs.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Numero", _fontStyleInfo));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.BackgroundColor = bgcolor;
                            tableDecs.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Objet", _fontStyleInfo));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.BackgroundColor = bgcolor;
                            tableDecs.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Type", _fontStyleInfo));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.BackgroundColor = bgcolor;
                            tableDecs.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Date", _fontStyleInfo));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.BackgroundColor = bgcolor;
                            tableDecs.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Numérisé ?", _fontStyleInfo));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.BackgroundColor = bgcolor;
                            tableDecs.AddCell(cell);
                            //fin entete
                            int i = 0;
                            foreach (var c in decs)
                            {
                                i++;
                                cell = new PdfPCell(new Phrase("" + i, _fontStyleInfolIatlic));
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                //cell.BackgroundColor = bgcolor;
                                tableDecs.AddCell(cell);
                                cell = new PdfPCell(new Phrase("" + c.Numero, _fontStyleInfolIatlic));
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                //cell.BackgroundColor = bgcolor;
                               tableDecs.AddCell(cell);
                                cell = new PdfPCell(new Phrase("" + c.Objet, _fontStyleInfolIatlic));
                                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                //cell.BackgroundColor = bgcolor;
                                tableDecs.AddCell(cell);
                                cell = new PdfPCell(new Phrase("" + c.Type, _fontStyleInfolIatlic));
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                //cell.BackgroundColor = bgcolor;
                                tableDecs.AddCell(cell);
                                
                                cell = new PdfPCell(new Phrase(" " + c.DateSign, _fontStyleInfolIatlic));
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                //cell.BackgroundColor = bgcolor;
                                tableDecs.AddCell(cell);
                                if (c.Status == 1)
                                {
                                    cell = new PdfPCell(new Phrase("Oui", _fontStyleInfolIatlic));
                                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                    //cell.BackgroundColor = bgcolor;
                                    tableDecs.AddCell(cell);
                                }
                                else
                                {
                                    cell = new PdfPCell(new Phrase("Non", _fontStyleInfolIatlic));
                                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                    //cell.BackgroundColor = bgcolor;
                                    tableDecs.AddCell(cell);
                                }
                            }
                            _pdoc.Add(tableDecs);

                        }
                //***************************************************************
                //*************************** entete communiques*********************
                float[] colwidthtablComm = { 0.5f, 2f, 2f, 1f, 1f};
                        PdfPTable tableComm = new PdfPTable(colwidthtablComm);
                        tableComm.WidthPercentage = 95;
                        if (communiqs != null)
                        {
                            // titre PV
                            PdfPTable tableTitrComm = new PdfPTable(1);

                            cell = new PdfPCell(new Phrase("IV.  LISTE DES COMMUNIQUES", _fontStyleTitre));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.Border = Rectangle.NO_BORDER;
                    //cell.BackgroundColor = bgcolor;
                            tableTitrComm.AddCell(cell);
                            _pdoc.Add(tableTitrComm);
                            // ENTETE TABLEAU CRP

                            cell = new PdfPCell(new Phrase("N°", _fontStyleInfo));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.BackgroundColor = bgcolor;
                            tableComm.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Numero", _fontStyleInfo));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.BackgroundColor = bgcolor;
                            tableComm.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Objet", _fontStyleInfo));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.BackgroundColor = bgcolor;
                            tableComm.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Date signature", _fontStyleInfo));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.BackgroundColor = bgcolor;
                            tableComm.AddCell(cell);

                            cell = new PdfPCell(new Phrase("Numérisé ?", _fontStyleInfo));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.BackgroundColor = bgcolor;
                            tableComm.AddCell(cell);
                            //fin entete
                            int i = 0;
                            foreach (var a in communiqs)
                            {
                                i++;
                                cell = new PdfPCell(new Phrase("" + i, _fontStyleInfolIatlic));
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                //cell.BackgroundColor = bgcolor;
                                tableComm.AddCell(cell);
                                cell = new PdfPCell(new Phrase("" + a.Numero, _fontStyleInfolIatlic));
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                //cell.BackgroundColor = bgcolor;
                                tableComm.AddCell(cell);
                                cell = new PdfPCell(new Phrase("" + a.Objet, _fontStyleInfolIatlic));
                                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                //cell.BackgroundColor = bgcolor;
                                tableComm.AddCell(cell);
                                cell = new PdfPCell(new Phrase("" + a.DateSign, _fontStyleInfolIatlic));
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                //cell.BackgroundColor = bgcolor;
                                tableComm.AddCell(cell);
                                if (a.Status == 1)
                                {
                                    cell = new PdfPCell(new Phrase("Oui", _fontStyleInfolIatlic));
                                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                    //cell.BackgroundColor = bgcolor;
                                    tableComm.AddCell(cell);
                                }
                                else
                                {
                                    cell = new PdfPCell(new Phrase("Non", _fontStyleInfolIatlic));
                                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                    //cell.BackgroundColor = bgcolor;
                                    tableComm.AddCell(cell);
                                }
                            }
                            _pdoc.Add(tableComm);
                        }
                //***************************************************************
                //*************************** entete attestations*********************
                float[] colwidthtableAttest = { 0.5f, 2f, 1f, 1f, 1f, 1f};
                    PdfPTable tableAttest = new PdfPTable(colwidthtableAttest);
                    tableAttest.WidthPercentage = 95;
                    if (atts != null)
                    {
                        // titre PV
                        PdfPTable tableTitreAttest = new PdfPTable(1);

                        cell = new PdfPCell(new Phrase("V.  LISTE DES ATTESTATIONS", _fontStyleTitre));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = Rectangle.NO_BORDER;
                    //cell.BackgroundColor = bgcolor;
                        tableTitreAttest.AddCell(cell);
                        _pdoc.Add(tableTitreAttest);
                        // ENTETE TABLEAU CRP

                        cell = new PdfPCell(new Phrase("N°", _fontStyleInfo));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.BackgroundColor = bgcolor;
                        tableAttest.AddCell(cell);
                        cell = new PdfPCell(new Phrase("Numero", _fontStyleInfo));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.BackgroundColor = bgcolor;
                        tableAttest.AddCell(cell);
                        cell = new PdfPCell(new Phrase("Type", _fontStyleInfo));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.BackgroundColor = bgcolor;
                        tableAttest.AddCell(cell);
                        cell = new PdfPCell(new Phrase("Béneficiaire", _fontStyleInfo));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.BackgroundColor = bgcolor;
                        tableAttest.AddCell(cell);
                        cell = new PdfPCell(new Phrase("Date", _fontStyleInfo));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.BackgroundColor = bgcolor;
                        tableAttest.AddCell(cell);
                        cell = new PdfPCell(new Phrase("Numérisé ?", _fontStyleInfo));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.BackgroundColor = bgcolor;
                        tableAttest.AddCell(cell);
                        //fin entete
                        int i = 0;
                        foreach (var pv in atts)
                        {
                            i++;
                            cell = new PdfPCell(new Phrase("" + i, _fontStyleInfolIatlic));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            //cell.BackgroundColor = bgcolor;
                            tableAttest.AddCell(cell);
                            cell = new PdfPCell(new Phrase("" + pv.Numero, _fontStyleInfolIatlic));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            //cell.BackgroundColor = bgcolor;
                            tableAttest.AddCell(cell);
                            cell = new PdfPCell(new Phrase("" + pv.Type, _fontStyleInfolIatlic));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            //cell.BackgroundColor = bgcolor;
                            tableAttest.AddCell(cell);
                            cell = new PdfPCell(new Phrase("" + pv.Destinataire, _fontStyleInfolIatlic));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            //cell.BackgroundColor = bgcolor;
                            tableAttest.AddCell(cell);
                            cell = new PdfPCell(new Phrase("" + pv.DateSign, _fontStyleInfolIatlic));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            //cell.BackgroundColor = bgcolor;
                            tableAttest.AddCell(cell);
                            if (pv.Status == 1)
                            {
                                cell = new PdfPCell(new Phrase("Oui", _fontStyleInfolIatlic));
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                //cell.BackgroundColor = bgcolor;
                                tableAttest.AddCell(cell);
                            }
                            else
                            {
                                cell = new PdfPCell(new Phrase("Non", _fontStyleInfolIatlic));
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                //cell.BackgroundColor = bgcolor;
                                tableAttest.AddCell(cell);
                            }
                        }
                        _pdoc.Add(tableAttest);

                    }
                //***************************************************************
                //*************************** entete arrretes*********************
                float[] colwidthtableArr = { 0.5f, 2f, 2f, 1f, 1f };
                    PdfPTable tableArrt = new PdfPTable(colwidthtableArr);
                    tableArrt.WidthPercentage = 95;
                    if (arrets != null)
                    {
                        // titre PV
                        PdfPTable tableTitreArret = new PdfPTable(1);

                        cell = new PdfPCell(new Phrase("VI.  LISTE DES ARRETES", _fontStyleTitre));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = Rectangle.NO_BORDER;
                        //cell.BackgroundColor = bgcolor;
                        tableTitreArret.AddCell(cell);
                        _pdoc.Add(tableTitreArret);
                        // ENTETE TABLEAU CRP

                        cell = new PdfPCell(new Phrase("N°", _fontStyleInfo));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.BackgroundColor = bgcolor;
                        tableArrt.AddCell(cell);
                        cell = new PdfPCell(new Phrase("Numero", _fontStyleInfo));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.BackgroundColor = bgcolor;
                        tableArrt.AddCell(cell);
                        cell = new PdfPCell(new Phrase("Objet", _fontStyleInfo));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.BackgroundColor = bgcolor;
                        tableArrt.AddCell(cell);
                        cell = new PdfPCell(new Phrase("Date", _fontStyleInfo));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.BackgroundColor = bgcolor;
                        tableArrt.AddCell(cell);
                        cell = new PdfPCell(new Phrase("Numérisé ?", _fontStyleInfo));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.BackgroundColor = bgcolor;
                        tableArrt.AddCell(cell);
                        //fin entete
                        int i = 0;
                        foreach (var pp in arrets)
                        {
                            i++;
                            cell = new PdfPCell(new Phrase("" + i, _fontStyleInfolIatlic));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            //cell.BackgroundColor = bgcolor;
                            tableArrt.AddCell(cell);
                            cell = new PdfPCell(new Phrase("" + pp.Numero, _fontStyleInfolIatlic));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            //cell.BackgroundColor = bgcolor;
                            tableArrt.AddCell(cell);
                            cell = new PdfPCell(new Phrase("" + pp.Objet, _fontStyleInfolIatlic));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            //cell.BackgroundColor = bgcolor;
                            tableArrt.AddCell(cell);
                            cell = new PdfPCell(new Phrase("" + pp.DateSign, _fontStyleInfolIatlic));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            //cell.BackgroundColor = bgcolor;
                            tableArrt.AddCell(cell);
                            
                            if (pp.Status == 1)
                            {
                                cell = new PdfPCell(new Phrase("Oui", _fontStyleInfolIatlic));
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                //cell.BackgroundColor = bgcolor;
                                tableArrt.AddCell(cell);
                            }
                            else
                            {
                                cell = new PdfPCell(new Phrase("Non", _fontStyleInfolIatlic));
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                //cell.BackgroundColor = bgcolor;
                                tableArrt.AddCell(cell);
                            }
                        }
                        _pdoc.Add(tableArrt);

                    }
                //***************************************************************
                //*************************** entete decrets*********************
                float[] colwidthtableDecrt = { 0.5f, 2f, 2f, 1f, 1f };
                PdfPTable tableDecret = new PdfPTable(colwidthtableDecrt);
                tableDecret.WidthPercentage = 95;
                if (decrts != null)
                {
                    // titre PV
                    PdfPTable tableTitreDecrt = new PdfPTable(1);

                    cell = new PdfPCell(new Phrase("VII.  LISTE DES DECRETS", _fontStyleTitre));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = Rectangle.NO_BORDER;
                    //cell.BackgroundColor = bgcolor;
                    tableTitreDecrt.AddCell(cell);
                    _pdoc.Add(tableTitreDecrt);
                    // ENTETE TABLEAU CRP

                    cell = new PdfPCell(new Phrase("N°", _fontStyleInfo));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.BackgroundColor = bgcolor;
                    tableDecret.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Numero", _fontStyleInfo));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.BackgroundColor = bgcolor;
                    tableDecret.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Objet", _fontStyleInfo));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.BackgroundColor = bgcolor;
                    tableDecret.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Date", _fontStyleInfo));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.BackgroundColor = bgcolor;
                    tableDecret.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Numérisé ?", _fontStyleInfo));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.BackgroundColor = bgcolor;
                    tableArrt.AddCell(cell);
                    //fin entete
                    int i = 0;
                    foreach (var pp in decrts)
                    {
                        i++;
                        cell = new PdfPCell(new Phrase("" + i, _fontStyleInfolIatlic));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        //cell.BackgroundColor = bgcolor;
                        tableDecret.AddCell(cell);
                        cell = new PdfPCell(new Phrase("" + pp.Numero, _fontStyleInfolIatlic));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        //cell.BackgroundColor = bgcolor;
                        tableDecret.AddCell(cell);
                        cell = new PdfPCell(new Phrase("" + pp.Objet, _fontStyleInfolIatlic));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        //cell.BackgroundColor = bgcolor;
                        tableDecret.AddCell(cell);
                        cell = new PdfPCell(new Phrase("" + pp.DateSign, _fontStyleInfolIatlic));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        //cell.BackgroundColor = bgcolor;
                        tableDecret.AddCell(cell);

                        if (pp.Status == 1)
                        {
                            cell = new PdfPCell(new Phrase("Oui", _fontStyleInfolIatlic));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            //cell.BackgroundColor = bgcolor;
                            tableDecret.AddCell(cell);
                        }
                        else
                        {
                            cell = new PdfPCell(new Phrase("Non", _fontStyleInfolIatlic));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            //cell.BackgroundColor = bgcolor;
                            tableDecret.AddCell(cell);
                        }
                    }
                    _pdoc.Add(tableArrt);

                }
                //***************************************************************
                //*************************** entete etat paiemnet*********************
                float[] colwidthtableEtatP = { 0.5f, 2f, 2f, 1f, 1f };
                PdfPTable tableEtatP = new PdfPTable(colwidthtableEtatP);
                tableEtatP.WidthPercentage = 95;
                if (etps != null)
                {
                    // titre PV
                    PdfPTable tableTitreEtatP = new PdfPTable(1);

                    cell = new PdfPCell(new Phrase("VIII.  LISTE DES ETATS DE PAIEMENTS", _fontStyleTitre));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = Rectangle.NO_BORDER;
                    //cell.BackgroundColor = bgcolor;
                    tableTitreEtatP.AddCell(cell);
                    _pdoc.Add(tableTitreEtatP);
                    // ENTETE TABLEAU CRP

                    cell = new PdfPCell(new Phrase("N°", _fontStyleInfo));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.BackgroundColor = bgcolor;
                    tableEtatP.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Numero", _fontStyleInfo));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.BackgroundColor = bgcolor;
                    tableEtatP.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Objet", _fontStyleInfo));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.BackgroundColor = bgcolor;
                    tableEtatP.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Date", _fontStyleInfo));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.BackgroundColor = bgcolor;
                    tableEtatP.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Numérisé ?", _fontStyleInfo));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.BackgroundColor = bgcolor;
                    tableEtatP.AddCell(cell);
                    //fin entete
                    int i = 0;
                    foreach (var pp in etps)
                    {
                        i++;
                        cell = new PdfPCell(new Phrase("" + i, _fontStyleInfolIatlic));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        //cell.BackgroundColor = bgcolor;
                        tableEtatP.AddCell(cell);
                        cell = new PdfPCell(new Phrase("" + pp.Numero, _fontStyleInfolIatlic));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        //cell.BackgroundColor = bgcolor;
                        tableEtatP.AddCell(cell);
                        cell = new PdfPCell(new Phrase("" + pp.Objet, _fontStyleInfolIatlic));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        //cell.BackgroundColor = bgcolor;
                        tableEtatP.AddCell(cell);
                        cell = new PdfPCell(new Phrase("" + pp.DateSign, _fontStyleInfolIatlic));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        //cell.BackgroundColor = bgcolor;
                        tableEtatP.AddCell(cell);

                        if (pp.Status == 1)
                        {
                            cell = new PdfPCell(new Phrase("Oui", _fontStyleInfolIatlic));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            //cell.BackgroundColor = bgcolor;
                            tableEtatP.AddCell(cell);
                        }
                        else
                        {
                            cell = new PdfPCell(new Phrase("Non", _fontStyleInfolIatlic));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            //cell.BackgroundColor = bgcolor;
                            tableEtatP.AddCell(cell);
                        }
                    }
                    _pdoc.Add(tableEtatP);

                }
                //***************************************************************
                //*************************** entete certificats*********************
                float[] colwidthtableCertf = { 0.5f, 2f, 1f, 1f, 1f, 1f };
                PdfPTable tableCrtf = new PdfPTable(colwidthtableCertf);
                tableCrtf.WidthPercentage = 95;
                if (crts != null)
                {
                    // titre PV
                    PdfPTable tableTitreCrtf = new PdfPTable(1);

                    cell = new PdfPCell(new Phrase("IX.  LISTE DES CERTIFICATS", _fontStyleTitre));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = Rectangle.NO_BORDER;
                    //cell.BackgroundColor = bgcolor;
                    tableTitreCrtf.AddCell(cell);
                    _pdoc.Add(tableTitreCrtf);
                    // ENTETE TABLEAU CRP

                    cell = new PdfPCell(new Phrase("N°", _fontStyleInfo));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.BackgroundColor = bgcolor;
                    tableCrtf.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Numero", _fontStyleInfo));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.BackgroundColor = bgcolor;
                    tableCrtf.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Type", _fontStyleInfo));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.BackgroundColor = bgcolor;
                    tableCrtf.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Béneficiaire", _fontStyleInfo));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.BackgroundColor = bgcolor;
                    tableCrtf.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Date", _fontStyleInfo));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.BackgroundColor = bgcolor;
                    tableCrtf.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Numérisé ?", _fontStyleInfo));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.BackgroundColor = bgcolor;
                    tableCrtf.AddCell(cell);
                    //fin entete
                    int i = 0;
                    foreach (var pv in crts)
                    {
                        i++;
                        cell = new PdfPCell(new Phrase("" + i, _fontStyleInfolIatlic));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        //cell.BackgroundColor = bgcolor;
                        tableCrtf.AddCell(cell);
                        cell = new PdfPCell(new Phrase("" + pv.Numero, _fontStyleInfolIatlic));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        //cell.BackgroundColor = bgcolor;
                        tableCrtf.AddCell(cell);
                        cell = new PdfPCell(new Phrase("" + pv.Type, _fontStyleInfolIatlic));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        //cell.BackgroundColor = bgcolor;
                        tableCrtf.AddCell(cell);
                        cell = new PdfPCell(new Phrase("" + pv.Destinataire, _fontStyleInfolIatlic));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        //cell.BackgroundColor = bgcolor;
                        tableCrtf.AddCell(cell);
                        cell = new PdfPCell(new Phrase("" + pv.DateSign, _fontStyleInfolIatlic));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        //cell.BackgroundColor = bgcolor;
                        tableCrtf.AddCell(cell);
                        if (pv.Status == 1)
                        {
                            cell = new PdfPCell(new Phrase("Oui", _fontStyleInfolIatlic));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            //cell.BackgroundColor = bgcolor;
                            tableCrtf.AddCell(cell);
                        }
                        else
                        {
                            cell = new PdfPCell(new Phrase("Non", _fontStyleInfolIatlic));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            //cell.BackgroundColor = bgcolor;
                            tableCrtf.AddCell(cell);
                        }
                    }
                    _pdoc.Add(tableCrtf);

                }
                //***************************************************************
                //*************************** entete note de service*********************
                float[] colwidthtableNts = { 0.5f, 2f, 2f, 1f, 1f };
                PdfPTable tableNts = new PdfPTable(colwidthtableNts);
                tableNts.WidthPercentage = 95;
                if (nts != null)
                {
                    // titre PV
                    PdfPTable tableTitreNts = new PdfPTable(1);

                    cell = new PdfPCell(new Phrase("X.  LISTE DES NOTES DE SERVICES", _fontStyleTitre));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = Rectangle.NO_BORDER;
                    //cell.BackgroundColor = bgcolor;
                    tableTitreNts.AddCell(cell);
                    _pdoc.Add(tableTitreNts);
                    // ENTETE TABLEAU CRP

                    cell = new PdfPCell(new Phrase("N°", _fontStyleInfo));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.BackgroundColor = bgcolor;
                    tableNts.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Numero", _fontStyleInfo));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.BackgroundColor = bgcolor;
                    tableNts.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Objet", _fontStyleInfo));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.BackgroundColor = bgcolor;
                    tableNts.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Date", _fontStyleInfo));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.BackgroundColor = bgcolor;
                    tableNts.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Numérisé ?", _fontStyleInfo));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.BackgroundColor = bgcolor;
                    tableNts.AddCell(cell);
                    //fin entete
                    int i = 0;
                    foreach (var pp in nts)
                    {
                        i++;
                        cell = new PdfPCell(new Phrase("" + i, _fontStyleInfolIatlic));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        //cell.BackgroundColor = bgcolor;
                        tableNts.AddCell(cell);
                        cell = new PdfPCell(new Phrase("" + pp.Numero, _fontStyleInfolIatlic));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        //cell.BackgroundColor = bgcolor;
                        tableNts.AddCell(cell);
                        cell = new PdfPCell(new Phrase("" + pp.Objet, _fontStyleInfolIatlic));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        //cell.BackgroundColor = bgcolor;
                        tableNts.AddCell(cell);
                        cell = new PdfPCell(new Phrase("" + pp.DateSign, _fontStyleInfolIatlic));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        //cell.BackgroundColor = bgcolor;
                        tableNts.AddCell(cell);

                        if (pp.Status == 1)
                        {
                            cell = new PdfPCell(new Phrase("Oui", _fontStyleInfolIatlic));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            //cell.BackgroundColor = bgcolor;
                            tableNts.AddCell(cell);
                        }
                        else
                        {
                            cell = new PdfPCell(new Phrase("Non", _fontStyleInfolIatlic));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            //cell.BackgroundColor = bgcolor;
                            tableNts.AddCell(cell);
                        }
                    }
                    _pdoc.Add(tableNts);

                }
                
            }
            _pdoc.Close();
            return _memoryStream.ToArray();
        }
        
    }
}
