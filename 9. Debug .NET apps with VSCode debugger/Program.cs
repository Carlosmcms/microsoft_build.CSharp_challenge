// See https://aka.ms/new-console-template for more information
using System.Diagnostics;

Console.WriteLine("Hello, World!");

int result = Fibonacci(5);
System.Diagnostics.Trace.WriteIf(result == 3, "Error in Fibonacci exec. ");
Console.WriteLine(result);

static int Fibonacci(int n)
{
    Debug.WriteLine($"Entering {nameof(Fibonacci)} method");
    Debug.WriteLine($"We are looking for the {n}th number");

    int n1 = 0;
    int n2 = 1;
    int sum;

    for (int i = 2; i <= n; i++)
    {
        sum = n1 + n2;
        n1 = n2;
        n2 = sum;
        Debug.WriteLineIf(sum == 1, $"sum is 1, n1 is {n1}, n2 is {n2}"); 
    }

    return n == 0 ? n1 : n2;
}

// System.Console.WriteLine vs System.Diagnostics.Trace vs System.Diagnostics.Debug
bool errorFlag = false;  
System.Diagnostics.Trace.WriteIf(errorFlag, "Error in AppendData procedure.");  
System.Diagnostics.Debug.WriteIf(errorFlag, "Transaction abandoned.");  
System.Diagnostics.Trace.Write("Invalid value for data request");

int IntegerDivide(int dividend, int divisor)
{
    Debug.Assert(divisor != 0, $"{nameof(divisor)} is 0 and will cause an exception.");

    return dividend / divisor;
}

// If n2 is 5 continue, else break.
// Debug.Assert(n2 == 5, "The return value is not 5 and it should be.");
// return n == 0 ? n1 : n2;