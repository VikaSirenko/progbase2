using System;
using System.Collections.Generic;

namespace lab5
{
    public class DataProcessor
    {
        public Root root;

        public int GetCountOfTotalPages()
        {
            const int pageSize = 10;
            int totalPages = (int)Math.Ceiling(this.root.customers.Count / (double)pageSize);
            return totalPages;
        }

        public List<Customer> GetPage(int pageNum)
        {
            List<Customer> customersFromPage = new List<Customer>();
            const int pageSize = 10;
            int index = (pageNum - 1) * pageSize;
            for (int i = index; i < index + pageSize; i++)
            {
                customersFromPage.Add(this.root.customers[i]);
            }

            return customersFromPage;
        }

        public void Output(List<Customer> customers)
        {
            for (int i = 0; i < customers.Count; i++)
            {
                Customer customer = customers[i];
                Console.WriteLine(
                @$"C_CUSTKEY: {customer.customerKey}
                C_NAME: {customer.name}
                C_ADDRESS: {customer.address}
                C_NATIONKEY: {customer.nationKey}
                C_PHONE: {customer.phoneNumber}
                C_ACCTBAL: {customer.accountBalanceRebate}
                C_MKTSEGMENT: {customer.marketSegmentation}
                C_COMMENT: {customer.comment}");
                Console.WriteLine();
            }

        }

        public List<Customer> FindWithGreatestValue(int N)
        {
            List<Customer> sortedList = new List<Customer>();
            for (int i = 0; i < this.root.customers.Count; i++)
            {
                sortedList.Add(this.root.customers[i]);
            }
            sortedList = this.DoSort(sortedList);
            List<Customer> customersWithGreatestValue = new List<Customer>();
            for (int i = sortedList.Count - 1; i > sortedList.Count - N - 1; i--)
            {
                customersWithGreatestValue.Add(sortedList[i]);
            }
            return customersWithGreatestValue;
        }

        public List<Customer> DoSort(List<Customer> customers)
        {
            customers.Sort((x, y) => x.accountBalanceRebate.CompareTo(y.accountBalanceRebate));
            return customers;

        }

        public List<string> GetNamesList(int nationKey)
        {
            List<string> names = new List<string>();
            for (int i = 0; i < this.root.customers.Count ; i++)
            {
                Customer customer = this.root.customers[i];
                if (customer.nationKey == nationKey)
                {
                    names.Add(customer.name);
                }

            }
            return names;
        }

        public List<string> GetCommentsList()
        {
            List<string> comments = new List<string>();
            for(int i=0; i<this.root.customers.Count; i++)
            {
                Customer customer= this.root.customers[i];
                comments.Add(customer.comment);
            }
            return comments;
        }


    }
}