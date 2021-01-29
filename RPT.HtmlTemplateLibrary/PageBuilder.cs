using System.Collections.Generic;
using System.Linq;

namespace RPT.HtmlTemplateLibrary
{
    public class PageBuilder
    {
        private int LinesPerFirstPage { get; set; }
        private int LinesPerMiddlePage { get; set; }
        private int LinesPerLastPage { get; set; }
        private int TotalLines { get; set; }
        public List<Page> Pages { get; set; } = new List<Page>();

        public PageBuilder(int linesPerFirstPage, int linesPerMiddlePage, int linesPerLastPage, int totalLines)
        {
            LinesPerFirstPage = linesPerFirstPage;
            LinesPerMiddlePage = linesPerMiddlePage;
            LinesPerLastPage = linesPerLastPage;
            TotalLines = totalLines;
            Calculate();
        }

        private void Calculate()
        {
            var lineCount = 0;
            var pageCount = 0;
            for (int i = 1; i < TotalLines; i++)
            {
                lineCount++;
                if ((Pages.Count == 0) && (lineCount == LinesPerFirstPage))
                {
                    pageCount++;
                    Pages.Add(new Page(pageCount, LinesPerFirstPage));
                    lineCount = 0;
                }

                if ((Pages.Count > 0) && lineCount == LinesPerMiddlePage)
                {
                    pageCount++;
                    Pages.Add(new Page(pageCount,LinesPerMiddlePage));
                }
            }

            var lastPage = Pages.LastOrDefault();

            if (lastPage != null)
            {
                //if (lastPage.ItemsThisPage > _linesPerLastPage)
                //{
                //    lastPage.ItemsThisPage 
                //}
            }

        }
    }

}
