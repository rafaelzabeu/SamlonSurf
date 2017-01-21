using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public static class ExpressionParser
{
    /// <summary>
    /// Trys to parse the passed string to a graph struct.
    /// In case of faliure returns false and the out paramenter has null value.
    /// In case of success returns true and the out parameter has the parsed graph.
    /// </summary>
    /// <param name="input"></param>
    /// <param name="graph"></param>
    /// <returns></returns>
    public static bool TryParse(string input, out Graph graph)
    {
        try
        {
            graph = Parse(input);
#if UNITY_EDITOR
            StringBuilder builder = new StringBuilder();
            foreach (var item in graph.expression.tokens)
            {
                builder.Append(item.token);
            }
            Debug.Log(builder.ToString());
#endif
            return true;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            graph = new Graph();
            return false;
        }
    }

    /// <summary>
    /// Parses the passed string to a Graph struct.
    /// </summary>
    /// <param name="input">String to parse</param>
    /// <returns>Parsed expression</returns>
    public static Graph Parse(string input)
    {
        Graph graph = new Graph();

        string[] c = new string[] { "{", "}", "," };

        //separates the startX and endX from the main equation, does not output empty strings
        string[] parameters = input.Split(c, StringSplitOptions.RemoveEmptyEntries);

        //gets the startX if its there
        if (parameters.Length >= 3)
        {
            //splits into parsable tokens, transform to rpn, creates a expression and evaluates is final expression
            graph.startX = new Expression(infixToPrefix(tokenize(parameters[1]))).solveForX(0);
            graph.endX = new Expression(infixToPrefix(tokenize(parameters[2]))).solveForX(0);

        }
        
        //split the string into parseable tokens
        List<Token> tokens = tokenize(parameters[0]);
        
        //transforms to rpn notation
        List<Token> infix = infixToPrefix(tokens);

        //creates the expression from the rpn 
        graph.expression = new Expression(infix);

        //creates and returns the expression
        return graph;
    }

    //Shunting-yard algorithm modified to aceppt variables
    //https://en.wikipedia.org/wiki/Shunting-yard_algorithm
    private static List<Token> infixToPrefix(List<Token> tokens)
    {
        List<Token> output = new List<Token>();
        Stack<Token> operatorStack = new Stack<Token>();

        foreach (var token in tokens)
        {
            if (token.tokenType == TokenType.Number || token.tokenType == TokenType.Variable)
            {
                output.Add(token);
            }
            else if (token.tokenType == TokenType.Operator)
            {
                while (operatorStack.Count > 0)
                {
                    if (operatorPrecedenceStackCheck(token, operatorStack.Peek()))
                    {
                        output.Add(operatorStack.Pop());
                    }
                    else
                    {
                        break;
                    }
                }
                operatorStack.Push(token);
            }
            else if (token.tokenType == TokenType.Parentesis)
            {
                if (token.token == "(")
                {
                    operatorStack.Push(token);
                }
                else
                {
                    while (operatorStack.Peek().token != "(")
                    {
                        output.Add(operatorStack.Pop());
                    }
                    operatorStack.Pop();
                }
            }
        }

        while (operatorStack.Count > 0)
        {
            output.Add(operatorStack.Pop());
        }

        return output;
    }

    private static bool operatorPrecedenceStackCheck(Token t1, Token t2)
    {
        bool result = false;

        result = result || t1.leftAssociative && t1.precedence <= t2.precedence;
        result = result || !t1.leftAssociative && t1.precedence < t2.precedence;

        return result;
    }

    private static List<Token> tokenize(string equation)
    {
        //removes white spaces and puts all letters to lower variant
        equation = Regex.Replace(equation, @"\s+", "").ToLower();

        List<Token> tokens = new List<Token>();
        Token token, lastToken = null;
        for (int i = 0; i < equation.Length; i++)
        {

            token = new Token(equation[i]);
            //Detects implict multiplication: 4x == 4*x, 2(x + 2) == 2 *(x + 2)
            if (isImplictMutiplication(token, lastToken))
            {
                tokens.Add(new Token("*"));
            }
            //Detects if the current char is part of the same number as the last one read
            if (isStillTheSameNumber(token, lastToken))
            {
                token = new Token(lastToken.token + token.token);
                tokens.RemoveAt(tokens.Count - 1);
            }
            tokens.Add(token);
            lastToken = token;
        }

        return tokens;
    }

    private static bool isStillTheSameNumber(Token currToken, Token lastToken)
    {
        if (lastToken == null)
        {
            return false;
        }

        return lastToken.tokenType == TokenType.Number && currToken.tokenType == TokenType.Number;
    }

    private static bool isImplictMutiplication(Token currToken, Token lastToken)
    {
        if (lastToken == null)
            return false;
        if (lastToken.tokenType == TokenType.Number)
        {
            return (currToken.tokenType == TokenType.Variable) || (currToken.token == "(");
        }
        else if (lastToken.tokenType == TokenType.Variable || lastToken.token == ")")
        {
            return (currToken.tokenType == TokenType.Variable || currToken.tokenType == TokenType.Number || currToken.token == "(");
        }

        return false;
    }
}
