using System;

namespace ConsoleApplication1
{
    /*
    Question:
    Implement an algorithm to find the nth to last element of a singly linked list.
    */

    /*
    Discussion:
    In an interview situation, I would start by asking questions for clarification.  
    Things like... what do you want to happen when the list is empty? what about when 
    there aren't enough items in the list? what about when N is negative? etc. 
    Generally speaking, you'd want to throw an exception (which is what I decided to do in the code).  
    But there may be some edge cases where returning null makes more sense.  
    The point is to ask for clarifications to ensure full understanding of the problem/specs/expectations.  
    In asking these clarifying questions, I would also come up with (and talk about) test cases to 
    verify later when writing the code.
    */

    class NthFromEndLinkedList
    {
        public static void Test()
        {
            RunTest_ExpectException(0);
            RunTest_ExpectException(1);
            RunTest_ExpectException(4);
            RunTest_ExpectException(-2);
            RunTest_ExpectException(4, "b", "b", "b");
            RunTest_ExpectException(9, "b", "b", "b");
            RunTest_ExpectException(-2, "b", "b", "b");

            RunTest_ExpectAnswer("a", 2, "a", "b");
            RunTest_ExpectAnswer("a", 3, "b", "b", "b", "b", "b", "b", "b", "a", "b", "b");
            RunTest_ExpectAnswer("a", 6, "b", "b", "b", "b", "b", "b", "b", "a", "b", "b", "b", "b", "b");
            RunTest_ExpectAnswer("a", 1, "a");

            Console.ReadLine();
        }

        public static string Find(Node<string> head, int n)
        {
            if (head == null)
                throw new Exception("No list provided.");
            if (n < 1)
                throw new Exception("N must be positive.");

            Node<string> cur = head;
            Node<string> end = head;

            //Advance the end pointer N-1 times
            for (int i = 0; i < n - 1; i++)
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

        private static Node<string> LoadList(params string[] data)
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

        private static void RunTest_ExpectAnswer(string expectedValue, int n, params string[] listData)
        {
            Node<string> lst = LoadList(listData);
            string found = Find(lst, n);
            if (found == expectedValue)
                Console.WriteLine("success, found " + found);
            else
                Console.WriteLine("failure, found " + found + " instead of " + expectedValue);
        }

        private static void RunTest_ExpectException(int n, params string[] listData)
        {
            Node<string> lst = LoadList(listData);
            try
            {
                Console.WriteLine("failure, found " + Find(lst, n) + " instead of getting an exception");
            }
            catch (Exception ex)
            {
                Console.WriteLine("success, expected exception: " + ex.Message);
            }
        }
    }

    public class Node<T>
    {
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
