namespace Model
{
    public static class Props
    {
        /// <summary>
        /// Returns a string that describes sorting.
        /// </summary>
        /// <param name="sortering"></param>
        /// <returns></returns>
        public static string GetSortingText(Sort sortering)
        {
            switch (sortering)
            {
                case Sort.none:
                    return "Ingen Sortering";

                case Sort.alfa:
                    return "Sortert Alfabetisk";

                case Sort.pris:
                    return "Sortert stigende etter pris";

                case Sort.prisHøy:
                    return "Sortert synkende etter pris";

                case Sort.tilf:
                    return "I tilfeldig rekkefølge";

                default:
                    return null;
            }
        }
    }

    /// <summary>
    /// Enum that says what sorting that is current.
    /// </summary>
    public enum Sort
    {
        none,
        pris,
        prisHøy,
        tilf,
        alfa
    }
}