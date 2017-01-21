using System.Collections;
using System.Collections.Generic;

public class Expression {

    public List<Token> tokens;
	
    public Expression(List<Token> tokens)
    {
        this.tokens = tokens;
    }

    /// <summary>
    /// Returns the result of the expression for the passed value for x.
    /// </summary>
    /// <param name="value">Value of x</param>
    /// <returns></returns>
    public float solveForX(float value)
    {
        float num1, num2;
        Stack<float> stack = new Stack<float>();
        foreach (var token in tokens)
        { 
            if(token.tokenType == TokenType.Number)
            {
                stack.Push(token.value);
            }
            else if(token.tokenType == TokenType.Variable)
            {
                stack.Push(value);
            }
            else
            {
                num1 = stack.Pop();
                if (stack.Count > 0)
                {
                    num2 = stack.Pop();
                    stack.Push(Solve(token.operatorType, num2, num1));
                }
                else if(token.operatorType == OperatorType.Subtract)
                {
                    stack.Push(-num1);
                }
            }
        }
        return stack.Pop();

    }

    public float Solve(OperatorType operatorType, float t1, float t2)
    {
        float result = 0;
        switch (operatorType)
        {
            case OperatorType.Add:
                result = t1 + t2;
                break;
            case OperatorType.Subtract:
                result = t1 - t2;
                break;
            case OperatorType.Multiply:
                result = t1 * t2;
                break;
            case OperatorType.Divide:
                result = t1 / t2;
                break;
            case OperatorType.Power:
                result = UnityEngine.Mathf.Pow(t1, t2);
                break;
            case OperatorType.SquareRoot:
                break;
        }
        return result;
    }

}
