using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Plagiarism.CoreLibrary.Libraries;
using Plagiarism.CoreLibrary.Models;
using Plagiarism.DataLayer.Models;
using System.Data.Entity;
namespace Plagiarism.ServiceRunner.Engine
{
    public class CrawlerEngine
    {
        private MainContext _context = new MainContext();
        public void Run()
        {
            FirstCrawler();
        }
        private void FirstCrawler()
        {
            try
            {
                Console.WriteLine("Crawling all data....");
                List<Assignment> allAssignments = _context.Assignments.Include(x => x.SubmittedAssignments).ToList();
                foreach (Assignment assignment in allAssignments)
                {
                    foreach (SubmittedAssignment submittedAssignment in assignment
                        .SubmittedAssignments
                        .Where(x=>!x.IsChecked)
                        .OrderBy(x => x.Counter)
                        .ToList())
                    {
                        SecondaryCrawler(submittedAssignment);
                    }
                }
                
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
            finally
            {
                Console.WriteLine("Crawling finished.");
            }
        }
        private void SecondaryCrawler(SubmittedAssignment submittedAssignment)
        {
            try
            {
                int max = submittedAssignment.Counter;
                Assignment assignment = _context.Assignments.FirstOrDefault(x => x.Id == submittedAssignment.Assignment.Id);
                if(assignment!=null)
                {
                    List<SubmittedAssignment> allSubmittedAssignment = assignment.SubmittedAssignments.OrderBy(x => x.Counter)
                        .Where(x => (x.Counter < max) && (x.IsAccepted)).ToList();
                    TextProcessing textProcessing = new TextProcessing(BaseWords.GetBaseWords());
                    PlagiarismChecker checker = new PlagiarismChecker();
                    int len = allSubmittedAssignment.Count;
                    for (int i=0;i<len;i++)
                    {
                        SubmittedAssignment comparer = allSubmittedAssignment[i];
                        try
                        {
                            string textSubject = submittedAssignment.Data;
                            string textComparer = comparer.Data;
                            string[] textSubjectArray = textProcessing.Process(textSubject);
                            string[] textComparerArray = textProcessing.Process(textComparer);
                            PlagiarismModel plagiarismResult = checker.Check(textSubjectArray, textComparerArray);
                            submittedAssignment.Description = plagiarismResult.Description;
                            submittedAssignment.Percentage = plagiarismResult.Percentage;
                            submittedAssignment.PercentageInteger = plagiarismResult.PercentageInteger;
                            submittedAssignment.IsAccepted = (plagiarismResult.PercentageInteger > 70) ? false : true;
                            if (!submittedAssignment.IsAccepted)
                                break;
                        }

                        catch (Exception ex)
                        {
                            Trace.WriteLine(ex);

                        }
                    }
                    submittedAssignment.IsChecked = true;
                    if(!_context.SubmittedAssignments.Local.Contains(submittedAssignment))
                    {
                        _context.SubmittedAssignments.Attach(submittedAssignment);
                    }
                    _context.Entry(submittedAssignment).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();
                }
                    
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);

            }
        }
    }
}
