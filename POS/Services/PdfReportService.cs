using NLog;
using Pechkin;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using Dal.Model;

namespace Pos.Services
{
    public class PdfReportService
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public string FilePath { get; private set; }

        public string MailTo { get; set; }

        public string MailFromName { get; set; }

        public string MailFromAddress { get; set; }

        public string MailServer { get; set; }

        public void GenerateCheckoutSheet(CheckoutSheet sheet)
        {
            try
            {
                string input = File.ReadAllText(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\Templates\\Html\\Template_Checkoutsheet.htm");
                DateTime dateTime = sheet.OpenTime ?? DateTime.Now;
                string str1 = "Kasblad JH Tjok Hove: " + dateTime.ToShortDateString();
                this.FilePath = Path.GetTempPath() + ("Kasblad JH Tjok Hove " + dateTime.ToString("dd_MM_yyyy HH_mm")) + ".pdf";
                List<IGrouping<Product, TicketLine>> list = sheet.Tickets.SelectMany<Ticket, TicketLine>((Func<Ticket, IEnumerable<TicketLine>>)(t => (IEnumerable<TicketLine>)t.TicketLines)).GroupBy<TicketLine, Product>((Func<TicketLine, Product>)(tl => tl.Product)).ToList<IGrouping<Product, TicketLine>>().OrderBy<IGrouping<Product, TicketLine>, string>((Func<IGrouping<Product, TicketLine>, string>)(pl => pl.Key.Name)).ToList<IGrouping<Product, TicketLine>>();
                StringBuilder stringBuilder = new StringBuilder();
                int num1 = 1;
                foreach (IGrouping<Product, TicketLine> source in list)
                {
                    stringBuilder.AppendLine(num1 % 2 > 0 ? "<tr>" : "<tr class=\"alt\">");
                    stringBuilder.AppendLine("<td>" + source.Key.Name + "</td>");
                    stringBuilder.AppendLine("<td>" + (object)source.Sum<TicketLine>((Func<TicketLine, int>)(l => (int)l.Amount)) + "</td>");
                    stringBuilder.AppendLine("</tr>");
                    ++num1;
                }
                Dictionary<string, string> formFields = new Dictionary<string, string>((IEqualityComparer<string>)StringComparer.OrdinalIgnoreCase);
                formFields.Add("Title", str1);
                Dictionary<string, string> dictionary1 = formFields;
                DateTime? nullable = sheet.OpenTime;
                string str2 = nullable.ToString();
                dictionary1.Add("OpeningTime", str2);
                Dictionary<string, string> dictionary2 = formFields;
                nullable = sheet.CloseTime;
                string str3 = nullable.ToString();
                dictionary2.Add("ClosureTime", str3);
                formFields.Add("ClosedBy", sheet.ClosedBy.Fullname);
                formFields.Add("OpenedBy", sheet.OpenedBy.Fullname);
                Dictionary<string, string> dictionary3 = formFields;
                int num2 = sheet.CloseEur500;
                string str4 = num2.ToString();
                dictionary3.Add("AmountEur500", str4);
                Dictionary<string, string> dictionary4 = formFields;
                num2 = sheet.CloseEur200;
                string str5 = num2.ToString();
                dictionary4.Add("AmountEur200", str5);
                Dictionary<string, string> dictionary5 = formFields;
                num2 = sheet.CloseEur100;
                string str6 = num2.ToString();
                dictionary5.Add("AmountEur100", str6);
                Dictionary<string, string> dictionary6 = formFields;
                num2 = sheet.CloseEur50;
                string str7 = num2.ToString();
                dictionary6.Add("AmountEur50", str7);
                formFields.Add("AmountEur20", sheet.CloseEur20.ToString());
                formFields.Add("AmountEur10", sheet.CloseEur10.ToString());
                Dictionary<string, string> dictionary7 = formFields;
                int num3 = sheet.CloseEur5;
                string str8 = num3.ToString();
                dictionary7.Add("AmountEur5", str8);
                Dictionary<string, string> dictionary8 = formFields;
                num3 = sheet.CloseEur2;
                string str9 = num3.ToString();
                dictionary8.Add("AmountEur2", str9);
                formFields.Add("AmountEur1", sheet.CloseEur1.ToString());
                Dictionary<string, string> dictionary9 = formFields;
                int num4 = sheet.CloseEur50c;
                string str10 = num4.ToString();
                dictionary9.Add("AmountEur50c", str10);
                Dictionary<string, string> dictionary10 = formFields;
                num4 = sheet.CloseEur20c;
                string str11 = num4.ToString();
                dictionary10.Add("AmountEur20c", str11);
                Dictionary<string, string> dictionary11 = formFields;
                num4 = sheet.CloseEur10c;
                string str12 = num4.ToString();
                dictionary11.Add("AmountEur10c", str12);
                Dictionary<string, string> dictionary12 = formFields;
                num4 = sheet.CloseEur5c;
                string str13 = num4.ToString();
                dictionary12.Add("AmountEur5c", str13);
                Dictionary<string, string> dictionary13 = formFields;
                num4 = sheet.CloseEur2c;
                string str14 = num4.ToString();
                dictionary13.Add("AmountEur2c", str14);
                Dictionary<string, string> dictionary14 = formFields;
                num4 = sheet.CloseEur1c;
                string str15 = num4.ToString();
                dictionary14.Add("AmountEur1c", str15);
                Dictionary<string, string> dictionary15 = formFields;
                num4 = sheet.CloseEur500 * 500;
                string str16 = num4.ToString("C");
                dictionary15.Add("SubtotalEur500", str16);
                Dictionary<string, string> dictionary16 = formFields;
                num4 = sheet.CloseEur200 * 200;
                string str17 = num4.ToString("C");
                dictionary16.Add("SubtotalEur200", str17);
                Dictionary<string, string> dictionary17 = formFields;
                num4 = sheet.CloseEur100 * 100;
                string str18 = num4.ToString("C");
                dictionary17.Add("SubtotalEur100", str18);
                Dictionary<string, string> dictionary18 = formFields;
                num4 = sheet.CloseEur50 * 50;
                string str19 = num4.ToString("C");
                dictionary18.Add("SubtotalEur50", str19);
                Dictionary<string, string> dictionary19 = formFields;
                num4 = sheet.CloseEur20 * 20;
                string str20 = num4.ToString("C");
                dictionary19.Add("SubtotalEur20", str20);
                Dictionary<string, string> dictionary20 = formFields;
                num4 = sheet.CloseEur10 * 10;
                string str21 = num4.ToString("C");
                dictionary20.Add("SubtotalEur10", str21);
                Dictionary<string, string> dictionary21 = formFields;
                num4 = sheet.CloseEur5 * 5;
                string str22 = num4.ToString("C");
                dictionary21.Add("SubtotalEur5", str22);
                Dictionary<string, string> dictionary22 = formFields;
                num4 = sheet.CloseEur2 * 2;
                string str23 = num4.ToString("C");
                dictionary22.Add("SubtotalEur2", str23);
                Dictionary<string, string> dictionary23 = formFields;
                num4 = sheet.CloseEur1 * 1;
                string str24 = num4.ToString("C");
                dictionary23.Add("SubtotalEur1", str24);
                Dictionary<string, string> dictionary24 = formFields;
                double num5 = (double)sheet.CloseEur50c * 0.5;
                string str25 = num5.ToString("C");
                dictionary24.Add("SubtotalEur50c", str25);
                Dictionary<string, string> dictionary25 = formFields;
                num5 = (double)sheet.CloseEur20c * 0.2;
                string str26 = num5.ToString("C");
                dictionary25.Add("SubtotalEur20c", str26);
                Dictionary<string, string> dictionary26 = formFields;
                num5 = (double)sheet.CloseEur10c * 0.1;
                string str27 = num5.ToString("C");
                dictionary26.Add("SubtotalEur10c", str27);
                Dictionary<string, string> dictionary27 = formFields;
                num5 = (double)sheet.CloseEur5c * 0.05;
                string str28 = num5.ToString("C");
                dictionary27.Add("SubtotalEur5c", str28);
                Dictionary<string, string> dictionary28 = formFields;
                num5 = (double)sheet.CloseEur2c * 0.02;
                string str29 = num5.ToString("C");
                dictionary28.Add("SubtotalEur2c", str29);
                Dictionary<string, string> dictionary29 = formFields;
                num5 = (double)sheet.CloseEur1c * 0.01;
                string str30 = num5.ToString("C");
                dictionary29.Add("SubtotalEur1c", str30);
                Dictionary<string, string> dictionary30 = formFields;
                num5 = sheet.CloseAmount;
                string str31 = num5.ToString("C");
                dictionary30.Add("EndTotal", str31);
                Dictionary<string, string> dictionary31 = formFields;
                num5 = sheet.OpenAmount;
                string str32 = num5.ToString("C");
                dictionary31.Add("BeginTotal", str32);
                Dictionary<string, string> dictionary32 = formFields;
                num5 = sheet.CloseAmount - sheet.OpenAmount;
                string str33 = num5.ToString("C");
                dictionary32.Add("Revenue", str33);
                formFields.Add("Remarks", sheet.Comments);
                formFields.Add("ImagePath", Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\Templates\\Html");
                formFields.Add("TableSoldProducts", stringBuilder.ToString());
                string str34 = new Regex("\\[(\\w+)\\]", RegexOptions.Compiled).Replace(input, (MatchEvaluator)(match => formFields[match.Groups[1].Value]));
                GlobalConfig config = new GlobalConfig();
                config.SetMargins(new Margins(70, 45, 70, 45)).SetPaperSize(PaperKind.A4);
                SimplePechkin simplePechkin = new SimplePechkin(config);
                ObjectConfig objectConfig = new ObjectConfig();
                objectConfig.SetLoadImages(true);
                objectConfig.SetPrintBackground(true);
                objectConfig.SetZoomFactor(1.1);
                objectConfig.SetAllowLocalContent(true);
                ObjectConfig doc = objectConfig;
                string html = str34;
                File.WriteAllBytes(this.FilePath, simplePechkin.Convert(doc, html));
                PdfReportService.logger.Info("Checkoutsheet PDF created: " + this.FilePath);
            }
            catch (Exception ex)
            {
                PdfReportService.logger.Error("Could not create temporary PDF file:" + ex.Message);
            }
        }

        public void GenerateCheckoutTicketOverview(CheckoutSheet sheet)
        {
            try
            {
                string input = File.ReadAllText(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\Templates\\Html\\Template_Checkoutsheet_TicketOverview.htm");
                DateTime dateTime = sheet.OpenTime ?? DateTime.Now;
                string str1 = "Ticketoverzicht: " + dateTime.ToShortDateString();
                this.FilePath = Path.GetTempPath() + ("Ticketoverzicht " + dateTime.ToString("dd_MM_yyyy HH_mm")) + ".pdf";
                StringBuilder stringBuilder1 = new StringBuilder();
                foreach (Ticket ticket in (Collection<Ticket>)sheet.Tickets)
                {
                    stringBuilder1.AppendLine("<table class=\"table3\">");
                    stringBuilder1.AppendLine("<tr>");
                    stringBuilder1.AppendLine("<td colspan=\"6\" class=\"ticketTitle\">Ticket &#35;" + (object)ticket.Id + "</td>");
                    stringBuilder1.AppendLine("</tr>");
                    stringBuilder1.AppendLine("<tr>");
                    stringBuilder1.AppendLine("<td class=\"ticketPropertyTitles\" colspan=\"2\">Datum</td>");
                    stringBuilder1.AppendLine("<td class=\"ticketPropertyTitles\">Waarde EUR</td>");
                    stringBuilder1.AppendLine("<td class=\"ticketPropertyTitles\">Waarde Bonnen</td>");
                    stringBuilder1.AppendLine("<td class=\"ticketPropertyTitles\" colspan=\"2\">Aangemaakt door</td>");
                    stringBuilder1.AppendLine("</tr>");
                    stringBuilder1.AppendLine("<tr>");
                    stringBuilder1.AppendLine("<td class =\"ticketPropertyValues\" colspan=\"2\">" + (object)ticket.CreationTime + "</td>");
                    stringBuilder1.AppendLine("<td class=\"ticketPropertyValues\">" + ticket.TotalPrice.ToString("C") + "</td>");
                    stringBuilder1.AppendLine("<td class=\"ticketPropertyValues\">" + (object)ticket.TotalCoins + "</td>");
                    stringBuilder1.AppendLine("<td class=\"ticketPropertyValues\" colspan=\"2\">" + ticket.CreatedBy.Fullname + "</td>");
                    stringBuilder1.AppendLine("</tr>");
                    stringBuilder1.AppendLine("<tr>");
                    stringBuilder1.AppendLine("<td colspan=\"6\">&nbsp;</td>");
                    stringBuilder1.AppendLine("</tr>");
                    stringBuilder1.AppendLine("<tr>");
                    stringBuilder1.AppendLine(" <td colspan=\"6\" class=\"productsTitle\">Producten</td>");
                    stringBuilder1.AppendLine("</tr>");
                    stringBuilder1.AppendLine("<tr>");
                    stringBuilder1.AppendLine("<td class=\"productsPropertyTitles\">Naam</td>");
                    stringBuilder1.AppendLine("<td class =\"productsPropertyTitles\">Hoeveelheid</td>");
                    stringBuilder1.AppendLine("<td class=\"productsPropertyTitles\">Stukprijs EUR</td>");
                    stringBuilder1.AppendLine("<td class=\"productsPropertyTitles\">Stukprijs Bonnen</td>");
                    stringBuilder1.AppendLine("<td class=\"productsPropertyTitles\">Subtotaal EUR</td>");
                    stringBuilder1.AppendLine("<td class=\"productsPropertyTitles\">Subtotaal Bonnen</td>");
                    stringBuilder1.AppendLine("</tr>");
                    foreach (TicketLine ticketLine in (Collection<TicketLine>)ticket.TicketLines)
                    {
                        stringBuilder1.AppendLine("<tr>");
                        stringBuilder1.AppendLine("<td class =\"productsPropertyValues\">" + ticketLine.Product.Name + "</td>");
                        stringBuilder1.AppendLine("<td class =\"productsPropertyValues\">" + (object)ticketLine.Amount + "</td>");
                        StringBuilder stringBuilder2 = stringBuilder1;
                        double num = ticketLine.UnitPrice;
                        string str2 = "<td class =\"productsPropertyValues\">" + num.ToString("C") + "</td>";
                        stringBuilder2.AppendLine(str2);
                        stringBuilder1.AppendLine("<td class =\"productsPropertyValues\">" + (object)ticketLine.UnitPriceCoins + "</td>");
                        StringBuilder stringBuilder3 = stringBuilder1;
                        num = ticketLine.LinePriceIncl;
                        string str3 = "<td class =\"productsPropertyValues\">" + num.ToString("C") + "</td>";
                        stringBuilder3.AppendLine(str3);
                        stringBuilder1.AppendLine("<td class =\"productsPropertyValues\">" + (object)ticketLine.LinePriceCoins + "</td>");
                        stringBuilder1.AppendLine("</tr>");
                    }
                    stringBuilder1.AppendLine("<tr>");
                    stringBuilder1.AppendLine("<td colspan=\"6\">&nbsp;</td>");
                    stringBuilder1.AppendLine("</tr>");
                    stringBuilder1.AppendLine("<tr>");
                    stringBuilder1.AppendLine("<td colspan=\"6\" class=\"transactionsTitle\">Transacties</td>");
                    stringBuilder1.AppendLine("</tr>");
                    stringBuilder1.AppendLine("<tr>");
                    stringBuilder1.AppendLine("<td class=\"transactionsPropertyTitles\">Datum</td>");
                    stringBuilder1.AppendLine("<td class=\"transactionsPropertyTitles\">Bedrag</td>");
                    stringBuilder1.AppendLine("<td class=\"transactionsPropertyTitles\">Betaalmethode</td>");
                    stringBuilder1.AppendLine("<td class=\"transactionsPropertyTitles\">Afgehandeld door</td>");
                    stringBuilder1.AppendLine("<td class=\"transactionsPropertyTitles\">Ontvangen</td>");
                    stringBuilder1.AppendLine("<td class=\"transactionsPropertyTitles\">Wisselgeld</td>");
                    stringBuilder1.AppendLine("</tr>");
                    foreach (Transaction transaction in (Collection<Transaction>)ticket.Transactions)
                    {
                        stringBuilder1.AppendLine("<tr>");
                        stringBuilder1.AppendLine("<td class =\"transactionsPropertyValues\">" + (object)transaction.PayTime + "</td>");
                        stringBuilder1.AppendLine("<td class =\"transactionsPropertyValues\">" + transaction.Amount.ToString("C") + "</td>");
                        stringBuilder1.AppendLine("<td class =\"transactionsPropertyValues\">" + (object)transaction.PaymentMethodUsed + "</td>");
                        stringBuilder1.AppendLine("<td class =\"transactionsPropertyValues\">" + transaction.PaymentHandledBy.Fullname + "</td>");
                        if (transaction.PaymentMethodUsed == Transaction.PaymentMethod.Cash)
                        {
                            stringBuilder1.AppendLine("<td class =\"transactionsPropertyValues\">" + ((CashTransaction)transaction).MoneyReceived.ToString("C") + "</td>");
                            stringBuilder1.AppendLine("<td class =\"transactionsPropertyValues\">" + ((CashTransaction)transaction).MoneyReturned.ToString("C") + "</td>");
                        }
                        else if (transaction.PaymentMethodUsed == Transaction.PaymentMethod.Coin)
                        {
                            stringBuilder1.AppendLine("<td class =\"transactionsPropertyValues\">" + (object)((CoinTransaction)transaction).CoinsReceived + "</td>");
                            stringBuilder1.AppendLine("<td class =\"transactionsPropertyValues\"></td>");
                        }
                        else if (transaction.PaymentMethodUsed == Transaction.PaymentMethod.Free)
                            stringBuilder1.AppendLine("<td class =\"transactionsPropertyValues\" colspan=\"2\">" + ((FreeTransaction)transaction).Reason + "</td>");
                        else if (transaction.PaymentMethodUsed == Transaction.PaymentMethod.NFC)
                        {
                            stringBuilder1.AppendLine("<td class =\"transactionsPropertyValues\"></td>");
                            stringBuilder1.AppendLine("<td class =\"transactionsPropertyValues\"></td>");
                        }
                        stringBuilder1.AppendLine("</tr>");
                    }
                    stringBuilder1.AppendLine("</table>");
                    stringBuilder1.AppendLine("<br />");
                }
                string str4 = new Regex("\\[(\\w+)\\]", RegexOptions.Compiled).Replace(input, (MatchEvaluator)(match => new Dictionary<string, string>((IEqualityComparer<string>)StringComparer.OrdinalIgnoreCase)
        {
          {
            "Title",
            str1
          },
          {
            "ImagePath",
            Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\Templates\\Html"
          },
          {
            "TicketOverviewTable",
            stringBuilder1.ToString()
          }
        }[match.Groups[1].Value]));
                GlobalConfig config = new GlobalConfig();
                config.SetMargins(new Margins(70, 45, 70, 45)).SetPaperSize(PaperKind.A4);
                SimplePechkin simplePechkin = new SimplePechkin(config);
                ObjectConfig objectConfig = new ObjectConfig();
                objectConfig.SetLoadImages(true);
                objectConfig.SetPrintBackground(true);
                objectConfig.SetZoomFactor(1.1);
                objectConfig.SetAllowLocalContent(true);
                ObjectConfig doc = objectConfig;
                string html = str4;
                File.WriteAllBytes(this.FilePath, simplePechkin.Convert(doc, html));
                PdfReportService.logger.Info("Ticketoverview PDF created: " + this.FilePath);
            }
            catch (Exception ex)
            {
                PdfReportService.logger.Error("Could not create temporary PDF file:" + ex.Message);
            }
        }

        public void GenerateMemberList(List<Member> members, DateTime snapshot)
        {
            try
            {
                string input = File.ReadAllText(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\Templates\\Html\\Template_Memberlist.htm");
                string str1 = "Ledenlijst: " + snapshot.ToShortDateString();
                this.FilePath = Path.GetTempPath() + ("tmp" + Util.Util.RandomString(10)) + ".pdf";
                StringBuilder stringBuilder = new StringBuilder();
                int num = 1;
                foreach (Member member in members)
                {
                    stringBuilder.Append(num % 2 > 0 ? "<tr>" : "<tr class=\"alt\">");
                    stringBuilder.Append("<td>" + (object)num + "</td>");
                    stringBuilder.Append("<td>" + member.LastName + "</td>");
                    stringBuilder.Append("<td>" + member.FirstName + "</td>");
                    stringBuilder.Append("<td>" + member.ZipCode + "</td>");
                    stringBuilder.Append("<td>" + member.City + "</td>");
                    stringBuilder.Append("<td>" + member.Address + "</td>");
                    stringBuilder.Append("<td>" + member.Country + "</td>");
                    DateTime dateTime = member.BirthDate ?? new DateTime();
                    stringBuilder.Append("<td>" + dateTime.ToShortDateString() + "</td>");
                    stringBuilder.Append("<td>" + member.Email + "</td>");
                    stringBuilder.Append("<td>" + member.TelephoneNr + "</td>");
                    stringBuilder.Append("</tr>");
                    ++num;
                }
                string str2 = new Regex("\\[(\\w+)\\]", RegexOptions.Compiled).Replace(input, (MatchEvaluator)(match => new Dictionary<string, string>((IEqualityComparer<string>)StringComparer.OrdinalIgnoreCase)
        {
          {
            "Title",
            str1
          },
          {
            "ImagePath",
            Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\Templates\\Html"
          },
          {
            "MemberOverviewTableElements",
            stringBuilder.ToString()
          }
        }[match.Groups[1].Value]));
                GlobalConfig config = new GlobalConfig();
                config.SetMargins(new Margins(70, 45, 70, 45)).SetPaperSize(PaperKind.A4Rotated);
                SimplePechkin simplePechkin = new SimplePechkin(config);
                ObjectConfig objectConfig = new ObjectConfig();
                objectConfig.SetLoadImages(true);
                objectConfig.SetPrintBackground(true);
                objectConfig.SetZoomFactor(1.1);
                objectConfig.SetAllowLocalContent(true);
                ObjectConfig doc = objectConfig;
                string html = str2;
                File.WriteAllBytes(this.FilePath, simplePechkin.Convert(doc, html));
                PdfReportService.logger.Info("Memberlist PDF created: " + this.FilePath);
            }
            catch (Exception ex)
            {
                PdfReportService.logger.Error("Could not create temporary PDF file:" + ex.Message);
            }
        }

        public void GenerateMemberCardList(List<Member> members)
        {
            string input = File.ReadAllText(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\Templates\\Html\\Template_Membercardlist.htm");
            string str1 = "Lidkaarten: " + DateTime.Now.ToShortDateString();
            this.FilePath = Path.GetTempPath() + ("tmp" + Util.Util.RandomString(10)) + ".pdf";
            StringBuilder stringBuilder = new StringBuilder();
            foreach (Member member in members)
            {
                stringBuilder.AppendLine("<table class=\"table3\">");
                stringBuilder.AppendLine("<tr>");
                stringBuilder.AppendLine("<td colspan=\"6\" class=\"memberTitle\">" + member.LastName + " " + member.FirstName + "</td>");
                stringBuilder.AppendLine("</tr>");
                stringBuilder.AppendLine("<tr>");
                stringBuilder.AppendLine("<td class=\"memberPropertyTitles\">Postcode</td>");
                stringBuilder.AppendLine("<td class=\"memberPropertyTitles\">Gemeente</td>");
                stringBuilder.AppendLine("<td class=\"memberPropertyTitles\" colspan=\"2\">Adres</td>");
                stringBuilder.AppendLine("<td class=\"memberPropertyTitles\">Land</td>");
                stringBuilder.AppendLine("<td class=\"memberPropertyTitles\">Geslacht</td>");
                stringBuilder.AppendLine("</tr>");
                stringBuilder.AppendLine("<tr>");
                stringBuilder.AppendLine("<td class=\"memberPropertyValues\">" + member.ZipCode + "</td>");
                stringBuilder.AppendLine("<td class=\"memberPropertyValues\">" + member.City + "</td>");
                stringBuilder.AppendLine("<td class=\"memberPropertyValues\" colspan=\"2\">" + member.Address + "</td>");
                stringBuilder.AppendLine("<td class=\"memberPropertyValues\">" + member.Country + "</td>");
                stringBuilder.AppendLine("<td class=\"memberPropertyValues\">" + (member.Gender == Member.Genders.Female ? "Vrouw" : "Man") + "</td>");
                stringBuilder.AppendLine("</tr>");
                stringBuilder.AppendLine("<tr>");
                stringBuilder.AppendLine("<td class=\"memberPropertyTitles\" colspan=\"2\">Geboortedatum</td>");
                stringBuilder.AppendLine("<td class=\"memberPropertyTitles\" colspan=\"2\">E-mail adres</td>");
                stringBuilder.AppendLine("<td class=\"memberPropertyTitles\" colspan=\"2\">Telefoonnummer</td>");
                stringBuilder.AppendLine("</tr>");
                DateTime dateTime = member.BirthDate ?? new DateTime();
                stringBuilder.AppendLine("<tr>");
                stringBuilder.AppendLine("<td class=\"memberPropertyValues\" colspan=\"2\">" + dateTime.ToShortDateString() + "</td>");
                stringBuilder.AppendLine("<td class=\"memberPropertyValues\" colspan=\"2\">" + member.Email + "</td>");
                stringBuilder.AppendLine("<td class=\"memberPropertyValues\" colspan=\"2\">" + member.TelephoneNr + "</td>");
                stringBuilder.AppendLine("</tr>");
                stringBuilder.AppendLine("<tr>");
                stringBuilder.AppendLine("<td colspan=\"6\">&nbsp;</td>");
                stringBuilder.AppendLine("</tr>");
                stringBuilder.AppendLine("<tr>");
                stringBuilder.AppendLine(" <td colspan=\"6\" class=\"memberCardTitle\">Lidkaarten</td>");
                stringBuilder.AppendLine("</tr>");
                stringBuilder.AppendLine("<tr>");
                stringBuilder.AppendLine("<td class=\"memberCardPropertyTitles\">Aanmaakdatum</td>");
                stringBuilder.AppendLine("<td class=\"memberCardPropertyTitles\">Vervaldatum</td>");
                stringBuilder.AppendLine("<td class=\"memberCardPropertyTitles\">Actief lid</td>");
                stringBuilder.AppendLine("<td class=\"memberCardPropertyTitles\">Afgedrukt</td>");
                stringBuilder.AppendLine("<td class=\"memberCardPropertyTitles\">Kaart ID</td>");
                stringBuilder.AppendLine("<td class=\"memberCardPropertyTitles\">Aangemaakt door</td>");
                stringBuilder.AppendLine("</tr>");
                foreach (MemberCard memberCard in (Collection<MemberCard>)member.MemberCards)
                {
                    stringBuilder.AppendLine("<tr>");
                    stringBuilder.AppendLine("<td class =\"memberCardPropertyValues\">" + memberCard.CreationTime.ToShortDateString() + "</td>");
                    stringBuilder.AppendLine("<td class =\"memberCardPropertyValues\">" + memberCard.ExpireDate.ToShortDateString() + "</td>");
                    stringBuilder.AppendLine("<td class =\"memberCardPropertyValues\">" + (memberCard.ActiveMember ? "Ja" : "Nee") + "</td>");
                    stringBuilder.AppendLine("<td class =\"memberCardPropertyValues\">" + (memberCard.Printed ? "Ja" : "Nee") + "</td>");
                    stringBuilder.AppendLine("<td class =\"memberCardPropertyValues\">" + memberCard.SmartCardId + "</td>");
                    stringBuilder.AppendLine("<td class =\"memberCardPropertyValues\">" + (memberCard.CreatedBy != null ? memberCard.CreatedBy.Fullname : string.Empty) + "</td>");
                    stringBuilder.AppendLine("</tr>");
                }
                stringBuilder.AppendLine("</table>");
                stringBuilder.AppendLine("<br />");
            }
            string str2 = new Regex("\\[(\\w+)\\]", RegexOptions.Compiled).Replace(input, (MatchEvaluator)(match => new Dictionary<string, string>((IEqualityComparer<string>)StringComparer.OrdinalIgnoreCase)
      {
        {
          "Title",
          str1
        },
        {
          "ImagePath",
          Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\Templates\\Html"
        },
        {
          "MemberCardOverviewTable",
          stringBuilder.ToString()
        }
      }[match.Groups[1].Value]));
            GlobalConfig config = new GlobalConfig();
            config.SetMargins(new Margins(70, 45, 70, 45)).SetPaperSize(PaperKind.A4);
            SimplePechkin simplePechkin = new SimplePechkin(config);
            ObjectConfig objectConfig = new ObjectConfig();
            objectConfig.SetLoadImages(true);
            objectConfig.SetPrintBackground(true);
            objectConfig.SetZoomFactor(1.1);
            objectConfig.SetAllowLocalContent(true);
            ObjectConfig doc = objectConfig;
            string html = str2;
            File.WriteAllBytes(this.FilePath, simplePechkin.Convert(doc, html));
            PdfReportService.logger.Info("Membercardlist PDF created: " + this.FilePath);
        }

        public void SendReportAsMail(string subject, string message = "")
        {
            if (!string.IsNullOrWhiteSpace(this.MailFromAddress) && !string.IsNullOrWhiteSpace(this.MailFromName) && (!string.IsNullOrWhiteSpace(this.MailServer) && !string.IsNullOrWhiteSpace(this.MailTo)))
            {
                if (!string.IsNullOrWhiteSpace(this.FilePath))
                {
                    try
                    {
                        using (MailMessage message1 = new MailMessage())
                        {
                            message1.To.Add(this.MailTo);
                            message1.Subject = subject;
                            message1.From = new MailAddress(this.MailFromAddress, this.MailFromName);
                            message1.Body = message;
                            message1.Attachments.Add(new Attachment(this.FilePath, new ContentType("application/pdf")));
                            new SmtpClient(this.MailServer)
                            {
                                Timeout = 10000
                            }.Send(message1);
                        }
                        PdfReportService.logger.Info("Mail succesfully sent to: " + this.MailTo);
                        File.Delete(this.FilePath);
                        return;
                    }
                    catch (Exception ex)
                    {
                        PdfReportService.logger.Error("Could not send report email: " + ex.Message);
                        return;
                    }
                }
            }
            try
            {
                File.Delete(this.FilePath);
            }
            catch (Exception ex)
            {
                PdfReportService.logger.Error("Could not send report email: " + ex.Message);
            }
            PdfReportService.logger.Warn("Mail not sent due to invalid mail configuration");
        }

        public bool SaveFile(string filePath)
        {
            try
            {
                File.Copy(this.FilePath, filePath);
                return true;
            }
            catch (IOException ex)
            {
                PdfReportService.logger.Error("Failed to copy report file from " + this.FilePath + " to " + this.FilePath + ": " + (object)ex.InnerException);
                return false;
            }
        }
    }
}
