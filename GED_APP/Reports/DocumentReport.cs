using GED_APP.Models;
using GED_APP.Repository.Interfaces;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Newtonsoft.Json;

namespace GED_APP.Reports
{
    public class DocumentReport
    {
        //Uri baseAdress = new Uri("http://localhost:5249/api/v1");
        //private readonly HttpClient _httpClient;
        private readonly IDocument _docRepo;

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

        Font _fontStyleTitre = new Font(Font.FontFamily.HELVETICA, 15f, Font.BOLD);
        Font _fontStyleSousTitreSmall = new Font(Font.FontFamily.HELVETICA, 10f, Font.BOLD);
        PdfPTable _tableau = new PdfPTable(3);
        MemoryStream _memoryStream = new MemoryStream();
        public DocumentReport(IDocument docRepo)
        {
            _docRepo = docRepo;
        }
        public byte[] prepareReportDocByYear(string annee)
        {
            // iTextSharp.text.Image logoM=default(iTextSharp.text.Image);
            //  logoM = iTextSharp.text.Image.GetInstance("./wwwroot/img/images/logo.png");
            Image logo = Image.GetInstance("./wwwroot/img/images/logo.png");

            _pdoc = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _pdoc.SetPageSize(PageSize.A4);
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

            cell = new PdfPCell(new Phrase("UNIVERSITE DE YAOUNDE I", _fontStyleEntete));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.Border = Rectangle.NO_BORDER;
            tableEntete.AddCell(cell);
            cell = new PdfPCell(new Phrase("THE UNIVERSITY OF YAOUNDE I", _fontStyleEntete));
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

            cell = new PdfPCell(new Phrase("ECOLE NORMALE SUPERIEURE", _fontStyleEntete));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.Border = Rectangle.NO_BORDER;
            tableEntete.AddCell(cell);
            cell = new PdfPCell(new Phrase("HIGHER TEACHERS TRAINING COLLEGE", _fontStyleEntete));
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
            tableEntete.AddCell(cell);
            _pdoc.Add(tableEntete);

            Paragraph p = new Paragraph(new Chunk(" ", _fontStyleSousTitreSmall));
            _pdoc.Add(p);
            // titre Fiche
            PdfPTable tableTitreFiche = new PdfPTable(1);

            cell = new PdfPCell(new Phrase("LISTES DES DOCUMENTS ", _fontStyleTitre));
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
            _pdoc.Add(tableTitreFiche);

            p = new Paragraph(new Chunk(" ", _fontStyleSousTitreSmall));
            _pdoc.Add(p);

            //ICollection<Doc> list = null;
            if (annee != null)
            {
                //HttpResponseMessage res = _httpClient.GetAsync(_httpClient.BaseAddress + "/Documents/years/" + annee).Result;
                //if (res.IsSuccessStatusCode)
                //{
                //    string data = res.Content.ReadAsStringAsync().Result;
                //    list = JsonConvert.DeserializeObject<List<Doc>>(data);
                //}
                //list = _docRepo.GetAllByAnnee(annee);
                //if (list != null)
                //{
                    ICollection<Doc> pvs = _docRepo.GetAllByTypeAnnee("PV", annee); 
                    ICollection<Doc> arretes = _docRepo.GetAllByTypeAnnee("ARR", annee);
                    ICollection<Doc> communiques = _docRepo.GetAllByTypeAnnee("CRP", annee);
                    ICollection<Doc> autres = _docRepo.GetAllByTypeAnnee("AUTRE", annee);
                    if (pvs != null)
                    {
                        // titre PV
                        PdfPTable tableTitreHisto = new PdfPTable(1);

                        cell = new PdfPCell(new Phrase("I.  LES PROCES-VERBAUX", _fontStyleTitre));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = Rectangle.NO_BORDER;
                        //cell.BackgroundColor = bgcolor;
                        tableTitreHisto.AddCell(cell);
                        _pdoc.Add(tableTitreHisto);

                        // ENTETE TABLEAU PVS
                        float[] colwidthtableHistoAs = { 0.5f, 1f, 1f, 1f, 1f, 1f, 1f, 1f };
                        PdfPTable tableHistoAss = new PdfPTable(colwidthtableHistoAs);
                        tableHistoAss.WidthPercentage = 95;
                        cell = new PdfPCell(new Phrase("N°", _fontStyleInfo));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.BackgroundColor = bgcolor;
                        tableHistoAss.AddCell(cell);
                        cell = new PdfPCell(new Phrase("Source", _fontStyleInfo));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.BackgroundColor = bgcolor;
                        tableHistoAss.AddCell(cell);
                        cell = new PdfPCell(new Phrase("Promotion", _fontStyleInfo));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.BackgroundColor = bgcolor;
                        tableHistoAss.AddCell(cell);
                        cell = new PdfPCell(new Phrase("Annee de sortie", _fontStyleInfo));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.BackgroundColor = bgcolor;
                        tableHistoAss.AddCell(cell);
                        cell = new PdfPCell(new Phrase("Session", _fontStyleInfo));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.BackgroundColor = bgcolor;
                        tableHistoAss.AddCell(cell);
                        cell = new PdfPCell(new Phrase("Cycle", _fontStyleInfo));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.BackgroundColor = bgcolor;
                        tableHistoAss.AddCell(cell);
                        cell = new PdfPCell(new Phrase("Filière", _fontStyleInfo));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.BackgroundColor = bgcolor;
                        tableHistoAss.AddCell(cell);
                        cell = new PdfPCell(new Phrase("Numérisé ?", _fontStyleInfo));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.BackgroundColor = bgcolor;
                        tableHistoAss.AddCell(cell);
                        //fin entete

                        int cpt = 0;
                        foreach (var pv in pvs)
                        {
                            cpt++;
                            cell = new PdfPCell(new Phrase("" + cpt, _fontStyleInfolIatlic));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            //cell.BackgroundColor = bgcolor;
                            tableHistoAss.AddCell(cell);
                            cell = new PdfPCell(new Phrase("" + pv.Source, _fontStyleInfolIatlic));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            //cell.BackgroundColor = bgcolor;
                            tableHistoAss.AddCell(cell);
                            cell = new PdfPCell(new Phrase("" + pv.Promotion, _fontStyleInfolIatlic));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            //cell.BackgroundColor = bgcolor;
                            tableHistoAss.AddCell(cell);
                            cell = new PdfPCell(new Phrase("" + pv.AnneeSortie, _fontStyleInfolIatlic));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            //cell.BackgroundColor = bgcolor;
                            tableHistoAss.AddCell(cell);
                            cell = new PdfPCell(new Phrase("" + pv.Session, _fontStyleInfolIatlic));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            //cell.BackgroundColor = bgcolor;
                            tableHistoAss.AddCell(cell);
                            cell = new PdfPCell(new Phrase("" + pv.Cycle.Code, _fontStyleInfolIatlic));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            //cell.BackgroundColor = bgcolor;
                            tableHistoAss.AddCell(cell);
                            cell = new PdfPCell(new Phrase("" + pv.Filiere.Code, _fontStyleInfolIatlic));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            //cell.BackgroundColor = bgcolor;
                            tableHistoAss.AddCell(cell);
                            if (pv.Fichier == 1)
                            {
                                cell = new PdfPCell(new Phrase("Oui", _fontStyleInfolIatlic));
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                //cell.BackgroundColor = bgcolor;
                                tableHistoAss.AddCell(cell);
                            }
                            else
                            {
                                cell = new PdfPCell(new Phrase("Non", _fontStyleInfolIatlic));
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                //cell.BackgroundColor = bgcolor;
                                tableHistoAss.AddCell(cell);
                            }


                        }
                        _pdoc.Add(tableHistoAss);
                        float[] colwidthtableArretes = { 0.5f, 1f, 2f, 1f, 1f, 1f, 1f };
                        PdfPTable tableArretes = new PdfPTable(colwidthtableArretes);
                        tableArretes.WidthPercentage = 95;
                        if (arretes != null)
                        {
                            // titre arretes
                            PdfPTable tableTitreArrete = new PdfPTable(1);

                            cell = new PdfPCell(new Phrase("II.  LES ARRETES", _fontStyleTitre));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.Border = Rectangle.NO_BORDER;
                            //cell.BackgroundColor = bgcolor;
                            tableTitreArrete.AddCell(cell);
                            _pdoc.Add(tableTitreArrete);

                            // ENTETE TABLEAU ARRETES

                            cell = new PdfPCell(new Phrase("N°", _fontStyleInfo));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.BackgroundColor = bgcolor;
                            tableArretes.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Source", _fontStyleInfo));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.BackgroundColor = bgcolor;
                            tableArretes.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Numero", _fontStyleInfo));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.BackgroundColor = bgcolor;
                            tableArretes.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Date signature", _fontStyleInfo));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.BackgroundColor = bgcolor;
                            tableArretes.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Année academique", _fontStyleInfo));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.BackgroundColor = bgcolor;
                            tableArretes.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Cycle", _fontStyleInfo));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.BackgroundColor = bgcolor;
                            tableArretes.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Numérisé ?", _fontStyleInfo));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.BackgroundColor = bgcolor;
                            tableArretes.AddCell(cell);
                            //fin entete
                            int i = 0;
                            foreach (var a in arretes)
                            {
                                i++;
                                cell = new PdfPCell(new Phrase("" + i, _fontStyleInfolIatlic));
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                //cell.BackgroundColor = bgcolor;
                                tableArretes.AddCell(cell);
                                cell = new PdfPCell(new Phrase("" + a.Source, _fontStyleInfolIatlic));
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                //cell.BackgroundColor = bgcolor;
                                tableArretes.AddCell(cell);
                                cell = new PdfPCell(new Phrase("" + a.Numero, _fontStyleInfolIatlic));
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                //cell.BackgroundColor = bgcolor;
                                tableArretes.AddCell(cell);
                                cell = new PdfPCell(new Phrase("" + a.DateSign, _fontStyleInfolIatlic));
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                //cell.BackgroundColor = bgcolor;
                                tableArretes.AddCell(cell);
                                cell = new PdfPCell(new Phrase(" " + a.AnneeAcademique, _fontStyleInfolIatlic));
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                //cell.BackgroundColor = bgcolor;
                                tableArretes.AddCell(cell);
                                cell = new PdfPCell(new Phrase(" " + a.Cycle.Code, _fontStyleInfolIatlic));
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                //cell.BackgroundColor = bgcolor;
                                tableArretes.AddCell(cell);
                                if (a.Fichier == 1)
                                {
                                    cell = new PdfPCell(new Phrase("Oui", _fontStyleInfolIatlic));
                                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                    //cell.BackgroundColor = bgcolor;
                                    tableArretes.AddCell(cell);
                                }
                                else
                                {
                                    cell = new PdfPCell(new Phrase("Non", _fontStyleInfolIatlic));
                                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                    //cell.BackgroundColor = bgcolor;
                                    tableArretes.AddCell(cell);
                                }
                            }
                            _pdoc.Add(tableArretes);

                        }
                        float[] colwidthtableCRP = { 0.5f, 1f, 2f, 1f, 1f, 1f, 1f, 1f };
                        PdfPTable tableCRP = new PdfPTable(colwidthtableCRP);
                        tableCRP.WidthPercentage = 95;
                        if (communiques != null)
                        {
                            // titre PV
                            PdfPTable tableTitreCommuniques = new PdfPTable(1);

                            cell = new PdfPCell(new Phrase("III.  LES COMMUNIQUES", _fontStyleTitre));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.Border = Rectangle.NO_BORDER;
                            //cell.BackgroundColor = bgcolor;
                            tableTitreCommuniques.AddCell(cell);
                            _pdoc.Add(tableTitreCommuniques);
                            // ENTETE TABLEAU CRP

                            cell = new PdfPCell(new Phrase("N°", _fontStyleInfo));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.BackgroundColor = bgcolor;
                            tableCRP.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Source", _fontStyleInfo));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.BackgroundColor = bgcolor;
                            tableCRP.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Numero", _fontStyleInfo));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.BackgroundColor = bgcolor;
                            tableCRP.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Date signature", _fontStyleInfo));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.BackgroundColor = bgcolor;
                            tableCRP.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Session", _fontStyleInfo));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.BackgroundColor = bgcolor;
                            tableCRP.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Année academique", _fontStyleInfo));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.BackgroundColor = bgcolor;
                            tableCRP.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Cycle", _fontStyleInfo));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.BackgroundColor = bgcolor;
                            tableCRP.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Numérisé ?", _fontStyleInfo));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.BackgroundColor = bgcolor;
                            tableCRP.AddCell(cell);
                            //fin entete
                            int i = 0;
                            foreach (var c in communiques)
                            {
                                i++;
                                cell = new PdfPCell(new Phrase("" + i, _fontStyleInfolIatlic));
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                //cell.BackgroundColor = bgcolor;
                                tableCRP.AddCell(cell);
                                cell = new PdfPCell(new Phrase("" + c.Source, _fontStyleInfolIatlic));
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                //cell.BackgroundColor = bgcolor;
                                tableCRP.AddCell(cell);
                                cell = new PdfPCell(new Phrase("" + c.Numero, _fontStyleInfolIatlic));
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                //cell.BackgroundColor = bgcolor;
                                tableCRP.AddCell(cell);
                                cell = new PdfPCell(new Phrase("" + c.DateSign, _fontStyleInfolIatlic));
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                //cell.BackgroundColor = bgcolor;
                                tableCRP.AddCell(cell);
                                cell = new PdfPCell(new Phrase("" + c.Session, _fontStyleInfolIatlic));
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                //cell.BackgroundColor = bgcolor;
                                tableCRP.AddCell(cell);
                                cell = new PdfPCell(new Phrase(" " + c.AnneeAcademique, _fontStyleInfolIatlic));
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                //cell.BackgroundColor = bgcolor;
                                tableCRP.AddCell(cell);
                                cell = new PdfPCell(new Phrase(" " + c.Cycle.Code, _fontStyleInfolIatlic));
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                //cell.BackgroundColor = bgcolor;
                                tableCRP.AddCell(cell);
                                if (c.Fichier == 1)
                                {
                                    cell = new PdfPCell(new Phrase("Oui", _fontStyleInfolIatlic));
                                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                    //cell.BackgroundColor = bgcolor;
                                    tableCRP.AddCell(cell);
                                }
                                else
                                {
                                    cell = new PdfPCell(new Phrase("Non", _fontStyleInfolIatlic));
                                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                    //cell.BackgroundColor = bgcolor;
                                    tableCRP.AddCell(cell);
                                }
                            }
                            _pdoc.Add(tableCRP);

                        }
                        float[] colwidthtableAutres = { 0.5f, 1f, 2f, 1f, 1f, 1f };
                        PdfPTable tableAutre = new PdfPTable(colwidthtableAutres);
                        tableAutre.WidthPercentage = 95;
                        if (autres != null)
                        {
                            // titre PV
                            PdfPTable tableTitreAutres = new PdfPTable(1);

                            cell = new PdfPCell(new Phrase("IV.  AUTRES", _fontStyleTitre));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.Border = Rectangle.NO_BORDER;
                            //cell.BackgroundColor = bgcolor;
                            tableTitreAutres.AddCell(cell);
                            _pdoc.Add(tableTitreAutres);
                            // ENTETE TABLEAU CRP

                            cell = new PdfPCell(new Phrase("N°", _fontStyleInfo));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.BackgroundColor = bgcolor;
                            tableAutre.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Source", _fontStyleInfo));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.BackgroundColor = bgcolor;
                            tableAutre.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Numero", _fontStyleInfo));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.BackgroundColor = bgcolor;
                            tableAutre.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Date signature", _fontStyleInfo));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.BackgroundColor = bgcolor;
                            tableAutre.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Objet", _fontStyleInfo));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.BackgroundColor = bgcolor;
                            tableAutre.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Numérisé ?", _fontStyleInfo));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.BackgroundColor = bgcolor;
                            tableAutre.AddCell(cell);
                            //fin entete
                            int i = 0;
                            foreach (var autr in autres)
                            {
                                i++;
                                cell = new PdfPCell(new Phrase("" + i, _fontStyleInfolIatlic));
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                //cell.BackgroundColor = bgcolor;
                                tableAutre.AddCell(cell);
                                cell = new PdfPCell(new Phrase("" + autr.Source, _fontStyleInfolIatlic));
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                //cell.BackgroundColor = bgcolor;
                                tableAutre.AddCell(cell);
                                cell = new PdfPCell(new Phrase("" + autr.Numero, _fontStyleInfolIatlic));
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                //cell.BackgroundColor = bgcolor;
                                tableAutre.AddCell(cell);
                                cell = new PdfPCell(new Phrase("" + autr.DateSign, _fontStyleInfolIatlic));
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                //cell.BackgroundColor = bgcolor;
                                tableAutre.AddCell(cell);
                                cell = new PdfPCell(new Phrase("" + autr.Objet, _fontStyleInfolIatlic));
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                //cell.BackgroundColor = bgcolor;
                                tableAutre.AddCell(cell);
                                if (autr.Fichier == 1)
                                {
                                    cell = new PdfPCell(new Phrase("Oui", _fontStyleInfolIatlic));
                                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                    //cell.BackgroundColor = bgcolor;
                                    tableAutre.AddCell(cell);
                                }
                                else
                                {
                                    cell = new PdfPCell(new Phrase("Non", _fontStyleInfolIatlic));
                                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                    //cell.BackgroundColor = bgcolor;
                                    tableAutre.AddCell(cell);
                                }
                            }
                            _pdoc.Add(tableAutre);
                        }
                    }
            }
            
            _pdoc.Close();
            return _memoryStream.ToArray();
        }
    }
}
