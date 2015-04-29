namespace DALSample
{
    public static class Tools
    {
        public static int GetPageCount(int pageSize, int count)
        {
            var pages = count / pageSize;
            if (count % pageSize > 0) pages++;
            return pages;
        }
    }
}