using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            runTest_ExpectException(0);
            runTest_ExpectException(1);
            runTest_ExpectException(4);
            runTest_ExpectException(-2);
            runTest_ExpectException(4, "b", "b", "b");
            runTest_ExpectException(9, "b", "b", "b");
            runTest_ExpectException(-2, "b", "b", "b");

            runTest_ExpectAnswer("a", 2, "a", "b");
            runTest_ExpectAnswer("a", 3, "b", "b", "b", "b", "b", "b", "b", "a", "b", "b");
            runTest_ExpectAnswer("a", 6, "b", "b", "b", "b", "b", "b", "b", "a", "b", "b", "b", "b", "b");
            runTest_ExpectAnswer("a", 1, "a");
                        
            Console.ReadLine();
        }

        private static string findNthNodeFromEnd(Node<string> head, int n)
        {
            if (head == null)
                throw new Exception("No list provided.");
            if (n < 1)
                throw new Exception("N must be positive.");

            Node<string> cur = head;
            Node<string> end = head;

            //Advance the end pointer N-1 times
            for (int i = 0; i < n-1; i++)
            {
                if (end.Next == null)
                    throw new Exception("Not enough items in the list.");
                
                end = end.Next;
            }

            //Advance both cur and end until you get to the end of the list. 
            //At that point, cur points to the Nth item from the end.
            while (end.Next != null)
            {
                end = end.Next;
                cur = cur.Next;
            }

            return cur.Data;
        }

        private static Node<string> loadList(params string[] data)
        {
            if (data == null || data.Length == 0)
                return null;

            Node<string> head = new Node<string>();
            Node<string> cur = head;
            for (int i = 0; i < data.Length; i++)
            {
                cur.Data = data[i];
                if (i < data.Length - 1)
                {
                    cur.Next = new Node<string>();
                    cur = cur.Next;
                }
            }
            return head;
        }

        private static void runTest_ExpectAnswer(string expectedValue, int n, params string[] listData)
        {
            Node<string> lst = loadList(listData);
            string found = findNthNodeFromEnd(lst, n);
            if (found == expectedValue)
                Console.WriteLine("success, found " + found);
            else
                Console.WriteLine("failure, found " + found + " instead of " + expectedValue);
        }

        private static void runTest_ExpectException(int n, params string[] listData)
        {
            Node<string> lst = loadList(listData);
            try
            {
                Console.WriteLine("failure, found " + findNthNodeFromEnd(lst, n) + " instead of getting an exception");
            }
            catch (Exception ex)
            {
                Console.WriteLine("success, expected exception: " + ex.Message);
            }
        }
    }

    public class Node<T> {
        public T Data { get; set; }
        public Node<T> Next { get; set; }

        public Node() { }
        
        public Node(T d, Node<T> n)
        {
            Data = d;
            Next = n;
        }
    }
}
