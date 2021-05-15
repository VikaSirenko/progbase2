using System;
using System.Collections.Generic;

namespace lab5
{
    public static class DataProcessor
    {

        public static int GetCountOfTotalPages(int costumersCount)
        {
            const int pageSize = 10;
            int totalPages = (int)Math.Ceiling(costumersCount / (double)pageSize);
            return totalPages;
        }

        public static List<Customer> GetPage(int pageNum, List<Customer> customers)
        {
            List<Customer> customersFromPage = new List<Customer>();
            const int pageSize = 10;
            int index = (pageNum - 1) * pageSize;
            for (int i = index; i < index + pageSize; i++)
            {
                customersFromPage.Add(customers[i]);
            }

            return customersFromPage;
        }


        public static List<Customer> FindWithGreatestValue(int N, List<Customer> customers)
        {
            List<Customer> sortedList = new List<Customer>();
            for (int i = 0; i < customers.Count; i++)
            {
                sortedList.Add(customers[i]);
            }
            sortedList = DoSort(sortedList);
            List<Customer> customersWithGreatestValue = new List<Customer>();
            for (int i = sortedList.Count - 1; i > sortedList.Count - N - 1; i--)
            {
                customersWithGreatestValue.Add(sortedList[i]);
            }
            return customersWithGreatestValue;
        }

        public static List<Customer> DoSort(List<Customer> customers)
        {
            customers.Sort((x, y) => x.accountBalanceRebate.CompareTo(y.accountBalanceRebate));
            return customers;

        }

        public static List<string> GetNamesList(int nationKey, List<Customer> customers)
        {
            List<string> names = new List<string>();
            for (int i = 0; i < customers.Count; i++)
            {
                Customer customer = customers[i];
                if (customer.nationKey == nationKey)
                {
                    names.Add(customer.name);
                }

            }
            return names;
        }

        public static List<string> GetCommentsList(List<Customer> customers)
        {
            List<string> comments = new List<string>();
            for (int i = 0; i < customers.Count; i++)
            {
                Customer customer = customers[i];
                comments.Add(customer.comment);
            }
            return comments;
        }


    }
}