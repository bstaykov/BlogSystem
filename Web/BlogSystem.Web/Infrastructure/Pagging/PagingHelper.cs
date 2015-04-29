namespace BlogSystem.Web.Infrastructure.Pagging
{
    using System;

    public static class PagingHelper
    {
        public static void CheckParams(ref int pageNumber, ref int postsPerPage)
        {
            if (pageNumber <= 0)
            {
                pageNumber = 1;
            }

            if (postsPerPage <= 0 || 20 < postsPerPage)
            {
                postsPerPage = 2;
            }
        }

        public static void SetPagingParams(int postsCount, int postsPerPage, int pageNumber, out int availablePages, out int startPage, out int endPage)
        {
            startPage = 1;
            endPage = 5;
            availablePages = (int)Math.Ceiling((double)postsCount / postsPerPage);

            if (availablePages <= 5)
            {
                startPage = 1;
                endPage = availablePages;
            }
            else
            {
                startPage = pageNumber - 2;
                endPage = pageNumber + 2;
                while (startPage < 1)
                {
                    startPage++;
                    endPage++;
                }

                while (endPage > availablePages)
                {
                    endPage--;
                    startPage--;
                }
            }
        }
    }
}