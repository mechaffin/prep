using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    /*
    Question:
    Write function to evaluate math expression
        int Eval(string exp)
    There are 4 operators + – * /, numbers can be multiple digits

    Example:
        Input: string  10 + 5 *10
        Output:  60
    */

    /*
    Discussion:

    In an interview, its important to verify the requirements. In this case, some of the things to ask about would be: 
    Are we using standard order of operations? The example does appear to use the normal order of operations where 
    multiplication and division happen before addition and subtraction. However, since division and subtraction are 
    not used in the example, I would want to ask.  Also, I would verify that we're using base 10 numbers.
    The example also used only whole numbers (integers) and an integer is the return value. These requirements should be verified as well.
    I would also seriously question the requirement that the output is an integer. With division allowed, its very easy to 
    get a non-integer answer for the equation.  So we'll need to clarify what to do in these cases? Round, truncate, throw an exception, etc.  
    Is there some reason the return value is an integer? There may be some business reason for it, so ask... it may be an oversight 
    and the requirement can be changed to allow decimals in the return value.  In fact, they could decide that decimals are allowed 
    in the input values as well.  I wouldn't expect this in an interview question (at least initially) because it adds some 
    complexity and would take time away from solving the equation (which I believe is what they are really after). But this could be 
    something they'll delve deeper into if you solve everything else within the alloted time.

    In the discussion about this type of question, I would also point out that there are really two parts of the 
    question: parsing the equation and solving it. Both parts are things that could be used to drill into deeper knowledge. 
    For parsing the string, there are issues around converting strings to numbers, determining if the equation is valid 
    (i.e. are there extra numbers or operators), decimal places inside the numbers, unknown operators, appearance of letters, 
    empty or null input string, etc. For solving the equation, there are lots of errors that could happen here as well. 
    Some of those include exceeding max int, divide by zero, non-integer result, etc. Verify what behavior they want in these cases. 
    
    There are also usually multiple ways to tackle the problem... brute force and/or use of data structures. 
    Talk about the options, including the Big O for each potential solution (volunteer at least best case and worst case, 
    knowing that they may ask about average case). 
    Generally speaking, they do not want to see a solution for brute force, but don't overlook it either.  
    If you can think of ways to solve it using multiple different data structures, talk about the options.  
    They want to know you can apply different structures and think otuside the box.  The Big O will likely lead 
    you to which one to actually implement. If you code and test it quickly, they'll likely add some new requirement 
    or impose a restriction that will change the solution.

    For the purpose of this excercise (and since I don't have a live interviewer to ask), I am going to assume that 
    the "point" of this question is the solving part.  So I will impose some rules for the string so the parsing can be 
    implemented without much effort. The numbers and operators will be separated by spaces and only valid equations will 
    be provided. I'll also utilize some built-in functions.  
    We can come back around to actually implementing the parsing at a later time, if you like.
    Also, I am going to assume standard order of operations for the 4 stated operators.

    */
    class EvalMathEquation
    {
        public static void Test()
        {
            RunTest_ExpectException(null);
            RunTest_ExpectException("");
            RunTest_ExpectException("100 / 0");

            RunTest_ExpectAnswer("10 + 5 * 10", 60); //original example 
            RunTest_ExpectAnswer("10 + 5 * 10 * 2 + 3", 113);
            RunTest_ExpectAnswer("10 - 5 * 10", -40);
            RunTest_ExpectAnswer("10 - 50 / 10", 5);
            RunTest_ExpectAnswer("1 + 2 + 3 + 4 + 5", 15);
            RunTest_ExpectAnswer("1 - 2 - 3 - 4 - 5", -13);
            RunTest_ExpectAnswer("1 * 2 * 3 * 4 * 5", 120);
            RunTest_ExpectAnswer("0 * 2", 0);
            RunTest_ExpectAnswer("0 + 2", 2);
            RunTest_ExpectAnswer("0 / 2", 0);
            RunTest_ExpectAnswer("13", 13);

            RunTest_ExpectAnswer("5 / 2", 2); 
            RunTest_ExpectAnswer("7 / 3", 2);

            Console.ReadLine();
        }

        public static int Eval(string exp)
        {
            string[] arr = Parse(exp);
            double result = Solve(arr);

            //The requirement for this function was to return an integer... we'll round it for now.  
            //Other options: change requirement, throw exception, truncate, etc.
            return Convert.ToInt32(result);
        }

        private static string[] Parse(string exp)
        {
            if (exp == null)
                throw new ArgumentNullException("exp");

            //Super simple parse... we can implement this by hand later, if you like.
            //If this were implemented by hand, we'd need to valdiate all sorts of things: valid numbers, 
            //determine if the equation is valid (i.e.are there extra numbers or operators), unknown operators, appearance of letters, etc.
            return exp.Split(' ');
        }

        private static double Solve(string[] arr)
        {
            /*
            Solution: 
            I think there's a way to do this using a tree (and that would be more flexible as requirements change).  
            However, for "thinking outside the box" points, I'm going to use a stack. Because there are only 2 levels of precedence, 
            this will work. But I totally acknowledge that it breaks down if/when additional levels are added (like when you add parentheses).
            
            I'm not going to implement the stack at this point. Again, that's another level of digging deeper... 
            where I can implement that later if asked.
            */

            Stack<double> s = new Stack<double>();

            int i = 0;
            while (i < arr.Length)
            {
                ItemType typ = GetItemType(arr[i]);
                switch (typ)
                {
                    case ItemType.OperatorLevel1:
                        double prevOperand = s.Pop();
                        double nextOperand = ConvertToNumber(arr[i + 1]);
                        if (arr[i] == "*")
                        {
                            s.Push(prevOperand * nextOperand); //multiplication
                        }
                        else
                        {
                            if (nextOperand == 0)
                                throw new DivideByZeroException();
                            s.Push(prevOperand / nextOperand); //division
                        }
                        i++; //need to advance past the operator 
                        break;
                    case ItemType.OperatorLevel2:
                        //Push the operand after this operator onto the stack. 
                        if (arr[i] == "-") 
                            s.Push(-1 * ConvertToNumber(arr[i+1])); //subtraction, needs to be a negative number
                        else
                            s.Push(ConvertToNumber(arr[i + 1])); //addition
                        i++; //need to advance past the operator 
                        break;
                    case ItemType.Operand:
                        //Put the operand on the stack.  This'll only be used for the first item.
                        s.Push(ConvertToNumber(arr[i]));
                        break;
                    default:
                        break;
                }
                i++; //go to the next item
            }

            //Everything that is left in the stack just needs to be added up.
            double result = 0;
            while (s.Count > 0)
            {
                result += s.Pop();
            }
            return result;
        }

        private enum ItemType { OperatorLevel1, OperatorLevel2, Operand}

        private static ItemType GetItemType(string input)
        {
            string[] OPERATORS_LEVEL1 = { "*", "/" }; //operators in the first level of precedence (they are evaluated first)
            string[] OPERATORS_LEVEL2 = { "+", "-" }; //operators in the second level of precedence 

            if (OPERATORS_LEVEL1.Contains(input)) return ItemType.OperatorLevel1;
            if (OPERATORS_LEVEL2.Contains(input)) return ItemType.OperatorLevel2;

            double d = 0;
            if (Double.TryParse(input, out d))
                return ItemType.Operand;
            else
                throw new InvalidCastException("Operand not valid.");
        }

        private static double ConvertToNumber(string input)
        {
            //Using built in functions for now... could  implement this by hand as well.
            return Convert.ToDouble(input);
        }

        private static void RunTest_ExpectAnswer(string exp, int expectedValue)
        {
            int actual = Eval(exp);

            if (actual == expectedValue)
                Console.WriteLine("success, sovled " + exp + " == " + actual.ToString());
            else
                Console.WriteLine("failure, soved " + exp + " == " + actual.ToString() + " (instead of "+ expectedValue + ")");
        }

        private static void RunTest_ExpectException(string exp)
        {
            try
            {
                int actual = Eval(exp);
                Console.WriteLine("failure, solved to " + actual.ToString() + " instead of getting an exception");
            }
            catch (Exception ex)
            {
                Console.WriteLine("success, expected exception: " + ex.Message);
            }
        }
    }
}
