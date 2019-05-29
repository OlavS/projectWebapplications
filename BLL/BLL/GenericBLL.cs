using System.Collections.Generic;

namespace BLL.BLL
{
    public class GenericBLL<T> : Interfaces.IGenericBLL<T>
    {
        /// <summary>
        /// Generic search method for lists.
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="inList">A list with a ToString that can match with the searchstring.</param>
        /// <returns></returns>
        public List<T> Search(string searchString, List<T> inList)
        {
            if (searchString == "")
            {
                return inList;
            }

            List<T> result = new List<T>();
            string[] searchSplit = searchString.ToLower().Split();

            foreach (var item in inList)
            {
                string[] tostringSplit = item.ToString().ToLower().Split();
                for (int j = 0; j < tostringSplit.Length; j++)
                {
                    for (int i = 0; i < searchSplit.Length; i++)
                    {
                        if (tostringSplit.Length == i + j)
                        {
                            break;
                        }

                        if (i + 1 == searchSplit.Length && tostringSplit[j + i].StartsWith(searchSplit[i]))
                        {
                            AddUnique(item, result);
                            break;
                        }
                        else if (searchSplit[i] != tostringSplit[j + i])
                        {
                            break;
                        }

                        if (searchSplit.Length == i + 1)
                        {
                            AddUnique(item, result);
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Adds to list if not existing.
        /// </summary>
        /// <param name="item">To be added</param>
        /// <param name="list">To be added to</param>
        /// <returns></returns>
        public List<T> AddUnique(T item, List<T> list)
        {
            if (!list.Contains(item))
            {
                list.Add(item);
            }
            return list;
        }
    }
}