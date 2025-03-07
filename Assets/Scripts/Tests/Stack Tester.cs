using UnityEngine;
using System;
using System.Collections.Generic;

public class StackTester : MonoBehaviour
{
    private void Start() {
        // works
        Stack<string> originalStack = new Stack<string>();
        originalStack.Push("a");
        originalStack.Push("b");
        originalStack.Push("c");
        originalStack.Push("d");
        PrintStack(originalStack, "original");

        // works
        List<string> dataList = new List<string> {"a", "b", "c", "d"};
        Stack<string> listStack = new Stack<string>(dataList);
        PrintStack(listStack, "list");

        // fails - backwards
        Stack<string> copyStack = new Stack<string>(originalStack);
        PrintStack(copyStack, "copy");

        // works
        Stack<string> doubleCopyStack = new Stack<string>(new Stack<string>(originalStack));
        PrintStack(doubleCopyStack, "double copy");
    }

    private void PrintStack(Stack<string> stack, string name)
    {
        string output = $"Data from {name}:\n\tCurrent element: {stack.Peek()}\n\tComplete stack:";
        foreach (string item in stack)
        {
            output += $"\n\t\t{item}";
        }
        Debug.Log(output);
    }
}
