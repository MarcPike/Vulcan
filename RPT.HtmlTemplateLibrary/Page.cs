namespace RPT.HtmlTemplateLibrary
{
    public class Page
    {
        public int PageNumber { get; set; }
        public int ItemsThisPage { get; set; }

        public Page(int pageNumber, int itemsThisPage)
        {
            PageNumber = pageNumber;
            ItemsThisPage = itemsThisPage;
        }
    }

}
